using System;
using Blish_HUD;
using Blish_HUD.Gw2WebApi;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Caching;
using Gw2Sharp.WebApi.Http;
using Gw2Sharp.WebApi.Middleware;

namespace Kenedia.Modules.Core.Models
{
	public class CustomApiManager
	{
		private readonly Connection _internalConnection;

		private readonly IGw2WebApiClient _internalClient;

		public IConnection Connection => (IConnection)(object)_internalConnection;

		public CustomApiManager(string accessToken, TokenComplianceMiddleware tokenComplianceMiddle, ICacheMethod webApiCache, ICacheMethod renderCache = null, TimeSpan? renderCacheDuration = null)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			string ua = $"kenedia/{Program.get_OverlayVersion()}";
			_internalConnection = new Connection(accessToken, GameService.Overlay.get_UserLocale().get_Value(), webApiCache, renderCache, (TimeSpan?)(renderCacheDuration ?? TimeSpan.MaxValue), ua, (IHttpClient)null);
			_internalConnection.get_Middleware().Add((IWebApiMiddleware)(object)tokenComplianceMiddle);
			_internalClient = new Gw2Client((IConnection)(object)_internalConnection).get_WebApi();
			SetupListeners();
		}

		private void SetupListeners()
		{
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocaleOnSettingChanged);
		}

		private void UserLocaleOnSettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_internalConnection.set_Locale(e.get_NewValue());
		}

		public bool SetApiKey(string apiKey)
		{
			if (string.Equals(_internalConnection.get_AccessToken(), apiKey))
			{
				return false;
			}
			_internalConnection.set_AccessToken(apiKey);
			return true;
		}
	}
}
