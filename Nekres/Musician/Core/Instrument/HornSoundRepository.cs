using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class HornSoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"E3"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"F3"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"G3"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"A3"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"B3"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.Middle}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Middle}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Middle}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Middle}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Middle}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Middle}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Middle}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Middle}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.High}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.High}",
				"E6"
			}
		};

		private Dictionary<string, SoundEffectInstance> _sound;

		public SoundEffectInstance Get(GuildWarsControls key, Octave octave)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return _sound[_map[$"{key}{octave}"]];
		}

		public void Dispose()
		{
			_map?.Clear();
			if (_sound == null)
			{
				return;
			}
			foreach (KeyValuePair<string, SoundEffectInstance> item in _sound)
			{
				item.Value?.Dispose();
			}
		}

		public async Task<ISoundRepository> Initialize()
		{
			return await Task.Run(delegate
			{
				_sound = new Dictionary<string, SoundEffectInstance>
				{
					{
						"E3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\E3.wav").CreateInstance()
					},
					{
						"F3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\F3.wav").CreateInstance()
					},
					{
						"G3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\G3.wav").CreateInstance()
					},
					{
						"A3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\A3.wav").CreateInstance()
					},
					{
						"B3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\B3.wav").CreateInstance()
					},
					{
						"C4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\C4.wav").CreateInstance()
					},
					{
						"D4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\D4.wav").CreateInstance()
					},
					{
						"E4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\E4.wav").CreateInstance()
					},
					{
						"F4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\F4.wav").CreateInstance()
					},
					{
						"G4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\G4.wav").CreateInstance()
					},
					{
						"A4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\A4.wav").CreateInstance()
					},
					{
						"B4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\B4.wav").CreateInstance()
					},
					{
						"C5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\C5.wav").CreateInstance()
					},
					{
						"D5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\D5.wav").CreateInstance()
					},
					{
						"E5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\E5.wav").CreateInstance()
					},
					{
						"F5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\F5.wav").CreateInstance()
					},
					{
						"G5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\G5.wav").CreateInstance()
					},
					{
						"A5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\A5.wav").CreateInstance()
					},
					{
						"B5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\B5.wav").CreateInstance()
					},
					{
						"C6",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\C6.wav").CreateInstance()
					},
					{
						"D6",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\D6.wav").CreateInstance()
					},
					{
						"E6",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Horn\\E6.wav").CreateInstance()
					}
				};
				return this;
			});
		}
	}
}
