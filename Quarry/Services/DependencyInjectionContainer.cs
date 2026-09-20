using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Services.Factories;

namespace Quarry.Services
{
	public class DependencyInjectionContainer
	{
		private readonly Gw2ApiManager gw2ApiManager;

		private readonly ContentsManager contentsManager;

		private readonly ContentService contentService;

		private readonly DirectoriesManager directoriesManager;

		private readonly Logger logger;

		private readonly GraphicsService graphicsService;

		public IAchievementTrackerService AchievementTrackerService { get; set; }

		public IAchievementCardFactory AchievementCardFactory { get; set; }

		public IAchievementItemOverviewFactory AchievementItemOverviewFactory { get; set; }

		public IAchievementService AchievementService { get; set; }

		public ITextureService TextureService { get; set; }

		public IPersistenceService PersistenceService { get; private set; }

		public IWikiSubpageDataService WikiSubpageDataService { get; set; }

		public IInspectorWindowManager InspectorWindowManager { get; set; }

		public IFormattedLabelHtmlService FormattedLabelHtmlService { get; set; }

		public IExternalImageService ExternalImageService { get; set; }

		public ICurrentMapService CurrentMapService { get; set; }

		public IHereService HereService { get; set; }

		public IHereExclusionService HereExclusionService { get; private set; }

		public SettingEntry<int> HereCap { get; private set; }

		public IPathingBridge PathingBridge { get; private set; }

		public IHuntService HuntService { get; private set; }

		public ISessionSummaryService SessionSummaryService { get; set; }

		public IBitAlignmentService BitAlignmentService { get; private set; }

		public IMarkerPackIndexService MarkerPackIndexService { get; private set; }

		public INearestObjectiveService NearestObjectiveService { get; private set; }

		public IWikiLocationService WikiLocationService { get; private set; }

		public DependencyInjectionContainer(Gw2ApiManager gw2ApiManager, ContentsManager contentsManager, ContentService contentService, DirectoriesManager directoriesManager, Logger logger, GraphicsService graphicsService)
		{
			this.gw2ApiManager = gw2ApiManager;
			this.contentsManager = contentsManager;
			this.contentService = contentService;
			this.directoriesManager = directoriesManager;
			this.logger = logger;
			this.graphicsService = graphicsService;
		}

		public async Task InitializeAsync(SettingEntry<bool> autoSave, SettingEntry<bool> limitAchievement, SettingEntry<bool> huntMode, SettingEntry<HereGuidanceFilter> hereGuidanceFilter, SettingEntry<int> hereCap, CancellationToken cancellationToken = default(CancellationToken))
		{
			ExternalImageService = new ExternalImageService(graphicsService, logger);
			TextureService = new TextureService(contentService, contentsManager);
			CurrentMapService = new CurrentMapService(gw2ApiManager, logger);
			AchievementService achievementService = (AchievementService)(AchievementService = new AchievementService(contentsManager, gw2ApiManager, logger, directoriesManager, () => PersistenceService, TextureService, () => BitAlignmentService));
			BitAlignmentService = new BitAlignmentService(AchievementService, gw2ApiManager, logger);
			MarkerPackIndexService = new MarkerPackIndexService(directoriesManager, logger);
			WikiSubpageDataService = new WikiSubpageDataService(contentsManager, logger);
			WikiLocationService = new WikiLocationService(AchievementService, WikiSubpageDataService, BitAlignmentService, CurrentMapService, logger);
			NearestObjectiveService = new NearestObjectiveService(MarkerPackIndexService, AchievementService, BitAlignmentService, WikiLocationService, CurrentMapService, logger);
			HereExclusionService hereExclusionService = (HereExclusionService)(HereExclusionService = new HereExclusionService(logger));
			HereService = new HereService(AchievementService, CurrentMapService, gw2ApiManager, BitAlignmentService, MarkerPackIndexService, NearestObjectiveService, HereExclusionService, hereGuidanceFilter, logger, contentsManager);
			HereCap = hereCap;
			PathingBridge = new PathingBridge(logger);
			FormattedLabelHtmlService = new FormattedLabelHtmlService(contentsManager, ExternalImageService);
			AchievementTrackerService achievementTrackerService = (AchievementTrackerService)(AchievementTrackerService = new AchievementTrackerService(logger, limitAchievement));
			SessionSummaryService = new SessionSummaryService(AchievementService, BitAlignmentService, AchievementTrackerService, logger);
			HuntService huntService = (HuntService)(HuntService = new HuntService(PathingBridge, MarkerPackIndexService, AchievementTrackerService, CurrentMapService, huntMode, logger));
			AchievementCardFactory = new AchievementCardFactory(AchievementTrackerService, AchievementService, TextureService, HuntService, NearestObjectiveService, CurrentMapService);
			InspectorWindowManager = new InspectorWindowManager(graphicsService, contentsManager, AchievementService, WikiSubpageDataService, BitAlignmentService, HuntService, NearestObjectiveService, CurrentMapService, FormattedLabelHtmlService, ExternalImageService, AchievementTrackerService);
			AchievementItemOverviewFactory = new AchievementItemOverviewFactory(AchievementCardFactory, AchievementService, BitAlignmentService);
			PersistenceService = new PersistenceService(directoriesManager, achievementTrackerService, logger, achievementService, HuntService, HereExclusionService, autoSave);
			await HereService.LoadAsync(cancellationToken);
			await achievementService.LoadAsync(cancellationToken);
			await WikiSubpageDataService.LoadAsync(cancellationToken);
			Task.Run(() => MarkerPackIndexService.LoadAsync(cancellationToken), cancellationToken);
			achievementTrackerService.Load(PersistenceService);
			huntService.Load(PersistenceService);
			hereExclusionService.Load(PersistenceService);
		}
	}
}
