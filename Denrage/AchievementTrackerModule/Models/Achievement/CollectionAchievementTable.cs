using System.Collections.Generic;
using System.Diagnostics;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	[DebuggerDisplay("{Name} Columns: {ColumnNames.Length} Rows: {Entries.Count}")]
	public class CollectionAchievementTable
	{
		public abstract class CollectionAchievementTableEntry
		{
		}

		[DebuggerDisplay("EmptyEntry")]
		public class CollectionAchievementTableEmptyEntry : CollectionAchievementTableEntry
		{
		}

		[DebuggerDisplay("String: {Text}")]
		public class CollectionAchievementTableStringEntry : CollectionAchievementTableEntry
		{
			public string Text { get; set; } = string.Empty;

		}

		[DebuggerDisplay("Map: {ImageLink}")]
		public class CollectionAchievementTableMapEntry : CollectionAchievementTableEntry
		{
			public string ImageLink { get; set; } = string.Empty;

		}

		[DebuggerDisplay("Link: {Link}")]
		public class CollectionAchievementTableLinkEntry : CollectionAchievementTableStringEntry
		{
			public string Link { get; set; } = string.Empty;

		}

		[DebuggerDisplay("Item: {Name}")]
		public class CollectionAchievementTableItemEntry : CollectionAchievementTableEntry
		{
			public string Link { get; set; } = string.Empty;


			public string Name { get; set; } = string.Empty;


			public string ImageUrl { get; set; } = string.Empty;


			public int Id { get; set; }
		}

		[DebuggerDisplay("Coin: {Type} {ItemId}")]
		public class CollectionAchievementTableCoinEntry : CollectionAchievementTableEntry
		{
			public enum TradingPostType
			{
				Buy,
				Sell
			}

			public int ItemId { get; set; }

			public TradingPostType Type { get; set; }
		}

		[DebuggerDisplay("Number: {Number}")]
		public class CollectionAchievementTableNumberEntry : CollectionAchievementTableEntry
		{
			public int Number { get; set; }
		}

		public string Name { get; set; } = string.Empty;


		public int Id { get; set; }

		public string Link { get; set; } = string.Empty;


		public string[] ColumnNames { get; set; }

		public List<List<CollectionAchievementTableEntry>> Entries { get; set; }
	}
}
