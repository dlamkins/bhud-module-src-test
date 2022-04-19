using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Lute : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<LuteNote.Keys, GuildWarsControls> NoteMap = new Dictionary<LuteNote.Keys, GuildWarsControls>
		{
			{
				LuteNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				LuteNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				LuteNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				LuteNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				LuteNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				LuteNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				LuteNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				LuteNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private LuteNote.Octaves CurrentOctave = LuteNote.Octaves.Low;

		public Lute()
		{
			Preview = new LutePreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			LuteNote luteNote = LuteNote.From(note);
			if (RequiresAction(luteNote))
			{
				if (luteNote.Key == LuteNote.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				luteNote = OptimizeNote(luteNote);
				PressNote(NoteMap[luteNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			LuteNote luteNote = LuteNote.From(note);
			if (!RequiresAction(luteNote))
			{
				return;
			}
			luteNote = OptimizeNote(luteNote);
			while (CurrentOctave != luteNote.Octave)
			{
				if (CurrentOctave < luteNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(LuteNote luteNote)
		{
			return luteNote.Key != LuteNote.Keys.None;
		}

		private LuteNote OptimizeNote(LuteNote note)
		{
			if (note.Equals(new LuteNote(LuteNote.Keys.Note1, LuteNote.Octaves.High)) && CurrentOctave == LuteNote.Octaves.Middle)
			{
				note = new LuteNote(LuteNote.Keys.Note8, LuteNote.Octaves.Middle);
			}
			else if (note.Equals(new LuteNote(LuteNote.Keys.Note8, LuteNote.Octaves.Middle)) && CurrentOctave == LuteNote.Octaves.High)
			{
				note = new LuteNote(LuteNote.Keys.Note1, LuteNote.Octaves.High);
			}
			else if (note.Equals(new LuteNote(LuteNote.Keys.Note1, LuteNote.Octaves.Middle)) && CurrentOctave == LuteNote.Octaves.Low)
			{
				note = new LuteNote(LuteNote.Keys.Note8, LuteNote.Octaves.Low);
			}
			else if (note.Equals(new LuteNote(LuteNote.Keys.Note8, LuteNote.Octaves.Low)) && CurrentOctave == LuteNote.Octaves.Middle)
			{
				note = new LuteNote(LuteNote.Keys.Note1, LuteNote.Octaves.Middle);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case LuteNote.Octaves.Low:
				CurrentOctave = LuteNote.Octaves.Middle;
				break;
			case LuteNote.Octaves.Middle:
				CurrentOctave = LuteNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case LuteNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case LuteNote.Octaves.Middle:
				CurrentOctave = LuteNote.Octaves.Low;
				break;
			case LuteNote.Octaves.High:
				CurrentOctave = LuteNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case LuteNote.Octaves.Low:
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
