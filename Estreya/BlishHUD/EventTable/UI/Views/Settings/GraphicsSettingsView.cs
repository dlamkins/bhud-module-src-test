using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class GraphicsSettingsView : BaseSettingsView
	{
		public GraphicsSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void BuildView(Panel parent)
		{
			RenderSetting<int>(parent, base.ModuleSettings.LocationX);
			RenderSetting<int>(parent, base.ModuleSettings.LocationY);
			RenderSetting<int>(parent, base.ModuleSettings.Width);
			RenderSetting<int>(parent, base.ModuleSettings.EventHeight);
			RenderEmptyLine(parent);
			RenderSetting<float>(parent, base.ModuleSettings.Opacity);
			RenderSetting<FontSize>(parent, base.ModuleSettings.EventFontSize);
			RenderEmptyLine(parent);
			RenderSetting<float>(parent, base.ModuleSettings.BackgroundColorOpacity);
			RenderColorSetting(parent, base.ModuleSettings.BackgroundColor);
			RenderEmptyLine(parent);
			RenderSetting<int>(parent, base.ModuleSettings.RefreshRate);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
