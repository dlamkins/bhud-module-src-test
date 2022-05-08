using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.AchievementControl
{
	public abstract class AchievementControlFactory
	{
		public abstract Control Create(AchievementTableEntry achievement, object description);
	}
	public abstract class AchievementControlFactory<T, TDescription> : AchievementControlFactory, IControlFactory<T, TDescription> where T : Panel, IAchievementControl
	{
		protected abstract T CreateInternal(AchievementTableEntry achievement, TDescription description);

		public override Control Create(AchievementTableEntry achievement, object description)
		{
			return (Control)(object)Create(achievement, (TDescription)description);
		}

		public T Create(AchievementTableEntry achievement, TDescription description)
		{
			T val = CreateInternal(achievement, description);
			((Container)(object)val).set_HeightSizingMode((SizingMode)1);
			val.BuildControl();
			return val;
		}
	}
}
