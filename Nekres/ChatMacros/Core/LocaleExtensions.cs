using System.Globalization;
using Gw2Sharp.WebApi;

namespace Nekres.ChatMacros.Core
{
	internal static class LocaleExtensions
	{
		public static CultureInfo GetCulture(this Locale locale)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected I4, but got Unknown
			return (int)locale switch
			{
				0 => CultureInfo.GetCultureInfo(9), 
				1 => CultureInfo.GetCultureInfo(10), 
				2 => CultureInfo.GetCultureInfo(7), 
				3 => CultureInfo.GetCultureInfo(12), 
				4 => CultureInfo.GetCultureInfo(18), 
				5 => CultureInfo.GetCultureInfo(30724), 
				_ => CultureInfo.GetCultureInfo(9), 
			};
		}
	}
}
