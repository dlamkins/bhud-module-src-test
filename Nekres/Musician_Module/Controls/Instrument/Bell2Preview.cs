using System;
using Blish_HUD.Controls.Intern;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Bell2Preview : IInstrumentPreview, IDisposable
	{
		private Bell2Note.Octaves _octave = Bell2Note.Octaves.Low;

		private readonly Bell2SoundRepository _soundRepository = new Bell2SoundRepository();

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
			case Bell2Note.Octaves.Low:
				_octave = Bell2Note.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Bell2Note.Octaves.None:
			case Bell2Note.Octaves.High:
				break;
			}
		}

		private void DecreaseOctave()
		{
			switch (_octave)
			{
			case Bell2Note.Octaves.High:
				_octave = Bell2Note.Octaves.Low;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Bell2Note.Octaves.None:
			case Bell2Note.Octaves.Low:
				break;
			}
		}

		public void Dispose()
		{
			_soundRepository?.Dispose();
		}
	}
}
