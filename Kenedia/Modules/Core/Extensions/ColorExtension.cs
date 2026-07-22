using System;
using System.Drawing;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ColorExtension
	{
		public static Microsoft.Xna.Framework.Color Guardian = new Microsoft.Xna.Framework.Color(103, 174, 203);

		public static Microsoft.Xna.Framework.Color Warrior = new Microsoft.Xna.Framework.Color(247, 157, 0);

		public static Microsoft.Xna.Framework.Color Engineer = new Microsoft.Xna.Framework.Color(152, 105, 44);

		public static Microsoft.Xna.Framework.Color Ranger = new Microsoft.Xna.Framework.Color(142, 165, 58);

		public static Microsoft.Xna.Framework.Color Thief = new Microsoft.Xna.Framework.Color(73, 85, 120);

		public static Microsoft.Xna.Framework.Color Elementalist = new Microsoft.Xna.Framework.Color(163, 54, 46);

		public static Microsoft.Xna.Framework.Color Mesmer = new Microsoft.Xna.Framework.Color(114, 65, 146);

		public static Microsoft.Xna.Framework.Color Necromancer = new Microsoft.Xna.Framework.Color(63, 88, 71);

		public static Microsoft.Xna.Framework.Color Revenant = new Microsoft.Xna.Framework.Color(87, 36, 53);

		public static Microsoft.Xna.Framework.Color GetProfessionColor(this ProfessionType profession)
		{
			return profession switch
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
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}

		public static Microsoft.Xna.Framework.Color GetSnowCrowColor(this ProfessionType profession)
		{
			return profession switch
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
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}

		public static Microsoft.Xna.Framework.Color GetWikiColor(this ProfessionType profession)
		{
			return profession switch
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
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}

		public static Microsoft.Xna.Framework.Color GetPvPColor(this ProfessionType profession)
		{
			return profession switch
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
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}

		public static Gw2Sharp.WebApi.V2.Models.Color ToApiColor(this Microsoft.Xna.Framework.Color col)
		{
			Gw2Sharp.WebApi.V2.Models.Color color = new Gw2Sharp.WebApi.V2.Models.Color();
			color.BaseRgb = new _003C_003Ez__ReadOnlyArray<int>(new int[4] { col.R, col.G, col.B, col.A });
			return color;
		}

		public static string ToHex(this Microsoft.Xna.Framework.Color col)
		{
			return $"#{col.A:X2}{col.R:X2}{col.G:X2}{col.B:X2}";
		}

		public static bool ColorFromHex(this string col, out Microsoft.Xna.Framework.Color outColor)
		{
			try
			{
				System.Drawing.Color c = ColorTranslator.FromHtml(col);
				outColor = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
				return true;
			}
			catch (Exception)
			{
			}
			outColor = Microsoft.Xna.Framework.Color.Transparent;
			return false;
		}
	}
}
