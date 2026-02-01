using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EventTimeTableSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		public EventTimeTableSettingsView(ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			return true;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.ShowEventTimeTableWindowKeybinding);
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.CloseEventTimeTableWithEsc);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowEventTimeTableSettingButtonInExternalWindow);
		}
	}
}
