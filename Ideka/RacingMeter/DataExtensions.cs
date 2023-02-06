using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	internal static class DataExtensions
	{
		public static string? Describe(this FullRace fullRace)
		{
			return fullRace?.Race.Name;
		}

		public static string? Describe(this FullGhost fullGhost, bool shortVersion = false)
		{
			if (!fullGhost.IsLocal)
			{
				if (!shortVersion)
				{
					return StringExtensions.Format(Strings.GhostDescribeBy, fullGhost.Meta.Time.Formatted(), fullGhost.Meta.AccountName);
				}
				return StringExtensions.Format(Strings.GhostDescribeRemote, fullGhost.Meta.Time.Formatted());
			}
			return StringExtensions.Format(Strings.GhostDescribeLocal, fullGhost.Ghost.Time.Formatted());
		}

		public static string? Describe(this RaceType type)
		{
			return type switch
			{
				RaceType.Custom => Strings.RaceTypeCustom, 
				RaceType.Official => Strings.RaceTypeOfficial, 
				_ => null, 
			};
		}

		public static string? Describe(this RacePointType type)
		{
			return type switch
			{
				RacePointType.Checkpoint => Strings.PointTypeCheckpoint, 
				RacePointType.LoopStart => Strings.PointTypeLoopStart, 
				RacePointType.Guide => Strings.PointTypeGuide, 
				RacePointType.Reset => Strings.PointTypeReset, 
				_ => null, 
			};
		}

		public static FullRace NewRace()
		{
			return FullRace.New(Strings.NewRaceName, GameService.Gw2Mumble.get_CurrentMap().get_Id());
		}
	}
}
