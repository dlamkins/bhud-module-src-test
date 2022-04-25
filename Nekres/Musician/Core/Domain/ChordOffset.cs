using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nekres.Musician.Core.Domain
{
	public class ChordOffset
	{
		private static readonly Regex NonWhitespace = new Regex("[^\\s]+");

		public Chord Chord { get; }

		public Beat Offset { get; }

		public ChordOffset(Chord chord, Beat offset)
		{
			Chord = chord;
			Offset = offset;
		}

		public override string ToString()
		{
			return Chord.ToString();
		}

		public static IEnumerable<ChordOffset> MelodyFromString(string s)
		{
			decimal currentBeat = default(decimal);
			return NonWhitespace.Matches(s).Cast<Match>().Select(delegate(Match textChord)
			{
				Chord chord = Chord.Parse(textChord.Value);
				ChordOffset result = new ChordOffset(chord, new Beat(currentBeat));
				currentBeat += (decimal)chord.Length;
				return result;
			});
		}
	}
}
