using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public class FluteNote : NoteBase
	{
		private static readonly Dictionary<string, FluteNote> Map = new Dictionary<string, FluteNote>
		{
			{
				$"{Note.E}{Octave.Low}",
				new FluteNote((GuildWarsControls)2, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Low}",
				new FluteNote((GuildWarsControls)3, Octave.Low)
			},
			{
				$"{Note.G}{Octave.Low}",
				new FluteNote((GuildWarsControls)4, Octave.Low)
			},
			{
				$"{Note.A}{Octave.Low}",
				new FluteNote((GuildWarsControls)5, Octave.Low)
			},
			{
				$"{Note.B}{Octave.Low}",
				new FluteNote((GuildWarsControls)6, Octave.Low)
			},
			{
				$"{Note.C}{Octave.Middle}",
				new FluteNote((GuildWarsControls)7, Octave.Low)
			},
			{
				$"{Note.D}{Octave.Middle}",
				new FluteNote((GuildWarsControls)8, Octave.Low)
			},
			{
				$"{Note.E}{Octave.Middle}",
				new FluteNote((GuildWarsControls)9, Octave.Low)
			},
			{
				$"{Note.F}{Octave.Middle}",
				new FluteNote((GuildWarsControls)3, Octave.High)
			},
			{
				$"{Note.G}{Octave.Middle}",
				new FluteNote((GuildWarsControls)4, Octave.High)
			},
			{
				$"{Note.A}{Octave.Middle}",
				new FluteNote((GuildWarsControls)5, Octave.High)
			},
			{
				$"{Note.B}{Octave.Middle}",
				new FluteNote((GuildWarsControls)6, Octave.High)
			},
			{
				$"{Note.C}{Octave.High}",
				new FluteNote((GuildWarsControls)7, Octave.High)
			},
			{
				$"{Note.D}{Octave.High}",
				new FluteNote((GuildWarsControls)8, Octave.High)
			},
			{
				$"{Note.E}{Octave.High}",
				new FluteNote((GuildWarsControls)9, Octave.High)
			}
		};

		public FluteNote(GuildWarsControls key, Octave octave)
			: base(key, octave)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public static FluteNote From(RealNote note)
		{
			if (note.Note == Note.Z)
			{
				return new FluteNote((GuildWarsControls)0, note.Octave);
			}
			return Map[$"{note.Note}{note.Octave}"];
		}
	}
}
