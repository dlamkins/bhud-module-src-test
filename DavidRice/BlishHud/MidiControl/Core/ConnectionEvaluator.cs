using System.Collections.Generic;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public static class ConnectionEvaluator
	{
		public enum Action
		{
			NoAction,
			Close,
			Reopen
		}

		public static Action Evaluate(string? targetDeviceName, IReadOnlyList<string> availableDevices, bool isDeviceOpen)
		{
			if (string.IsNullOrEmpty(targetDeviceName))
			{
				return Action.NoAction;
			}
			bool deviceAvailable = Contains(availableDevices, targetDeviceName);
			if (isDeviceOpen && !deviceAvailable)
			{
				return Action.Close;
			}
			if (!isDeviceOpen && deviceAvailable)
			{
				return Action.Reopen;
			}
			return Action.NoAction;
		}

		private static bool Contains(IReadOnlyList<string> list, string value)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
