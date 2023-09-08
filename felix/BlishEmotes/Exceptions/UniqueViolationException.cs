using System;

namespace felix.BlishEmotes.Exceptions
{
	internal class UniqueViolationException : Exception
	{
		public UniqueViolationException()
			: base("Violated unique constraint")
		{
		}

		public UniqueViolationException(string message)
			: base(message)
		{
		}

		public UniqueViolationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
