using System;
using System.Runtime.CompilerServices;
using GuildWars2.Hero.Achievements;
using Microsoft.Extensions.DependencyInjection;

namespace SL.ChatLinks.UI.Tabs.Achievements.Tooltips
{
	public sealed class AchievementTooltipViewModelFactory
	{
		[CompilerGenerated]
		private IServiceProvider _003Csp_003EP;

		public AchievementTooltipViewModelFactory(IServiceProvider sp)
		{
			_003Csp_003EP = sp;
			base._002Ector();
		}

		public AchievementTooltipViewModel Create(Achievement achievement)
		{
			return ActivatorUtilities.CreateInstance<AchievementTooltipViewModel>(_003Csp_003EP, new object[1] { achievement });
		}
	}
}
