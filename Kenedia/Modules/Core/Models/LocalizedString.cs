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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			foreach (Locale locale in Enum.GetValues(typeof(Locale)))
			{
				Add(locale, null);
			}
		}

		public LocalizedString(string comon)
			: this()
		{
			base[(Locale)0] = comon;
		}

		public LocalizedString(string comon, Locale lang)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			base[lang] = comon;
		}

		private void SetText(string value, Locale? lang = null)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Locale valueOrDefault = lang.GetValueOrDefault();
			if (!lang.HasValue)
			{
				valueOrDefault = GameService.Overlay.get_UserLocale().get_Value();
				lang = valueOrDefault;
			}
			if (lang.HasValue)
			{
				base[lang.Value] = value;
			}
		}

		public string GetText(Locale? lang = null)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Locale valueOrDefault = lang.GetValueOrDefault();
			if (!lang.HasValue)
			{
				valueOrDefault = GameService.Overlay.get_UserLocale().get_Value();
				lang = valueOrDefault;
			}
			if (!lang.HasValue || !TryGetValue(lang.Value, out var text) || string.IsNullOrEmpty(text))
			{
				if (!TryGetValue((Locale)0, out var english) || string.IsNullOrEmpty(english))
				{
					return null;
				}
				return english;
			}
			return text;
		}
	}
}
