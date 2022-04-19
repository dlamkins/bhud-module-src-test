using System;
using System.Collections.Generic;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public abstract class Instrument : IDisposable
	{
		protected static readonly Dictionary<GuildWarsControls, VirtualKeyShort> VirtualKeyShorts = new Dictionary<GuildWarsControls, VirtualKeyShort>
		{
			{
				(GuildWarsControls)2,
				(VirtualKeyShort)49
			},
			{
				(GuildWarsControls)3,
				(VirtualKeyShort)50
			},
			{
				(GuildWarsControls)4,
				(VirtualKeyShort)51
			},
			{
				(GuildWarsControls)5,
				(VirtualKeyShort)52
			},
			{
				(GuildWarsControls)6,
				(VirtualKeyShort)53
			},
			{
				(GuildWarsControls)7,
				(VirtualKeyShort)54
			},
			{
				(GuildWarsControls)8,
				(VirtualKeyShort)55
			},
			{
				(GuildWarsControls)9,
				(VirtualKeyShort)56
			},
			{
				(GuildWarsControls)10,
				(VirtualKeyShort)57
			},
			{
				(GuildWarsControls)11,
				(VirtualKeyShort)48
			}
		};

		protected IInstrumentPreview Preview;

		public InstrumentMode Mode { get; set; }

		public bool IsInstrument(string instrument)
		{
			return string.Equals(GetType().Name, instrument, StringComparison.OrdinalIgnoreCase);
		}

		protected virtual void PressKey(GuildWarsControls key, string octave)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (Mode == InstrumentMode.Practice)
			{
				InstrumentSkillType noteType = (((int)key == 10) ? InstrumentSkillType.DecreaseOctave : (((int)key != 11) ? InstrumentSkillType.Note : InstrumentSkillType.IncreaseOctave));
				MusicianModule.ModuleInstance.Conveyor.SpawnNoteBlock(key, noteType, Note.OctaveColors[octave]);
			}
			else if (Mode == InstrumentMode.Emulate)
			{
				Keyboard.Press(VirtualKeyShorts[key], false);
				Keyboard.Release(VirtualKeyShorts[key], false);
			}
			else if (Mode == InstrumentMode.Preview)
			{
				Preview.PlaySoundByKey(key);
			}
		}

		public abstract void PlayNote(Note note);

		public abstract void GoToOctave(Note note);

		public abstract void Dispose();
	}
}
