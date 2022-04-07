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
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Eclipse1807.BlishHUD.FishingBuddy.Utils;
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

		private Texture2D _imgDawn;

		private Texture2D _imgDay;

		private Texture2D _imgDusk;

		private Texture2D _imgNight;

		private Texture2D _imgBorderBlack;

		private Texture2D _imgBorderJunk;

		private Texture2D _imgBorderBasic;

		private Texture2D _imgBorderFine;

		private Texture2D _imgBorderMasterwork;

		private Texture2D _imgBorderRare;

		private Texture2D _imgBorderExotic;

		private Texture2D _imgBorderAscended;

		private Texture2D _imgBorderLegendary;

		private ClickThroughImage _dawn;

		private ClickThroughImage _day;

		private ClickThroughImage _dusk;

		private ClickThroughImage _night;

		private AsyncCache<int, Map> _mapRepository;

		private ClickThroughPanel _fishPanel;

		private bool _draggingFishPanel;

		private Point _dragFishPanelStart = Point.get_Zero();

		private ClickThroughPanel _timeOfDayPanel;

		private bool _draggingTimeOfDayPanel;

		private Point _dragTimeOfDayPanelStart = Point.get_Zero();

		public static SettingEntry<bool> _dragFishPanel;

		public static SettingEntry<int> _fishImgWidth;

		private static SettingEntry<Point> _fishPanelLoc;

		public static SettingEntry<bool> _dragTimeOfDayPanel;

		public static SettingEntry<int> _timeOfDayImgWidth;

		private static SettingEntry<Point> _timeOfDayPanelLoc;

		public static SettingEntry<bool> _ignoreCaughtFish;

		public static SettingEntry<bool> _includeWorldClass;

		public static SettingEntry<bool> _includeSaltwater;

		private List<Fish> catchableFish;

		private FishingMaps fishingMaps;

		private IEnumerable<AccountAchievement> accountFishingAchievements;

		public static SettingEntry<bool> _showRarityBorder;

		private List<Fish> _allFishList;

		private Map _currentMap;

		private bool _useAPIToken;

		private readonly SemaphoreSlim _updateFishSemaphore = new SemaphoreSlim(1, 1);

		private double _runningTime;

		private int _prevMapId;

		private string _timeOfDay = "";

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

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		private string timeOfDay
		{
			get
			{
				return _timeOfDay;
			}
			set
			{
				if (!object.Equals(_timeOfDay, value))
				{
					Logger.Debug("Time of day changed " + timeOfDay + " -> " + value);
					_timeOfDay = value;
					TimeOfDayChanged();
				}
			}
		}

		[ImportingConstructor]
		public FishingBuddyModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)


		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			_ignoreCaughtFish = settings.DefineSetting<bool>("IgnoreCaughtFish", true, (Func<string>)(() => "Ignore Caught Fish"), (Func<string>)(() => "Ignore fish already counted towards achievements"));
			_ignoreCaughtFish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_includeSaltwater = settings.DefineSetting<bool>("IncludeSaltwater", false, (Func<string>)(() => "Include Saltwater Fish"), (Func<string>)(() => "Include Saltwater Fisher fish"));
			_includeSaltwater.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_includeWorldClass = settings.DefineSetting<bool>("IncludeWorldClass", false, (Func<string>)(() => "Include World Class Fish"), (Func<string>)(() => "Include World Class Fisher fish"));
			_includeWorldClass.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_fishPanelLoc = settings.DefineSetting<Point>("FishPanelLoc", new Point(160, 100), (Func<string>)(() => "Fish Location"), (Func<string>)(() => ""));
			_fishPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragFishPanel = settings.DefineSetting<bool>("FishPanelDrag", false, (Func<string>)(() => "Drag Fish"), (Func<string>)(() => ""));
			_dragFishPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_fishImgWidth = settings.DefineSetting<int>("FishImgWidth", 30, (Func<string>)(() => "Fish Size"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_fishImgWidth, 16, 96);
			_fishImgWidth.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			_timeOfDayPanelLoc = settings.DefineSetting<Point>("TimeOfDayPanelLoc", new Point(100, 100), (Func<string>)(() => "Time of Day Details Location"), (Func<string>)(() => ""));
			_timeOfDayPanelLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragTimeOfDayPanel = settings.DefineSetting<bool>("TimeOfDayPanelDrag", false, (Func<string>)(() => "Drag Time of Day Details"), (Func<string>)(() => ""));
			_dragTimeOfDayPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_timeOfDayImgWidth = settings.DefineSetting<int>("TimeImgWidth", 64, (Func<string>)(() => "Time of Day Size"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_timeOfDayImgWidth, 16, 96);
			_timeOfDayImgWidth.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			_showRarityBorder = settings.DefineSetting<bool>("ShowRarityBorder", true, (Func<string>)(() => "Show Fish Rarity"), (Func<string>)(() => ""));
			_showRarityBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			catchableFish = new List<Fish>();
			fishingMaps = new FishingMaps();
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_imgDawn = ContentsManager.GetTexture("dawn.png");
			_imgDay = ContentsManager.GetTexture("day.png");
			_imgDusk = ContentsManager.GetTexture("dusk.png");
			_imgNight = ContentsManager.GetTexture("night.png");
			_imgBorderBlack = ContentsManager.GetTexture("border_black.png");
			_imgBorderJunk = ContentsManager.GetTexture("border_junk.png");
			_imgBorderBasic = ContentsManager.GetTexture("border_basic.png");
			_imgBorderFine = ContentsManager.GetTexture("border_fine.png");
			_imgBorderMasterwork = ContentsManager.GetTexture("border_masterwork.png");
			_imgBorderRare = ContentsManager.GetTexture("border_rare.png");
			_imgBorderExotic = ContentsManager.GetTexture("border_exotic.png");
			_imgBorderAscended = ContentsManager.GetTexture("border_ascended.png");
			_imgBorderLegendary = ContentsManager.GetTexture("border_legendary.png");
			_dawn = new ClickThroughImage();
			_day = new ClickThroughImage();
			_dusk = new ClickThroughImage();
			_night = new ClickThroughImage();
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
			GetCurrentMapTime();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			DrawIcons();
		}

		protected override async void Update(GameTime gameTime)
		{
			_runningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_runningTime > 180000.0)
			{
				_runningTime -= 180000.0;
				await getCurrentMapsFish();
				DrawIcons();
			}
			if (uiIsAvailable)
			{
				GetCurrentMapTime();
				((Control)_timeOfDayPanel).Show();
				((Control)_fishPanel).Show();
			}
			else
			{
				((Control)_timeOfDayPanel).Hide();
				((Control)_fishPanel).Hide();
			}
			if (_draggingTimeOfDayPanel)
			{
				Point nOffset2 = GameService.Input.get_Mouse().get_Position() - _dragTimeOfDayPanelStart;
				ClickThroughPanel timeOfDayPanel = _timeOfDayPanel;
				((Control)timeOfDayPanel).set_Location(((Control)timeOfDayPanel).get_Location() + nOffset2);
				_dragTimeOfDayPanelStart = GameService.Input.get_Mouse().get_Position();
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
			ClickThroughPanel timeOfDayPanel = _timeOfDayPanel;
			if (timeOfDayPanel != null)
			{
				((Control)timeOfDayPanel).Dispose();
			}
			_timeOfDayPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragTimeOfDayPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_timeOfDayImgWidth.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			ClickThroughPanel fishPanel = _fishPanel;
			if (fishPanel != null)
			{
				((Control)fishPanel).Dispose();
			}
			_fishPanelLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnUpdateSettings);
			_dragFishPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateSettings);
			_fishImgWidth.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnUpdateSettings);
			_ignoreCaughtFish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_includeSaltwater.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_includeWorldClass.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			_showRarityBorder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUpdateFish);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
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

		protected virtual async void OnUpdateFish(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			Logger.Debug("Fish settings updated");
			GetCurrentMapTime();
			await getCurrentMapsFish();
			DrawIcons();
		}

		protected void DrawIcons()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			ClickThroughPanel timeOfDayPanel = _timeOfDayPanel;
			if (timeOfDayPanel != null)
			{
				((Control)timeOfDayPanel).Dispose();
			}
			ClickThroughPanel fishPanel = _fishPanel;
			if (fishPanel != null)
			{
				((Control)fishPanel).Dispose();
			}
			ClickThroughPanel clickThroughPanel = new ClickThroughPanel();
			((Control)clickThroughPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)clickThroughPanel).set_Location(_timeOfDayPanelLoc.get_Value());
			((Control)clickThroughPanel).set_Size(new Point(_timeOfDayImgWidth.get_Value()));
			clickThroughPanel.capture = _dragTimeOfDayPanel.get_Value();
			_timeOfDayPanel = clickThroughPanel;
			int fishPanelRows = Clamp((int)Math.Ceiling((double)catchableFish.Count() / 2.0), 1, 7);
			int fishPanelColumns = Clamp((int)Math.Ceiling((double)catchableFish.Count() / (double)fishPanelRows), 1, 7);
			if (fishPanelRows < fishPanelColumns)
			{
				int num = fishPanelRows;
				fishPanelRows = fishPanelColumns;
				fishPanelColumns = num;
			}
			ClickThroughPanel clickThroughPanel2 = new ClickThroughPanel();
			((Control)clickThroughPanel2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)clickThroughPanel2).set_Location(_fishPanelLoc.get_Value());
			((Control)clickThroughPanel2).set_Size(new Point(fishPanelColumns * _fishImgWidth.get_Value(), fishPanelRows * _fishImgWidth.get_Value()));
			clickThroughPanel2.capture = _dragFishPanel.get_Value();
			_fishPanel = clickThroughPanel2;
			Logger.Debug($"Fish Panel Size; Rows: {fishPanelRows} Columns: {fishPanelColumns}, {((Control)_fishPanel).get_Size()}");
			ClickThroughImage clickThroughImage = new ClickThroughImage();
			((Control)clickThroughImage).set_Parent((Container)(object)_timeOfDayPanel);
			((Image)clickThroughImage).set_Texture(AsyncTexture2D.op_Implicit(_imgDawn));
			((Control)clickThroughImage).set_Size(new Point(_timeOfDayImgWidth.get_Value()));
			((Control)clickThroughImage).set_Location(new Point(0));
			((Control)clickThroughImage).set_Opacity(1f);
			((Control)clickThroughImage).set_BasicTooltipText("Dawn");
			((Control)clickThroughImage).set_Visible(timeOfDay == "Dawn");
			clickThroughImage.capture = _dragTimeOfDayPanel.get_Value();
			_dawn = clickThroughImage;
			ClickThroughImage clickThroughImage2 = new ClickThroughImage();
			((Control)clickThroughImage2).set_Parent((Container)(object)_timeOfDayPanel);
			((Image)clickThroughImage2).set_Texture(AsyncTexture2D.op_Implicit(_imgDay));
			((Control)clickThroughImage2).set_Size(new Point(_timeOfDayImgWidth.get_Value()));
			((Control)clickThroughImage2).set_Location(new Point(0));
			((Control)clickThroughImage2).set_Opacity(1f);
			((Control)clickThroughImage2).set_BasicTooltipText("Day");
			((Control)clickThroughImage2).set_Visible(timeOfDay == "Day");
			clickThroughImage2.capture = _dragTimeOfDayPanel.get_Value();
			_day = clickThroughImage2;
			ClickThroughImage clickThroughImage3 = new ClickThroughImage();
			((Control)clickThroughImage3).set_Parent((Container)(object)_timeOfDayPanel);
			((Image)clickThroughImage3).set_Texture(AsyncTexture2D.op_Implicit(_imgDusk));
			((Control)clickThroughImage3).set_Size(new Point(_timeOfDayImgWidth.get_Value()));
			((Control)clickThroughImage3).set_Location(new Point(0));
			((Control)clickThroughImage3).set_Opacity(1f);
			((Control)clickThroughImage3).set_BasicTooltipText("Dusk");
			((Control)clickThroughImage3).set_Visible(timeOfDay == "Dusk");
			clickThroughImage3.capture = _dragTimeOfDayPanel.get_Value();
			_dusk = clickThroughImage3;
			ClickThroughImage clickThroughImage4 = new ClickThroughImage();
			((Control)clickThroughImage4).set_Parent((Container)(object)_timeOfDayPanel);
			((Image)clickThroughImage4).set_Texture(AsyncTexture2D.op_Implicit(_imgNight));
			((Control)clickThroughImage4).set_Size(new Point(_timeOfDayImgWidth.get_Value()));
			((Control)clickThroughImage4).set_Location(new Point(0));
			((Control)clickThroughImage4).set_Opacity(1f);
			((Control)clickThroughImage4).set_BasicTooltipText("Night");
			((Control)clickThroughImage4).set_Visible(timeOfDay == "Night");
			clickThroughImage4.capture = _dragTimeOfDayPanel.get_Value();
			_night = clickThroughImage4;
			int x = 0;
			int y = 0;
			int count = 1;
			foreach (Fish fish in catchableFish)
			{
				string openWater = (fish.openWater ? ", Open Water" : "");
				ClickThroughImage clickThroughImage5 = new ClickThroughImage();
				((Control)clickThroughImage5).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage5).set_Texture(GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(fish.icon)));
				((Control)clickThroughImage5).set_Size(new Point(_fishImgWidth.get_Value()));
				((Control)clickThroughImage5).set_Location(new Point(x, y));
				((Control)clickThroughImage5).set_ZIndex(0);
				clickThroughImage5.capture = _dragFishPanel.get_Value();
				ClickThroughImage clickThroughImage6 = new ClickThroughImage();
				((Control)clickThroughImage6).set_Parent((Container)(object)_fishPanel);
				((Image)clickThroughImage6).set_Texture(AsyncTexture2D.op_Implicit(_showRarityBorder.get_Value() ? GetImageBorder(fish.rarity) : _imgBorderBlack));
				((Control)clickThroughImage6).set_Size(new Point(_fishImgWidth.get_Value()));
				((Control)clickThroughImage6).set_Opacity(0.8f);
				((Control)clickThroughImage6).set_Location(new Point(x, y));
				((Control)clickThroughImage6).set_BasicTooltipText(fish.name + "\nFishing Hole: " + fish.fishingHole + openWater + "\nFavored Bait: " + fish.bait + "\nTime of Day: " + ((fish.timeOfDay == Fish.TimeOfDay.DawnDusk) ? "Dusk/Dawn" : fish.timeOfDay.ToString()) + "\nAchievement: " + fish.achievement + "\nRarity: " + fish.rarity);
				((Control)clickThroughImage6).set_ZIndex(1);
				clickThroughImage6.capture = _dragFishPanel.get_Value();
				x += _fishImgWidth.get_Value();
				if (count == fishPanelColumns)
				{
					x = 0;
					y += _fishImgWidth.get_Value();
					count = 0;
				}
				count++;
			}
			if (_dragTimeOfDayPanel.get_Value())
			{
				_timeOfDayPanel.capture = true;
				((Control)_timeOfDayPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					_draggingTimeOfDayPanel = true;
					_dragTimeOfDayPanelStart = GameService.Input.get_Mouse().get_Position();
					((Panel)_timeOfDayPanel).set_ShowTint(true);
				});
				((Control)_timeOfDayPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					_draggingTimeOfDayPanel = false;
					_timeOfDayPanelLoc.set_Value(((Control)_timeOfDayPanel).get_Location());
					((Panel)_timeOfDayPanel).set_ShowTint(false);
				});
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
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
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
			switch (timeOfDay)
			{
			case "Dawn":
				((Control)_night).set_Visible(false);
				((Control)_dawn).set_Visible(true);
				break;
			case "Day":
				((Control)_dawn).set_Visible(false);
				((Control)_day).set_Visible(true);
				break;
			case "Dusk":
				((Control)_day).set_Visible(false);
				((Control)_dusk).set_Visible(true);
				break;
			case "Night":
				((Control)_dusk).set_Visible(false);
				((Control)_night).set_Visible(true);
				break;
			}
			DrawIcons();
		}

		private void GetCurrentMapTime()
		{
			if (MumbleIsAvailable)
			{
				timeOfDay = TyriaTime.CurrentMapTime(GameService.Gw2Mumble.get_CurrentMap().get_Id());
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
							.get_Achievements()).GetAsync(default(CancellationToken)))).Where((AccountAchievement achievement) => FishingMaps.FISHER_ACHIEVEMENT_IDS.Contains(achievement.get_Id()) && (!achievement.get_Done() || achievement.get_Repeated().HasValue));
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
				catch (Exception ex3)
				{
					Logger.Debug(ex3, "Failed to query Guild Wars 2 API.");
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
					catch (Exception ex2)
					{
						Logger.Debug(ex2, "Couldn't get player's current map.");
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
					Logger.Debug("Current map achieveable: " + string.Join(", ", currentMapAchievable.Select((AccountAchievement achievement) => achievement.get_Id())));
					int bitsCounter = 0;
					foreach (AccountAchievement accountAchievement in currentMapAchievable)
					{
						Achievement currentAchievement2 = await RequestAchievement(accountAchievement.get_Id());
						if (currentAchievement2 == null)
						{
							continue;
						}
						foreach (AchievementBit bit2 in currentAchievement2.get_Bits())
						{
							if (bit2 == null)
							{
								Logger.Debug($"Bit in {currentAchievement2.get_Id()} is null");
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
							if (ghoti2.timeOfDay != Fish.TimeOfDay.Any && !timeOfDay.Equals("Dawn") && !timeOfDay.Equals("Dusk") && !object.Equals(ghoti2.timeOfDay.ToString(), timeOfDay))
							{
								bitsCounter++;
								continue;
							}
							ghoti2.icon = fish2.get_Icon();
							ghoti2.itemId = fish2.get_Id();
							ghoti2.achievementId = currentAchievement2.get_Id();
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
						Achievement currentAchievement2 = await RequestAchievement(achievementId2);
						if (currentAchievement2 == null)
						{
							continue;
						}
						foreach (AchievementBit bit in currentAchievement2.get_Bits())
						{
							if (bit == null)
							{
								Logger.Debug($"Bit in {currentAchievement2.get_Id()} is null");
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
							}
							else if (ghoti.timeOfDay == Fish.TimeOfDay.Any || timeOfDay.Equals("Dawn") || timeOfDay.Equals("Dusk") || object.Equals(ghoti.timeOfDay.ToString(), timeOfDay))
							{
								ghoti.icon = fish.get_Icon();
								ghoti.itemId = fish.get_Id();
								ghoti.achievementId = currentAchievement2.get_Id();
								catchableFish.Add(ghoti);
							}
						}
					}
				}
				Logger.Debug("Shown catchable fish in current map count: " + catchableFish.Count());
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
	}
}
