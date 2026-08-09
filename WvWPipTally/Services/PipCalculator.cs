using System;
using System.Collections.Generic;
using System.Linq;
using WvWPipTally.Models;

namespace WvWPipTally.Services
{
	public static class PipCalculator
	{
		public const int TotalPips = 1450;

		public const int TotalTickets = 365;

		public const int TickMinutes = 5;

		public static readonly ChestSegment[] ChestSegments = new ChestSegment[35]
		{
			new ChestSegment("wood-0", "Wood", 0, 25, 3),
			new ChestSegment("wood-1", "Wood", 1, 25, 3),
			new ChestSegment("wood-2", "Wood", 2, 25, 3),
			new ChestSegment("wood-3", "Wood", 3, 25, 8),
			new ChestSegment("bronze-0", "Bronze", 0, 30, 5),
			new ChestSegment("bronze-1", "Bronze", 1, 30, 5),
			new ChestSegment("bronze-2", "Bronze", 2, 30, 5),
			new ChestSegment("bronze-3", "Bronze", 3, 30, 10),
			new ChestSegment("silver-0", "Silver", 0, 35, 7),
			new ChestSegment("silver-1", "Silver", 1, 35, 7),
			new ChestSegment("silver-2", "Silver", 2, 35, 7),
			new ChestSegment("silver-3", "Silver", 3, 35, 7),
			new ChestSegment("silver-4", "Silver", 4, 35, 12),
			new ChestSegment("gold-0", "Gold", 0, 40, 9),
			new ChestSegment("gold-1", "Gold", 1, 40, 9),
			new ChestSegment("gold-2", "Gold", 2, 40, 9),
			new ChestSegment("gold-3", "Gold", 3, 40, 9),
			new ChestSegment("gold-4", "Gold", 4, 40, 14),
			new ChestSegment("platinum-0", "Platinum", 0, 45, 11),
			new ChestSegment("platinum-1", "Platinum", 1, 45, 11),
			new ChestSegment("platinum-2", "Platinum", 2, 45, 11),
			new ChestSegment("platinum-3", "Platinum", 3, 45, 11),
			new ChestSegment("platinum-4", "Platinum", 4, 45, 16),
			new ChestSegment("mithril-0", "Mithril", 0, 50, 13),
			new ChestSegment("mithril-1", "Mithril", 1, 50, 13),
			new ChestSegment("mithril-2", "Mithril", 2, 50, 13),
			new ChestSegment("mithril-3", "Mithril", 3, 50, 13),
			new ChestSegment("mithril-4", "Mithril", 4, 50, 13),
			new ChestSegment("mithril-5", "Mithril", 5, 50, 18),
			new ChestSegment("diamond-0", "Diamond", 0, 55, 14),
			new ChestSegment("diamond-1", "Diamond", 1, 55, 14),
			new ChestSegment("diamond-2", "Diamond", 2, 55, 14),
			new ChestSegment("diamond-3", "Diamond", 3, 55, 14),
			new ChestSegment("diamond-4", "Diamond", 4, 55, 14),
			new ChestSegment("diamond-5", "Diamond", 5, 55, 20)
		};

		private static readonly Dictionary<Placement, int> PlacementPips = new Dictionary<Placement, int>
		{
			{
				Placement.First,
				6
			},
			{
				Placement.Second,
				5
			},
			{
				Placement.Third,
				4
			}
		};

		private static readonly Dictionary<RankTier, int> RankPips = new Dictionary<RankTier, int>
		{
			{
				RankTier.Wood,
				1
			},
			{
				RankTier.Bronze,
				2
			},
			{
				RankTier.Silver,
				3
			},
			{
				RankTier.Gold,
				4
			},
			{
				RankTier.Platinum,
				5
			},
			{
				RankTier.Mithril,
				6
			},
			{
				RankTier.Diamond,
				7
			},
			{
				RankTier.Max,
				8
			}
		};

