using System;
using System.Linq;

namespace Nekres.Stream_Out
{
	public static class IntegerExtensions
	{
		public static string ToRomanNumeral(this int number)
		{
			number = Math.Abs(number);
			string[,] romanNumerals = new string[4, 10]
			{
				{ "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" },
				{ "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" },
				{ "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" },
				{ "", "M", "MM", "MMM", "", "", "", "", "", "" }
			};
			char[] intArr = number.ToString().ToCharArray().Reverse()
				.ToArray();
			int len = intArr.Length - 1;
			string romanNumeral = "";
			for (int i = len; i >= 0; i--)
			{
				romanNumeral += romanNumerals[i, int.Parse(intArr[i].ToString())];
			}
			return romanNumeral;
		}

		public static bool InRange(this int number, int[] range)
		{
			return number >= range.Min() && number <= range.Max();
		}
	}
}
