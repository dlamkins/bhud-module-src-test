using System;

namespace Maestro.Models
{
	public class CommunitySong
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Artist { get; set; }

		public string Transcriber { get; set; }

		public string Instrument { get; set; }

		public long DurationMs { get; set; }

		public DateTime CreatedAt { get; set; }

		public string DisplayDuration
		{
			get
			{
				TimeSpan span = TimeSpan.FromMilliseconds(DurationMs);
				if (!(span.TotalHours >= 1.0))
				{
					return $"{span.Minutes}:{span.Seconds:D2}";
				}
				return $"{(int)span.TotalHours}:{span.Minutes:D2}:{span.Seconds:D2}";
			}
		}

		public InstrumentType InstrumentType
		{
			get
			{
				if (Enum.TryParse<InstrumentType>(Instrument, ignoreCase: true, out var type))
				{
					return type;
				}
				return InstrumentType.Harp;
			}
		}
	}
}
