using Estreya.BlishHUD.Shared.Attributes;

namespace Estreya.BlishHUD.Shared.Models.GameIntegration.Chat
{
	public enum ChatChannel
	{
		Say,
		Map,
		Party,
		Squad,
		Team,
		Private,
		RepresentedGuild,
		[Translation("chatChannel-guild1_5", "Guild 1-5")]
		Guild1_5
	}
}
