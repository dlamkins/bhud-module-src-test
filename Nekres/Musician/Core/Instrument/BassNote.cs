using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class BassNote : NoteBase
	{
		private static readonly Dictionary<string, BassNote> Map = new Dictionary<string, BassNote>
		{
			{
				$"{Note.C}{Octave.Middle}",
				new BassNote((GuildWarsControls)2, Octave.Low)
			},
			{
				$"{Note.D}{Octave.Middle}",
				new BassNote((GuildWarsControls)3, Octave.Low)
			},
			{
				$"{Note.E}{Octave.Middle}",
				new BassNote((GuildWarsControls)4, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Middle}",
				new BassNote((GuildWarsControls)5, Octave.Low)
			},
			{
				$"{Note.G}{Octave.Middle}",
				new BassNote((GuildWarsControls)6, Octave.Low)
			},
			{
				$"{Note.A}{Octave.Middle}",
				new BassNote((GuildWarsControls)7, Octave.Low)
			},
			{
				$"{Note.B}{Octave.Middle}",
				new BassNote((GuildWarsControls)8, Octave.Low)
			},
			{
				$"{Note.C}{Octave.Low}",
				new BassNote((GuildWarsControls)2, Octave.High)
			},
			{
				$"{Note.D}{Octave.Low}",
				new BassNote((GuildWarsControls)3, Octave.High)
			},
			{
				$"{Note.E}{Octave.Low}",
				new BassNote((GuildWarsControls)4, Octave.High)
			},
			{
				$"{Note.F}{Octave.Low}",
				new BassNote((GuildWarsControls)5, Octave.High)
			},
			{
				$"{Note.G}{Octave.Low}",
				new BassNote((GuildWarsControls)6, Octave.High)
			},
			{
				$"{Note.A}{Octave.Low}",
				new BassNote((GuildWarsControls)7, Octave.High)
			},
			{
				$"{Note.B}{Octave.Low}",
				new BassNote((GuildWarsControls)8, Octave.High)
			},
			{
				$"{Note.C}{Octave.Lowest}",
				new BassNote((GuildWarsControls)9, Octave.High)
			}
		};

		public BassNote(GuildWarsControls key, Octave octave)
			: base(key, octave)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public static BassNote From(RealNote note)
		{
			if (note.Note == Note.Z)
			{
				return new BassNote((GuildWarsControls)0, note.Octave);
			}
			return Map[$"{note.Note}{note.Octave}"];
		}
	}
}
