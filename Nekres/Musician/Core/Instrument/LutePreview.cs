using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	internal class LutePreview : InstrumentBase
	{
		private readonly ISoundRepository _soundRepository;

		public LutePreview(ISoundRepository soundRepo)
		{
			base.CurrentOctave = Octave.Middle;
			_soundRepository = soundRepo;
		}

		protected override NoteBase ConvertNote(RealNote note)
		{
			return LuteNote.From(note);
		}

		protected override NoteBase OptimizeNote(NoteBase note)
		{
			if (note.Equals(new LuteNote((GuildWarsControls)2, Octave.High)) && base.CurrentOctave == Octave.Middle)
			{
				note = new LuteNote((GuildWarsControls)9, Octave.Middle);
			}
			else if (note.Equals(new LuteNote((GuildWarsControls)9, Octave.Middle)) && base.CurrentOctave == Octave.High)
			{
				note = new LuteNote((GuildWarsControls)2, Octave.High);
			}
			else if (note.Equals(new LuteNote((GuildWarsControls)2, Octave.Middle)) && base.CurrentOctave == Octave.Low)
			{
				note = new LuteNote((GuildWarsControls)9, Octave.Low);
			}
			else if (note.Equals(new LuteNote((GuildWarsControls)9, Octave.Low)) && base.CurrentOctave == Octave.Middle)
			{
				note = new LuteNote((GuildWarsControls)2, Octave.Middle);
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
			case Octave.Low:
				break;
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
				MusicianModule.ModuleInstance.MusicPlayer.PlaySound(_soundRepository.Get(key, base.CurrentOctave));
				break;
			case 8:
				DecreaseOctave();
				break;
			case 9:
				IncreaseOctave();
				break;
			}
		}

		public override void Dispose()
		{
		}
	}
}
