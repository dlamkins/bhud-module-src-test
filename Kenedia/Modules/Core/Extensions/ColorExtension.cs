using System;
using System.Drawing;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ColorExtension
	{
		public static Color Guardian = new Color(0, 180, 255);

		public static Color Warrior = new Color(247, 157, 0);

		public static Color Engineer = new Color(255, 222, 0);

		public static Color Ranger = new Color(234, 255, 0);

		public static Color Thief = new Color(255, 83, 0);

		public static Color Elementalist = new Color(247, 0, 116);

		public static Color Mesmer = new Color(255, 0, 240);

		public static Color Necromancer = new Color(192, 255, 0);

		public static Color Revenant = new Color(255, 0, 0);

		public static Color GetProfessionColor(ProfessionType profession)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
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
			return (Color)((profession - 1) switch
			{
				0 => Guardian, 
				1 => Warrior, 
				2 => Engineer, 
				3 => Ranger, 
				4 => Thief, 
				5 => Elementalist, 
				6 => Mesmer, 
				7 => Necromancer, 
				8 => Revenant, 
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
