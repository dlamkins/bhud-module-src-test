using System;
using System.IO;
using System.Net;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics;
using Gw2Sharp.Models;
using LiteDB;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Music_Mixer.Core.Services.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Nekres.Music_Mixer.Core.Services
{
	internal class DataService : IDisposable
	{
		private ConnectionString _connectionString;

		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		private ManualResetEvent _lockReleased = new ManualResetEvent(initialState: false);

		private bool _lockAcquired;

		private const string TBL_PLAYLISTS = "playlists";

		public const string TBL_AUDIO_SOURCES = "audio_sources";

		private const string TBL_THUMBNAILS = "thumbnails";

		private const string TBL_THUMBNAIL_CHUNKS = "thumbnail_chunks";

		private const string LITEDB_FILENAME = "music.db";

		public DataService()
		{
			_connectionString = new ConnectionString
			{
				Filename = Path.Combine(MusicMixer.Instance.ModuleDirectory, "music.db"),
				Connection = ConnectionType.Shared
			};
		}

		public AsyncTexture2D GetThumbnail(AudioSource source)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			LiteFileStream<string> stream = null;
			AsyncTexture2D texture = new AsyncTexture2D();
			if (string.IsNullOrWhiteSpace(source.ExternalId))
			{
				return texture;
			}
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				ILiteStorage<string> thumbnails = db.GetStorage<string>("thumbnails", "thumbnail_chunks");
				if (thumbnails.Exists(source.ExternalId))
				{
					stream = thumbnails.OpenRead(source.ExternalId);
				}
			}
			catch (Exception e2)
			{
				MusicMixer.Logger.Warn(e2, e2.Message);
			}
			finally
			{
				ReleaseWriteLock();
			}
			if (stream == null)
			{
				if (string.IsNullOrWhiteSpace(source.PageUrl))
				{
					return texture;
				}
				MusicMixer.Instance.YtDlp.GetThumbnail(source.PageUrl, delegate(string thumbnailUri)
				{
					ThumbnailUrlReceived(source.ExternalId, thumbnailUri, texture);
				});
				return texture;
			}
			try
			{
				GraphicsDeviceContext gdx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					texture.SwapTexture(Texture2D.FromStream(((GraphicsDeviceContext)(ref gdx)).get_GraphicsDevice(), (Stream)stream));
					return texture;
				}
				finally
				{
					((GraphicsDeviceContext)(ref gdx)).Dispose();
				}
			}
			catch (Exception e)
			{
				MusicMixer.Logger.Info(e, e.Message);
			}
			return texture;
		}

		public bool GetTrackByMediaId(string mediaId, out AudioSource source)
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				ILiteCollection<AudioSource> collection = db.GetCollection<AudioSource>("audio_sources");
				source = collection.FindOne((AudioSource x) => x.ExternalId.Equals(mediaId));
				return source != null;
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		private bool GetPlaylist(string externalId, out Playlist playlist)
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				ILiteCollection<Playlist> collection = db.GetCollection<Playlist>("playlists");
				playlist = collection.Include((Playlist x) => x.Tracks).FindOne((Playlist x) => x.ExternalId == externalId);
				return playlist != null;
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		public bool GetMountPlaylist(MountType mountType, out Playlist playlist)
		{
			return GetPlaylist(((object)(MountType)(ref mountType)).ToString(), out playlist);
		}

		public bool GetDefeatedPlaylist(out Playlist context)
		{
			return GetPlaylist("Defeated", out context);
		}

		public bool GetMapPlaylist(int mapId, out Playlist playlist)
		{
			return GetPlaylist($"map_{mapId}", out playlist);
		}

		public bool Remove(AudioSource model)
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				return db.GetCollection<AudioSource>("audio_sources").Delete(model.Id);
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		public bool RemoveAudioUrls()
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				ILiteCollection<AudioSource> collection = db.GetCollection<AudioSource>("audio_sources");
				return collection.Count() == collection.UpdateMany((AudioSource src) => new AudioSource
				{
					Id = src.Id,
					ExternalId = src.ExternalId,
					PageUrl = src.PageUrl,
					Title = src.Title,
					Uploader = src.Uploader,
					Duration = src.Duration,
					Volume = src.Volume,
					DayCycles = src.DayCycles,
					AudioUrl = string.Empty
				}, (AudioSource src) => true);
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		public bool Upsert(AudioSource model)
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				db.GetCollection<AudioSource>("audio_sources").Upsert(model);
				return true;
			}
			catch (Exception e)
			{
				MusicMixer.Logger.Warn(e, e.Message);
				return false;
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		public bool Upsert(Playlist model)
		{
			AcquireWriteLock();
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				db.GetCollection<Playlist>("playlists").Upsert(model);
				return true;
			}
			catch (Exception e)
			{
				MusicMixer.Logger.Warn(e, e.Message);
				return false;
			}
			finally
			{
				ReleaseWriteLock();
			}
		}

		public void Dispose()
		{
			if (_lockAcquired)
			{
				_lockReleased.WaitOne(500);
			}
			_lockReleased.Dispose();
			try
			{
				_rwLock.Dispose();
			}
			catch (Exception ex)
			{
				MusicMixer.Logger.Debug(ex, ex.Message);
			}
		}

		private void ThumbnailUrlReceived(string id, string url, AsyncTexture2D texture)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			WebClient client = new WebClient();
			client.OpenReadAsync(new Uri(url));
			client.OpenReadCompleted += delegate(object _, OpenReadCompletedEventArgs e)
			{
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				using Stream stream = e.Result;
				try
				{
					if (!e.Cancelled)
					{
						if (e.Error != null)
						{
							throw e.Error;
						}
						if (stream != null)
						{
							using Image source = Image.Load(stream);
							using MemoryStream memoryStream = new MemoryStream();
							source.Save(memoryStream, JpegFormat.Instance);
							memoryStream.Position = 0L;
							AcquireWriteLock();
							try
							{
								using LiteDatabase liteDatabase = new LiteDatabase(_connectionString);
								liteDatabase.GetStorage<string>("thumbnails", "thumbnail_chunks").Upload(id, url, memoryStream);
							}
							finally
							{
								ReleaseWriteLock();
							}
							GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
							try
							{
								texture.SwapTexture(Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream));
							}
							finally
							{
								((GraphicsDeviceContext)(ref val)).Dispose();
							}
						}
					}
				}
				catch (Exception ex)
				{
					MusicMixer.Logger.Info(ex, ex.Message);
				}
				finally
				{
					client.Dispose();
				}
			};
		}

		private void AcquireWriteLock()
		{
			try
			{
				_rwLock.EnterWriteLock();
				_lockAcquired = true;
			}
			catch (Exception ex)
			{
				MusicMixer.Logger.Debug(ex, ex.Message);
			}
		}

		private void ReleaseWriteLock()
		{
			try
			{
				if (_lockAcquired)
				{
					_rwLock.ExitWriteLock();
					_lockAcquired = false;
				}
			}
			catch (Exception ex)
			{
				MusicMixer.Logger.Debug(ex, ex.Message);
			}
			finally
			{
				_lockReleased.Set();
			}
		}
	}
}
