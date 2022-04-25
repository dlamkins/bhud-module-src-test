using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Bell2SoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.High}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.High}",
				"E6"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.High}",
				"F6"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.High}",
				"G6"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.High}",
				"A6"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.High}",
				"B6"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.High}",
				"C7"
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
							"C5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\C5.wav").CreateInstance()
						},
						{
							"D5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\D5.wav").CreateInstance()
						},
						{
							"E5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\E5.wav").CreateInstance()
						},
						{
							"F5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\F5.wav").CreateInstance()
						},
						{
							"G5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\G5.wav").CreateInstance()
						},
						{
							"A5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\A5.wav").CreateInstance()
						},
						{
							"B5",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\B5.wav").CreateInstance()
						},
						{
							"C6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\C6.wav").CreateInstance()
						},
						{
							"D6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\D6.wav").CreateInstance()
						},
						{
							"E6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\E6.wav").CreateInstance()
						},
						{
							"F6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\F6.wav").CreateInstance()
						},
						{
							"G6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\G6.wav").CreateInstance()
						},
						{
							"A6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\A6.wav").CreateInstance()
						},
						{
							"B6",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\B6.wav").CreateInstance()
						},
						{
							"C7",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bell2\\C7.wav").CreateInstance()
						}
					};
				}
				return this;
			});
		}
	}
}
