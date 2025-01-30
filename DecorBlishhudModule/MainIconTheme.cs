using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace DecorBlishhudModule
{
	public static class MainIconTheme
	{
		public static async Task<Texture2D> GetThemeIconAsync(Texture2D defaultIcon, Texture2D _homesteadIconMenuLunar, Texture2D _homesteadIconMenuSAB, Texture2D _homesteadIconMenuDragonBash, Texture2D _homesteadIconMenuFOTFW, Texture2D _homesteadIconMenuHalloween, Texture2D wintersdayIcon)
		{
			int currentYear = DateTime.Now.Year;
			Dictionary<Texture2D, List<string>> iconMapping = new Dictionary<Texture2D, List<string>>
			{
				{
					_homesteadIconMenuLunar,
					new List<string>
					{
						"id=\"Current_release:_“Lunar_New_Year”",
						$"id=\"Current_release:_“Lunar_New_Year_{currentYear}”"
					}
				},
				{
					_homesteadIconMenuSAB,
					new List<string>
					{
						"id=\"Current_release:_“Super_Adventure_Festival”",
						$"id=\"Current_release:_“Super_Adventure_Festival_{currentYear}”"
					}
				},
				{
					_homesteadIconMenuDragonBash,
					new List<string>
					{
						"id=\"Current_release:_“Dragon_Bash”",
						$"id=\"Current_release:_“Dragon_Bash_{currentYear}”"
					}
				},
				{
					_homesteadIconMenuFOTFW,
					new List<string>
					{
						"id=\"Current_release:_“Festival_of_the_Four_Winds”",
						$"id=\"Current_release:_“Festival_of_the_Four_Winds_{currentYear}”"
					}
				},
				{
					_homesteadIconMenuHalloween,
					new List<string>
					{
						"id=\"Current_release:_“Halloween”",
						"id=\"Current_release:_“Shadow_of_the_Mad_King”",
						$"id=\"Current_release:_“Halloween_{currentYear}”",
						$"id=\"Current_release:_“Shadow_of_the_Mad_King_{currentYear}”"
					}
				},
				{
					wintersdayIcon,
					new List<string>
					{
						"id=\"Current_release:_“Wintersday”",
						"id=\"Current_release:_“A_Very_Merry_Wintersday”",
						$"id=\"Current_release:_“Wintersday_{currentYear}”",
						$"id=\"Current_release:_“A_Very_Merry_Wintersday_{currentYear}”"
					}
				}
			};
			try
			{
				DecorModule.DecorModuleInstance.Client.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("Mozilla/5.0");
				JToken obj = JObject.Parse(await DecorModule.DecorModuleInstance.Client.GetStringAsync("https://wiki.guildwars2.com/api.php?action=parse&page=Main_Page&format=json&prop=text")).get_Item("parse");
				object obj2;
				if (obj == null)
				{
					obj2 = null;
				}
				else
				{
					JToken obj3 = obj.get_Item((object)"text");
					obj2 = ((obj3 == null) ? null : ((object)obj3.get_Item((object)"*"))?.ToString());
				}
				if (obj2 == null)
				{
					obj2 = string.Empty;
				}
				string parsedText = (string)obj2;
				foreach (KeyValuePair<Texture2D, List<string>> entry in iconMapping)
				{
					foreach (string keyword in entry.Value)
					{
						if (parsedText.Contains(keyword))
						{
							return entry.Key;
						}
					}
				}
			}
			catch
			{
			}
			return defaultIcon;
		}
	}
}
