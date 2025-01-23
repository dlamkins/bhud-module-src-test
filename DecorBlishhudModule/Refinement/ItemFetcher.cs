using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
			string url = type switch
			{
				"farm" => "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Farm&format=json&prop=text&section=6", 
				"lumber" => "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Lumber_Mill&format=json&prop=text&section=6", 
				"metal" => "https://wiki.guildwars2.com/api.php?action=parse&page=Homestead_Refinement%E2%80%94Metal_Forge&format=json&prop=text&section=3", 
				_ => null, 
			};
			if (string.IsNullOrWhiteSpace(url))
			{
				Logger.Error("Invalid refinement type provided.");
				return new Dictionary<string, List<Item>>();
			}
			string response;
			try
			{
				response = await RetryPolicyAsync(() => DecorModule.DecorModuleInstance.Client.GetStringAsync(url));
			}
			catch (HttpRequestException val)
			{
				HttpRequestException ex2 = val;
				Logger.Error("Failed to fetch items for type '" + type + "': " + ((Exception)(object)ex2).Message);
				return new Dictionary<string, List<Item>>();
			}
			try
			{
				if (!JsonDocument.Parse(response).RootElement.TryGetProperty("parse", out var parseElement) || !parseElement.TryGetProperty("text", out var textElement))
				{
					Logger.Error("Invalid response structure from the server.");
					return new Dictionary<string, List<Item>>();
				}
				string htmlContent = textElement.GetProperty("*").GetString();
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(htmlContent);
				HtmlNodeCollection source = htmlDocument.DocumentNode.SelectNodes("//table//tr") ?? new HtmlNodeCollection(null);
				List<Item> items = new List<Item>();
				foreach (HtmlNode item2 in source.Skip(1))
				{
					HtmlNodeCollection columns = item2.SelectNodes("td") ?? new HtmlNodeCollection(null);
					if (columns.Count >= 10)
					{
						string s = columns[2].SelectSingleNode(".//span[@data-id]")?.GetAttributeValue("data-id", "0");
						string imageUrl = columns[0].SelectSingleNode(".//img")?.GetAttributeValue("src", "");
						int.TryParse(s, out var itemId);
						int defaultQty;
						int te1xQty;
						double te2xQty;
						Item item = new Item
						{
							Id = itemId,
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
			catch (Exception ex)
			{
				Logger.Error("Error parsing items for type '" + type + "': " + ex.Message);
				return new Dictionary<string, List<Item>>();
			}
		}

		public static async Task<List<Item>> UpdateItemPrices(List<Item> items)
		{
			List<int> source = items.Select((Item i) => i.Id).Distinct().ToList();
			int batchSize = 200;
			IEnumerable<IEnumerable<int>> batches = source.Batch(batchSize);
			foreach (IEnumerable<int> batch in batches)
			{
				string ids = string.Join(",", batch);
				string priceApiUrl = "https://api.guildwars2.com/v2/commerce/prices?ids=" + ids;
				try
				{
					List<ItemPrice> priceData = JsonSerializer.Deserialize<List<ItemPrice>>(await RetryPolicyAsync(() => DecorModule.DecorModuleInstance.Client.GetStringAsync(priceApiUrl)), new JsonSerializerOptions
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
				}
				catch (HttpRequestException val)
				{
					HttpRequestException ex = val;
					Logger.Error("Error fetching prices for batch " + ids + ": " + ((Exception)(object)ex).Message);
				}
			}
			return items;
		}

		private static async Task<string> RetryPolicyAsync(Func<Task<string>> action, int retries = 3)
		{
			object obj2 = default(object);
			int result = default(int);
			HttpRequestException ex = default(HttpRequestException);
			for (int i = 0; i < retries; i++)
			{
				try
				{
					return await action();
				}
				catch (object obj) when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					obj2 = ((obj is HttpRequestException) ? obj : null);
					if (obj2 == null)
					{
						result = 0;
					}
					else
					{
						ex = (HttpRequestException)obj2;
						result = ((i < retries - 1) ? 1 : 0);
					}
					return (byte)result != 0;
				}).Invoke())
				{
					Logger.Warn("Retrying due to error: " + ((Exception)(object)ex).Message);
					await Task.Delay(1000);
				}
			}
			throw new HttpRequestException("Failed after multiple retries.");
		}
	}
}
