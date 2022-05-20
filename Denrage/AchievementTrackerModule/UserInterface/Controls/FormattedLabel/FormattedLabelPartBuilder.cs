using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel
{
	public class FormattedLabelPartBuilder
	{
		private readonly string _text;

		private bool _isItalic;

		private bool _isStrikeThrough;

		private bool _isUnderlined;

		private Action _link;

		private AsyncTexture2D _prefixImage;

		private AsyncTexture2D _suffixImage;

		private Point _prefixImageSize = new Point(32, 32);

		private Point _suffixImageSize = new Point(32, 32);

		private Color _textColor;

		private Color _hoverColor = Color.get_LightBlue();

		private FontSize _fontSize = (FontSize)18;

		internal FormattedLabelPartBuilder(string text)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_text = text;
		}

		public FormattedLabelPartBuilder MakeItalic()
		{
			_isItalic = true;
			return this;
		}

		public FormattedLabelPartBuilder MakeStrikeThrough()
		{
			_isStrikeThrough = true;
			return this;
		}

		public FormattedLabelPartBuilder MakeUnderlined()
		{
			_isUnderlined = true;
			return this;
		}

		public FormattedLabelPartBuilder SetLink(Action onLink)
		{
			_link = onLink;
			return this;
		}

		public FormattedLabelPartBuilder SetHyperLink(string link)
		{
			_link = delegate
			{
				Process.Start(link);
			};
			return this;
		}

		public FormattedLabelPartBuilder SetPrefixImage(AsyncTexture2D prefixImage)
		{
			_prefixImage = prefixImage;
			return this;
		}

		public FormattedLabelPartBuilder SetSuffixImage(AsyncTexture2D suffixImage)
		{
			_suffixImage = suffixImage;
			return this;
		}

		public FormattedLabelPartBuilder SetPrefixImageSize(Point imageSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_prefixImageSize = imageSize;
			return this;
		}

		public FormattedLabelPartBuilder SetSuffixImageSize(Point imageSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_suffixImageSize = imageSize;
			return this;
		}

		public FormattedLabelPartBuilder SetTextColor(Color textColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_textColor = textColor;
			return this;
		}

		public FormattedLabelPartBuilder SetHoverColor(Color hoverColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_hoverColor = hoverColor;
			return this;
		}

		public FormattedLabelPartBuilder SetFontSize(FontSize fontSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_fontSize = fontSize;
			return this;
		}

		internal FormattedLabelPart Build()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelPart(_isItalic, _isStrikeThrough, _isUnderlined, _text, _link, _prefixImage, _suffixImage, _prefixImageSize, _suffixImageSize, _textColor, _hoverColor, _fontSize, (FontFace)0);
		}
	}
}
