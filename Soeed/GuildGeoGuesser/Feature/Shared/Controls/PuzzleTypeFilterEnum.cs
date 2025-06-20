using System.ComponentModel;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public enum PuzzleTypeFilterEnum
	{
		[Description("Well Liked")]
		LIKED,
		[Description("Not Completed")]
		NON_COMPLETED,
		[Description("Completed")]
		ONLY_COMPLETED,
		[Description("My Puzzles")]
		ONLY_MINE,
		[Description("All")]
		ALL
	}
}
