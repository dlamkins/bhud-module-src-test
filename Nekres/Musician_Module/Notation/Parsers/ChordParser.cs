using System.Linq;
using System.Text.RegularExpressions;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Notation.Parsers
{
	public class ChordParser
	{
		private static readonly Regex NotesAndDurationRegex = new Regex("\\[?([ABCDEFGZabcdefgz',]+)\\]?(\\d+)?\\/?(\\d+)?");

		private static readonly Regex NoteRegex = new Regex("([ABCDEFGZabcdefgz][,]{0,3}[']{0,2})");

		private readonly NoteParser _noteParser;

		private readonly string _instrument;

		public ChordParser(NoteParser noteParser, string instrument)
		{
			_noteParser = noteParser;
			_instrument = instrument;
		}

		public Chord Parse(string text)
		{
			Match match = NotesAndDurationRegex.Match(text);
			string notes = match.Groups[1].Value;
			string nominator = match.Groups[2].Value;
			string denomintor = match.Groups[3].Value;
			Fraction length = ParseFraction(nominator, denomintor);
			return new Chord(from Match x in NoteRegex.Matches(notes)
				select _noteParser.Parse(x.Groups[1].Value, _instrument), length);
		}

		private static Fraction ParseFraction(string nominator, string denominator)
		{
			return new Fraction(string.IsNullOrEmpty(nominator) ? 1 : int.Parse(nominator), string.IsNullOrEmpty(denominator) ? 1 : int.Parse(denominator));
		}
	}
}
