using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Maestro.Models;

namespace Maestro.Services.Data
{
	public class UserSongStorage
	{
		private static readonly Logger Logger = Logger.GetLogger<UserSongStorage>();

		private readonly string _userSongsPath;

		public UserSongStorage(DirectoriesManager directories)
		{
			string moduleDir = directories.GetFullDirectoryPath("common");
			_userSongsPath = Path.Combine(moduleDir, "UserSongs");
			EnsureDirectoryExists();
		}

		private void EnsureDirectoryExists()
		{
			if (!Directory.Exists(_userSongsPath))
			{
				Directory.CreateDirectory(_userSongsPath);
				Logger.Info("Created user songs directory: " + _userSongsPath);
			}
		}

		public async Task<List<Song>> LoadUserSongsAsync()
		{
			List<Song> songs = new List<Song>();
			if (!Directory.Exists(_userSongsPath))
			{
				return songs;
			}
			string[] files = Directory.GetFiles(_userSongsPath, "*.json");
			string[] array = files;
			foreach (string file in array)
			{
				try
				{
					Song song = SongSerializer.DeserializeJsonContent(await Task.Run(() => File.ReadAllText(file)));
					song.IsUserImported = true;
					songs.Add(song);
					Logger.Debug("Loaded user song: " + song.DisplayName);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load user song from " + file);
				}
			}
			Logger.Info($"Loaded {songs.Count} user songs");
			return songs;
		}

		public async Task SaveSongAsync(Song song)
		{
			EnsureDirectoryExists();
			string fileName = GetSafeFileName(song);
			string filePath = Path.Combine(_userSongsPath, fileName);
			try
			{
				string json = SongSerializer.SerializeToJson(song);
				await Task.Run(delegate
				{
					File.WriteAllText(filePath, json);
				});
				Logger.Info("Saved user song: " + song.DisplayName + " to " + filePath);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save user song: " + song.DisplayName);
				throw;
			}
		}

		public void DeleteSong(Song song)
		{
			string fileName = GetSafeFileName(song);
			string filePath = Path.Combine(_userSongsPath, fileName);
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
					Logger.Info("Deleted user song: " + song.DisplayName);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to delete user song: " + song.DisplayName);
				throw;
			}
		}

		private string GetSafeFileName(Song song)
		{
			string source = song.Name + " - " + song.Artist + ".json";
			char[] invalidChars = Path.GetInvalidFileNameChars();
			return new string(source.Select((char c) => (!invalidChars.Contains(c)) ? c : '_').ToArray());
		}
	}
}
