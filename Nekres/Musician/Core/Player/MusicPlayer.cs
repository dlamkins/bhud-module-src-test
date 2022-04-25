using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Instrument;
using Nekres.Musician.Core.Models;
using Nekres.Musician.Core.Player.Algorithms;

namespace Nekres.Musician.Core.Player
{
	internal class MusicPlayer : IDisposable
	{
		private readonly Dictionary<Nekres.Musician.Core.Models.Instrument, ISoundRepository> _soundRepositories;

		private PlayAlgorithmBase _algorithm;

		private SoundEffectInstance _activeSfx;

		private Guid _activeMusicSheet;

		private float _audioVolume => MusicianModule.ModuleInstance.audioVolume.get_Value() / 1000f;

		public MusicPlayer()
		{
			_soundRepositories = new Dictionary<Nekres.Musician.Core.Models.Instrument, ISoundRepository>
			{
				{
					Nekres.Musician.Core.Models.Instrument.Bass,
					new BassSoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Bell,
					new BellSoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Bell2,
					new Bell2SoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Flute,
					new FluteSoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Harp,
					new HarpSoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Horn,
					new HornSoundRepository()
				},
				{
					Nekres.Musician.Core.Models.Instrument.Lute,
					new LuteSoundRepository()
				}
			};
		}

		public void Dispose()
		{
			foreach (KeyValuePair<Nekres.Musician.Core.Models.Instrument, ISoundRepository> soundRepository in _soundRepositories)
			{
				soundRepository.Deconstruct(out var _, out var value);
				value.Dispose();
			}
		}

		public void PlaySound(SoundEffectInstance sfx, bool loops = false)
		{
			if (loops)
			{
				StopSound();
				sfx.IsLooped = true;
			}
			_activeSfx = sfx;
			sfx.Volume = _audioVolume;
			sfx.Play();
		}

		public void StopSound()
		{
			_activeSfx?.Stop();
		}

		public bool IsMySongPlaying(Guid id)
		{
			return _activeMusicSheet.Equals(id);
		}

		public async Task PlayPreview(MusicSheet musicSheet)
		{
			Play(musicSheet, await GetInstrumentPreview(musicSheet.Instrument));
		}

		public void PlayEmulate(MusicSheet musicSheet)
		{
			Play(musicSheet, GetInstrumentEmulate(musicSheet.Instrument));
		}

		private void Play(MusicSheet musicSheet, InstrumentBase instrument)
		{
			Stop();
			_algorithm = ((musicSheet.Algorithm == Algorithm.FavorChords) ? ((PlayAlgorithmBase)new FavorChordsAlgorithm(instrument)) : ((PlayAlgorithmBase)new FavorNotesAlgorithm(instrument)));
			new Thread((ThreadStart)delegate
			{
				_algorithm?.Play(musicSheet.Tempo, musicSheet.Melody.ToArray());
			}).Start();
			_activeMusicSheet = musicSheet.Id;
		}

		public void Stop()
		{
			StopSound();
			_activeMusicSheet = Guid.Empty;
			_algorithm?.Dispose();
			_algorithm = null;
		}

		private InstrumentBase GetInstrumentEmulate(Nekres.Musician.Core.Models.Instrument instrument)
		{
			return instrument switch
			{
				Nekres.Musician.Core.Models.Instrument.Bass => new Bass(), 
				Nekres.Musician.Core.Models.Instrument.Bell => new Bell(), 
				Nekres.Musician.Core.Models.Instrument.Bell2 => new Bell2(), 
				Nekres.Musician.Core.Models.Instrument.Flute => new Flute(), 
				Nekres.Musician.Core.Models.Instrument.Harp => new Harp(), 
				Nekres.Musician.Core.Models.Instrument.Horn => new Horn(), 
				Nekres.Musician.Core.Models.Instrument.Lute => new Lute(), 
				_ => null, 
			};
		}

		private async Task<InstrumentBase> GetInstrumentPreview(Nekres.Musician.Core.Models.Instrument instrument)
		{
			return instrument switch
			{
				Nekres.Musician.Core.Models.Instrument.Bass => new BassPreview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Bell => new BellPreview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Bell2 => new Bell2Preview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Flute => new FlutePreview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Harp => new HarpPreview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Horn => new HornPreview(await _soundRepositories[instrument].Initialize()), 
				Nekres.Musician.Core.Models.Instrument.Lute => new LutePreview(await _soundRepositories[instrument].Initialize()), 
				_ => null, 
			};
		}
	}
}
