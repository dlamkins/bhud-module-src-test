using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder.Services
{
	public class MapTileService
	{
		private static readonly Logger Logger = Logger.GetLogger<MapTileService>();

		private const int TileSize = 256;

		private const int MaxParallel = 6;

		private const int MaxTexturePerTick = 4;

		private static readonly HttpClient Http = CreateClient();

		private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

		private readonly ConcurrentDictionary<string, bool> _pending = new ConcurrentDictionary<string, bool>();

		private readonly ConcurrentDictionary<string, bool> _failed = new ConcurrentDictionary<string, bool>();

		private readonly ConcurrentQueue<(string Key, byte[] Data)> _downloaded = new ConcurrentQueue<(string, byte[])>();

		private int _inFlight;

		public static int MaxZoomFor(int continentId)
		{
			if (continentId != 2)
			{
				return 7;
			}
			return 6;
		}

		private static HttpClient CreateClient()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			try
			{
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
			}
			catch
			{
			}
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(15.0));
			((HttpHeaders)val.get_DefaultRequestHeaders()).Add("User-Agent", "Blish-HUD Wayfinder module");
			return val;
		}

		private static string Key(int continentId, int floor, int zoom, int x, int y)
		{
			return $"{continentId}/{floor}/{zoom}/{x}/{y}";
		}

		public Texture2D GetTile(int continentId, int floor, int zoom, int x, int y)
		{
			if (continentId < 0 || zoom < 0 || x < 0 || y < 0)
			{
				return null;
			}
			string key = Key(continentId, floor, zoom, x, y);
			if (_textures.TryGetValue(key, out var tex))
			{
				return tex;
			}
			if (_failed.ContainsKey(key) || _pending.ContainsKey(key))
			{
				return null;
			}
			if (Volatile.Read(ref _inFlight) >= 6)
			{
				return null;
			}
			_pending[key] = true;
			Interlocked.Increment(ref _inFlight);
			DownloadAsync(key);
			return null;
		}

		private async Task DownloadAsync(string key)
		{
			try
			{
				byte[] data = await Http.GetByteArrayAsync("https://tiles.guildwars2.com/" + key + ".jpg");
				_downloaded.Enqueue((key, data));
			}
			catch (Exception ex)
			{
				Logger.Debug("Tile " + key + " unavailable: " + ex.Message);
				_failed[key] = true;
				_pending.TryRemove(key, out var _);
			}
			finally
			{
				Interlocked.Decrement(ref _inFlight);
			}
		}

		public void ProcessDownloads()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < 4; i++)
			{
				if (!_downloaded.TryDequeue(out var item))
				{
					break;
				}
				try
				{
					using MemoryStream ms = new MemoryStream(item.Item2);
					GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
					try
					{
						_textures[item.Item1] = Texture2D.FromStream(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), (Stream)ms);
					}
					finally
					{
						((GraphicsDeviceContext)(ref ctx)).Dispose();
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Could not decode tile " + item.Item1 + ".");
					_failed[item.Item1] = true;
				}
				finally
				{
					_pending.TryRemove(item.Item1, out var _);
				}
			}
		}

		public static double ContinentUnitsPerTile(int continentId, int zoom)
		{
			return 256.0 * Math.Pow(2.0, MaxZoomFor(continentId) - zoom);
		}

		public void Dispose()
		{
			foreach (Texture2D value in _textures.Values)
			{
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			_textures.Clear();
		}
	}
}
