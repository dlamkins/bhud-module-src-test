using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public static class ColorService
	{
		public static Color JunkColor = new Color(170, 170, 170);

		public static Color FineColor = new Color(85, 153, 255);

		public static Color MasterworkColor = new Color(51, 204, 17);

		public static Color RareColor = new Color(255, 221, 34);

		public static Color ExoticColor = new Color(255, 170, 0);

		public static Color AscendedColor = new Color(255, 68, 136);

		public static Color LegendaryColor = new Color(153, 51, 255);

		public static Color GetRarityBorderColor(ItemRarity rarity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			return (Color)((rarity - 1) switch
			{
				0 => Color.get_Transparent(), 
				1 => Color.get_Transparent(), 
				2 => FineColor, 
				3 => MasterworkColor, 
				4 => RareColor, 
				5 => ExoticColor, 
				6 => AscendedColor, 
				7 => LegendaryColor, 
				_ => Color.get_Transparent(), 
			});
		}

		public static Color GetRarityTextColor(ItemRarity rarity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			return (Color)((rarity - 1) switch
			{
				0 => JunkColor, 
				1 => Color.get_White(), 
				2 => FineColor, 
				3 => MasterworkColor, 
				4 => RareColor, 
				5 => ExoticColor, 
				6 => AscendedColor, 
				7 => LegendaryColor, 
				_ => Color.get_White(), 
			});
		}
	}
}
