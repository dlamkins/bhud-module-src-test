using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Manlaan.Mounts.Controls;
using Manlaan.Mounts.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static Collection<Mount> _mounts;

		public static string mountsDirectory;

		private TabbedWindow2 _settingsWindow;

		public static List<MountImageFile> _mountImageFiles = new List<MountImageFile>();

		public static int[] _mountOrder = new int[11]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10
		};

		public static string[] _mountBehaviour = new string[2] { "DefaultMount", "Radial" };

		public static string[] _mountOrientation = new string[2] { "Horizontal", "Vertical" };

		public static string[] _mountRadialCenterMountBehavior = new string[3] { "None", "Default", "LastUsed" };

		public static SettingEntry<string> _settingDefaultMountChoice;

		public static SettingEntry<string> _settingDefaultWaterMountChoice;

		public static SettingEntry<string> _settingDefaultFlyingMountChoice;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<bool> _settingDisplayMountQueueing;

		public static SettingEntry<string> _settingDefaultMountBehaviour;

		public static SettingEntry<bool> _settingMountRadialSpawnAtMouse;

		public static SettingEntry<float> _settingMountRadialRadiusModifier;

		public static SettingEntry<float> _settingMountRadialStartAngle;

		public static SettingEntry<float> _settingMountRadialIconSizeModifier;

		public static SettingEntry<float> _settingMountRadialIconOpacity;

		public static SettingEntry<string> _settingMountRadialCenterMountBehavior;

		public static SettingEntry<bool> _settingMountRadialRemoveCenterMount;

		public static SettingEntry<KeyBinding> _settingMountRadialToggleActionCameraKeyBinding;

		public static SettingEntry<bool> _settingDisplayModuleOnLoadingScreen;

		public static SettingEntry<bool> _settingMountAutomaticallyAfterLoadingScreen;

		public static SettingEntry<string> _settingDisplay;

		public static SettingEntry<bool> _settingDisplayCornerIcons;

		public static SettingEntry<bool> _settingDisplayManualIcons;

		public static SettingEntry<string> _settingOrientation;

		private SettingEntry<Point> _settingLoc;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<int> _settingImgWidth;

		public static SettingEntry<float> _settingOpacity;

		private Panel _mountPanel;

		public static DebugControl _debug;

		private DrawRadial _radial;

		private LoadingSpinner _queueingSpinner;

		private DrawMouseCursor _drawMouseCursor;

		private Helper _helper;

		private TextureCache _textureCache;

		private float _lastZPosition;

		private double _lastUpdateSeconds;

		public static bool IsPlayerGlidingOrFalling;

		private bool _lastIsMountSwitchable;

		private bool _dragging;

		private Point _dragStart = Point.get_Zero();

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal static List<Mount> _availableOrderedMounts => (from m in _mounts
			where m.IsAvailable
			orderby m.OrderSetting.get_Value()
			select m).ToList();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_helper = new Helper();
		}

		protected override void Initialize()
		{
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Expected O, but got Unknown
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Expected O, but got Unknown
			List<string> obj = new List<string>
			{
				"griffon-text.png", "griffon-trans.png", "griffon.png", "jackal-text.png", "jackal-trans.png", "jackal.png", "raptor-text.png", "raptor-trans.png", "raptor.png", "roller-text.png",
				"roller-trans.png", "roller.png", "skimmer-text.png", "skimmer-trans.png", "skimmer.png", "skyscale-text.png", "skyscale-trans.png", "skyscale.png", "springer-text.png", "springer-trans.png",
				"springer.png", "turtle-text.png", "turtle-trans.png", "turtle.png", "warclaw-text.png", "warclaw-trans.png", "warclaw.png"
			};
			mountsDirectory = DirectoriesManager.GetFullDirectoryPath("mounts");
			obj.ForEach(delegate(string f)
			{
				ExtractFile(f, mountsDirectory);
			});
			_mountImageFiles = (from file in Directory.GetFiles(mountsDirectory, ".")
				where file.ToLower().Contains(".png")
				select new MountImageFile
				{
					Name = file.Substring(mountsDirectory.Length + 1)
				}).ToList();
			_textureCache = new TextureCache(ContentsManager);
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)async delegate(object sender, ValueEventArgs<bool> e)
			{
				await HandleCombatChangeAsync(sender, e);
			});
			Texture2D mountsIcon = _textureCache.GetImgFile(TextureCache.MountLogoTextureName);
			TabbedWindow2 val = new TabbedWindow2(_textureCache.GetImgFile(TextureCache.TabBackgroundTextureName), new Rectangle(35, 36, 1300, 900), new Rectangle(95, 42, 1221, 792));
			((WindowBase2)val).set_Title("Mounts");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(new Point(100, 100));
			((WindowBase2)val).set_Emblem(mountsIcon);
			((WindowBase2)val).set_Id(((Module)this).get_Namespace() + "_SettingsWindow");
			((WindowBase2)val).set_SavesPosition(true);
			_settingsWindow = val;
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(_textureCache.GetImgFile(TextureCache.SettingsIconTextureName)), (Func<IView>)(() => (IView)(object)new SettingsView(_textureCache)), Strings.Window_AllSettingsTab, (int?)null));
		}

		private void ExtractFile(string filePath, string directoryToExtractTo)
		{
			string fullPath = Path.Combine(directoryToExtractTo, filePath);
			using Stream fs = ContentsManager.GetFileStream(filePath);
			fs.Position = 0L;
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int)fs.Length);
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			File.WriteAllBytes(fullPath, buffer);
		}

		private void MigrateDisplaySettings()
		{
			if (_settingDisplay.get_Value().Contains("Corner") || _settingDisplay.get_Value().Contains("Manual"))
			{
				_settingDisplayCornerIcons.set_Value(_settingDisplay.get_Value().Contains("Corner"));
				_settingDisplayManualIcons.set_Value(_settingDisplay.get_Value().Contains("Solid"));
				if (_settingDisplay.get_Value().Contains("Text"))
				{
					_settingDisplay.set_Value("SolidText");
				}
				else if (_settingDisplay.get_Value().Contains("Solid"))
				{
					_settingDisplay.set_Value("Solid");
				}
				else if (_settingDisplay.get_Value().Contains("Transparent"))
				{
					_settingDisplay.set_Value("Transparent");
				}
			}
		}

		private void MigrateMountFileNameSettings()
		{
			if (!_mounts.All((Mount m) => m.ImageFileNameSetting.get_Value().Equals("")))
			{
				return;
			}
			string partOfFileName = "";
			if (_settingDisplay.get_Value().Equals("Transparent"))
			{
				partOfFileName = "-trans";
			}
			else if (_settingDisplay.get_Value().Equals("SolidText"))
			{
				partOfFileName = "-text";
			}
			else if (_settingDisplay.get_Value().Equals("Solid"))
			{
				partOfFileName = "";
			}
			foreach (Mount mount in _mounts)
			{
				mount.ImageFileNameSetting.set_Value(mount.ImageFileName + partOfFileName + ".png");
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Expected O, but got Unknown
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			_mounts = new Collection<Mount>
			{
				new Raptor(settings, _helper),
				new Springer(settings, _helper),
				new Skimmer(settings, _helper),
				new Jackal(settings, _helper),
				new Griffon(settings, _helper),
				new RollerBeetle(settings, _helper),
				new Warclaw(settings, _helper),
				new Skyscale(settings, _helper),
				new SiegeTurtle(settings, _helper)
			};
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Setting_DefaultMountBinding), (Func<string>)(() => ""));
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await DoDefaultMountActionAsync();
			});
			_settingDefaultMountChoice = settings.DefineSetting<string>("DefaultMountChoice", "Disabled", (Func<string>)(() => Strings.Setting_DefaultMountChoice), (Func<string>)(() => ""));
			_settingDefaultWaterMountChoice = settings.DefineSetting<string>("DefaultWaterMountChoice", "Disabled", (Func<string>)(() => Strings.Setting_DefaultWaterMountChoice), (Func<string>)(() => ""));
			_settingDefaultFlyingMountChoice = settings.DefineSetting<string>("DefaultFlyingMountChoice", "Disabled", (Func<string>)(() => Strings.Setting_DefaultFlyingMountChoice), (Func<string>)(() => ""));
			_settingDefaultMountBehaviour = settings.DefineSetting<string>("DefaultMountBehaviour", "Radial", (Func<string>)(() => Strings.Setting_DefaultMountBehaviour), (Func<string>)(() => ""));
			_settingDisplayMountQueueing = settings.DefineSetting<bool>("DisplayMountQueueing", false, (Func<string>)(() => Strings.Setting_DisplayMountQueueing), (Func<string>)(() => ""));
			_settingMountRadialSpawnAtMouse = settings.DefineSetting<bool>("MountRadialSpawnAtMouse", false, (Func<string>)(() => Strings.Setting_MountRadialSpawnAtMouse), (Func<string>)(() => ""));
			_settingMountRadialIconSizeModifier = settings.DefineSetting<float>("MountRadialIconSizeModifier", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialIconSizeModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconSizeModifier, 0.05f, 1f);
			_settingMountRadialRadiusModifier = settings.DefineSetting<float>("MountRadialRadiusModifier", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialRadiusModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialRadiusModifier, 0.2f, 1f);
			_settingMountRadialStartAngle = settings.DefineSetting<float>("MountRadialStartAngle", 0f, (Func<string>)(() => Strings.Setting_MountRadialStartAngle), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialStartAngle, 0f, 1f);
			_settingMountRadialIconOpacity = settings.DefineSetting<float>("MountRadialIconOpacity", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialIconOpacity), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconOpacity, 0.05f, 1f);
			_settingMountRadialCenterMountBehavior = settings.DefineSetting<string>("MountRadialCenterMountBehavior", "Default", (Func<string>)(() => Strings.Setting_MountRadialCenterMountBehavior), (Func<string>)(() => ""));
			_settingMountRadialRemoveCenterMount = settings.DefineSetting<bool>("MountRadialRemoveCenterMount", true, (Func<string>)(() => Strings.Setting_MountRadialRemoveCenterMount), (Func<string>)(() => ""));
			_settingMountRadialToggleActionCameraKeyBinding = settings.DefineSetting<KeyBinding>("MountRadialToggleActionCameraKeyBinding", new KeyBinding((Keys)121), (Func<string>)(() => Strings.Setting_MountRadialToggleActionCameraKeyBinding), (Func<string>)(() => ""));
			_settingDisplayModuleOnLoadingScreen = settings.DefineSetting<bool>("DisplayModuleOnLoadingScreen", false, (Func<string>)(() => Strings.Setting_DisplayModuleOnLoadingScreen), (Func<string>)(() => ""));
			_settingMountAutomaticallyAfterLoadingScreen = settings.DefineSetting<bool>("MountAutomaticallyAfterLoadingScreen", false, (Func<string>)(() => Strings.Setting_MountAutomaticallyAfterLoadingScreen), (Func<string>)(() => ""));
			_settingDisplay = settings.DefineSetting<string>("MountDisplay", "Transparent", (Func<string>)(() => Strings.Setting_MountDisplay), (Func<string>)(() => ""));
			_settingDisplayCornerIcons = settings.DefineSetting<bool>("MountDisplayCornerIcons", false, (Func<string>)(() => Strings.Setting_MountDisplayCornerIcons), (Func<string>)(() => ""));
			_settingDisplayManualIcons = settings.DefineSetting<bool>("MountDisplayManualIcons", false, (Func<string>)(() => Strings.Setting_MountDisplayManualIcons), (Func<string>)(() => ""));
			_settingOrientation = settings.DefineSetting<string>("Orientation", "Horizontal", (Func<string>)(() => Strings.Setting_Orientation), (Func<string>)(() => ""));
			_settingLoc = settings.DefineSetting<Point>("MountLoc", new Point(100, 100), (Func<string>)(() => Strings.Setting_MountLoc), (Func<string>)(() => ""));
			_settingDrag = settings.DefineSetting<bool>("MountDrag", false, (Func<string>)(() => Strings.Setting_MountDrag), (Func<string>)(() => ""));
			_settingImgWidth = settings.DefineSetting<int>("MountImgWidth", 50, (Func<string>)(() => Strings.Setting_MountImgWidth), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingImgWidth, 0, 200);
			_settingOpacity = settings.DefineSetting<float>("MountOpacity", 1f, (Func<string>)(() => Strings.Setting_MountOpacity), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingOpacity, 0f, 1f);
			MigrateDisplaySettings();
			MigrateMountFileNameSettings();
			foreach (Mount mount in _mounts)
			{
				mount.OrderSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
				mount.KeybindingSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
				mount.ImageFileNameSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			}
			_settingDefaultMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultFlyingMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayModuleOnLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountAutomaticallyAfterLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialStartAngle.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingMountRadialToggleActionCameraKeyBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingMountRadialIconOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRemoveCenterMount.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplay.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayCornerIcons.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayManualIcons.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingOrientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingImgWidth.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
		}

		public override IView GetSettingsView()
		{
			DummySettingsView dummySettingsView = new DummySettingsView();
			dummySettingsView.OnSettingsButtonClicked = (EventHandler)Delegate.Combine(dummySettingsView.OnSettingsButtonClicked, (EventHandler)delegate
			{
				_settingsWindow.set_SelectedTab(((IEnumerable<Tab>)_settingsWindow.get_Tabs()).First());
				((Control)_settingsWindow).Show();
			});
			return (IView)(object)dummySettingsView;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DrawUI();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			float currentZPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
			double currentUpdateSeconds = gameTime.get_TotalGameTime().TotalSeconds;
			double secondsDiff = currentUpdateSeconds - _lastUpdateSeconds;
			float zPositionDiff = currentZPosition - _lastZPosition;
			if ((double)zPositionDiff < -0.0001 && secondsDiff != 0.0)
			{
				IsPlayerGlidingOrFalling = (double)zPositionDiff / secondsDiff < -2.5;
			}
			else
			{
				IsPlayerGlidingOrFalling = false;
			}
			_lastZPosition = currentZPosition;
			_lastUpdateSeconds = currentUpdateSeconds;
			bool isMountSwitchable = IsMountSwitchable();
			bool num = _lastIsMountSwitchable && !isMountSwitchable;
			bool moduleShown = !_lastIsMountSwitchable && isMountSwitchable;
			MountType currentMount = GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount();
			string currentCharacterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			if (num && (int)currentMount != 0 && _settingMountAutomaticallyAfterLoadingScreen.get_Value())
			{
				_helper.StoreMountForLaterUse(_mounts.Single((Mount m) => m.MountType == currentMount), currentCharacterName);
			}
			if (moduleShown && (int)currentMount == 0 && _helper.IsCharacterTheSameAfterMapLoad(currentCharacterName))
			{
				_helper.DoMountActionForLaterUse();
				_helper.ClearMountForLaterUse();
			}
			_lastIsMountSwitchable = isMountSwitchable;
			bool shouldShowModule = ShouldShowModule();
			if (shouldShowModule)
			{
				Panel mountPanel = _mountPanel;
				if (mountPanel != null)
				{
					((Control)mountPanel).Show();
				}
				foreach (Mount availableOrderedMount in _availableOrderedMounts)
				{
					CornerIcon cornerIcon = availableOrderedMount.CornerIcon;
					if (cornerIcon != null)
					{
						((Control)cornerIcon).Show();
					}
				}
			}
			else
			{
				Panel mountPanel2 = _mountPanel;
				if (mountPanel2 != null)
				{
					((Control)mountPanel2).Hide();
				}
				foreach (Mount availableOrderedMount2 in _availableOrderedMounts)
				{
					CornerIcon cornerIcon2 = availableOrderedMount2.CornerIcon;
					if (cornerIcon2 != null)
					{
						((Control)cornerIcon2).Hide();
					}
				}
			}
			if (_dragging)
			{
				Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragStart;
				Panel mountPanel3 = _mountPanel;
				((Control)mountPanel3).set_Location(((Control)mountPanel3).get_Location() + nOffset);
				_dragStart = GameService.Input.get_Mouse().get_Position();
			}
			if (_settingDisplayMountQueueing.get_Value() && _mounts.Any((Mount m) => m.QueuedTimestamp.HasValue))
			{
				LoadingSpinner queueingSpinner = _queueingSpinner;
				if (queueingSpinner != null)
				{
					((Control)queueingSpinner).Show();
				}
			}
			if ((((Control)_radial).get_Visible() && !_settingDefaultMountBinding.get_Value().get_IsTriggering()) || !shouldShowModule)
			{
				((Control)_radial).Hide();
			}
		}

		public static bool ShouldShowModule()
		{
			if (!_settingDisplayModuleOnLoadingScreen.get_Value())
			{
				return IsMountSwitchable();
			}
			return true;
		}

		public static bool IsMountSwitchable()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		protected override void Unload()
		{
			DebugControl debug = _debug;
			if (debug != null)
			{
				((Control)debug).Dispose();
			}
			Panel mountPanel = _mountPanel;
			if (mountPanel != null)
			{
				((Control)mountPanel).Dispose();
			}
			DrawRadial radial = _radial;
			if (radial != null)
			{
				((Control)radial).Dispose();
			}
			foreach (Mount mount in _mounts)
			{
				mount.OrderSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
				mount.KeybindingSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
				mount.ImageFileNameSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
				mount.DisposeCornerIcon();
			}
			_settingDefaultMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultFlyingMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialStartAngle.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingMountRadialToggleActionCameraKeyBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingMountRadialIconOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRemoveCenterMount.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayModuleOnLoadingScreen.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountAutomaticallyAfterLoadingScreen.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayCornerIcons.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayManualIcons.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingOrientation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingImgWidth.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<KeyBinding> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<float> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			DrawUI();
		}

		internal void DrawManualIcons()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			int curX = 0;
			int curY = 0;
			int totalMounts = 0;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(_settingLoc.get_Value());
			((Control)val).set_Size(new Point(_settingImgWidth.get_Value() * 8, _settingImgWidth.get_Value() * 8));
			_mountPanel = val;
			foreach (Mount mount in _availableOrderedMounts)
			{
				Texture2D img = _textureCache.GetMountImgFile(mount);
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)_mountPanel);
				val2.set_Texture(AsyncTexture2D.op_Implicit(img));
				((Control)val2).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
				((Control)val2).set_Location(new Point(curX, curY));
				((Control)val2).set_Opacity(_settingOpacity.get_Value());
				((Control)val2).set_BasicTooltipText(mount.DisplayName);
				((Control)val2).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)async delegate
				{
					await mount.DoMountAction();
				});
				if (_settingOrientation.get_Value().Equals("Horizontal"))
				{
					curX += _settingImgWidth.get_Value();
				}
				else
				{
					curY += _settingImgWidth.get_Value();
				}
				totalMounts++;
			}
			if (_settingDrag.get_Value())
			{
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)_mountPanel);
				((Control)val3).set_Location(new Point(0, 0));
				((Control)val3).set_Size(new Point(_settingImgWidth.get_Value() / 2, _settingImgWidth.get_Value() / 2));
				((Control)val3).set_BackgroundColor(Color.get_White());
				((Control)val3).set_ZIndex(10);
				((Control)val3).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					_dragging = true;
					_dragStart = GameService.Input.get_Mouse().get_Position();
				});
				((Control)val3).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					_dragging = false;
					_settingLoc.set_Value(((Control)_mountPanel).get_Location());
				});
			}
			if (_settingOrientation.get_Value().Equals("Horizontal"))
			{
				((Control)_mountPanel).set_Size(new Point(_settingImgWidth.get_Value() * totalMounts, _settingImgWidth.get_Value()));
			}
			else
			{
				((Control)_mountPanel).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value() * totalMounts));
			}
		}

		private void DrawCornerIcons()
		{
			foreach (Mount mount in _availableOrderedMounts)
			{
				mount.CreateCornerIcon(_textureCache.GetMountImgFile(mount));
			}
		}

		private void DrawUI()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			Panel mountPanel = _mountPanel;
			if (mountPanel != null)
			{
				((Control)mountPanel).Dispose();
			}
			foreach (Mount mount in _mounts)
			{
				mount.DisposeCornerIcon();
			}
			DebugControl debug = _debug;
			if (debug != null)
			{
				((Control)debug).Dispose();
			}
			DebugControl debugControl = new DebugControl();
			((Control)debugControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)debugControl).set_Location(new Point(0, 0));
			((Control)debugControl).set_Size(new Point(500, 500));
			_debug = debugControl;
			if (_settingDisplayCornerIcons.get_Value())
			{
				DrawCornerIcons();
			}
			if (_settingDisplayManualIcons.get_Value())
			{
				DrawManualIcons();
			}
			LoadingSpinner queueingSpinner = _queueingSpinner;
			if (queueingSpinner != null)
			{
				((Control)queueingSpinner).Dispose();
			}
			_queueingSpinner = new LoadingSpinner();
			((Control)_queueingSpinner).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 + 400, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_queueingSpinner).get_Height() - 25));
			((Control)_queueingSpinner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)_queueingSpinner).Hide();
			DrawMouseCursor drawMouseCursor = _drawMouseCursor;
			if (drawMouseCursor != null)
			{
				((Control)drawMouseCursor).Dispose();
			}
			_drawMouseCursor = new DrawMouseCursor(_textureCache);
			((Control)_drawMouseCursor).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)_drawMouseCursor).Hide();
			DrawRadial radial = _radial;
			if (radial != null)
			{
				((Control)radial).Dispose();
			}
			_radial = new DrawRadial(_helper, _textureCache);
			((Control)_radial).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			DrawRadial radial2 = _radial;
			radial2.OnSettingsButtonClicked = (EventHandler)Delegate.Combine(radial2.OnSettingsButtonClicked, (EventHandler)delegate
			{
				_settingsWindow.set_SelectedTab(((IEnumerable<Tab>)_settingsWindow.get_Tabs()).First());
				((Control)_settingsWindow).Show();
			});
		}

		private async Task DoDefaultMountActionAsync()
		{
			Logger.Debug("DoDefaultMountActionAsync entered");
			if ((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() != 0 && IsMountSwitchable())
			{
				await (_helper.GetCurrentlyActiveMount()?.DoUnmountAction() ?? Task.CompletedTask);
				Logger.Debug("DoDefaultMountActionAsync dismounted");
				return;
			}
			Mount instantMount = _helper.GetInstantMount();
			if (instantMount != null)
			{
				await instantMount.DoMountAction();
				Logger.Debug("DoDefaultMountActionAsync instantmount");
				return;
			}
			Mount defaultMount = _helper.GetDefaultMount();
			if (defaultMount != null && GameService.Input.get_Mouse().get_CameraDragging())
			{
				await (defaultMount?.DoMountAction() ?? Task.CompletedTask);
				Logger.Debug("DoDefaultMountActionAsync CameraDragging defaultmount");
				return;
			}
			string value = _settingDefaultMountBehaviour.get_Value();
			if (!(value == "DefaultMount"))
			{
				if (value == "Radial" && ShouldShowModule())
				{
					DrawRadial radial = _radial;
					if (radial != null)
					{
						((Control)radial).Show();
					}
					Logger.Debug("DoDefaultMountActionAsync DefaultMountBehaviour radial");
				}
			}
			else
			{
				await (defaultMount?.DoMountAction() ?? Task.CompletedTask);
				Logger.Debug("DoDefaultMountActionAsync DefaultMountBehaviour defaultmount");
			}
		}

		private async Task HandleCombatChangeAsync(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				return;
			}
			await ((from m in _mounts
				where m.QueuedTimestamp.HasValue
				orderby m.QueuedTimestamp descending
				select m).FirstOrDefault()?.DoMountAction() ?? Task.CompletedTask);
			foreach (Mount mount in _mounts)
			{
				mount.QueuedTimestamp = null;
			}
			LoadingSpinner queueingSpinner = _queueingSpinner;
			if (queueingSpinner != null)
			{
				((Control)queueingSpinner).Hide();
			}
		}
	}
}
