using System;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Bell : InstrumentBase
	{
		public Bell()
		{
			base.CurrentOctave = Octave.Middle;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return BellNote.From(note);
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new BellNote((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Middle)
			{
				note = new BellNote((GuildWarsControls)9, Octave.Middle);
			}
			else if (note.Equals(new BellNote((GuildWarsControls)9, Octave.Middle)) && base.CurrentOctave == Octave.High)
			{
				note = new BellNote((GuildWarsControls)2, Octave.High);
			}
			else if (note.Equals(new BellNote((GuildWarsControls)2, Octave.Middle)) && base.CurrentOctave == Octave.Low)
			{
				note = new BellNote((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new BellNote((GuildWarsControls)9, Octave.Low)) && base.CurrentOctave == Octave.Middle)
			{
				note = new BellNote((GuildWarsControls)2, Octave.Middle);
			}
			return note;
		}

		protected override void IncreaseOctave()
		{
			switch (base.CurrentOctave)
			{
			case Octave.Low:
				base.CurrentOctave = Octave.Middle;
				break;
			case Octave.Middle:
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
			switch (base.CurrentOctave)
			{
			case Octave.Middle:
				base.CurrentOctave = Octave.Low;
				break;
			case Octave.High:
				base.CurrentOctave = Octave.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Octave.Low:
				break;
			}
			PressKey((GuildWarsControls)10);
			Thread.Sleep(OctaveTimeout);
		}

		public override void Dispose()
		{
		}
	}
}
