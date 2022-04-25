using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class Bell2Note : NoteBase
	{
		private static readonly Dictionary<string, Bell2Note> Map = new Dictionary<string, Bell2Note>
		{
			{
				$"{Note.C}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)2, Octave.Low)
			},
			{
				$"{Note.D}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)3, Octave.Low)
			},
			{
				$"{Note.E}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)4, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)5, Octave.Low)
			},
			{
				$"{Note.G}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)6, Octave.Low)
			},
			{
				$"{Note.A}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)7, Octave.Low)
			},
			{
				$"{Note.B}{Octave.Middle}",
				new Bell2Note((GuildWarsControls)8, Octave.Low)
			},
			{
				$"{Note.C}{Octave.High}",
				new Bell2Note((GuildWarsControls)2, Octave.High)
			},
			{
				$"{Note.D}{Octave.High}",
				new Bell2Note((GuildWarsControls)3, Octave.High)
			},
			{
				$"{Note.E}{Octave.High}",
				new Bell2Note((GuildWarsControls)4, Octave.High)
			},
			{
				$"{Note.F}{Octave.High}",
				new Bell2Note((GuildWarsControls)5, Octave.High)
			},
			{
				$"{Note.G}{Octave.High}",
				new Bell2Note((GuildWarsControls)6, Octave.High)
			},
			{
				$"{Note.A}{Octave.High}",
				new Bell2Note((GuildWarsControls)7, Octave.High)
			},
			{
				$"{Note.B}{Octave.High}",
				new Bell2Note((GuildWarsControls)8, Octave.High)
			},
			{
				$"{Note.C}{Octave.Highest}",
				new Bell2Note((GuildWarsControls)9, Octave.High)
			}
		};

		public Bell2Note(GuildWarsControls key, Octave octave)
			: base(key, octave)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public static Bell2Note From(RealNote note)
		{
			if (note.Note == Note.Z)
			{
				return new Bell2Note((GuildWarsControls)0, note.Octave);
			}
			return Map[$"{note.Note}{note.Octave}"];
		}
	}
}
