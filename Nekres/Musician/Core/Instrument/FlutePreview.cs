using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	internal class FlutePreview : InstrumentBase
	{
		private readonly ISoundRepository _soundRepository;

		public FlutePreview(ISoundRepository soundRepo)
		{
			base.CurrentOctave = Octave.Low;
			_soundRepository = soundRepo;
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
			Octave currentOctave = base.CurrentOctave;
			if (currentOctave != Octave.Low)
			{
				_ = 3;
			}
			else
			{
				base.CurrentOctave = Octave.High;
			}
		}

		protected override void DecreaseOctave()
		{
			Octave currentOctave = base.CurrentOctave;
			if (currentOctave != Octave.Low && currentOctave == Octave.High)
			{
				base.CurrentOctave = Octave.Low;
			}
		}

		protected override void PressKey(GuildWarsControls key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
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
				MusicianModule.ModuleInstance.MusicPlayer.PlaySound(_soundRepository.Get(key, base.CurrentOctave), loops: true);
				break;
			case 8:
				if (base.CurrentOctave == Octave.Low)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
				break;
			case 9:
				MusicianModule.ModuleInstance.MusicPlayer.StopSound();
				break;
			}
		}

		public override void Dispose()
		{
		}
	}
}
