using System;
using Quarry.UserInterface.Windows;
using Quarry.WikiData.Achievement;

namespace Quarry.Interfaces
{
	public interface IInspectorWindowManager : IDisposable
	{
		void ShowAchievement(AchievementTableEntry achievement);

		void NotifyPinned(InspectorWindow window);

		void Update();
	}
}