		public static RankTier RankTierFromWvWRank(int rank)
		{
			if (rank >= 10000)
			{
				return RankTier.Max;
			}
			if (rank >= 6445)
			{
				return RankTier.Diamond;
			}
			if (rank >= 4095)
			{
				return RankTier.Mithril;
			}
			if (rank >= 2545)
			{
				return RankTier.Platinum;
			}
			if (rank >= 1395)
			{
				return RankTier.Gold;
			}
			if (rank >= 620)
			{
				return RankTier.Silver;
			}
			if (rank >= 150)
			{
				return RankTier.Bronze;
			}
			return RankTier.Wood;
		}

		public static int FindSegmentIndex(string division, int rewardNumberOneBased)
		{
			if (string.IsNullOrWhiteSpace(division) || rewardNumberOneBased < 1)
			{
				return -1;
			}
			for (int i = 0; i < ChestSegments.Length; i++)
			{
				ChestSegment seg = ChestSegments[i];
				if (string.Equals(seg.Division, division, StringComparison.OrdinalIgnoreCase) && seg.SegmentIndex == rewardNumberOneBased - 1)
				{
					return i;
				}
			}
			return -1;
		}

		public static ChestSegment GetSegment(int index)
		{
			if (index < 0 || index >= ChestSegments.Length)
			{
				return null;
			}
			return ChestSegments[index];
		}

		public static int CalcPipsPerTick(PipInputs input)
		{
			if (input == null)
			{
				return 0;
			}
			if (input.ScannedPipsPerTick.HasValue && input.ScannedPipsPerTick.Value > 0)
			{
				return input.ScannedPipsPerTick.Value;
			}
			int ppt = PlacementPips[input.Placement];
			ppt += ((!RankPips.TryGetValue(input.Rank, out var rankPips)) ? 1 : rankPips);
			if (input.Commitment)
			{
				ppt++;
			}
			if (input.Commander)
			{
				ppt++;
			}
			if (input.PublicCommander)
			{
				ppt += 3;
			}
			return ppt;
		}

		public static (int RemainingPips, int RemainingTickets) CalcRemaining(int startingSegmentIndex)
		{
			int num = Math.Max(0, startingSegmentIndex);
			int remainingPips = 0;
			int remainingTickets = 0;
			for (int i = num; i < ChestSegments.Length; i++)
			{
				remainingPips += ChestSegments[i].Pips;
				remainingTickets += ChestSegments[i].Tickets;
			}
			return (remainingPips, remainingTickets);
		}

		public static PipResults CalcResults(PipInputs input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			int pipsPerTick = CalcPipsPerTick(input);
			(int RemainingPips, int RemainingTickets) tuple = CalcRemaining(input.StartingSegmentIndex);
			int remainingPips = tuple.RemainingPips;
			int remainingTickets = tuple.RemainingTickets;
			int finishedPips = 1450 - remainingPips;
			int earnedTickets = 365 - remainingTickets;
			int ticksRemaining = ((remainingPips != 0 && pipsPerTick > 0) ? ((int)Math.Ceiling((double)remainingPips / (double)pipsPerTick)) : 0);
			int totalMinutes = ticksRemaining * 5;
			return new PipResults
			{
				PipsPerTick = pipsPerTick,
				RemainingPips = remainingPips,
				RemainingTickets = remainingTickets,
				FinishedPips = finishedPips,
				EarnedTickets = earnedTickets,
				TotalPips = 1450,
				TotalTickets = 365,
				PipPercent = (int)Math.Round((double)finishedPips / 1450.0 * 100.0),
				TicketPercent = (int)Math.Round((double)earnedTickets / 365.0 * 100.0),
				TicksRemaining = ticksRemaining,
				HoursRemaining = totalMinutes / 60,
				MinutesRemaining = totalMinutes % 60
			};
		}

		public static string FormatSegmentLabel(int segmentIndex)
		{
			ChestSegment seg = GetSegment(segmentIndex);
			if (seg == null)
			{
				return "Unknown";
			}
			int rewardsInDivision = ChestSegments.Count((ChestSegment s) => string.Equals(s.Division, seg.Division, StringComparison.OrdinalIgnoreCase));
			return $"{seg.Division} - Reward {seg.SegmentIndex + 1}/{rewardsInDivision}";
		}
	}
}
