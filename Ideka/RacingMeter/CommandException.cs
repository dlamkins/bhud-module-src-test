using System;

namespace Ideka.RacingMeter
{
	public class CommandException : Exception
	{
		public CommandException(string message)
			: base(message)
		{
		}
	}
}
