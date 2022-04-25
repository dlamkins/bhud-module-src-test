using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Musician.UI.Controls
{
	internal class ClipboardButton : Image
	{
		private Texture2D _clipboard;

		private Texture2D _clipboardHover;

		public ClipboardButton()
			: this()
		{
			_clipboard = MusicianModule.ModuleInstance.ContentsManager.GetTexture("clipboard_hover.png");
			_clipboardHover = MusicianModule.ModuleInstance.ContentsManager.GetTexture("clipboard.png");
			((Image)this).set_Texture(AsyncTexture2D.op_Implicit(_clipboard));
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Image)this).set_Texture(AsyncTexture2D.op_Implicit(_clipboardHover));
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Image)this).set_Texture(AsyncTexture2D.op_Implicit(_clipboard));
			((Control)this).OnMouseLeft(e);
		}

		protected override void DisposeControl()
		{
			_clipboard.Dispose();
			_clipboardHover.Dispose();
			((Control)this).DisposeControl();
		}
	}
}
