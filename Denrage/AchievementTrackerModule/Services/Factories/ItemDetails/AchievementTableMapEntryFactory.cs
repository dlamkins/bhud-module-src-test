using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableMapEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableMapEntry>
	{
		private readonly IAchievementService achievementService;

		private readonly Logger logger;

		public AchievementTableMapEntryFactory(IAchievementService achievementService, Logger logger)
		{
			this.achievementService = achievementService;
			this.logger = logger;
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableMapEntry entry)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Width(250);
			((Control)val).set_Height(250);
			Panel panel = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(((Control)panel).get_Width() / 2, ((Control)panel).get_Height() / 2));
			((Control)val2).set_Parent((Container)(object)panel);
			LoadingSpinner spinner = val2;
			((Control)spinner).Show();
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)panel);
			val3.set_Texture(achievementService.GetImageFromIndirectLink(entry.ImageLink, delegate
			{
				((Control)spinner).Dispose();
			}));
			Rectangle contentRegion = ((Container)panel).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			Image result = val3;
			((Control)panel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				Image obj = result;
				Rectangle contentRegion2 = ((Container)panel).get_ContentRegion();
				((Control)obj).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
			});
			((Control)result).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(async delegate
				{
					try
					{
						Process.Start("https://wiki.guildwars2.com" + await achievementService.GetDirectImageLink(entry.ImageLink));
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Exception occured on opening map in browser");
					}
				});
			});
			return (Control)(object)panel;
		}
	}
}
