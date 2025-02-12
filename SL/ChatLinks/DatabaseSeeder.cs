using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
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

		public DatabaseSeeder(ILogger<DatabaseSeeder> logger, IOptions<DatabaseOptions> options, IDbContextFactory contextFactory, IEventAggregator eventAggregator, Gw2Client gw2Client, StaticDataClient staticDataClient)
		{
			_logger = logger;
			_options = options;
			_contextFactory = contextFactory;
			_eventAggregator = eventAggregator;
			_gw2Client = gw2Client;
			_staticDataClient = staticDataClient;
			eventAggregator.Subscribe(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
		}

		private async ValueTask OnLocaleChanged(LocaleChanged args)
		{
			LocaleChanged args2 = args;
			await Task.Run(async delegate
			{
				await Migrate(args2.Language);
				await Sync(args2.Language, CancellationToken.None);
			});
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
				return await JsonSerializer.DeserializeAsync<DataManifest>(stream);
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
				await JsonSerializer.SerializeAsync((Stream)stream, manifest, (JsonSerializerOptions?)null, default(CancellationToken));
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
			DataManifest currentDataManifest = (await DataManifest()) ?? new DataManifest
			{
				Version = 1,
				Databases = new Dictionary<string, Database>()
			};
			if (!currentDataManifest.Databases.TryGetValue(language.Alpha2Code, out var currentDatabase) || currentDatabase.SchemaVersion > ChatLinksContext.SchemaVersion || IsEmpty(currentDatabase))
			{
				Database seedDatabase = await DownloadDatabase(language);
				if ((object)seedDatabase != null)
				{
					currentDatabase = seedDatabase;
					currentDataManifest.Databases[language.Alpha2Code] = seedDatabase;
					await SaveManifest(currentDataManifest);
					await _eventAggregator.PublishAsync(new DatabaseDownloaded());
				}
			}
			if ((object)currentDatabase == null)
			{
				_logger.LogWarning("No usable database found for language {Language}.", language.Alpha2Code);
			}
			else
			{
				await using ChatLinksContext context = _contextFactory.CreateDbContext(currentDatabase.Name);
				await context.Database.MigrateAsync();
			}
		}

		private async Task<Database?> DownloadDatabase(Language language)
		{
			Language language2 = language;
			SeedDatabase seedDatabase = (await _staticDataClient.GetSeedIndex(CancellationToken.None)).Databases.SingleOrDefault((SeedDatabase seed) => seed.SchemaVersion == ChatLinksContext.SchemaVersion && seed.Language == language2.Alpha2Code);
			if ((object)seedDatabase == null)
			{
				return null;
			}
			string destination = Path.Combine(_options.Value.Directory, seedDatabase.Name);
			await _staticDataClient.Download(seedDatabase, destination, CancellationToken.None);
			return new Database
			{
				Name = seedDatabase.Name,
				SchemaVersion = seedDatabase.SchemaVersion
			};
		}

		public async Task Sync(Language language, CancellationToken cancellationToken)
		{
			Language language2 = language;
			await _syncSemaphore.WaitAsync(cancellationToken);
			try
			{
				if (_currentSync == null || _currentSync!.IsCompleted)
				{
					_currentSync = Task.Run(async delegate
					{
						await using ChatLinksContext context = _contextFactory.CreateDbContext(language2);
						context.ChangeTracker.AutoDetectChangesEnabled = false;
						await Seed(context, language2, cancellationToken);
					}, cancellationToken);
				}
			}
			finally
			{
				_syncSemaphore.Release();
			}
			await _currentSync;
			await _eventAggregator.PublishAsync(new DatabaseSyncCompleted(), cancellationToken);
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
				await using ChatLinksContext context = _contextFactory.CreateDbContext(database.Name);
				await context.Database.MigrateAsync();
				await Seed(context, language, CancellationToken.None);
				manifest.Databases[language.Alpha2Code] = database;
			}
			await SaveManifest(manifest);
		}

		private async Task Seed(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, int> dictionary2 = dictionary;
			dictionary2["items"] = await SeedItems(context, language, cancellationToken);
			Dictionary<string, int> dictionary3 = dictionary;
			dictionary3["skins"] = await SeedSkins(context, language, cancellationToken);
			Dictionary<string, int> dictionary4 = dictionary;
			dictionary4["recipes"] = await SeedRecipes(context, language, cancellationToken);
			Dictionary<string, int> dictionary5 = dictionary;
			dictionary5["colors"] = await SeedColors(context, language, cancellationToken);
			Dictionary<string, int> dictionary6 = dictionary;
			dictionary6["finishers"] = await SeedFinishers(context, language, cancellationToken);
			Dictionary<string, int> dictionary7 = dictionary;
			dictionary7["gliders"] = await SeedGliders(context, language, cancellationToken);
			Dictionary<string, int> dictionary8 = dictionary;
			dictionary8["jadeBots"] = await SeedJadeBots(context, language, cancellationToken);
			Dictionary<string, int> dictionary9 = dictionary;
			dictionary9["mailCarriers"] = await SeedMailCarriers(context, language, cancellationToken);
			Dictionary<string, int> dictionary10 = dictionary;
			dictionary10["miniatures"] = await SeedMiniatures(context, language, cancellationToken);
			Dictionary<string, int> dictionary11 = dictionary;
			dictionary11["mistChampions"] = await SeedMistChampions(context, language, cancellationToken);
			Dictionary<string, int> dictionary12 = dictionary;
			dictionary12["novelties"] = await SeedNovelties(context, language, cancellationToken);
			Dictionary<string, int> dictionary13 = dictionary;
			dictionary13["outfits"] = await SeedOutfits(context, language, cancellationToken);
			await _eventAggregator.PublishAsync(new DatabaseSeeded(language, dictionary), cancellationToken);
		}

		private async Task<int> SeedItems(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding items.");
			HashSet<int> index = await _gw2Client.Items.GetItemsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} items in the API.", index.Count);
			index.ExceptWith(await context.Items.Select((Item item) => item.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} items.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("items", report));
				});
				int count = 0;
				await foreach (Item item2 in _gw2Client.Items.GetItemsBulk(index, language, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, cancellationToken).ValueOnly(cancellationToken))
				{
					context.Add(item2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} items.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedSkins(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding skins.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Wardrobe.GetSkinsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} skins in the API.", index.Count);
			index.ExceptWith(await context.Skins.Select((EquipmentSkin skin) => skin.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} skins.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("skins", report));
				});
				int count = 0;
				await foreach (EquipmentSkin skin2 in _gw2Client.Hero.Equipment.Wardrobe.GetSkinsBulk(index, language, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, cancellationToken).ValueOnly(cancellationToken))
				{
					context.Add(skin2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} skins.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedColors(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding colors.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Dyes.GetColorsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} colors in the API.", index.Count);
			List<int> existing = await context.Colors.Select((DyeColor color) => color.Id).ToListAsync(cancellationToken);
			index.ExceptWith(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} colors.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Dyes.GetColors(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly()).Where((DyeColor color) => index.Contains(color.Id)), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} colors.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedRecipes(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding recipes.");
			HashSet<int> index = await _gw2Client.Hero.Crafting.Recipes.GetRecipesIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} recipes in the API.", index.Count);
			index.ExceptWith(await context.Recipes.Select((Recipe recipe) => recipe.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} recipes.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					_eventAggregator.Publish(new DatabaseSyncProgress("recipes", report));
				});
				int count = 0;
				await foreach (Recipe recipe2 in _gw2Client.Hero.Crafting.Recipes.GetRecipesBulk(index, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, cancellationToken).ValueOnly(cancellationToken))
				{
					context.Add(recipe2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await context.SaveChangesAsync(cancellationToken);
						DetachAllEntities(context);
					}
				}
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} recipes.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedFinishers(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding finishers.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Finishers.GetFinishersIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} finishers in the API.", index.Count);
			index.ExceptWith(await context.Finishers.Select((Finisher finisher) => finisher.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} finishers.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.Finishers.GetFinishersByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly(), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} finishers.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedGliders(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding gliders.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Gliders.GetGliderSkinsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} gliders in the API.", index.Count);
			index.ExceptWith(await context.Gliders.Select((GliderSkin glider) => glider.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} gliders.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.Gliders.GetGliderSkinsByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly(), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} gliders.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedJadeBots(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding jade bots.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.JadeBots.GetJadeBotSkinsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} jade bots in the API.", index.Count);
			index.ExceptWith(await context.JadeBots.Select((JadeBotSkin jadeBot) => jadeBot.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} jade bots.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.JadeBots.GetJadeBotSkinsByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly(), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} jade bots.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMailCarriers(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding mail carriers.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.MailCarriers.GetMailCarriersIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} mail carriers in the API.", index.Count);
			index.ExceptWith(await context.MailCarrriers.Select((MailCarrier mailCarrier) => mailCarrier.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} mail carriers.", index.Count);
				await context.AddRangeAsync(await _gw2Client.Hero.Equipment.MailCarriers.GetMailCarriersByIds(index, language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly(), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} mail carriers.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMiniatures(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding miniatures.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Miniatures.GetMiniaturesIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} miniatures in the API.", index.Count);
			List<int> existing = await context.Miniatures.Select((GuildWars2.Hero.Equipment.Miniatures.Miniature miniature) => miniature.Id).ToListAsync(cancellationToken);
			index.ExceptWith(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} miniatures.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Miniatures.GetMiniatures(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly()).Where((GuildWars2.Hero.Equipment.Miniatures.Miniature miniature) => index.Contains(miniature.Id)), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} miniatures.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedMistChampions(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding mist champions.");
			HashSet<MistChampion> champions = await _gw2Client.Pvp.GetMistChampions(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly();
			HashSet<int> index = champions.SelectMany((MistChampion champion) => champion.Skins.Select((MistChampionSkin skin) => skin.Id)).ToHashSet();
			_logger.LogDebug("Found {Count} mist champions in the API.", index.Count);
			List<int> existing = await context.MistChampions.Select((MistChampionSkin mistChampion) => mistChampion.Id).ToListAsync(cancellationToken);
			index.ExceptWith(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} mist champions.", index.Count);
				List<MistChampionSkin> mistChampions = (from skin in champions.SelectMany((MistChampion champion) => champion.Skins)
					where index.Contains(skin.Id)
					select skin).ToList();
				await context.AddRangeAsync(mistChampions, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} mist champions.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedNovelties(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding novelties.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Novelties.GetNoveltiesIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} novelties in the API.", index.Count);
			List<int> existing = await context.Novelties.Select((Novelty novelty) => novelty.Id).ToListAsync(cancellationToken);
			index.ExceptWith(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} novelties.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Novelties.GetNovelties(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly()).Where((Novelty novelty) => index.Contains(novelty.Id)), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} novelties.", index.Count);
			return index.Count;
		}

		private async Task<int> SeedOutfits(ChatLinksContext context, Language language, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start seeding outfits.");
			HashSet<int> index = await _gw2Client.Hero.Equipment.Outfits.GetOutfitsIndex(cancellationToken).ValueOnly();
			_logger.LogDebug("Found {Count} outfits in the API.", index.Count);
			List<int> existing = await context.Outfits.Select((Outfit outfit) => outfit.Id).ToListAsync(cancellationToken);
			index.ExceptWith(existing);
			if (index.Count != 0)
			{
				_logger.LogDebug("Start seeding {Count} outfits.", index.Count);
				await context.AddRangeAsync((await _gw2Client.Hero.Equipment.Outfits.GetOutfits(language, MissingMemberBehavior.Undefined, cancellationToken).ValueOnly()).Where((Outfit outfit) => index.Contains(outfit.Id)), cancellationToken);
				await context.SaveChangesAsync(cancellationToken);
				DetachAllEntities(context);
			}
			_logger.LogInformation("Finished seeding {Count} outfits.", index.Count);
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
			_eventAggregator.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
		}
	}
}
