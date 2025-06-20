using System.Reflection;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser.Settings.Views.Generic
{
	public class CustomSettingMenuView : SettingsMenuView
	{
		protected static int NEW_MENU_WIDTH = 200;

		public CustomSettingMenuView(ISettingsMenuRegistrar settingsMenuRegistrar)
			: this(settingsMenuRegistrar)
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			((SettingsMenuView)this).Build(buildPanel);
			Menu menuPanel = GetMenuPanel();
			if (menuPanel != null)
			{
				Container parent = ((Control)menuPanel).get_Parent();
				((Panel)((parent is Panel) ? parent : null)).set_Title("");
				((Control)((Control)menuPanel).get_Parent()).set_Size(new Point(NEW_MENU_WIDTH, ((Control)buildPanel).get_Height()));
				Rectangle contentRegion = ((Control)menuPanel).get_Parent().get_ContentRegion();
				((Control)menuPanel).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			}
			ViewContainer viewContainer = GetViewContainer();
			if (viewContainer != null)
			{
				int num = buildPanel.get_ContentRegion().Width - NEW_MENU_WIDTH;
				Thickness padding = ((Control)buildPanel).get_Padding();
				((Control)viewContainer).set_Size(new Point(num - (int)((Thickness)(ref padding)).get_Left(), buildPanel.get_ContentRegion().Height));
				((Control)viewContainer).set_Location(new Point(NEW_MENU_WIDTH + 5, 10));
			}
		}

		private Menu? GetMenuPanel()
		{
			object obj = typeof(SettingsMenuView).GetField("_menuSettingsList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
			return (Menu?)((obj is Menu) ? obj : null);
		}

		private ViewContainer? GetViewContainer()
		{
			object obj = typeof(SettingsMenuView).GetField("_settingViewContainer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
			return (ViewContainer?)((obj is ViewContainer) ? obj : null);
		}
	}
}
