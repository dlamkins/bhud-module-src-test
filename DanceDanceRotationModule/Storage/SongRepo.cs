using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace DanceDanceRotationModule.Storage
{
	public class SongRepo
	{
		private static readonly Logger Logger = Logger.GetLogger<SongRepo>();

		private const string DdrDirectoryName = "danceDanceRotation";

		private const string CustomSongsFolderName = "customSongs";

		private const string DefaultSongsFolderName = "defaultSongs";

		private Song.ID _selectedSongId;

		private Dictionary<Song.ID, Song> _songs;

		private Dictionary<Song.ID, SongData> _songDatas;

		private FileSystemWatcher _fileSystemWatcher;

		private EventHandler<SelectedSongInfo> _selectedSongChangedHandler;

		private static string DdrDir => DanceDanceRotationModule.Instance.DirectoriesManager.GetFullDirectoryPath("danceDanceRotation");

		private static string DefaultSongsDir => Path.Combine(DdrDir, "defaultSongs");

		private static string CustomSongsDir => Path.Combine(DdrDir, "customSongs");

		public event EventHandler OnSongsChanged;

		public event EventHandler<SelectedSongInfo> OnSelectedSongChanged
		{
			add
			{
				_selectedSongChangedHandler = (EventHandler<SelectedSongInfo>)Delegate.Combine(_selectedSongChangedHandler, value);
				InvokeSelectedSongInfo();
			}
			remove
			{
				_selectedSongChangedHandler = (EventHandler<SelectedSongInfo>)Delegate.Remove(_selectedSongChangedHandler, value);
			}
		}

		public SongRepo()
		{
			if (!Directory.Exists(CustomSongsDir))
			{
				Logger.Info("Creating " + CustomSongsDir);
				Directory.CreateDirectory(CustomSongsDir);
			}
			if (!Directory.Exists(DefaultSongsDir))
			{
				Logger.Info("Creating " + DefaultSongsDir);
				Directory.CreateDirectory(DefaultSongsDir);
			}
			_songs = new Dictionary<Song.ID, Song>();
			_songDatas = new Dictionary<Song.ID, SongData>();
		}

		public void SetSelectedSong(Song.ID songId)
		{
			if (!_selectedSongId.Equals(songId))
			{
				Logger.Info("Setting selected song to: " + songId.Name);
				_selectedSongId = songId;
				DanceDanceRotationModule.Settings.SelectedSong.set_Value(songId);
				InvokeSelectedSongInfo();
			}
		}

		public Song AddSong(string json, bool showNotification, bool isDefaultSong = false)
		{
			Song song = null;
			Logger.Info("Attempting to decode into a song:\n" + json);
			try
			{
				song = SongTranslator.FromJson(json);
				Logger.Info(_songs.ContainsKey(song.Id) ? ("Successfully replaced song: " + song.Name) : ("Successfully added song: " + song.Name));
				_songs[song.Id] = song;
				if (!isDefaultSong)
				{
					this.OnSongsChanged?.Invoke(this, null);
				}
				if (_selectedSongId.Equals(song.Id))
				{
					InvokeSelectedSongInfo();
				}
				string fullFilePath = (isDefaultSong ? GetSongPath(DefaultSongsDir, song) : GetSongPath(CustomSongsDir, song));
				Logger.Info("Attempting to save song file " + fullFilePath);
				_fileSystemWatcher.EnableRaisingEvents = false;
				string prettyJson = JsonHelper.FormatJson(json);
				File.WriteAllText(fullFilePath, prettyJson);
				Logger.Info("Successfully saved pretty song file " + fullFilePath);
				if (showNotification)
				{
					ScreenNotification.ShowNotification("Added Song Successfully", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			catch (Exception exception)
			{
				Logger.Warn($"Failed to decode clipboard contents into a song:\n{exception.Message}\n{exception}");
				if (showNotification)
				{
					ScreenNotification.ShowNotification("Failed to decode song.", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			_fileSystemWatcher.EnableRaisingEvents = true;
			return song;
		}

		public SongData GetSongData(Song.ID songId)
		{
			if (_songDatas.ContainsKey(songId))
			{
				return _songDatas[songId];
			}
			return SongData.DefaultSettings(songId);
		}

		public void UpdateData(Song.ID songId, Func<SongData, SongData> work)
		{
			SongData originalData = GetSongData(songId);
			SongData updatedSongData = work(originalData);
			if (!updatedSongData.Equals(originalData))
			{
				Logger.Info($"SongData Updated: {songId.Name}\n{updatedSongData}");
				updatedSongData.Id = songId;
				_songDatas[songId] = updatedSongData;
				Save();
				if (songId.Equals(_selectedSongId) && _songs.ContainsKey(songId))
				{
					InvokeSelectedSongInfo();
				}
			}
		}

		public void DeleteSong(Song.ID songId)
		{
			if (_songs.ContainsKey(songId))
			{
				Logger.Info("Removing song: " + songId.Name);
				Song song = _songs[songId];
				_songs.Remove(songId);
				_songDatas.Remove(songId);
				string songFilePath = GetSongPath(CustomSongsDir, song);
				if (!File.Exists(songFilePath))
				{
					songFilePath = GetSongPath(DefaultSongsDir, song);
				}
				Logger.Info("Attempting to delete song file " + songFilePath);
				_fileSystemWatcher.EnableRaisingEvents = false;
				File.Delete(songFilePath);
				_fileSystemWatcher.EnableRaisingEvents = true;
				this.OnSongsChanged?.Invoke(this, null);
				if (_selectedSongId.Equals(songId))
				{
					InvokeSelectedSongInfo();
				}
			}
		}

		public Task Load()
		{
			LoadAllSongFiles();
			MaybeLoadDefaultSongFiles();
			foreach (SongData songData in DanceDanceRotationModule.Settings.SongDatas.get_Value())
			{
				_songDatas[songData.Id] = songData;
			}
			Logger.Info("Setting initial default song " + DanceDanceRotationModule.Settings.SelectedSong.get_Value().Name);
			SetSelectedSong(DanceDanceRotationModule.Settings.SelectedSong.get_Value());
			return Task.CompletedTask;
		}

		private void LoadAllSongFiles()
		{
			_songs.Clear();
			LoadSongFiles(DefaultSongsDir);
			LoadSongFiles(CustomSongsDir);
			this.OnSongsChanged?.Invoke(this, null);
		}

		private void LoadSongFiles(string directory)
		{
			List<Song> loadedSongs = new List<Song>();
			Logger.Info("Loading song .json files in " + directory);
			string[] files = Directory.GetFiles(directory, "*.json");
			foreach (string fileName in files)
			{
				try
				{
					using StreamReader r = new StreamReader(fileName);
					Song song = SongTranslator.FromJson(r.ReadToEnd());
					loadedSongs.Add(song);
					Logger.Trace("Successfully loaded song file: " + fileName);
				}
				catch (Exception exception)
				{
					Logger.Warn(exception, "Failed to load song file: " + fileName);
				}
			}
			Logger.Info($"Successfully loaded {loadedSongs.Count} songs.");
			foreach (Song song2 in loadedSongs)
			{
				_songs[song2.Id] = song2;
			}
		}

		private void MaybeLoadDefaultSongFiles()
		{
			string lastDefaultSongsLoadedVersion = DanceDanceRotationModule.Settings.LastDefaultSongsLoadedVersion.get_Value();
			string version = ((Module)DanceDanceRotationModule.Instance).get_Version().ToString();
			bool shouldLoadDefaultSongs = false;
			if (!lastDefaultSongsLoadedVersion.Equals(version))
			{
				Logger.Info("Module version has changed (" + lastDefaultSongsLoadedVersion + " -> " + version + ")! Loading default songs");
				shouldLoadDefaultSongs = true;
			}
			else if (_songs.Count == 0)
			{
				Logger.Info("No songs were found in " + DefaultSongsDir + "! Loading default songs");
				shouldLoadDefaultSongs = true;
			}
			if (!shouldLoadDefaultSongs)
			{
				return;
			}
			string[] files = Directory.GetFiles(DefaultSongsDir);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			try
			{
				using StreamReader r = new StreamReader(DanceDanceRotationModule.Instance.ContentsManager.GetFileStream("defaultSongs.json"));
				DanceDanceRotationModule.Settings.LastDefaultSongsLoadedVersion.set_Value(version);
				foreach (object songJson in JsonConvert.DeserializeObject<List<object>>(r.ReadToEnd()))
				{
					AddSong(songJson.ToString(), showNotification: false, isDefaultSong: true);
				}
				Logger.Info($"Successfully loaded {_songs.Count} songs.");
				this.OnSongsChanged?.Invoke(this, null);
			}
			catch (Exception exception)
			{
				Logger.Warn(exception, "Failed to load song file: defaultSongs.json");
			}
		}

		private void Save()
		{
			DanceDanceRotationModule.Settings.SelectedSong.set_Value(_selectedSongId);
			DanceDanceRotationModule.Settings.SongDatas.set_Value(_songDatas.Values.ToList());
			Logger.Info("Saved SongRepo");
		}

		public void StartDirectoryWatcher()
		{
			_fileSystemWatcher = new FileSystemWatcher();
			_fileSystemWatcher.Path = CustomSongsDir;
			_fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
			_fileSystemWatcher.Filter = "*.json";
			_fileSystemWatcher.Changed += OnSongChanged;
			_fileSystemWatcher.Created += OnSongChanged;
			_fileSystemWatcher.Deleted += OnSongChanged;
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		private static void OnSongChanged(object source, FileSystemEventArgs @event)
		{
			Logger.Info("Song File Event: " + @event.FullPath + " " + @event.ChangeType);
			switch (@event.ChangeType)
			{
			case WatcherChangeTypes.Created:
			case WatcherChangeTypes.Changed:
			{
				string json = "";
				try
				{
					using StreamReader r = new StreamReader(@event.FullPath);
					json = r.ReadToEnd();
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to load newly created or changed file!");
				}
				if (json.Length > 0)
				{
					DanceDanceRotationModule.SongRepo.AddSong(json, showNotification: true);
				}
				break;
			}
			default:
				DanceDanceRotationModule.SongRepo.LoadAllSongFiles();
				break;
			case WatcherChangeTypes.Renamed:
				break;
			}
		}

		public List<Song> GetAllSongs()
		{
			return _songs.Values.ToList();
		}

		public Song GetSong(Song.ID songId)
		{
			return _songs[songId];
		}

		public Song.ID GetSelectedSongId()
		{
			return _selectedSongId;
		}

		public void Dispose()
		{
			_fileSystemWatcher.EnableRaisingEvents = false;
			_fileSystemWatcher.Dispose();
			_songs.Clear();
			_songDatas.Clear();
			Logger.Info("Disposed SongRepo");
		}

		private void InvokeSelectedSongInfo()
		{
			Song song = (_songs.ContainsKey(_selectedSongId) ? _songs[_selectedSongId] : null);
			SongData songData = (_songDatas.ContainsKey(_selectedSongId) ? _songDatas[_selectedSongId] : SongData.DefaultSettings(_selectedSongId));
			_selectedSongChangedHandler?.Invoke(this, new SelectedSongInfo
			{
				Song = song,
				Data = songData
			});
		}

		private string GetSongPath(string dir, Song song)
		{
			return Path.Combine(DdrDir, dir, song.Name + ".json");
		}
	}
}
