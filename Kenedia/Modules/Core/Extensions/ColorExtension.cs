using System;
using System.Drawing;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ColorExtension
	{
		public static Color Guardian = new Color(103, 174, 203);

		public static Color Warrior = new Color(247, 157, 0);

		public static Color Engineer = new Color(152, 105, 44);

		public static Color Ranger = new Color(142, 165, 58);

		public static Color Thief = new Color(73, 85, 120);

		public static Color Elementalist = new Color(163, 54, 46);

		public static Color Mesmer = new Color(114, 65, 146);

		public static Color Necromancer = new Color(63, 88, 71);

		public static Color Revenant = new Color(87, 36, 53);

		public static Color GetProfessionColor(this ProfessionType profession)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(profession switch
			{
				ProfessionType.Guardian => Guardian, 
				ProfessionType.Warrior => Warrior, 
				ProfessionType.Engineer => Engineer, 
				ProfessionType.Ranger => Ranger, 
				ProfessionType.Thief => Thief, 
				ProfessionType.Elementalist => Elementalist, 
				ProfessionType.Mesmer => Mesmer, 
				ProfessionType.Necromancer => Necromancer, 
				ProfessionType.Revenant => Revenant, 
				_ => Color.get_White(), 
			});
		}

		public static Color GetSnowCrowColor(this ProfessionType profession)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(profession switch
			{
				ProfessionType.Guardian => SnowCrowsColor.Guardian, 
				ProfessionType.Warrior => SnowCrowsColor.Warrior, 
				ProfessionType.Engineer => SnowCrowsColor.Engineer, 
				ProfessionType.Ranger => SnowCrowsColor.Ranger, 
				ProfessionType.Thief => SnowCrowsColor.Thief, 
				ProfessionType.Elementalist => SnowCrowsColor.Elementalist, 
				ProfessionType.Mesmer => SnowCrowsColor.Mesmer, 
				ProfessionType.Necromancer => SnowCrowsColor.Necromancer, 
				ProfessionType.Revenant => SnowCrowsColor.Revenant, 
				_ => Color.get_White(), 
			});
		}

		public static Color GetWikiColor(this ProfessionType profession)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(profession switch
			{
				ProfessionType.Guardian => WikiColor.Guardian, 
				ProfessionType.Warrior => WikiColor.Warrior, 
				ProfessionType.Engineer => WikiColor.Engineer, 
				ProfessionType.Ranger => WikiColor.Ranger, 
				ProfessionType.Thief => WikiColor.Thief, 
				ProfessionType.Elementalist => WikiColor.Elementalist, 
				ProfessionType.Mesmer => WikiColor.Mesmer, 
				ProfessionType.Necromancer => WikiColor.Necromancer, 
				ProfessionType.Revenant => WikiColor.Revenant, 
				_ => Color.get_White(), 
			});
		}

		public static Color GetPvPColor(this ProfessionType profession)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(profession switch
			{
				ProfessionType.Guardian => PvPColor.Guardian, 
				ProfessionType.Warrior => PvPColor.Warrior, 
				ProfessionType.Engineer => PvPColor.Engineer, 
				ProfessionType.Ranger => PvPColor.Ranger, 
				ProfessionType.Thief => PvPColor.Thief, 
				ProfessionType.Elementalist => PvPColor.Elementalist, 
				ProfessionType.Mesmer => PvPColor.Mesmer, 
				ProfessionType.Necromancer => PvPColor.Necromancer, 
				ProfessionType.Revenant => PvPColor.Revenant, 
				_ => Color.get_White(), 
			});
		}

		public static string ToHex(this Color col)
		{
			Color.FromArgb(((Color)(ref col)).get_A(), ((Color)(ref col)).get_R(), ((Color)(ref col)).get_G(), ((Color)(ref col)).get_B());
			return $"#{((Color)(ref col)).get_A():X2}{((Color)(ref col)).get_R():X2}{((Color)(ref col)).get_G():X2}{((Color)(ref col)).get_B():X2}";
		}

		public static bool ColorFromHex(this string col, out Color outColor)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Color c = ColorTranslator.FromHtml(col);
				outColor = new Color(c.R, c.G, c.B, c.A);
				return true;
			}
			catch (Exception)
			{
			}
			outColor = Color.get_Transparent();
			return false;
		}
	}
}
