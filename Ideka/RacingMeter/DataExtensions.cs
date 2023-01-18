using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	internal static class DataExtensions
	{
		public static string Describe(this FullRace race)
		{
			return race?.Race.Name;
		}

		public static string Describe(this FullGhost ghost, bool shortVersion = false)
		{
			if (ghost != null)
			{
				if (!ghost.IsLocal)
				{
					if (!shortVersion)
					{
						return StringExtensions.Format(Strings.GhostDescribeBy, ghost.Meta.Time.Formatted(), ghost.Meta.AccountName);
					}
					return StringExtensions.Format(Strings.GhostDescribeRemote, ghost.Meta.Time.Formatted());
				}
				return StringExtensions.Format(Strings.GhostDescribeLocal, ghost.Ghost.Time.Formatted());
			}
			return null;
		}

		public static string Describe(this RaceType type)
		{
			return type switch
			{
				RaceType.Custom => Strings.RaceTypeCustom, 
				RaceType.Official => Strings.RaceTypeOfficial, 
				_ => null, 
			};
		}

		public static string Describe(this RacePointType type)
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
