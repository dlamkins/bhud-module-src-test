using Blish_HUD;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.ProofLogix.Core
{
	public static class LabelUtil
	{
		public static Point GetLabelSize(BitmapFont font, string text, bool hasPrefix = false, bool hasSuffix = false)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			Size2 icon = font.MeasureString(".");
			Size2 size = font.MeasureString(text);
			float width = ((hasPrefix && hasSuffix) ? (size.Width + icon.Height * 4f) : ((!(hasPrefix || hasSuffix)) ? size.Width : (size.Width + icon.Height * 2f)));
			return new Point((int)width, (int)size.Height);
		}

		public static Point GetLabelSize(FontSize fontSize, string text, bool hasPrefix = false, bool hasSuffix = false)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return GetLabelSize(GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0), text, hasPrefix, hasSuffix);
		}
	}
}
