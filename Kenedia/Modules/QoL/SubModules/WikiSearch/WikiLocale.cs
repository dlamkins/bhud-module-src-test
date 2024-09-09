using System.Collections.Generic;
using System.Linq;

namespace Kenedia.Modules.QoL.SubModules.WikiSearch
{
	public static class WikiLocale
	{
		public enum Locale
		{
			Default,
			English,
			German,
			Spanish,
			French
		}

		public static Dictionary<Locale, string> Locales { get; } = new Dictionary<Locale, string>
		{
			{
				Locale.Default,
				"Default"
			},
			{
				Locale.English,
				"English"
			},
			{
				Locale.German,
				"Deutsch"
			},
			{
				Locale.Spanish,
				"Español"
			},
			{
				Locale.French,
				"Français"
			}
		};


		public static string ToDisplayString(Locale locale)
		{
			if (!Locales.TryGetValue(locale, out var s))
			{
				return Locales[Locale.English];
			}
			return s;
		}

		public static Locale FromDisplayString(string displayString)
		{
			return Locales.FirstOrDefault((KeyValuePair<Locale, string> x) => x.Value == displayString).Key;
		}
	}
}
