using Blish_HUD;
using Blish_HUD.Controls;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DanceDanceRotationModule.Views
{
	public class HelpWindow : StandardWindow
	{
		public const int InitialWidth = 830;

		public const int InitialHeight = 360;

		public HelpWindow()
			: this(Resources.Instance.WindowBackgroundTexture, new Rectangle(40, 26, 913, 691), new Rectangle(40, 26, 913, 691))
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Welcome to Dance Dance Rotation!");
			((WindowBase2)this).set_Emblem(Resources.Instance.DdrLogoEmblemTexture);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_CanCloseWithEscape(false);
			((Control)this).set_Size(new Point(830, 360));
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_SavesSize(false);
			((WindowBase2)this).set_Id("DDR_HelpWindow_ID");
		}

		public HelpWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(830, 360);
		}
	}
}
