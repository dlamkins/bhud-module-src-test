using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableMapEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableMapEntry>
	{
		private readonly IExternalImageService externalImageService;

		private readonly Logger logger;

		public AchievementTableMapEntryFactory(IExternalImageService externalImageService, Logger logger)
		{
			this.externalImageService = externalImageService;
			this.logger = logger;
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableMapEntry entry)
		{
			ImageSpinner imageSpinner = new ImageSpinner(externalImageService.GetImageFromIndirectLink(entry.ImageLink));
			((Control)imageSpinner).set_Width(250);
			((Control)imageSpinner).set_Height(250);
			((Control)imageSpinner).set_ZIndex(1);
			((Control)imageSpinner).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(async delegate
				{
					try
					{
						Process.Start("https://wiki.guildwars2.com" + await externalImageService.GetDirectImageLink(entry.ImageLink));
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Exception occured on opening map in browser");
					}
				});
			});
			return (Control)(object)imageSpinner;
		}
	}
}
