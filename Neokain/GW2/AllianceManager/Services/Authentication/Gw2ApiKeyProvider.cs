using System;
using Blish_HUD.Settings;
using Neokain.GW2.WebClient.Models;

namespace Neokain.GW2.AllianceManager.Services.Authentication
{
	internal class Gw2ApiKeyProvider
	{
		private readonly SettingEntry<string> _customApiKey;

		public Gw2ApiKeyProvider(SettingEntry<string> customApiKey)
		{
			_customApiKey = customApiKey ?? throw new ArgumentNullException("customApiKey");
		}

		public bool HasApiKey()
		{
			return CustomApiKeyFormat.IsCustomApiKey(_customApiKey.get_Value());
		}

		public string GetApiKey()
		{
			return _customApiKey.get_Value()?.Trim() ?? string.Empty;
		}
	}
}
