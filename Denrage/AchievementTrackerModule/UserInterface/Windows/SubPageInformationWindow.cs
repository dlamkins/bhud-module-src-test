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

		private readonly Texture2D texture;

		public SubPageInformationWindow(ContentsManager contentsManager, IAchievementService achievementService, IFormattedLabelHtmlService formattedLabelHtmlService, SubPageInformation subPageInformation)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.subPageInformation = subPageInformation;
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
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Expected O, but got Unknown
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Expected O, but got Unknown
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Expected O, but got Unknown
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Expected O, but got Unknown
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Expected O, but got Unknown
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0682: Unknown result type (might be due to invalid IL or missing references)
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
				Image val3 = new Image();
				((Control)val3).set_Width(((Container)panel).get_ContentRegion().Width / 2 - 5);
				((Control)val3).set_Height(200);
				((Control)val3).set_Location(new Point(labelWidth + 5, 0));
				((Control)val3).set_Parent((Container)(object)panel);
				Image image = val3;
				if (statisticsControl != null)
				{
					((Control)image).set_Location(new Point(labelWidth + 5, statisticsControl.get_Height() + 5));
				}
				LoadingSpinner val4 = new LoadingSpinner();
				((Control)val4).set_Parent((Container)(object)panel);
				LoadingSpinner spinner2 = val4;
				((Control)spinner2).set_Location(new Point(labelWidth + 5 + (((Control)panel).get_Width() - labelWidth - 5) / 2 - ((Control)spinner2).get_Width() / 2, ((Control)image).get_Location().Y + ((Control)image).get_Height() / 2 - ((Control)spinner2).get_Height() / 2));
				((Control)spinner2).Show();
				image.set_Texture(achievementService.GetImageFromIndirectLink(hasImage.ImageUrl, delegate
				{
					((Control)spinner2).Dispose();
				}));
				imageControl = (Control)(object)image;
			}
			IHasDescriptionList descriptionList = subPageInformation as IHasDescriptionList;
			if (descriptionList != null)
			{
				FlowPanel val5 = new FlowPanel();
				((Container)val5).set_HeightSizingMode((SizingMode)1);
				val5.set_FlowDirection((ControlFlowDirection)3);
				((Control)val5).set_Width(((Container)panel).get_ContentRegion().Width / 2);
				((Control)val5).set_Location(new Point(labelWidth + 5, 0));
				val5.set_ControlPadding(new Vector2(0f, 30f));
				((Control)val5).set_Parent((Container)(object)panel);
				FlowPanel descriptionListPanel = val5;
				foreach (KeyValuePair<string, string> item2 in descriptionList.DescriptionList)
				{
					Panel val6 = new Panel();
					((Control)val6).set_Width(((Container)descriptionListPanel).get_ContentRegion().Width);
					((Container)val6).set_HeightSizingMode((SizingMode)1);
					((Control)val6).set_Parent((Container)(object)descriptionListPanel);
					Panel descriptionEntryPanel = val6;
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
				FlowPanel val7 = new FlowPanel();
				((Control)val7).set_Parent((Container)(object)flowPanel);
				((Control)val7).set_Width(((Container)flowPanel).get_ContentRegion().Width - 45);
				val7.set_FlowDirection((ControlFlowDirection)0);
				((Container)val7).set_HeightSizingMode((SizingMode)1);
				FlowPanel additionalImagesFlowPanel = val7;
				foreach (string item in additionalImages.AdditionalImages)
				{
					LoadingSpinner val8 = new LoadingSpinner();
					((Control)val8).set_Location(new Point(((Control)additionalImagesFlowPanel).get_Width() / 2, ((Control)additionalImagesFlowPanel).get_Height() / 2));
					((Control)val8).set_Parent((Container)(object)additionalImagesFlowPanel);
					LoadingSpinner spinner = val8;
					((Control)spinner).Show();
					Image val9 = new Image();
					val9.set_Texture(achievementService.GetImageFromIndirectLink(item, delegate
					{
						((Control)spinner).Dispose();
					}));
					((Control)val9).set_Width(((Control)additionalImagesFlowPanel).get_Width() / 3);
					((Control)val9).set_Height(100);
					((Control)val9).set_Parent((Container)(object)additionalImagesFlowPanel);
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
