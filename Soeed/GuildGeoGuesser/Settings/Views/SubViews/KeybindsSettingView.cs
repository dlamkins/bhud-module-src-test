using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.SubViews
{
	public class KeybindsSettingView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Open/Close the " + Service.Config.Name + " window").AddSetting((SettingEntry)(object)Service.Settings.MainWindowToggle)
				.AddSpace(30)
				.AddString("When capturing a screenshot, automatically use the keybinds for:")
				.AddSetting((SettingEntry)(object)Service.Settings.SendHideGw2Ui)
				.AddSetting((SettingEntry)(object)Service.Settings.SendHideBlishUi)
				.AddSetting((SettingEntry)(object)Service.Settings.SendHideArcDpsUi)
				.AddSetting((SettingEntry)(object)Service.Settings.SendHideNexusUi)
				.AddSpace(30)
				.AddString("Hide/Show Keybinds")
				.AddSetting((SettingEntry)(object)Service.Settings.HideGw2Ui)
				.AddSpace(5)
				.AddSetting((SettingEntry)(object)Service.Settings.HideBlishUi)
				.AddSpace(5)
				.AddSetting((SettingEntry)(object)Service.Settings.HideArcDpsUi)
				.AddSpace(5)
				.AddSetting((SettingEntry)(object)Service.Settings.HideNexusUi);
		}

		public KeybindsSettingView()
			: this()
		{
		}
	}
}
