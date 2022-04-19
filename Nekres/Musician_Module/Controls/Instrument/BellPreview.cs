using System;
using Blish_HUD.Controls.Intern;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class BellPreview : IInstrumentPreview, IDisposable
	{
		private BellNote.Octaves _octave = BellNote.Octaves.Middle;

		private readonly BellSoundRepository _soundRepository = new BellSoundRepository();

		public void PlaySoundByKey(GuildWarsControls key)
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
			case BellNote.Octaves.Low:
				_octave = BellNote.Octaves.Middle;
				break;
			case BellNote.Octaves.Middle:
				_octave = BellNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BellNote.Octaves.None:
			case BellNote.Octaves.High:
				break;
			}
		}

		private void DecreaseOctave()
		{
			switch (_octave)
			{
			case BellNote.Octaves.Middle:
				_octave = BellNote.Octaves.Low;
				break;
			case BellNote.Octaves.High:
				_octave = BellNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BellNote.Octaves.None:
			case BellNote.Octaves.Low:
				break;
			}
		}

		public void Dispose()
		{
			_soundRepository?.Dispose();
		}
	}
}
