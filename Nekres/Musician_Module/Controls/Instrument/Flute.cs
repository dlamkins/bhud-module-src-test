using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public class Flute : Instrument
	{
		private readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		private readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		private readonly Dictionary<FluteNote.Keys, GuildWarsControls> NoteMap = new Dictionary<FluteNote.Keys, GuildWarsControls>
		{
			{
				FluteNote.Keys.Note1,
				(GuildWarsControls)2
			},
			{
				FluteNote.Keys.Note2,
				(GuildWarsControls)3
			},
			{
				FluteNote.Keys.Note3,
				(GuildWarsControls)4
			},
			{
				FluteNote.Keys.Note4,
				(GuildWarsControls)5
			},
			{
				FluteNote.Keys.Note5,
				(GuildWarsControls)6
			},
			{
				FluteNote.Keys.Note6,
				(GuildWarsControls)7
			},
			{
				FluteNote.Keys.Note7,
				(GuildWarsControls)8
			},
			{
				FluteNote.Keys.Note8,
				(GuildWarsControls)9
			}
		};

		private FluteNote.Octaves CurrentOctave = FluteNote.Octaves.Low;

		public Flute()
		{
			Preview = new FlutePreview();
		}

		public override void PlayNote(Note note)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			FluteNote fluteNote = FluteNote.From(note);
			if (RequiresAction(fluteNote))
			{
				if (fluteNote.Key == FluteNote.Keys.None)
				{
					PressNote((GuildWarsControls)11);
					return;
				}
				fluteNote = OptimizeNote(fluteNote);
				PressNote(NoteMap[fluteNote.Key]);
			}
		}

		public override void GoToOctave(Note note)
		{
			FluteNote fluteNote = FluteNote.From(note);
			if (!RequiresAction(fluteNote))
			{
				return;
			}
			fluteNote = OptimizeNote(fluteNote);
			while (CurrentOctave != fluteNote.Octave)
			{
				if (CurrentOctave < fluteNote.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		private static bool RequiresAction(FluteNote fluteNote)
		{
			return fluteNote.Key != FluteNote.Keys.None;
		}

		private FluteNote OptimizeNote(FluteNote note)
		{
			if (note.Equals(new FluteNote(FluteNote.Keys.Note1, FluteNote.Octaves.High)) && CurrentOctave == FluteNote.Octaves.Low)
			{
				note = new FluteNote(FluteNote.Keys.Note8, FluteNote.Octaves.Low);
			}
			else if (note.Equals(new FluteNote(FluteNote.Keys.Note8, FluteNote.Octaves.Low)) && CurrentOctave == FluteNote.Octaves.High)
			{
				note = new FluteNote(FluteNote.Keys.Note1, FluteNote.Octaves.High);
			}
			return note;
		}

		private void IncreaseOctave()
		{
			switch (CurrentOctave)
			{
			case FluteNote.Octaves.Low:
				CurrentOctave = FluteNote.Octaves.High;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case FluteNote.Octaves.High:
				break;
			}
			PressKey((GuildWarsControls)10, CurrentOctave.ToString());
			Thread.Sleep(OctaveTimeout);
		}

		private void DecreaseOctave()
		{
			switch (CurrentOctave)
			{
			case FluteNote.Octaves.High:
				CurrentOctave = FluteNote.Octaves.Low;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case FluteNote.Octaves.Low:
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

		protected override void PressKey(GuildWarsControls key, string octave)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (base.Mode == InstrumentMode.Practice)
			{
				InstrumentSkillType noteType = (((int)key == 10) ? ((CurrentOctave == FluteNote.Octaves.Low) ? InstrumentSkillType.IncreaseOctave : InstrumentSkillType.DecreaseOctave) : (((int)key != 11) ? InstrumentSkillType.Note : InstrumentSkillType.StopPlaying));
				MusicianModule.ModuleInstance.Conveyor.SpawnNoteBlock(key, noteType, Note.OctaveColors[octave]);
			}
			else if (base.Mode == InstrumentMode.Emulate)
			{
				Keyboard.Press(Instrument.VirtualKeyShorts[key], false);
				Keyboard.Release(Instrument.VirtualKeyShorts[key], false);
			}
			else if (base.Mode == InstrumentMode.Preview)
			{
				Preview.PlaySoundByKey(key);
			}
		}

		public override void Dispose()
		{
			Preview?.Dispose();
		}
	}
}
