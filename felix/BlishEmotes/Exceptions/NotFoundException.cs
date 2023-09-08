using System;

namespace felix.BlishEmotes.Exceptions
{
	internal class NotFoundException : Exception
	{
		public NotFoundException()
			: base("No object for for given id")
		{
		}

		public NotFoundException(string message)
			: base(message)
		{
		}

		public NotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
