using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;

namespace Nekres.Stream_Out.Core.Services
{
	internal class ClientService : IExportService, IDisposable
	{
		private const string SERVER_ADDRESS = "server_address.txt";

		private string _prevServerAddress;

		private Logger Logger => StreamOutModule.Logger;

		private DirectoriesManager DirectoriesManager => StreamOutModule.ModuleInstance?.DirectoriesManager;

		private SettingEntry<bool> OnlyLastDigitSettingEntry => StreamOutModule.ModuleInstance?.OnlyLastDigitSettingEntry;

		public ClientService()
		{
			_prevServerAddress = string.Empty;
		}

		public async Task Update()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && !_prevServerAddress.Equals(GameService.Gw2Mumble.get_Info().get_ServerAddress(), StringComparison.InvariantCultureIgnoreCase))
			{
				_prevServerAddress = GameService.Gw2Mumble.get_Info().get_ServerAddress();
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/server_address.txt", string.IsNullOrEmpty(GameService.Gw2Mumble.get_Info().get_ServerAddress()) ? string.Empty : (OnlyLastDigitSettingEntry.get_Value() ? ("*" + GameService.Gw2Mumble.get_Info().get_ServerAddress().Substring(GameService.Gw2Mumble.get_Info().get_ServerAddress().LastIndexOf('.'))) : GameService.Gw2Mumble.get_Info().get_ServerAddress()));
			}
		}

		public async Task Initialize()
		{
		}

		public async Task ResetDaily()
		{
		}

		public void Dispose()
		{
		}
	}
}
