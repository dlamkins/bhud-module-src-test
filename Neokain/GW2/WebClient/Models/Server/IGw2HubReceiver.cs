using System;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Alliances.Tags;
using Neokain.GW2.WebClient.Models.Bans;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubReceiver
	{
		Task GuildSpamAdded(Guid guildId, SpamDetailDto spam);

		Task GuildSpamChanged(Guid guildId, SpamDetailDto spam);

		Task GuildSpamRemoved(Guid guildId, Guid spamId);

		Task GuildSpamUsed(Guid guildId, SpamUsedNotificationDto notification);

		Task GuildBanAdded(Guid guildId, BanDto ban);

		Task GuildBanChanged(Guid guildId, BanDto ban);

		Task GuildBanRemoved(Guid guildId, Guid banId);

		Task AllianceSpamAdded(Guid allianceId, SpamDetailDto spam);

		Task AllianceSpamChanged(Guid allianceId, SpamDetailDto spam);

		Task AllianceSpamRemoved(Guid allianceId, Guid spamId);

		Task AllianceSpamUsed(Guid allianceId, SpamUsedNotificationDto notification);

		Task AllianceBanAdded(Guid allianceId, BanDto ban);

		Task AllianceBanChanged(Guid allianceId, BanDto ban);

		Task AllianceBanRemoved(Guid allianceId, Guid banId);

		Task AllianceTagTypeAdded(Guid allianceId, AllianceTagTypeDto tagType);

		Task AllianceTagTypeChanged(Guid allianceId, AllianceTagTypeDto tagType);

		Task AllianceTagTypeRemoved(Guid allianceId, Guid tagTypeId);

		Task AllianceMemberTagAdded(Guid allianceId, AllianceMemberTagDto memberTag);

		Task AllianceMemberTagChanged(Guid allianceId, AllianceMemberTagDto memberTag);

		Task AllianceMemberTagRevoked(Guid allianceId, Guid tagId);

		Task TaskProgressUpdated(Guid taskId, TaskProgressDto progress);

		Task TaskCompleted(Guid taskId, TaskDto task);

		Task TaskFailed(Guid taskId, TaskDto task);

		Task TaskCancelled(Guid taskId);

		Task SpamFavoriteAdded(Guid accountId, SpamFavoriteDto favorite);

		Task SpamFavoriteRemoved(Guid accountId, Guid favoriteId);

		Task SpamFavoritesReordered(Guid accountId);

		Task GuildSpamCategoryAdded(Guid guildId, SpamCategoryDto category);

		Task GuildSpamCategoryChanged(Guid guildId, SpamCategoryDto category);

		Task GuildSpamCategoryRemoved(Guid guildId, Guid categoryId);

		Task AllianceSpamCategoryAdded(Guid allianceId, SpamCategoryDto category);

		Task AllianceSpamCategoryChanged(Guid allianceId, SpamCategoryDto category);

		Task AllianceSpamCategoryRemoved(Guid allianceId, Guid categoryId);
	}
}
