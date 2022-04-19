using System.Text;
using Nekres.Musician_Module.Domain;
using Nekres.Musician_Module.Domain.Values;
using Nekres.Musician_Module.Notation.Persistance;

namespace Nekres.Musician_Module.Notation.Serializer
{
	public class MusicSheetSerializer
	{
		private readonly ChordOffsetSerializer _chordOffsetSerializer;

		public MusicSheetSerializer(ChordOffsetSerializer chordOffsetSerializer)
		{
			_chordOffsetSerializer = chordOffsetSerializer;
		}

		public RawMusicSheet Serialize(MusicSheet musicSheet)
		{
			string artist = musicSheet.Artist;
			string title = musicSheet.Title;
			string user = musicSheet.User;
			string instrument = musicSheet.Instrument;
			string tempo = SerializeTempo(musicSheet);
			string meter = SerializeMeter(musicSheet);
			string melody = SerializeMelody(musicSheet);
			string algorithm = SerializeAlgorithm();
			return new RawMusicSheet(artist, title, user, instrument, tempo, meter, melody, algorithm);
		}

		private static string SerializeTempo(MusicSheet musicSheet)
		{
			return musicSheet.MetronomeMark.Metronome.ToString();
		}

		private static string SerializeMeter(MusicSheet musicSheet)
		{
			Fraction beatsPerMeasure = musicSheet.MetronomeMark.BeatsPerMeasure;
			return $"{beatsPerMeasure.Nominator}/{beatsPerMeasure.Denominator}";
		}

		private string SerializeMelody(MusicSheet musicSheet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ChordOffset chordOffset in musicSheet.Melody)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(_chordOffsetSerializer.Serialize(chordOffset));
			}
			return stringBuilder.ToString();
		}

		private static string SerializeAlgorithm()
		{
			return "favor chords";
		}
	}
}
