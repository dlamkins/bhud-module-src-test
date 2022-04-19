using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Bell2 : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<Bell2Note.Keys, GuildWarsControls> NoteMap = new Dictionary<Bell2Note.Keys, GuildWarsControls>
		{
			{
				Bell2Note.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				Bell2Note.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				Bell2Note.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				Bell2Note.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				Bell2Note.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				Bell2Note.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				Bell2Note.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				Bell2Note.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private Bell2Note.Octaves CurrentOctave = Bell2Note.Octaves.Low;

		public Bell2()
		{
			Preview = new Bell2Preview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Bell2Note bell2Note = Bell2Note.From(note);
			if (RequiresAction(bell2Note))
			{
				if (bell2Note.Key == Bell2Note.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				bell2Note = OptimizeNote(bell2Note);
				PressNote(NoteMap[bell2Note.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			Bell2Note bell2Note = Bell2Note.From(note);
			if (!RequiresAction(bell2Note))
			{
				return;
			}
			bell2Note = OptimizeNote(bell2Note);
			while (CurrentOctave != bell2Note.Octave)
			{
				if (CurrentOctave < bell2Note.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(Bell2Note bell2Note)
		{
			return bell2Note.Key != Bell2Note.Keys.None;
		}

		private Bell2Note OptimizeNote(Bell2Note note)
		{
			if (note.Equals(new Bell2Note(Bell2Note.Keys.Note1, Bell2Note.Octaves.High)) && CurrentOctave == Bell2Note.Octaves.Low)
			{
				note = new Bell2Note(Bell2Note.Keys.Note8, Bell2Note.Octaves.Low);
			}
			else if (note.Equals(new Bell2Note(Bell2Note.Keys.Note8, Bell2Note.Octaves.Low)) && CurrentOctave == Bell2Note.Octaves.High)
			{
				note = new Bell2Note(Bell2Note.Keys.Note1, Bell2Note.Octaves.High);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case Bell2Note.Octaves.Low:
				CurrentOctave = Bell2Note.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Bell2Note.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case Bell2Note.Octaves.High:
				CurrentOctave = Bell2Note.Octaves.Low;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Bell2Note.Octaves.Low:
				break;
			}
			PressKey((GuildWarsControls)10, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void PressNote(GuildWarsControls key)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			PressKey(key, CurrentOctave.ToString());
			Thread.Sleep(NoteTimeout);
		}

		public override void Dispose()
		{
			Preview?.Dispose();
		}
	}
}
