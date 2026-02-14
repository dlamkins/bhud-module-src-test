using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DecorBlishhudModule.Homestead;
using DecorBlishhudModule.Model;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace DecorBlishhudModule
{
	public class GuildHallDecorationFetcher
	{
		private static readonly Dictionary<string, string> IngredientIconCache = new Dictionary<string, string>();

		public static async Task<Dictionary<string, List<Decoration>>> FetchDecorationsAsync()
		{
			Dictionary<string, List<Decoration>> decorationsByCategory = new Dictionary<string, List<Decoration>>();
			List<string> categories = GuildHallCategories.GetCategories();
			foreach (string category in categories)
			{
				string formattedCategoryName = category.Replace(" ", "_");
				decorationsByCategory[category] = await FetchDecorationsForCategoryAsync("https://wiki.guildwars2.com/api.php?action=parse&page=Decoration/Guild_hall/" + formattedCategoryName + "&prop=text&format=json&origin=*");
				await Task.Delay(300);
			}
			return decorationsByCategory;
		}

		private static async Task<List<Decoration>> FetchDecorationsForCategoryAsync(string url)
		{
			List<Decoration> decorations = new List<Decoration>();
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), url);
			try
			{
				request.get_Headers().get_Accept().ParseAdd("application/json");
				HttpResponseMessage response = await DecorModule.DecorModuleInstance.Client.SendAsync(request);
				try
				{
					response.EnsureSuccessStatusCode();
					JToken obj = JObject.Parse(await response.get_Content().ReadAsStringAsync()).get_Item("parse");
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
					string htmlContent = (string)obj2;
					if (string.IsNullOrWhiteSpace(htmlContent))
					{
						return decorations;
					}
					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml(htmlContent);
					HtmlNode listNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'smw-ul-columns')]");
					if (listNode != null)
					{
						HtmlNodeCollection listItems = listNode.SelectNodes(".//li[contains(@class,'smw-row')]");
						if (listItems != null)
						{
							foreach (HtmlNode item2 in (IEnumerable<HtmlNode>)listItems)
							{
								decorations.Add(ParseDecorationItem(item2));
							}
						}
					}
					else if (url.Contains("/Monuments"))
					{
						HtmlNode div1 = doc.DocumentNode.SelectSingleNode("//h2/span[@id='List_of_monument_decorations']/following::div[1]//ul");
						HtmlNode div2 = doc.DocumentNode.SelectSingleNode("//h2/span[@id='List_of_monument_decorations']/following::div[2]//ul");
						List<HtmlNode> combinedItems = new List<HtmlNode>();
						if (div1 != null)
						{
							IEnumerable<HtmlNode> enumerable = div1.SelectNodes(".//li");
							combinedItems.AddRange(enumerable ?? Enumerable.Empty<HtmlNode>());
						}
						if (div2 != null)
						{
							IEnumerable<HtmlNode> enumerable = div2.SelectNodes(".//li");
							combinedItems.AddRange(enumerable ?? Enumerable.Empty<HtmlNode>());
						}
						foreach (HtmlNode item in combinedItems)
						{
							decorations.Add(ParseDecorationItem(item));
						}
					}
					HtmlNode galleryNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'srf-gallery')]");
					if (galleryNode != null)
					{
						HtmlNodeCollection galleryItems = galleryNode.SelectNodes(".//li[contains(@class,'gallerybox')]");
						if (galleryItems != null)
						{
							foreach (HtmlNode item3 in (IEnumerable<HtmlNode>)galleryItems)
							{
								HtmlNode nameNode = item3.SelectSingleNode(".//div[@class='gallerytext']//a");
								HtmlNode imgNode = item3.SelectSingleNode(".//img");
								if (nameNode != null && imgNode != null)
								{
									string galleryName = nameNode.InnerText.Trim();
									string imageUrl = "https://wiki.guildwars2.com" + imgNode.GetAttributeValue("src", "").Replace("/images/thumb/", "/images/");
									imageUrl = Regex.Replace(imageUrl, "/\\d+px-[^/]+$", "");
									Decoration matched = decorations.FirstOrDefault((Decoration d) => d.Name.Equals(galleryName, StringComparison.OrdinalIgnoreCase));
									if (matched != null)
									{
										matched.ImageUrl = imageUrl;
									}
								}
							}
						}
					}
					HtmlNodeCollection recipesNode = doc.DocumentNode.SelectNodes("//table[contains(@class,'recipe')]//tr[position()>1]");
					if (recipesNode != null)
					{
						foreach (HtmlNode item4 in (IEnumerable<HtmlNode>)recipesNode)
						{
							HtmlNodeCollection cells = item4.SelectNodes("td");
							if (cells == null || cells.Count < 5)
							{
								continue;
							}
							string recipeName = cells[0].InnerText.Split(new string[1] { "  " }, StringSplitOptions.None)[0].Trim();
							Decoration decoration = decorations.FirstOrDefault((Decoration d) => d.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
							if (decoration != null)
							{
								decoration.Book = ((cells[0].InnerText.Split(new string[1] { "  " }, StringSplitOptions.None).Length > 1) ? cells[0].InnerText.Split(new string[1] { "  " }, StringSplitOptions.None)[1].Replace("(Learned from: ", "").Replace(")", "").Trim() : null);
								decoration.CraftingRating = cells[3]?.InnerText.Trim();
								HtmlNode ingredientsNode = cells[4].SelectSingleNode(".//dl");
								if (ingredientsNode != null)
								{
									ParseIngredients(ingredientsNode, decoration);
								}
							}
						}
					}
					return decorations;
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)request)?.Dispose();
			}
		}

		private static Decoration ParseDecorationItem(HtmlNode item)
		{
			Decoration obj = new Decoration
			{
				Name = item.InnerText.Replace("\u00a0", " ").Replace("&nbsp;", "").Trim()
			};
			HtmlNode iconNode = item.SelectSingleNode(".//img");
			obj.IconUrl = ((iconNode != null) ? ("https://wiki.guildwars2.com" + iconNode.GetAttributeValue("src", "").Trim()) : "https://wiki.guildwars2.com/images/7/74/Skill.png");
			return obj;
		}

		private static void ParseIngredients(HtmlNode ingredientsNode, Decoration decoration)
		{
			HtmlNodeCollection dtNodes = ingredientsNode.SelectNodes(".//dt");
			HtmlNodeCollection ddNodes = ingredientsNode.SelectNodes(".//dd");
			if (dtNodes == null || ddNodes == null || dtNodes.Count != ddNodes.Count)
			{
				return;
			}
			for (int i = 0; i < dtNodes.Count; i++)
			{
				string ingredientQuantity = dtNodes[i]?.InnerText.Trim();
				string ingredientName = ddNodes[i]?.InnerText.Trim();
				HtmlNode iconNode = ddNodes[i]?.SelectSingleNode(".//img");
				string ingredientIconUrl = null;
				if (iconNode != null)
				{
					string rawUrl = "https://wiki.guildwars2.com" + iconNode.GetAttributeValue("src", "").Trim();
					if (!IngredientIconCache.TryGetValue(ingredientName, out ingredientIconUrl))
					{
						ingredientIconUrl = rawUrl;
						IngredientIconCache[ingredientName] = ingredientIconUrl;
					}
				}
				switch (i + 1)
				{
				case 1:
					decoration.CraftingIngredientName1 = ingredientName;
					decoration.CraftingIngredientIcon1 = ingredientIconUrl;
					decoration.CraftingIngredientQty1 = ingredientQuantity;
					break;
				case 2:
					decoration.CraftingIngredientName2 = ingredientName;
					decoration.CraftingIngredientIcon2 = ingredientIconUrl;
					decoration.CraftingIngredientQty2 = ingredientQuantity;
					break;
				case 3:
					decoration.CraftingIngredientName3 = ingredientName;
					decoration.CraftingIngredientIcon3 = ingredientIconUrl;
					decoration.CraftingIngredientQty3 = ingredientQuantity;
					break;
				case 4:
					decoration.CraftingIngredientName4 = ingredientName;
					decoration.CraftingIngredientIcon4 = ingredientIconUrl;
					decoration.CraftingIngredientQty4 = ingredientQuantity;
					break;
				}
			}
		}
	}
}
