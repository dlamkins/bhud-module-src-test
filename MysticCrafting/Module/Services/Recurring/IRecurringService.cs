using System;
using System.Threading.Tasks;

namespace MysticCrafting.Module.Services.Recurring
{
	public interface IRecurringService
	{
		string Name { get; }

		bool Loaded { get; set; }

		bool Loading { get; set; }

		DateTime LastLoaded { get; set; }

		DateTime LastFailed { get; set; }

		event EventHandler<EventArgs> LoadingStarted;

		event EventHandler<EventArgs> LoadingFinished;

		Task<string> LoadAsync();

		Task StartTimedLoadingAsync(int interval);

		void StopTimedLoading();
	}
}
