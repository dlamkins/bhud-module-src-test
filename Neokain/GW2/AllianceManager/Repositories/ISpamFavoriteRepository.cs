using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Repositories
{
	public interface ISpamFavoriteRepository
	{
		event EventHandler<SpamFavoriteDto> FavoriteAdded;

		event EventHandler<Guid> FavoriteRemoved;

		Task<List<SpamFavoriteDto>> GetFavoritesAsync();

		Task<SpamFavoriteDto> AddFavoriteAsync(SpamFavoriteCreateDto dto);

		Task RemoveFavoriteAsync(Guid favoriteId);

		Task<SpamDetailDto> UseFavoriteAsync(Guid favoriteId, SpamUsageRequestDto request = null);

		Task ReorderFavoritesAsync(SpamFavoriteReorderDto dto);
	}
}
