using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	[System.Runtime.CompilerServices.RequiredMember]
	public class AchievementGroupMenuItem
	{
		[System.Runtime.CompilerServices.RequiredMember]
		public AchievementGroup Group { get; set; }

		[System.Runtime.CompilerServices.RequiredMember]
		public IEnumerable<AchievementCategory> Categories { get; set; }

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public AchievementGroupMenuItem()
		{
		}
	}
}
