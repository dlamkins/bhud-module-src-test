using System;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Input;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public abstract class Instrument : IDisposable
	{
		protected IInstrumentPreview Preview;

		public InstrumentMode Mode { get; set; }

		public Keys GetKeyBinding(GuildWarsControls key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected I4, but got Unknown
			return (key - 1) switch
			{
				0 => MusicianModule.ModuleInstance.keySwapWeapons.get_Value().get_PrimaryKey(), 
				1 => MusicianModule.ModuleInstance.keyWeaponSkill1.get_Value().get_PrimaryKey(), 
				2 => MusicianModule.ModuleInstance.keyWeaponSkill2.get_Value().get_PrimaryKey(), 
				3 => MusicianModule.ModuleInstance.keyWeaponSkill3.get_Value().get_PrimaryKey(), 
				4 => MusicianModule.ModuleInstance.keyWeaponSkill4.get_Value().get_PrimaryKey(), 
				5 => MusicianModule.ModuleInstance.keyWeaponSkill5.get_Value().get_PrimaryKey(), 
				6 => MusicianModule.ModuleInstance.keyHealingSkill.get_Value().get_PrimaryKey(), 
				7 => MusicianModule.ModuleInstance.keyUtilitySkill1.get_Value().get_PrimaryKey(), 
				8 => MusicianModule.ModuleInstance.keyUtilitySkill2.get_Value().get_PrimaryKey(), 
				9 => MusicianModule.ModuleInstance.keyUtilitySkill3.get_Value().get_PrimaryKey(), 
				10 => MusicianModule.ModuleInstance.keyEliteSkill.get_Value().get_PrimaryKey(), 
				_ => Keys.None, 
			};
		}

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
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (Mode == InstrumentMode.Practice)
			{
				InstrumentSkillType noteType = (((int)key == 10) ? InstrumentSkillType.DecreaseOctave : (((int)key != 11) ? InstrumentSkillType.Note : InstrumentSkillType.IncreaseOctave));
				MusicianModule.ModuleInstance.Conveyor.SpawnNoteBlock(key, noteType, Note.OctaveColors[octave]);
			}
			else if (Mode == InstrumentMode.Emulate)
			{
				Keyboard.Stroke((VirtualKeyShort)(short)GetKeyBinding(key), false);
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
