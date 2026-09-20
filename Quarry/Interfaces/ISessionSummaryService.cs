using System;
using Quarry.Models;

namespace Quarry.Interfaces
{
	public interface ISessionSummaryService
	{
		SessionSummary Summary { get; }

		event Action Changed;

		event Action<int> AchievementCompleted;

		string GetSummaryLine();
	}
}
