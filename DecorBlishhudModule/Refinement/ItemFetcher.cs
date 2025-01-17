using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blish_HUD;
using HtmlAgilityPack;

namespace DecorBlishhudModule.Refinement
{
	public class ItemFetcher
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

		public static async Task<Dictionary<string, List<Item>>> FetchItemsAsync(string type)
		{
			string url = null;
			if (type == "farm")
			{
				url = "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Farm&format=json&prop=text&section=6";
			}
			if (type == "lumber")
			{
				url = "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Lumber_Mill&format=json&prop=text&section=6";
			}
			if (type == "metal")
			{
				url = "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Metal_Forge&format=json&prop=text&section=3";
			}
			string htmlContent = JsonDocument.Parse(await DecorModule.DecorModuleInstance.Client.GetStringAsync(url)).RootElement.GetProperty("parse").GetProperty("text").GetProperty("*")
				.GetString();
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);
			HtmlNodeCollection source = htmlDocument.DocumentNode.SelectNodes("//table//tr");
			List<Item> items = new List<Item>();
			foreach (HtmlNode item2 in source.Skip(1))
			{
				HtmlNodeCollection columns = item2.SelectNodes("td");
				if (columns != null && columns.Count >= 10)
				{
					string id = columns[2].SelectSingleNode(".//span[@data-id]")?.GetAttributeValue("data-id", "");
					string imageUrl = columns[0].SelectSingleNode(".//img")?.GetAttributeValue("src", "");
					int defaultQty;
					int te1xQty;
					double te2xQty;
					Item item = new Item
					{
						Id = int.Parse(id),
						Name = columns[0].InnerText.Trim(),
						Icon = "https://wiki.guildwars2.com" + imageUrl,
						DefaultQty = (int.TryParse(columns[1].InnerText.Trim(), out defaultQty) ? defaultQty : 0),
						DefaultBuy = columns[2].InnerText.Trim(),
						DefaultSell = columns[3].InnerText.Trim(),
						TradeEfficiency1Qty = (int.TryParse(columns[4].InnerText.Trim(), out te1xQty) ? te1xQty : 0),
						TradeEfficiency1Buy = columns[5].InnerText.Trim(),
						TradeEfficiency1Sell = columns[6].InnerText.Trim(),
						TradeEfficiency2Qty = (double.TryParse(columns[7].InnerText.Trim(), out te2xQty) ? te2xQty : 0.5),
						TradeEfficiency2Buy = columns[8].InnerText.Trim(),
						TradeEfficiency2Sell = columns[9].InnerText.Trim()
					};
					items.Add(item);
				}
			}
			return (from i in await UpdateItemPrices(items)
				group i by i.Name).ToDictionary((IGrouping<string, Item> g) => g.Key, (IGrouping<string, Item> g) => g.ToList());
		}

		public static async Task<List<Item>> UpdateItemPrices(List<Item> items)
		{
			List<int> itemIds = items.Select((Item i) => i.Id).Distinct().ToList();
			string ids = string.Join(",", itemIds);
			string priceApiUrl = "https://api.guildwars2.com/v2/commerce/prices?ids=" + ids;
			List<ItemPrice> priceData = JsonSerializer.Deserialize<List<ItemPrice>>(await DecorModule.DecorModuleInstance.Client.GetStringAsync(priceApiUrl), new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			foreach (Item item in items)
			{
				ItemPrice priceInfo = priceData?.FirstOrDefault((ItemPrice p) => p.Id == item.Id);
				if (priceInfo != null)
				{
					item.DefaultBuy = (priceInfo.Buys?.Unit_Price * item.DefaultQty).ToString();
					item.DefaultSell = (priceInfo.Sells?.Unit_Price * item.DefaultQty).ToString();
					item.TradeEfficiency1Buy = (priceInfo.Buys?.Unit_Price * item.TradeEfficiency1Qty).ToString();
					item.TradeEfficiency1Sell = (priceInfo.Sells?.Unit_Price * item.TradeEfficiency1Qty).ToString();
					item.TradeEfficiency2Buy = Math.Ceiling((double)(priceInfo.Buys?.Unit_Price ?? 0) * item.TradeEfficiency2Qty).ToString();
					item.TradeEfficiency2Sell = Math.Ceiling((double)(priceInfo.Sells?.Unit_Price ?? 0) * item.TradeEfficiency2Qty).ToString();
				}
			}
			return items;
		}
	}
}
