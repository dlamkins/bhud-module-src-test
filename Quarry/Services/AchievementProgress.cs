using System.Collections.Generic;
using System.Linq;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public static class AchievementProgress
	{
		private const int MaxRemainingNamesShown = 3;

		private const int MaxNearestShown = 8;

		public static (int Current, int Max, string Text, string RemainingText, double Fraction) Get(IAchievementService achievementService, AchievementTableEntry achievement, IReadOnlyList<RemainingObjective> nearest = null)
		{
			achievementService.PlayerAchievementsById.TryGetValue(achievement.Id, out var playerAchievement);
			int current;
			int max;
			string text;
			if (playerAchievement == null)
			{
				current = 0;
				max = 0;
				text = "Not started";
			}
			else
			{
				current = playerAchievement.get_Current();
				max = playerAchievement.get_Max();
				text = ((max > 0) ? $"{current} / {max}" : null);
			}
			double fraction = ((max > 0) ? ((double)current / (double)max) : 0.0);
			string remainingText = ((nearest != null) ? FormatNearestText(nearest) : GetRemainingCollectionText(achievementService, achievement));
			return (current, max, text, remainingText, fraction);
		}

		public static string FormatNearestText(IReadOnlyList<RemainingObjective> nearest)
		{
			if (nearest.Count == 0)
			{
				return null;
			}
			IEnumerable<string> lines = from o in nearest.Take(8)
				select (o.AreaHint == null) ? string.Format("{0} — {1:F0} m{2}", o.Name, o.DistanceMetres, o.GroundDistanceOnly ? " (ground)" : string.Empty) : $"{o.Name} — {o.DistanceMetres:F0} m (ground). Distance to the centre of {o.AreaHint}; the objective is somewhere in that area.";
			string text = string.Join("\n", lines);
			if (nearest.Count > 8)
			{
				text += $"\n(+{nearest.Count - 8} more)";
			}
			return text;
		}

		private static string GetRemainingCollectionText(IAchievementService achievementService, AchievementTableEntry achievement)
		{
			CollectionDescription collectionDescription = achievement.Description as CollectionDescription;
			if (collectionDescription == null || collectionDescription.EntryList.Count == 0)
			{
				return null;
			}
			List<CollectionDescriptionEntry> entries = collectionDescription.EntryList;
			List<string> remainingNames = new List<string>();
			int remainingCount = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (!achievementService.HasFinishedAchievementBit(achievement.Id, i))
				{
					remainingCount++;
					if (remainingNames.Count < 3)
					{
						remainingNames.Add(entries[i].DisplayName);
					}
				}
			}
			if (remainingCount == 0)
			{
				return null;
			}
			string names = string.Join(", ", remainingNames);
			if (remainingCount > remainingNames.Count)
			{
				names += $" (+{remainingCount - remainingNames.Count} more)";
			}
			return $"{remainingCount} of {entries.Count} remaining: {names}";
		}
	}
}
