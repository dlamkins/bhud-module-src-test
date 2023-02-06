using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.LiveMap.Models;
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

		public SettingsView(Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, ModuleSettings moduleSettings, Func<string> getGlobalUrl, Func<string> getGuildUrl, BitmapFont font = null)
			: base(apiManager, iconState, translationState, font)
		{
			_moduleSettings = moduleSettings;
			_getGlobalUrl = getGlobalUrl;
			_getGuildUrl = getGuildUrl;
		}

		protected override void BuildView(Panel parent)
		{
			RenderEnumSetting<PublishType>(parent, _moduleSettings.PublishType);
			RenderEnumSetting<PlayerFacingType>(parent, _moduleSettings.PlayerFacingType);
			RenderBoolSetting(parent, _moduleSettings.HideCommander);
			RenderBoolSetting(parent, _moduleSettings.StreamerModeEnabled);
			RenderEmptyLine(parent);
			RenderButton(parent, "Open Global Map", delegate
			{
				Process.Start(_getGlobalUrl());
			});
			RenderButton(parent, "Open Guild Map", delegate
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
