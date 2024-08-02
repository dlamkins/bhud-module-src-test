using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Extensions
{
	public static class ColorHelper
	{
		public static Color BrightGreen = new Color(3, 240, 2);

		public static Color BrightBlue = new Color(143, 211, 206);

		public static Color CurrencyName = new Color(252, 201, 117);

		public static Color VendorName = new Color(75, 163, 77);

		public static Color VendorNameHighlight = new Color(34, 221, 34);

		public static Color FromRarity(string rarity)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(rarity))
			{
				return Color.get_White();
			}
			string text = rarity.ToLower();
			if (text != null)
			{
				switch (text.Length)
				{
				case 4:
					switch (text[0])
					{
					case 'f':
						if (!(text == "fine"))
						{
							break;
						}
						return new Color(46, 157, 254);
					case 'r':
						if (!(text == "rare"))
						{
							break;
						}
						return new Color(255, 229, 31);
					}
					break;
				case 5:
					if (!(text == "basic"))
					{
						break;
					}
					return Color.get_White();
				case 10:
					if (!(text == "masterwork"))
					{
						break;
					}
					return new Color(45, 197, 7);
				case 6:
					if (!(text == "exotic"))
					{
						break;
					}
					return new Color(253, 165, 0);
				case 8:
					if (!(text == "ascended"))
					{
						break;
					}
					return new Color(255, 68, 136);
				case 9:
					if (!(text == "legendary"))
					{
						break;
					}
					return new Color(160, 46, 247);
				}
			}
			return Color.get_White();
		}

		public static Color FromItemCount(int totalItemCount, int totalRequiredItemCount)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Color materialColor = default(Color);
			((Color)(ref materialColor))._002Ector(237, 2, 2);
			if (totalItemCount >= totalRequiredItemCount)
			{
				materialColor = Color.get_White();
				return materialColor;
			}
			return materialColor;
		}

		public static Color FromServicesLoading(IEnumerable<IApiService> services)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			Color color = Color.get_White();
			if (services.All(delegate(IApiService s)
			{
				_ = s.LastFailed;
				_ = s.LastLoaded;
				return s.LastFailed > s.LastLoaded;
			}))
			{
				color = Color.get_Red();
			}
			if (services.Any(delegate(IApiService s)
			{
				_ = s.LastFailed;
				_ = s.LastLoaded;
				return s.LastFailed > s.LastLoaded;
			}))
			{
				color = Color.get_Orange();
			}
			else if (services.Any((IApiService s) => s.Loaded))
			{
				color = Color.get_LightGreen();
			}
			return color;
		}
	}
}
