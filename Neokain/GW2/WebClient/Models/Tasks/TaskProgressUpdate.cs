namespace Neokain.GW2.WebClient.Models.Tasks
{
	internal class TaskProgressUpdate
	{
		public int Progress { get; set; }

		public string? CurrentAction { get; set; }

		public string? CurrentPhase { get; set; }

		public int? TotalSteps { get; set; }

		public int? CompletedSteps { get; set; }

		public TaskProgressUpdate(int progress, string? currentAction = null, string? currentPhase = null, int? totalSteps = null, int? completedSteps = null)
		{
			Progress = progress;
			CurrentAction = currentAction;
			CurrentPhase = currentPhase;
			TotalSteps = totalSteps;
			CompletedSteps = completedSteps;
		}
	}
}
