using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using HtmlAgilityPack;

namespace Denrage.AchievementTrackerModule.Services
{
	public class FormattedLabelHtmlService : IFormattedLabelHtmlService
	{
		private const string USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		private readonly IAchievementService achievementService;

		private readonly ISubPageInformationWindowManager subPageInformationWindowManager;

		private readonly IExternalImageService externalImageService;

		public readonly ContentsManager contentsManager;

		public FormattedLabelHtmlService(ContentsManager contentsManager, IAchievementService achievementService, ISubPageInformationWindowManager subPageInformationWindowManager, IExternalImageService externalImageService)
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.subPageInformationWindowManager = subPageInformationWindowManager;
			this.externalImageService = externalImageService;
		}

		public FormattedLabelBuilder CreateLabel(string textWithHtml)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
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
	}
}
