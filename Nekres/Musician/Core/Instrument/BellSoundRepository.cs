using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class BellSoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.Middle}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Middle}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Middle}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Middle}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Middle}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Middle}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Middle}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Middle}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.High}",
				"E6"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.High}",
				"F6"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.High}",
				"G6"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.High}",
				"A6"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.High}",
				"B6"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.High}",
				"C7"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.High}",
				"D7"
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
				if (_sound == null)
				{
					_sound = new Dictionary<string, SoundEffectInstance>
					{
						{
							"D4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\D4.wav").CreateInstance()
						},
						{
							"E4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\E4.wav").CreateInstance()
						},
						{
							"F4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\F4.wav").CreateInstance()
						},
						{
							"G4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\G4.wav").CreateInstance()
						},
						{
							"A4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\A4.wav").CreateInstance()
						},
						{
							"B4",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\B4.wav").CreateInstance()
						},
						{
							"C5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\C5.wav").CreateInstance()
						},
						{
							"D5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\D5.wav").CreateInstance()
						},
						{
							"E5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\E5.wav").CreateInstance()
						},
						{
							"F5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\F5.wav").CreateInstance()
						},
						{
							"G5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\G5.wav").CreateInstance()
						},
						{
							"A5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\A5.wav").CreateInstance()
						},
						{
							"B5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\B5.wav").CreateInstance()
						},
						{
							"C6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\C6.wav").CreateInstance()
						},
						{
							"D6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\D6.wav").CreateInstance()
						},
						{
							"E6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\E6.wav").CreateInstance()
						},
						{
							"F6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\F6.wav").CreateInstance()
						},
						{
							"G6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\G6.wav").CreateInstance()
						},
						{
							"A6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\A6.wav").CreateInstance()
						},
						{
							"B6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\B6.wav").CreateInstance()
						},
						{
							"C7",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\C7.wav").CreateInstance()
						},
						{
							"D7",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell\\D7.wav").CreateInstance()
						}
					};
				}
				return this;
			});
		}
	}
}
