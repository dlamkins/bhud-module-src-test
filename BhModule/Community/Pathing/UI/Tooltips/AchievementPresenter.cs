using System;
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

		private const int LOAD_ATTEMPTS = 1;

		private AchievementCategory _achievementCategories;

		private Achievement _achievement;

		public AchievementPresenter(AchievementTooltipView view, int achievementId)
			: base(view, achievementId)
		{
		}

		private async Task<bool> AttemptLoadAchievement(IProgress<string> progress, int attempt = 1)
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

		private async Task<bool> AttemptLoadAchievementCategory(IProgress<string> progress, int attempt = 1)
		{
			progress.Report("Loading achievement group details...");
			_achievementCategories = PathingModule.Instance.PackInitiator.PackState.AchievementStates.GetAchievementCategory(base.get_Model());
			return _achievement != null;
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			Task.Run(async delegate
			{
				if (!(await AttemptLoadAchievement(progress)) && _achievement != null)
				{
					progress.Report("Failed to load achievement details.");
				}
				else
				{
					await AttemptLoadAchievementCategory(progress);
					((Presenter<AchievementTooltipView, int>)this).UpdateView();
				}
			});
			return true;
		}

		protected override void UpdateView()
		{
			base.get_View().Achievement = _achievement;
			base.get_View().AchievementCategory = _achievementCategories;
		}
	}
}
