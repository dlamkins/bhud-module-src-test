using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nekres.Musician.Core.Domain
{
	public class Chord
	{
		private static readonly Regex NotesAndDurationRegex = new Regex("\\[?([ABCDEFGZabcdefgz',]+)\\]?(\\d+)?\\/?(\\d+)?");

		private static readonly Regex NoteRegex = new Regex("([ABCDEFGZabcdefgz][,]{0,3}[']{0,2})");

		public Fraction Length { get; }

		public IEnumerable<RealNote> Notes { get; }

		private Chord(IEnumerable<RealNote> notes, Fraction length)
		{
			Length = length;
			Notes = notes;
		}

		public static Chord Parse(string text)
		{
			Match match = NotesAndDurationRegex.Match(text);
			string chord = match.Groups[1].Value;
			string nominator = match.Groups[2].Value;
			string denominator = match.Groups[3].Value;
			MatchCollection source = NoteRegex.Matches(chord);
			return new Chord(length: Fraction.Parse(nominator, denominator), notes: from Match x in source
				select RealNote.Deserialize(x.Groups[1].Value));
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Notes.Count() > 1)
			{
				stringBuilder.Append("[");
			}
			foreach (RealNote note in Notes)
			{
				stringBuilder.Append(note.Serialize());
			}
			if (Notes.Count() > 1)
			{
				stringBuilder.Append("]");
			}
			if (Length.Nominator != 1)
			{
				stringBuilder.Append(Length.Nominator);
			}
			if (Length.Denominator != 1)
			{
				stringBuilder.Append("/");
				stringBuilder.Append(Length.Denominator);
			}
			return stringBuilder.ToString();
		}
	}
}
