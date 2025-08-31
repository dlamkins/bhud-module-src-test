using System.Linq;
using Gw2Sharp.WebApi;

namespace Blish_HUD.Extended
{
	public static class LocaleExtensions
	{
		public static string TwoLetterISOLanguageName(this Locale locale)
		{
			return locale switch
			{
				Locale.English => "en", 
				Locale.Spanish => "es", 
				Locale.German => "de", 
				Locale.French => "fr", 
				Locale.Korean => "kr", 
				Locale.Chinese => "zh", 
				_ => "en", 
			};
		}

		public static Locale SupportedOrDefault(this Locale locale, params Locale[] supported)
		{
			if (supported == null || !supported.Any())
			{
				return locale switch
				{
					Locale.Spanish => locale, 
					Locale.German => locale, 
					Locale.French => locale, 
					_ => Locale.English, 
				};
			}
			if (!supported.Contains(locale))
			{
				return supported.FirstOrDefault();
			}
			return locale;
		}
	}
}
