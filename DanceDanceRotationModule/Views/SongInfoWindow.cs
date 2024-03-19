using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DanceDanceRotationModule.Views
{
	public class SongInfoWindow : StandardWindow
	{
		public SongInfoWindow()
			: this(Resources.Instance.SongInfoBackground, new Rectangle(40, 26, 333, 676), new Rectangle(40, 26, 344, 676))
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Song Info");
			((WindowBase2)this).set_Emblem(Resources.Instance.DdrLogoEmblemTexture);
			((Control)this).set_Size(new Point(333, 676));
			((WindowBase2)this).set_CanResize(false);
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("DDR_SongInfo_ID");
		}

		public SongInfoWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public SongInfoWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public SongInfoWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		public SongInfoWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)

	}
}
