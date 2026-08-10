using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Alliances.Tags;
using Neokain.GW2.WebClient.Models.Bans;
using Neokain.GW2.WebClient.Models.Guilds.Stash;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubServerAlliance
	{
		Task<List<AllianceMembershipDto>> GetAllianceMemberships(Guid accountId);

		Task<AllianceDetailDto> GetAllianceDetail(Guid allianceId);

		Task<AllianceDetailDto> UpdateAllianceDetail(Guid allianceId, AllianceDetailUpdateDto dto);

		Task<List<AllianceRankDto>> GetAllianceRanks(Guid allianceId);

		Task<AllianceRankDto> GetAllianceRank(Guid allianceId, Guid rankId);

		Task<List<AllianceMemberDetailDto>> GetAllianceMembers(Guid allianceId);

		Task<AllianceMemberDetailDto> GetAllianceMember(Guid allianceId, string memberName);

		Task<List<SpamDto>> GetAllianceSpams(Guid allianceId);

		Task<SpamDetailDto> GetAllianceSpam(Guid allianceId, Guid spamId);

		Task<bool> CanUseAllianceSpam(Guid allianceId, Guid spamId, int? mapId = null);

		Task<SpamDetailDto> UseAllianceSpam(Guid allianceId, Guid spamId, SpamUsageRequestDto? request = null);

		Task<SpamDetailDto> AddAllianceSpam(Guid allianceId, SpamCreateDto dto);

		Task<SpamDetailDto> UpdateAllianceSpam(Guid allianceId, Guid spamId, SpamUpdateDto dto);

		Task DeleteAllianceSpam(Guid allianceId, Guid spamId);

		Task<List<BanDto>> GetAllianceBans(Guid allianceId);

		Task<BanDto> GetAllianceBan(Guid allianceId, Guid banId);

		Task<BanDto> AddAllianceBan(Guid allianceId, BanCreateDto dto);

		Task<BanDto> UpdateAllianceBan(Guid allianceId, Guid banId, BanUpdateDto dto);

		Task DeleteAllianceBan(Guid allianceId, Guid banId);

		Task<List<AllianceTagTypeDto>> GetAllianceTagTypes(Guid allianceId);

		Task<AllianceTagTypeDto?> GetAllianceTagType(Guid allianceId, Guid tagTypeId);

		Task<AllianceTagTypeDto> AddAllianceTagType(Guid allianceId, AllianceTagTypeCreateDto dto);

		Task<AllianceTagTypeDto> UpdateAllianceTagType(Guid allianceId, Guid tagTypeId, AllianceTagTypeUpdateDto dto);

		Task DeleteAllianceTagType(Guid allianceId, Guid tagTypeId);

		Task<List<AllianceMemberTagDto>> GetAllianceMemberTags(Guid allianceId, string memberName);

		Task<List<AllianceMemberTagDto>> GetAllianceActiveTagsByType(Guid allianceId, Guid tagTypeId);

		Task<AllianceMemberTagDto> AddAllianceMemberTag(Guid allianceId, string memberName, AllianceMemberTagCreateDto dto);

		Task<AllianceMemberTagDto> ExtendAllianceMemberTag(Guid allianceId, Guid tagId, AllianceMemberTagExtendDto dto);

		Task RevokeAllianceMemberTag(Guid allianceId, Guid tagId);

		Task<List<AllianceTagHealthCheckResultDto>> GetAllianceTagHealthCheck(Guid allianceId, int expiringInDays = 7);

		Task<int> DeactivateExpiredAllianceTags(Guid allianceId);

		Task<List<StashSearchResultDto>> SearchAllianceStash(Guid allianceId, StashSearchFilterDto? filter);

		Task<List<SpamCategoryDto>> GetAllianceSpamCategories(Guid allianceId);

		Task<SpamCategoryDto> AddAllianceSpamCategory(Guid allianceId, SpamCategoryCreateDto dto);

		Task<SpamCategoryDto> UpdateAllianceSpamCategory(Guid allianceId, Guid categoryId, SpamCategoryUpdateDto dto);

		Task DeleteAllianceSpamCategory(Guid allianceId, Guid categoryId);

		Task<List<SpamMapHistoryDto>> GetAllianceSpamMapHistory(Guid allianceId, Guid spamId);

		Task<List<SpamUsageLogDto>> GetAllianceSpamUsageLogs(Guid allianceId, Guid spamId, int? limit = 50);
	}
}
