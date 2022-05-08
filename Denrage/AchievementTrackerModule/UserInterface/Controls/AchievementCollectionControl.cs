using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementCollectionControl : AchievementListControl<CollectionDescription, CollectionDescriptionEntry>
	{
		public AchievementCollectionControl(IItemDetailWindowManager itemDetailWindowManager, IAchievementService achievementService, ContentsManager contentsManager, AchievementTableEntry achievement, CollectionDescription description)
			: base(itemDetailWindowManager, achievementService, contentsManager, achievement, description)
		{
		}

		protected override void ColorControl(Control control, bool achievementBitFinished)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Image image = (Image)(object)((control is Image) ? control : null);
			if (image != null)
			{
				image.set_Tint(achievementBitFinished ? Color.get_White() : Color.FromNonPremultiplied(255, 255, 255, 50));
			}
		}

		protected override Control CreateEntryControl(int index, CollectionDescriptionEntry entry, Container parent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent(parent);
			LoadingSpinner spinner = val;
			((Control)spinner).set_Location(new Point((((Control)parent).get_Width() - ((Control)spinner).get_Width()) / 2, (((Control)parent).get_Height() - ((Control)spinner).get_Height()) / 2));
			((Control)spinner).Show();
			Image val2 = new Image();
			((Control)val2).set_Parent(parent);
			((Control)val2).set_Width(32);
			((Control)val2).set_Height(32);
			val2.set_Texture(base.AchievementService.GetImage(entry.ImageUrl, delegate
			{
				((Control)spinner).Dispose();
			}));
			((Control)val2).set_ZIndex(1);
			return (Control)val2;
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
