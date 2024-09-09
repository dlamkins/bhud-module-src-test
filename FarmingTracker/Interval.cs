using System;
using Blish_HUD;

namespace FarmingTracker
{
	public class Interval
	{
		private readonly double _intervalInMilliseconds;

		private double _intervalEndInMilliseconds;

		public Interval(TimeSpan intervalTimeSpan, bool firstIntervalEnded = false)
		{
			_intervalInMilliseconds = intervalTimeSpan.TotalMilliseconds;
			if (firstIntervalEnded)
			{
				_intervalEndInMilliseconds = 0.0;
			}
			else
			{
				UpdateIntervalEnd();
			}
		}

		public bool HasEnded()
		{
			if (_intervalEndInMilliseconds > GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds)
			{
				return false;
			}
			UpdateIntervalEnd();
			return true;
		}

		private void UpdateIntervalEnd()
		{
			_intervalEndInMilliseconds = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds + _intervalInMilliseconds;
		}
	}
}
