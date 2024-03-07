using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Settings.Views.Generic;
using Manlaan.CommanderMarkers.Settings.Views.Tabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Controls
{
	public class SettingsPanel : TabbedWindow2
	{
		public ModuleSettingsTab modSettingsTab = new ModuleSettingsTab();

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
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Id("Module_96aaaa83-4163-4d97-b894-282406b29a49");
			((WindowBase2)this).set_Emblem(Service.Textures?._blishHeart);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Commander Markers");
			((WindowBase2)this).set_Subtitle("configuration");
			((WindowBase2)this).set_SavesPosition(true);
			BuildTabs();
		}

		private void BuildTabs()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(Service.Settings.CornerIconTexture.get_Value().GetIcon()), (Func<IView>)(() => (IView)(object)new CustomSettingMenuView((ISettingsMenuRegistrar)(object)modSettingsTab)), "Settings", (int?)null));
		}

		public void ShowLibrary()
		{
			((Control)this).Show();
			modSettingsTab.ActivateLibraryTab();
		}
	}
}
