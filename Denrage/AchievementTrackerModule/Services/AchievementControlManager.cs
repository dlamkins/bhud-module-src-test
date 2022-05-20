using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementControlManager : IAchievementControlManager, IDisposable
	{
		private readonly Dictionary<int, Control> controls = new Dictionary<int, Control>();

		private readonly IAchievementControlProvider achievementControlProvider;

		public AchievementControlManager(IAchievementControlProvider achievementControlProvider)
		{
			this.achievementControlProvider = achievementControlProvider;
		}

		public void InitializeControl(int achievementId, AchievementTableEntry achievement, AchievementTableEntryDescription description)
		{
			if (!controls.ContainsKey(achievementId))
			{
				Control control = achievementControlProvider.GetAchievementControl(achievement, achievement.Description);
				controls[achievementId] = control;
			}
		}

		public bool ControlExists(int achievementId)
		{
			return controls.ContainsKey(achievementId);
		}

		public void ChangeParent(int achievementId, Container parent)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (controls.TryGetValue(achievementId, out var control))
			{
				control.set_Parent(parent);
				if (parent != null)
				{
					control.set_Width(parent.get_ContentRegion().Width);
					control.set_Height(parent.get_ContentRegion().Height);
				}
			}
		}

		public void RemoveParent(int achievementId)
		{
			ChangeParent(achievementId, null);
		}

		public Control GetControl(int achievementId)
		{
			return controls[achievementId];
		}

		public void Dispose()
		{
			foreach (KeyValuePair<int, Control> control in controls)
			{
				control.Value.Dispose();
			}
			controls.Clear();
		}
	}
}
