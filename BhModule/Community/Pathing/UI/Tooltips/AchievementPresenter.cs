using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	public class AchievementPresenter : Presenter<AchievementTooltipView, int>
	{
		private static readonly Logger Logger = Logger.GetLogger<AchievementPresenter>();

		private const int LOAD_ATTEMPTS = 3;

		private AchievementCategory _achievementCategories;

		private Achievement _achievement;

		public AchievementPresenter(AchievementTooltipView view, int achievementId)
			: base(view, achievementId)
		{
		}

		private async Task<bool> AttemptLoadAchievement(IProgress<string> progress, int attempt = 3)
		{
			progress.Report("Loading achievement details...");
			try
			{
				_achievement = await ((IBulkExpandableClient<Achievement, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Achievements()).GetAsync(base.get_Model(), default(CancellationToken));
			}
			catch (Exception ex)
			{
				if (attempt > 0)
				{
					AchievementPresenter achievementPresenter = this;
					int num = attempt - 1;
					attempt = num;
					return await achievementPresenter.AttemptLoadAchievement(progress, num);
				}
				Logger.Warn(ex, "Failed to load details for achievement with ID {achievementId}.", new object[1] { base.get_Model() });
				return false;
			}
			return true;
		}

		private async Task<bool> AttemptLoadAchievementCategory(IProgress<string> progress, int attempt = 3)
		{
			progress.Report("Loading achievement group details...");
			try
			{
				_achievementCategories = ((IEnumerable<AchievementCategory>)(await ((IAllExpandableClient<AchievementCategory>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Achievements()
					.get_Categories()).AllAsync(default(CancellationToken)))).FirstOrDefault((AchievementCategory category) => category.get_Achievements().Contains(base.get_Model()));
			}
			catch (Exception ex)
			{
				if (attempt > 0)
				{
					AchievementPresenter achievementPresenter = this;
					int num = attempt - 1;
					attempt = num;
					return await achievementPresenter.AttemptLoadAchievementCategory(progress, num);
				}
				Logger.Warn(ex, "Failed to load achievement details.");
				return false;
			}
			return true;
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			if (!(await AttemptLoadAchievement(progress)) && _achievement != null)
			{
				progress.Report("Failed to load achievement details.");
				return false;
			}
			await AttemptLoadAchievementCategory(progress);
			return true;
		}

		protected override void UpdateView()
		{
			base.get_View().Achievement = _achievement;
			base.get_View().AchievementCategory = _achievementCategories;
		}
	}
}
