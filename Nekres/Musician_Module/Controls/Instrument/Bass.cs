using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Bass : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<BassNote.Keys, GuildWarsControls> NoteMap = new Dictionary<BassNote.Keys, GuildWarsControls>
		{
			{
				BassNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				BassNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				BassNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				BassNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				BassNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				BassNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				BassNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				BassNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private BassNote.Octaves CurrentOctave = BassNote.Octaves.Low;

		public Bass()
		{
			Preview = new BassPreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			BassNote bassNote = BassNote.From(note);
			if (RequiresAction(bassNote))
			{
				if (bassNote.Key == BassNote.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				bassNote = OptimizeNote(bassNote);
				PressNote(NoteMap[bassNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			BassNote bassNote = BassNote.From(note);
			if (!RequiresAction(bassNote))
			{
				return;
			}
			bassNote = OptimizeNote(bassNote);
			while (CurrentOctave != bassNote.Octave)
			{
				if (CurrentOctave < bassNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(BassNote bassNote)
		{
			return bassNote.Key != BassNote.Keys.None;
		}

		private BassNote OptimizeNote(BassNote note)
		{
			if (note.Equals(new BassNote(BassNote.Keys.Note1, BassNote.Octaves.High)) && CurrentOctave == BassNote.Octaves.Low)
			{
				note = new BassNote(BassNote.Keys.Note8, BassNote.Octaves.Low);
			}
			else if (note.Equals(new BassNote(BassNote.Keys.Note8, BassNote.Octaves.Low)) && CurrentOctave == BassNote.Octaves.High)
			{
				note = new BassNote(BassNote.Keys.Note1, BassNote.Octaves.High);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case BassNote.Octaves.Low:
				CurrentOctave = BassNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BassNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case BassNote.Octaves.High:
				CurrentOctave = BassNote.Octaves.Low;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case BassNote.Octaves.Low:
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
