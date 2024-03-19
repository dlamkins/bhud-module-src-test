using Blish_HUD;
using Blish_HUD.Controls;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DanceDanceRotationModule.Views
{
	public class SongListWindow : StandardWindow
	{
		public SongListWindow()
			: this(Resources.Instance.WindowBackgroundTexture, new Rectangle(40, 26, 913, 691), new Rectangle(40, 26, 913, 691))
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Song List");
			((WindowBase2)this).set_Subtitle("Dance Dance Rotation");
			((WindowBase2)this).set_Emblem(Resources.Instance.DdrLogoEmblemTexture);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((Control)this).set_Size(new Point(500, 400));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_Id("DDR_SongList_ID");
		}

		public SongListWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, 500, 500), MathHelper.Clamp(newSize.Y, 280, 800));
		}
	}
}
