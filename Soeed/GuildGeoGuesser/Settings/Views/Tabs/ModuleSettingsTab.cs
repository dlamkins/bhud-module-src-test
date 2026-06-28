using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Soeed.GuildGeoGuesser.Settings.Views.SubViews;
using Soeed.GuildGeoGuesser.Settings.Views.TabViews;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.Tabs
{
	public class ModuleSettingsTab : ISettingsMenuRegistrar
	{
		private readonly List<MenuViewItem> _registeredMenuItems = new List<MenuViewItem>();

		public event EventHandler<EventArgs>? RegistrarListChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		public ModuleSettingsTab()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("General Settings"), (MenuItem _) => (IView)(object)new GeneralSettingsView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Keybinds"), (MenuItem _) => (IView)(object)new KeybindsSettingView()));
			_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Help and Info"), (MenuItem _) => (IView)(object)new HelpScreenVersion1View()));
			if (Service.Settings.DebugInformationEnabled.get_Value())
			{
				_registeredMenuItems.Add(new MenuViewItem(new MenuItem("Debug"), (MenuItem _) => (IView)(object)new ConfigValuesView()));
			}
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

		public void ActivateKeybindsTab()
		{
			_registeredMenuItems[1].MenuItem.Select();
		}
	}
}
