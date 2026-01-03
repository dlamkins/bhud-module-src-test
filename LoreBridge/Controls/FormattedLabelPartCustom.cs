using System;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace LoreBridge.Controls
{
	internal class FormattedLabelPartCustom : IDisposable
	{
		public SpriteFontBase Font { get; set; }

		public string Text { get; }

		public Color TextColor { get; }

		public FormattedLabelPartCustom(SpriteFontBase font, string text, Color textColor)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			Font = font;
			Text = text;
			TextColor = ((textColor == default(Color)) ? Color.get_White() : textColor);
			base._002Ector();
		}

		public void Dispose()
		{
		}
	}
}
