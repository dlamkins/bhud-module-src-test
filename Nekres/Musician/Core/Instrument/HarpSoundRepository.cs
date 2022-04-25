using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class HarpSoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"C3"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"D3"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"E3"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"F3"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"G3"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"A3"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"B3"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.Middle}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Middle}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Middle}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Middle}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Middle}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Middle}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Middle}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Middle}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.High}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.High}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.High}",
				"C6"
			}
		};

		private Dictionary<string, SoundEffectInstance> _sound;

		public async Task<ISoundRepository> Initialize()
		{
			return await Task.Run(delegate
			{
				_sound = new Dictionary<string, SoundEffectInstance>
				{
					{
						"C3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\C3.wav").CreateInstance()
					},
					{
						"D3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\D3.wav").CreateInstance()
					},
					{
						"E3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\E3.wav").CreateInstance()
					},
					{
						"F3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\F3.wav").CreateInstance()
					},
					{
						"G3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\G3.wav").CreateInstance()
					},
					{
						"A3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\A3.wav").CreateInstance()
					},
					{
						"B3",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\B3.wav").CreateInstance()
					},
					{
						"C4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\C4.wav").CreateInstance()
					},
					{
						"D4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\D4.wav").CreateInstance()
					},
					{
						"E4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\E4.wav").CreateInstance()
					},
					{
						"F4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\F4.wav").CreateInstance()
					},
					{
						"G4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\G4.wav").CreateInstance()
					},
					{
						"A4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\A4.wav").CreateInstance()
					},
					{
						"B4",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\B4.wav").CreateInstance()
					},
					{
						"C5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\C5.wav").CreateInstance()
					},
					{
						"D5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\D5.wav").CreateInstance()
					},
					{
						"E5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\E5.wav").CreateInstance()
					},
					{
						"F5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\F5.wav").CreateInstance()
					},
					{
						"G5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\G5.wav").CreateInstance()
					},
					{
						"A5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\A5.wav").CreateInstance()
					},
					{
						"B5",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\B5.wav").CreateInstance()
					},
					{
						"C6",
						MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Harp\\C6.wav").CreateInstance()
					}
				};
				return this;
			});
		}

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
	}
}
