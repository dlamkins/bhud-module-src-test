using System;
using System.Collections.Generic;
using System.Linq;
using Maestro.Services.Data;

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

		public string BuiltInId { get; set; }

		public bool IsUploaded { get; set; }

		public bool IsSubmittal { get; set; }

		public int? Bpm { get; set; }

		public SeekData SeekData { get; set; }

		public bool IsCommunityDownloaded
		{
			get
			{
				if (!string.IsNullOrEmpty(CommunityId) && !IsUserImported)
				{
					return !IsCreated;
				}
				return false;
			}
		}

		public string DisplayName => Name + " - " + Artist;

		public long DurationMs
		{
			get
			{
				if (Notes.Count <= 0)
				{
					return Commands.Where((SongCommand c) => c.Type == CommandType.Wait).Sum((SongCommand c) => c.Duration);
				}
				return SongCompiler.CalculateDurationMs(Notes, Instrument);
			}
		}

		public string DisplayDuration
		{
			get
			{
				if (DurationMs <= 0)
				{
					return null;
				}
				TimeSpan span = TimeSpan.FromMilliseconds(DurationMs);
				if (!(span.TotalHours >= 1.0))
				{
					return span.ToString("m\\:ss");
				}
				return span.ToString("h\\:mm\\:ss");
			}
		}
	}
}
