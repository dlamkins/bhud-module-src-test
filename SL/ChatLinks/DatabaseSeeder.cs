using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
using GuildWars2.Collections;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;
using GuildWars2.Hero.Crafting.Recipes;
using GuildWars2.Hero.Equipment.Dyes;
using GuildWars2.Hero.Equipment.Finishers;
using GuildWars2.Hero.Equipment.Gliders;
using GuildWars2.Hero.Equipment.JadeBots;
using GuildWars2.Hero.Equipment.MailCarriers;
using GuildWars2.Hero.Equipment.Miniatures;
using GuildWars2.Hero.Equipment.Novelties;
using GuildWars2.Hero.Equipment.Outfits;
using GuildWars2.Hero.Equipment.Wardrobe;
using GuildWars2.Items;
using GuildWars2.Pvp.MistChampions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SL.ChatLinks.StaticFiles;
using SL.ChatLinks.Storage;
using SL.ChatLinks.Storage.Metadata;
using SL.Common;

namespace SL.ChatLinks
{
	public sealed class DatabaseSeeder : IDisposable
	{
		private readonly ILogger<DatabaseSeeder> _logger;

		private readonly IOptions<DatabaseOptions> _options;

		private readonly IDbContextFactory _contextFactory;

		private readonly IEventAggregator _eventAggregator;

		private readonly Gw2Client _gw2Client;

		private readonly StaticDataClient _staticDataClient;

		private readonly ILocale _locale;

		private readonly SemaphoreSlim _syncSemaphore = new SemaphoreSlim(1, 1);

		private Task? _currentSync;

		public bool IsSynchronizing
		{
			get
			{
				Task currentSync = _currentSync;
				if (currentSync != null)
				{
					return !currentSync.IsCompleted;
				}
				return false;
			}
		}

		public DatabaseSeeder(ILogger<DatabaseSeeder> logger, IOptions<DatabaseOptions> options, IDbContextFactory contextFactory, IEventAggregator eventAggregator, Gw2Client gw2Client, StaticDataClient staticDataClient, ILocale locale)
		{
			ThrowHelper.ThrowIfNull(eventAggregator, "eventAggregator");
			_logger = logger;
			_options = options;
			_contextFactory = contextFactory;
			_eventAggregator = eventAggregator;
			_gw2Client = gw2Client;
			_staticDataClient = staticDataClient;
			_locale = locale;
			eventAggregator.Subscribe(new Func<LocaleChanged, Task>(OnLocaleChanged));
			eventAggregator.Subscribe(new Func<HourStarted, Task>(OnHourStarted));
		}

