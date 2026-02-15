using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Flurl.Http;
using Microsoft.Xna.Framework.Graphics;

namespace ModuleManagerPlus
{
	public class TextureLoader
	{
		private Dictionary<int, AsyncTexture2D> _textureCache = new Dictionary<int, AsyncTexture2D>();

		private readonly ContentsManager _contents;

		public TextureLoader(ContentsManager contents)
		{
			_contents = contents;
		}

		public Texture2D LoadTextureFromRef(string path)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			int hash = path.GetHashCode();
			if (!_textureCache.ContainsKey(hash))
			{
				_textureCache[hash] = new AsyncTexture2D(_contents.GetTexture(path));
			}
			return AsyncTexture2D.op_Implicit(_textureCache[hash]);
		}

		public AsyncTexture2D LoadTextureFromWeb(string url)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			int hash = url.GetHashCode();
			if (_textureCache.ContainsKey(hash))
			{
				return _textureCache[hash];
			}
			AsyncTexture2D newTexture = new AsyncTexture2D((Texture2D)null);
			PopulateTexture(newTexture, url);
			_textureCache[hash] = newTexture;
			return newTexture;
		}

		private void PopulateTexture(AsyncTexture2D target, string url)
		{
			url.GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0).ContinueWith(delegate(Task<Stream> t)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				if (t.IsFaulted)
				{
					target.SwapTexture(Textures.get_Error());
				}
				else
				{
					GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
					try
					{
						Texture2D val2 = TextureUtil.FromStreamPremultiplied(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), t.Result);
						target.SwapTexture(val2);
					}
					catch
					{
						target.SwapTexture(Textures.get_Error());
					}
					finally
					{
						((GraphicsDeviceContext)(ref val)).Dispose();
					}
				}
			});
		}

		public void Unload()
		{
			foreach (AsyncTexture2D value in _textureCache.Values)
			{
				value.Dispose();
			}
			_textureCache.Clear();
		}
	}
}
