using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class FluteSoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{FluteNote.Octaves.Low}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)3}{FluteNote.Octaves.Low}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)4}{FluteNote.Octaves.Low}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)5}{FluteNote.Octaves.Low}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)6}{FluteNote.Octaves.Low}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)7}{FluteNote.Octaves.Low}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)8}{FluteNote.Octaves.Low}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)9}{FluteNote.Octaves.Low}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)2}{FluteNote.Octaves.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)3}{FluteNote.Octaves.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)4}{FluteNote.Octaves.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)5}{FluteNote.Octaves.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)6}{FluteNote.Octaves.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)7}{FluteNote.Octaves.High}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)8}{FluteNote.Octaves.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)9}{FluteNote.Octaves.High}",
				"E6"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"E4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\E4.ogg"))
			},
			{
				"F4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\F4.ogg"))
			},
			{
				"G4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\G4.ogg"))
			},
			{
				"A4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\A4.ogg"))
			},
			{
				"B4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\B4.ogg"))
			},
			{
				"C5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\C5.ogg"))
			},
			{
				"D5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\D5.ogg"))
			},
			{
				"E5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\E5.ogg"))
			},
			{
				"F5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\F5.ogg"))
			},
			{
				"G5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\G5.ogg"))
			},
			{
				"A5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\A5.ogg"))
			},
			{
				"B5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\B5.ogg"))
			},
			{
				"C6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\C6.ogg"))
			},
			{
				"D6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\D6.ogg"))
			},
			{
				"E6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Flute\\E6.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, FluteNote.Octaves octave)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return Sound[Map[$"{key}{octave}"]];
		}

		public void Dispose()
		{
			Map?.Clear();
			foreach (KeyValuePair<string, OggSource> item in Sound)
			{
				item.Value?.Dispose();
			}
		}
	}
}
