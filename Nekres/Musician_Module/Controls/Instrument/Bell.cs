using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Bell : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<BellNote.Keys, GuildWarsControls> NoteMap = new Dictionary<BellNote.Keys, GuildWarsControls>
		{
			{
				BellNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				BellNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				BellNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				BellNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				BellNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				BellNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				BellNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				BellNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private BellNote.Octaves CurrentOctave = BellNote.Octaves.Low;

		public Bell()
		{
			Preview = new BellPreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			BellNote bellNote = BellNote.From(note);
			if (RequiresAction(bellNote))
			{
				if (bellNote.Key == BellNote.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				bellNote = OptimizeNote(bellNote);
				PressNote(NoteMap[bellNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			BellNote bellNote = BellNote.From(note);
			if (!RequiresAction(bellNote))
			{
				return;
			}
			bellNote = OptimizeNote(bellNote);
			while (CurrentOctave != bellNote.Octave)
			{
				if (CurrentOctave < bellNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(BellNote bellNote)
		{
			return bellNote.Key != BellNote.Keys.None;
		}

		private BellNote OptimizeNote(BellNote note)
		{
			if (note.Equals(new BellNote(BellNote.Keys.Note1, BellNote.Octaves.High)) && CurrentOctave == BellNote.Octaves.Middle)
			{
				note = new BellNote(BellNote.Keys.Note8, BellNote.Octaves.Middle);
			}
			else if (note.Equals(new BellNote(BellNote.Keys.Note8, BellNote.Octaves.Middle)) && CurrentOctave == BellNote.Octaves.High)
			{
				note = new BellNote(BellNote.Keys.Note1, BellNote.Octaves.High);
			}
			else if (note.Equals(new BellNote(BellNote.Keys.Note1, BellNote.Octaves.Middle)) && CurrentOctave == BellNote.Octaves.Low)
			{
				note = new BellNote(BellNote.Keys.Note8, BellNote.Octaves.Low);
			}
			else if (note.Equals(new BellNote(BellNote.Keys.Note8, BellNote.Octaves.Low)) && CurrentOctave == BellNote.Octaves.Middle)
			{
				note = new BellNote(BellNote.Keys.Note1, BellNote.Octaves.Middle);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case BellNote.Octaves.Low:
				CurrentOctave = BellNote.Octaves.Middle;
				break;
			case BellNote.Octaves.Middle:
				CurrentOctave = BellNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BellNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case BellNote.Octaves.Middle:
				CurrentOctave = BellNote.Octaves.Low;
				break;
			case BellNote.Octaves.High:
				CurrentOctave = BellNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BellNote.Octaves.Low:
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
