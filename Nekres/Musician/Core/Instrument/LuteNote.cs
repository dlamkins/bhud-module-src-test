using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class LuteNote : NoteBase
	{
		private static readonly Dictionary<string, LuteNote> Map = new Dictionary<string, LuteNote>
		{
			{
				$"{Note.C}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)2, Octave.Low)
			},
			{
				$"{Note.D}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)3, Octave.Low)
			},
			{
				$"{Note.E}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)4, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)5, Octave.Low)
			},
			{
				$"{Note.G}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)6, Octave.Low)
			},
			{
				$"{Note.A}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)7, Octave.Low)
			},
			{
				$"{Note.B}{Octave.Lowest}",
				new LuteNote((GuildWarsControls)8, Octave.Low)
			},
			{
				$"{Note.C}{Octave.Low}",
				new LuteNote((GuildWarsControls)2, Octave.Middle)
			},
			{
				$"{Note.D}{Octave.Low}",
				new LuteNote((GuildWarsControls)3, Octave.Middle)
			},
			{
				$"{Note.E}{Octave.Low}",
				new LuteNote((GuildWarsControls)4, Octave.Middle)
			},
			{
				$"{Note.F}{Octave.Low}",
				new LuteNote((GuildWarsControls)5, Octave.Middle)
			},
			{
				$"{Note.G}{Octave.Low}",
				new LuteNote((GuildWarsControls)6, Octave.Middle)
			},
			{
				$"{Note.A}{Octave.Low}",
				new LuteNote((GuildWarsControls)7, Octave.Middle)
			},
			{
				$"{Note.B}{Octave.Low}",
				new LuteNote((GuildWarsControls)8, Octave.Middle)
			},
			{
				$"{Note.C}{Octave.Middle}",
				new LuteNote((GuildWarsControls)2, Octave.High)
			},
			{
				$"{Note.D}{Octave.Middle}",
				new LuteNote((GuildWarsControls)3, Octave.High)
			},
			{
				$"{Note.E}{Octave.Middle}",
				new LuteNote((GuildWarsControls)4, Octave.High)
			},
			{
				$"{Note.F}{Octave.Middle}",
				new LuteNote((GuildWarsControls)5, Octave.High)
			},
			{
				$"{Note.G}{Octave.Middle}",
				new LuteNote((GuildWarsControls)6, Octave.High)
			},
			{
				$"{Note.A}{Octave.Middle}",
				new LuteNote((GuildWarsControls)7, Octave.High)
			},
			{
				$"{Note.B}{Octave.Middle}",
				new LuteNote((GuildWarsControls)8, Octave.High)
			},
			{
				$"{Note.C}{Octave.High}",
				new LuteNote((GuildWarsControls)9, Octave.High)
			}
		};

		public LuteNote(GuildWarsControls key, Octave octave)
			: base(key, octave)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public static LuteNote From(RealNote note)
		{
			if (note.Note == Note.Z)
			{
				return new LuteNote((GuildWarsControls)0, note.Octave);
			}
			return Map[$"{note.Note}{note.Octave}"];
		}
	}
}
