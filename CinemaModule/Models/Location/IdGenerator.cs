using System;

namespace CinemaModule.Models.Location
{
	public static class IdGenerator
	{
		private const int DefaultIdLength = 8;

		public static string Generate(int length = 8)
		{
			return Guid.NewGuid().ToString("N").Substring(0, length);
		}
	}
}
