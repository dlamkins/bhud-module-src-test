using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using HtmlAgilityPack;
using Quarry.Interfaces;

namespace Quarry.Services
{
	public class FormattedLabelHtmlService : IFormattedLabelHtmlService
	{
		private static readonly Dictionary<string, int> GameLinkTypeBytes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
		{
			["item"] = 2,
			["text"] = 3,
			["map"] = 4,
			["skill"] = 6,
			["trait"] = 7,
			["recipe"] = 9,
			["skin"] = 10,
			["outfit"] = 11
		};

		private readonly IExternalImageService externalImageService;

		private readonly ContentsManager contentsManager;

		public FormattedLabelHtmlService(ContentsManager contentsManager, IExternalImageService externalImageService)
		{
			this.contentsManager = contentsManager;
			this.externalImageService = externalImageService;
		}

		public FormattedLabelBuilder CreateLabel(string textWithHtml)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			FormattedLabelBuilder labelBuilder = new FormattedLabelBuilder();
			HtmlNode htmlNode = HtmlNode.CreateNode("<div>" + textWithHtml + "</div>");
			MergeGameLinksIntoPrecedingAnchors(htmlNode);
			foreach (HtmlNode childNode in (IEnumerable<HtmlNode>)htmlNode.ChildNodes)
			{
				foreach (FormattedLabelPartBuilder item in CreateParts(childNode, labelBuilder))
				{
					labelBuilder.CreatePart(item);
				}
			}
			return labelBuilder;
		}

		private static void MergeGameLinksIntoPrecedingAnchors(HtmlNode root)
		{
			HtmlNodeCollection gameLinkSpans = root.SelectNodes(".//span[@class='gamelink']");
			if (gameLinkSpans == null)
			{
				return;
			}
			foreach (HtmlNode gameLinkSpan in gameLinkSpans.ToList())
			{
				string chatLink = DecodeGameLink(gameLinkSpan.GetAttributeValue("data-type", null), gameLinkSpan.GetAttributeValue("data-id", null));
				if (chatLink == null)
				{
					continue;
				}
				List<HtmlNode> separators = new List<HtmlNode>();
				HtmlNode cursor = gameLinkSpan.PreviousSibling;
				while (cursor != null && cursor.Name != "a")
				{
					bool isBareSpan = cursor.Name == "span" && !cursor.GetClasses().Any();
					if (!(cursor.Name == "#text" || isBareSpan) || !IsDashOrBlank(cursor.InnerText))
					{
						cursor = null;
						break;
					}
					separators.Add(cursor);
					cursor = cursor.PreviousSibling;
				}
				if (cursor == null || cursor.Name != "a")
				{
					continue;
				}
				cursor.SetAttributeValue("data-copy-chatlink", chatLink);
				foreach (HtmlNode item in separators)
				{
					item.Remove();
				}
				HtmlNode script = gameLinkSpan.NextSibling;
				gameLinkSpan.Remove();
				if (script != null && script.Name == "script")
				{
					script.Remove();
				}
			}
		}

		private static bool IsDashOrBlank(string text)
		{
			string trimmed = WebUtility.HtmlDecode(text)?.Trim();
			if (!string.IsNullOrEmpty(trimmed) && !(trimmed == "—"))
			{
				return trimmed == "-";
			}
			return true;
		}

		[IteratorStateMachine(typeof(_003CCreateParts_003Ed__7))]
		private IEnumerable<FormattedLabelPartBuilder> CreateParts(HtmlNode childNode, FormattedLabelBuilder labelBuilder)
		{
			return new _003CCreateParts_003Ed__7(-2)
			{
				_003C_003E4__this = this,
				_003C_003E3__childNode = childNode,
				_003C_003E3__labelBuilder = labelBuilder
			};
		}

		private static string DecodeGameLink(string type, string idText)
		{
			if (string.IsNullOrEmpty(type) || !GameLinkTypeBytes.TryGetValue(type.Trim(), out var typeByte) || !long.TryParse(idText, out var id) || id < 0)
			{
				return null;
			}
			List<byte> bytes = new List<byte>();
			for (long remaining = id; remaining > 0; remaining >>= 8)
			{
				bytes.Add((byte)(remaining & 0xFF));
			}
			while (bytes.Count < 4 || bytes.Count % 2 != 0)
			{
				bytes.Add(0);
			}
			if (typeByte == GameLinkTypeBytes["item"])
			{
				bytes.Insert(0, 1);
			}
			bytes.Insert(0, (byte)typeByte);
			return "[&" + Convert.ToBase64String(bytes.ToArray()) + "]";
		}
	}
}
