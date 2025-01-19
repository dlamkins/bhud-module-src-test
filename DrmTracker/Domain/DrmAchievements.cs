using Gw2Sharp.WebApi.V2.Models;

namespace DrmTracker.Domain
{
	public class DrmAchievements
	{
		public AccountAchievement Clear { get; set; }

		public AccountAchievement FullCM { get; set; }

		public AccountAchievement Factions { get; set; }

		public bool HasFullSuccess
		{
			get
			{
				if (Clear != null && Clear.get_Done() && FullCM != null && FullCM.get_Done())
				{
					if (Factions != null)
					{
						return Factions.get_Done();
					}
					return false;
				}
				return false;
			}
		}
	}
}
