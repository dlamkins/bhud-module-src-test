using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Horn : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<HornNote.Keys, GuildWarsControls> NoteMap = new Dictionary<HornNote.Keys, GuildWarsControls>
		{
			{
				HornNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				HornNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				HornNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				HornNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				HornNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				HornNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				HornNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				HornNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private HornNote.Octaves CurrentOctave = HornNote.Octaves.Low;

		public Horn()
		{
			Preview = new HornPreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			HornNote hornNote = HornNote.From(note);
			if (RequiresAction(hornNote))
			{
				if (hornNote.Key == HornNote.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				hornNote = OptimizeNote(hornNote);
				PressNote(NoteMap[hornNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			HornNote hornNote = HornNote.From(note);
			if (!RequiresAction(hornNote))
			{
				return;
			}
			hornNote = OptimizeNote(hornNote);
			while (CurrentOctave != hornNote.Octave)
			{
				if (CurrentOctave < hornNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(HornNote hornNote)
		{
			return hornNote.Key != HornNote.Keys.None;
		}

		private HornNote OptimizeNote(HornNote note)
		{
			if (note.Equals(new HornNote(HornNote.Keys.Note1, HornNote.Octaves.High)) && CurrentOctave == HornNote.Octaves.Middle)
			{
				note = new HornNote(HornNote.Keys.Note8, HornNote.Octaves.Middle);
			}
			else if (note.Equals(new HornNote(HornNote.Keys.Note8, HornNote.Octaves.Middle)) && CurrentOctave == HornNote.Octaves.High)
			{
				note = new HornNote(HornNote.Keys.Note1, HornNote.Octaves.High);
			}
			else if (note.Equals(new HornNote(HornNote.Keys.Note1, HornNote.Octaves.Middle)) && CurrentOctave == HornNote.Octaves.Low)
			{
				note = new HornNote(HornNote.Keys.Note8, HornNote.Octaves.Low);
			}
			else if (note.Equals(new HornNote(HornNote.Keys.Note8, HornNote.Octaves.Low)) && CurrentOctave == HornNote.Octaves.Middle)
			{
				note = new HornNote(HornNote.Keys.Note1, HornNote.Octaves.Middle);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case HornNote.Octaves.Low:
				CurrentOctave = HornNote.Octaves.Middle;
				break;
			case HornNote.Octaves.Middle:
				CurrentOctave = HornNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HornNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case HornNote.Octaves.Middle:
				CurrentOctave = HornNote.Octaves.Low;
				break;
			case HornNote.Octaves.High:
				CurrentOctave = HornNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HornNote.Octaves.Low:
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
