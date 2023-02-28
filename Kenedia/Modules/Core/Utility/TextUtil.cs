using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Utility
{
	public static class TextUtil
	{
		private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			string[] array = text.Split(' ');
			StringBuilder sb = new StringBuilder();
			float lineWidth = 0f;
			float spaceWidth = spriteFont.MeasureString(" ").Width;
			string[] array2 = array;
			foreach (string word in array2)
			{
				Vector2 size = Size2.op_Implicit(spriteFont.MeasureString(word));
				if (lineWidth + size.X < maxLineWidth)
				{
					sb.Append(word + " ");
					lineWidth += size.X + spaceWidth;
				}
				else
				{
					sb.Append("\n" + word + " ");
					lineWidth = size.X + spaceWidth;
				}
			}
			return sb.ToString();
		}

		public static string WrapText(BitmapFont spriteFont, string text, float maxLineWidth)
		{
			if (!string.IsNullOrEmpty(text))
			{
				return string.Join("\n", from s in text.Split('\n')
					select WrapTextSegment(spriteFont, s, maxLineWidth));
			}
			return string.Empty;
		}
	}
}
