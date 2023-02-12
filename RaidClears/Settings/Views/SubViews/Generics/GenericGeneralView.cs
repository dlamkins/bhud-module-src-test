using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews.Generics
{
	public class GenericGeneralView : View
	{
		private readonly GenericSettings _settings;

		private readonly IEnumerable<SettingEntry>? _extraSettings;

		public GenericGeneralView(GenericSettings settings, IEnumerable<SettingEntry>? extraSettings = null)
			: this()
		{
			_settings = settings;
			_extraSettings = extraSettings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSetting((SettingEntry)(object)_settings.PositionLock).AddSpace()
				.AddSetting((SettingEntry)(object)_settings.Visible)
				.AddSetting((SettingEntry)(object)_settings.Tooltips)
				.AddSetting((SettingEntry)(object)_settings.ToolbarIcon)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.ShowHideKeyBind)
				.AddString(Strings.SharedKeybind)
				.AddSpace()
				.AddSetting(_extraSettings);
		}
	}
}
