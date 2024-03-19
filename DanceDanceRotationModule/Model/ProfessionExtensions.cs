using System;
using System.Globalization;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule.Model
{
	public static class ProfessionExtensions
	{
		public static Profession ProfessionFromBuildTemplate(int buildTemplateCode)
		{
			return buildTemplateCode switch
			{
				0 => Profession.Common, 
				1 => Profession.Guardian, 
				2 => Profession.Warrior, 
				3 => Profession.Engineer, 
				4 => Profession.Ranger, 
				5 => Profession.Thief, 
				6 => Profession.Elementalist, 
				7 => Profession.Mesmer, 
				8 => Profession.Necromancer, 
				9 => Profession.Revenant, 
				_ => Profession.Unknown, 
			};
		}

		public static Profession CurrentProfessionOfPlayer()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected I4, but got Unknown
			return (int)GameService.Gw2Mumble.get_PlayerCharacter().get_Profession() switch
			{
				0 => Profession.Unknown, 
				1 => Profession.Guardian, 
				2 => Profession.Warrior, 
				3 => Profession.Engineer, 
				4 => Profession.Ranger, 
				5 => Profession.Thief, 
				6 => Profession.Elementalist, 
				7 => Profession.Mesmer, 
				8 => Profession.Necromancer, 
				9 => Profession.Revenant, 
				_ => Profession.Unknown, 
			};
		}

		public static string GetProfessionDisplayText(Profession profession)
		{
			return profession switch
			{
				Profession.Common => "Common", 
				Profession.Guardian => "Guardian", 
				Profession.Warrior => "Warrior", 
				Profession.Engineer => "Engineer", 
				Profession.Ranger => "Ranger", 
				Profession.Thief => "Thief", 
				Profession.Elementalist => "Elementalist", 
				Profession.Mesmer => "Mesmer", 
				Profession.Necromancer => "Necromancer", 
				Profession.Revenant => "Revenant", 
				_ => "Unknown", 
			};
		}

		public static Color GetProfessionColor(Profession profession)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			return FromRgbHex(profession switch
			{
				Profession.Common => "#BBBBBB", 
				Profession.Guardian => "#72C1D9", 
				Profession.Warrior => "#FFD166", 
				Profession.Engineer => "#D09C59", 
				Profession.Ranger => "#8CDC82", 
				Profession.Thief => "#C08F95", 
				Profession.Elementalist => "#F68A87", 
				Profession.Mesmer => "#B679D5", 
				Profession.Necromancer => "#52A76F", 
				Profession.Revenant => "#D16E5A", 
				_ => "#BBBBBB", 
			});
		}

		private static Color FromRgbHex(string hex)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (hex.StartsWith("#"))
			{
				hex = hex.Substring(1);
			}
			if (hex.Length != 6)
			{
				throw new Exception("Color not valid");
			}
			return new Color(int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber), int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber), int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
		}
	}
}
