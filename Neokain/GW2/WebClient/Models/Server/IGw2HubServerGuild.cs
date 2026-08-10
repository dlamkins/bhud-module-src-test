using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Bans;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Guilds.Stash;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubServerGuild
	{
		Task<List<GuildMembershipDto>> GetGuilds(Guid accountId);

		Task<GuildDetailDto> GetGuildDetail(Guid guildId);

		Task<byte[]?> GetGuildEmblemAsync(Guid guildId, int size = 64);

		Task<List<GuildRankDto>> GetGuildRanks(Guid guildId);

		Task<GuildRankDto> GetGuildRank(Guid guildId, string rankId);

		Task<List<GuildMemberDetailDto>> GetGuildMembers(Guid guildId);

		Task<GuildMemberDetailDto> GetGuildMember(Guid guildId, string memberName);

		Task<List<SpamDto>> GetGuildSpams(Guid guildId);

		Task<SpamDetailDto> GetGuildSpam(Guid guildId, Guid spamId);

		Task<bool> CanUseGuildSpam(Guid guildId, Guid spamId, int? mapId = null);

		Task<SpamDetailDto> UseGuildSpam(Guid guildId, Guid spamId, SpamUsageRequestDto? request = null);

		Task<SpamDetailDto> AddGuildSpam(Guid guildId, SpamCreateDto dto);

		Task<SpamDetailDto> UpdateGuildSpam(Guid guildId, Guid spamId, SpamUpdateDto dto);

		Task DeleteGuildSpam(Guid guildId, Guid spamId);

		Task<List<BanDto>> GetGuildBans(Guid guildId);

		Task<BanDto> GetGuildBan(Guid guildId, Guid banId);

		Task<BanDto> AddGuildBan(Guid guildId, BanCreateDto dto);

		Task<BanDto> UpdateGuildBan(Guid guildId, Guid banId, BanUpdateDto dto);

		Task DeleteGuildBan(Guid guildId, Guid banId);

		Task<List<GuildStashTabDto>> GetGuildStash(Guid guildId);

		Task<List<GuildTreasuryItemDto>> GetGuildTreasury(Guid guildId);

		Task<List<GuildStashChangeDto>> GetGuildStashHistory(Guid guildId, GuildStashChangeFilterDto? filter);

		Task<List<StashSearchResultDto>> SearchGuildStash(Guid guildId, StashSearchFilterDto? filter);

		Task<List<SpamCategoryDto>> GetGuildSpamCategories(Guid guildId);

		Task<SpamCategoryDto> AddGuildSpamCategory(Guid guildId, SpamCategoryCreateDto dto);

		Task<SpamCategoryDto> UpdateGuildSpamCategory(Guid guildId, Guid categoryId, SpamCategoryUpdateDto dto);

		Task DeleteGuildSpamCategory(Guid guildId, Guid categoryId);

		Task<List<SpamMapHistoryDto>> GetGuildSpamMapHistory(Guid guildId, Guid spamId);

		Task<List<SpamUsageLogDto>> GetGuildSpamUsageLogs(Guid guildId, Guid spamId, int? limit = 50);
	}
}
