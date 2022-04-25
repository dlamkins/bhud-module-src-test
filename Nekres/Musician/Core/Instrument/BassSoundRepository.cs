using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class BassSoundRepository : ISoundRepository, IDisposable
	{
		private readonly Dictionary<string, string> _map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Octave.Low}",
				"C1"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.Low}",
				"D1"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.Low}",
				"E1"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.Low}",
				"F1"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.Low}",
				"G1"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.Low}",
				"A1"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.Low}",
				"B1"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.Low}",
				"C2"
			},
			{
				$"{(object)(GuildWarsControls)2}{Octave.High}",
				"C2"
			},
			{
				$"{(object)(GuildWarsControls)3}{Octave.High}",
				"D2"
			},
			{
				$"{(object)(GuildWarsControls)4}{Octave.High}",
				"E2"
			},
			{
				$"{(object)(GuildWarsControls)5}{Octave.High}",
				"F2"
			},
			{
				$"{(object)(GuildWarsControls)6}{Octave.High}",
				"G2"
			},
			{
				$"{(object)(GuildWarsControls)7}{Octave.High}",
				"A2"
			},
			{
				$"{(object)(GuildWarsControls)8}{Octave.High}",
				"B2"
			},
			{
				$"{(object)(GuildWarsControls)9}{Octave.High}",
				"C3"
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
							"C1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\C1.wav").CreateInstance()
						},
						{
							"D1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\D1.wav").CreateInstance()
						},
						{
							"E1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\E1.wav").CreateInstance()
						},
						{
							"F1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\F1.wav").CreateInstance()
						},
						{
							"G1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\G1.wav").CreateInstance()
						},
						{
							"A1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\A1.wav").CreateInstance()
						},
						{
							"B1",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\B1.wav").CreateInstance()
						},
						{
							"C2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\C2.wav").CreateInstance()
						},
						{
							"D2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\D2.wav").CreateInstance()
						},
						{
							"E2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\E2.wav").CreateInstance()
						},
						{
							"F2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\F2.wav").CreateInstance()
						},
						{
							"G2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\G2.wav").CreateInstance()
						},
						{
							"A2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\A2.wav").CreateInstance()
						},
						{
							"B2",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\B2.wav").CreateInstance()
						},
						{
							"C3",
							MusicianModule.ModuleInstance.ContentsManager.GetSound("instruments\\Bass\\C3.wav").CreateInstance()
						}
					};
				}
				return this;
			});
		}
	}
}
