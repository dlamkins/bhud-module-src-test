using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementCollectionControl : AchievementListControl<CollectionDescription, CollectionDescriptionEntry>
	{
		private readonly IExternalImageService externalImageService;

		public AchievementCollectionControl(IItemDetailWindowManager itemDetailWindowManager, IAchievementService achievementService, IFormattedLabelHtmlService formattedLabelHtmlService, IExternalImageService externalImageService, ContentsManager contentsManager, AchievementTableEntry achievement, CollectionDescription description)
			: base(itemDetailWindowManager, achievementService, formattedLabelHtmlService, contentsManager, achievement, description)
		{
			this.externalImageService = externalImageService;
		}

		protected override void ColorControl(Control control, bool achievementBitFinished)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			ImageSpinner image = control as ImageSpinner;
			if (image != null)
			{
				image.Tint = (achievementBitFinished ? Color.get_White() : Color.FromNonPremultiplied(255, 255, 255, 50));
			}
		}

		protected override Control CreateEntryControl(int index, CollectionDescriptionEntry entry, Container parent)
		{
			ImageSpinner imageSpinner = new ImageSpinner(externalImageService.GetImageFromIndirectLink(entry.ImageUrl));
			((Control)imageSpinner).set_Parent(parent);
			((Control)imageSpinner).set_Width(32);
			((Control)imageSpinner).set_Height(32);
			((Control)imageSpinner).set_ZIndex(1);
			return (Control)(object)imageSpinner;
		}

		protected override string GetDisplayName(CollectionDescriptionEntry entry)
		{
			return entry?.DisplayName ?? string.Empty;
		}

		protected override IEnumerable<CollectionDescriptionEntry> GetEntries(CollectionDescription description)
		{
			return description?.EntryList ?? Array.Empty<CollectionDescriptionEntry>().ToList();
		}
	}
}
