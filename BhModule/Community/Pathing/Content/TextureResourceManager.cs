using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TmfLib;

namespace BhModule.Community.Pathing.Content
{
	public class TextureResourceManager : IPackResourceManager
	{
		private static readonly Texture2D _textureFailedToLoad = PathingModule.Instance.ContentsManager.GetTexture("png\\missing-texture.png");

		private static readonly ConcurrentDictionary<IPackResourceManager, TextureResourceManager> _textureResourceManagerLookup = new ConcurrentDictionary<IPackResourceManager, TextureResourceManager>();

		private readonly ConcurrentDictionary<string, TaskCompletionSource<Texture2D>> _textureCache = new ConcurrentDictionary<string, TaskCompletionSource<Texture2D>>(StringComparer.OrdinalIgnoreCase);

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

		private static void LoadTexture(TaskCompletionSource<Texture2D> textureTcs, Stream textureStream)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				if (textureStream == null)
				{
					textureTcs.SetResult(_textureFailedToLoad);
				}
				else
				{
					try
					{
						textureTcs.SetResult(TextureUtil.FromStreamPremultiplied(graphicsDevice, textureStream));
					}
					catch (Exception)
					{
						textureTcs.SetResult(_textureFailedToLoad);
					}
				}
			});
		}

		public async Task PreloadTexture(string texturePath)
		{
			if (!_textureCache.ContainsKey(texturePath))
			{
				TaskCompletionSource<Texture2D> textureTcs = new TaskCompletionSource<Texture2D>();
				_textureCache[texturePath] = textureTcs;
				await Task.Yield();
				TaskCompletionSource<Texture2D> textureTcs2 = textureTcs;
				LoadTexture(textureTcs2, await LoadResourceStreamAsync(texturePath));
			}
		}

		public async Task<Texture2D> LoadTextureAsync(string texturePath)
		{
			return await _textureCache[texturePath].Task;
		}

		public async Task ClearCache()
		{
			ICollection<TaskCompletionSource<Texture2D>> textureCollection = _textureCache.Values;
			_textureCache.Clear();
			List<Texture2D> textures = new List<Texture2D>(textureCollection.Count);
			foreach (TaskCompletionSource<Texture2D> item in textureCollection)
			{
				List<Texture2D> list = textures;
				list.Add(await item.Task);
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				foreach (Texture2D item2 in textures)
				{
					((GraphicsResource)item2).Dispose();
				}
			});
		}
	}
}
