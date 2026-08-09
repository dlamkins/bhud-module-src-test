using System;
using System.Linq;
using System.Text.RegularExpressions;
using WvWPipTally.Models;

namespace WvWPipTally.Services
{
	public static class RewardTrackParser
	{
		private static readonly string[] Divisions = new string[7] { "Wood", "Bronze", "Silver", "Gold", "Platinum", "Mithril", "Diamond" };

		private static readonly Regex NextRewardClean = new Regex("(?:Next\\s*:?\\s*)?Reward\\s*(\\d+)\\s*(?:of|ol|0f|/)\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex NextRewardDirty = new Regex("(?:Next|text)\\s*:?\\s*(?:R[e]?[vw]?[aeo]?r[dq]|Retard|Rewar)\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex TickPredictionClean = new Regex("Tick\\s*Pred(?:i|l)ction\\s*\\(?\\s*(\\d+)\\s*\\)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex TickPredictionDirty = new Regex("(?:All\\s*Rewards|Current Match Rewards).{0,40}?\\((\\d{1,2})\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex TwinTickParens = new Regex("\\((\\d{1,2})\\)\\s*\\((\\d{1,2})\\)", RegexOptions.Compiled);

		private static readonly Regex PreviousTickRegex = new Regex("Previous\\s*Tick\\s*\\(?\\s*(\\d+)\\s*\\)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static ScanResult Parse(string ocrText)
		{
			ScanResult result = new ScanResult
			{
				RawText = (ocrText ?? string.Empty),
				Success = false,
				Message = "Match Overview not open. Press B, then Scan."
			};
			if (string.IsNullOrWhiteSpace(ocrText))
			{
				result.Message = "Match Overview not open. Press B, then Scan.";
				return result;
			}
			string text = ocrText.Replace('\r', ' ').Replace('\n', ' ');
			bool panelVisible = LooksLikeMatchOverview(text);
			result.Division = FindDivision(text);
			ParseReward(text, result);
			ParseTicks(text, result);
			if (result.Division == null || !result.RewardNumber.HasValue)
			{
				result.Message = (panelVisible ? "Couldn't read chest / reward. Keep Match Overview open and try again." : "Match Overview not open. Press B, then Scan.");
				return result;
			}
			if (!panelVisible)
			{
				result.Message = "Match Overview not open. Press B, then Scan.";
				result.StartingSegmentIndex = null;
				result.Division = null;
				result.RewardNumber = null;
				return result;
			}
			if (!result.RewardsInDivision.HasValue)
			{
				result.RewardsInDivision = PipCalculator.ChestSegments.Count((ChestSegment s) => string.Equals(s.Division, result.Division, StringComparison.OrdinalIgnoreCase));
			}
			int segmentIndex = PipCalculator.FindSegmentIndex(result.Division, result.RewardNumber.Value);
			if (segmentIndex < 0)
			{
				result.Message = $"Unrecognized reward: {result.Division} reward {result.RewardNumber}.";
				return result;
			}
			result.StartingSegmentIndex = segmentIndex;
			result.Success = true;
			result.Message = "Scanned " + PipCalculator.FormatSegmentLabel(segmentIndex) + (result.TickPrediction.HasValue ? $" · {result.TickPrediction.Value} pips/tick" : string.Empty);
			return result;
		}

		private static void ParseReward(string text, ScanResult result)
		{
			Match clean = NextRewardClean.Match(text);
			if (clean.Success)
			{
				result.RewardNumber = int.Parse(clean.Groups[1].Value);
				result.RewardsInDivision = int.Parse(clean.Groups[2].Value);
				return;
			}
			Match dirty = NextRewardDirty.Match(text);
			if (dirty.Success)
			{
				int j = int.Parse(dirty.Groups[1].Value);
				if (j >= 1 && j <= 8)
				{
					result.RewardNumber = j;
					return;
				}
			}
			if (result.Division == null)
			{
				return;
			}
			Match near = Regex.Match(text, "\\b" + Regex.Escape(result.Division) + "\\b.{0,48}?(\\d)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			if (near.Success)
			{
				int i = int.Parse(near.Groups[1].Value);
				if (i >= 1 && i <= 8)
				{
					result.RewardNumber = i;
				}
			}
		}

		private static void ParseTicks(string text, ScanResult result)
		{
			Match tick = TickPredictionClean.Match(text);
			if (tick.Success)
			{
				result.TickPrediction = int.Parse(tick.Groups[1].Value);
			}
			else
			{
				Match dirty = TickPredictionDirty.Match(text);
				if (dirty.Success)
				{
					int i = int.Parse(dirty.Groups[1].Value);
					if (i >= 1 && i <= 30)
					{
						result.TickPrediction = i;
					}
				}
				else
				{
					Match twin = TwinTickParens.Match(text);
					if (twin.Success)
					{
						int a = int.Parse(twin.Groups[1].Value);
						int b = int.Parse(twin.Groups[2].Value);
						if (a >= 1 && a <= 30)
						{
							result.TickPrediction = a;
						}
						if (b >= 1 && b <= 30)
						{
							result.PreviousTick = b;
						}
					}
				}
			}
			Match prev = PreviousTickRegex.Match(text);
			if (prev.Success)
			{
				result.PreviousTick = int.Parse(prev.Groups[1].Value);
			}
		}

		private static bool LooksLikeMatchOverview(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (!Regex.IsMatch(text, "Current\\s*Match\\s*Rewards", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "Tick\\s*Pred", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "Match\\s*Overview", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "World\\s*vs\\.?\\s*World", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "Skirmish\\s*Details", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "Next\\s*:?\\s*Reward", RegexOptions.IgnoreCase))
			{
				return Regex.IsMatch(text, "(?:Next|text)\\s*:?\\s*(?:R[e]?[vw]?[aeo]?r[dq]|Retard|Rewar)\\s*\\d", RegexOptions.IgnoreCase);
			}
			return true;
		}

		private static string FindDivision(string text)
		{
			string[] divisions = Divisions;
			foreach (string division in divisions)
			{
				if (Regex.IsMatch(text, "\\b" + Regex.Escape(division) + "\\b", RegexOptions.IgnoreCase))
				{
					return division;
				}
			}
			if (Regex.IsMatch(text, "\\bm[il1]thr[il1]l\\b", RegexOptions.IgnoreCase))
			{
				return "Mithril";
			}
			if (Regex.IsMatch(text, "\\bplat[il1]num\\b", RegexOptions.IgnoreCase))
			{
				return "Platinum";
			}
			if (Regex.IsMatch(text, "\\bd[il1]amond\\b", RegexOptions.IgnoreCase))
			{
				return "Diamond";
			}
			if (Regex.IsMatch(text, "\\bbronze\\b", RegexOptions.IgnoreCase))
			{
				return "Bronze";
			}
			if (Regex.IsMatch(text, "\\bs[il1]lver\\b", RegexOptions.IgnoreCase))
			{
				return "Silver";
			}
			return null;
		}
	}
}
