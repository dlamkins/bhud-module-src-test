using Blish_HUD;
using Blish_HUD.Content;
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
				((Control)Label).set_Location(new Point(Font.LineHeight + 4 + _Gap, 0));
				((Control)this).Invalidate();
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
					Image.set_Texture(AsyncTexture2D.op_Implicit(_Texture));
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
					Label.set_Text(_Text);
					((Control)this).Invalidate();
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
					Label.set_Font(_Font);
					((Control)Label).set_Height(Font.LineHeight + 4);
					((Control)Image).set_Size(new Point(Font.LineHeight + 4, Font.LineHeight + 4));
					((Control)Label).set_Location(new Point(Font.LineHeight + 4 + _Gap, 0));
					((Control)this).Invalidate();
				}
			}
		}

		public IconLabel()
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			_Font = new ContentService().GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
			Image val = new Image();
			((Control)val).set_Size(new Point(Font.LineHeight + 4, Font.LineHeight + 4));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Parent((Container)(object)this);
			Image = val;
			Label val2 = new Label();
			((Control)val2).set_Height(Font.LineHeight + 4);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Location(new Point(((Control)Image).get_Size().X + _Gap, 0));
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_AutoSizeWidth(true);
			Label = val2;
		}
	}
}
