using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class LuteSoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{LuteNote.Octaves.Low}",
				"C3"
			},
			{
				$"{(object)(GuildWarsControls)3}{LuteNote.Octaves.Low}",
				"D3"
			},
			{
				$"{(object)(GuildWarsControls)4}{LuteNote.Octaves.Low}",
				"E3"
			},
			{
				$"{(object)(GuildWarsControls)5}{LuteNote.Octaves.Low}",
				"F3"
			},
			{
				$"{(object)(GuildWarsControls)6}{LuteNote.Octaves.Low}",
				"G3"
			},
			{
				$"{(object)(GuildWarsControls)7}{LuteNote.Octaves.Low}",
				"A3"
			},
			{
				$"{(object)(GuildWarsControls)8}{LuteNote.Octaves.Low}",
				"B3"
			},
			{
				$"{(object)(GuildWarsControls)9}{LuteNote.Octaves.Low}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)2}{LuteNote.Octaves.Middle}",
				"C4"
			},
			{
				$"{(object)(GuildWarsControls)3}{LuteNote.Octaves.Middle}",
				"D4"
			},
			{
				$"{(object)(GuildWarsControls)4}{LuteNote.Octaves.Middle}",
				"E4"
			},
			{
				$"{(object)(GuildWarsControls)5}{LuteNote.Octaves.Middle}",
				"F4"
			},
			{
				$"{(object)(GuildWarsControls)6}{LuteNote.Octaves.Middle}",
				"G4"
			},
			{
				$"{(object)(GuildWarsControls)7}{LuteNote.Octaves.Middle}",
				"A4"
			},
			{
				$"{(object)(GuildWarsControls)8}{LuteNote.Octaves.Middle}",
				"B4"
			},
			{
				$"{(object)(GuildWarsControls)9}{LuteNote.Octaves.Middle}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)2}{LuteNote.Octaves.High}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)3}{LuteNote.Octaves.High}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)4}{LuteNote.Octaves.High}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)5}{LuteNote.Octaves.High}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)6}{LuteNote.Octaves.High}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)7}{LuteNote.Octaves.High}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)8}{LuteNote.Octaves.High}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)9}{LuteNote.Octaves.High}",
				"C6"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"C3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\C3.ogg"))
			},
			{
				"D3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\D3.ogg"))
			},
			{
				"E3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\E3.ogg"))
			},
			{
				"F3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\F3.ogg"))
			},
			{
				"G3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\G3.ogg"))
			},
			{
				"A3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\A3.ogg"))
			},
			{
				"B3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\B3.ogg"))
			},
			{
				"C4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\C4.ogg"))
			},
			{
				"D4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\D4.ogg"))
			},
			{
				"E4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\E4.ogg"))
			},
			{
				"F4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\F4.ogg"))
			},
			{
				"G4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\G4.ogg"))
			},
			{
				"A4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\A4.ogg"))
			},
			{
				"B4",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\B4.ogg"))
			},
			{
				"C5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\C5.ogg"))
			},
			{
				"D5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\D5.ogg"))
			},
			{
				"E5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\E5.ogg"))
			},
			{
				"F5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\F5.ogg"))
			},
			{
				"G5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\G5.ogg"))
			},
			{
				"A5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\A5.ogg"))
			},
			{
				"B5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\B5.ogg"))
			},
			{
				"C6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Lute\\C6.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, LuteNote.Octaves octave)
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
