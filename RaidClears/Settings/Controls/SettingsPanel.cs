using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Localization;
using RaidClears.Settings.Views;
using RaidClears.Settings.Views.SubViews;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Settings.Controls
{
	public class SettingsPanel : TabbedWindow2
	{
		private static Texture2D? Background => Service.Textures?.SettingWindowBackground;

		private static Rectangle SettingPanelRegion
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				Rectangle result = default(Rectangle);
				((Rectangle)(ref result)).set_Location(new Point(38, 25));
				((Rectangle)(ref result)).set_Size(new Point(1100, 705));
				return result;
			}
		}

		private static Rectangle SettingPanelContentRegion
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle result = default(Rectangle);
				Rectangle settingPanelRegion = SettingPanelRegion;
				((Rectangle)(ref result)).set_Location(((Rectangle)(ref settingPanelRegion)).get_Location() + new Point(52, 0));
				settingPanelRegion = SettingPanelRegion;
				Point size = ((Rectangle)(ref settingPanelRegion)).get_Size();
				settingPanelRegion = SettingPanelRegion;
				((Rectangle)(ref result)).set_Size(size - ((Rectangle)(ref settingPanelRegion)).get_Location());
				return result;
			}
		}

		private static Point SettingPanelWindowSize => new Point(800, 600);

		public SettingsPanel()
			: this(Background, SettingPanelRegion, SettingPanelContentRegion, SettingPanelWindowSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Id("Module_96b38a83-4163-4d97-b894-282406b29a48");
			((WindowBase2)this).set_Emblem(Service.Textures?.SettingWindowEmblem);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title(Strings.Module_Title);
			((WindowBase2)this).set_Subtitle(Strings.SettingsPanel_Subtitle);
			((WindowBase2)this).set_SavesPosition(true);
			Service.Settings.SettingsPanelKeyBind.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				((WindowBase2)this).ToggleWindow();
			});
			BuildTabs();
		}

		private void BuildTabs()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Expected O, but got Unknown
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Textures?.SettingTabRaid), (Func<IView>)(() => (IView)(object)new CustomSettingMenuView((ISettingsMenuRegistrar)(object)new RaidsSettingTab())), Strings.SettingsPanel_Tab_Raids, (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Textures?.SettingTabStrikes), (Func<IView>)(() => (IView)(object)new CustomSettingMenuView((ISettingsMenuRegistrar)(object)new StrikesSettingTab())), Strings.SettingsPanel_Tab_Strikes, (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Textures?.SettingTabFractals), (Func<IView>)(() => (IView)(object)new CustomSettingMenuView((ISettingsMenuRegistrar)(object)new FractalSettingTab())), Strings.SettingsPanel_Tab_Fractals, (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Textures?.SettingTabDungeon), (Func<IView>)(() => (IView)(object)new CustomSettingMenuView((ISettingsMenuRegistrar)(object)new DungeonSettingTab())), Strings.SettingsPanel_Tab_Dunegons, (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Textures?.SettingTabGeneral), (Func<IView>)(() => (IView)(object)new MainSettingsView()), Strings.SettingsPanel_Tab_General, (int?)null));
		}
	}
}
