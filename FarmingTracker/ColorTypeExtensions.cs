using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public static class ColorTypeExtensions
	{
		public static Dictionary<ColorType, Color> ColorByColorType = new Dictionary<ColorType, Color>
		{
			[ColorType.White] = Color.get_White(),
			[ColorType.Black] = Color.get_Black(),
			[ColorType.LightGray] = Color.get_LightGray(),
			[ColorType.Gray] = Color.get_Gray(),
			[ColorType.DimGray] = Color.get_DimGray(),
			[ColorType.Lime] = Color.get_Lime(),
			[ColorType.LightGreen] = Color.get_LightGreen(),
			[ColorType.Green] = Color.get_Green(),
			[ColorType.DarkGreen] = Color.get_DarkGreen(),
			[ColorType.Red] = Color.get_Red(),
			[ColorType.DarkRed] = Color.get_DarkRed(),
			[ColorType.Cyan] = Color.get_Cyan(),
			[ColorType.LightBlue] = Color.get_LightBlue(),
			[ColorType.Blue] = Color.get_Blue(),
			[ColorType.DarkBlue] = Color.get_DarkBlue(),
			[ColorType.Beige] = Color.get_Beige(),
			[ColorType.Orange] = Color.get_Orange(),
			[ColorType.Gold] = Color.get_Gold(),
			[ColorType.Brown] = Color.get_Brown(),
			[ColorType.Yellow] = Color.get_Yellow(),
			[ColorType.Magenta] = Color.get_Magenta(),
			[ColorType.Violet] = Color.get_Violet(),
			[ColorType.Purple] = Color.get_Purple()
		};

		public static Color GetColor(this ColorType colorType)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ColorByColorType[colorType];
		}
	}
}
