namespace Taskmaster.Services
{
	public enum TaskStoreLoadOutcome
	{
		LoadedPrimary,
		LoadedBackup,
		StartedEmpty,
		StartedEmptyAfterCorruption,
		VersionTooNew
	}
}
