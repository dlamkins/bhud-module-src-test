using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Harp : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<HarpNote.Keys, GuildWarsControls> NoteMap = new Dictionary<HarpNote.Keys, GuildWarsControls>
		{
			{
				HarpNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				HarpNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				HarpNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				HarpNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				HarpNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				HarpNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				HarpNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				HarpNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private HarpNote.Octaves CurrentOctave = HarpNote.Octaves.Middle;

		public Harp()
		{
			Preview = new HarpPreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			HarpNote harpNote = HarpNote.From(note);
			if (RequiresAction(harpNote))
			{
				harpNote = OptimizeNote(harpNote);
				PressNote(NoteMap[harpNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			HarpNote harpNote = HarpNote.From(note);
			if (!RequiresAction(harpNote))
			{
				return;
			}
			harpNote = OptimizeNote(harpNote);
			while (CurrentOctave != harpNote.Octave)
			{
				if (CurrentOctave < harpNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(HarpNote harpNote)
		{
			return harpNote.Key != HarpNote.Keys.None;
		}

		private HarpNote OptimizeNote(HarpNote note)
		{
			if (note.Equals(new HarpNote(HarpNote.Keys.Note1, HarpNote.Octaves.Middle)) && CurrentOctave == HarpNote.Octaves.Low)
			{
				note = new HarpNote(HarpNote.Keys.Note8, HarpNote.Octaves.Low);
			}
			else if (note.Equals(new HarpNote(HarpNote.Keys.Note1, HarpNote.Octaves.High)) && CurrentOctave == HarpNote.Octaves.Middle)
			{
				note = new HarpNote(HarpNote.Keys.Note8, HarpNote.Octaves.Middle);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case HarpNote.Octaves.Low:
				CurrentOctave = HarpNote.Octaves.Middle;
				break;
			case HarpNote.Octaves.Middle:
				CurrentOctave = HarpNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HarpNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)11, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case HarpNote.Octaves.Middle:
				CurrentOctave = HarpNote.Octaves.Low;
				break;
			case HarpNote.Octaves.High:
				CurrentOctave = HarpNote.Octaves.Middle;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case HarpNote.Octaves.Low:
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
