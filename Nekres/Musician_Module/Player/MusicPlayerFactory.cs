using System.Collections.Generic;
using Blish_HUD.Controls;
using Nekres.Musician_Module.Controls.Instrument;
using Nekres.Musician_Module.Domain;
using Nekres.Musician_Module.Notation.Parsers;
using Nekres.Musician_Module.Notation.Persistance;
using Nekres.Musician_Module.Player.Algorithms;

namespace Nekres.Musician_Module.Player
{
	internal static class MusicPlayerFactory
	{
		private static Dictionary<string, Instrument> InstrumentRepository = new Dictionary<string, Instrument>
		{
			{
				"harp",
				new Harp()
			},
			{
				"flute",
				new Flute()
			},
			{
				"lute",
				new Lute()
			},
			{
				"horn",
				new Horn()
			},
			{
				"bass",
				new Bass()
			},
			{
				"bell",
				new Bell()
			},
			{
				"bell2",
				new Bell2()
			}
		};

		internal static void Dispose()
		{
			foreach (KeyValuePair<string, Instrument> item in InstrumentRepository)
			{
				item.Value?.Dispose();
			}
			InstrumentRepository?.Clear();
			InstrumentRepository = null;
		}

		internal static MusicPlayer Create(RawMusicSheet rawMusicSheet, InstrumentMode mode)
		{
			return MusicBoxNotationMusicPlayerFactory(rawMusicSheet, mode);
		}

		private static MusicPlayer MusicBoxNotationMusicPlayerFactory(RawMusicSheet rawMusicSheet, InstrumentMode mode)
		{
			MusicSheet musicSheet = new MusicSheetParser(new ChordParser(new NoteParser(), rawMusicSheet.Instrument)).Parse(rawMusicSheet.Melody, int.Parse(rawMusicSheet.Tempo), int.Parse(rawMusicSheet.Meter.Split('/')[0]), int.Parse(rawMusicSheet.Meter.Split('/')[1]));
			IPlayAlgorithm playAlgorithm2;
			if (!(rawMusicSheet.Algorithm == "favor notes"))
			{
				IPlayAlgorithm playAlgorithm = new FavorChordsAlgorithm();
				playAlgorithm2 = playAlgorithm;
			}
			else
			{
				IPlayAlgorithm playAlgorithm = new FavorNotesAlgorithm();
				playAlgorithm2 = playAlgorithm;
			}
			IPlayAlgorithm algorithm = playAlgorithm2;
			Instrument instrument = InstrumentRepository[rawMusicSheet.Instrument];
			instrument.Mode = mode;
			((Control)MusicianModule.ModuleInstance.Conveyor).set_Visible(mode == InstrumentMode.Practice);
			return new MusicPlayer(musicSheet, instrument, algorithm);
		}
	}
}
