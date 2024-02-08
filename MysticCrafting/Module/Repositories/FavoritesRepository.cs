using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class FavoritesRepository : IFavoritesRepository, IRepository
	{
		private readonly IDataService _dataService;

		private string DirectoryPath = "Local";

		public bool LocalOnly => true;

		private IList<int> FavoriteIds { get; set; } = new List<int>();


		public bool Loaded { get; set; }

		public string FileName => DirectoryPath + "\\favorites.json";

		public FavoritesRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			if (!File.Exists(_dataService.GetFilePath(FileName)))
			{
				string fullDirPath = _dataService.GetFilePath(DirectoryPath);
				if (!Directory.Exists(fullDirPath))
				{
					Directory.CreateDirectory(fullDirPath);
				}
				return "No favorites were found.";
			}
			FavoriteIds = (await _dataService.LoadFromFileAsync<IList<int>>(FileName)) ?? new List<int>();
			return $"{FavoriteIds.Count} favorites loaded";
		}

		public IList<int> GetAll()
		{
			return FavoriteIds;
		}

		public bool IsFavorite(int itemId)
		{
			return FavoriteIds.Contains(itemId);
		}

		public void SaveFavorite(int itemId)
		{
			FavoriteIds.Add(itemId);
			Save();
		}

		public void RemoveFavorite(int itemId)
		{
			FavoriteIds.Remove(itemId);
			Save();
		}

		private void Save()
		{
			_dataService.SaveFile(FileName, FavoriteIds);
		}
	}
}
