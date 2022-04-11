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

		public static SettingEntry<bool> _dragTimeOfDayClock;

		public static SettingEntry<int> _timeOfDayImgSize;

		public static SettingEntry<Point> _timeOfDayPanelLoc;

		public static SettingEntry<bool> _ignoreCaughtFish;

		public static SettingEntry<bool> _includeWorldClass;

		public static SettingEntry<bool> _includeSaltwater;

		public static SettingEntry<bool> _displayUncatchableFish;

		public static SettingEntry<bool> _hideInCombat;

		public static SettingEntry<bool> _hideTimeOfDay;

		private List<Fish> catchableFish;

		private FishingMaps fishingMaps;

		private IEnumerable<AccountAchievement> accountFishingAchievements;

		public static SettingEntry<bool> _showRarityBorder;

		private Clock _timeOfDayClock;

		private List<Fish> _allFishList;

		private Map _currentMap;

		private bool _useAPIToken;

		private readonly SemaphoreSlim _updateFishSemaphore = new SemaphoreSlim(1, 1);

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

		private bool uiIsAvailable
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

		private bool hidingInCombat
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
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			_ignoreCaughtFish = settings.DefineSetting<bool>("IgnoreCaughtFish", true, (Func<string>)(() => "Ignore Caught"), (Func<string>)(() => "Ignore fish already counted towards achievements"));
			_includeSaltwater = settings.DefineSetting<bool>("IncludeSaltwater", false, (Func<string>)(() => "Display Saltwater"), (Func<string>)(() => "Include Saltwater Fisher fish"));
			_includeWorldClass = settings.DefineSetting<bool>("IncludeWorldClass", false, (Func<string>)(() => "Display World Class"), (Func<string>)(() => "Include World Class Fisher fish"));
			_displayUncatchableFish = settings.DefineSetting<bool>("DisplayUncatchable", false, (Func<string>)(() => "Display Uncatchable"), (Func<string>)(() => "Display fish that cannot be caught at this time of day"));
			_fishPanelLoc = settings.DefineSetting<Point>("FishPanelLoc", new Point(160, 100), (Func<string>)(() => "Fish Panel Location"), (Func<string>)(() => ""));
			_dragFishPanel = settings.DefineSetting<bool>("FishPanelDrag", false, (Func<string>)(() => "Drag Fish"), (Func<string>)(() => ""));
			_fishImgSize = settings.DefineSetting<int>("FishImgWidth", 30, (Func<string>)(() => "Fish Size"), (Func<string>)(() => ""));
			_showRarityBorder = settings.DefineSetting<bool>("ShowRarityBorder", true, (Func<string>)(() => "Show Rarity"), (Func<string>)(() => "Display fish rarity border"));
			_ignoreCaughtFish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_includeSaltwater.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_includeWorldClass.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_displayUncatchableFish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_fishPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragFishPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_showRarityBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_fishImgSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			SettingComplianceExtensions.SetRange(_fishImgSize, 16, 96);
			_timeOfDayPanelLoc = settings.DefineSetting<Point>("TimeOfDayPanelLoc", new Point(100, 100), (Func<string>)(() => "Time of Day Details Location"), (Func<string>)(() => ""));
			_dragTimeOfDayClock = settings.DefineSetting<bool>("TimeOfDayPanelDrag", false, (Func<string>)(() => "Drag Time Display"), (Func<string>)(() => "Drag time of day display"));
			_timeOfDayImgSize = settings.DefineSetting<int>("TimeImgWidth", 64, (Func<string>)(() => "Time of Day Size"), (Func<string>)(() => ""));
			_hideTimeOfDay = settings.DefineSetting<bool>("HideTimeOfDay", false, (Func<string>)(() => "Hide Time Display"), (Func<string>)(() => "Opption to hide time display"));
			_timeOfDayPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateClockLocation);
			_dragTimeOfDayClock.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			SettingComplianceExtensions.SetRange(_timeOfDayImgSize, 16, 96);
			_timeOfDayImgSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateClockSize);
			_dragTimeOfDayClock.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_hideTimeOfDay.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_hideInCombat = settings.DefineSetting<bool>("HideInCombat", false, (Func<string>)(() => "Hide In Combat"), (Func<string>)(() => "Hide all fishing info in combat"));
			_hideInCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			catchableFish = new List<Fish>();
			fishingMaps = new FishingMaps();
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
			_imgDawn = ContentsManager.GetTexture("dawn.png");
			_imgDay = ContentsManager.GetTexture("day.png");
			_imgDusk = ContentsManager.GetTexture("dusk.png");
			_imgNight = ContentsManager.GetTexture("night.png");
			_allFishList = new List<Fish>();
			using (StreamReader r = new StreamReader(ContentsManager.GetFileStream("fish.json")))
			{
				string json = r.ReadToEnd();
				_allFishList.AddRange(JsonConvert.DeserializeObject<List<Fish>>(json));
				Logger.Debug("fish list: " + string.Join(", ", _allFishList.Select((Fish fish) => fish.name)));
			}
			_useAPIToken = true;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			_timeOfDayClock = new Clock();
			((Control)_timeOfDayClock).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			OnUpdateClockSettings();
			OnUpdateClockLocation();
			OnUpdateClockSize();
			GetCurrentMapTime();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			DrawIcons();
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if (uiIsAvailable && !hidingInCombat)
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
			_ignoreCaughtFish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_includeSaltwater.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_includeWorldClass.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_displayUncatchableFish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_fishPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragFishPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_showRarityBorder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			_fishImgSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			ClickThroughPanel fishPanel = _fishPanel;
			if (fishPanel != null)
			{
				((Control)fishPanel).Dispose();
			}
			_timeOfDayPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateClockLocation);
			_dragTimeOfDayClock.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			_timeOfDayImgSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateClockSize);
			_hideTimeOfDay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateClockSettings);
			Clock timeOfDayClock = _timeOfDayClock;
			if (timeOfDayClock != null)
			{
				((Control)timeOfDayClock).Dispose();
			}
			_hideInCombat.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFishSettings);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
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
			FishingBuddyModule moduleInstance = ModuleInstance;
			if (moduleInstance != null)
			{
				((Module)moduleInstance).Dispose();
			}
			ModuleInstance = null;
		}

		protected virtual void OnUpdateSettings(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			Logger.Debug("Settings updated");
			GetCurrentMapTime();
			DrawIcons();
		}

		protected virtual void OnUpdateSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			Logger.Debug("Settings updated");
			GetCurrentMapTime();
			DrawIcons();
		}

		protected virtual void OnUpdateSettings(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			Logger.Debug("Settings updated");
			GetCurrentMapTime();
			DrawIcons();
		}

		protected virtual async void OnUpdateFishSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			Logger.Debug("Fish settings updated");
			GetCurrentMapTime();
			await getCurrentMapsFish();
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
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
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
			ClickThroughPanel clickThroughPanel = new ClickThroughPanel();
			((Control)clickThroughPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)clickThroughPanel).set_Location(_fishPanelLoc.get_Value());
			((Control)clickThroughPanel).set_Size(new Point(fishPanelColumns * _fishImgSize.get_Value(), fishPanelRows * _fishImgSize.get_Value()));
			clickThroughPanel.capture = _dragFishPanel.get_Value();
			_fishPanel = clickThroughPanel;
			Logger.Debug($"Fish Panel Size; Rows: {fishPanelRows} Columns: {fishPanelColumns}, {((Control)_fishPanel).get_Size()}");
			int x = 0;
			int y = 0;
			int count = 1;
			foreach (Fish fish in catchableFish)
			{
				string openWater = (fish.openWater ? ", Open Water" : "");
				ClickThroughImage clickThroughImage = new ClickThroughImage();
				((Control)clickThroughImage).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage).set_Texture(fish.iconImg);
				((Control)clickThroughImage).set_Size(new Point(_fishImgSize.get_Value()));
				((Control)clickThroughImage).set_Location(new Point(x, y));
				((Control)clickThroughImage).set_ZIndex(0);
				clickThroughImage.Capture = _dragFishPanel.get_Value();
				((Control)clickThroughImage).set_Opacity(fish.Visible ? 1f : 0.5f);
				if (_displayUncatchableFish.get_Value() && !fish.Visible)
				{
					ClickThroughImage clickThroughImage2 = new ClickThroughImage();
					((Control)clickThroughImage2).set_Parent((Container)(object)_fishPanel);
					((Image)clickThroughImage2).set_Texture(AsyncTexture2D.op_Implicit(_imgBorderX));
					((Control)clickThroughImage2).set_Size(new Point(_fishImgSize.get_Value()));
					((Control)clickThroughImage2).set_Location(new Point(x, y));
					((Control)clickThroughImage2).set_ZIndex(1);
					clickThroughImage2.Capture = _dragFishPanel.get_Value();
				}
				ClickThroughImage clickThroughImage3 = new ClickThroughImage();
				((Control)clickThroughImage3).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage3).set_Texture(AsyncTexture2D.op_Implicit(_showRarityBorder.get_Value() ? GetImageBorder(fish.rarity) : _imgBorderBlack));
				((Control)clickThroughImage3).set_Size(new Point(_fishImgSize.get_Value()));
				((Control)clickThroughImage3).set_Opacity(0.8f);
				((Control)clickThroughImage3).set_Location(new Point(x, y));
				((Control)clickThroughImage3).set_BasicTooltipText(fish.name + "\nFishing Hole: " + fish.fishingHole + openWater + "\nFavored Bait: " + fish.bait + "\nTime of Day: " + ((fish.timeOfDay == Fish.TimeOfDay.DawnDusk) ? "Dusk/Dawn" : fish.timeOfDay.ToString()) + "\nAchievement: " + fish.achievement + "\nRarity: " + fish.rarity);
				((Control)clickThroughImage3).set_ZIndex(2);
				clickThroughImage3.Capture = _dragFishPanel.get_Value();
				x += _fishImgSize.get_Value();
				if (count == fishPanelColumns)
				{
					x = 0;
					y += _fishImgSize.get_Value();
					count = 0;
				}
				count++;
			}
			if (_dragFishPanel.get_Value())
			{
				_fishPanel.capture = true;
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

		private Texture2D GetImageBorder(string rarity)
		{
			return (Texture2D)(rarity switch
			{
				"Junk" => _imgBorderJunk, 
				"Basic" => _imgBorderBasic, 
				"Fine" => _imgBorderFine, 
				"Masterwork" => _imgBorderMasterwork, 
				"Rare" => _imgBorderRare, 
				"Exotic" => _imgBorderExotic, 
				"Ascended" => _imgBorderAscended, 
				"Legendary" => _imgBorderLegendary, 
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
				await getCurrentMapsFish();
				DrawIcons();
			}
		}

		private async void TimeOfDayChanged()
		{
			await getCurrentMapsFish();
			DrawIcons();
		}

		private void GetCurrentMapTime()
		{
			if (MumbleIsAvailable)
			{
				_timeOfDayClock.TimePhase = TyriaTime.CurrentMapPhase(GameService.Gw2Mumble.get_CurrentMap().get_Id());
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
				await getCurrentMapsFish();
				DrawIcons();
				_useAPIToken = true;
			}
			catch (Exception)
			{
				Logger.Debug("Failed to get info from api.");
			}
		}

		private async Task getCurrentMapsFish(CancellationToken cancellationToken = default(CancellationToken))
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
				if (_currentMap != null && fishingMaps.mapAchievements.ContainsKey(_currentMap.get_Id()))
				{
					achievementsInMap.AddRange(fishingMaps.mapAchievements[_currentMap.get_Id()]);
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
					Logger.Debug("No achieveable fish in map.");
					return;
				}
				Logger.Debug("All map achievements: " + string.Join(", ", achievementsInMap));
				if (_ignoreCaughtFish.get_Value() && _useAPIToken)
				{
					IEnumerable<AccountAchievement> currentMapAchievable = accountFishingAchievements.Where((AccountAchievement achievement) => achievementsInMap.Contains(achievement.get_Id()));
					Logger.Debug("Current map achieveable: " + string.Join(", ", currentMapAchievable.Select((AccountAchievement achievement) => $"id: {achievement.get_Id()} current: {achievement.get_Current()} done: {achievement.get_Done()}")));
					int bitsCounter = 0;
					foreach (AccountAchievement accountAchievement in currentMapAchievable)
					{
						Achievement currentAccountAchievement2 = await RequestAchievement(accountAchievement.get_Id());
						if (currentAccountAchievement2 == null || currentAccountAchievement2.get_Bits() == null)
						{
							continue;
						}
						foreach (AchievementBit bit2 in currentAccountAchievement2.get_Bits())
						{
							if (bit2 == null)
							{
								Logger.Debug($"Bit in {currentAccountAchievement2.get_Id()} is null");
								continue;
							}
							if (accountAchievement.get_Bits() != null && accountAchievement.get_Bits().Contains(bitsCounter))
							{
								bitsCounter++;
								continue;
							}
							int itemId2 = ((AchievementItemBit)bit2).get_Id();
							Item fish2 = await RequestItem(itemId2);
							IEnumerable<Fish> fishNameMatch2 = _allFishList.Where((Fish phish) => phish.name == fish2.get_Name());
							Fish ghoti2 = ((fishNameMatch2.Count() != 0) ? fishNameMatch2.First() : null);
							if (ghoti2 == null)
							{
								Logger.Debug("Missing fish from all fish list: " + fish2.get_Name());
								continue;
							}
							if (ghoti2.timeOfDay != Fish.TimeOfDay.Any && !_timeOfDayClock.TimePhase.Equals("Dawn") && !_timeOfDayClock.TimePhase.Equals("Dusk") && !object.Equals(ghoti2.timeOfDay.ToString(), _timeOfDayClock.TimePhase))
							{
								ghoti2.Visible = false;
							}
							else
							{
								ghoti2.Visible = true;
							}
							ghoti2.icon = fish2.get_Icon();
							ghoti2.itemId = fish2.get_Id();
							ghoti2.achievementId = currentAccountAchievement2.get_Id();
							ghoti2.iconImg = RequestItemIcon(fish2);
							catchableFish.Add(ghoti2);
							bitsCounter++;
						}
						bitsCounter = 0;
					}
				}
				else
				{
					IEnumerable<int> currentMapAchievableIds = FishingMaps.BASE_FISHER_ACHIEVEMENT_IDS.Where((int achievementId) => achievementsInMap.Contains(achievementId));
					Logger.Debug("Current map achieveable: " + string.Join(", ", currentMapAchievableIds));
					foreach (int achievementId2 in currentMapAchievableIds)
					{
						Achievement currentAccountAchievement2 = await RequestAchievement(achievementId2);
						if (currentAccountAchievement2 == null)
						{
							continue;
						}
						foreach (AchievementBit bit in currentAccountAchievement2.get_Bits())
						{
							if (bit == null)
							{
								Logger.Debug($"Bit in {currentAccountAchievement2.get_Id()} is null");
								continue;
							}
							int itemId = ((AchievementItemBit)bit).get_Id();
							Item fish = await RequestItem(itemId);
							Logger.Debug($"Found Fish {fish.get_Name()} {fish.get_Id()}");
							IEnumerable<Fish> fishNameMatch = _allFishList.Where((Fish phish) => phish.name == fish.get_Name());
							Fish ghoti = ((fishNameMatch.Count() != 0) ? fishNameMatch.First() : null);
							if (ghoti == null)
							{
								Logger.Warn("Missing fish from all fish list: " + fish.get_Name());
								continue;
							}
							if (ghoti.timeOfDay != Fish.TimeOfDay.Any && !_timeOfDayClock.TimePhase.Equals("Dawn") && !_timeOfDayClock.TimePhase.Equals("Dusk") && !object.Equals(ghoti.timeOfDay.ToString(), _timeOfDayClock.TimePhase))
							{
								ghoti.Visible = false;
							}
							else
							{
								ghoti.Visible = true;
							}
							ghoti.icon = fish.get_Icon();
							ghoti.itemId = fish.get_Id();
							ghoti.achievementId = currentAccountAchievement2.get_Id();
							ghoti.iconImg = RequestItemIcon(fish);
							catchableFish.Add(ghoti);
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
				Logger.Error(ex, "Unknown exception getting current map fish");
			}
			finally
			{
				_updateFishSemaphore.Release();
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
