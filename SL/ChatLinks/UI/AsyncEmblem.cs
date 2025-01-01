using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace SL.ChatLinks.UI
{
	internal sealed class AsyncEmblem : IDisposable
	{
		private readonly AsyncTexture2D _emblem;

		private readonly WindowBase2 _window;

		private AsyncEmblem(WindowBase2 window, AsyncTexture2D emblem)
		{
			_window = window;
			_emblem = emblem;
		}

		public void Dispose()
		{
			_emblem.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
		}

		public static AsyncEmblem Attach(WindowBase2 window, AsyncTexture2D emblem)
		{
			AsyncEmblem asyncEmblem = new AsyncEmblem(window, emblem);
			if (emblem.get_HasSwapped())
			{
				window.set_Emblem(emblem.get_Texture());
			}
			else
			{
				emblem.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)asyncEmblem.OnTextureSwapped);
			}
			return asyncEmblem;
		}

		private void OnTextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_window.set_Emblem(e.get_NewValue());
		}
	}
}
