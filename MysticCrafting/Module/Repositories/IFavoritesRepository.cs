using System.Collections.Generic;

namespace MysticCrafting.Module.Repositories
{
	public interface IFavoritesRepository : IRepository
	{
		IList<int> GetAll();

		bool IsFavorite(int itemId);

		void SaveFavorite(int itemId);

		void RemoveFavorite(int itemId);
	}
}
