using Blish_HUD;

namespace FarmingTracker
{
	public static class DebugMode
	{
		public const bool VisualStudioRunningInDebugMode = false;

		public static bool DebugLoggingRequired => GameService.Debug.get_EnableDebugLogging().get_Value();
	}
}
