using Blish_HUD;
using Microsoft.Xna.Framework;

namespace SongbookOfTyria.UI.Controls.Notation
{
	public static class NotationColorHelper
	{
		private static readonly Color BlueColor = new Color(74, 144, 226);

		private static readonly Color RedColor = new Color(226, 74, 74);

		private static readonly Color GreenColor = new Color(74, 226, 144);

		public static Color GetColorFromName(string colorName)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(colorName))
			{
				return Color.get_White();
			}
			Color hexColor = default(Color);
			if ((colorName.StartsWith("#") || (colorName.Length == 6 && IsHexString(colorName))) && ColorUtil.TryParseHex(colorName, ref hexColor))
			{
				return hexColor;
			}
			return (Color)(colorName.ToLowerInvariant() switch
			{
				"blue" => BlueColor, 
				"red" => RedColor, 
				"green" => GreenColor, 
				"yellow" => Color.get_Yellow(), 
				"orange" => Color.get_Orange(), 
				"purple" => Color.get_Purple(), 
				_ => Color.get_White(), 
			});
		}

		private static bool IsHexString(string text)
		{
			foreach (char c in text)
			{
				if ((c < '0' || c > '9') && (c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
				{
					return false;
				}
			}
			return true;
		}
	}
}
