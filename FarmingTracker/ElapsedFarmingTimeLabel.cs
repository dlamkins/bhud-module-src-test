using System;
using Blish_HUD;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class ElapsedFarmingTimeLabel : Label
	{
		private TimeSpan _oldFarmingTime = TimeSpan.Zero;

		private readonly Services _services;

		public ElapsedFarmingTimeLabel(Services services, Container parent)
			: this()
		{
			_services = services;
			((Label)this).set_Text(CreateFarmingTimeText("-:--:--"));
			((Label)this).set_Font(services.FontService.Fonts[(FontSize)14]);
			((Label)this).set_AutoSizeHeight(true);
			((Label)this).set_AutoSizeWidth(true);
			((Control)this).set_Parent(parent);
		}

		public void UpdateTimeEverySecond()
		{
			TimeSpan farmingTime = _services.FarmingDuration.Elapsed;
			if (farmingTime >= _oldFarmingTime + TimeSpan.FromSeconds(1.0))
			{
				_oldFarmingTime = farmingTime;
				UpdateLabelText(farmingTime);
			}
		}

		public void RestartTime()
		{
			_services.FarmingDuration.Restart();
			_oldFarmingTime = TimeSpan.Zero;
			UpdateLabelText(TimeSpan.Zero);
		}

		private void UpdateLabelText(TimeSpan farmingTime)
		{
			string formattedFarmingTime = CreateFormattedTimeText(farmingTime);
			((Label)this).set_Text(CreateFarmingTimeText(formattedFarmingTime));
		}

		private static string CreateFormattedTimeText(TimeSpan timeSpan)
		{
			if (timeSpan >= TimeSpan.FromHours(1.0))
			{
				return $"{timeSpan:h' hr  'm' min  'ss' sec'}";
			}
			if (timeSpan >= TimeSpan.FromMinutes(1.0))
			{
				return $"{timeSpan:m' min  'ss' sec'}";
			}
			return $"{timeSpan:ss' sec'}";
		}

		private static string CreateFarmingTimeText(string timeString)
		{
			return "Farming for " + timeString;
		}
	}
}
