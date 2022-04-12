using System.ComponentModel;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class Separator : Panel
	{
		public Image _Separator;

		public int TopPadding;

		public int BottomPadding = 4;

		public int LeftPadding;

		public int RightPadding;

		public int Thickness = 4;

		public Separator()
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Location(new Point(LeftPadding, TopPadding));
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[19]));
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(((Control)this).get_Width() - (LeftPadding + RightPadding), Thickness));
			_Separator = val;
			((Control)this).set_Height(TopPadding + BottomPadding + ((Control)_Separator).get_Height());
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				((Control)this).set_Height(TopPadding + BottomPadding + ((Control)_Separator).get_Height());
				((Control)_Separator).set_Location(new Point(LeftPadding, TopPadding));
				((Control)_Separator).set_Size(new Point(((Control)this).get_Width() - (LeftPadding + RightPadding), Thickness));
			});
		}
	}
}
