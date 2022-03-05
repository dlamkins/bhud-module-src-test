using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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

		public static int[] _mountOrder = new int[11]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10
		};

		public static string[] _mountDisplay = new string[3] { "Transparent", "Solid", "SolidText" };

		public static string[] _mountBehaviour = new string[2] { "DefaultMount", "Radial" };

		public static string[] _mountOrientation = new string[2] { "Horizontal", "Vertical" };

		public static string[] _mountRadialCenterMountBehavior = new string[3] { "None", "Default", "LastUsed" };

		public static SettingEntry<string> _settingDefaultMountChoice;

		public static SettingEntry<string> _settingDefaultWaterMountChoice;

		public static SettingEntry<bool> _settingMountBlockKeybindFromGame;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<bool> _settingDisplayMountQueueing;

		public static SettingEntry<string> _settingDefaultMountBehaviour;

		public static SettingEntry<bool> _settingMountRadialSpawnAtMouse;

		public static SettingEntry<float> _settingMountRadialRadiusModifier;

		public static SettingEntry<float> _settingMountRadialIconSizeModifier;

		public static SettingEntry<float> _settingMountRadialIconOpacity;

		public static SettingEntry<string> _settingMountRadialCenterMountBehavior;

		public static SettingEntry<bool> _settingMountRadialRemoveCenterMount;

		public static SettingEntry<KeyBinding> _settingMountRadialToggleActionCameraKeyBinding;

		public static SettingEntry<string> _settingDisplay;

		public static SettingEntry<bool> _settingDisplayCornerIcons;

		public static SettingEntry<bool> _settingDisplayManualIcons;

		public static SettingEntry<string> _settingOrientation;

		private SettingEntry<Point> _settingLoc;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<int> _settingImgWidth;

		public static SettingEntry<float> _settingOpacity;

		private WindowTab windowTab;

		private Panel _mountPanel;

		private DrawRadial _radial;

		private LoadingSpinner _queueingSpinner;

		private Helper _helper;

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
			_helper = new Helper(ContentsManager);
		}

		protected override void Initialize()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)async delegate(object sender, ValueEventArgs<bool> e)
			{
				await HandleCombatChangeAsync(sender, e);
			});
			windowTab = new WindowTab("Mounts", AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("514394-grey.png")));
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

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Expected O, but got Unknown
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
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
			_settingMountBlockKeybindFromGame = settings.DefineSetting<bool>("MountBlockKeybindFromGame", false, (Func<string>)(() => Strings.Setting_MountBlockKeybindFromGame), (Func<string>)(() => ""));
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Setting_DefaultMountBinding), (Func<string>)(() => ""));
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().set_BlockSequenceFromGw2(true);
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await DoDefaultMountActionAsync();
			});
			_settingDefaultMountChoice = settings.DefineSetting<string>("DefaultMountChoice", "Disabled", (Func<string>)(() => Strings.Setting_DefaultMountChoice), (Func<string>)(() => ""));
			_settingDefaultWaterMountChoice = settings.DefineSetting<string>("DefaultWaterMountChoice", "Disabled", (Func<string>)(() => Strings.Setting_DefaultWaterMountChoice), (Func<string>)(() => ""));
			_settingDefaultMountBehaviour = settings.DefineSetting<string>("DefaultMountBehaviour", "Radial", (Func<string>)(() => Strings.Setting_DefaultMountBehaviour), (Func<string>)(() => ""));
			_settingDisplayMountQueueing = settings.DefineSetting<bool>("DisplayMountQueueing", false, (Func<string>)(() => Strings.Setting_DisplayMountQueueing), (Func<string>)(() => ""));
			_settingMountRadialSpawnAtMouse = settings.DefineSetting<bool>("MountRadialSpawnAtMouse", false, (Func<string>)(() => Strings.Setting_MountRadialSpawnAtMouse), (Func<string>)(() => ""));
			_settingMountRadialIconSizeModifier = settings.DefineSetting<float>("MountRadialIconSizeModifier", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialIconSizeModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconSizeModifier, 0.05f, 1f);
			_settingMountRadialRadiusModifier = settings.DefineSetting<float>("MountRadialRadiusModifier", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialRadiusModifier), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialRadiusModifier, 0.2f, 1f);
			_settingMountRadialIconOpacity = settings.DefineSetting<float>("MountRadialIconOpacity", 0.5f, (Func<string>)(() => Strings.Setting_MountRadialIconOpacity), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_settingMountRadialIconOpacity, 0.05f, 1f);
			_settingMountRadialCenterMountBehavior = settings.DefineSetting<string>("MountRadialCenterMountBehavior", "Default", (Func<string>)(() => Strings.Setting_MountRadialCenterMountBehavior), (Func<string>)(() => ""));
			_settingMountRadialRemoveCenterMount = settings.DefineSetting<bool>("MountRadialRemoveCenterMount", true, (Func<string>)(() => Strings.Setting_MountRadialRemoveCenterMount), (Func<string>)(() => ""));
			_settingMountRadialToggleActionCameraKeyBinding = settings.DefineSetting<KeyBinding>("MountRadialToggleActionCameraKeyBinding", new KeyBinding((Keys)121), (Func<string>)(() => Strings.Setting_MountRadialToggleActionCameraKeyBinding), (Func<string>)(() => ""));
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
			foreach (Mount mount in _mounts)
			{
				mount.OrderSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
				mount.KeybindingSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			}
			_settingDefaultMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
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
			return (IView)(object)new DummySettingsView(ContentsManager);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DrawUI();
			GameService.Overlay.get_BlishHudWindow().AddTab(windowTab, (Func<IView>)(() => (IView)(object)new SettingsView(ContentsManager)));
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (_mountPanel != null)
			{
				if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen())
				{
					((Control)_mountPanel).Show();
				}
				else
				{
					((Control)_mountPanel).Hide();
				}
				if (_dragging)
				{
					Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragStart;
					Panel mountPanel = _mountPanel;
					((Control)mountPanel).set_Location(((Control)mountPanel).get_Location() + nOffset);
					_dragStart = GameService.Input.get_Mouse().get_Position();
				}
			}
			if (_settingDisplayMountQueueing.get_Value() && _mounts.Any((Mount m) => m.QueuedTimestamp.HasValue))
			{
				LoadingSpinner queueingSpinner = _queueingSpinner;
				if (queueingSpinner != null)
				{
					((Control)queueingSpinner).Show();
				}
			}
			if (((Control)_radial).get_Visible() && !_settingDefaultMountBinding.get_Value().get_IsTriggering())
			{
				((Control)_radial).Hide();
			}
		}

		protected override void Unload()
		{
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
				mount.DisposeCornerIcon();
			}
			_settingDefaultMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingMountRadialIconOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRemoveCenterMount.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayCornerIcons.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDisplayManualIcons.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingOrientation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingImgWidth.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			GameService.Overlay.get_BlishHudWindow().RemoveTab(windowTab);
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
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
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
				Texture2D img = _helper.GetImgFile(mount.ImageFileName);
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)_mountPanel);
				val2.set_Texture(AsyncTexture2D.op_Implicit(img));
				((Control)val2).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
				((Control)val2).set_Location(new Point(curX, curY));
				((Control)val2).set_Opacity(_settingOpacity.get_Value());
				((Control)val2).set_BasicTooltipText(mount.DisplayName);
				((Control)val2).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)async delegate
				{
					await mount.DoHotKey();
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
				mount.CreateCornerIcon(_helper.GetImgFile(mount.ImageFileName));
			}
		}

		private void DrawUI()
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			Panel mountPanel = _mountPanel;
			if (mountPanel != null)
			{
				((Control)mountPanel).Dispose();
			}
			foreach (Mount mount in _mounts)
			{
				mount.DisposeCornerIcon();
			}
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
			DrawRadial radial = _radial;
			if (radial != null)
			{
				((Control)radial).Dispose();
			}
			_radial = new DrawRadial(_helper);
			((Control)_radial).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		private async Task DoDefaultMountActionAsync()
		{
			Logger.Debug("DoDefaultMountActionAsync entered");
			if ((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() != 0)
			{
				await (_availableOrderedMounts.FirstOrDefault()?.DoHotKey() ?? Task.CompletedTask);
				Logger.Debug("DoDefaultMountActionAsync dismounted");
				return;
			}
			Mount instantMount = _helper.GetInstantMount();
			if (instantMount != null)
			{
				await instantMount.DoHotKey();
				Logger.Debug("DoDefaultMountActionAsync instantmount");
				return;
			}
			string value = _settingDefaultMountBehaviour.get_Value();
			if (!(value == "DefaultMount"))
			{
				if (value == "Radial")
				{
					((Control)_radial).Show();
					Logger.Debug("DoDefaultMountActionAsync radial");
				}
			}
			else
			{
				await (_helper.GetDefaultMount()?.DoHotKey() ?? Task.CompletedTask);
				Logger.Debug("DoDefaultMountActionAsync defaultmount");
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
				select m).FirstOrDefault()?.DoHotKey() ?? Task.CompletedTask);
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
