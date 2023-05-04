using System;

namespace Ideka.RacingMeter
{
	internal static class IntegerExtensions
	{
		public static string Ordinalize(this int value)
		{
			int abs = Math.Abs(value);
			object arg = value;
			int num = abs % 100;
			string arg2 = (((uint)(num - 11) > 2u) ? ((abs % 10) switch
			{
				1 => "st", 
				2 => "nd", 
				3 => "rd", 
				_ => "th", 
			}) : "th");
			return $"{arg}{arg2}";
		}
	}
}
