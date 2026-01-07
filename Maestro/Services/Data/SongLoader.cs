using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;

namespace Maestro.Services.Data
{
	public static class SongLoader
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(SongLoader));

		public static async Task<List<Song>> LoadAllAsync(string songsDirectory)
		{
			return await Task.Run(delegate
			{
				List<Song> list = new List<Song>();
				try
				{
					if (Directory.Exists(songsDirectory))
					{
						Logger.Info("Loading songs from: " + songsDirectory);
						LoadFromDirectory(list, songsDirectory);
					}
					else
					{
						Logger.Warn("Songs directory not found: " + songsDirectory);
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
					Logger.Debug($"Loaded song: {song.DisplayName} ({song.Commands.Count} commands)");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load song: " + file);
				}
			}
		}
	}
}
