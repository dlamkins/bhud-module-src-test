using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using RaidClears.Localization;
using RaidClears.Settings.Views.SubViews;
using RaidClears.Settings.Views.SubViews.Generics;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.Tabs
{
	public class DungeonSettingTab : ISettingsMenuRegistrar
	{
		private readonly List<MenuViewItem> _registeredMenuItems = new List<MenuViewItem>();

		public event EventHandler<EventArgs>? RegistrarListChanged;

		public DungeonSettingTab()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Dun_Heading_General), (MenuItem _) => (IView)(object)new GenericGeneralView(Service.Settings.DungeonSettings.Generic, new List<SettingEntry> { (SettingEntry)(object)Service.Settings.DungeonSettings.DungeonHighlightFrequenter })));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Dun_Heading_Layout), (MenuItem _) => (IView)(object)new GenericStyleView(Service.Settings.DungeonSettings.Style, new List<SettingEntry<string>> { Service.Settings.DungeonSettings.DungeonPanelColorFreq }, showCopyRaids: true)));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Dun_Heading_PathSelection), (MenuItem _) => (IView)(object)new DungeonPathSelectionView(Service.Settings.DungeonSettings)));
		}

		public IEnumerable<MenuItem> GetSettingMenus()
		{
			return _registeredMenuItems.Select((MenuViewItem mi) => mi.MenuItem);
		}

		public IView? GetMenuItemView(MenuItem selectedMenuItem)
		{
			foreach (var (menuItem, viewFunc) in _registeredMenuItems)
			{
				if (menuItem == selectedMenuItem || ((Container)menuItem).GetDescendants().Contains((Control)(object)selectedMenuItem))
				{
					return viewFunc(selectedMenuItem);
				}
			}
			return null;
		}
	}
}
