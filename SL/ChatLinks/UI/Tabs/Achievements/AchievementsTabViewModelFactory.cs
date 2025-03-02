using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementsTabViewModelFactory
	{
		[CompilerGenerated]
		private IServiceProvider _003Csp_003EP;

		public AchievementsTabViewModelFactory(IServiceProvider sp)
		{
			_003Csp_003EP = sp;
			base._002Ector();
		}

		public AchievementsTabViewModel Create()
		{
			return ActivatorUtilities.CreateInstance<AchievementsTabViewModel>(_003Csp_003EP, Array.Empty<object>());
		}
	}
}
