using System;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Bell2 : InstrumentBase
	{
		public Bell2()
		{
			base.CurrentOctave = Octave.Low;
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new Bell2Note((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Low)
			{
				note = new Bell2Note((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new Bell2Note((GuildWarsControls)9, Octave.Low)) && base.CurrentOctave == Octave.High)
			{
				note = new Bell2Note((GuildWarsControls)2, Octave.High);
			}
			return note;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return Bell2Note.From(note);
		}

		protected override void IncreaseOctave()
		{
			switch (base.CurrentOctave)
			{
			case Octave.Low:
				base.CurrentOctave = Octave.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Octave.High:
				break;
			}
			PressKey((GuildWarsControls)11);
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
