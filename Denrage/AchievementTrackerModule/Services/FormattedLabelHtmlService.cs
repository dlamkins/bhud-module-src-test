using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;
using Flurl.Http;
using HtmlAgilityPack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.Services
{
	public class FormattedLabelHtmlService : IFormattedLabelHtmlService
	{
		private const string USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		private readonly IAchievementService achievementService;

		private readonly ISubPageInformationWindowManager subPageInformationWindowManager;

		public readonly ContentsManager contentsManager;

		public FormattedLabelHtmlService(ContentsManager contentsManager, IAchievementService achievementService, ISubPageInformationWindowManager subPageInformationWindowManager)
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.subPageInformationWindowManager = subPageInformationWindowManager;
		}

		public FormattedLabelBuilder CreateLabel(string textWithHtml)
		{
			FormattedLabelBuilder labelBuilder = new FormattedLabelBuilder();
			foreach (HtmlNode childNode in (IEnumerable<HtmlNode>)HtmlNode.CreateNode("<div>" + textWithHtml + "</div>").ChildNodes)
			{
				foreach (FormattedLabelPartBuilder item in CreateParts(childNode, labelBuilder))
				{
					labelBuilder.CreatePart(item);
				}
			}
			return labelBuilder;
		}

		private IEnumerable<FormattedLabelPartBuilder> CreateParts(HtmlNode childNode, FormattedLabelBuilder labelBuilder)
		{
			if (childNode.Name == "#text")
			{
				yield return labelBuilder.CreatePart(childNode.InnerText);
			}
			else if (childNode.Name == "a")
			{
				if (!childNode.GetClasses().Contains("mw-selflink"))
				{
					foreach (HtmlNode innerChildNode10 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
					{
						foreach (FormattedLabelPartBuilder part3 in CreateParts(innerChildNode10, labelBuilder))
						{
							string link = childNode.GetAttributeValue("href", "");
							bool inSubpages = false;
							foreach (SubPageInformation subPage in achievementService.Subpages)
							{
								if (subPage.Link.Contains(link) && !inSubpages)
								{
									inSubpages = true;
									yield return part3.SetLink(delegate
									{
										subPageInformationWindowManager.Create(subPage);
									}).MakeUnderlined();
								}
							}
							if (!inSubpages)
							{
								if (link.StartsWith("/"))
								{
									link = "https://wiki.guildwars2.com/" + link;
								}
								yield return part3.SetHyperLink(link).MakeUnderlined();
							}
						}
					}
					yield break;
				}
				foreach (HtmlNode innerChildNode14 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item3 in CreateParts(innerChildNode14, labelBuilder))
					{
						yield return item3;
					}
				}
			}
			else if (childNode.Name == "span")
			{
				if (childNode.GetClasses().Contains("inline-icon"))
				{
					HtmlNode imageNode = childNode.ChildNodes.FindFirst("img");
					if (imageNode != null)
					{
						FormattedLabelPartBuilder builder2 = labelBuilder.CreatePart("");
						builder2.SetPrefixImage(GetTexture(imageNode.GetAttributeValue("src", ""))).SetPrefixImageSize(new Point(24, 24));
						yield return builder2;
					}
					yield break;
				}
				foreach (HtmlNode innerChildNode13 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item4 in CreateParts(innerChildNode13, labelBuilder))
					{
						yield return item4;
					}
				}
			}
			else if (childNode.Name == "b")
			{
				foreach (HtmlNode innerChildNode12 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item5 in CreateParts(innerChildNode12, labelBuilder))
					{
						yield return item5;
					}
				}
			}
			else if (childNode.Name == "i")
			{
				foreach (HtmlNode innerChildNode11 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder part2 in CreateParts(innerChildNode11, labelBuilder))
					{
						yield return part2.MakeItalic();
					}
				}
			}
			else if (childNode.Name == "sup")
			{
				yield return labelBuilder.CreatePart(string.Empty);
			}
			else if (childNode.Name == "h3")
			{
				foreach (HtmlNode innerChildNode9 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item6 in CreateParts(innerChildNode9, labelBuilder))
					{
						yield return item6;
					}
				}
			}
			else if (childNode.Name == "style" || childNode.Name == "script")
			{
				yield return labelBuilder.CreatePart(string.Empty);
			}
			else if (childNode.Name == "p")
			{
				foreach (HtmlNode innerChildNode8 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item7 in CreateParts(innerChildNode8, labelBuilder))
					{
						yield return item7;
					}
				}
			}
			else if (childNode.Name == "br")
			{
				yield return labelBuilder.CreatePart("\n");
			}
			else if (childNode.Name == "small")
			{
				foreach (HtmlNode innerChildNode7 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item8 in CreateParts(innerChildNode7, labelBuilder))
					{
						yield return item8;
					}
				}
			}
			else if (childNode.Name == "ul")
			{
				foreach (HtmlNode item2 in childNode.ChildNodes.Where((HtmlNode x) => x.Name == "li"))
				{
					foreach (FormattedLabelPartBuilder item9 in CreateParts(item2, labelBuilder))
					{
						yield return item9;
					}
				}
			}
			else if (childNode.Name == "li")
			{
				foreach (HtmlNode innerChildNode6 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item10 in CreateParts(innerChildNode6, labelBuilder))
					{
						yield return item10;
					}
				}
			}
			else if (childNode.Name == "div")
			{
				foreach (HtmlNode innerChildNode5 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item11 in CreateParts(innerChildNode5, labelBuilder))
					{
						yield return item11;
					}
				}
			}
			else if (childNode.Name == "code")
			{
				yield return labelBuilder.CreatePart(childNode.InnerText);
			}
			else if (childNode.Name == "img")
			{
				FormattedLabelPartBuilder builder = labelBuilder.CreatePart(string.Empty);
				yield return builder.SetPrefixImage(GetTexture(childNode.GetAttributeValue("src", string.Empty)));
			}
			else if (childNode.Name == "s")
			{
				foreach (HtmlNode innerChildNode4 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder part in CreateParts(innerChildNode4, labelBuilder))
					{
						yield return part.MakeStrikeThrough();
					}
				}
			}
			else if (childNode.Name == "dl")
			{
				foreach (HtmlNode innerChildNode3 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item12 in CreateParts(innerChildNode3, labelBuilder))
					{
						yield return item12;
					}
				}
			}
			else if (childNode.Name == "font")
			{
				foreach (HtmlNode innerChildNode2 in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item13 in CreateParts(innerChildNode2, labelBuilder))
					{
						yield return item13;
					}
				}
			}
			else if (childNode.Name == "big")
			{
				foreach (HtmlNode innerChildNode in (IEnumerable<HtmlNode>)childNode.ChildNodes)
				{
					foreach (FormattedLabelPartBuilder item14 in CreateParts(innerChildNode, labelBuilder))
					{
						yield return item14;
					}
				}
			}
			else
			{
				if (!(childNode.Name == "ol"))
				{
					yield break;
				}
				foreach (HtmlNode item in childNode.ChildNodes.Where((HtmlNode x) => x.Name == "li"))
				{
					foreach (FormattedLabelPartBuilder item15 in CreateParts(item, labelBuilder))
					{
						yield return item15;
					}
				}
			}
		}

		private AsyncTexture2D GetTexture(string url)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			AsyncTexture2D texture = new AsyncTexture2D(Textures.get_TransparentPixel());
			Stream imageStream;
			Task.Run(delegate
			{
				try
				{
					imageStream = GeneratedExtensions.GetStreamAsync(GeneratedExtensions.WithHeader("https://wiki.guildwars2.com" + url, "user-agent", (object)"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"), default(CancellationToken), (HttpCompletionOption)0).Result;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
					{
						texture.SwapTexture(TextureUtil.FromStreamPremultiplied(device, imageStream));
						imageStream.Close();
					});
				}
				catch (Exception ex)
				{
					Exception innerException = ex.InnerException;
					FlurlHttpException val = (FlurlHttpException)(object)((innerException is FlurlHttpException) ? innerException : null);
					if (val == null)
					{
						throw;
					}
					if (!((Exception)(object)val).Message.Contains("404 (Not Found)"))
					{
						throw;
					}
				}
			});
			return texture;
		}
	}
}
