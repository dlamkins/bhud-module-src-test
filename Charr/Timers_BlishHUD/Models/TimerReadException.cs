using System;

namespace Charr.Timers_BlishHUD.Models
{
	public class TimerReadException : Exception
	{
		public TimerReadException()
		{
		}

		public TimerReadException(string message)
			: base(message)
		{
		}

		public TimerReadException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
