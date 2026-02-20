using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using CinemaModule;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.SettingsSmall
{
	public abstract class SmallWindow : StandardWindow
	{
		private static readonly Rectangle DefaultWindowRegion = new Rectangle(25, 26, 435, 500);

		private static readonly Rectangle DefaultContentRegion = new Rectangle(40, 40, 415, 450);

		private static AsyncTexture2D BackgroundTexture => global::CinemaModule.CinemaModule.Instance.TextureService.GetSmallWindowBackground();

		protected SmallWindow(string title)
			: this(BackgroundTexture, DefaultWindowRegion, DefaultContentRegion)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title(title);
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(global::CinemaModule.CinemaModule.Instance.TextureService.GetEmblem()));
			((Control)this).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)this).get_Height()) / 2));
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_CanResize(false);
		}

		protected void Initialize()
		{
			BuildContent();
		}

		protected abstract void BuildContent();
	}
}
