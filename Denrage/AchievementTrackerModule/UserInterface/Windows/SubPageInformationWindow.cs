using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.Libs.Interfaces;
using Denrage.AchievementTrackerModule.UserInterface.Controls;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	internal class SubPageInformationWindow : WindowBase2
	{
		private const int PADDING = 15;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly SubPageInformation subPageInformation;

		private readonly IExternalImageService externalImageService;

		private readonly Texture2D texture;

		public SubPageInformationWindow(ContentsManager contentsManager, IAchievementService achievementService, IFormattedLabelHtmlService formattedLabelHtmlService, SubPageInformation subPageInformation, IExternalImageService externalImageService)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.subPageInformation = subPageInformation;
			this.externalImageService = externalImageService;
			texture = this.contentsManager.GetTexture("subpage_background.png");
			BuildWindow();
		}

		private void BuildWindow()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Expected O, but got Unknown
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Expected O, but got Unknown
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Expected O, but got Unknown
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			string title = subPageInformation.Title.Substring(0, Math.Min(subPageInformation.Title.Length, 25));
			if (title != subPageInformation.Title)
			{
				title += " ...";
			}
			((WindowBase2)this).set_Title(title);
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 550, 400), new Rectangle(0, 30, 550, 370));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val).set_Height(((Container)this).get_ContentRegion().Height);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 20f));
			val.set_OuterControlPadding(new Vector2(15f, 15f));
			FlowPanel flowPanel = val;
			((Control)new FormattedLabelBuilder().CreatePart(subPageInformation.Title, delegate(FormattedLabelPartBuilder x)
			{
				x.SetHyperLink(subPageInformation.Link).SetFontSize((FontSize)24).MakeUnderlined();
			}).CreatePart("  ", delegate(FormattedLabelPartBuilder x)
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				x.SetSuffixImage(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("wiki.png"))).SetSuffixImageSize(new Point(24, 24));
			}).AutoSizeHeight()
				.SetWidth(((Container)flowPanel).get_ContentRegion().Width)
				.Wrap()
				.Build()).set_Parent((Container)(object)flowPanel);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)flowPanel);
			((Control)val2).set_Width(((Container)flowPanel).get_ContentRegion().Width - 45);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			Panel panel = val2;
			int labelWidth = ((Container)panel).get_ContentRegion().Width;
			if (subPageInformation is IHasDescriptionList)
			{
				labelWidth = ((Container)panel).get_ContentRegion().Width / 2;
			}
			((Control)formattedLabelHtmlService.CreateLabel(subPageInformation.Description).AutoSizeHeight().SetWidth(labelWidth - 5)
				.Wrap()
				.Build()).set_Parent((Container)(object)panel);
			Control statisticsControl = null;
			LocationSubPageInformation locationSubPage = subPageInformation as LocationSubPageInformation;
			if (locationSubPage != null)
			{
				FormattedLabel formattedLabel = formattedLabelHtmlService.CreateLabel(locationSubPage.Statistics).AutoSizeHeight().Wrap()
					.SetWidth(((Container)panel).get_ContentRegion().Width / 2 - 5)
					.SetHorizontalAlignment((HorizontalAlignment)1)
					.Build();
				((Control)formattedLabel).set_Location(new Point(labelWidth + 5, 0));
				((Control)formattedLabel).set_Parent((Container)(object)panel);
				statisticsControl = (Control)(object)formattedLabel;
			}
			Control imageControl = null;
			IHasImage hasImage = subPageInformation as IHasImage;
			if (hasImage != null && !string.IsNullOrEmpty(hasImage.ImageUrl))
			{
				ImageSpinner imageSpinner2 = new ImageSpinner(externalImageService.GetImageFromIndirectLink(hasImage.ImageUrl));
				((Control)imageSpinner2).set_Parent((Container)(object)panel);
				((Control)imageSpinner2).set_Width(((Container)panel).get_ContentRegion().Width / 2 - 5);
				((Control)imageSpinner2).set_Height(200);
				((Control)imageSpinner2).set_Location(new Point(labelWidth + 5, 0));
				ImageSpinner imageSpinner = imageSpinner2;
				if (statisticsControl != null)
				{
					((Control)imageSpinner).set_Location(new Point(labelWidth + 5, statisticsControl.get_Height() + 5));
				}
				imageControl = (Control)(object)imageSpinner;
			}
			IHasDescriptionList descriptionList = subPageInformation as IHasDescriptionList;
			if (descriptionList != null)
			{
				FlowPanel val3 = new FlowPanel();
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				val3.set_FlowDirection((ControlFlowDirection)3);
				((Control)val3).set_Width(((Container)panel).get_ContentRegion().Width / 2);
				((Control)val3).set_Location(new Point(labelWidth + 5, 0));
				val3.set_ControlPadding(new Vector2(0f, 30f));
				((Control)val3).set_Parent((Container)(object)panel);
				FlowPanel descriptionListPanel = val3;
				foreach (KeyValuePair<string, string> item2 in descriptionList.DescriptionList)
				{
					Panel val4 = new Panel();
					((Control)val4).set_Width(((Container)descriptionListPanel).get_ContentRegion().Width);
					((Container)val4).set_HeightSizingMode((SizingMode)1);
					((Control)val4).set_Parent((Container)(object)descriptionListPanel);
					Panel descriptionEntryPanel = val4;
					((Control)formattedLabelHtmlService.CreateLabel(item2.Key).AutoSizeHeight().Wrap()
						.SetWidth(((Container)descriptionEntryPanel).get_ContentRegion().Width / 2)
						.Build()).set_Parent((Container)(object)descriptionEntryPanel);
					FormattedLabel formattedLabel2 = formattedLabelHtmlService.CreateLabel(item2.Value).AutoSizeHeight().Wrap()
						.SetWidth(((Container)descriptionEntryPanel).get_ContentRegion().Width / 2)
						.Build();
					((Control)formattedLabel2).set_Parent((Container)(object)descriptionEntryPanel);
					((Control)formattedLabel2).set_Location(new Point(((Container)descriptionEntryPanel).get_ContentRegion().Width / 2, 0));
				}
				if (imageControl != null)
				{
					((Control)descriptionListPanel).set_Location(new Point(labelWidth + 5, imageControl.get_Location().Y + imageControl.get_Height() + 5));
				}
			}
			ItemSubPageInformation itemSubPage = subPageInformation as ItemSubPageInformation;
			if (itemSubPage != null)
			{
				((Control)formattedLabelHtmlService.CreateLabel(itemSubPage.Acquisition).AutoSizeHeight().Wrap()
					.SetWidth(((Container)flowPanel).get_ContentRegion().Width - 45)
					.Build()).set_Parent((Container)(object)flowPanel);
			}
			IHasAdditionalImages additionalImages = subPageInformation as IHasAdditionalImages;
			if (additionalImages != null)
			{
				FlowPanel val5 = new FlowPanel();
				((Control)val5).set_Parent((Container)(object)flowPanel);
				((Control)val5).set_Width(((Container)flowPanel).get_ContentRegion().Width - 45);
				val5.set_FlowDirection((ControlFlowDirection)0);
				((Container)val5).set_HeightSizingMode((SizingMode)1);
				FlowPanel additionalImagesFlowPanel = val5;
				foreach (string item in additionalImages.AdditionalImages)
				{
					ImageSpinner imageSpinner3 = new ImageSpinner(externalImageService.GetImageFromIndirectLink(item));
					((Control)imageSpinner3).set_Parent((Container)(object)additionalImagesFlowPanel);
					((Control)imageSpinner3).set_Width(((Control)additionalImagesFlowPanel).get_Width() / 3);
					((Control)imageSpinner3).set_Height(100);
				}
			}
			IHasInteractiveMap interactiveMap = subPageInformation as IHasInteractiveMap;
			if (interactiveMap != null && interactiveMap.InteractiveMap != null)
			{
				InteractiveMapControl interactiveMapControl = new InteractiveMapControl(interactiveMap.InteractiveMap.IconUrl, interactiveMap.InteractiveMap.LocalTiles, interactiveMap.InteractiveMap.Coordinates, interactiveMap.InteractiveMap.Path, interactiveMap.InteractiveMap.Bounds);
				((Control)interactiveMapControl).set_Parent((Container)(object)flowPanel);
				((Control)interactiveMapControl).set_Width(((Container)flowPanel).get_ContentRegion().Width - 45);
				((Control)interactiveMapControl).set_Height(400);
			}
		}
	}
}
