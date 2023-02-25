using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.LiveMap.UI.Views
{
	public class SettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Func<string> _getGlobalUrl;

		private readonly Func<string> _getGuildUrl;

		public SettingsView(Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, ModuleSettings moduleSettings, Func<string> getGlobalUrl, Func<string> getGuildUrl, BitmapFont font = null)
			: base(apiManager, iconState, translationState, settingEventState, font)
		{
			_moduleSettings = moduleSettings;
			_getGlobalUrl = getGlobalUrl;
			_getGuildUrl = getGuildUrl;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideCommander);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.StreamerModeEnabled);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.FollowOnMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.SendGroupInformation);
			RenderEmptyLine((Panel)(object)parent);
			RenderButton((Panel)(object)parent, "Open Global Map", delegate
			{
				Process.Start(_getGlobalUrl());
			});
			RenderButton((Panel)(object)parent, "Open Guild Map", delegate
			{
				Process.Start(_getGuildUrl());
			}, () => string.IsNullOrWhiteSpace(_getGuildUrl()));
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
