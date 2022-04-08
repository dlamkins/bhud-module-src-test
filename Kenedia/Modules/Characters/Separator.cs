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
		{
			_Separator = new Image
			{
				Location = new Point(LeftPadding, TopPadding),
				Texture = Textures.Icons[19],
				Parent = this,
				Size = new Point(base.Width - (LeftPadding + RightPadding), Thickness)
			};
			base.Height = TopPadding + BottomPadding + _Separator.Height;
			WidthSizingMode = SizingMode.Fill;
			base.PropertyChanged += delegate
			{
				base.Height = TopPadding + BottomPadding + _Separator.Height;
				_Separator.Location = new Point(LeftPadding, TopPadding);
				_Separator.Size = new Point(base.Width - (LeftPadding + RightPadding), Thickness);
			};
		}
	}
}
