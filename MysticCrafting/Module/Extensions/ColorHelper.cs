using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Extensions
{
	public static class ColorHelper
	{
		public static Color FromRarity(string rarity)
		{
			if (string.IsNullOrEmpty(rarity))
			{
				return default(Color);
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
					return Color.White;
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
			return Color.White;
		}

		public static Color FromItemCount(int totalItemCount, int totalRequiredItemCount)
		{
			Color materialColor = Color.Red;
			if (totalItemCount >= totalRequiredItemCount)
			{
				materialColor = new Color(45, 197, 7);
			}
			else if (totalItemCount > 0)
			{
				materialColor = new Color(255, 170, 0);
			}
			return materialColor;
		}

		public static Color FromServicesLoading(IEnumerable<IRecurringService> services)
		{
			Color color = Color.White;
			if (services.All(delegate(IRecurringService s)
			{
				_ = s.LastFailed;
				_ = s.LastLoaded;
				return s.LastFailed > s.LastLoaded;
			}))
			{
				color = Color.Red;
			}
			if (services.Any(delegate(IRecurringService s)
			{
				_ = s.LastFailed;
				_ = s.LastLoaded;
				return s.LastFailed > s.LastLoaded;
			}))
			{
				color = Color.Orange;
			}
			else if (services.Any((IRecurringService s) => s.Loaded))
			{
				color = Color.LightGreen;
			}
			return color;
		}
	}
}
