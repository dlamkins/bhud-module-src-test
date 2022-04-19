using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class HarpSoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{HarpNote.Octaves.Low}",
				"C3"
			},
			{
				$"{(object)(GuildWarsControls)3}{HarpNote.Octaves.Low}",
				"D3"
			},
			{
				$"{(object)(GuildWarsControls)4}{HarpNote.Octaves.Low}",
				"E3"
			},
			{
				$"{(object)(GuildWarsControls)5}{HarpNote.Octaves.Low}",
				"F3"
			},
			{
				$"{(object)(GuildWarsControls)6}{HarpNote.Octaves.Low}",
				"G3"
			},
			{
				$"{(object)(GuildWarsControls)7}{HarpNote.Octaves.Low}",
				"A3"
			},
			{
				$"{(object)(GuildWarsControls)8}{HarpNote.Octaves.Low}",
				"B3"
			},
			{
				$"{(object)(GuildWarsControls)9}{HarpNote.Octaves.Low}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)2}{HarpNote.Octaves.Middle}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)3}{HarpNote.Octaves.Middle}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)4}{HarpNote.Octaves.Middle}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)5}{HarpNote.Octaves.Middle}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)6}{HarpNote.Octaves.Middle}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)7}{HarpNote.Octaves.Middle}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)8}{HarpNote.Octaves.Middle}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)9}{HarpNote.Octaves.Middle}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)2}{HarpNote.Octaves.High}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)3}{HarpNote.Octaves.High}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)4}{HarpNote.Octaves.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)5}{HarpNote.Octaves.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)6}{HarpNote.Octaves.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)7}{HarpNote.Octaves.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)8}{HarpNote.Octaves.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)9}{HarpNote.Octaves.High}",
				"C6"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"C3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\C3.ogg"))
			},
			{
				"D3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\D3.ogg"))
			},
			{
				"E3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\E3.ogg"))
			},
			{
				"F3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\F3.ogg"))
			},
			{
				"G3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\G3.ogg"))
			},
			{
				"A3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\A3.ogg"))
			},
			{
				"B3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\B3.ogg"))
			},
			{
				"C4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\C4.ogg"))
			},
			{
				"D4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\D4.ogg"))
			},
			{
				"E4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\E4.ogg"))
			},
			{
				"F4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\F4.ogg"))
			},
			{
				"G4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\G4.ogg"))
			},
			{
				"A4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\A4.ogg"))
			},
			{
				"B4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\B4.ogg"))
			},
			{
				"C5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\C5.ogg"))
			},
			{
				"D5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\D5.ogg"))
			},
			{
				"E5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\E5.ogg"))
			},
			{
				"F5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\F5.ogg"))
			},
			{
				"G5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\G5.ogg"))
			},
			{
				"A5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\A5.ogg"))
			},
			{
				"B5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\B5.ogg"))
			},
			{
				"C6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Harp\\C6.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, HarpNote.Octaves octave)
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
