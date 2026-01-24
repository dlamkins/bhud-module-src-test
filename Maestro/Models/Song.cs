using System;
using System.Collections.Generic;
using System.Linq;

namespace Maestro.Models
{
	public class Song
	{
		public string Name { get; set; }

		public string Artist { get; set; }

		public string Transcriber { get; set; }

		public InstrumentType Instrument { get; set; }

		public List<SongCommand> Commands { get; set; } = new List<SongCommand>();


		public List<string> Notes { get; set; } = new List<string>();


		public bool IsUserImported { get; set; }

		public bool IsCreated { get; set; }

		public bool SkipOctaveReset { get; set; }

		public string CommunityId { get; set; }

		public int Downloads { get; set; }

		public bool IsCommunityDownloaded => !string.IsNullOrEmpty(CommunityId);

		public string DisplayDownloads
		{
			get
			{
				if (Downloads <= 0)
				{
					return null;
				}
				if (Downloads >= 1000000)
				{
					return $"{(double)Downloads / 1000000.0:F1}M";
				}
				if (Downloads >= 1000)
				{
					return $"{(double)Downloads / 1000.0:F1}k";
				}
				return Downloads.ToString();
			}
		}

		public string DisplayName => Name + " - " + Artist;

		public string DisplayDuration
		{
			get
			{
				int totalMs = Commands.Where((SongCommand c) => c.Type == CommandType.Wait).Sum((SongCommand c) => c.Duration);
				if (totalMs <= 0)
				{
					return null;
				}
				TimeSpan span = TimeSpan.FromMilliseconds(totalMs);
				if (!(span.TotalHours >= 1.0))
				{
					return span.ToString("m\\:ss");
				}
				return span.ToString("h\\:mm\\:ss");
			}
		}
	}
}
