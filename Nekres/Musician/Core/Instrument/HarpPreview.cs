using System;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	internal class HarpPreview : InstrumentBase
	{
		private readonly ISoundRepository _soundRepository;

		public HarpPreview(ISoundRepository soundRepo)
		{
			base.CurrentOctave = Octave.Middle;
			_soundRepository = soundRepo;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return HarpNote.From(note);
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new HarpNote((GuildWarsControls)2, Octave.Middle)) && base.CurrentOctave == Octave.Low)
			{
				note = new HarpNote((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new HarpNote((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Middle)
			{
				note = new HarpNote((GuildWarsControls)9, Octave.Middle);
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
		}

		protected override void PressKey(GuildWarsControls key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			switch (key - 2)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				MusicianModule.ModuleInstance.MusicPlayer.PlaySound(_soundRepository.Get(key, base.CurrentOctave));
				break;
			case 8:
				DecreaseOctave();
				break;
			case 9:
				IncreaseOctave();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public override void Dispose()
		{
		}
	}
}
