using System;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class ScoreVisuals
	{
		public static Color AccentColorForGuess(Location guess, Location puzzleLocation)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			ScoreModel band = guess.GetScoreBand(puzzleLocation);
			if (band != null)
			{
				return AccentColorForBand(band);
			}
			if (string.Equals(guess.Score(puzzleLocation), Service.Config.ScoreWrongMap, StringComparison.Ordinal))
			{
				return ScoreModel.ParseHexColor(Service.Config.ScoreWrongMapColor);
			}
			return Color.get_LightGoldenrodYellow();
		}

		public static Color AccentColorForBand(ScoreModel band)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (band.IsRainbow)
			{
				return RainbowColors.ColorFromHue(0.12f);
			}
			return ScoreModel.ParseHexColor(band.Color);
		}
	}
}
