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
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.Mounts.Controls;
using Manlaan.Mounts.Things;
using Manlaan.Mounts.Things.Mounts;
using Manlaan.Mounts.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mounts;
using Mounts.Settings;
using Newtonsoft.Json;
using Taimi.UndaDaSea_BlishHUD;

namespace Manlaan.Mounts
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static SettingCollection settingscollection;

		internal static Collection<Thing> _things = new Collection<Thing>();

		internal static List<ContextualRadialThingSettings> ContextualRadialSettings;

		internal static List<UserDefinedRadialThingSettings> UserDefinedRadialSettings;

		public static SettingEntry<List<int>> _settingUserDefinedRadialIds;

		internal static List<IconThingSettings> IconThingSettings;

		public static SettingEntry<List<int>> _settingDrawIconIds;

		public static List<ThingImageFile> _thingImageFiles = new List<ThingImageFile>();

		public static string thingsDirectory;

		public static string[] _keybindBehaviours = new string[2] { "Default", "Radial" };

		private bool previousTriggeringState;

		private DateTime? lastTriggered;

		private TappedModuleKeybindState tappedModuleKeybind;

		public static SettingEntry<int> _settingsLastRunMigrationVersion;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<bool> _settingBlockSequenceFromGw2;

		public static SettingEntry<bool> _settingDisplayMountQueueing;

		public static SettingEntry<bool> _settingDisplayLaterActivation;

		public static SettingEntry<bool> _settingDisplayGroundTargetingAction;

		public static SettingEntry<GroundTargeting> _settingGroundTargeting;

		public static SettingEntry<bool> _settingEnableMountQueueing;

		public static SettingEntry<Point> _settingInfoPanelLocation;

		public static SettingEntry<bool> _settingDragInfoPanel;

		public static SettingEntry<bool> _settingCombatLaunchMasteryUnlocked;

		public static SettingEntry<string> _settingDefaultMountBehaviour;

		public static SettingEntry<string> _settingKeybindBehaviour;

		public static SettingEntry<bool> _settingMountRadialSpawnAtMouse;

		public static SettingEntry<float> _settingMountRadialRadiusModifier;

		public static SettingEntry<float> _settingMountRadialStartAngle;

		public static SettingEntry<float> _settingMountRadialIconSizeModifier;

		public static SettingEntry<float> _settingMountRadialIconOpacity;

		public static SettingEntry<KeyBinding> _settingMountRadialToggleActionCameraKeyBinding;

		public static SettingEntry<bool> _settingDisplayModuleOnLoadingScreen;

		public static SettingEntry<bool> _settingMountAutomaticallyAfterLoadingScreen;

		public static SettingEntry<KeyBinding> _settingJumpBinding;

		public static SettingEntry<float> _settingFallingOrGlidingUpdateFrequency;

		public static SettingEntry<int> _settingTapThresholdInMilliseconds;

		private TabbedWindow2 _settingsWindow;

		public static DebugControl _debug;

		private DrawRadial _radial;

		private ICollection<DrawIcons> _drawIcons = new List<DrawIcons>();

		private InfoPanel _drawInfoPanel;

		private DrawMouseCursor _drawMouseCursor;

		private Helper _helper;

		private TextureCache _textureCache;

		public static List<SkyLake> _skyLakes = new List<SkyLake>();

		private bool _lastIsThingSwitchable;

		private int _lastInUseThingsCount;

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			_debug = new DebugControl();
			_helper = new Helper(Gw2ApiManager);
		}

		protected override void Initialize()
		{
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Expected O, but got Unknown
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Expected O, but got Unknown
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Expected O, but got Unknown
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Expected O, but got Unknown
			//IL_0665: Unknown result type (might be due to invalid IL or missing references)
			//IL_066f: Expected O, but got Unknown
			List<string> obj = new List<string>
			{
				"griffon-text.png", "griffon-trans.png", "griffon.png", "jackal-text.png", "jackal-trans.png", "jackal.png", "raptor-text.png", "raptor-trans.png", "raptor.png", "roller-text.png",
				"roller-trans.png", "roller.png", "skimmer-text.png", "skimmer-trans.png", "skimmer.png", "skyscale-text.png", "skyscale-trans.png", "skyscale.png", "springer-text.png", "springer-trans.png",
				"springer.png", "turtle-text.png", "turtle-trans.png", "turtle.png", "warclaw-text.png", "warclaw-trans.png", "warclaw.png", "fishing.png", "skiff.png", "jadebotwaypoint.png",
				"chair.png", "music.png", "held.png", "toy.png", "tonic.png", "scanforrift.png", "skyscaleleap.png", "unmount.png", "unmount-trans.png", "fishing-trans.png",
				"fishing-trans-color.png", "jadebotwaypoint-trans.png", "jadebotwaypoint-trans-color.png", "scanforrift-trans.png", "scanforrift-trans-color.png", "skiff-trans.png", "skiff-trans-color.png", "skyscaleleap-trans.png", "skyscaleleap-trans-color.png", "tonic-paint.png",
				"tonic-white.png", "toy-paint.png", "toy-white.png", "chair-paint.png", "chair-whiite.png", "held-paint.png", "held-white.png", "music-paint.png", "music-white.png", "skimmer-remix.png",
				"skyscaleleap-remix.png", "skyscale-remix.png", "springer-remix.png", "tonic-remix.png", "toy-remix.png", "turtle-remix.png", "unmount-remix.png", "warclaw-remix.png", "chair-remix.png", "fishing-remix.png",
				"griffon-remix.png", "held-remix.png", "jackal-remix.png", "jadebotwaypoint-remix.png", "music-remix.png", "raptor-remix.png", "roller-remix.png", "scanforrift-remix.png", "skiff-remix.png", "summonconjureddoorway.png",
				"summonconjureddoorway-trans.png", "summonconjureddoorway-trans-color.png", "griffon_natural.png", "jackal_natural.png", "raptor_natural.png", "roller_natural.png", "skimmer_natural.png", "skyscale_natural.png", "springer_natural.png", "turtle_natural.png",
				"warclaw_natural.png"
			};
			thingsDirectory = DirectoriesManager.GetFullDirectoryPath("mounts");
			obj.ForEach(delegate(string f)
			{
				ExtractFile(f, thingsDirectory);
			});
			_thingImageFiles = (from file in Directory.GetFiles(thingsDirectory, ".")
				where file.ToLower().Contains(".png")
				select new ThingImageFile
				{
					Name = file.Substring(thingsDirectory.Length + 1)
				}).ToList();
			_textureCache = new TextureCache(ContentsManager);
			_skyLakes = LoadSkyLakesFromJson();
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)async delegate(object sender, ValueEventArgs<bool> e)
			{
				await HandleCombatChangeAsync(sender, e);
			});
			Texture2D mountsIcon = _textureCache.GetImgFile(TextureCache.ModuleLogoTextureName);
			TabbedWindow2 val = new TabbedWindow2(_textureCache.GetImgFile(TextureCache.TabBackgroundTextureName), new Rectangle(35, 36, 1300, 900), new Rectangle(95, 42, 1221, 900));
			((WindowBase2)val).set_Title("Mounts & More");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(new Point(100, 100));
			((WindowBase2)val).set_Emblem(mountsIcon);
			((WindowBase2)val).set_Id(((Module)this).get_Namespace() + "_SettingsWindow");
			((WindowBase2)val).set_SavesPosition(true);
			_settingsWindow = val;
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(_textureCache.GetImgFile(TextureCache.SettingsTextureName)), (Func<IView>)(() => (IView)(object)new SettingsView(_textureCache)), Strings.Window_GeneralSettingsTab, (int?)null));
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(_textureCache.GetImgFile(TextureCache.RadialSettingsTextureName)), (Func<IView>)(() => (IView)(object)new RadialThingSettingsView(DoKeybindActionAsync, _helper)), Strings.Window_RadialSettingsTab, (int?)null));
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(_textureCache.GetImgFile(TextureCache.IconSettingsTextureName)), (Func<IView>)(() => (IView)(object)new IconThingSettingsView()), Strings.Window_IconSettingsTab, (int?)null));
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(_textureCache.GetImgFile(TextureCache.SupportMeTabTextureName)), (Func<IView>)(() => (IView)(object)new SupportMeView(_textureCache)), Strings.Window_SupportMeTab, (int?)null));
		}

		public List<SkyLake> LoadSkyLakesFromJson()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			JsonSerializer serializer = new JsonSerializer();
			((Collection<JsonConverter>)(object)serializer.get_Converters()).Add((JsonConverter)(object)new Vector3Converter());
			using StreamReader stream = new StreamReader(ContentsManager.GetFileStream("SkyLakes.json"));
			JsonReader reader = (JsonReader)new JsonTextReader((TextReader)stream);
			try
			{
				return serializer.Deserialize<List<SkyLake>>(reader);
			}
			finally
			{
				((IDisposable)reader)?.Dispose();
			}
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

		private void MigrateToInfoPanelLocation(SettingCollection settings)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (settings.ContainsSetting("DisplayMountQueueingLocation"))
			{
				SettingEntry<Point> settingDisplayMountQueueingLocation = settings.get_Item("DisplayMountQueueingLocation") as SettingEntry<Point>;
				_settingInfoPanelLocation.set_Value(new Point(settingDisplayMountQueueingLocation.get_Value().X, settingDisplayMountQueueingLocation.get_Value().Y));
			}
			if (settings.ContainsSetting("DragMountQueueing"))
			{
				SettingEntry<bool> settingDragMountQueueing = settings.get_Item("DragMountQueueing") as SettingEntry<bool>;
				_settingDragInfoPanel.set_Value(settingDragMountQueueing.get_Value());
			}
		}

		private void MigrateAwayFromMount(SettingCollection settings)
		{
			if (_settingDefaultMountBehaviour.get_Value() == "DefaultMount")
			{
				_settingKeybindBehaviour.set_Value("Default");
			}
			if (_settingDefaultMountBehaviour.get_Value() == "Radial")
			{
				_settingKeybindBehaviour.set_Value("Radial");
			}
		}

		private void MigrateRadialThingSettings(SettingCollection settings)
		{
			if (settings.ContainsSetting("DefaultFlyingMountChoice"))
			{
				ContextualRadialThingSettings flyingRadialSettings = ContextualRadialSettings.Single((ContextualRadialThingSettings c) => c.Name == "IsPlayerGlidingOrFalling");
				SettingEntry<string> settingDefaultFlyingMountChoice = settings.get_Item("DefaultFlyingMountChoice") as SettingEntry<string>;
				if (settingDefaultFlyingMountChoice.get_Value() != "Disabled" && _things.Count((Thing t) => t.Name == settingDefaultFlyingMountChoice.get_Value()) == 1)
				{
					flyingRadialSettings.ApplyInstantlyIfSingle.set_Value(true);
					flyingRadialSettings.IsEnabled.set_Value(true);
					flyingRadialSettings.SetThings(new List<Thing> { _things.Single((Thing t) => t.Name == settingDefaultFlyingMountChoice.get_Value()) });
				}
			}
			if (settings.ContainsSetting("DefaultWaterMountChoice"))
			{
				ContextualRadialThingSettings underwaterRadialSettings = ContextualRadialSettings.Single((ContextualRadialThingSettings c) => c.Name == "IsPlayerUnderWater");
				SettingEntry<string> settingDefaultWaterMountChoice = settings.get_Item("DefaultWaterMountChoice") as SettingEntry<string>;
				if (settingDefaultWaterMountChoice.get_Value() != "Disabled" && _things.Count((Thing t) => t.Name == settingDefaultWaterMountChoice.get_Value()) == 1)
				{
					underwaterRadialSettings.ApplyInstantlyIfSingle.set_Value(true);
					underwaterRadialSettings.IsEnabled.set_Value(true);
					underwaterRadialSettings.SetThings(new List<Thing> { _things.Single((Thing t) => t.Name == settingDefaultWaterMountChoice.get_Value()) });
				}
			}
			ContextualRadialThingSettings defaultRadialSettings = ContextualRadialSettings.Single((ContextualRadialThingSettings c) => c.Name == "Default");
			if (settings.ContainsSetting("DefaultMountChoice"))
			{
				SettingEntry<string> settingDefaultMountChoice = settings.get_Item("DefaultMountChoice") as SettingEntry<string>;
				defaultRadialSettings.DefaultThingChoice.set_Value(settingDefaultMountChoice.get_Value());
			}
			if (settings.ContainsSetting("MountRadialRemoveCenterMount"))
			{
				SettingEntry<bool> settingMountRadialRemoveCenterMount = settings.get_Item("MountRadialRemoveCenterMount") as SettingEntry<bool>;
				defaultRadialSettings.RemoveCenterThing.set_Value(settingMountRadialRemoveCenterMount.get_Value());
			}
			if (settings.ContainsSetting("MountRadialCenterMountBehavior") && Enum.TryParse<CenterBehavior>((settings.get_Item("MountRadialCenterMountBehavior") as SettingEntry<string>).get_Value(), out var result))
			{
				defaultRadialSettings.CenterThingBehavior.set_Value(result);
			}
		}

		private void MigrateIconThingSettings(SettingCollection settings)
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			IconThingSettings defaultIconThingSettings = IconThingSettings.Single((IconThingSettings c) => c.IsDefault);
			if (settings.ContainsSetting("MountDisplayManualIcons"))
			{
				SettingEntry<bool> settingMountDisplayManualIcons = settings.get_Item("MountDisplayManualIcons") as SettingEntry<bool>;
				defaultIconThingSettings.IsEnabled.set_Value(settingMountDisplayManualIcons.get_Value());
			}
			if (settings.ContainsSetting("MountDisplayCornerIcons"))
			{
				SettingEntry<bool> settingMountDisplayCornerIcons = settings.get_Item("MountDisplayCornerIcons") as SettingEntry<bool>;
				defaultIconThingSettings.DisplayCornerIcons.set_Value(settingMountDisplayCornerIcons.get_Value());
			}
			if (settings.ContainsSetting("Orientation") && Enum.TryParse<IconOrientation>((settings.get_Item("Orientation") as SettingEntry<string>).get_Value(), out var result))
			{
				defaultIconThingSettings.Orientation.set_Value(result);
			}
			if (settings.ContainsSetting("MountLoc"))
			{
				SettingEntry<Point> _settingLoc = settings.get_Item("MountLoc") as SettingEntry<Point>;
				defaultIconThingSettings.Location.set_Value(_settingLoc.get_Value());
			}
			if (settings.ContainsSetting("MountDrag"))
			{
				SettingEntry<bool> _settingDrag = settings.get_Item("MountDrag") as SettingEntry<bool>;
				defaultIconThingSettings.IsDraggingEnabled.set_Value(_settingDrag.get_Value());
			}
			if (settings.ContainsSetting("MountImgWidth"))
			{
				SettingEntry<int> _settingMountImgWidth = settings.get_Item("MountImgWidth") as SettingEntry<int>;
				defaultIconThingSettings.Size.set_Value(_settingMountImgWidth.get_Value());
			}
			if (settings.ContainsSetting("MountOpacity"))
			{
				SettingEntry<float> _settingMountOpacity = settings.get_Item("MountOpacity") as SettingEntry<float>;
				defaultIconThingSettings.Opacity.set_Value(_settingMountOpacity.get_Value());
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Expected O, but got Unknown
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Expected O, but got Unknown
			//IL_0758: Unknown result type (might be due to invalid IL or missing references)
			//IL_0764: Expected O, but got Unknown
			settingscollection = settings;
			List<Thing> obj = new List<Thing>
			{
				new UnMount(settings, _helper),
				new Raptor(settings, _helper),
				new Springer(settings, _helper),
				new Skimmer(settings, _helper),
				new Jackal(settings, _helper),
				new Griffon(settings, _helper),
				new RollerBeetle(settings, _helper),
				new Warclaw(settings, _helper),
				new Skyscale(settings, _helper),
				new SiegeTurtle(settings, _helper),
				new Fishing(settings, _helper),
				new Skiff(settings, _helper),
				new JadeBotWaypoint(settings, _helper),
				new ScanForRift(settings, _helper),
				new SkyscaleLeap(settings, _helper),
				new SummonConjuredDoorway(settings, _helper),
				new Chair(settings, _helper),
				new Music(settings, _helper),
				new Held(settings, _helper),
				new Toy(settings, _helper),
				new Tonic(settings, _helper)
			};
			_things = new Collection<Thing>(obj);
			List<Thing> thingsForMigration = obj.ToList();
			_settingsLastRunMigrationVersion = settings.DefineSetting<int>("LastRunMigrationVersion", 0, (Func<string>)null, (Func<string>)null);
			_settingBlockSequenceFromGw2 = settings.DefineSetting<bool>("BlockSequenceFromGw2", true, (Func<string>)null, (Func<string>)null);
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Setting_DefaultMountBinding), (Func<string>)(() => ""));
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().set_BlockSequenceFromGw2(_settingBlockSequenceFromGw2.get_Value());
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await DoKeybindActionAsync(KeybindTriggerType.Module);
			});
			_settingDefaultMountBinding.get_Value().add_BindingChanged((EventHandler<EventArgs>)UpdateSettings);
			_settingDefaultMountBehaviour = settings.DefineSetting<string>("DefaultMountBehaviour", "Radial", (Func<string>)null, (Func<string>)null);
			_settingKeybindBehaviour = settings.DefineSetting<string>("KeybindBehaviour", "Radial", (Func<string>)null, (Func<string>)null);
			_settingDisplayMountQueueing = settings.DefineSetting<bool>("DisplayMountQueueing", false, (Func<string>)null, (Func<string>)null);
			_settingEnableMountQueueing = settings.DefineSetting<bool>("EnableMountQueueing", false, (Func<string>)null, (Func<string>)null);
			_settingDisplayLaterActivation = settings.DefineSetting<bool>("DisplayLaterActivation", false, (Func<string>)null, (Func<string>)null);
			_settingDisplayGroundTargetingAction = settings.DefineSetting<bool>("DisplayGroundTargetingAction", false, (Func<string>)null, (Func<string>)null);
			_settingGroundTargeting = settings.DefineSetting<GroundTargeting>("GroundTargeting", GroundTargeting.Normal, (Func<string>)null, (Func<string>)null);
			_settingCombatLaunchMasteryUnlocked = settings.DefineSetting<bool>("CombatLaunchMasteryUnlocked", false, (Func<string>)null, (Func<string>)null);
			_settingInfoPanelLocation = settings.DefineSetting<Point>("InfoPanelLocation", new Point(200, 200), (Func<string>)null, (Func<string>)null);
			_settingDragInfoPanel = settings.DefineSetting<bool>("DragInfoPanel", false, (Func<string>)null, (Func<string>)null);
			_settingMountRadialSpawnAtMouse = settings.DefineSetting<bool>("MountRadialSpawnAtMouse", false, (Func<string>)(() => Strings.Setting_MountRadialSpawnAtMouse), (Func<string>)(() => ""));
			_settingMountRadialIconSizeModifier = settings.DefineSetting<float>("MountRadialIconSizeModifier", 0.28f, (Func<string>)(() => Strings.Setting_MountRadialIconSizeModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconSizeModifier, 0.05f, 1f);
			_settingMountRadialRadiusModifier = settings.DefineSetting<float>("MountRadialRadiusModifier", 0.6f, (Func<string>)(() => Strings.Setting_MountRadialRadiusModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialRadiusModifier, 0.2f, 1f);
			_settingMountRadialStartAngle = settings.DefineSetting<float>("MountRadialStartAngle", 0f, (Func<string>)(() => Strings.Setting_MountRadialStartAngle), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialStartAngle, 0f, 1f);
			_settingMountRadialIconOpacity = settings.DefineSetting<float>("MountRadialIconOpacity", 0.7f, (Func<string>)(() => Strings.Setting_MountRadialIconOpacity), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconOpacity, 0.05f, 1f);
			_settingMountRadialToggleActionCameraKeyBinding = settings.DefineSetting<KeyBinding>("MountRadialToggleActionCameraKeyBinding", new KeyBinding((Keys)121), (Func<string>)(() => Strings.Setting_MountRadialToggleActionCameraKeyBinding), (Func<string>)(() => ""));
			_settingDrawIconIds = settings.DefineSetting<List<int>>("DrawIconIds", new List<int> { 0 }, (Func<string>)null, (Func<string>)null);
			_settingUserDefinedRadialIds = settings.DefineSetting<List<int>>("UserDefinedRadialIds", new List<int>(), (Func<string>)null, (Func<string>)null);
			_settingDisplayModuleOnLoadingScreen = settings.DefineSetting<bool>("DisplayModuleOnLoadingScreen", false, (Func<string>)(() => Strings.Setting_DisplayModuleOnLoadingScreen), (Func<string>)(() => ""));
			_settingMountAutomaticallyAfterLoadingScreen = settings.DefineSetting<bool>("MountAutomaticallyAfterLoadingScreen", false, (Func<string>)(() => Strings.Setting_MountAutomaticallyAfterLoadingScreen), (Func<string>)(() => ""));
			_settingJumpBinding = settings.DefineSetting<KeyBinding>("JumpKeybinding", new KeyBinding((Keys)32), (Func<string>)null, (Func<string>)null);
			_settingJumpBinding.get_Value().set_Enabled(true);
			_settingJumpBinding.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				_helper.UpdateLastJumped();
			});
			_settingFallingOrGlidingUpdateFrequency = settings.DefineSetting<float>("FallingOrGlidingUpdateFrequency", 0.1f, (Func<string>)null, (Func<string>)null);
			_settingTapThresholdInMilliseconds = settings.DefineSetting<int>("TapThresholdInMilliseconds", 500, (Func<string>)null, (Func<string>)null);
			ContextualRadialSettings = new List<ContextualRadialThingSettings>
			{
				new ContextualRadialThingSettings(settings, "IsPlayerMounted", 0, _helper.IsPlayerMounted, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: true, defaultUnconditionallyDoAction: true, _things.Where((Thing t) => t is Raptor).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerInWvwMap", 1, _helper.IsPlayerInWvwMap, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: true, defaultUnconditionallyDoAction: false, _things.Where((Thing t) => t is Warclaw).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerInCombat", 2, _helper.IsPlayerInCombat, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: true, defaultUnconditionallyDoAction: false, _things.Where((Thing t) => t is Skyscale).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerUnderWater", 3, _helper.IsPlayerUnderWater, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: false, defaultUnconditionallyDoAction: false, _things.Where((Thing t) => t is Skimmer || t is SiegeTurtle).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerOnWaterSurface", 4, _helper.IsPlayerOnWaterSurface, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: true, defaultUnconditionallyDoAction: false, _things.Where((Thing t) => t is Skiff).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerGlidingOrFalling", 5, _helper.IsPlayerGlidingOrFalling, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: false, defaultUnconditionallyDoAction: false, _things.Where((Thing t) => t is Griffon || t is Skyscale).ToList()),
				new ContextualRadialThingSettings(settings, "Default", 99, () => true, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: false, defaultUnconditionallyDoAction: false, thingsForMigration)
			};
			IconThingSettings = new List<IconThingSettings>
			{
				new IconThingSettings(settings, 0, "Default", thingsForMigration)
			};
			IconThingSettings.AddRange(from id in _settingDrawIconIds.get_Value().Skip(1)
				select new IconThingSettings(settings, id));
			UserDefinedRadialSettings = (from id in _settingUserDefinedRadialIds.get_Value()
				select new UserDefinedRadialThingSettings(settings, id, DoKeybindActionAsync)).ToList();
			if (_settingsLastRunMigrationVersion.get_Value() < 1)
			{
				MigrateRadialThingSettings(settings);
				MigrateIconThingSettings(settings);
				MigrateAwayFromMount(settings);
				_settingsLastRunMigrationVersion.set_Value(1);
			}
			if (_settingsLastRunMigrationVersion.get_Value() < 2)
			{
				MigrateToInfoPanelLocation(settings);
				_settingsLastRunMigrationVersion.set_Value(2);
			}
			foreach (Thing thing in _things)
			{
				thing.KeybindingSetting.get_Value().add_BindingChanged((EventHandler<EventArgs>)UpdateSettings);
				thing.ImageFileNameSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			}
			_settingDisplayModuleOnLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountAutomaticallyAfterLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayLaterActivation.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingEnableMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDragInfoPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingInfoPanelLocation.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialStartAngle.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialToggleActionCameraKeyBinding.get_Value().add_BindingChanged((EventHandler<EventArgs>)UpdateSettings);
			_settingMountRadialIconOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingDrawIconIds.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)UpdateSettings);
			_settingUserDefinedRadialIds.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)UpdateSettings);
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
			_debug.Add("GetTriggeredRadialSettings", delegate
			{
				RadialThingSettings triggeredRadialSettings = _helper.GetTriggeredRadialSettings();
				return (triggeredRadialSettings != null) ? $"Name: {triggeredRadialSettings.Name}, Number of things: {triggeredRadialSettings.AvailableThings.Count}" : "";
			});
			ContextualRadialSettings.ForEach(delegate(ContextualRadialThingSettings c)
			{
				_debug.Add($"Contextual RadialSettings {c.Order} {c.Name} A", () => $"IsApplicable: {c.IsApplicable()}, ApplyInstantlyIfSingle: {c.ApplyInstantlyIfSingle.get_Value()}, ApplyInstantlyOnTap: {c.ApplyInstantlyOnTap.get_Value()}");
			});
			ContextualRadialSettings.ForEach(delegate(ContextualRadialThingSettings c)
			{
				_debug.Add($"Contextual RadialSettings {c.Order} {c.Name} B", () => $"Center: {c.GetCenterThing()?.Name}, CenterBehavior: {c.CenterThingBehavior.get_Value()}, LastUsed: {c.GetLastUsedThing()?.Name}");
			});
			_debug.Add("Applicable Contextual RadialSettings", () => $"Name: {_helper.GetApplicableContextualRadialThingSettings()?.Name}, Things count: {_helper.GetApplicableContextualRadialThingSettings()?.AvailableThings.Count}");
			_debug.Add("Queued for out of combat", () => _helper.GetQueuedThing()?.Name ?? "");
			_debug.Add("TappedModuleKeybind", () => string.Format("{0} {1} {2} {3}", DateTime.Now, tappedModuleKeybind, lastTriggered, lastTriggered.HasValue ? ((object)(int)(DateTime.Now - lastTriggered.Value).TotalMilliseconds) : ""));
			_helper.IsCombatLaunchUnlockedAsync();
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)async delegate
			{
				await _helper.IsCombatLaunchUnlockedAsync();
			});
			DrawUI();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			_helper.UpdatePlayerGlidingOrFalling(gameTime);
			MouseState state = Mouse.GetState();
			if ((int)((MouseState)(ref state)).get_LeftButton() == 1)
			{
				_helper.DoRangedThing();
			}
			bool currentTriggeringState = _settingDefaultMountBinding.get_Value().get_IsTriggering();
			double howLongIsModuleKeybindHeldHown = 0.0;
			if (lastTriggered.HasValue)
			{
				howLongIsModuleKeybindHeldHown = (DateTime.Now - lastTriggered.Value).TotalMilliseconds;
			}
			bool isWithinThreshold = howLongIsModuleKeybindHeldHown <= (double)_settingTapThresholdInMilliseconds.get_Value();
			if (((previousTriggeringState && !currentTriggeringState && isWithinThreshold) || howLongIsModuleKeybindHeldHown > (double)_settingTapThresholdInMilliseconds.get_Value()) && lastTriggered.HasValue)
			{
				tappedModuleKeybind = ((!isWithinThreshold) ? TappedModuleKeybindState.Hold : TappedModuleKeybindState.Tap);
				DoKeybindActionAsync(KeybindTriggerType.Module);
				lastTriggered = null;
			}
			previousTriggeringState = currentTriggeringState;
			bool isThingSwitchable = CanThingBeActivated();
			bool moduleHidden = _lastIsThingSwitchable && !isThingSwitchable;
			bool moduleShown = !_lastIsThingSwitchable && isThingSwitchable;
			int inUseThingsCount = _things.Count((Thing m) => m.IsInUse());
			if (inUseThingsCount == 0 && _lastInUseThingsCount > 0 && !moduleHidden && !moduleShown)
			{
				_helper.ClearSomethingStoredForLaterActivation();
			}
			if (moduleHidden && inUseThingsCount == 1 && _settingMountAutomaticallyAfterLoadingScreen.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				_helper.StoreThingForLaterActivation(_things.Single((Thing m) => m.IsInUse()), "ModuleHidden");
			}
			if (moduleShown && inUseThingsCount == 0 && _helper.IsSomethingStoredForLaterActivation() != null && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				_helper.DoThingActionForLaterActivation();
			}
			_lastInUseThingsCount = inUseThingsCount;
			_lastIsThingSwitchable = isThingSwitchable;
			bool shouldShowModule = ShouldShowModule();
			if (shouldShowModule)
			{
				foreach (DrawIcons drawIcon in _drawIcons)
				{
					((Control)drawIcon).Show();
				}
				InfoPanel drawInfoPanel = _drawInfoPanel;
				if (drawInfoPanel != null)
				{
					((Control)drawInfoPanel).Show();
				}
			}
			else
			{
				foreach (DrawIcons drawIcon2 in _drawIcons)
				{
					((Control)drawIcon2).Hide();
				}
				InfoPanel drawInfoPanel2 = _drawInfoPanel;
				if (drawInfoPanel2 != null)
				{
					((Control)drawInfoPanel2).Hide();
				}
			}
			if ((((Control)_radial).get_Visible() && !_settingDefaultMountBinding.get_Value().get_IsTriggering() && !UserDefinedRadialSettings.Any((UserDefinedRadialThingSettings s) => s.Keybind.get_Value().get_IsTriggering())) || !shouldShowModule)
			{
				((Control)_radial).Hide();
			}
		}

		public static bool ShouldShowModule()
		{
			if (!_settingDisplayModuleOnLoadingScreen.get_Value())
			{
				return CanThingBeActivated();
			}
			return true;
		}

		public static bool CanThingBeActivated()
		{
			return GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
		}

		protected override void Unload()
		{
			_skyLakes?.Clear();
			DebugControl debug = _debug;
			if (debug != null)
			{
				((Control)debug).Dispose();
			}
			foreach (DrawIcons drawIcon in _drawIcons)
			{
				((Control)drawIcon).Dispose();
			}
			DrawRadial radial = _radial;
			if (radial != null)
			{
				((Control)radial).Dispose();
			}
			foreach (Thing thing in _things)
			{
				thing.KeybindingSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
				thing.ImageFileNameSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
				thing.DisposeCornerIcon();
			}
			_settingDisplayMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayLaterActivation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingEnableMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDragInfoPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingInfoPanelLocation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialStartAngle.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialToggleActionCameraKeyBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingMountRadialIconOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingDisplayModuleOnLoadingScreen.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountAutomaticallyAfterLoadingScreen.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDrawIconIds.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)UpdateSettings);
			_settingUserDefinedRadialIds.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)UpdateSettings);
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			DrawUI();
		}

		private void UpdateSettings(object sender = null, EventArgs e = null)
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

		private void UpdateSettings(object sender, ValueChangedEventArgs<List<int>> e)
		{
			DrawUI();
		}

		private void DrawUI()
		{
			foreach (DrawIcons drawIcon in _drawIcons)
			{
				((Control)drawIcon).Dispose();
			}
			_drawIcons = IconThingSettings.Select((IconThingSettings iconSetting) => new DrawIcons(iconSetting, _helper, _textureCache)).ToList();
			InfoPanel drawInfoPanel = _drawInfoPanel;
			if (drawInfoPanel != null)
			{
				((Control)drawInfoPanel).Dispose();
			}
			_drawInfoPanel = new InfoPanel(_textureCache, _helper);
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
				_settingsWindow.set_SelectedTab(((IEnumerable<Tab>)_settingsWindow.get_Tabs()).Skip(1).First());
				((Control)_settingsWindow).Show();
			});
		}

		private async Task DoKeybindActionAsync(KeybindTriggerType caller)
		{
			Logger.Debug("DoKeybindActionAsync entered");
			if (caller == KeybindTriggerType.UserDefined)
			{
				ShowRadial(caller);
				return;
			}
			ContextualRadialThingSettings selectedRadialSettings = _helper.GetApplicableContextualRadialThingSettings();
			Logger.Debug("DoKeybindActionAsync radial applicable settings: " + selectedRadialSettings.Name);
			ICollection<Thing> things = selectedRadialSettings.AvailableThings;
			if (!lastTriggered.HasValue && selectedRadialSettings.IsTapApplicable())
			{
				Logger.Debug("DoKeybindActionAsync radial tap is applicable.");
				lastTriggered = DateTime.Now;
				return;
			}
			Thing tappedThing = things.FirstOrDefault((Thing t) => t.Name == selectedRadialSettings.ApplyInstantlyOnTap.get_Value());
			if (selectedRadialSettings.IsTapApplicable())
			{
				if (tappedModuleKeybind == TappedModuleKeybindState.Tap)
				{
					await (tappedThing?.DoAction(selectedRadialSettings.UnconditionallyDoAction.get_Value(), isActionComingFromMouseActionOnModuleUI: false));
					Logger.Debug("DoKeybindActionAsync not showing radial selected thing (tappedModuleKeybind): " + tappedThing?.Name);
					tappedModuleKeybind = TappedModuleKeybindState.Unknown;
					return;
				}
				if (things.Count() > 1 && tappedThing != null)
				{
					things.Remove(tappedThing);
				}
				tappedModuleKeybind = TappedModuleKeybindState.Unknown;
			}
			if (things.Count() == 1 && selectedRadialSettings.ApplyInstantlyIfSingle.get_Value())
			{
				await (things.FirstOrDefault()?.DoAction(selectedRadialSettings.UnconditionallyDoAction.get_Value(), isActionComingFromMouseActionOnModuleUI: false));
				Logger.Debug("DoKeybindActionAsync not showing radial selected thing (ApplyInstantlyIfSingle): " + things.First().Name);
				return;
			}
			Thing defaultThing = selectedRadialSettings.GetDefaultThing();
			if (defaultThing != null && GameService.Input.get_Mouse().get_CameraDragging())
			{
				await (defaultThing?.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: false) ?? Task.CompletedTask);
				Logger.Debug("DoKeybindActionAsync CameraDragging default");
				return;
			}
			string value = _settingKeybindBehaviour.get_Value();
			if (!(value == "Default"))
			{
				if (value == "Radial")
				{
					ShowRadial(caller);
				}
			}
			else
			{
				await (defaultThing?.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: false) ?? Task.CompletedTask);
				Logger.Debug("DoKeybindActionAsync KeybindBehaviour default");
			}
		}

		private void ShowRadial(KeybindTriggerType caller)
		{
			if (ShouldShowModule())
			{
				DrawRadial radial = _radial;
				if (radial != null)
				{
					((Control)radial).Show();
				}
				Logger.Debug(string.Format("{0} KeybindBehaviour radial, caller {1}", "DoKeybindActionAsync", caller));
			}
		}

		private async Task HandleCombatChangeAsync(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				return;
			}
			Logger.Debug("HandleCombatChangeAsync Trying queued for out of combat");
			if (!_helper.IsPlayerMounted())
			{
				Thing thingInCombat = _helper.GetQueuedThing();
				Logger.Debug("HandleCombatChangeAsync Applied queued for out of combat: " + thingInCombat?.Name);
				await (thingInCombat?.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: false) ?? Task.CompletedTask);
			}
			else
			{
				Logger.Debug("HandleCombatChangeAsync Not applying queued for out of combat: player mounted");
			}
			foreach (Thing thing in _things)
			{
				thing.QueuedTimestamp = null;
			}
		}
	}
}
