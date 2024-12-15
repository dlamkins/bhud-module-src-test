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
	public class RaidsSettingTab : ISettingsMenuRegistrar
	{
		private readonly List<MenuViewItem> _registeredMenuItems = new List<MenuViewItem>();

		public event EventHandler<EventArgs>? RegistrarListChanged;

		public RaidsSettingTab()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Raids_Heading_General), (MenuItem _) => (IView)(object)new GenericGeneralView(Service.Settings.RaidSettings.Generic, new List<SettingEntry>
			{
				(SettingEntry)(object)Service.Settings.RaidSettings.RaidPanelHighlightEmbolden,
				(SettingEntry)(object)Service.Settings.RaidSettings.RaidPanelHighlightCotM
			})));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Raids_Heading_Layout), (MenuItem _) => (IView)(object)new GenericStyleView(Service.Settings.RaidSettings.Style, new List<SettingEntry<string>>
			{
				Service.Settings.RaidSettings.RaidPanelColorEmbolden,
				Service.Settings.RaidSettings.RaidPanelColorCotm
			})));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem(Strings.SettingsPanel_Raids_Heading_WingSelection), (MenuItem _) => (IView)(object)new DynamicRaidSelectionView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Customize Labels"), (MenuItem _) => (IView)(object)new RaidLabelCustomizationView()));
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
