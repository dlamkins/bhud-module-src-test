using System;
using System.Collections.Generic;
using System.Globalization;

namespace Frtal.LorebookReader
{
	public class LorebookEntry
	{
		public string Id { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string TimestampUtc { get; set; }

		public string ColorTag { get; set; }

		public string IconKey { get; set; }

		public string Expansion { get; set; }

		public string Theme { get; set; }

		public string Location { get; set; }

		public string Notes { get; set; }

		public string TranslatedText { get; set; }

		public string TranslatedLang { get; set; }

		public bool Opened { get; set; } = true;


		public string DisplayTitle
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(Title))
				{
					return Title;
				}
				return LorebookCatalog.MakeFallbackTitle(Text);
			}
		}

		public DateTime TimestampLocal
		{
			get
			{
				if (!DateTime.TryParse(TimestampUtc, null, DateTimeStyles.RoundtripKind, out var dt))
				{
					return DateTime.MinValue;
				}
				return dt.ToLocalTime();
			}
		}

		public string MetadataLine
		{
			get
			{
				List<string> parts = new List<string>();
				if (!string.IsNullOrWhiteSpace(Expansion))
				{
					parts.Add(Expansion);
				}
				if (!string.IsNullOrWhiteSpace(Theme))
				{
					parts.Add(Theme);
				}
				if (!string.IsNullOrWhiteSpace(Location))
				{
					parts.Add(Location);
				}
				return string.Join(" · ", parts);
			}
		}

		public LorebookEntry()
		{
			Id = Guid.NewGuid().ToString("N");
		}
	}
}
