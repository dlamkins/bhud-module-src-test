using System.Threading.Tasks;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Services
{
	public interface IDataService
	{
		Task LoadAsync();

		Task DownloadRepositoryFilesAsync();

		void RegisterRepository(IRepository repository);

		string GetFilePath(string fileName);

		void SaveFile(string fileName, object data);

		Task SaveFileAsync(string fileName, object data);

		Task<T> LoadFromFileAsync<T>(string fileName) where T : class;
	}
}
