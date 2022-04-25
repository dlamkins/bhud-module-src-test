using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Flute : InstrumentBase
	{
		public Flute()
			: base(walkable: false)
		{
			base.CurrentOctave = Octave.Low;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return FluteNote.From(note);
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new FluteNote((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Low)
			{
				note = new FluteNote((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new FluteNote((GuildWarsControls)9, Octave.Low)) && base.CurrentOctave == Octave.High)
			{
				note = new FluteNote((GuildWarsControls)2, Octave.High);
			}
			return note;
		}

		protected override void IncreaseOctave()
		{
			switch (base.CurrentOctave)
			{
			case Octave.Low:
				base.CurrentOctave = Octave.High;
				break;
			}
			PressKey((GuildWarsControls)10);
			Thread.Sleep(OctaveTimeout);
		}

		protected override void DecreaseOctave()
		{
			Octave currentOctave = base.CurrentOctave;
			if (currentOctave != Octave.Low && currentOctave == Octave.High)
			{
				base.CurrentOctave = Octave.Low;
			}
			PressKey((GuildWarsControls)10);
			Thread.Sleep(OctaveTimeout);
		}

		public override void Dispose()
		{
		}
	}
}
