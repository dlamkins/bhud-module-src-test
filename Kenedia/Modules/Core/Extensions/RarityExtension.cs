using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class RarityExtension
	{
		public static Color GetColor(this ItemRarity rarity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
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
			return (Color)((rarity - 1) switch
			{
				0 => Color.get_DarkGray(), 
				1 => new Color(200, 200, 200), 
				2 => new Color(74, 146, 236), 
				3 => new Color(43, 184, 14), 
				4 => new Color(237, 214, 30), 
				5 => new Color(235, 154, 1), 
				6 => new Color(234, 58, 132), 
				7 => new Color(159, 47, 244), 
				_ => Color.get_White(), 
			});
		}
	}
}
