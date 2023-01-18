using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	internal class UserData
	{
		private static readonly TimeSpan TokenDuration = TimeSpan.FromHours(1.0);

		private static readonly TimeSpan TokenRefreshSlack = TimeSpan.FromMinutes(5.0);

		public Account Account { get; private set; }

		public string AccessToken { get; private set; }

		public Gw2Jwt Token { get; private set; }

		public string AccountId
		{
			get
			{
				Account account = Account;
				if (account == null)
				{
					return null;
				}
				return account.get_Id().ToString();
			}
		}

		public async Task Refresh(CancellationToken ct)
		{
			if (Token == null || !(Token.Payload.Expires - DateTime.UtcNow > TokenRefreshSlack))
			{
				Account = null;
				AccessToken = null;
				Token = null;
				Account = await ((IBlobClient<Account>)(object)RacingModule.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(ct);
				AccessToken = (await ((IBlobClient<CreateSubtoken>)(object)RacingModule.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_CreateSubtoken()
					.Expires(DateTimeOffset.UtcNow + TokenDuration)
					.WithPermissions((IEnumerable<string>)new string[1] { "account" })).GetAsync(ct)).get_Subtoken();
				Token = new Gw2Jwt(AccessToken);
				if (Account == null || AccessToken == null || Token == null)
				{
					throw FriendlyError.Create(new Exception(Strings.ExceptionApiAccount));
				}
			}
		}
	}
}
