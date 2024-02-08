using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Services
{
	public class DataService : IDataService
	{
		private readonly Logger Logger = Logger.GetLogger<DataService>();

		private const string DirectoryName = "mystic_crafting";

		private const string VersionFileName = "version.txt";

		private string RemoteVersion;

		private string LocalVersion;

		private const string BaseRemoteUrl = "https://bhm.blishhud.com/MysticCrafting.Module/Data/";

		private readonly DirectoriesManager _directoriesManager;

		private IList<IRepository> _repositoryList = new List<IRepository>();

		private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = null,
			PropertyNameCaseInsensitive = true
		};

		private string BaseDirectory => _directoriesManager.GetFullDirectoryPath("mystic_crafting");

		public DataService(DirectoriesManager directoriesManager)
		{
			_directoriesManager = directoriesManager;
		}

		public void RegisterRepository(IRepository repository)
		{
			if (!_repositoryList.Contains(repository))
			{
				_repositoryList.Add(repository);
			}
		}

		public async Task LoadAsync()
		{
			await Task.WhenAll(_repositoryList.Select((IRepository r) => r.LoadAsync()));
		}

		public string GetFilePath(string fileName)
		{
			return Path.Combine(BaseDirectory, fileName);
		}

		public void SaveFile(string fileName, object data)
		{
			SaveFileAsync(fileName, data);
		}

		public async Task SaveFileAsync(string fileName, object data)
		{
			string filePath = GetFilePath(fileName);
			try
			{
				using FileStream createStream = File.Create(filePath);
				await JsonSerializer.SerializeAsync(createStream, data, _serializerOptions);
			}
			catch (Exception ex)
			{
				Logger.Error("Saving file '" + fileName + "' failed with error: " + ex.Message);
			}
		}

		public async Task SaveFileAsync(string fileName, Stream data)
		{
			string filePath = GetFilePath(fileName);
			try
			{
				using FileStream fileStream = File.Create(filePath);
				await data.CopyToAsync(fileStream);
				data.Position = 0L;
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}

		public async Task<T> LoadFromFileAsync<T>(string fileName) where T : class
		{
			string filePath = GetFilePath(fileName);
			if (!File.Exists(filePath))
			{
				Logger.Warn("The file '" + filePath + "' could not be loaded because it does not exist.");
				return await Task.FromResult<T>(null);
			}
			try
			{
				using FileStream data = File.Open(filePath, FileMode.Open);
				return await JsonSerializer.DeserializeAsync<T>(data, _serializerOptions);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
			return await Task.FromResult<T>(null);
		}

		public async Task DownloadRepositoryFilesAsync()
		{
			if (_repositoryList == null || !_repositoryList.Any())
			{
				return;
			}
			bool newVersionAvailable = await NewVersionAvailable();
			foreach (string fileName in from r in _repositoryList
				where !r.LocalOnly
				select r.FileName)
			{
				if (!File.Exists(GetFilePath(fileName)) || newVersionAvailable)
				{
					await DownloadFileAsync<object>(fileName, storeLocally: true);
				}
			}
			await SaveFileAsync("version.txt", RemoteVersion);
			LocalVersion = RemoteVersion;
		}

		public async Task<T> DownloadFileAsync<T>(string fileName, bool storeLocally) where T : class
		{
			string sourceUrl = "https://bhm.blishhud.com/MysticCrafting.Module/Data/" + fileName;
			try
			{
				HttpClient client = new HttpClient();
				try
				{
					HttpResponseMessage response = await client.GetAsync(sourceUrl);
					using Stream input = await response.get_Content().ReadAsStreamAsync();
					if (storeLocally)
					{
						await SaveFileAsync(fileName, input);
					}
					if (typeof(T) == typeof(string))
					{
						return (await response.get_Content().ReadAsStringAsync()) as T;
					}
					return await JsonSerializer.DeserializeAsync<T>(input, _serializerOptions);
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
			return await Task.FromResult<T>(null);
		}

		public async Task<bool> NewVersionAvailable()
		{
			if (!string.IsNullOrWhiteSpace(RemoteVersion) && !string.IsNullOrEmpty(LocalVersion))
			{
				return await Task.FromResult(!RemoteVersion.Equals(LocalVersion, StringComparison.InvariantCultureIgnoreCase));
			}
			RemoteVersion = await DownloadFileAsync<string>("version.txt", storeLocally: false);
			LocalVersion = await LoadFromFileAsync<string>("version.txt");
			if (string.IsNullOrEmpty(RemoteVersion))
			{
				return await Task.FromResult(result: false);
			}
			return !RemoteVersion.Equals(LocalVersion, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
