using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Horn : InstrumentBase
	{
		public Horn()
			: base(walkable: false)
		{
			base.CurrentOctave = Octave.Low;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return HornNote.From(note);
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new HornNote((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Middle)
			{
				note = new HornNote((GuildWarsControls)9, Octave.Middle);
			}
			else if (note.Equals(new HornNote((GuildWarsControls)9, Octave.Middle)) && base.CurrentOctave == Octave.High)
			{
				note = new HornNote((GuildWarsControls)2, Octave.High);
			}
			else if (note.Equals(new HornNote((GuildWarsControls)2, Octave.Middle)) && base.CurrentOctave == Octave.Low)
			{
				note = new HornNote((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new HornNote((GuildWarsControls)9, Octave.Low)) && base.CurrentOctave == Octave.Middle)
			{
				note = new HornNote((GuildWarsControls)2, Octave.Middle);
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
			}
			PressKey((GuildWarsControls)10);
			Thread.Sleep(OctaveTimeout);
		}

		public override void Dispose()
		{
		}
	}
}
