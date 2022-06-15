using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters
{
	public class HeaderUnderlined : Panel
	{
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
				textLabel.set_Font(value);
				((Control)textLabel).set_Height(_Font.LineHeight + 4);
				if (AlignCentered)
				{
					((Control)textLabel).set_Width(Math.Max((int)Math.Ceiling(_Font.MeasureString(textLabel.get_Text()).Width) + _HorizontalPadding, ((Control)this).get_Width() - _HorizontalPadding * 2));
				}
				((Control)this).Invalidate();
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
				textLabel.set_Text(value);
				((Control)textLabel).set_Height(Font.LineHeight + 4);
				if (AlignCentered)
				{
					((Control)textLabel).set_Width(Math.Max((int)Math.Ceiling(Font.MeasureString(value).Width) + _HorizontalPadding, ((Control)this).get_Width() - _HorizontalPadding * 2));
					((Control)this).Invalidate();
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
				((Control)textLabel).set_Location(new Point(_HorizontalPadding, _VerticalPadding));
				((Control)Separator_Image).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + _VerticalPadding));
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
				((Control)textLabel).set_Location(new Point(_HorizontalPadding, _VerticalPadding));
				((Control)Separator_Image).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + _VerticalPadding));
			}
		}

		public HeaderUnderlined()
			: this()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			_Font = GameService.Content.get_DefaultFont18();
			Label val = new Label();
			((Control)val).set_Location(new Point(_HorizontalPadding, 0));
			val.set_Text("");
			((Control)val).set_Parent((Container)(object)this);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Height(Font.LineHeight + 4);
			val.set_Font(Font);
			textLabel = val;
			Image val2 = new Image();
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[19]));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + VerticalPadding));
			((Control)val2).set_Size(new Point(((Control)this).get_Width(), 4));
			Separator_Image = val2;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)textLabel).Invalidate();
				((Control)Separator_Image).set_Size(new Point(((Control)this).get_Width(), 4));
				((Control)Separator_Image).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + VerticalPadding));
			});
		}

		public HeaderUnderlined(bool centered)
			: this()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			AlignCentered = centered;
			((Container)this).set_WidthSizingMode((SizingMode)((!AlignCentered) ? 1 : 0));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			_Font = GameService.Content.get_DefaultFont18();
			Label val = new Label();
			((Control)val).set_Location(new Point(_HorizontalPadding, 0));
			val.set_Text("");
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(Font.LineHeight + 4);
			val.set_Font(Font);
			textLabel = val;
			Image val2 = new Image();
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[19]));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + VerticalPadding));
			((Control)val2).set_Size(new Point(((Control)this).get_Width(), 4));
			Separator_Image = val2;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)Separator_Image).set_Size(new Point(((Control)this).get_Width(), 4));
				((Control)Separator_Image).set_Location(new Point(0, ((Control)textLabel).get_Location().Y + ((Control)textLabel).get_Height() + VerticalPadding));
				int num = Math.Max((int)Math.Ceiling(Font.MeasureString(textLabel.get_Text()).Width) + _HorizontalPadding, ((Control)this).get_Width() - _HorizontalPadding);
				((Control)textLabel).set_Width(num);
				if (num > ((Control)this).get_Width() - _HorizontalPadding)
				{
					((Control)this).set_Width(num + _HorizontalPadding);
					((Control)this).Invalidate();
				}
				Module.Logger.Debug("TEXT:" + textLabel.get_Text() + "; LABEL WIDTH: " + ((Control)textLabel).get_Width() + "; HEADER WIDTH: " + ((Control)this).get_Width());
			});
		}
	}
}
