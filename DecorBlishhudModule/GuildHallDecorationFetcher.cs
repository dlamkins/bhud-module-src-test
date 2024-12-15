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
	public class GuildHallDecorationFetcher
	{
		public static async Task<Dictionary<string, List<Decoration>>> FetchDecorationsAsync()
		{
			Dictionary<string, List<Decoration>> decorationsByCategory = new Dictionary<string, List<Decoration>>();
			KeyValuePair<string, List<Decoration>>[] array = await Task.WhenAll(GuildHallCategories.GetCategories().Select(async delegate(string category)
			{
				string formattedCategoryName = category.Replace(" ", "_");
				return new KeyValuePair<string, List<Decoration>>(category, await FetchDecorationsForCategoryAsync("https://wiki.guildwars2.com/api.php?action=parse&page=Decoration/Guild_hall/" + formattedCategoryName + "&format=json&prop=text"));
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
			string[] sections = new string[2] { "1", "2" };
			string combinedHtmlContent = string.Empty;
			string[] array = sections;
			foreach (string section in array)
			{
				string url = baseUrl + "&section=" + section;
				JToken obj = JObject.Parse(await DecorModule.DecorModuleInstance.Client.GetStringAsync(url)).get_Item("parse");
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
					Decoration matchedDecoration = decorations.FirstOrDefault((Decoration d) => d.Name.Trim().Equals(galleryName?.Trim(), StringComparison.OrdinalIgnoreCase));
					if (matchedDecoration != null && imageUrl != null)
					{
						matchedDecoration.ImageUrl = imageUrl;
					}
				}
				return decorations;
			}
			return decorations;
		}
	}
}
