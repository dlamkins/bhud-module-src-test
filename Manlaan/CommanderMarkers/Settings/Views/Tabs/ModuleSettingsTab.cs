using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Views.SubViews;
using Manlaan.CommanderMarkers.Utils;

namespace Manlaan.CommanderMarkers.Settings.Views.Tabs
{
	public class ModuleSettingsTab : ISettingsMenuRegistrar
	{
		private readonly List<MenuViewItem> _registeredMenuItems = new List<MenuViewItem>();

		public event EventHandler<EventArgs>? RegistrarListChanged;

		public ModuleSettingsTab()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Clickable Markers"), (MenuItem _) => (IView)(object)new MarkerPanelSettingsView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("AutoMarker Settings"), (MenuItem _) => (IView)(object)new AutoMarkerSettingsView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("AutoMarker Library"), (MenuItem _) => (IView)(object)new AutoMarkerLibraryView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Community Library"), (MenuItem _) => (IView)(object)new AutoMarkerCommunityLibraryView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Keybinds"), (MenuItem _) => (IView)(object)new KeybindSettingsView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("General"), (MenuItem _) => (IView)(object)new CornerIconSettingsView()));
		}

		public void ActivateLibraryTab()
		{
			_registeredMenuItems[2].MenuItem.Select();
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
