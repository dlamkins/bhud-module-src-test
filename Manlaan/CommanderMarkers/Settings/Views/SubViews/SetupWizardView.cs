using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class SetupWizardView : View
	{
		protected SettingService _settings;

		protected override void Build(Container buildPanel)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			((Container)(object)FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel)).AddControl((Control)new FlowPanel(), out var GeneralSettingsPanel);
			FlowPanelExtensions.AddSetting((FlowPanel)GeneralSettingsPanel, (SettingEntry)(object)_settings.CornerIconTexture);
		}

		public SetupWizardView()
			: this()
		{
		}
	}
}
