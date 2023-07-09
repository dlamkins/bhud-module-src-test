using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.IO;
using Estreya.BlishHUD.Shared.Json.Converter;
using Estreya.BlishHUD.Shared.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.Shared.Services
{
	public abstract class FilesystemAPIService<T> : APIService<T>
	{
		private const string LAST_UPDATED_FILE_NAME = "last_updated.txt";

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private readonly string _baseModulePath;

		protected JsonSerializerSettings _serializerSettings;

		protected abstract string BASE_FOLDER_STRUCTURE { get; }

		protected abstract string FILE_NAME { get; }

		protected string DirectoryPath => Path.Combine(_baseModulePath, BASE_FOLDER_STRUCTURE);

		protected virtual bool ForceAPI => false;

		protected FilesystemAPIService(Gw2ApiManager apiManager, APIServiceConfiguration configuration, string baseModulePath)
			: base(apiManager, configuration)
		{
			CreateJsonSettings();
			_baseModulePath = baseModulePath;
		}

		private void CreateJsonSettings()
		{
			_serializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				Converters = new JsonConverter[3]
				{
					new RenderUrlConverter(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection()),
					new NullableRenderUrlConverter(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection()),
					new StringEnumConverter()
				}
			};
		}

		protected override async Task Load()
		{
			_ = 6;
			try
			{
				bool forceAPI = ForceAPI;
				bool canLoadFiles = !forceAPI && CanLoadFiles();
				bool flag = !forceAPI;
				if (flag)
				{
					flag = await ShouldLoadFiles();
				}
				bool shouldLoadFiles = flag;
				if (forceAPI)
				{
					Logger.Debug("Force API is active.");
				}
				if (!forceAPI && canLoadFiles)
				{
					try
					{
						base.Loading = true;
						string filePath = Path.Combine(DirectoryPath, FILE_NAME);
						if (await OnBeforeFilesystemLoad(filePath))
						{
							return;
						}
						ReportProgress("Loading file content...");
						using FileStream stream = FileUtil.ReadStream(filePath);
						using ReadProgressStream progressStream = new ReadProgressStream(stream);
						progressStream.ProgressChanged += delegate(object s, ReadProgressStream.ProgressChangedEventArgs e)
						{
							ReportProgress($"Parsing json... {Math.Round(e.Progress, 0)}%");
						};
						JsonSerializer serializer = JsonSerializer.CreateDefault(_serializerSettings);
						using StreamReader sr = new StreamReader(progressStream);
						using JsonReader reader = new JsonTextReader(sr);
						List<T> entities = serializer.Deserialize<List<T>>(reader);
						await OnAfterFilesystemLoad(entities);
						using (_apiObjectListLock.Lock())
						{
							base.APIObjectList.Clear();
							base.APIObjectList.AddRange(entities);
						}
						SignalUpdated();
					}
					catch (Exception ex2)
					{
						Logger.Warn(ex2, "Could not load from filesystem. Fallback to API.");
						forceAPI = true;
					}
					finally
					{
						base.Loading = false;
						SignalCompletion();
					}
				}
				if (forceAPI || !shouldLoadFiles)
				{
					await LoadFromAPI(!canLoadFiles);
					if (!base.CancellationToken.IsCancellationRequested)
					{
						try
						{
							base.Loading = true;
							ReportProgress("Saving...");
							await OnAfterLoadFromAPIBeforeSave();
							await Save();
							await OnAfterLoadFromAPIAfterSave();
						}
						finally
						{
							base.Loading = false;
						}
					}
				}
				Logger.Debug("Loaded {0} entities.", new object[1] { base.APIObjectList.Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading entites:");
			}
		}

		protected virtual Task<bool> OnBeforeFilesystemLoad(string filePath)
		{
			return Task.FromResult(result: false);
		}

		protected virtual Task OnAfterFilesystemLoad(List<T> loadedEntites)
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnAfterLoadFromAPIBeforeSave()
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnAfterLoadFromAPIAfterSave()
		{
			return Task.CompletedTask;
		}

		private bool CanLoadFiles()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				return false;
			}
			if (!File.Exists(Path.Combine(DirectoryPath, FILE_NAME)))
			{
				return false;
			}
			return true;
		}

		private async Task<bool> ShouldLoadFiles()
		{
			if (!CanLoadFiles())
			{
				return false;
			}
			string lastUpdatedFilePath = Path.Combine(DirectoryPath, "last_updated.txt");
			if (File.Exists(lastUpdatedFilePath))
			{
				if (!DateTime.TryParseExact(await FileUtil.ReadStringAsync(lastUpdatedFilePath), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
				{
					Logger.Debug("Failed parsing last updated.");
					return false;
				}
				return true;
			}
			return false;
		}

		protected override async Task Save()
		{
			Directory.CreateDirectory(DirectoryPath);
			using (await _apiObjectListLock.LockAsync())
			{
				string itemJson = JsonConvert.SerializeObject(base.APIObjectList, Formatting.Indented, _serializerSettings);
				await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, FILE_NAME), itemJson);
			}
			await CreateLastUpdatedFile();
		}

		private async Task CreateLastUpdatedFile()
		{
			await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "last_updated.txt"), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
		}

		protected override Task DoClear()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				return Task.CompletedTask;
			}
			Directory.Delete(DirectoryPath, recursive: true);
			return Task.CompletedTask;
		}
	}
}