		private async Task OnLocaleChanged(LocaleChanged args)
		{
			LocaleChanged args2 = args;
			await Task.Run(async delegate
			{
				await Migrate(args2.Language).ConfigureAwait(continueOnCapturedContext: false);
				await Sync(args2.Language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await Vacuum(args2.Language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await Optimize(args2.Language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task OnHourStarted(HourStarted args)
		{
			await Task.Run(async delegate
			{
				await Migrate(_locale.Current).ConfigureAwait(continueOnCapturedContext: false);
				await Sync(_locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await Vacuum(_locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await Optimize(_locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async ValueTask<DataManifest?> DataManifest()
		{
			try
			{
				string path = Path.Combine(_options.Value.Directory, "manifest.json");
				if (!File.Exists(path))
				{
					return null;
				}
				using FileStream stream = File.OpenRead(path);
				return await JsonSerializer.DeserializeAsync<DataManifest>(stream).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason)
			{
				_logger.LogError(reason, "Failed to read manifest.");
				return null;
			}
		}

		public async ValueTask SaveManifest(DataManifest manifest)
		{
			try
			{
				string path = Path.Combine(_options.Value.Directory, "manifest.json");
				using FileStream stream = File.OpenWrite(path);
				await JsonSerializer.SerializeAsync(stream, manifest).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason)
			{
				_logger.LogError(reason, "Failed to save updated manifest.");
			}
		}

		private bool IsEmpty(Database database)
		{
			FileInfo fileInfo = new FileInfo(Path.Combine(_options.Value.Directory, database.Name));
			if (fileInfo != null && (!fileInfo.Exists || fileInfo.Length == 0L))
			{
				return true;
			}
			return false;
		}

		public async Task Migrate(Language language)
		{
			ThrowHelper.ThrowIfNull(language, "language");
			DataManifest currentDataManifest = (await DataManifest().ConfigureAwait(continueOnCapturedContext: false)) ?? new DataManifest
			{
				Version = 1,
				Databases = new Dictionary<string, Database>()
			};
			Database currentDatabase;
			bool flag = !currentDataManifest.Databases.TryGetValue(language.Alpha2Code, out currentDatabase) || currentDatabase.SchemaVersion != ChatLinksContext.SchemaVersion || IsEmpty(currentDatabase);
			if (!flag)
			{
				flag = !(await IsReady(currentDatabase).ConfigureAwait(continueOnCapturedContext: false));
			}
			if (flag)
			{
				Database seedDatabase = await DownloadDatabase(language).ConfigureAwait(continueOnCapturedContext: false);
				if ((object)seedDatabase != null)
				{
					currentDatabase = seedDatabase;
					currentDataManifest.Databases[language.Alpha2Code] = seedDatabase;
					await SaveManifest(currentDataManifest).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			if ((object)currentDatabase == null)
			{
				_logger.LogWarning("No usable database found for language {Language}.", language.Alpha2Code);
				return;
			}
			ChatLinksContext context = _contextFactory.CreateDbContext(currentDatabase.Name);
			if (await HasPendingMigrations(context).ConfigureAwait(continueOnCapturedContext: false))
			{
				{
					ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						await context.Database.MigrateAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					finally
					{
						IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
						if (asyncDisposable != null)
						{
							await asyncDisposable.DisposeAsync();
						}
					}
				}
				currentDatabase.SchemaVersion = ChatLinksContext.SchemaVersion;
				await SaveManifest(currentDataManifest).ConfigureAwait(continueOnCapturedContext: false);
				await _eventAggregator.PublishAsync(new DatabaseMigrated()).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private static async Task<bool> HasPendingMigrations(ChatLinksContext context)
		{
			return (await context.Database.GetPendingMigrationsAsync().ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		private async Task<bool> IsReady(Database database)
		{
			_ = 1;
			try
			{
				ChatLinksContext context = _contextFactory.CreateDbContext(database.Name);
				ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					return !(await HasPendingMigrations(context).ConfigureAwait(continueOnCapturedContext: false));
				}
				finally
				{
					IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
					if (asyncDisposable != null)
					{
						await asyncDisposable.DisposeAsync();
					}
				}
			}
			catch (SqliteException)
			{
				return false;
			}
		}

		private async Task<Database?> DownloadDatabase(Language language)
		{
			Language language2 = language;
			SeedDatabase seedDatabase = (await _staticDataClient.GetSeedIndex(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false)).Databases.OrderByDescending((SeedDatabase seed) => seed.SchemaVersion).FirstOrDefault((SeedDatabase seed) => seed.SchemaVersion <= ChatLinksContext.SchemaVersion && seed.Language == language2.Alpha2Code);
			if ((object)seedDatabase == null)
			{
				return null;
			}
			string destination = Path.Combine(_options.Value.Directory, seedDatabase.Name);
			await _staticDataClient.Download(seedDatabase, destination, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			return new Database
			{
				Name = seedDatabase.Name,
				SchemaVersion = seedDatabase.SchemaVersion
			};
		}

		public async Task Optimize(Language language, CancellationToken cancellationToken)
		{
			ChatLinksContext context = _contextFactory.CreateDbContext(language);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await context.Database.ExecuteSqlRawAsync("PRAGMA optimize;", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		public async Task Vacuum(Language language, CancellationToken cancellationToken)
		{
			ChatLinksContext context = _contextFactory.CreateDbContext(language);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await context.Database.ExecuteSqlRawAsync("VACUUM;", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		public async Task Sync(Language language, CancellationToken cancellationToken)
		{
			Language language2 = language;
			await _syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (_currentSync == null || _currentSync!.IsCompleted)
				{
					_currentSync = Task.Run(async delegate
					{
						ChatLinksContext context = _contextFactory.CreateDbContext(language2);
						ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
						try
						{
							context.ChangeTracker.AutoDetectChangesEnabled = false;
							await Seed(context, language2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						finally
						{
							IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
							if (asyncDisposable != null)
							{
								await asyncDisposable.DisposeAsync();
							}
						}
					}, cancellationToken);
				}
			}
			finally
			{
				_syncSemaphore.Release();
			}
			await _currentSync!.ConfigureAwait(continueOnCapturedContext: false);
			await _eventAggregator.PublishAsync(new DatabaseSyncCompleted(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task SeedAll()
		{
			Directory.CreateDirectory(_options.Value.Directory);
			DataManifest manifest = new DataManifest
			{
				Version = 1,
				Databases = new Dictionary<string, Database>()
			};
			Language[] array = new Language[4]
			{
				Language.English,
				Language.German,
				Language.French,
				Language.Spanish
			};
			foreach (Language language in array)
			{
				Database database = new Database
				{
					Name = _options.Value.DatabaseFileName(language),
					SchemaVersion = ChatLinksContext.SchemaVersion
				};
				ChatLinksContext context = _contextFactory.CreateDbContext(database.Name);
				{
					ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						await context.Database.MigrateAsync().ConfigureAwait(continueOnCapturedContext: false);
						await Seed(context, language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
						await Vacuum(language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
						await Optimize(language, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
					}
					finally
					{
						IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
						if (asyncDisposable != null)
						{
							await asyncDisposable.DisposeAsync();
						}
					}
				}
				manifest.Databases[language.Alpha2Code] = database;
			}
			await SaveManifest(manifest).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task Seed(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, int> dictionary2 = dictionary;
			dictionary2["items"] = await SeedItems(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary3 = dictionary;
			dictionary3["skins"] = await SeedSkins(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary4 = dictionary;
			dictionary4["recipes"] = await SeedRecipes(context, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary5 = dictionary;
			dictionary5["colors"] = await SeedColors(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary6 = dictionary;
			dictionary6["finishers"] = await SeedFinishers(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary7 = dictionary;
			dictionary7["gliders"] = await SeedGliders(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary8 = dictionary;
			dictionary8["jade_bots"] = await SeedJadeBots(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary9 = dictionary;
			dictionary9["mail_carriers"] = await SeedMailCarriers(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary10 = dictionary;
			dictionary10["miniatures"] = await SeedMiniatures(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary11 = dictionary;
			dictionary11["mist_champions"] = await SeedMistChampions(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary12 = dictionary;
			dictionary12["novelties"] = await SeedNovelties(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary13 = dictionary;
			dictionary13["outfits"] = await SeedOutfits(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary14 = dictionary;
			dictionary14["achievements"] = await SeedAchievements(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary15 = dictionary;
			dictionary15["achievement_categories"] = await SeedAchievementCategories(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dictionary16 = dictionary;
			dictionary16["achievement_groups"] = await SeedAchievementGroups(context, language, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await _eventAggregator.PublishAsync(new DatabaseSeeded(language, dictionary), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<int> SeedItems(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding items.");
			IImmutableValueSet<int> index = await _gw2Client.Items.GetItemsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} items in the API.", index.Count);
			index = index.Except(await context.Items.Select((Item item) => item.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} items.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("items", report));
				});
				int count = 0;
				await foreach (Item item2 in _gw2Client.Items.GetItemsBulk(index, language, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, in cancellationToken).ValueOnly(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					context.Add(item2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} items.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedSkins(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding skins.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Wardrobe.GetSkinsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} skins in the API.", index.Count);
			index = index.Except(await context.Skins.Select((EquipmentSkin skin) => skin.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} skins.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("skins", report));
				});
				int count = 0;
				await foreach (EquipmentSkin skin2 in _gw2Client.Hero.Equipment.Wardrobe.GetSkinsBulk(index, language, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, in cancellationToken).ValueOnly(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					context.Add(skin2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} skins.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedColors(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding colors.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Dyes.GetColorsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} colors in the API.", index.Count);
			List<int> existing = await context.Colors.Select((DyeColor color) => color.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			index = index.Except(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} colors.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Dyes.GetColors(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false)).Where((DyeColor color) => index.Contains(color.Id)), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} colors.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedRecipes(ChatLinksContext context, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding recipes.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Crafting.Recipes.GetRecipesIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} recipes in the API.", index.Count);
			index = index.Except(await context.Recipes.Select((Recipe recipe) => recipe.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} recipes.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("recipes", report));
				});
				int count = 0;
				await foreach (Recipe recipe2 in _gw2Client.Hero.Crafting.Recipes.GetRecipesBulk(index, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, in cancellationToken).ValueOnly(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					context.Add(recipe2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} recipes.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedFinishers(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding finishers.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Finishers.GetFinishersIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} finishers in the API.", index.Count);
			index = index.Except(await context.Finishers.Select((Finisher finisher) => finisher.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} finishers.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.Finishers.GetFinishersByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} finishers.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedGliders(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding gliders.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Gliders.GetGliderSkinsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} gliders in the API.", index.Count);
			index = index.Except(await context.Gliders.Select((GliderSkin glider) => glider.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} gliders.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.Gliders.GetGliderSkinsByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} gliders.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedJadeBots(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding jade bots.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.JadeBots.GetJadeBotSkinsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} jade bots in the API.", index.Count);
			index = index.Except(await context.JadeBots.Select((JadeBotSkin jadeBot) => jadeBot.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} jade bots.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.JadeBots.GetJadeBotSkinsByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} jade bots.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMailCarriers(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding mail carriers.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.MailCarriers.GetMailCarriersIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} mail carriers in the API.", index.Count);
			index = index.Except(await context.MailCarrriers.Select((MailCarrier mailCarrier) => mailCarrier.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} mail carriers.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.MailCarriers.GetMailCarriersByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} mail carriers.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMiniatures(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding miniatures.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Miniatures.GetMiniaturesIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} miniatures in the API.", index.Count);
			List<int> existing = await context.Miniatures.Select((Miniature miniature) => miniature.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			index = index.Except(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} miniatures.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Miniatures.GetMiniatures(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false)).Where((Miniature miniature) => index.Contains(miniature.Id)), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} miniatures.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMistChampions(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding mist champions.");
			IImmutableValueSet<MistChampion> champions = await _gw2Client.Pvp.GetMistChampions(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			IImmutableValueSet<int> index = ImmutableValueSet.Create(new ReadOnlySpan<int>(champions.SelectMany((MistChampion champion) => champion.Skins.Select((MistChampionSkin skin) => skin.Id)).ToArray()));
			_logger.LogDebug("Found {Count} mist champions in the API.", index.Count);
			List<int> existing = await context.MistChampions.Select((MistChampionSkin mistChampion) => mistChampion.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			index = index.Except(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} mist champions.", index.Count);
				List<MistChampionSkin> mistChampions = (from skin in champions.SelectMany((MistChampion champion) => champion.Skins)
					where index.Contains(skin.Id)
					select skin).ToList();
				await context.AddRangeAsync(mistChampions, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} mist champions.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedNovelties(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding novelties.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Novelties.GetNoveltiesIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} novelties in the API.", index.Count);
			List<int> existing = await context.Novelties.Select((Novelty novelty) => novelty.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			index = index.Except(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} novelties.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Novelties.GetNovelties(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false)).Where((Novelty novelty) => index.Contains(novelty.Id)), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} novelties.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedOutfits(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding outfits.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Equipment.Outfits.GetOutfitsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} outfits in the API.", index.Count);
			List<int> existing = await context.Outfits.Select((Outfit outfit) => outfit.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			index = index.Except(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} outfits.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Outfits.GetOutfits(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false)).Where((Outfit outfit) => index.Contains(outfit.Id)), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} outfits.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedAchievements(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding achievements.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Achievements.GetAchievementsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} achievements in the API.", index.Count);
			index = index.Except(await context.Achievements.Select((Achievement achievement) => achievement.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} achievements.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("achievements", report));
				});
				int count = 0;
				await foreach (Achievement achievement2 in _gw2Client.Hero.Achievements.GetAchievementsBulk(index, language, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, in cancellationToken).ValueOnly(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					context.Add(achievement2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} achievements.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedAchievementCategories(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding achievement categories.");
			IImmutableValueSet<int> index = await _gw2Client.Hero.Achievements.GetAchievementCategoriesIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} achievement categories in the API.", index.Count);
			List<int> existing = await context.AchievementCategories.Select((AchievementCategory category) => category.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Start seeding {Count} achievement categories.", index.Count);
			foreach (AchievementCategory category2 in await _gw2Client.Hero.Achievements.GetAchievementCategories(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false))
			{
				context.Entry(category2).State = (existing.Contains(category2.Id) ? EntityState.Modified : EntityState.Added);
			}
			await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DetachAllEntities(context);
			_logger.LogInformation("Finished seeding {Count} achievement categories.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedAchievementGroups(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding achievement groups.");
			IImmutableValueSet<string> index = await _gw2Client.Hero.Achievements.GetAchievementGroupsIndex(cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Found {Count} achievement groups in the API.", index.Count);
			List<string> existing = await context.AchievementGroups.Select((AchievementGroup group) => group.Id).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_logger.LogDebug("Start seeding {Count} achievement groups.", index.Count);
			foreach (AchievementGroup group2 in await _gw2Client.Hero.Achievements.GetAchievementGroups(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false))
			{
				context.Entry(group2).State = (existing.Contains(group2.Id) ? EntityState.Modified : EntityState.Added);
			}
			await context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DetachAllEntities(context);
			_logger.LogInformation("Finished seeding {Count} achievement groups.", index.Count);
			return index.Count;
		}

		private static void DetachAllEntities(ChatLinksContext context)
		{
			foreach (EntityEntry item in context.ChangeTracker.Entries().ToList())
			{
				item.State = EntityState.Detached;
			}
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<HourStarted>(new Func<HourStarted, Task>(OnHourStarted));
			_eventAggregator.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, Task>(OnLocaleChanged));
			_syncSemaphore.Dispose();
		}
	}
}
