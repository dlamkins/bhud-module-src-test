using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility.ColorThief;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TmfLib;

namespace BhModule.Community.Pathing.Content
{
	public class TextureResourceManager : IPackResourceManager
	{
		private static readonly Texture2D _textureFailedToLoad = PathingModule.Instance.ContentsManager.GetTexture("png\\missing-texture.png");

		private static readonly Color _defaultSampleColor = Color.get_DarkGray();

		private static readonly ConcurrentDictionary<IPackResourceManager, TextureResourceManager> _textureResourceManagerLookup = new ConcurrentDictionary<IPackResourceManager, TextureResourceManager>();

		private readonly ConcurrentDictionary<string, TaskCompletionSource<(Texture2D Texture, Color Sample)>> _textureCache = new ConcurrentDictionary<string, TaskCompletionSource<(Texture2D, Color)>>(StringComparer.OrdinalIgnoreCase);

		private readonly IPackResourceManager _packResourceManager;

		public static TextureResourceManager GetTextureResourceManager(IPackResourceManager referencePackResourceManager)
		{
			return _textureResourceManagerLookup.GetOrAdd(referencePackResourceManager, (IPackResourceManager packResourceManager) => new TextureResourceManager(packResourceManager));
		}

		public static async Task UnloadAsync()
		{
			ICollection<TextureResourceManager> managers = _textureResourceManagerLookup.Values;
			foreach (TextureResourceManager item in managers)
			{
				await item.ClearCache();
			}
		}

		private TextureResourceManager(IPackResourceManager packResourceManager)
		{
			_packResourceManager = packResourceManager;
		}

		public bool ResourceExists(string resourcePath)
		{
			return _packResourceManager.ResourceExists(resourcePath);
		}

		public async Task<byte[]> LoadResourceAsync(string resourcePath)
		{
			return await _packResourceManager.LoadResourceAsync(resourcePath);
		}

		public async Task<Stream> LoadResourceStreamAsync(string resourcePath)
		{
			return await _packResourceManager.LoadResourceStreamAsync(resourcePath);
		}

		private static void LoadTexture(TaskCompletionSource<(Texture2D Texture, Color Sample)> textureTcs, Stream textureStream, bool shouldSample)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				if (textureStream == null)
				{
					textureTcs.SetResult((_textureFailedToLoad, Color.get_DarkGray()));
				}
				else
				{
					try
					{
						Texture2D val = TextureUtil.FromStreamPremultiplied(graphicsDevice, textureStream);
						Color item = (shouldSample ? SampleColor(val) : _defaultSampleColor);
						textureTcs.SetResult((val, item));
					}
					catch (Exception)
					{
						textureTcs.SetResult((_textureFailedToLoad, _defaultSampleColor));
					}
				}
			});
		}

		private static Color SampleColor(Texture2D texture)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			List<QuantizedColor> palette = ColorThief.GetPalette(texture);
			palette.Sort((QuantizedColor color, QuantizedColor color2) => color2.Population.CompareTo(color.Population));
			return (Color)(((_003F?)palette.FirstOrDefault()?.Color) ?? _defaultSampleColor);
		}

		public async Task PreloadTexture(string texturePath, bool shouldSample)
		{
			if (!_textureCache.ContainsKey(texturePath))
			{
				TaskCompletionSource<(Texture2D, Color)> textureTcs = new TaskCompletionSource<(Texture2D, Color)>();
				_textureCache[texturePath] = textureTcs;
				TaskCompletionSource<(Texture2D Texture, Color Sample)> textureTcs2 = textureTcs;
				LoadTexture(textureTcs2, await LoadResourceStreamAsync(texturePath), shouldSample);
			}
		}

		public async Task<(Texture2D Texture, Color Sample)> LoadTextureAsync(string texturePath)
		{
			if (_textureCache.TryGetValue(texturePath, out var texture))
			{
				return await texture.Task;
			}
			return (_textureFailedToLoad, _defaultSampleColor);
		}

		public async Task ClearCache()
		{
			ICollection<TaskCompletionSource<(Texture2D, Color)>> textureCollection = _textureCache.Values;
			_textureCache.Clear();
			List<(Texture2D Texture, Color Sample)> texturePairs = new List<(Texture2D, Color)>(textureCollection.Count);
			foreach (TaskCompletionSource<(Texture2D, Color)> item in textureCollection)
			{
				List<(Texture2D Texture, Color Sample)> list = texturePairs;
				list.Add(await item.Task);
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				foreach (var item2 in texturePairs)
				{
					((GraphicsResource)item2.Texture).Dispose();
				}
			});
		}
	}
}
