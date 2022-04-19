using System;
using Blish_HUD.Controls.Intern;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class HornPreview : IInstrumentPreview, IDisposable
	{
		private HornNote.Octaves _octave = HornNote.Octaves.Middle;

		private readonly HornSoundRepository _soundRepository = new HornSoundRepository();

		public void PlaySoundByKey(GuildWarsControls key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
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
				MusicianModule.ModuleInstance.MusicPlayer.StopSound();
				MusicianModule.ModuleInstance.MusicPlayer.PlaySound(_soundRepository.Get(key, _octave));
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

		private void IncreaseOctave()
		{
			switch (_octave)
			{
			case HornNote.Octaves.Low:
				_octave = HornNote.Octaves.Middle;
				break;
			case HornNote.Octaves.Middle:
				_octave = HornNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HornNote.Octaves.None:
			case HornNote.Octaves.High:
				break;
			}
		}

		private void DecreaseOctave()
		{
			switch (_octave)
			{
			case HornNote.Octaves.Middle:
				_octave = HornNote.Octaves.Low;
				break;
			case HornNote.Octaves.High:
				_octave = HornNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HornNote.Octaves.None:
			case HornNote.Octaves.Low:
				break;
			}
		}

		public void Dispose()
		{
			_soundRepository?.Dispose();
		}
	}
}
