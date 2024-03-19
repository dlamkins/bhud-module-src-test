using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class ItemExtensions
	{
		public static Color AsColor(this ItemRarity rarity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected I4, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			return (Color)((int)rarity switch
			{
				0 => Color.get_White(), 
				1 => new Color(170, 170, 170), 
				2 => Color.get_White(), 
				3 => new Color(98, 164, 218), 
				4 => new Color(26, 147, 6), 
				5 => new Color(252, 208, 11), 
				6 => new Color(255, 164, 5), 
				7 => new Color(251, 62, 141), 
				8 => new Color(136, 79, 217), 
				_ => Color.get_White(), 
			});
		}
	}
}
