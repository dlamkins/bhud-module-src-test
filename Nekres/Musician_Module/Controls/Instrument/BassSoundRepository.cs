using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class BassSoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{FluteNote.Octaves.Low}",
				"C1"
			},
			{
				$"{(object)(GuildWarsControls)3}{FluteNote.Octaves.Low}",
				"D1"
			},
			{
				$"{(object)(GuildWarsControls)4}{FluteNote.Octaves.Low}",
				"E1"
			},
			{
				$"{(object)(GuildWarsControls)5}{FluteNote.Octaves.Low}",
				"F1"
			},
			{
				$"{(object)(GuildWarsControls)6}{FluteNote.Octaves.Low}",
				"G1"
			},
			{
				$"{(object)(GuildWarsControls)7}{FluteNote.Octaves.Low}",
				"A1"
			},
			{
				$"{(object)(GuildWarsControls)8}{FluteNote.Octaves.Low}",
				"B1"
			},
			{
				$"{(object)(GuildWarsControls)9}{FluteNote.Octaves.Low}",
				"C2"
			},
			{
				$"{(object)(GuildWarsControls)2}{FluteNote.Octaves.High}",
				"C2"
			},
			{
				$"{(object)(GuildWarsControls)3}{FluteNote.Octaves.High}",
				"D2"
			},
			{
				$"{(object)(GuildWarsControls)4}{FluteNote.Octaves.High}",
				"E2"
			},
			{
				$"{(object)(GuildWarsControls)5}{FluteNote.Octaves.High}",
				"F2"
			},
			{
				$"{(object)(GuildWarsControls)6}{FluteNote.Octaves.High}",
				"G2"
			},
			{
				$"{(object)(GuildWarsControls)7}{FluteNote.Octaves.High}",
				"A2"
			},
			{
				$"{(object)(GuildWarsControls)8}{FluteNote.Octaves.High}",
				"B2"
			},
			{
				$"{(object)(GuildWarsControls)9}{FluteNote.Octaves.High}",
				"C3"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"C1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\C1.ogg"))
			},
			{
				"D1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\D1.ogg"))
			},
			{
				"E1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\E1.ogg"))
			},
			{
				"F1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\F1.ogg"))
			},
			{
				"G1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\G1.ogg"))
			},
			{
				"A1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\A1.ogg"))
			},
			{
				"B1",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\B1.ogg"))
			},
			{
				"C2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\C2.ogg"))
			},
			{
				"D2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\D2.ogg"))
			},
			{
				"E2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\E2.ogg"))
			},
			{
				"F2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\F2.ogg"))
			},
			{
				"G2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\G2.ogg"))
			},
			{
				"A2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\A2.ogg"))
			},
			{
				"B2",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\B2.ogg"))
			},
			{
				"C3",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bass\\C3.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, BassNote.Octaves octave)
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
