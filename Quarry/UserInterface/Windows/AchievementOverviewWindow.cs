using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.Models.Persistence;
using Quarry.UserInterface.Views;

namespace Quarry.UserInterface.Windows
{
	public class AchievementOverviewWindow : TabbedWindow2
	{
		private static readonly Point MinWindowSize = new Point(900, 680);

		private static readonly Point MaxWindowSize = new Point(1200, 1100);

		private const int ScreenMarginX = 200;

		private const int ScreenMarginY = 180;

		private bool applyingSizeClamp;

		public AchievementOverviewWindow(ContentsManager contentsManager, IAchievementItemOverviewFactory achievementItemOverviewFactory, IAchievementService achievementService, ITextureService textureService, IHereService hereService, ICurrentMapService currentMapService, IAchievementCardFactory achievementCardFactory, IHereExclusionService hereExclusionService, IAchievementTrackerService achievementTrackerService, IPersistenceService persistenceService, SettingEntry<int> hereCap, Action openTrackedWindow)
			: this(contentsManager.GetTexture("window_blank.png"), new Rectangle(0, 0, 900, 640), new Rectangle(95, 42, 821, 592))
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Quarry");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("achievement_icon.png"));
			((WindowBase2)this).set_Id("Quarry_OverviewWindow");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_CanResize(true);
			Tab achievementsTab = new Tab(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("achievement_icon.png")), (Func<IView>)(() => (IView)(object)new AchievementTrackerView(achievementItemOverviewFactory, achievementService, textureService, openTrackedWindow)), "All", (int?)null);
			Tab hereTab = new Tab(AsyncTexture2D.FromAssetId(157123), (Func<IView>)(() => (IView)(object)new HereView(hereService, currentMapService, achievementCardFactory, hereExclusionService, achievementTrackerService, hereCap, openTrackedWindow)), "Here", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(achievementsTab);
			((TabbedWindow2)this).get_Tabs().Add(hereTab);
			Storage storage = persistenceService.Get();
			Screen screen = GameService.Graphics.get_SpriteScreen();
			Point defaultSize = default(Point);
			((Point)(ref defaultSize))._002Ector(MathHelper.Clamp(((Control)screen).get_Width() - 200, MinWindowSize.X, MaxWindowSize.X), MathHelper.Clamp(((Control)screen).get_Height() - 180, MinWindowSize.Y, MaxWindowSize.Y));
			((Control)this).set_Size((Point)((storage.OverviewWindowWidth > 0 && storage.OverviewWindowHeight > 0) ? new Point(MathHelper.Clamp(storage.OverviewWindowWidth, MinWindowSize.X, MaxWindowSize.X), MathHelper.Clamp(storage.OverviewWindowHeight, MinWindowSize.Y, MaxWindowSize.Y)) : defaultSize));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			if (!applyingSizeClamp)
			{
				Point clamped = default(Point);
				((Point)(ref clamped))._002Ector(MathHelper.Clamp(((Control)this).get_Size().X, MinWindowSize.X, MaxWindowSize.X), MathHelper.Clamp(((Control)this).get_Size().Y, MinWindowSize.Y, MaxWindowSize.Y));
				if (clamped != ((Control)this).get_Size())
				{
					applyingSizeClamp = true;
					((Control)this).set_Size(clamped);
					applyingSizeClamp = false;
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			WindowBodyPainter.PaintBody(spriteBatch, (Control)(object)this, ((Container)this).get_ContentRegion(), showLeftAccent: true);
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
