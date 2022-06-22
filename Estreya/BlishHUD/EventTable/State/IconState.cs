using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.State
{
	public class IconState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<IconState>();

		private const string FOLDER_NAME = "images";

		private static TimeSpan _saveInterval = TimeSpan.FromMinutes(2.0);

		private readonly ContentsManager _contentsManager;

		private readonly AsyncLock _textureLock = new AsyncLock();

		private readonly Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();

		private string _basePath;

		private string _path;

		private string Path
		{
			get
			{
				if (_path == null)
				{
					_path = System.IO.Path.Combine(_basePath, "images");
				}
				return _path;
			}
		}

		public IconState(ContentsManager contentsManager, string basePath)
			: base(awaitLoad: true, (int)_saveInterval.TotalMilliseconds)
		{
			_contentsManager = contentsManager;
			_basePath = basePath;
		}

		protected override async Task InternalReload()
		{
			await LoadImages();
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			AsyncHelper.RunSync(Save);
			using (_textureLock.Lock())
			{
				foreach (KeyValuePair<string, Texture2D> loadedTexture in _loadedTextures)
				{
					((GraphicsResource)loadedTexture.Value).Dispose();
				}
				_loadedTextures.Clear();
			}
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			await LoadImages();
		}

		protected override async Task Save()
		{
			Logger.Debug("Save loaded textures to filesystem.");
			if (!Directory.Exists(Path))
			{
				Directory.CreateDirectory(Path);
			}
			using (await _textureLock.LockAsync())
			{
				string[] currentLoadedTexturesArr = new string[_loadedTextures.Keys.Count];
				_loadedTextures.Keys.CopyTo(currentLoadedTexturesArr, 0);
				List<string> currentLoadedTextures = new List<string>(currentLoadedTexturesArr);
				string[] files = GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					string sanitizedFileName = FileUtil.SanitizeFileName(System.IO.Path.GetFileNameWithoutExtension(files[i]));
					if (currentLoadedTextures.Contains(sanitizedFileName))
					{
						currentLoadedTextures.Remove(sanitizedFileName);
					}
				}
				foreach (string newTextureIdentifier in currentLoadedTextures)
				{
					try
					{
						using FileStream fileStream = new FileStream(System.IO.Path.ChangeExtension(System.IO.Path.Combine(Path, newTextureIdentifier), "png"), FileMode.Create, FileAccess.Write);
						Texture2D newTexture = _loadedTextures[newTextureIdentifier];
						if (newTexture == Textures.get_Error())
						{
							Logger.Warn("Texture \"{0}\" is errorneous. Skipping saving.", new object[1] { newTextureIdentifier });
						}
						else
						{
							newTexture.SaveAsPng((Stream)fileStream, newTexture.get_Width(), newTexture.get_Height());
						}
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Failed saving texture \"{1}\": {0}", new object[1] { newTextureIdentifier });
					}
				}
			}
		}

		private Task LoadImages()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			Logger.Info("Load cached images from filesystem.");
			using (_textureLock.Lock())
			{
				_loadedTextures.Clear();
				if (!Directory.Exists(Path))
				{
					return Task.CompletedTask;
				}
				string[] files = GetFiles();
				FileStream fileStream;
				AsyncTexture2D asyncTexture;
				foreach (string filePath in files)
				{
					try
					{
						fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
						if (fileStream.Length == 0L)
						{
							Logger.Warn("Image is empty: {0}", new object[1] { filePath });
							continue;
						}
						asyncTexture = new AsyncTexture2D(Textures.get_Pixel());
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
						{
							Texture2D val = TextureUtil.FromStreamPremultiplied(device, (Stream)fileStream);
							fileStream.Dispose();
							asyncTexture.SwapTexture(val);
						});
						string fileName = FileUtil.SanitizeFileName(System.IO.Path.GetFileNameWithoutExtension(filePath));
						HandleAsyncTextureSwap(asyncTexture, fileName);
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Failed preloading texture \"{1}\": {0}", new object[1] { filePath });
					}
				}
			}
			return Task.CompletedTask;
		}

		private void HandleAsyncTextureSwap(AsyncTexture2D asyncTexture2D, string identifier)
		{
			asyncTexture2D.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object s, ValueChangedEventArgs<Texture2D> e)
			{
				using (_textureLock.Lock())
				{
					_loadedTextures[identifier] = e.get_NewValue();
				}
				Logger.Debug("Async texture \"{0}\" was swapped in cache.", new object[1] { identifier });
			});
		}

		private string[] GetFiles()
		{
			return Directory.GetFiles(Path, "*.png");
		}

		public bool HasIcon(string identifier)
		{
			string sanitizedIdentifier = FileUtil.SanitizeFileName(identifier);
			return _loadedTextures.ContainsKey(sanitizedIdentifier);
		}

		public Texture2D GetIcon(string identifier, bool checkRenderAPI = true)
		{
			if (string.IsNullOrWhiteSpace(identifier))
			{
				return null;
			}
			string sanitizedIdentifier = FileUtil.SanitizeFileName(System.IO.Path.ChangeExtension(identifier, null));
			using (_textureLock.Lock())
			{
				if (_loadedTextures.ContainsKey(sanitizedIdentifier))
				{
					return _loadedTextures[sanitizedIdentifier];
				}
				Texture2D icon = Textures.get_Error();
				if (!string.IsNullOrWhiteSpace(identifier))
				{
					if (checkRenderAPI && identifier.Contains("/"))
					{
						try
						{
							AsyncTexture2D asyncTexture = GameService.Content.GetRenderServiceTexture(identifier);
							HandleAsyncTextureSwap(asyncTexture, sanitizedIdentifier);
							icon = AsyncTexture2D.op_Implicit(asyncTexture);
						}
						catch (Exception ex2)
						{
							Logger.Warn(ex2, "Could not load icon from render api:");
						}
					}
					else
					{
						try
						{
							Texture2D texture = _contentsManager.GetTexture(identifier);
							if (texture == Textures.get_Error())
							{
								texture = GameService.Content.GetTexture(identifier);
							}
							icon = texture;
						}
						catch (Exception ex)
						{
							Logger.Warn(ex, "Could not load icon from ref folders:");
						}
					}
				}
				_loadedTextures.Add(sanitizedIdentifier, icon);
				return icon;
			}
		}

		public Task<Texture2D> GetIconAsync(string identifier, bool checkRenderAPI = true)
		{
			return Task.Run(() => GetIcon(identifier, checkRenderAPI));
		}

		public override Task Clear()
		{
			return Task.CompletedTask;
		}
	}
}
