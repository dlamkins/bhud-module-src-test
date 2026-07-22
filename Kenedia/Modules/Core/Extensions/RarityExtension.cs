using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class RarityExtension
	{
		public static Microsoft.Xna.Framework.Color GetColor(this ItemRarity rarity)
		{
			return rarity switch
			{
				ItemRarity.Junk => Microsoft.Xna.Framework.Color.DarkGray, 
				ItemRarity.Basic => new Microsoft.Xna.Framework.Color(200, 200, 200), 
				ItemRarity.Fine => new Microsoft.Xna.Framework.Color(74, 146, 236), 
				ItemRarity.Masterwork => new Microsoft.Xna.Framework.Color(43, 184, 14), 
				ItemRarity.Rare => new Microsoft.Xna.Framework.Color(237, 214, 30), 
				ItemRarity.Exotic => new Microsoft.Xna.Framework.Color(235, 154, 1), 
				ItemRarity.Ascended => new Microsoft.Xna.Framework.Color(234, 58, 132), 
				ItemRarity.Legendary => new Microsoft.Xna.Framework.Color(159, 47, 244), 
				_ => Microsoft.Xna.Framework.Color.White, 
			};
		}
	}
}
