using System.Diagnostics;
using Denrage.AchievementTrackerModule.Interfaces;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	[DebuggerDisplay("{DisplayName}")]
	public class TableDescriptionEntry : ILinkEntry
	{
		public string DisplayName { get; set; } = string.Empty;


		public string Link { get; set; } = string.Empty;

	}
}
