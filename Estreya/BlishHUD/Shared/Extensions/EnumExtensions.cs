using System;
using System.Collections.Generic;
using System.Linq;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.Services;
using Humanizer;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<Enum> GetFlags(this Enum e)
		{
			return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
		}

		public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
		{
			object[] attributes = enumVal.GetType().GetMember(enumVal.ToString())[0].GetCustomAttributes(typeof(T), inherit: false);
			if (attributes.Length == 0)
			{
				return null;
			}
			return (T)attributes[0];
		}

		public static string GetTranslatedValue(this Enum enumVal, TranslationService translationService)
		{
			return enumVal.GetTranslatedValue(translationService, LetterCasing.Title);
		}

		public static string GetTranslatedValue(this Enum enumVal, TranslationService translationService, LetterCasing fallbackCasing)
		{
			TranslationAttribute translationsAttribute = enumVal.GetAttributeOfType<TranslationAttribute>();
			if (translationsAttribute == null)
			{
				return enumVal.Humanize(fallbackCasing);
			}
			return translationService.GetTranslation(translationsAttribute.TranslationKey, translationsAttribute.DefaultValue);
		}
	}
}
