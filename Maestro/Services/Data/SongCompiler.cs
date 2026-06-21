using System.Collections.Generic;
using Maestro.Models;

namespace Maestro.Services.Data
{
	public static class SongCompiler
	{
		private static bool IsPercussion(InstrumentType instrument)
		{
			return InstrumentCatalog.Get(instrument).IsPercussion;
		}

		public static NoteParser.ParseResult ParseWithMapping(List<string> notes, InstrumentType instrument)
		{
			if (!IsPercussion(instrument))
			{
				return NoteParser.ParseWithMapping(notes);
			}
			return DrumParser.ParseWithMapping(notes);
		}

		public static List<SongCommand> Parse(List<string> notes, InstrumentType instrument)
		{
			if (!IsPercussion(instrument))
			{
				return NoteParser.Parse(notes);
			}
			return DrumParser.Parse(notes);
		}

		public static long CalculateDurationMs(List<string> notes, InstrumentType instrument)
		{
			if (!IsPercussion(instrument))
			{
				return NoteParser.CalculateDurationMs(notes);
			}
			return DrumParser.CalculateDurationMs(notes);
		}

		public static SeekData ComputeSeekData(List<SongCommand> commands, InstrumentType instrument)
		{
			if (!IsPercussion(instrument))
			{
				return NoteParser.ComputeSeekData(commands);
			}
			return DrumParser.ComputeSeekData(commands);
		}
	}
}
