using System.Linq;
using System.Text;
using FontStashSharp;

namespace LoreBridge
{
	public static class DrawUtilCustom
	{
		private static string WrapTextSegment(SpriteFontBase spriteFont, string text, float maxLineWidth)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			bool num = text.Any(IsCjk);
			StringBuilder sb = new StringBuilder();
			if (num)
			{
				float lineWidth2 = 0f;
				for (int i = 0; i < text.Length; i++)
				{
					char character = text[i];
					float charWidth = spriteFont.MeasureString(character.ToString()).X;
					if (lineWidth2 + charWidth > maxLineWidth)
					{
						sb.Append("\n");
						lineWidth2 = 0f;
					}
					sb.Append(character);
					lineWidth2 += charWidth;
				}
			}
			else
			{
				string[] array = text.Split(' ');
				float lineWidth = 0f;
				float spaceWidth = spriteFont.MeasureString(" ").X;
				string[] array2 = array;
				foreach (string word in array2)
				{
					float wordWidth = spriteFont.MeasureString(word).X;
					if (lineWidth + wordWidth < maxLineWidth)
					{
						sb.Append(word + " ");
						lineWidth += wordWidth + spaceWidth;
					}
					else
					{
						sb.Append("\n" + word + " ");
						lineWidth = wordWidth + spaceWidth;
					}
				}
			}
			return sb.ToString();
		}

		private static bool IsCjk(char c)
		{
			if ((c < '一' || c > '\u9fff') && (c < '\u3040' || c > 'ゟ') && (c < '゠' || c > 'ヿ'))
			{
				if (c >= '\uff00')
				{
					return c <= '\uffef';
				}
				return false;
			}
			return true;
		}

		public static string WrapText(SpriteFontBase spriteFont, string text, float maxLineWidth)
		{
			if (!string.IsNullOrEmpty(text))
			{
				return string.Join("\n", from s in text.Split('\n')
					select WrapTextSegment(spriteFont, s, maxLineWidth));
			}
			return "";
		}
	}
}
