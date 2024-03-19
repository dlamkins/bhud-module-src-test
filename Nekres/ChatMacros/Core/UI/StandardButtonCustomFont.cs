using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.ChatMacros.Core.UI
{
	internal class StandardButtonCustomFont : StandardButton
	{
		public StandardButtonCustomFont(BitmapFont font)
			: this()
		{
			((LabelBase)this)._font = (BitmapFont)(object)font;
		}
	}
}
