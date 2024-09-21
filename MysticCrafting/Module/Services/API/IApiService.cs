using System;
using System.Threading.Tasks;

namespace MysticCrafting.Module.Services.API
{
	public interface IApiService
	{
		bool CanReloadManually { get; }

		string Name { get; }

		bool Loaded { get; set; }

		bool Loading { get; set; }

		int ExecutionIntervalMinutes { get; set; }

		DateTime LastLoaded { get; set; }

		DateTime LastFailed { get; set; }

		event EventHandler<EventArgs> LoadingStarted;

		event EventHandler<EventArgs> LoadingFinished;

		Task StartTimedLoadingAsync(int minutes);

		Task LoadSafeAsync();

		Task<string> LoadAsync();

		bool LastLoadFailed();
	}
}
