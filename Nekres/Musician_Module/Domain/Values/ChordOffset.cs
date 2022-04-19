namespace Nekres.Musician_Module.Domain.Values
{
	public class ChordOffset
	{
		public Chord Chord { get; }

		public Beat Offest { get; }

		public ChordOffset(Chord chord, Beat offest)
		{
			Chord = chord;
			Offest = offest;
		}
	}
}
