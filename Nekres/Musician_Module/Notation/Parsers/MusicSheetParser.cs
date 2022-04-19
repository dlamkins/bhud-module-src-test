using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Nekres.Musician_Module.Domain;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Notation.Parsers
{
	public class MusicSheetParser
	{
		private static readonly Regex NonWhitespace = new Regex("[^\\s]+");

		private readonly ChordParser _chordParser;

		public MusicSheetParser(ChordParser chordParser)
		{
			_chordParser = chordParser;
		}

		public MusicSheet Parse(string text, int metronome, int nominator, int denominator)
		{
			Fraction beatsPerMeasure = BeatsPerMeasure(nominator, denominator);
			return new MusicSheet(string.Empty, string.Empty, new MetronomeMark(metronome, beatsPerMeasure), ParseMelody(text));
		}

		private static Fraction BeatsPerMeasure(int nominator, int denominator)
		{
			return new Fraction(nominator, denominator);
		}

		private IEnumerable<ChordOffset> ParseMelody(string textMelody)
		{
			decimal currentBeat = default(decimal);
			return NonWhitespace.Matches(textMelody).Cast<Match>().Select(delegate(Match textChord)
			{
				Chord chord = _chordParser.Parse(textChord.Value);
				ChordOffset result = new ChordOffset(chord, new Beat(currentBeat));
				currentBeat += (decimal)chord.Length;
				return result;
			});
		}
	}
}
