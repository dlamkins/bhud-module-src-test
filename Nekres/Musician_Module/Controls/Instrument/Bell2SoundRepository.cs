using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using CSCore.Codecs.OGG;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Bell2SoundRepository : IDisposable
	{
		private readonly Dictionary<string, string> Map = new Dictionary<string, string>
		{
			{
				$"{(object)(GuildWarsControls)2}{Bell2Note.Octaves.Low}",
				"C5"
			},
			{
				$"{(object)(GuildWarsControls)3}{Bell2Note.Octaves.Low}",
				"D5"
			},
			{
				$"{(object)(GuildWarsControls)4}{Bell2Note.Octaves.Low}",
				"E5"
			},
			{
				$"{(object)(GuildWarsControls)5}{Bell2Note.Octaves.Low}",
				"F5"
			},
			{
				$"{(object)(GuildWarsControls)6}{Bell2Note.Octaves.Low}",
				"G5"
			},
			{
				$"{(object)(GuildWarsControls)7}{Bell2Note.Octaves.Low}",
				"A5"
			},
			{
				$"{(object)(GuildWarsControls)8}{Bell2Note.Octaves.Low}",
				"B5"
			},
			{
				$"{(object)(GuildWarsControls)9}{Bell2Note.Octaves.Low}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)2}{Bell2Note.Octaves.High}",
				"C6"
			},
			{
				$"{(object)(GuildWarsControls)3}{Bell2Note.Octaves.High}",
				"D6"
			},
			{
				$"{(object)(GuildWarsControls)4}{Bell2Note.Octaves.High}",
				"E6"
			},
			{
				$"{(object)(GuildWarsControls)5}{Bell2Note.Octaves.High}",
				"F6"
			},
			{
				$"{(object)(GuildWarsControls)6}{Bell2Note.Octaves.High}",
				"G6"
			},
			{
				$"{(object)(GuildWarsControls)7}{Bell2Note.Octaves.High}",
				"A6"
			},
			{
				$"{(object)(GuildWarsControls)8}{Bell2Note.Octaves.High}",
				"B6"
			},
			{
				$"{(object)(GuildWarsControls)9}{Bell2Note.Octaves.High}",
				"C7"
			}
		};

		private readonly Dictionary<string, OggSource> Sound = new Dictionary<string, OggSource>
		{
			{
				"C5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\C5.ogg"))
			},
			{
				"D5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\D5.ogg"))
			},
			{
				"E5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\E5.ogg"))
			},
			{
				"F5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\F5.ogg"))
			},
			{
				"G5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\G5.ogg"))
			},
			{
				"A5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\A5.ogg"))
			},
			{
				"B5",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\B5.ogg"))
			},
			{
				"C6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\C6.ogg"))
			},
			{
				"D6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\D6.ogg"))
			},
			{
				"E6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\E6.ogg"))
			},
			{
				"F6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\F6.ogg"))
			},
			{
				"G6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\G6.ogg"))
			},
			{
				"A6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\A6.ogg"))
			},
			{
				"B6",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\B6.ogg"))
			},
			{
				"C7",
				new OggSource(MusicianModule.ModuleInstance.ContentsManager.GetFileStream("instruments\\Bell2\\C7.ogg"))
			}
		};

		public OggSource Get(string id)
		{
			return Sound[id];
		}

		public OggSource Get(GuildWarsControls key, Bell2Note.Octaves octave)
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
