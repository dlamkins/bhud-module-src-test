namespace RaidClears.Features.Shared.Models
{
	public interface IEncounter
	{
		string Id { get; }

		string Name { get; }

		string Abbriviation { get; }

		int IconAssetId { get; }

		int? DailyBountyAchievementId { get; }
	}
}
