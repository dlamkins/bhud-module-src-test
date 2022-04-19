using System;
using Blish_HUD.Controls.Intern;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class BassPreview : IInstrumentPreview, IDisposable
	{
		private BassNote.Octaves _octave = BassNote.Octaves.Low;

		private BassSoundRepository _soundRepository = new BassSoundRepository();

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
			case BassNote.Octaves.Low:
				_octave = BassNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BassNote.Octaves.None:
			case BassNote.Octaves.High:
				break;
			}
		}

		private void DecreaseOctave()
		{
			switch (_octave)
			{
			case BassNote.Octaves.High:
				_octave = BassNote.Octaves.Low;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BassNote.Octaves.None:
			case BassNote.Octaves.Low:
				break;
			}
		}

		public void Dispose()
		{
			_soundRepository?.Dispose();
		}
	}
}
