using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
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

		public static string DatabaseFileName = "data.db";

		private string _remoteVersion;

		private string _localVersion;

		private const string BaseRemoteUrl = "https://bhm.blishhud.com/MysticCrafting.Module/Data/";

		private readonly DirectoriesManager _directoriesManager;

		private IList<IRepository> _repositoryList = new List<IRepository>();

		public static JsonSerializerOptions _serializerOptions;

		public string DatabaseFilePath => GetFilePath(DatabaseFileName);

		public string DatabaseFileResourceName => "MysticCrafting.Module.EmbeddedResources." + DatabaseFileName;

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
				await JsonSerializer.SerializeAsync<object>((Stream)createStream, data, _serializerOptions, default(CancellationToken));
				Logger.Info($"File '{fileName}' saved to disk with size {createStream.Length}.");
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
				Logger.Info($"File '{fileName}' has been saved to the disk with size {fileStream.Length}.");
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}

		public void DeleteFile(string fileName)
		{
			string filePath = GetFilePath(fileName);
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
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
				return await JsonSerializer.DeserializeAsync<T>((Stream)data, _serializerOptions, default(CancellationToken));
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
			return await Task.FromResult<T>(null);
		}

		public async Task CopyDatabaseResource()
		{
			string databaseFileName = "data.db";
			if (!NewDatabaseFileAvailable())
			{
				return;
			}
			using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseFileResourceName);
			if (resourceStream == null)
			{
				Logger.Error("Could not find embedded resource " + databaseFileName);
				return;
			}
			using FileStream file = new FileStream(GetFilePath(databaseFileName), FileMode.Create, FileAccess.Write);
			await resourceStream.CopyToAsync(file);
		}

		private bool NewDatabaseFileAvailable()
		{
			if (!File.Exists(DatabaseFilePath))
			{
				return true;
			}
			using MD5 md5 = MD5.Create();
			using FileStream stream = File.OpenRead(DatabaseFilePath);
			using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseFileResourceName);
			if (resourceStream == null)
			{
				Logger.Error("Could not find embedded resource " + DatabaseFileName);
				return false;
			}
			byte[] first = md5.ComputeHash(stream);
			byte[] resourceFileHash = md5.ComputeHash(resourceStream);
			return !first.SequenceEqual(resourceFileHash);
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
			await SaveFileAsync("version.txt", _remoteVersion);
			_localVersion = _remoteVersion;
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
					return await JsonSerializer.DeserializeAsync<T>(input, _serializerOptions, default(CancellationToken));
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
			if (!string.IsNullOrWhiteSpace(_remoteVersion) && !string.IsNullOrEmpty(_localVersion))
			{
				return await Task.FromResult(!_remoteVersion.Equals(_localVersion, StringComparison.InvariantCultureIgnoreCase));
			}
			_remoteVersion = await DownloadFileAsync<string>("version.txt", storeLocally: false);
			_localVersion = await LoadFromFileAsync<string>("version.txt");
			if (string.IsNullOrEmpty(_remoteVersion))
			{
				return await Task.FromResult(result: false);
			}
			return !_remoteVersion.Equals(_localVersion, StringComparison.InvariantCultureIgnoreCase);
		}

		static DataService()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			JsonSerializerOptions val = new JsonSerializerOptions();
			val.set_PropertyNamingPolicy((JsonNamingPolicy)null);
			val.set_PropertyNameCaseInsensitive(true);
			_serializerOptions = val;
		}
	}
}
