using System;
using Blish_HUD;

namespace Kenedia.Modules.Core.Utility
{
	public class Common
	{
		public static double Now()
		{
			return GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		public static bool SetProperty<T>(ref T property, T newValue, bool triggerOnUpdate = true, Action OnUpdated = null)
		{
			if (object.Equals(property, newValue))
			{
				return false;
			}
			property = newValue;
			if (triggerOnUpdate)
			{
				OnUpdated?.Invoke();
			}
			return true;
		}
	}
}
