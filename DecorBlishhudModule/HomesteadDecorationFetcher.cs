using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DecorBlishhudModule.Homestead;
using DecorBlishhudModule.Model;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace DecorBlishhudModule
{
	public class HomesteadDecorationFetcher
	{
		private static readonly Dictionary<string, string> IngredientIconCache = new Dictionary<string, string>();

		public static async Task<Dictionary<string, List<Decoration>>> FetchDecorationsAsync()
		{
			DecorModule.DecorModuleInstance.Client.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("Mozilla/5.0");
			Dictionary<string, List<Decoration>> decorationsByCategory = new Dictionary<string, List<Decoration>>();
			KeyValuePair<string, List<Decoration>>[] array = await Task.WhenAll(HomesteadCategories.GetCategories().Select(async delegate(string category)
			{
				string formattedCategoryName = category.Replace(" ", "_");
				return new KeyValuePair<string, List<Decoration>>(category, await FetchDecorationsForCategoryAsync("https://wiki.guildwars2.com/api.php?action=parse&page=Decoration/Homestead/" + formattedCategoryName + "&format=json&prop=text"));
			}).ToList());
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<string, List<Decoration>> result = array[i];
				decorationsByCategory[result.Key] = result.Value;
			}
			return decorationsByCategory;
		}

		private static async Task<List<Decoration>> FetchDecorationsForCategoryAsync(string baseUrl)
		{
			List<Decoration> decorations = new List<Decoration>();
			string combinedHtmlContent = string.Empty;
			JToken obj = JObject.Parse(await DecorModule.DecorModuleInstance.Client.GetStringAsync(baseUrl)).get_Item("parse");
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
			if (!string.IsNullOrEmpty(htmlContent))
			{
				combinedHtmlContent += htmlContent;
			}
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(combinedHtmlContent);
			HtmlNode listNode = doc.DocumentNode.SelectSingleNode("ancestor::h2/following-sibling::div[@class='smw-ul-columns'] | //div[contains(@class, 'smw-ul-columns')]");
			if (listNode != null)
			{
				foreach (HtmlNode item in (IEnumerable<HtmlNode>)listNode.SelectNodes(".//li[@class='smw-row']"))
				{
					Decoration decoration = new Decoration();
					string nameNode = item.InnerText;
					if (nameNode != null)
					{
						decoration.Name = nameNode.Trim().Replace("\u00a0", " ").Replace("&nbsp;", "")
							.Trim();
					}
					HtmlNode iconNode = item.SelectSingleNode(".//img");
					if (iconNode != null)
					{
						decoration.IconUrl = "https://wiki.guildwars2.com" + iconNode.GetAttributeValue("src", "").Trim();
					}
					else
					{
						decoration.IconUrl = "https://wiki.guildwars2.com/images/7/74/Skill.png";
						string[] nameParts = nameNode?.Split(new string[1] { ".png" }, StringSplitOptions.None);
						if (nameParts != null && nameParts.Length > 1)
						{
							decoration.Name = nameParts[1].Trim();
						}
					}
					decorations.Add(decoration);
				}
			}
			HtmlNode galleryNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'srf-gallery')]");
			if (galleryNode != null)
			{
				foreach (HtmlNode item2 in (IEnumerable<HtmlNode>)galleryNode.SelectNodes(".//li[@class='gallerybox']"))
				{
					string galleryName = item2.SelectSingleNode(".//div[@class='gallerytext']//a")?.InnerText.Trim();
					HtmlNode imgNode = item2.SelectSingleNode(".//img");
					string imageUrl = ((imgNode != null) ? ("https://wiki.guildwars2.com" + imgNode.GetAttributeValue("src", "").Trim()) : null);
					if (imageUrl != null)
					{
						imageUrl = imageUrl.Replace("/images/thumb/", "/images/");
						imageUrl = Regex.Replace(imageUrl, "/\\d+px-[^/]+$", "");
					}
					Decoration matchedDecoration2 = decorations.FirstOrDefault((Decoration d) => d.Name.Trim().Equals(galleryName?.Trim(), StringComparison.OrdinalIgnoreCase));
					if (matchedDecoration2 != null && imageUrl != null)
					{
						matchedDecoration2.ImageUrl = imageUrl;
					}
				}
			}
			HtmlNodeCollection recipesNode = doc.DocumentNode.SelectNodes("//table[@class='recipe sortable table']//tr[position()>1]");
			if (recipesNode != null)
			{
				foreach (HtmlNode item3 in (IEnumerable<HtmlNode>)recipesNode)
				{
					HtmlNodeCollection cells = item3.SelectNodes("td");
					if (cells != null && cells.Count >= 5)
					{
						string recipeName = cells[0].InnerText.Trim().Split(new string[1] { "  " }, StringSplitOptions.None)[0];
						Decoration matchedDecoration = decorations.FirstOrDefault((Decoration d) => d.Name.Trim().Equals(recipeName, StringComparison.OrdinalIgnoreCase));
						if (matchedDecoration != null)
						{
							matchedDecoration.Book = ((cells[0].InnerText.Trim().Split(new string[1] { "  " }, StringSplitOptions.None).Length <= 1) ? null : cells[0].InnerText.Trim().Split(new string[1] { "  " }, StringSplitOptions.None).ElementAtOrDefault(1)?.Replace("(Learned from: ", "").Replace(")", "").Trim());
							matchedDecoration.CraftingRating = cells[3]?.InnerText.Trim();
							HtmlNode ingredientsNode = cells[4].SelectSingleNode(".//dl");
							if (ingredientsNode != null)
							{
								ParseIngredients(ingredientsNode, matchedDecoration);
							}
						}
					}
				}
				return decorations;
			}
			return decorations;
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
				HtmlNode ingredientIconNode = ddNodes[i]?.SelectSingleNode(".//img");
				string ingredientIconUrl = null;
				if (ingredientIconNode != null)
				{
					string rawIconUrl = "https://wiki.guildwars2.com" + ingredientIconNode.GetAttributeValue("src", "").Trim();
					if (!IngredientIconCache.TryGetValue(ingredientName, out ingredientIconUrl))
					{
						ingredientIconUrl = rawIconUrl;
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
