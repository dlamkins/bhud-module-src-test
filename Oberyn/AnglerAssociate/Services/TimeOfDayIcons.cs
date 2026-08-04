using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public static class TimeOfDayIcons
	{
		private static AsyncTexture2D _favorDay;

		private static AsyncTexture2D _favorNight;

		private static AsyncTexture2D _favorAny;

		private static AsyncTexture2D _duskDawn;

		private static bool _loaded;

		private static void EnsureLoaded(ContentsManager contentsManager)
		{
			if (!_loaded)
			{
				_favorDay = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/favor_day.png"));
				_favorNight = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/favor_night.png"));
				_favorAny = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/favor_any.png"));
				_duskDawn = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/dusk_dawn.png"));
				_loaded = true;
			}
		}

		public static List<AsyncTexture2D> GetTextures(ContentsManager contentsManager, Fish fish)
		{
			EnsureLoaded(contentsManager);
			AsyncTexture2D baseIcon = GetBaseIcon(fish);
			AsyncTexture2D biasIcon = GetBiasIcon(fish);
			List<AsyncTexture2D> result = new List<AsyncTexture2D> { baseIcon };
			if (biasIcon != null && biasIcon != baseIcon)
			{
				result.Add(biasIcon);
			}
			return result;
		}

		private static AsyncTexture2D GetBaseIcon(Fish fish)
		{
			if (fish.TimeOfDay2.HasValue)
			{
				return _duskDawn;
			}
			if (fish.TimeOfDay == TimeOfDay.Day)
			{
				return _favorDay;
			}
			if (fish.TimeOfDay == TimeOfDay.Night)
			{
				return _favorNight;
			}
			return _favorAny;
		}

		private static AsyncTexture2D GetBiasIcon(Fish fish)
		{
			if (fish.HigherChance.GetValueOrDefault() == TimeOfDay.Day)
			{
				return _favorDay;
			}
			if (fish.HigherChance.GetValueOrDefault() == TimeOfDay.Night)
			{
				return _favorNight;
			}
			return null;
		}
	}
}
