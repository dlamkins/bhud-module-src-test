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

		public static SettingEntry<int> _settingsLastRunMigrationVersion;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<bool> _settingDisplayMountQueueing;

		public static SettingEntry<bool> _settingEnableMountQueueing;

		public static SettingEntry<Point> _settingDisplayMountQueueingLocation;

		public static SettingEntry<bool> _settingDragMountQueueing;

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

		private TabbedWindow2 _settingsWindow;

		public static DebugControl _debug;

		private DrawRadial _radial;

		private ICollection<DrawIcons> _drawIcons = new List<DrawIcons>();

		private DrawOutOfCombat _drawOutOfCombat;

		private DrawMouseCursor _drawMouseCursor;

		private Helper _helper;

		private TextureCache _textureCache;

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
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Expected O, but got Unknown
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Expected O, but got Unknown
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Expected O, but got Unknown
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Expected O, but got Unknown
			List<string> obj = new List<string>
			{
				"griffon-text.png", "griffon-trans.png", "griffon.png", "jackal-text.png", "jackal-trans.png", "jackal.png", "raptor-text.png", "raptor-trans.png", "raptor.png", "roller-text.png",
				"roller-trans.png", "roller.png", "skimmer-text.png", "skimmer-trans.png", "skimmer.png", "skyscale-text.png", "skyscale-trans.png", "skyscale.png", "springer-text.png", "springer-trans.png",
				"springer.png", "turtle-text.png", "turtle-trans.png", "turtle.png", "warclaw-text.png", "warclaw-trans.png", "warclaw.png", "fishing.png", "skiff.png", "jadebotwaypoint.png",
				"chair.png", "music.png", "held.png", "toy.png", "tonic.png", "scanforrift.png", "skyscaleleap.png", "unmount.png"
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
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Expected O, but got Unknown
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Expected O, but got Unknown
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
				new Chair(settings, _helper),
				new Music(settings, _helper),
				new Held(settings, _helper),
				new Toy(settings, _helper),
				new Tonic(settings, _helper)
			};
			_things = new Collection<Thing>(obj);
			List<Thing> thingsForMigration = obj.ToList();
			_settingsLastRunMigrationVersion = settings.DefineSetting<int>("LastRunMigrationVersion", 0, (Func<string>)null, (Func<string>)null);
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Setting_DefaultMountBinding), (Func<string>)(() => ""));
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await DoKeybindActionAsync();
			});
			_settingDefaultMountBinding.get_Value().add_BindingChanged((EventHandler<EventArgs>)UpdateSettings);
			_settingDefaultMountBehaviour = settings.DefineSetting<string>("DefaultMountBehaviour", "Radial", (Func<string>)null, (Func<string>)null);
			_settingKeybindBehaviour = settings.DefineSetting<string>("KeybindBehaviour", "Radial", (Func<string>)null, (Func<string>)null);
			_settingDisplayMountQueueing = settings.DefineSetting<bool>("DisplayMountQueueing", false, (Func<string>)null, (Func<string>)null);
			_settingEnableMountQueueing = settings.DefineSetting<bool>("EnableMountQueueing", false, (Func<string>)null, (Func<string>)null);
			_settingDragMountQueueing = settings.DefineSetting<bool>("DragMountQueueing", false, (Func<string>)null, (Func<string>)null);
			_settingCombatLaunchMasteryUnlocked = settings.DefineSetting<bool>("CombatLaunchMasteryUnlocked", false, (Func<string>)null, (Func<string>)null);
			_settingDisplayMountQueueingLocation = settings.DefineSetting<Point>("DisplayMountQueueingLocation", new Point(200, 200), (Func<string>)null, (Func<string>)null);
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
			ContextualRadialSettings = new List<ContextualRadialThingSettings>
			{
				new ContextualRadialThingSettings(settings, "IsPlayerMounted", 0, _helper.IsPlayerMounted, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: true, _things.Where((Thing t) => t is UnMount).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerInWvwMap", 1, _helper.IsPlayerInWvwMap, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: true, _things.Where((Thing t) => t is Warclaw).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerInCombat", 2, _helper.IsPlayerInCombat, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: true, _things.Where((Thing t) => t is Skyscale).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerUnderWater", 3, _helper.IsPlayerUnderWater, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: false, _things.Where((Thing t) => t is Skimmer || t is SiegeTurtle).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerOnWaterSurface", 4, _helper.IsPlayerOnWaterSurface, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: true, _things.Where((Thing t) => t is Skiff).ToList()),
				new ContextualRadialThingSettings(settings, "IsPlayerGlidingOrFalling", 5, _helper.IsPlayerGlidingOrFalling, defaultIsEnabled: false, defaultApplyInstantlyIfSingle: false, _things.Where((Thing t) => t is Griffon || t is Skyscale).ToList()),
				new ContextualRadialThingSettings(settings, "Default", 99, () => true, defaultIsEnabled: true, defaultApplyInstantlyIfSingle: false, thingsForMigration)
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
			foreach (Thing thing in _things)
			{
				thing.KeybindingSetting.get_Value().add_BindingChanged((EventHandler<EventArgs>)UpdateSettings);
				thing.ImageFileNameSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			}
			_settingDisplayModuleOnLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountAutomaticallyAfterLoadingScreen.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingEnableMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDragMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayMountQueueingLocation.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
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
			_debug.Add("GetTriggeredRadialSettings Name", () => _helper.GetTriggeredRadialSettings()?.Name ?? "");
			ContextualRadialSettings.ForEach(delegate(ContextualRadialThingSettings c)
			{
				_debug.Add($"Contextual RadialSettings {c.Order} {c.Name}", () => $"IsApplicable: {c.IsApplicable()}, Center: {c.GetCenterThing()?.Name}, CenterBehavior: {c.CenterThingBehavior.get_Value()}, ApplyInstantlyIfSingle: {c.ApplyInstantlyIfSingle.get_Value()}, LastUsed: {c.GetLastUsedThing()?.Name}");
			});
			_debug.Add("Applicable Contextual RadialSettings Name", () => _helper.GetApplicableContextualRadialThingSettings()?.Name ?? "");
			_debug.Add("Applicable Contextual RadialSettings Things", () => string.Join(", ", _helper.GetApplicableContextualRadialThingSettings()?.AvailableThings.Select((Thing t) => t.Name)) ?? "");
			_debug.Add("Queued for out of combat", () => _helper.GetQueuedThing()?.Name ?? "");
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)async delegate
			{
				await _helper.IsCombatLaunchUnlockedAsync();
			});
			DrawUI();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			_helper.UpdatePlayerGlidingOrFalling(gameTime);
			bool isThingSwitchable = CanThingBeActivated();
			bool moduleHidden = _lastIsThingSwitchable && !isThingSwitchable;
			bool moduleShown = !_lastIsThingSwitchable && isThingSwitchable;
			string currentCharacterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			int inUseThingsCount = _things.Count((Thing m) => m.IsInUse());
			if (inUseThingsCount == 0 && _lastInUseThingsCount > 0 && !moduleHidden && !moduleShown)
			{
				_helper.ClearSomethingStoredForLaterActivation(currentCharacterName);
			}
			if (moduleHidden && inUseThingsCount == 1 && _settingMountAutomaticallyAfterLoadingScreen.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				_helper.StoreThingForLaterActivation(_things.Single((Thing m) => m.IsInUse()), currentCharacterName, "ModuleHidden");
			}
			if (moduleShown && inUseThingsCount == 0 && _helper.IsSomethingStoredForLaterActivation(currentCharacterName) && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				_helper.DoThingActionForLaterActivation(currentCharacterName);
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
			}
			else
			{
				foreach (DrawIcons drawIcon2 in _drawIcons)
				{
					((Control)drawIcon2).Hide();
				}
			}
			if (_things.Any((Thing m) => m.QueuedTimestamp.HasValue) || _settingDragMountQueueing.get_Value())
			{
				_drawOutOfCombat?.ShowSpinner();
			}
			else
			{
				_drawOutOfCombat?.HideSpinner();
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
			_settingEnableMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDragMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayMountQueueingLocation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
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
			DrawOutOfCombat drawOutOfCombat = _drawOutOfCombat;
			if (drawOutOfCombat != null)
			{
				((Control)drawOutOfCombat).Dispose();
			}
			_drawOutOfCombat = new DrawOutOfCombat();
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

		private async Task DoKeybindActionAsync()
		{
			Logger.Debug("DoKeybindActionAsync entered");
			ContextualRadialThingSettings selectedRadialSettings = _helper.GetApplicableContextualRadialThingSettings();
			Logger.Debug("DoKeybindActionAsync radial applicable settings: " + selectedRadialSettings.Name);
			ICollection<Thing> things = selectedRadialSettings.AvailableThings;
			if (things.Count() == 1 && selectedRadialSettings.ApplyInstantlyIfSingle.get_Value())
			{
				await (things.FirstOrDefault()?.DoAction());
				Logger.Debug("DoKeybindActionAsync not showing radial selected thing: " + selectedRadialSettings.Things.First().Name);
				return;
			}
			Thing defaultThing = selectedRadialSettings.GetDefaultThing();
			if (defaultThing != null && GameService.Input.get_Mouse().get_CameraDragging())
			{
				await (defaultThing?.DoAction() ?? Task.CompletedTask);
				Logger.Debug("DoKeybindActionAsync CameraDragging default");
				return;
			}
			string value = _settingKeybindBehaviour.get_Value();
			if (!(value == "Default"))
			{
				if (value == "Radial" && ShouldShowModule())
				{
					DrawRadial radial = _radial;
					if (radial != null)
					{
						((Control)radial).Show();
					}
					Logger.Debug("DoKeybindActionAsync KeybindBehaviour radial");
				}
			}
			else
			{
				await (defaultThing?.DoAction() ?? Task.CompletedTask);
				Logger.Debug("DoKeybindActionAsync KeybindBehaviour default");
			}
		}

		private async Task HandleCombatChangeAsync(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				return;
			}
			if (_helper.IsPlayerMounted())
			{
				Thing thingInCombat = _helper.GetQueuedThing();
				Logger.Debug("HandleCombatChangeAsync Applied queued for out of combat: " + thingInCombat?.Name);
				await (thingInCombat?.DoAction() ?? Task.CompletedTask);
			}
			foreach (Thing thing in _things)
			{
				thing.QueuedTimestamp = null;
			}
		}
	}
}
