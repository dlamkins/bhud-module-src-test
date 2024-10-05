using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class RarityExtension
	{
		public static Color GetColor(this ItemRarity rarity)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(rarity switch
			{
				ItemRarity.Junk => Color.get_DarkGray(), 
				ItemRarity.Basic => new Color(200, 200, 200), 
				ItemRarity.Fine => new Color(74, 146, 236), 
				ItemRarity.Masterwork => new Color(43, 184, 14), 
				ItemRarity.Rare => new Color(237, 214, 30), 
				ItemRarity.Exotic => new Color(235, 154, 1), 
				ItemRarity.Ascended => new Color(234, 58, 132), 
				ItemRarity.Legendary => new Color(159, 47, 244), 
				_ => Color.get_White(), 
			});
		}
	}
}
