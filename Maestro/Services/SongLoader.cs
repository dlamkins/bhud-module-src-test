using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;

namespace Maestro.Services
{
	public static class SongLoader
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(SongLoader));

		private const string DebugSongsPath = "C:\\git\\Maestro\\Songs";

		private const string EmbeddedResourceName = "Maestro.Data.songs.bin";

		public static async Task<List<Song>> LoadAllAsync()
		{
			return await Task.Run(delegate
			{
				List<Song> list = new List<Song>();
				try
				{
					if (Directory.Exists("C:\\git\\Maestro\\Songs"))
					{
						Logger.Info("Debug mode: Loading songs from source directory: C:\\git\\Maestro\\Songs");
						LoadFromDirectory(list, "C:\\git\\Maestro\\Songs");
					}
					if (list.Count == 0)
					{
						LoadFromBundle(list);
					}
					Logger.Info($"Total songs loaded: {list.Count}");
					return list;
				}
				catch (Exception exception)
				{
					Logger.Warn(exception, "Failed to load songs - module will continue with empty song list");
					return list;
				}
			});
		}

		private static void LoadFromBundle(List<Song> songs)
		{
			try
			{
				using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Maestro.Data.songs.bin");
				if (stream == null)
				{
					Logger.Warn("Could not find embedded resource: Maestro.Data.songs.bin");
					return;
				}
				List<Song> bundleSongs = SongSerializer.DeserializeBundle(stream);
				songs.AddRange(bundleSongs);
				Logger.Info($"Loaded {bundleSongs.Count} songs from embedded bundle");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load songs from embedded bundle");
			}
		}

		private static void LoadFromDirectory(List<Song> songs, string path)
		{
			string[] jsonFiles = Directory.GetFiles(path, "*.json");
			Logger.Info($"Found {jsonFiles.Length} .json files in {path}");
			string[] array = jsonFiles;
			foreach (string file in array)
			{
				try
				{
					Song song = SongSerializer.DeserializeJson(file);
					songs.Add(song);
					Logger.Debug("Loaded song: " + song.DisplayName);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load song: " + file);
				}
			}
		}
	}
}
