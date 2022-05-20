using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Services.Factories;
using Denrage.AchievementTrackerModule.Services.Factories.ItemDetails;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Services
{
	public class DependencyInjectionContainer
	{
		private readonly Gw2ApiManager gw2ApiManager;

		private readonly ContentsManager contentsManager;

		private readonly ContentService contentService;

		private readonly DirectoriesManager directoriesManager;

		private readonly Logger logger;

		public IItemDetailWindowManager ItemDetailWindowManager { get; set; }

		public IAchievementTrackerService AchievementTrackerService { get; set; }

		public IAchievementListItemFactory AchievementListItemFactory { get; set; }

		public IAchievementItemOverviewFactory AchievementItemOverviewFactory { get; set; }

		public IAchievementService AchievementService { get; set; }

		public IPersistanceService PersistanceService { get; private set; }

		public IAchievementControlProvider AchievementControlProvider { get; set; }

		public IAchievementControlManager AchievementControlManager { get; private set; }

		public IAchievementTableEntryProvider AchievementTableEntryProvider { get; set; }

		public IItemDetailWindowFactory ItemDetailWindowFactory { get; set; }

		public IAchievementDetailsWindowFactory AchievementDetailsWindowFactory { get; set; }

		public IAchievementDetailsWindowManager AchievementDetailsWindowManager { get; set; }

		public ISubPageInformationWindowManager SubPageInformationWindowManager { get; set; }

		public IFormattedLabelHtmlService FormattedLabelHtmlService { get; set; }

		public DependencyInjectionContainer(Gw2ApiManager gw2ApiManager, ContentsManager contentsManager, ContentService contentService, DirectoriesManager directoriesManager, Logger logger)
		{
			this.gw2ApiManager = gw2ApiManager;
			this.contentsManager = contentsManager;
			this.contentService = contentService;
			this.directoriesManager = directoriesManager;
			this.logger = logger;
		}

		public async Task InitializeAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			AchievementService achievementService = (AchievementService)(AchievementService = new AchievementService(contentsManager, gw2ApiManager, logger));
			SubPageInformationWindowManager = new SubPageInformationWindowManager(GameService.Graphics, contentsManager, AchievementService, () => FormattedLabelHtmlService);
			FormattedLabelHtmlService = new FormattedLabelHtmlService(contentsManager, AchievementService, SubPageInformationWindowManager);
			AchievementTrackerService achievementTrackerService = (AchievementTrackerService)(AchievementTrackerService = new AchievementTrackerService(logger));
			AchievementListItemFactory = new AchievementListItemFactory(AchievementTrackerService, contentService, AchievementService);
			AchievementItemOverviewFactory = new AchievementItemOverviewFactory(AchievementListItemFactory, AchievementService);
			AchievementTableEntryProvider = new AchievementTableEntryProvider(AchievementService, FormattedLabelHtmlService, logger);
			ItemDetailWindowFactory = new ItemDetailWindowFactory(contentsManager, AchievementService, AchievementTableEntryProvider, SubPageInformationWindowManager);
			ItemDetailWindowManager itemDetailWindowManager = (ItemDetailWindowManager)(ItemDetailWindowManager = new ItemDetailWindowManager(ItemDetailWindowFactory, AchievementService, logger));
			AchievementControlProvider = new AchievementControlProvider(AchievementService, ItemDetailWindowManager, FormattedLabelHtmlService, contentsManager);
			AchievementControlManager = new AchievementControlManager(AchievementControlProvider);
			AchievementDetailsWindowFactory = new AchievementDetailsWindowFactory(contentsManager, AchievementService, AchievementControlProvider, AchievementControlManager);
			AchievementDetailsWindowManager achievementDetailsWindowManager = (AchievementDetailsWindowManager)(AchievementDetailsWindowManager = new AchievementDetailsWindowManager(AchievementDetailsWindowFactory, AchievementControlManager, AchievementService, logger));
			PersistanceService = new PersistanceService(directoriesManager, achievementDetailsWindowManager, itemDetailWindowManager, achievementTrackerService, logger);
			await achievementService.LoadAsync(cancellationToken);
			achievementDetailsWindowManager.Load(PersistanceService);
			itemDetailWindowManager.Load(PersistanceService);
			achievementTrackerService.Load(PersistanceService);
		}
	}
}
