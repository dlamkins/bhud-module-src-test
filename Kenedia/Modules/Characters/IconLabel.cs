using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters
{
	public class IconLabel : Panel
	{
		public CharacterCrafting _Crafting;

		private int _Gap = 8;

		public Image Image;

		public Label Label;

		private Texture2D _Texture;

		private string _Text;

		private BitmapFont _Font;

		public int Gap
		{
			get
			{
				return _Gap;
			}
			set
			{
				_Gap = value;
				Label.Location = new Point(Font.LineHeight + 4 + _Gap, 0);
				Invalidate();
			}
		}

		public Texture2D Texture
		{
			get
			{
				return _Texture;
			}
			set
			{
				_Texture = value;
				if (Image != null)
				{
					Image.Texture = _Texture;
				}
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
				if (Label != null)
				{
					Label.Text = _Text;
					Invalidate();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _Font;
			}
			set
			{
				_Font = value;
				if (Label != null)
				{
					Label.Font = _Font;
					Label.Height = Font.LineHeight + 4;
					Image.Size = new Point(Font.LineHeight + 4, Font.LineHeight + 4);
					Label.Location = new Point(Font.LineHeight + 4 + _Gap, 0);
					Invalidate();
				}
			}
		}

		public IconLabel()
		{
			HeightSizingMode = SizingMode.AutoSize;
			WidthSizingMode = SizingMode.AutoSize;
			_Font = new ContentService().GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular);
			Image = new Image
			{
				Size = new Point(Font.LineHeight + 4, Font.LineHeight + 4),
				Location = new Point(0, 0),
				Parent = this
			};
			Label = new Label
			{
				Height = Font.LineHeight + 4,
				VerticalAlignment = VerticalAlignment.Middle,
				Location = new Point(Image.Size.X + _Gap, 0),
				Parent = this,
				AutoSizeWidth = true
			};
		}
	}
}
