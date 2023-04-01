using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;

namespace Nekres.Stream_Out.Core.Services
{
	internal class ClientService : ExportService
	{
		private const string SERVER_ADDRESS = "server_address.txt";

		private string _prevServerAddress;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private SettingEntry<bool> OnlyLastDigitSettingEntry => StreamOutModule.Instance?.OnlyLastDigitSettingEntry;

		public ClientService(SettingCollection settings)
			: base(settings)
		{
			_prevServerAddress = string.Empty;
		}

		protected override async Task Update()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && !_prevServerAddress.Equals(GameService.Gw2Mumble.get_Info().get_ServerAddress(), StringComparison.InvariantCultureIgnoreCase))
			{
				_prevServerAddress = GameService.Gw2Mumble.get_Info().get_ServerAddress();
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/server_address.txt", string.IsNullOrEmpty(GameService.Gw2Mumble.get_Info().get_ServerAddress()) ? string.Empty : (OnlyLastDigitSettingEntry.get_Value() ? ("*" + GameService.Gw2Mumble.get_Info().get_ServerAddress().Substring(GameService.Gw2Mumble.get_Info().get_ServerAddress().LastIndexOf('.'))) : GameService.Gw2Mumble.get_Info().get_ServerAddress()));
			}
		}

		public override async Task Clear()
		{
			await FileUtil.DeleteAsync(Path.Combine(DirectoriesManager.GetFullDirectoryPath("stream_out"), "server_address.txt"));
		}

		public override void Dispose()
		{
		}
	}
}
