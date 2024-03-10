using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.IO;
using Estreya.BlishHUD.Shared.Json.Converter;
using Estreya.BlishHUD.Shared.Json.Converter.GW2Sharp;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp;
using Gw2Sharp.Json.Converters;
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

		protected JsonSerializerOptions _gw2SharpSerializerOptions;

		protected IFlurlClient _flurlClient;

		protected string _fileRootUrl;

		protected abstract string BASE_FOLDER_STRUCTURE { get; }

		protected abstract string FILE_NAME { get; }

		protected string DirectoryPath => Path.Combine(_baseModulePath, BASE_FOLDER_STRUCTURE);

		protected virtual bool ForceAPI => false;

		protected FilesystemAPIService(Gw2ApiManager apiManager, APIServiceConfiguration configuration, string baseModulePath, IFlurlClient flurlClient, string fileRootUrl)
			: base(apiManager, configuration)
		{
			CreateJsonSettings();
			_baseModulePath = baseModulePath;
			_flurlClient = flurlClient;
			_fileRootUrl = fileRootUrl;
		}

		private void CreateJsonSettings()
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			_serializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				Converters = new Newtonsoft.Json.JsonConverter[3]
				{
					new RenderUrlConverter(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection()),
					new NullableRenderUrlConverter(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection()),
					new StringEnumConverter()
				}
			};
			_gw2SharpSerializerOptions = new JsonSerializerOptions
			{
				AllowTrailingCommas = true,
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase
			};
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new ApiEnumConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new ApiFlagsConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new ApiObjectConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new ApiObjectListConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new CastableTypeConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new DictionaryIntKeyConverter());
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new RenderUrlConverter(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection(), (IGw2Client)new Gw2Client(GameService.Gw2WebApi.get_AnonymousConnection().get_Connection())));
			_gw2SharpSerializerOptions.Converters.Add((System.Text.Json.Serialization.JsonConverter)new TimeSpanConverter());
		}

		protected override async Task Load()
		{
			_ = 9;
			try
			{
				bool forceAPI = ForceAPI;
				if (forceAPI)
				{
					Logger.Debug("Force API is active.");
				}
				bool loadedFromStatic = false;
				if (!forceAPI)
				{
					try
					{
						base.Loading = true;
						ReportProgress("Loading static file content...");
						IProgress<string> progress = new Progress<string>(base.ReportProgress);
						List<T> entities2 = await FetchFromStaticFile(progress, base.CancellationToken);
						if (entities2 != null)
						{
							using (await _apiObjectListLock.LockAsync())
							{
								base.APIObjectList.Clear();
								base.APIObjectList.AddRange(entities2);
							}
							SignalUpdated();
							loadedFromStatic = true;
						}
					}
					catch (Exception ex3)
					{
						Logger.Warn(ex3, "Could not load from static file. Fallback to filesystem cache.");
					}
					finally
					{
						base.Loading = false;
						SignalCompletion();
					}
				}
				bool canLoadFiles = !loadedFromStatic && !forceAPI && CanLoadFiles();
				bool flag = !loadedFromStatic && !forceAPI;
				if (flag)
				{
					flag = await ShouldLoadFiles();
				}
				bool shouldLoadFiles = flag;
				if (!loadedFromStatic && !forceAPI && canLoadFiles)
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
						Newtonsoft.Json.JsonSerializer serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(_serializerSettings);
						using StreamReader sr = new StreamReader(progressStream);
						using JsonReader reader = new JsonTextReader(sr);
						List<T> entities2 = serializer.Deserialize<List<T>>(reader);
						await OnAfterFilesystemLoad(entities2);
						using (await _apiObjectListLock.LockAsync())
						{
							base.APIObjectList.Clear();
							base.APIObjectList.AddRange(entities2);
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
				if (!loadedFromStatic && (forceAPI || !shouldLoadFiles))
				{
					bool result = await LoadFromAPI(!canLoadFiles);
					if (!base.CancellationToken.IsCancellationRequested)
					{
						try
						{
							base.Loading = true;
							ReportProgress("Saving...");
							bool flag2 = result;
							if (flag2 | await OnAfterLoadFromAPIBeforeSave())
							{
								await Save();
								await OnAfterLoadFromAPIAfterSave();
							}
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

		protected virtual Task<List<T>> FetchFromStaticFile(IProgress<string> progress, CancellationToken cancellationToken)
		{
			return Task.FromResult<List<T>>(null);
		}

		protected virtual Task<bool> OnBeforeFilesystemLoad(string filePath)
		{
			return Task.FromResult(result: false);
		}

		protected virtual Task OnAfterFilesystemLoad(List<T> loadedEntites)
		{
			return Task.CompletedTask;
		}

		protected virtual Task<bool> OnAfterLoadFromAPIBeforeSave()
		{
			return Task.FromResult(result: false);
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
