using System;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DanceDanceRotationModule.Model
{
	public static class NoteTypeExtensions
	{
		public static Keys DefaultHotkey(NoteType noteType)
		{
			return (Keys)(noteType switch
			{
				NoteType.Dodge => 86, 
				NoteType.WeaponSwap => 192, 
				NoteType.WeaponStow => 0, 
				NoteType.Weapon1 => 49, 
				NoteType.Weapon2 => 50, 
				NoteType.Weapon3 => 51, 
				NoteType.Weapon4 => 52, 
				NoteType.Weapon5 => 53, 
				NoteType.Heal => 54, 
				NoteType.Utility1 => 55, 
				NoteType.Utility2 => 56, 
				NoteType.Utility3 => 57, 
				NoteType.Elite => 48, 
				NoteType.Profession1 => 112, 
				NoteType.Profession2 => 113, 
				NoteType.Profession3 => 114, 
				NoteType.Profession4 => 115, 
				NoteType.Profession5 => 115, 
				NoteType.Unknown => 0, 
				_ => 0, 
			});
		}

		public static string HotkeyDescription(NoteType noteType)
		{
			return noteType switch
			{
				NoteType.Dodge => "Dodge", 
				NoteType.WeaponSwap => "Swap Weapons", 
				NoteType.WeaponStow => "Stow Weapons", 
				NoteType.Weapon1 => "Weapon Skill 1", 
				NoteType.Weapon2 => "Weapon Skill 2", 
				NoteType.Weapon3 => "Weapon Skill 3", 
				NoteType.Weapon4 => "Weapon Skill 4", 
				NoteType.Weapon5 => "Weapon Skill 5", 
				NoteType.Heal => "Healing Skill", 
				NoteType.Utility1 => "Utility Skill 1", 
				NoteType.Utility2 => "Utility Skill 2", 
				NoteType.Utility3 => "Utility Skill 3", 
				NoteType.Elite => "Elite Skill", 
				NoteType.Profession1 => "Profession Skill 1", 
				NoteType.Profession2 => "Profession Skill 2", 
				NoteType.Profession3 => "Profession Skill 3", 
				NoteType.Profession4 => "Profession Skill 4", 
				NoteType.Profession5 => "Profession Skill 5", 
				NoteType.Unknown => "Unknown", 
				_ => "", 
			};
		}

		public static int NoteLane(NotesOrientation orientation, NoteType noteType)
		{
			switch (orientation)
			{
			case NotesOrientation.RightToLeft:
			case NotesOrientation.LeftToRight:
			case NotesOrientation.TopToBottom:
			case NotesOrientation.BottomToTop:
				switch (noteType)
				{
				case NoteType.Weapon1:
				case NoteType.Heal:
				case NoteType.Profession1:
					return 0;
				case NoteType.Weapon2:
				case NoteType.Utility1:
				case NoteType.Profession2:
					return 1;
				case NoteType.Weapon3:
				case NoteType.Utility2:
				case NoteType.Profession3:
					return 2;
				case NoteType.Weapon4:
				case NoteType.Utility3:
				case NoteType.Profession4:
					return 3;
				case NoteType.Weapon5:
				case NoteType.Elite:
				case NoteType.Profession5:
					return 4;
				case NoteType.Dodge:
				case NoteType.WeaponSwap:
				case NoteType.WeaponStow:
				case NoteType.Unknown:
					return 5;
				default:
					return 0;
				}
			case NotesOrientation.AbilityBarStyle:
				switch (noteType)
				{
				case NoteType.WeaponSwap:
				case NoteType.WeaponStow:
				case NoteType.Weapon1:
				case NoteType.Profession1:
					return 0;
				case NoteType.Weapon2:
				case NoteType.Profession2:
					return 1;
				case NoteType.Weapon3:
				case NoteType.Profession3:
					return 2;
				case NoteType.Weapon4:
				case NoteType.Profession4:
					return 3;
				case NoteType.Weapon5:
				case NoteType.Profession5:
					return 4;
				case NoteType.Heal:
					return 5;
				case NoteType.Dodge:
				case NoteType.Utility1:
					return 6;
				case NoteType.Utility2:
					return 7;
				case NoteType.Utility3:
					return 8;
				case NoteType.Elite:
				case NoteType.Unknown:
					return 9;
				default:
					return 0;
				}
			default:
				throw new ArgumentOutOfRangeException("orientation", orientation, null);
			}
		}

		public static Texture2D NoteImage(NoteType noteType)
		{
			switch (noteType)
			{
			case NoteType.WeaponSwap:
			case NoteType.Weapon1:
			case NoteType.Weapon2:
			case NoteType.Weapon3:
			case NoteType.Weapon4:
			case NoteType.Weapon5:
				return Resources.Instance.DdrNoteRedTexture;
			case NoteType.Dodge:
			case NoteType.Heal:
			case NoteType.Utility1:
			case NoteType.Utility2:
			case NoteType.Utility3:
			case NoteType.Elite:
				return Resources.Instance.DdrNotePurpleTexture;
			case NoteType.WeaponStow:
			case NoteType.Profession1:
			case NoteType.Profession2:
			case NoteType.Profession3:
			case NoteType.Profession4:
			case NoteType.Profession5:
			case NoteType.Unknown:
				return Resources.Instance.DdrNoteGreenTexture;
			default:
				return Resources.Instance.DdrNoteRedTexture;
			}
		}
	}
}
