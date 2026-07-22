using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Utility
{
	public static class TextUtil
	{
		private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth)
		{
			string[] array = text.Split(' ');
			StringBuilder sb = new StringBuilder();
			float lineWidth = 0f;
			float spaceWidth = spriteFont.MeasureString(" ").Width;
			string[] array2 = array;
			foreach (string word in array2)
			{
				Vector2 size = spriteFont.MeasureString(word);
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
			BitmapFont spriteFont2 = spriteFont;
			if (!string.IsNullOrEmpty(text))
			{
				return string.Join("\n", from s in text.Split('\n')
					select WrapTextSegment(spriteFont2, s, maxLineWidth));
			}
			return string.Empty;
		}
	}
}
