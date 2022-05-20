using System;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel
{
	internal class FormattedLabelPart : IDisposable
	{
		public BitmapFont Font { get; }

		public bool IsItalic { get; }

		public bool IsStrikeThrough { get; }

		public bool IsUnderlined { get; }

		public string Text { get; }

		public Action Link { get; }

		public AsyncTexture2D PrefixImage { get; }

		public AsyncTexture2D SuffixImage { get; }

		public Point PrefixImageSize { get; }

		public Point SuffixImageSize { get; }

		public FontSize FontSize { get; }

		public FontFace FontFace { get; }

		public Color TextColor { get; }

		public Color HoverColor { get; }

		public FormattedLabelPart(bool isItalic, bool isStrikeThrough, bool isUnderlined, string text, Action link, AsyncTexture2D prefixImage, AsyncTexture2D suffixImage, Point prefixImageSize, Point suffixImageSize, Color textColor, Color hoverColor, FontSize fontSize, FontFace fontFace)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			IsItalic = isItalic;
			IsStrikeThrough = isStrikeThrough;
			IsUnderlined = isUnderlined;
			Text = text;
			Link = link;
			PrefixImage = prefixImage;
			SuffixImage = suffixImage;
			PrefixImageSize = prefixImageSize;
			SuffixImageSize = suffixImageSize;
			HoverColor = hoverColor;
			FontSize = fontSize;
			FontFace = fontFace;
			TextColor = ((textColor == default(Color)) ? Color.get_White() : textColor);
			FontStyle style = (FontStyle)0;
			if (IsItalic)
			{
				style = (FontStyle)1;
			}
			Font = GameService.Content.GetFont(FontFace, FontSize, style);
		}

		public void Dispose()
		{
			AsyncTexture2D prefixImage = PrefixImage;
			if (prefixImage != null)
			{
				prefixImage.Dispose();
			}
			AsyncTexture2D suffixImage = SuffixImage;
			if (suffixImage != null)
			{
				suffixImage.Dispose();
			}
		}
	}
}
