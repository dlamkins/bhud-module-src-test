using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class HornSoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{HornNote.Octaves.Low}",
				"E3"
			},
			{
				$"{(object)(GuildWarsControls)3}{HornNote.Octaves.Low}",
				"F3"
			},
			{
				$"{(object)(GuildWarsControls)4}{HornNote.Octaves.Low}",
				"G3"
			},
			{
				$"{(object)(GuildWarsControls)5}{HornNote.Octaves.Low}",
				"A3"
			},
			{
				$"{(object)(GuildWarsControls)6}{HornNote.Octaves.Low}",
				"B3"
			},
			{
				$"{(object)(GuildWarsControls)7}{HornNote.Octaves.Low}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)8}{HornNote.Octaves.Low}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)9}{HornNote.Octaves.Low}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)2}{HornNote.Octaves.Middle}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)3}{HornNote.Octaves.Middle}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)4}{HornNote.Octaves.Middle}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)5}{HornNote.Octaves.Middle}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)6}{HornNote.Octaves.Middle}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)7}{HornNote.Octaves.Middle}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)8}{HornNote.Octaves.Middle}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)9}{HornNote.Octaves.Middle}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)2}{HornNote.Octaves.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)3}{HornNote.Octaves.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)4}{HornNote.Octaves.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)5}{HornNote.Octaves.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)6}{HornNote.Octaves.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)7}{HornNote.Octaves.High}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)8}{HornNote.Octaves.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)9}{HornNote.Octaves.High}",
				"E6"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"E3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\E3.ogg"))
			},
			{
				"F3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\F3.ogg"))
			},
			{
				"G3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\G3.ogg"))
			},
			{
				"A3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\A3.ogg"))
			},
			{
				"B3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\B3.ogg"))
			},
			{
				"C4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\C4.ogg"))
			},
			{
				"D4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\D4.ogg"))
			},
			{
				"E4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\E4.ogg"))
			},
			{
				"F4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\F4.ogg"))
			},
			{
				"G4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\G4.ogg"))
			},
			{
				"A4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\A4.ogg"))
			},
			{
				"B4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\B4.ogg"))
			},
			{
				"C5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\C5.ogg"))
			},
			{
				"D5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\D5.ogg"))
			},
			{
				"E5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\E5.ogg"))
			},
			{
				"F5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\F5.ogg"))
			},
			{
				"G5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\G5.ogg"))
			},
			{
				"A5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\A5.ogg"))
			},
			{
				"B5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\B5.ogg"))
			},
			{
				"C6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\C6.ogg"))
			},
			{
				"D6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\D6.ogg"))
			},
			{
				"E6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Horn\\E6.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, HornNote.Octaves octave)
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
