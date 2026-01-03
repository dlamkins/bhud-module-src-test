using FontStashSharp;
using Microsoft.Xna.Framework;

namespace LoreBridge.Controls
{
	public class FormattedLabelPartBuilderCustom
	{
		private SpriteFontBase _font;

		private readonly string _text;

		private Color _textColor;

		internal FormattedLabelPartBuilderCustom(string text)
		{
			_text = text;
		}

		public FormattedLabelPartBuilderCustom SetFont(SpriteFontBase font)
		{
			_font = font;
			return this;
		}

		public FormattedLabelPartBuilderCustom SetTextColor(Color textColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_textColor = textColor;
			return this;
		}

		internal FormattedLabelPartCustom Build()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelPartCustom(_font, _text, _textColor);
		}
	}
}
