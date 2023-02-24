using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class GeneralSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		public GeneralSettingsView(ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, settingEventState, font)
		{
			_moduleSettings = moduleSettings;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisible);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisibleHotkey);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RegisterCornerIcon);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconLeftClickAction);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconRightClickAction);
			RenderEmptyLine((Panel)(object)parent);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.MapKeybinding);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideOnMissingMumbleTicks);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideOnOpenMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideInCombat);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideInPvE_OpenWorld);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideInPvE_Competetive);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideInWvW);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideInPvP);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
