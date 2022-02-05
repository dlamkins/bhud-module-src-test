using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class GraphicsSettingsView : BaseSettingsView
	{
		public GraphicsSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void InternalBuild(FlowPanel parent)
		{
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.LocationX);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.LocationY);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.Width);
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.Opacity);
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.BackgroundColorOpacity);
			RenderColorSetting((Panel)(object)parent, base.ModuleSettings.BackgroundColor);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
