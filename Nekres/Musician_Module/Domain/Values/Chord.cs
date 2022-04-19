using System.Collections.Generic;

namespace Nekres.Musician_Module.Domain.Values
{
	public class Chord
	{
		public Fraction Length { get; }

		public IEnumerable<Note> Notes { get; }

		public Chord(IEnumerable<Note> notes, Fraction length)
		{
			Length = length;
			Notes = notes;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", string.Join(":", Notes), Length);
		}
	}
}
