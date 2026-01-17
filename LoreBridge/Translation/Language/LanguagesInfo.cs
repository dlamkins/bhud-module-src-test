using System;
using System.Collections.Generic;
using System.Linq;

namespace LoreBridge.Translation.Language
{
	public static class LanguagesInfo
	{
		public static readonly List<LanguageInfo> List = new List<LanguageInfo>(29)
		{
			new LanguageInfo
			{
				Language = Languages.Bulgarian,
				Code = "bg-BG",
				IsoCode = "bg",
				Name = "Bulgarian"
			},
			new LanguageInfo
			{
				Language = Languages.ChineseSimplified,
				Code = "zh-CN",
				IsoCode = "zh-CN",
				Name = "Chinese (Simplified)"
			},
			new LanguageInfo
			{
				Language = Languages.ChineseTraditional,
				Code = "zh-TW",
				IsoCode = "zh-TW",
				Name = "Chinese (Traditional)"
			},
			new LanguageInfo
			{
				Language = Languages.Czech,
				Code = "cs-CZ",
				IsoCode = "cs",
				Name = "Czech"
			},
			new LanguageInfo
			{
				Language = Languages.Danish,
				Code = "da-DK",
				IsoCode = "da",
				Name = "Danish"
			},
			new LanguageInfo
			{
				Language = Languages.Dutch,
				Code = "nl-NL",
				IsoCode = "nl",
				Name = "Dutch"
			},
			new LanguageInfo
			{
				Language = Languages.Estonian,
				Code = "et-EE",
				IsoCode = "et",
				Name = "Estonian"
			},
			new LanguageInfo
			{
				Language = Languages.Finnish,
				Code = "fi-FI",
				IsoCode = "fi",
				Name = "Finnish"
			},
			new LanguageInfo
			{
				Language = Languages.Greek,
				Code = "el-GR",
				IsoCode = "el",
				Name = "Greek"
			},
			new LanguageInfo
			{
				Language = Languages.German,
				Code = "de-DE",
				IsoCode = "de",
				Name = "German"
			},
			new LanguageInfo
			{
				Language = Languages.Hungarian,
				Code = "hu-HU",
				IsoCode = "hu",
				Name = "Hungarian"
			},
			new LanguageInfo
			{
				Language = Languages.Indonesian,
				Code = "id-ID",
				IsoCode = "id",
				Name = "Indonesian"
			},
			new LanguageInfo
			{
				Language = Languages.Italian,
				Code = "it-IT",
				IsoCode = "it",
				Name = "Italian"
			},
			new LanguageInfo
			{
				Language = Languages.Japanese,
				Code = "ja-JP",
				IsoCode = "ja",
				Name = "Japanese"
			},
			new LanguageInfo
			{
				Language = Languages.Latvian,
				Code = "lv-LV",
				IsoCode = "lv",
				Name = "Latvian"
			},
			new LanguageInfo
			{
				Language = Languages.Lithuanian,
				Code = "lt-LT",
				IsoCode = "lt",
				Name = "Lithuanian"
			},
			new LanguageInfo
			{
				Language = Languages.Norwegian,
				Code = "no-NO",
				IsoCode = "no",
				Name = "Norwegian"
			},
			new LanguageInfo
			{
				Language = Languages.Polish,
				Code = "pl-PL",
				IsoCode = "pl",
				Name = "Polish"
			},
			new LanguageInfo
			{
				Language = Languages.Portuguese,
				Code = "pt-PT",
				IsoCode = "pt",
				Name = "Portuguese"
			},
			new LanguageInfo
			{
				Language = Languages.PortugueseBrazilian,
				Code = "pt-BR",
				IsoCode = "pt-BR",
				Name = "Portuguese (Brazil)"
			},
			new LanguageInfo
			{
				Language = Languages.Romanian,
				Code = "ro-RO",
				IsoCode = "ro",
				Name = "Romanian"
			},
			new LanguageInfo
			{
				Language = Languages.Russian,
				Code = "ru-RU",
				IsoCode = "ru",
				Name = "Russian"
			},
			new LanguageInfo
			{
				Language = Languages.Slovak,
				Code = "sk-SK",
				IsoCode = "sk",
				Name = "Slovak"
			},
			new LanguageInfo
			{
				Language = Languages.Slovenian,
				Code = "sl-SI",
				IsoCode = "sl",
				Name = "Slovenian"
			},
			new LanguageInfo
			{
				Language = Languages.Korean,
				Code = "ko-KR",
				IsoCode = "ko",
				Name = "Korean"
			},
			new LanguageInfo
			{
				Language = Languages.Swedish,
				Code = "sv-SE",
				IsoCode = "sv",
				Name = "Swedish"
			},
			new LanguageInfo
			{
				Language = Languages.Turkish,
				Code = "tr-TR",
				IsoCode = "tr",
				Name = "Turkish"
			},
			new LanguageInfo
			{
				Language = Languages.Ukrainian,
				Code = "uk-UA",
				IsoCode = "uk",
				Name = "Ukrainian"
			},
			new LanguageInfo
			{
				Language = Languages.Vietnamese,
				Code = "vi-VN",
				IsoCode = "vi",
				Name = "Vietnamese"
			}
		};

		private static readonly Dictionary<Languages, LanguageInfo> _byLanguage = List.ToDictionary((LanguageInfo d) => d.Language);

		private static readonly Dictionary<string, LanguageInfo> _byName = List.ToDictionary((LanguageInfo d) => d.Name);

		public static LanguageInfo GetByLanguage(int languageCode)
		{
			if (!Enum.IsDefined(typeof(Languages), languageCode))
			{
				return null;
			}
			if (!_byLanguage.TryGetValue((Languages)languageCode, out var detail))
			{
				return null;
			}
			return detail;
		}

		public static LanguageInfo GetByName(string name)
		{
			if (!_byName.TryGetValue(name, out var detail))
			{
				return null;
			}
			return detail;
		}
	}
}
