using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class WalletService : ExportService
	{
		private const string WALLET_COINS = "wallet_coins.png";

		private const string WALLET_KARMA = "wallet_karma.png";

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		public WalletService(SettingCollection settings)
			: base(settings)
		{
		}

		protected override async Task Update()
		{
			await UpdateWallet();
		}

		public override async Task Initialize()
		{
			await Gw2Util.GenerateCoinsImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_coins.png", 10000000, overwrite: false);
			await Gw2Util.GenerateKarmaImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_karma.png", 10000000, overwrite: false);
		}

		private async Task UpdateWallet()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)10
			}))
			{
				return;
			}
			IApiV2ObjectList<AccountCurrency> wallet = await TaskUtil.RetryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountCurrency>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Wallet()).GetAsync(default(CancellationToken))).Unwrap();
			if (wallet != null)
			{
				AccountCurrency obj = ((IEnumerable<AccountCurrency>)wallet).FirstOrDefault((AccountCurrency x) => x.get_Id() == 1);
				int coins = ((obj != null) ? obj.get_Value() : 0);
				await Gw2Util.GenerateCoinsImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_coins.png", coins);
				AccountCurrency obj2 = ((IEnumerable<AccountCurrency>)wallet).FirstOrDefault((AccountCurrency x) => x.get_Id() == 2);
				int karma = ((obj2 != null) ? obj2.get_Value() : 0);
				await Gw2Util.GenerateKarmaImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_karma.png", karma);
			}
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "wallet_coins.png"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "wallet_karma.png"));
		}

		public override void Dispose()
		{
		}
	}
}
