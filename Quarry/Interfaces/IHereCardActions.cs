namespace Quarry.Interfaces
{
	public interface IHereCardActions
	{
		bool IsHidden(int achievementId);

		void SnoozeUntilReset(int achievementId);

		void HideIndefinitely(int achievementId);

		void Unhide(int achievementId);

		string DescribeExclusion(int achievementId);
	}
}
