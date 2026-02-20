using System;

namespace CinemaModule.Models
{
	public static class StreamTypeExtensions
	{
		private const string RadioType = "radio";

		public static StreamType ParseFromString(string typeString)
		{
			if (!string.Equals(typeString, "radio", StringComparison.OrdinalIgnoreCase))
			{
				return StreamType.Video;
			}
			return StreamType.Radio;
		}
	}
}
