using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using LiteDB;
using Maestro.Models;
using Maestro.Services.Data;

namespace Maestro.Services.Community
{
	public class CommunitySongCache : IDisposable
	{
		private class CachedSong
		{
			public ObjectId Id { get; set; }

			public string SongKey { get; set; }

			public Song Song { get; set; }

			public DateTime SavedAt { get; set; }
		}

		private class CachedManifest
		{
			public int Id { get; set; }

			public CommunityManifest Manifest { get; set; }

			public DateTime CachedAt { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<CommunitySongCache>();

		private const string DatabaseFileName = "maestro-cache.db";

		private const string CommunitySongsCollection = "community_songs";

		private const string ImportedSongsCollection = "imported_songs";

		private const string ManifestCollection = "manifest";

		private readonly LiteDatabase _database;

		private readonly ILiteCollection<CachedSong> _communityCollection;

		private readonly ILiteCollection<CachedSong> _importedCollection;

		private readonly ILiteCollection<CachedManifest> _manifestCollection;

		public CommunitySongCache(DirectoriesManager directoriesManager)
		{
			string fullDirectoryPath = directoriesManager.GetFullDirectoryPath("maestro");
			Directory.CreateDirectory(fullDirectoryPath);
			string dbPath = Path.Combine(fullDirectoryPath, "maestro-cache.db");
			Logger.Info("Opening song cache at " + dbPath);
			_database = new LiteDatabase("Filename=" + dbPath + ";Connection=shared");
			_communityCollection = _database.GetCollection<CachedSong>("community_songs");
			_importedCollection = _database.GetCollection<CachedSong>("imported_songs");
			_manifestCollection = _database.GetCollection<CachedManifest>("manifest");
			_communityCollection.EnsureIndex((CachedSong x) => x.SongKey, unique: true);
			_importedCollection.EnsureIndex((CachedSong x) => x.SongKey, unique: true);
		}

		public CommunityManifest GetCachedManifest()
		{
			return _manifestCollection.FindById(1)?.Manifest;
		}

		public void SaveManifest(CommunityManifest manifest)
		{
			CachedManifest cached = new CachedManifest
			{
				Id = 1,
				Manifest = manifest,
				CachedAt = DateTime.UtcNow
			};
			_manifestCollection.Upsert(cached);
			Logger.Debug("Saved manifest to cache");
		}

		public Song GetCachedSong(string communityId)
		{
			CachedSong cached = _communityCollection.FindOne((CachedSong x) => x.SongKey == communityId);
			if (cached?.Song != null)
			{
				EnsureCommandsParsed(cached.Song);
			}
			return cached?.Song;
		}

		public List<Song> GetAllCachedSongs()
		{
			List<Song> songs = (from x in _communityCollection.FindAll()
				select x.Song).ToList();
			foreach (Song song in songs)
			{
				EnsureCommandsParsed(song);
			}
			return songs;
		}

		public void SaveSong(Song song)
		{
			if (string.IsNullOrEmpty(song.CommunityId))
			{
				Logger.Warn("Attempted to cache song without CommunityId");
				return;
			}
			CachedSong cached = new CachedSong
			{
				SongKey = song.CommunityId,
				Song = song,
				SavedAt = DateTime.UtcNow
			};
			_communityCollection.Upsert(cached);
			Logger.Info("Cached community song: " + song.Name + " (" + song.CommunityId + ")");
		}

		public bool IsSongCached(string communityId)
		{
			return _communityCollection.Exists((CachedSong x) => x.SongKey == communityId);
		}

		public void DeleteSong(string communityId)
		{
			_communityCollection.DeleteMany((CachedSong x) => x.SongKey == communityId);
			Logger.Info("Removed cached song: " + communityId);
		}

		public int GetCachedSongCount()
		{
			return _communityCollection.Count();
		}

		public List<Song> GetAllImportedSongs()
		{
			List<Song> songs = (from x in _importedCollection.FindAll()
				select x.Song).ToList();
			foreach (Song song in songs)
			{
				song.IsUserImported = true;
				EnsureCommandsParsed(song);
			}
			return songs;
		}

		public void SaveImportedSong(Song song)
		{
			string key = GetImportedSongKey(song);
			CachedSong cached = new CachedSong
			{
				SongKey = key,
				Song = song,
				SavedAt = DateTime.UtcNow
			};
			_importedCollection.Upsert(cached);
			Logger.Info("Saved imported song: " + song.Name + " by " + song.Artist);
		}

		public void DeleteImportedSong(Song song)
		{
			string key = GetImportedSongKey(song);
			_importedCollection.DeleteMany((CachedSong x) => x.SongKey == key);
			Logger.Info("Deleted imported song: " + song.Name + " by " + song.Artist);
		}

		private string GetImportedSongKey(Song song)
		{
			return $"{song.Name}|{song.Artist}|{song.Instrument}".ToLowerInvariant();
		}

		private void EnsureCommandsParsed(Song song)
		{
			if (song.Commands.Count == 0)
			{
				List<string> notes = song.Notes;
				if (notes != null && notes.Count > 0)
				{
					List<SongCommand> commands = NoteParser.Parse(song.Notes);
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
