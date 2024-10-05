using System;
using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Kenedia.Modules.Core.Models
{
	public class LocalizedString : Dictionary<Locale, string>
	{
		public string Text
		{
			get
			{
				return GetText();
			}
			set
			{
				SetText(value);
			}
		}

		public LocalizedString()
		{
			foreach (Locale locale in Enum.GetValues(typeof(Locale)))
			{
				if (locale != Locale.Korean && locale != Locale.Chinese)
				{
					Add(locale, null);
				}
			}
		}

		public LocalizedString(string comon)
			: this()
		{
			base[Locale.English] = comon;
		}

		public LocalizedString(string comon, Locale lang)
			: this()
		{
			base[lang] = comon;
		}

		private void SetText(string value, Locale? lang = null)
		{
			Locale valueOrDefault = lang.GetValueOrDefault();
			if (!lang.HasValue)
			{
				valueOrDefault = GameService.Overlay.UserLocale.Value;
				lang = valueOrDefault;
			}
			if (lang.HasValue)
			{
				base[lang.Value] = value;
			}
		}

		public string GetText(Locale? lang = null)
		{
			Locale valueOrDefault = lang.GetValueOrDefault();
			if (!lang.HasValue)
			{
				valueOrDefault = GameService.Overlay.UserLocale.Value;
				lang = valueOrDefault;
			}
			if (!lang.HasValue || !TryGetValue(lang.Value, out var text) || string.IsNullOrEmpty(text))
			{
				if (!TryGetValue(Locale.English, out var english) || string.IsNullOrEmpty(english))
				{
					return null;
				}
				return english;
			}
			return text;
		}
	}
}
