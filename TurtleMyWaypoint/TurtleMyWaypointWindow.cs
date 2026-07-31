using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;

namespace TurtleMyWaypoint
{
	public class TurtleMyWaypointWindow : TabbedWindow2
	{
		private const int LeftOffset = 58;

		private const int RightMargin = 20;

		private const int WindowRegionWidth = 913;

		private const int ContentWidth = 835;

		private const int ContentHeight = 636;

		private readonly Tab _coreTyriaTab;

		private readonly Tab _heartOfThornsTab;

		private readonly Tab _pathOfFireTab;

		private readonly Tab _endOfDragonsTab;

		private readonly Tab _secretsOfTheObscureTab;

		private readonly Tab _janthirWildsTab;

		private readonly Tab _visionsOfEternityTab;

		private readonly Tab _livingWorldTab;

		public TurtleMyWaypointWindow(ContentsManager contentsManager)
			: this(contentsManager.GetTexture("window_bg.png"), new Rectangle(40, 26, 913, 691), new Rectangle(98, 40, 835, 636))
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Expected O, but got Unknown
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Expected O, but got Unknown
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Expected O, but got Unknown
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Turtle My Waypoint");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("window_emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("TurtleMyWaypointWindow_com.floraaubry.turtlemywaypoint_9e2a1d3c-6b7f-4a3e-8c1a-1f9d2e5b7a44");
			AsyncTexture2D coreIcon = new AsyncTexture2D(contentsManager.GetTexture("core.png"));
			AsyncTexture2D hotIcon = new AsyncTexture2D(contentsManager.GetTexture("hot.png"));
			AsyncTexture2D pofIcon = new AsyncTexture2D(contentsManager.GetTexture("pof.png"));
			AsyncTexture2D eodIcon = new AsyncTexture2D(contentsManager.GetTexture("eod.png"));
			AsyncTexture2D sotoIcon = new AsyncTexture2D(contentsManager.GetTexture("soto.png"));
			AsyncTexture2D janthirIcon = new AsyncTexture2D(contentsManager.GetTexture("janthir.png"));
			AsyncTexture2D voeIcon = new AsyncTexture2D(contentsManager.GetTexture("voe.png"));
			AsyncTexture2D lwIcon = new AsyncTexture2D(contentsManager.GetTexture("lw.png"));
			_coreTyriaTab = new Tab(coreIcon, (Func<IView>)(() => (IView)(object)new CoreTyriaView(835, 636)), Strings.Tab_CoreTyria, (int?)0);
			_heartOfThornsTab = new Tab(hotIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.HeartOfThorns, "Maguuma Jungle", "Jungle de Maguuma", 835, 636)), Strings.Tab_HeartOfThorns, (int?)10);
			_pathOfFireTab = new Tab(pofIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.PathOfFire, "Crystal Desert", "Désert de cristal", 835, 636)), Strings.Tab_PathOfFire, (int?)20);
			_endOfDragonsTab = new Tab(eodIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.EndOfDragons, "Cantha", "Cantha", 835, 636)), Strings.Tab_EndOfDragons, (int?)30);
			_secretsOfTheObscureTab = new Tab(sotoIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.SecretsOfTheObscure, "Horn of Maguuma", "Corne de Maguuma", 835, 636)), Strings.Tab_SecretsOfTheObscure, (int?)40);
			_janthirWildsTab = new Tab(janthirIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.JanthirWilds, "Janthir Bay", "Baie de Janthir", 835, 636)), Strings.Tab_JanthirWilds, (int?)50);
			_visionsOfEternityTab = new Tab(voeIcon, (Func<IView>)(() => (IView)(object)new MapWaypointsView(WaypointData.VisionsOfEternity, "Castora", "Castora", 835, 636)), Strings.Tab_VisionsOfEternity, (int?)60);
			_livingWorldTab = new Tab(lwIcon, (Func<IView>)(() => (IView)(object)new LivingWorldView(835, 636)), Strings.Tab_LivingWorld, (int?)70);
			((TabbedWindow2)this).get_Tabs().Add(_coreTyriaTab);
			((TabbedWindow2)this).get_Tabs().Add(_heartOfThornsTab);
			((TabbedWindow2)this).get_Tabs().Add(_pathOfFireTab);
			((TabbedWindow2)this).get_Tabs().Add(_endOfDragonsTab);
			((TabbedWindow2)this).get_Tabs().Add(_secretsOfTheObscureTab);
			((TabbedWindow2)this).get_Tabs().Add(_janthirWildsTab);
			((TabbedWindow2)this).get_Tabs().Add(_visionsOfEternityTab);
			((TabbedWindow2)this).get_Tabs().Add(_livingWorldTab);
			((TabbedWindow2)this).set_SelectedTab(_coreTyriaTab);
			((WindowBase2)this).set_Subtitle(_coreTyriaTab.get_Name());
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)delegate
			{
				Tab selectedTab = ((TabbedWindow2)this).get_SelectedTab();
				((WindowBase2)this).set_Subtitle((selectedTab != null) ? selectedTab.get_Name() : null);
			});
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}

		private void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			_coreTyriaTab.set_Name(Strings.Tab_CoreTyria);
			_heartOfThornsTab.set_Name(Strings.Tab_HeartOfThorns);
			_pathOfFireTab.set_Name(Strings.Tab_PathOfFire);
			_endOfDragonsTab.set_Name(Strings.Tab_EndOfDragons);
			_secretsOfTheObscureTab.set_Name(Strings.Tab_SecretsOfTheObscure);
			_janthirWildsTab.set_Name(Strings.Tab_JanthirWilds);
			_visionsOfEternityTab.set_Name(Strings.Tab_VisionsOfEternity);
			_livingWorldTab.set_Name(Strings.Tab_LivingWorld);
			Tab selectedTab = ((TabbedWindow2)this).get_SelectedTab();
			((WindowBase2)this).set_Subtitle((selectedTab != null) ? selectedTab.get_Name() : null);
			if (((Control)this).get_Visible() && ((TabbedWindow2)this).get_SelectedTab() != null)
			{
				((WindowBase2)this).ShowView(((TabbedWindow2)this).get_SelectedTab().get_View()());
			}
		}

		protected override void DisposeControl()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			((WindowBase2)this).DisposeControl();
		}
	}
}
