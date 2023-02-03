using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.LiveMap.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Flurl.Util;
using Humanizer;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.LiveMap.UI.Views
{
	public class SettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Func<string> _getGuildId;

		private readonly Func<double> _getPosX;

		private readonly Func<double> _getPosY;

		private readonly Func<string> _getGlobalUrl;

		private readonly Func<string> _getGuildUrl;

		public SettingsView(Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, ModuleSettings moduleSettings, Func<string> getGuildId, Func<double> getPosX, Func<double> getPosY, Func<string> getGlobalUrl, Func<string> getGuildUrl, BitmapFont font = null)
			: base(apiManager, iconState, translationState, font)
		{
			_moduleSettings = moduleSettings;
			_getGuildId = getGuildId;
			_getPosX = getPosX;
			_getPosY = getPosY;
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
				Process.Start(FormatUrlWithPositions(_getGlobalUrl()));
			});
			RenderButton(parent, "Open Guild Map", delegate
			{
				string url = _getGuildUrl().FormatWith(_getGuildId());
				Process.Start(FormatUrlWithPositions(url));
			}, () => string.IsNullOrWhiteSpace(_getGuildId()));
		}

		private string FormatUrlWithPositions(string url)
		{
			return url + "?posX=" + _getPosX().ToInvariantString() + "&posY=" + _getPosY().ToInvariantString() + "&zoom=6";
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
