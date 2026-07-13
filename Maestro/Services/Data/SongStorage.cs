using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using LiteDB;
using Maestro.Models;

namespace Maestro.Services.Data
{
	public class SongStorage : IDisposable
	{
		private class StoredSong
		{
			public ObjectId Id { get; set; }

			public string SongKey { get; set; }

			public Song Song { get; set; }

			public DateTime SavedAt { get; set; }
		}

		private class CachedManifest
		{
			public string Id { get; set; }

			public CommunityManifest Manifest { get; set; }

			public DateTime CachedAt { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<SongStorage>();

		private const string DATABASE_FILE_NAME = "maestro-songs.db";

		private const string SONGS_COLLECTION = "songs";

		private const string MANIFEST_COLLECTION = "manifest";

		private readonly LiteDatabase _database;

		private readonly ILiteCollection<StoredSong> _songsCollection;

		private readonly ILiteCollection<CachedManifest> _manifestCollection;

		public LiteDatabase Database => _database;

		public SongStorage(DirectoriesManager directoriesManager)
		{
			string fullDirectoryPath = directoriesManager.GetFullDirectoryPath("maestro");
			Directory.CreateDirectory(fullDirectoryPath);
			string dbPath = Path.Combine(fullDirectoryPath, "maestro-songs.db");
			Logger.Info("Opening song storage at " + dbPath);
			_database = new LiteDatabase("Filename=" + dbPath + ";Connection=shared");
			_songsCollection = _database.GetCollection<StoredSong>("songs");
			_manifestCollection = _database.GetCollection<CachedManifest>("manifest");
			_songsCollection.EnsureIndex((StoredSong x) => x.SongKey, unique: true);
		}

		public List<Song> GetAllSongs()
		{
			List<Song> songs = (from x in _songsCollection.FindAll()
				select x.Song).ToList();
			foreach (Song song in songs)
			{
				EnsureCommandsParsed(song);
			}
			return songs;
		}

		public Song GetSong(string key)
		{
			StoredSong stored = _songsCollection.FindOne((StoredSong x) => x.SongKey == key);
			if (stored?.Song != null)
			{
				EnsureCommandsParsed(stored.Song);
			}
			return stored?.Song;
		}

		public void SaveSong(Song song)
		{
			string key = GetSongKey(song);
			StoredSong existing = _songsCollection.FindOne((StoredSong x) => x.SongKey == key);
			StoredSong stored = new StoredSong
			{
				Id = (existing?.Id ?? ObjectId.NewObjectId()),
				SongKey = key,
				Song = song,
				SavedAt = DateTime.UtcNow
			};
			_songsCollection.Upsert(stored);
			Logger.Info("Saved song: " + song.Name + " by " + song.Artist + " (key: " + key + ")");
		}

		public void DeleteSong(Song song)
		{
			string key = GetSongKey(song);
			_songsCollection.DeleteMany((StoredSong x) => x.SongKey == key);
			Logger.Info("Deleted song: " + song.Name + " by " + song.Artist);
		}

		public bool SongExists(string key)
		{
			return _songsCollection.Exists((StoredSong x) => x.SongKey == key);
		}

		public bool SongExists(Song song)
		{
			string key = GetSongKey(song);
			return SongExists(key);
		}

		public CommunityManifest GetCachedManifest(SongNamespace ns)
		{
			return _manifestCollection.FindById(ns.ToString())?.Manifest;
		}

		public void SaveManifest(SongNamespace ns, CommunityManifest manifest)
		{
			CachedManifest cached = new CachedManifest
			{
				Id = ns.ToString(),
				Manifest = manifest,
				CachedAt = DateTime.UtcNow
			};
			_manifestCollection.Upsert(cached);
			Logger.Debug($"Saved {ns} manifest to cache");
		}

		private string GetSongKey(Song song)
		{
			if (!string.IsNullOrEmpty(song.CommunityId))
			{
				return song.CommunityId;
			}
			if (!string.IsNullOrEmpty(song.BuiltInId))
			{
				return song.BuiltInId;
			}
			return $"{song.Name}|{song.Artist}|{song.Instrument}".ToLowerInvariant();
		}

		private void EnsureCommandsParsed(Song song)
		{
			if (song.Commands.Count == 0)
			{
				List<string> notes = song.Notes;
				if (notes != null && notes.Count > 0)
				{
					List<SongCommand> commands = SongCompiler.Parse(song.Notes, song.Instrument);
					song.Commands.AddRange(commands);
				}
			}
		}

		public void Dispose()
		{
			_database?.Dispose();
		}
	}
}
