using System.Threading.Tasks;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Services
{
	public interface IDataService
	{
		string DatabaseFilePath { get; }

		Task LoadAsync();

		Task DownloadRepositoryFilesAsync();

		Task CopyDatabaseResource();

		void RegisterRepository(IRepository repository);

		string GetFilePath(string fileName);

		void SaveFile(string fileName, object data);

		void DeleteFile(string fileName);

		Task SaveFileAsync(string fileName, object data);

		Task<T> LoadFromFileAsync<T>(string fileName) where T : class;
	}
}
