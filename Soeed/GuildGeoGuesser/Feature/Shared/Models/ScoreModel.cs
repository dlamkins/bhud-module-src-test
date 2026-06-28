using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	public class ScoreModel
	{
		[JsonProperty("min")]
		public float Min { get; set; }

		[JsonProperty("max")]
		public float Max { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; } = "missing value";


		[JsonProperty("color")]
		public string Color { get; set; } = "#FFFFFF";


		[JsonProperty("vfx")]
		public bool Vfx { get; set; }

		public bool IsRainbow => IsRainbowColorValue(Color);

		public static bool IsRainbowColorValue(string? color)
		{
			return string.Equals(color, "rainbow", StringComparison.OrdinalIgnoreCase);
		}

		public static Color ParseHexColor(string hexColor)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (hexColor.StartsWith("#"))
				{
					hexColor = hexColor.Substring(1);
				}
				if (hexColor.Length == 6)
				{
					int num = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
					int g2 = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
					int b2 = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
					return new Color(num, g2, b2);
				}
				if (hexColor.Length == 8)
				{
					int num2 = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
					int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
					int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
					int a = int.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber);
					return new Color(num2, g, b, a);
				}
			}
			catch
			{
			}
			return Color.get_White();
		}
	}
}
