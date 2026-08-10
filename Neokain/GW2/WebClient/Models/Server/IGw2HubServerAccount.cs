using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Bans;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubServerAccount
	{
		Task<AccountDataDto> GetMyAccount();

		Task<AccountDataDto> GetAccountData(Guid accountId);

		Task<List<SpamDto>> GetAccountSpams(Guid accountId);

		Task<SpamDetailDto> GetAccountSpam(Guid accountId, Guid spamId);

		Task<SpamDetailDto> AddAccountSpam(Guid accountId, SpamCreateDto dto);

		Task<SpamDetailDto> UpdateAccountSpam(Guid accountId, Guid spamId, SpamUpdateDto dto);

		Task DeleteAccountSpam(Guid accountId, Guid spamId);

		Task<bool> CanUseAccountSpam(Guid accountId, Guid spamId, int? mapId = null);

		Task<SpamDetailDto> UseAccountSpam(Guid accountId, Guid spamId, SpamUsageRequestDto? request = null);

		Task<List<SpamCategoryDto>> GetAccountSpamCategories(Guid accountId);

		Task<SpamCategoryDto> AddAccountSpamCategory(Guid accountId, SpamCategoryCreateDto dto);

		Task<SpamCategoryDto> UpdateAccountSpamCategory(Guid accountId, Guid categoryId, SpamCategoryUpdateDto dto);

		Task DeleteAccountSpamCategory(Guid accountId, Guid categoryId);

		Task<List<SpamMapHistoryDto>> GetAccountSpamMapHistory(Guid accountId, Guid spamId);

		Task<List<SpamUsageLogDto>> GetAccountSpamUsageLogs(Guid accountId, Guid spamId, int? limit = 50);

		Task<List<BanDto>> GetAccountBans(Guid accountId);

		Task<BanDto> GetAccountBan(Guid accountId, Guid banId);

		Task<BanDto> AddAccountBan(Guid accountId, BanCreateDto dto);

		Task<BanDto> UpdateAccountBan(Guid accountId, Guid banId, BanUpdateDto dto);

		Task DeleteAccountBan(Guid accountId, Guid banId);

		Task<List<GuildMembershipDto>> GetAccountGuildMemberships(Guid accountId);

		Task<GuildMembershipDto> GetAccountGuildMembership(Guid accountId, Guid guildId);

		Task<List<GuildLeadershipDto>> GetAccountGuildLeaderships(Guid accountId);

		Task<List<AllianceMembershipDto>> GetAccountAllianceMemberships(Guid accountId);

		Task<AllianceMembershipDto> GetAccountAllianceMembership(Guid accountId, Guid allianceId);

		Task<List<AllianceLeadershipDto>> GetAccountAllianceLeaderships(Guid accountId);

		Task<TaskDto> CreateAccountTask(string taskType, Guid accountId, object? payload = null);

		Task<TaskDto> CreateGuildTask(string taskType, Guid guildId, object? payload = null);

		Task<TaskDto> CreateAllianceTask(string taskType, Guid allianceId, object? payload = null);

		Task<TaskDto?> GetTask(Guid taskId);

		Task<List<TaskDto>> GetMyTasks(int? status = null);

		Task<List<TaskDto>> GetAccountTasks(Guid accountId, int? status = null);

		Task<bool> CancelTask(Guid taskId);

		Task<List<SpamFavoriteDto>> GetSpamFavorites(Guid accountId);

		Task<SpamFavoriteDto> AddSpamFavorite(Guid accountId, SpamFavoriteCreateDto dto);

		Task RemoveSpamFavorite(Guid accountId, Guid favoriteId);

		Task<SpamDetailDto> UseSpamFavorite(Guid accountId, Guid favoriteId, SpamUsageRequestDto? request = null);

		Task ReorderSpamFavorites(Guid accountId, SpamFavoriteReorderDto dto);
	}
}
