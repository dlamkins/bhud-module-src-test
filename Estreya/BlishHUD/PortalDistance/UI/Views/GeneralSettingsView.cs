using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;

namespace Estreya.BlishHUD.PortalDistance.UI.Views
{
	public class GeneralSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		public GeneralSettingsView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, ModuleSettings moduleSettings)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RegisterCornerIcon);
			RenderEmptyLine((Panel)(object)parent);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.ManualKeyBinding);
			RenderEmptyLine((Panel)(object)parent, 10);
			((Control)new FormattedLabelBuilder().AutoSizeHeight().SetWidth(((Container)parent).get_ContentRegion().Width).CreatePart("This should not be the same key as your portal in-game keybind. It will prevent you from pressing it.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart(" \n ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Allowing the same key would result in desyncs over time as tracking is not 100% accurate.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.UseArcDPS);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
