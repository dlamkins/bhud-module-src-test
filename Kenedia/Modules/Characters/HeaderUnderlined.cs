using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters
{
	public class HeaderUnderlined : Panel
	{
		private static ContentService contentService = new ContentService();

		private BitmapFont _Font;

		private bool AlignCentered;

		public Label textLabel;

		public Image Separator_Image;

		private string _Text;

		private int _HorizontalPadding = 5;

		private int _VerticalPadding = 3;

		public BitmapFont Font
		{
			get
			{
				return _Font;
			}
			set
			{
				_Font = value;
				textLabel.Font = value;
				textLabel.Height = _Font.LineHeight + 4;
				if (AlignCentered)
				{
					textLabel.Width = Math.Max((int)Math.Ceiling(_Font.MeasureString(textLabel.Text).Width) + _HorizontalPadding, base.Width - _HorizontalPadding * 2);
				}
				Invalidate();
			}
		}

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
				textLabel.Text = value;
				textLabel.Height = Font.LineHeight + 4;
				if (AlignCentered)
				{
					textLabel.Width = Math.Max((int)Math.Ceiling(Font.MeasureString(value).Width) + _HorizontalPadding, base.Width - _HorizontalPadding * 2);
					Invalidate();
				}
			}
		}

		public int HorizontalPadding
		{
			get
			{
				return _HorizontalPadding;
			}
			set
			{
				_HorizontalPadding = value;
				textLabel.Location = new Point(_HorizontalPadding, _VerticalPadding);
				Separator_Image.Location = new Point(0, textLabel.Location.Y + textLabel.Height + _VerticalPadding);
			}
		}

		public int VerticalPadding
		{
			get
			{
				return _VerticalPadding;
			}
			set
			{
				_VerticalPadding = value;
				textLabel.Location = new Point(_HorizontalPadding, _VerticalPadding);
				Separator_Image.Location = new Point(0, textLabel.Location.Y + textLabel.Height + _VerticalPadding);
			}
		}

		public HeaderUnderlined()
		{
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			_Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular);
			textLabel = new Label
			{
				Location = new Point(_HorizontalPadding, 0),
				Text = "",
				Parent = this,
				AutoSizeWidth = true,
				Height = Font.LineHeight + 4,
				Font = Font
			};
			Separator_Image = new Image
			{
				Texture = Textures.Icons[19],
				Parent = this,
				Location = new Point(0, textLabel.Location.Y + textLabel.Height + VerticalPadding),
				Size = new Point(base.Width, 4)
			};
			base.Resized += delegate
			{
				textLabel.Invalidate();
				Separator_Image.Size = new Point(base.Width, 4);
				Separator_Image.Location = new Point(0, textLabel.Location.Y + textLabel.Height + VerticalPadding);
			};
		}

		public HeaderUnderlined(bool centered)
		{
			AlignCentered = centered;
			WidthSizingMode = ((!AlignCentered) ? SizingMode.AutoSize : SizingMode.Standard);
			HeightSizingMode = SizingMode.AutoSize;
			_Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular);
			textLabel = new Label
			{
				Location = new Point(_HorizontalPadding, 0),
				Text = "",
				Parent = this,
				Height = Font.LineHeight + 4,
				Font = Font
			};
			Separator_Image = new Image
			{
				Texture = Textures.Icons[19],
				Parent = this,
				Location = new Point(0, textLabel.Location.Y + textLabel.Height + VerticalPadding),
				Size = new Point(base.Width, 4)
			};
			base.Resized += delegate
			{
				Separator_Image.Size = new Point(base.Width, 4);
				Separator_Image.Location = new Point(0, textLabel.Location.Y + textLabel.Height + VerticalPadding);
				int num = Math.Max((int)Math.Ceiling(Font.MeasureString(textLabel.Text).Width) + _HorizontalPadding, base.Width - _HorizontalPadding);
				textLabel.Width = num;
				if (num > base.Width - _HorizontalPadding)
				{
					base.Width = num + _HorizontalPadding;
					Invalidate();
				}
				Module.Logger.Debug("TEXT:" + textLabel.Text + "; LABEL WIDTH: " + textLabel.Width + "; HEADER WIDTH: " + base.Width);
			};
		}
	}
}
