using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Repositories
{
	public class SpamFavoriteRepository : ISpamFavoriteRepository
	{
		private readonly IGw2WebClient _webClient;

		private readonly Guid _accountId;

		public event EventHandler<SpamFavoriteDto> FavoriteAdded;

		public event EventHandler<Guid> FavoriteRemoved;

		public SpamFavoriteRepository(IGw2WebClient webClient, Guid accountId)
		{
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			_accountId = accountId;
		}

		public Task<List<SpamFavoriteDto>> GetFavoritesAsync()
		{
			return _webClient.GetSpamFavorites(_accountId);
		}

		public async Task<SpamFavoriteDto> AddFavoriteAsync(SpamFavoriteCreateDto dto)
		{
			SpamFavoriteDto result = await _webClient.AddSpamFavorite(_accountId, dto);
			this.FavoriteAdded?.Invoke(this, result);
			return result;
		}

		public async Task RemoveFavoriteAsync(Guid favoriteId)
		{
			await _webClient.RemoveSpamFavorite(_accountId, favoriteId);
			this.FavoriteRemoved?.Invoke(this, favoriteId);
		}

		public Task<SpamDetailDto> UseFavoriteAsync(Guid favoriteId, SpamUsageRequestDto request = null)
		{
			return _webClient.UseSpamFavorite(_accountId, favoriteId, request);
		}

		public Task ReorderFavoritesAsync(SpamFavoriteReorderDto dto)
		{
			return _webClient.ReorderSpamFavorites(_accountId, dto);
		}
	}
}
