using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class BellNote : NoteBase
	{
		private static readonly Dictionary<string, BellNote> Map = new Dictionary<string, BellNote>
		{
			{
				$"{Note.D}{Octave.Low}",
				new BellNote((GuildWarsControls)2, Octave.Low)
			},
			{
				$"{Note.E}{Octave.Low}",
				new BellNote((GuildWarsControls)3, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Low}",
				new BellNote((GuildWarsControls)4, Octave.Low)
			},
			{
				$"{Note.G}{Octave.Low}",
				new BellNote((GuildWarsControls)5, Octave.Low)
			},
			{
				$"{Note.A}{Octave.Low}",
				new BellNote((GuildWarsControls)6, Octave.Low)
			},
			{
				$"{Note.B}{Octave.Low}",
				new BellNote((GuildWarsControls)7, Octave.Low)
			},
			{
				$"{Note.C}{Octave.Middle}",
				new BellNote((GuildWarsControls)8, Octave.Low)
			},
			{
				$"{Note.D}{Octave.Middle}",
				new BellNote((GuildWarsControls)2, Octave.Middle)
			},
			{
				$"{Note.E}{Octave.Middle}",
				new BellNote((GuildWarsControls)3, Octave.Middle)
			},
			{
				$"{Note.F}{Octave.Middle}",
				new BellNote((GuildWarsControls)4, Octave.Middle)
			},
			{
				$"{Note.G}{Octave.Middle}",
				new BellNote((GuildWarsControls)5, Octave.Middle)
			},
			{
				$"{Note.A}{Octave.Middle}",
				new BellNote((GuildWarsControls)6, Octave.Middle)
			},
			{
				$"{Note.B}{Octave.Middle}",
				new BellNote((GuildWarsControls)7, Octave.Middle)
			},
			{
				$"{Note.C}{Octave.High}",
				new BellNote((GuildWarsControls)8, Octave.Middle)
			},
			{
				$"{Note.D}{Octave.High}",
				new BellNote((GuildWarsControls)2, Octave.High)
			},
			{
				$"{Note.E}{Octave.High}",
				new BellNote((GuildWarsControls)3, Octave.High)
			},
			{
				$"{Note.F}{Octave.High}",
				new BellNote((GuildWarsControls)4, Octave.High)
			},
			{
				$"{Note.G}{Octave.High}",
				new BellNote((GuildWarsControls)5, Octave.High)
			},
			{
				$"{Note.A}{Octave.High}",
				new BellNote((GuildWarsControls)6, Octave.High)
			},
			{
				$"{Note.B}{Octave.High}",
				new BellNote((GuildWarsControls)7, Octave.High)
			},
			{
				$"{Note.C}{Octave.Highest}",
				new BellNote((GuildWarsControls)8, Octave.High)
			},
			{
				$"{Note.D}{Octave.Highest}",
				new BellNote((GuildWarsControls)9, Octave.High)
			}
		};

		public BellNote(GuildWarsControls key, Octave octave)
			: base(key, octave)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public static BellNote From(RealNote note)
		{
			if (note.Note == Note.Z)
			{
				return new BellNote((GuildWarsControls)0, note.Octave);
			}
			return Map[$"{note.Note}{note.Octave}"];
		}
	}
}
