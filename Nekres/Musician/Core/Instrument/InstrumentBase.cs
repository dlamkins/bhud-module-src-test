using System;
using System.Threading;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Input;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public abstract class InstrumentBase : IDisposable
	{
		protected readonly TimeSpan NoteTimeout = TimeSpan.FromMilliseconds(5.0);

		protected readonly TimeSpan OctaveTimeout = TimeSpan.FromTicks(500L);

		public readonly bool Walkable;

		protected Octave CurrentOctave { get; set; }

		protected InstrumentBase(bool walkable = true)
		{
			Walkable = walkable;
		}

		protected virtual void PressKey(GuildWarsControls key)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Keyboard.Press((VirtualKeyShort)(short)GetKeyBinding(key), false);
			Thread.Sleep(TimeSpan.FromMilliseconds(1.0));
			Keyboard.Release((VirtualKeyShort)(short)GetKeyBinding(key), false);
			Thread.Sleep(NoteTimeout);
		}

		public void PlayNote(RealNote realNote)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			NoteBase note = ConvertNote(realNote);
			if (RequiresAction(note))
			{
				if ((int)note.Key == 0)
				{
					PressKey((GuildWarsControls)11);
					return;
				}
				note = OptimizeNote(note);
				PressKey(note.Key);
			}
		}

		public void GoToOctave(RealNote realNote)
		{
			NoteBase note = ConvertNote(realNote);
			if (!RequiresAction(note))
			{
				return;
			}
			note = OptimizeNote(note);
			while (CurrentOctave != note.Octave)
			{
				if (CurrentOctave < note.Octave)
				{
					IncreaseOctave();
				}
				else
				{
					DecreaseOctave();
				}
			}
		}

		protected abstract NoteBase ConvertNote(RealNote note);

		private bool RequiresAction(NoteBase note)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)note.Key > 0;
		}

		protected abstract NoteBase OptimizeNote(NoteBase note);

		protected abstract void IncreaseOctave();

		protected abstract void DecreaseOctave();

		public abstract void Dispose();

		private Keys GetKeyBinding(GuildWarsControls key)
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
	}
}
