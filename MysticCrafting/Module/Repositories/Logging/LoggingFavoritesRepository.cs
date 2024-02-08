using System.Collections.Generic;
using System.Linq;
using Blish_HUD;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingFavoritesRepository : LoggingRepository, IFavoritesRepository, IRepository
	{
		private static readonly Logger Logger = Logger.GetLogger<IFavoritesRepository>();

		private readonly IFavoritesRepository _favoritesRepository;

		public LoggingFavoritesRepository(IFavoritesRepository favoritesRepository)
			: base(favoritesRepository)
		{
			_favoritesRepository = favoritesRepository;
		}

		public IList<int> GetAll()
		{
			IList<int> ids = _favoritesRepository.GetAll();
			if (ids == null || !ids.Any())
			{
				Logger.Info("No favorites were found.");
			}
			return ids;
		}

		public bool IsFavorite(int itemId)
		{
			return _favoritesRepository.IsFavorite(itemId);
		}

		public void RemoveFavorite(int itemId)
		{
			_favoritesRepository.RemoveFavorite(itemId);
		}

		public void SaveFavorite(int itemId)
		{
			_favoritesRepository.SaveFavorite(itemId);
		}
	}
}
