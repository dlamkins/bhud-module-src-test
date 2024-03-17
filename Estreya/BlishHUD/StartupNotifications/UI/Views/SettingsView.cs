using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;

namespace Estreya.BlishHUD.StartupNotifications.UI.Views
{
	public class SettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		public SettingsView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, ModuleSettings moduleSettings)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderIntSetting((Panel)(object)parent, _moduleSettings.Duration);
			RenderEnumSetting<ScreenNotification.NotificationType>((Panel)(object)parent, _moduleSettings.Type);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.AwaitEach);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
