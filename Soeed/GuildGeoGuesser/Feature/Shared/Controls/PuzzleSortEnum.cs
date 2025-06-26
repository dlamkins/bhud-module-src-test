using System.ComponentModel;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public enum PuzzleSortEnum
	{
		[Description("Newest First")]
		NEWEST_FIRST,
		[Description("Oldest First")]
		OLDEST_FIRST,
		[Description("Title A -> Z")]
		TITLE_ASC,
		[Description("Title Z -> A")]
		TITLE_DESC,
		[Description("Most Played")]
		MOST_PLAYED,
		[Description("Most Upvoted")]
		MOST_LIKED,
		[Description("Author A -> Z")]
		AUTHOR_ASC,
		[Description("Author Z -> A")]
		AUTHOR_DESC
	}
}
