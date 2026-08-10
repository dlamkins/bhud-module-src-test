using System;
using System.Threading.Tasks;
using Neokain.GW2.AllianceManager.Models;

namespace Neokain.GW2.AllianceManager.Services.Spam
{
	public interface ISpamOrchestrator
	{
		bool IsSpamming { get; }

		event EventHandler SpamStateChanged;

		Task<bool> UseAsync(SpamSource source, Guid spamId, SpamContext context, Func<string, Task<bool>> confirmOverride);
	}
}
