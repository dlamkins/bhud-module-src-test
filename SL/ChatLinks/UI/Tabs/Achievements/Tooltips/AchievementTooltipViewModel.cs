using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2.Hero.Achievements;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SL.ChatLinks.Storage;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Achievements.Tooltips
{
	public sealed class AchievementTooltipViewModel : ViewModel
	{
		[CompilerGenerated]
		private Achievement _003Cachievement_003EP;

		public IStringLocalizer<AchievementTooltipView> Localizer { get; }

		public string Name => _003Cachievement_003EP.Name;

		public string Requirement
		{
			get
			{
				string requirement = _003Cachievement_003EP.Requirement;
				if (_003Cachievement_003EP.Tiers.Count > 0)
				{
					IReadOnlyList<AchievementTier> tiers = _003Cachievement_003EP.Tiers;
					AchievementTier tier = tiers[tiers.Count - 1];
					if ((object)tier != null)
					{
						string count = $" {tier.Count:N0} ";
						requirement = requirement.Replace("  ", count);
					}
				}
				return requirement;
			}
		}

		public string Description => _003Cachievement_003EP.Description;

		public AchievementTooltipViewModel(ILogger<AchievementTooltipViewModel> logger, IStringLocalizer<AchievementTooltipView> localizer, IDbContextFactory contextFactory, ILocale locale, AccountUnlocks hero, Achievement achievement)
		{
			_003Cachievement_003EP = achievement;
			Localizer = localizer;
			base._002Ector();
		}
	}
}
