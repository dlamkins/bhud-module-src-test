using System;
using System.Collections.Generic;
using System.Linq;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<Enum> GetFlags(this Enum e)
		{
			return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
		}
	}
}
