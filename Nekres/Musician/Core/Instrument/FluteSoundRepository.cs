using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class FluteSoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
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
			if (_sound == null)
			{
				return;
			}
			foreach (KeyValuePair<string, SoundEffectInstance> item in _sound)
			{
				SoundEffectInstance value = item.Value;
				if (value != null)
				{
					value.Dispose();
				}
			}
		}

		public async Task<ISoundRepository> Initialize()
		{
			return await Task.Run(delegate
			{
				if (_sound == null)
				{
					_sound = new Dictionary<string, SoundEffectInstance>
					{
						{
							"E4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\E4.wav").CreateInstance()
						},
						{
							"F4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\F4.wav").CreateInstance()
						},
						{
							"G4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\G4.wav").CreateInstance()
						},
						{
							"A4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\A4.wav").CreateInstance()
						},
						{
							"B4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\B4.wav").CreateInstance()
						},
						{
							"C5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\C5.wav").CreateInstance()
						},
						{
							"D5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\D5.wav").CreateInstance()
						},
						{
							"E5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\E5.wav").CreateInstance()
						},
						{
							"F5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\F5.wav").CreateInstance()
						},
						{
							"G5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\G5.wav").CreateInstance()
						},
						{
							"A5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\A5.wav").CreateInstance()
						},
						{
							"B5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\B5.wav").CreateInstance()
						},
						{
							"C6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\C6.wav").CreateInstance()
						},
						{
							"D6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\D6.wav").CreateInstance()
						},
						{
							"E6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Flute\\E6.wav").CreateInstance()
						}
					};
				}
				return this;
			});
		}
	}
}
