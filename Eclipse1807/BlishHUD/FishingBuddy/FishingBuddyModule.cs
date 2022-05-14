using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;
using Eclipse1807.BlishHUD.FishingBuddy.Utils;
using Eclipse1807.BlishHUD.FishingBuddy.Views;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Eclipse1807.BlishHUD.FishingBuddy
{
	[Export(typeof(Module))]
	public class FishingBuddyModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FishingBuddyModule));

		internal static FishingBuddyModule ModuleInstance;

		private static Texture2D _imgBorderBlack;

		private static Texture2D _imgBorderJunk;

		private static Texture2D _imgBorderBasic;

		private static Texture2D _imgBorderFine;

		private static Texture2D _imgBorderMasterwork;

		private static Texture2D _imgBorderRare;

		private static Texture2D _imgBorderExotic;

		private static Texture2D _imgBorderAscended;

		private static Texture2D _imgBorderLegendary;

		private static Texture2D _imgBorderX;

		private static Texture2D _imgBorderXY;

		internal static Texture2D _imgDawn;

		internal static Texture2D _imgDay;

		internal static Texture2D _imgDusk;

		internal static Texture2D _imgNight;

		private AsyncCache<int, Map> _mapRepository;

		private static ClickThroughPanel _fishPanel;

		private bool _draggingFishPanel;

		private Point _dragFishPanelStart = Point.get_Zero();

		public static SettingEntry<bool> _dragFishPanel;

		public static SettingEntry<int> _fishImgSize;

		public static SettingEntry<Point> _fishPanelLoc;

		public static readonly string[] _fishPanelOrientations = new string[2]
		{
			Strings.Vertical,
			Strings.Horizontal
		};

		public static SettingEntry<string> _fishPanelOrientation;

		public static readonly string[] _fishPanelDirections = new string[4]
		{
			Strings.Top_left,
			Strings.Top_right,
			Strings.Bottom_left,
			Strings.Bottom_right
		};

		public static SettingEntry<string> _fishPanelDirection;

		public static SettingEntry<string> _fishPanelTooltipDisplay;

		public static SettingEntry<bool> _dragTimeOfDayClock;

		public static SettingEntry<int> _timeOfDayImgSize;

		public static SettingEntry<Point> _timeOfDayPanelLoc;

		public static SettingEntry<bool> _ignoreCaughtFish;

		public static SettingEntry<bool> _includeWorldClass;

		public static SettingEntry<bool> _includeSaltwater;

		public static SettingEntry<bool> _displayUncatchableFish;

		public static SettingEntry<bool> _hideTimeOfDay;

		public static SettingEntry<bool> _settingClockLabel;

		public static SettingEntry<bool> _hideInCombat;

		private List<Fish> catchableFish;

		private FishingMaps _fishingMaps;

		private IEnumerable<AccountAchievement> accountFishingAchievements;

		public static SettingEntry<bool> _showRarityBorder;

		public static readonly string[] _verticalAlignmentOptions = new string[3] { "Top", "Middle", "Bottom" };

		public static SettingEntry<string> _settingClockAlign;

		private Clock _timeOfDayClock;

		private List<Fish> _allFishList;

		internal static Map _currentMap;

		private bool _useAPIToken;

		private readonly SemaphoreSlim _updateFishSemaphore = new SemaphoreSlim(1, 1);

		private readonly double INTERVAL_UPDATE_FISH = 300000.0;

		private double _lastUpdateFish;

		private int _prevMapId;

		private bool MumbleIsAvailable
		{
			get
			{
				if (GameService.Gw2Mumble.get_IsAvailable())
				{
					return GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
				}
				return false;
			}
		}

		private bool UiIsAvailable
		{
			get
			{
				if (MumbleIsAvailable)
				{
					return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
				}
				return false;
			}
		}

		private bool HidingInCombat
		{
			get
			{
				if (MumbleIsAvailable && _hideInCombat.get_Value())
				{
					return GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
				}
				return false;
			}
		}

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		[ImportingConstructor]
		public FishingBuddyModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			_ignoreCaughtFish = settings.DefineSetting<bool>("IgnoreCaughtFish", true, (Func<string>)(() => Strings.SettingsIgnoreCaught), (Func<string>)(() => Strings.SettingsIgnoreCaughtDescription));
			_includeSaltwater = settings.DefineSetting<bool>("IncludeSaltwater", false, (Func<string>)(() => Strings.SettingsDisplaySaltwater), (Func<string>)(() => Strings.SettingsDisplaySaltwaterDescription));
			_includeWorldClass = settings.DefineSetting<bool>("IncludeWorldClass", false, (Func<string>)(() => Strings.SettingsDisplayWorldClass), (Func<string>)(() => Strings.SettingsDisplayWorldClassDescription));
			_displayUncatchableFish = settings.DefineSetting<bool>("DisplayUncatchable", false, (Func<string>)(() => Strings.SettingsDisplayUncatchable), (Func<string>)(() => Strings.SettingsDisplayUncatchableDescription));
			_fishPanelLoc = settings.DefineSetting<Point>("FishPanelLoc", new Point(160, 100), (Func<string>)(() => Strings.FishPanelLocation), (Func<string>)(() => string.Empty));
			_dragFishPanel = settings.DefineSetting<bool>("FishPanelDrag", false, (Func<string>)(() => Strings.FishPanelDrag), (Func<string>)(() => string.Empty));
			_fishImgSize = settings.DefineSetting<int>("FishImgWidth", 30, (Func<string>)(() => Strings.FishPanelSize), (Func<string>)(() => string.Empty));
			_showRarityBorder = settings.DefineSetting<bool>("ShowRarityBorder", true, (Func<string>)(() => Strings.SettingsRarity), (Func<string>)(() => Strings.RarityDescription));
			_fishPanelOrientation = settings.DefineSetting<string>("FishPanelOrientation", _fishPanelOrientations.First(), (Func<string>)(() => Strings.Orientation), (Func<string>)(() => Strings.OrientationDescription));
			_fishPanelDirection = settings.DefineSetting<string>("FishPanelDirection", _fishPanelDirections.First(), (Func<string>)(() => Strings.SettingsDirection), (Func<string>)(() => Strings.SettingsDirectionDescription));
			_ignoreCaughtFish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_includeSaltwater.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_includeWorldClass.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_displayUncatchableFish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_fishPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings<Point>);
			_dragFishPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings<bool>);
			_showRarityBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_fishImgSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings<int>);
			SettingComplianceExtensions.SetRange(_fishImgSize, 16, 96);
			_fishPanelOrientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			_fishPanelDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			_fishPanelTooltipDisplay = settings.DefineSetting<string>("FishPanelTooltipDisplay", "#1\n@2\n@3\n@4\n@5\n@6\n@7", (Func<string>)(() => Strings.TooltipDisplay), (Func<string>)(() => Strings.Default + ": #1\\n@2\\n@3\\n@4\\n@5\\n@6\\n@7\n" + Strings.SimpleTooltip + Strings.CompactTooltip + Strings.FishPanelTooltipDescription + "@#1: " + Strings.FishName + "\n@#2: " + Strings.FishFavoredBait + "\n@#3: " + Strings.FishTimeOfDay + "\n@#4: " + Strings.FishFishingHole + "\n@#5: " + Strings.Achievement + "\n@#6: " + Strings.SettingsRarity + "\n@7:  " + Strings.ReasonForHiding + "\n@#8: " + Strings.FishyNotes + "\n(\\n " + Strings.AddsNewLines + ")"));
			_fishPanelTooltipDisplay.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			_timeOfDayPanelLoc = settings.DefineSetting<Point>("TimeOfDayPanelLoc", new Point(100, 100), (Func<string>)(() => Strings.TimeOfDayPanelLoc), (Func<string>)(() => string.Empty));
			_dragTimeOfDayClock = settings.DefineSetting<bool>("TimeOfDayPanelDrag", false, (Func<string>)(() => Strings.TimeOfDayPanelDrag), (Func<string>)(() => Strings.TimeOfDayPanelDragDescription));
			_timeOfDayImgSize = settings.DefineSetting<int>("TimeImgWidth", 64, (Func<string>)(() => Strings.TimeOfDaySize), (Func<string>)(() => string.Empty));
			_settingClockLabel = settings.DefineSetting<bool>("ClockLabel", false, (Func<string>)(() => Strings.TimeOfDayHideLabel), (Func<string>)(() => Strings.TimeOfDayHideLabelDescription));
			_settingClockAlign = settings.DefineSetting<string>("TimeLabelAlign", "Bottom", (Func<string>)(() => Strings.TimeOfDayLabelPosition), (Func<string>)(() => Strings.TimeOfDayLabelPositionDescription));
			_hideTimeOfDay = settings.DefineSetting<bool>("HideTimeOfDay", false, (Func<string>)(() => Strings.TimeOfDayHide), (Func<string>)(() => Strings.TimeOfDayHideDescription));
			_timeOfDayPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateClockLocation);
			_dragTimeOfDayClock.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			SettingComplianceExtensions.SetRange(_timeOfDayImgSize, 16, 96);
			_timeOfDayImgSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateClockSize);
			_dragTimeOfDayClock.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_hideTimeOfDay.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_settingClockAlign.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateClockLabelAlign);
			_settingClockLabel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateHideClockLabel);
			_hideInCombat = settings.DefineSetting<bool>("HideInCombat", false, (Func<string>)(() => Strings.HideInCombat), (Func<string>)(() => Strings.HideInCombatDescription));
			_hideInCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			catchableFish = new List<Fish>();
			_fishingMaps = new FishingMaps();
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_imgBorderBlack = ContentsManager.GetTexture("border_black.png");
			_imgBorderJunk = ContentsManager.GetTexture("border_junk.png");
			_imgBorderBasic = ContentsManager.GetTexture("border_basic.png");
			_imgBorderFine = ContentsManager.GetTexture("border_fine.png");
			_imgBorderMasterwork = ContentsManager.GetTexture("border_masterwork.png");
			_imgBorderRare = ContentsManager.GetTexture("border_rare.png");
			_imgBorderExotic = ContentsManager.GetTexture("border_exotic.png");
			_imgBorderAscended = ContentsManager.GetTexture("border_ascended.png");
			_imgBorderLegendary = ContentsManager.GetTexture("border_legendary.png");
			_imgBorderX = ContentsManager.GetTexture("border_x.png");
			_imgBorderXY = ContentsManager.GetTexture("border_xy.png");
			_imgDawn = ContentsManager.GetTexture("dawn.png");
			_imgDay = ContentsManager.GetTexture("day.png");
			_imgDusk = ContentsManager.GetTexture("dusk.png");
			_imgNight = ContentsManager.GetTexture("night.png");
			_allFishList = new List<Fish>();
			using (StreamReader r = new StreamReader(ContentsManager.GetFileStream("fish.json")))
			{
				string json = r.ReadToEnd();
				_allFishList.AddRange(JsonConvert.DeserializeObject<List<Fish>>(json));
				Logger.Debug("Fish list: " + string.Join(", ", _allFishList.Select((Fish fish) => fish.Name)));
			}
			_useAPIToken = true;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			((Module)this).OnModuleLoaded(e);
			Clock clock = new Clock();
			((Control)clock).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)clock).set_Visible(_hideTimeOfDay.get_Value());
			((Control)clock).set_Location(_timeOfDayPanelLoc.get_Value());
			((Control)clock).set_Size(new Point(_timeOfDayImgSize.get_Value(), _timeOfDayImgSize.get_Value() + 40));
			clock.Drag = _dragTimeOfDayClock.get_Value();
			clock.HideLabel = _settingClockLabel.get_Value();
			clock.LabelVerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), _settingClockAlign.get_Value());
			_timeOfDayClock = clock;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			DrawIcons();
			_timeOfDayClock.TimeOfDayChanged += OnTimeOfDayChanged;
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			UpdateCadenceUtil.UpdateAsyncWithCadence(GetCurrentMapsFish, gameTime, INTERVAL_UPDATE_FISH, ref _lastUpdateFish);
			if (UiIsAvailable && !HidingInCombat)
			{
				GetCurrentMapTime();
				if (!_hideTimeOfDay.get_Value())
				{
					((Control)_timeOfDayClock).Show();
				}
				((Control)_fishPanel).Show();
			}
			else
			{
				((Control)_timeOfDayClock).Hide();
				((Control)_fishPanel).Hide();
			}
			if (_draggingFishPanel)
			{
				Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragFishPanelStart;
				ClickThroughPanel fishPanel = _fishPanel;
				((Control)fishPanel).set_Location(((Control)fishPanel).get_Location() + nOffset);
				_dragFishPanelStart = GameService.Input.get_Mouse().get_Position();
			}
		}

		protected override void Unload()
		{
			_ignoreCaughtFish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_includeSaltwater.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_includeWorldClass.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_displayUncatchableFish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_fishPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings<Point>);
			_dragFishPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings<bool>);
			_showRarityBorder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			_fishImgSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings<int>);
			_fishPanelOrientation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			_fishPanelDirection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			_fishPanelTooltipDisplay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateSettings<string>);
			ClickThroughPanel fishPanel = _fishPanel;
			if (fishPanel != null)
			{
				((Control)fishPanel).Dispose();
			}
			_timeOfDayPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateClockLocation);
			_dragTimeOfDayClock.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_timeOfDayImgSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateClockSize);
			_settingClockAlign.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnUpdateClockLabelAlign);
			_settingClockLabel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateHideClockLabel);
			_hideTimeOfDay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			Clock timeOfDayClock = _timeOfDayClock;
			if (timeOfDayClock != null)
			{
				((Control)timeOfDayClock).Dispose();
			}
			_hideInCombat.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings<bool>);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			_timeOfDayClock.TimeOfDayChanged -= OnTimeOfDayChanged;
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			Texture2D imgBorderBlack = _imgBorderBlack;
			if (imgBorderBlack != null)
			{
				((GraphicsResource)imgBorderBlack).Dispose();
			}
			Texture2D imgBorderJunk = _imgBorderJunk;
			if (imgBorderJunk != null)
			{
				((GraphicsResource)imgBorderJunk).Dispose();
			}
			Texture2D imgBorderBasic = _imgBorderBasic;
			if (imgBorderBasic != null)
			{
				((GraphicsResource)imgBorderBasic).Dispose();
			}
			Texture2D imgBorderFine = _imgBorderFine;
			if (imgBorderFine != null)
			{
				((GraphicsResource)imgBorderFine).Dispose();
			}
			Texture2D imgBorderMasterwork = _imgBorderMasterwork;
			if (imgBorderMasterwork != null)
			{
				((GraphicsResource)imgBorderMasterwork).Dispose();
			}
			Texture2D imgBorderRare = _imgBorderRare;
			if (imgBorderRare != null)
			{
				((GraphicsResource)imgBorderRare).Dispose();
			}
			Texture2D imgBorderExotic = _imgBorderExotic;
			if (imgBorderExotic != null)
			{
				((GraphicsResource)imgBorderExotic).Dispose();
			}
			Texture2D imgBorderAscended = _imgBorderAscended;
			if (imgBorderAscended != null)
			{
				((GraphicsResource)imgBorderAscended).Dispose();
			}
			Texture2D imgBorderLegendary = _imgBorderLegendary;
			if (imgBorderLegendary != null)
			{
				((GraphicsResource)imgBorderLegendary).Dispose();
			}
			Texture2D imgBorderX = _imgBorderX;
			if (imgBorderX != null)
			{
				((GraphicsResource)imgBorderX).Dispose();
			}
			Texture2D imgBorderXY = _imgBorderXY;
			if (imgBorderXY != null)
			{
				((GraphicsResource)imgBorderXY).Dispose();
			}
			FishingBuddyModule moduleInstance = ModuleInstance;
			if (moduleInstance != null)
			{
				((Module)moduleInstance).Dispose();
			}
			ModuleInstance = null;
		}

		protected virtual void OnUpdateSettings<T>(object sender = null, ValueChangedEventArgs<T> e = null)
		{
			Logger.Debug("Settings updated");
			GetCurrentMapTime();
			DrawIcons();
		}

		protected virtual async void OnUpdateFishSettings<T>(object sender = null, ValueChangedEventArgs<T> e = null)
		{
			Logger.Debug("Fish settings updated");
			GetCurrentMapTime();
			await GetCurrentMapsFish();
			DrawIcons();
		}

		private void OnUpdateClockSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			if (_hideTimeOfDay.get_Value())
			{
				_timeOfDayClock.Drag = false;
				((Control)_timeOfDayClock).Hide();
			}
			else
			{
				((Control)_timeOfDayClock).Show();
				_timeOfDayClock.Drag = _dragTimeOfDayClock.get_Value();
			}
		}

		private void OnUpdateClockLabelAlign(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			_timeOfDayClock.LabelVerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), _settingClockAlign.get_Value());
		}

		private void OnUpdateHideClockLabel(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_timeOfDayClock.HideLabel = _settingClockLabel.get_Value();
		}

		private void OnUpdateClockLocation(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			if (_timeOfDayPanelLoc.get_Value().X < 0)
			{
				_timeOfDayPanelLoc.set_Value(new Point(0, _timeOfDayPanelLoc.get_Value().Y));
			}
			if (_timeOfDayPanelLoc.get_Value().Y < 0)
			{
				_timeOfDayPanelLoc.set_Value(new Point(_timeOfDayPanelLoc.get_Value().X, 0));
			}
			((Control)_timeOfDayClock).set_Location(_timeOfDayPanelLoc.get_Value());
		}

		private void OnUpdateClockSize(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			((Control)_timeOfDayClock).set_Size(new Point(_timeOfDayImgSize.get_Value()));
		}

		protected void DrawIcons()
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			ClickThroughPanel fishPanel = _fishPanel;
			if (fishPanel != null)
			{
				((Control)fishPanel).Dispose();
			}
			int fishPanelRows = Clamp((int)Math.Ceiling((double)catchableFish.Count() / 2.0), 1, 7);
			int fishPanelColumns = Clamp((int)Math.Ceiling((double)catchableFish.Count() / (double)fishPanelRows), 1, 7);
			if (fishPanelRows < fishPanelColumns)
			{
				int num = fishPanelRows;
				fishPanelRows = fishPanelColumns;
				fishPanelColumns = num;
			}
			if (object.Equals(_fishPanelOrientation.get_Value(), Strings.Horizontal))
			{
				int num2 = fishPanelRows;
				fishPanelRows = fishPanelColumns;
				fishPanelColumns = num2;
			}
			ClickThroughPanel clickThroughPanel = new ClickThroughPanel();
			((Control)clickThroughPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)clickThroughPanel).set_Location(_fishPanelLoc.get_Value());
			((Control)clickThroughPanel).set_Size(new Point(_fishImgSize.get_Value() * 7));
			clickThroughPanel.Capture = _dragFishPanel.get_Value();
			_fishPanel = clickThroughPanel;
			Logger.Debug($"Fish Panel Size; Rows: {fishPanelRows} Columns: {fishPanelColumns}, {((Control)_fishPanel).get_Size()}");
			int x = 0;
			int y = 0;
			int count = 1;
			int xStart = x;
			if (object.Equals(_fishPanelDirection.get_Value(), Strings.Top_right))
			{
				x = ((Control)_fishPanel).get_Size().X - _fishImgSize.get_Value();
				xStart = x;
			}
			else if (object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_left))
			{
				y = ((Control)_fishPanel).get_Size().Y - _fishImgSize.get_Value();
			}
			else if (object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_right))
			{
				x = ((Control)_fishPanel).get_Size().X - _fishImgSize.get_Value();
				xStart = x;
				y = ((Control)_fishPanel).get_Size().Y - _fishImgSize.get_Value();
			}
			foreach (Fish fish in from f in catchableFish
				orderby f.Visible descending, f.Caught && f.Visible, f.Rarity, f.Name
				select f)
			{
				string fishTooltip = BuildTooltip(fish);
				ClickThroughImage clickThroughImage = new ClickThroughImage();
				((Control)clickThroughImage).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage).set_Texture(fish.IconImg);
				((Control)clickThroughImage).set_Size(new Point(_fishImgSize.get_Value()));
				((Control)clickThroughImage).set_Location(new Point(x, y));
				((Control)clickThroughImage).set_ZIndex(0);
				clickThroughImage.Capture = _dragFishPanel.get_Value();
				((Control)clickThroughImage).set_Opacity((fish.Visible && !fish.Caught) ? 1f : 0.5f);
				if (!_ignoreCaughtFish.get_Value() && fish.Caught)
				{
					ClickThroughImage clickThroughImage2 = new ClickThroughImage();
					((Control)clickThroughImage2).set_Parent((Container)(object)_fishPanel);
					((Image)clickThroughImage2).set_Texture(AsyncTexture2D.op_Implicit(_imgBorderXY));
					((Control)clickThroughImage2).set_Size(new Point(_fishImgSize.get_Value()));
					((Control)clickThroughImage2).set_Location(new Point(x, y));
					((Control)clickThroughImage2).set_ZIndex(1);
					clickThroughImage2.Capture = _dragFishPanel.get_Value();
					((Control)clickThroughImage2).set_Opacity(0.75f);
				}
				if (_displayUncatchableFish.get_Value() && !fish.Visible)
				{
					ClickThroughImage clickThroughImage3 = new ClickThroughImage();
					((Control)clickThroughImage3).set_Parent((Container)(object)_fishPanel);
					((Image)clickThroughImage3).set_Texture(AsyncTexture2D.op_Implicit(_imgBorderX));
					((Control)clickThroughImage3).set_Size(new Point(_fishImgSize.get_Value()));
					((Control)clickThroughImage3).set_Location(new Point(x, y));
					((Control)clickThroughImage3).set_ZIndex(2);
					clickThroughImage3.Capture = _dragFishPanel.get_Value();
					((Control)clickThroughImage3).set_Opacity(1f);
				}
				ClickThroughImage clickThroughImage4 = new ClickThroughImage();
				((Control)clickThroughImage4).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage4).set_Texture(AsyncTexture2D.op_Implicit(_showRarityBorder.get_Value() ? GetImageBorder(fish.Rarity) : _imgBorderBlack));
				((Control)clickThroughImage4).set_Size(new Point(_fishImgSize.get_Value()));
				((Control)clickThroughImage4).set_Opacity(0.8f);
				((Control)clickThroughImage4).set_Location(new Point(x, y));
				((Control)clickThroughImage4).set_BasicTooltipText(fishTooltip);
				((Control)clickThroughImage4).set_ZIndex(3);
				clickThroughImage4.Capture = _dragFishPanel.get_Value();
				if (object.Equals(_fishPanelDirection.get_Value(), Strings.Top_left) || object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_left))
				{
					x += _fishImgSize.get_Value();
				}
				if (object.Equals(_fishPanelDirection.get_Value(), Strings.Top_right) || object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_right))
				{
					x -= _fishImgSize.get_Value();
				}
				if (count == fishPanelColumns)
				{
					x = xStart;
					if (object.Equals(_fishPanelDirection.get_Value(), Strings.Top_left) || object.Equals(_fishPanelDirection.get_Value(), Strings.Top_right))
					{
						y += _fishImgSize.get_Value();
					}
					if (object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_left) || object.Equals(_fishPanelDirection.get_Value(), Strings.Bottom_right))
					{
						y -= _fishImgSize.get_Value();
					}
					count = 0;
				}
				count++;
			}
			if (_dragFishPanel.get_Value())
			{
				_fishPanel.Capture = true;
				((Control)_fishPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					_draggingFishPanel = true;
					_dragFishPanelStart = GameService.Input.get_Mouse().get_Position();
					((Panel)_fishPanel).set_ShowTint(true);
				});
				((Control)_fishPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					_draggingFishPanel = false;
					_fishPanelLoc.set_Value(((Control)_fishPanel).get_Location());
					((Panel)_fishPanel).set_ShowTint(false);
				});
			}
		}

		private string BuildTooltip(Fish fish)
		{
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			string name = Strings.FishName + ": " + fish.Name;
			string bait = Strings.FishFavoredBait + ": " + fish.Bait.GetEnumMemberValue();
			string time = Strings.FishTimeOfDay + ": " + fish.Time.GetEnumMemberValue();
			string hole = Strings.FishFishingHole + ": " + fish.Hole.GetEnumMemberValue() + (fish.OpenWater ? (", " + Strings.OpenWater) : string.Empty);
			string achieve = Strings.Achievement + ": " + fish.Achievement;
			string rarity = $"{Strings.Rarity}: {fish.Rarity}";
			string hiddenReason = string.Empty;
			if (_useAPIToken)
			{
				if (!fish.Visible && fish.Caught)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.TimeOfDay + ", " + Strings.HiddenCaught;
				}
				else if (!fish.Visible)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.TimeOfDay;
				}
				else if (fish.Caught)
				{
					hiddenReason = Strings.Hidden + ": " + Strings.HiddenCaught;
				}
			}
			string notes = ((!string.IsNullOrWhiteSpace(fish.Notes)) ? (Strings.Notes + ": " + fish.Notes) : string.Empty);
			string text = _fishPanelTooltipDisplay.get_Value().Replace("@1", name).Replace("@2", bait)
				.Replace("@3", time)
				.Replace("@4", hole)
				.Replace("@5", achieve)
				.Replace("@6", rarity)
				.Replace("@7", hiddenReason)
				.Replace("@8", notes)
				.Replace("#1", fish.Name)
				.Replace("#2", fish.Bait.GetEnumMemberValue())
				.Replace("#3", fish.Time.GetEnumMemberValue())
				.Replace("#4", fish.Hole.GetEnumMemberValue() + (fish.OpenWater ? (", " + Strings.OpenWater) : string.Empty))
				.Replace("#5", fish.Achievement);
			ItemRarity rarity2 = fish.Rarity;
			return text.Replace("#6", ((object)(ItemRarity)(ref rarity2)).ToString()).Replace("#8", fish.Notes).Replace("\\n", "\n")
				.Replace("\n\n", "\n")
				.Trim();
		}

		private Texture2D GetImageBorder(ItemRarity rarity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
			return (Texture2D)((rarity - 1) switch
			{
				0 => _imgBorderJunk, 
				1 => _imgBorderBasic, 
				2 => _imgBorderFine, 
				3 => _imgBorderMasterwork, 
				4 => _imgBorderRare, 
				5 => _imgBorderExotic, 
				6 => _imgBorderAscended, 
				7 => _imgBorderLegendary, 
				_ => _imgBorderBlack, 
			});
		}

		public static int Clamp(int n, int min, int max)
		{
			if (n < min)
			{
				return min;
			}
			if (n > max)
			{
				return max;
			}
			return n;
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			Logger.Debug("Map Changed");
			_currentMap = await _mapRepository.GetItem(e.get_Value());
			if (_currentMap != null && _currentMap.get_Id() != _prevMapId)
			{
				Logger.Debug($"Current map {_currentMap.get_Name()} {_currentMap.get_Id()}");
				_prevMapId = _currentMap.get_Id();
				GetCurrentMapTime();
				await GetCurrentMapsFish();
				DrawIcons();
			}
		}

		private async void OnTimeOfDayChanged(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			await GetCurrentMapsFish();
			DrawIcons();
			_lastUpdateFish = 0.0;
		}

		private void GetCurrentMapTime()
		{
			if (MumbleIsAvailable && _currentMap != null)
			{
				_timeOfDayClock.TimePhase = TyriaTime.CurrentMapPhase(_currentMap);
			}
			else
			{
				((Control)_timeOfDayClock).Hide();
			}
		}

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Gw2ApiManager.get_Permissions()))
			{
				Logger.Debug("API permissions are missing");
				_useAPIToken = false;
				return;
			}
			try
			{
				await GetCurrentMapsFish();
				DrawIcons();
				_useAPIToken = true;
			}
			catch (Exception)
			{
				Logger.Debug("Failed to get info from api.");
			}
		}

		private async Task GetCurrentMapsFish(GameTime gameTime)
		{
			await GetCurrentMapsFish();
			DrawIcons();
		}

		private async Task GetCurrentMapsFish(CancellationToken cancellationToken = default(CancellationToken))
		{
			await _updateFishSemaphore.WaitAsync(cancellationToken);
			try
			{
				try
				{
					if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Gw2ApiManager.get_Permissions()))
					{
						accountFishingAchievements = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
							.get_Achievements()).GetAsync(default(CancellationToken)))).Where((AccountAchievement achievement) => FishingMaps.FISHER_ACHIEVEMENT_IDS.Contains(achievement.get_Id()) && achievement.get_Current() != achievement.get_Max());
						_useAPIToken = true;
						IEnumerable<int> currentAchievementIds = accountFishingAchievements.Select((AccountAchievement achievement) => achievement.get_Id());
						IEnumerable<int> first = accountFishingAchievements.Select((AccountAchievement achievement) => achievement.get_Current());
						IEnumerable<int> progressMax = accountFishingAchievements.Select((AccountAchievement achievement) => achievement.get_Max());
						IEnumerable<string> currentOfMax = first.Zip(progressMax, (int current, int max) => current + "/" + max);
						Logger.Debug("All account fishing achievement Ids: " + string.Join(", ", currentAchievementIds));
						Logger.Debug("Account fishing achievement progress: " + string.Join(", ", currentOfMax));
					}
					else
					{
						Logger.Debug("API permissions are missing");
						_useAPIToken = false;
					}
				}
				catch (Exception ex2)
				{
					Logger.Debug(ex2, "Failed to query Guild Wars 2 API.");
					_useAPIToken = false;
				}
				catchableFish.Clear();
				List<int> achievementsInMap = new List<int>();
				List<int> verifyMapAchievable = new List<int>();
				if (_currentMap == null)
				{
					try
					{
						_currentMap = await _mapRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
					}
					catch (Exception ex3)
					{
						Logger.Debug(ex3, "Couldn't get player's current map.");
					}
				}
				if (_currentMap != null && _fishingMaps.MapAchievements.ContainsKey(_currentMap.get_Id()))
				{
					achievementsInMap.AddRange(_fishingMaps.MapAchievements[_currentMap.get_Id()]);
					verifyMapAchievable.AddRange(_fishingMaps.MapAchievements[_currentMap.get_Id()]);
				}
				else
				{
					Logger.Debug("Couldn't get player's current map, skipping current map fish.");
				}
				if (_includeSaltwater.get_Value())
				{
					achievementsInMap.AddRange(FishingMaps.SaltwaterFisher);
				}
				if (_includeWorldClass.get_Value())
				{
					achievementsInMap.AddRange(FishingMaps.WorldClassFisher);
				}
				if (achievementsInMap.Count == 0)
				{
					Logger.Debug("No achievable fish in map.");
					return;
				}
				Logger.Debug("All map achievements: " + string.Join(", ", achievementsInMap));
				if (_useAPIToken)
				{
					Logger.Debug("Using API");
					List<AccountAchievement> currentMapAchievable = accountFishingAchievements.Where((AccountAchievement achievement) => achievementsInMap.Contains(achievement.get_Id())).ToList();
					if (!currentMapAchievable.Any((AccountAchievement a) => verifyMapAchievable.Contains(a.get_Id())))
					{
						AccountAchievement val = new AccountAchievement();
						val.set_Id(_fishingMaps.MapAchievements[_currentMap.get_Id()].First());
						val.set_Current(0);
						val.set_Done(false);
						currentMapAchievable.Add(val);
					}
					if (_includeSaltwater.get_Value() && !currentMapAchievable.Any((AccountAchievement a) => FishingMaps.SaltwaterFisher.Contains(a.get_Id())))
					{
						AccountAchievement val2 = new AccountAchievement();
						val2.set_Id(FishingMaps.SaltwaterFisher.First());
						val2.set_Current(0);
						val2.set_Done(false);
						currentMapAchievable.Add(val2);
					}
					if (_includeWorldClass.get_Value() && !currentMapAchievable.Any((AccountAchievement a) => FishingMaps.WorldClassFisher.Contains(a.get_Id())))
					{
						AccountAchievement val3 = new AccountAchievement();
						val3.set_Id(FishingMaps.WorldClassFisher.First());
						val3.set_Current(0);
						val3.set_Done(false);
						currentMapAchievable.Add(val3);
					}
					Logger.Debug("Current map achievable: " + string.Join(", ", currentMapAchievable.Select((AccountAchievement achievement) => $"id: {achievement.get_Id()} current: {achievement.get_Current()} done: {achievement.get_Done()}")));
					int bitsCounter2 = 0;
					foreach (AccountAchievement accountAchievement in currentMapAchievable)
					{
						Achievement currentAccountAchievement = await RequestAchievement(accountAchievement.get_Id());
						if (currentAccountAchievement == null)
						{
							Logger.Debug($"Requested achievement by id is null, account achievement id: {accountAchievement.get_Id()}");
							continue;
						}
						if (currentAccountAchievement.get_Bits() == null)
						{
							Logger.Warn($"Requested achievement bits are null, account achievement id: {accountAchievement.get_Id()}");
							continue;
						}
						foreach (AchievementBit bit2 in currentAccountAchievement.get_Bits())
						{
							if (bit2 == null)
							{
								Logger.Debug($"Bit in {currentAccountAchievement.get_Id()} is null");
								continue;
							}
							if (_ignoreCaughtFish.get_Value() && accountAchievement.get_Bits() != null && accountAchievement.get_Bits().Contains(bitsCounter2))
							{
								bitsCounter2++;
								continue;
							}
							AddCatchableFish(((AchievementItemBit)bit2).get_Id(), currentAccountAchievement, accountAchievement.get_Bits() != null && accountAchievement.get_Bits().Contains(bitsCounter2));
							bitsCounter2++;
						}
						bitsCounter2 = 0;
					}
				}
				else
				{
					Logger.Debug("Not using API");
					IEnumerable<int> currentMapAchievableIds = FishingMaps.BASE_FISHER_ACHIEVEMENT_IDS.Where((int achievementId) => achievementsInMap.Contains(achievementId));
					Logger.Debug("Current map achievable: " + string.Join(", ", currentMapAchievableIds));
					foreach (int bitsCounter2 in currentMapAchievableIds)
					{
						Achievement currentAchievement = await RequestAchievement(bitsCounter2);
						if (currentAchievement == null)
						{
							Logger.Debug($"Requested achievement by id is null, achievement id: {bitsCounter2}");
							continue;
						}
						foreach (AchievementBit bit in currentAchievement.get_Bits())
						{
							if (bit == null)
							{
								Logger.Debug($"Bit in {currentAchievement.get_Id()} is null");
							}
							else
							{
								AddCatchableFish(((AchievementItemBit)bit).get_Id(), currentAchievement, caught: false);
							}
						}
					}
				}
				if (!_displayUncatchableFish.get_Value())
				{
					catchableFish = catchableFish.Where((Fish phish) => phish.Visible).ToList();
				}
				Logger.Debug("Shown fish in current map count: " + catchableFish.Count());
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Unknown exception getting current map ({_currentMap.get_Name()} {_currentMap.get_Id()}) fish");
			}
			finally
			{
				_updateFishSemaphore.Release();
			}
		}

		private async void AddCatchableFish(int fishItemId, Achievement achievement, bool caught)
		{
			Item fish = await RequestItem(fishItemId);
			if (fish == null)
			{
				Logger.Warn($"Skipping fish due to API issue. id: '{fishItemId}'");
				return;
			}
			Logger.Debug($"Found Fish '{fish.get_Name()}' id: '{fish.get_Id()}'");
			IEnumerable<Fish> fishIdMatch = _allFishList.Where((Fish phish) => phish.ItemId == fish.get_Id());
			Fish ghoti = ((fishIdMatch.Count() != 0) ? fishIdMatch.First() : null);
			if (ghoti == null)
			{
				Logger.Warn($"Missing fish from all fish list: name: '{fish.get_Name()}' id: '{fish.get_Id()}'");
				return;
			}
			ghoti.Caught = caught;
			ghoti.Visible = ghoti.Time == Fish.TimeOfDay.Any || _timeOfDayClock.TimePhase.Equals(Strings.Dawn) || _timeOfDayClock.TimePhase.Equals(Strings.Dusk) || object.Equals(ghoti.Time.ToString(), _timeOfDayClock.TimePhase);
			ghoti.Name = fish.get_Name();
			ghoti.Icon = fish.get_Icon();
			ghoti.ItemId = fish.get_Id();
			ghoti.Achievement = achievement.get_Name();
			ghoti.AchievementId = achievement.get_Id();
			ghoti.Rarity = ApiEnum<ItemRarity>.op_Implicit(fish.get_Rarity());
			ghoti.ChatLink = fish.get_ChatLink();
			ghoti.IconImg = RequestItemIcon(fish);
			if (ghoti.Locations == null || ghoti.Locations.Contains(_currentMap.get_Id()))
			{
				catchableFish.Add(ghoti);
			}
			else
			{
				Logger.Debug($"Skipping {fish.get_Name()} {fish.get_Id()}, not available in current map.");
			}
		}

		private async Task<Map> RequestMap(int id)
		{
			Logger.Debug($"Requested map id: {id}");
			try
			{
				Task<Map> mapTask = ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(id, default(CancellationToken));
				await mapTask;
				return mapTask.Result;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to query Guild Wars 2 API.");
				return null;
			}
		}

		private async Task<Achievement> RequestAchievement(int id)
		{
			Logger.Debug($"Requested achievement id: {id}");
			try
			{
				Task<Achievement> achievementTask = ((IBulkExpandableClient<Achievement, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()).GetAsync(id, default(CancellationToken));
				await achievementTask;
				return achievementTask.Result;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to query Guild Wars 2 API.");
				return null;
			}
		}

		private async Task<Item> RequestItem(int id)
		{
			Logger.Debug($"Requested item id: {id}");
			try
			{
				Task<Item> itemTask = ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).GetAsync(id, default(CancellationToken));
				await itemTask;
				return itemTask.Result;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to query Guild Wars 2 API.");
				return null;
			}
		}

		private AsyncTexture2D RequestItemIcon(Item item)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(item.get_Icon()));
		}
	}
}
