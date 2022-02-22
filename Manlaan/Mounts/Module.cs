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

		public static SettingEntry<KeyBinding> InputQueuingKeybindSetting = null;

		public static SettingEntry<string> _settingDefaultMountChoice;

		public static SettingEntry<string> _settingDefaultWaterMountChoice;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<bool> _settingDisplayMountQueueing;

		public static SettingEntry<bool> _settingDefaultMountUseRadial;

		public static SettingEntry<string> _settingDefaultMountBehaviour;

		public static SettingEntry<bool> _settingMountRadialSpawnAtMouse;

		public static SettingEntry<float> _settingMountRadialRadiusModifier;

		public static SettingEntry<float> _settingMountRadialIconSizeModifier;

		public static SettingEntry<string> _settingMountRadialCenterMountBehavior;

		public static SettingEntry<string> _settingDisplay;

		public static SettingEntry<bool> _settingDisplayCornerIcons;

		public static SettingEntry<bool> _settingDisplayManualIcons;

		public static SettingEntry<string> _settingOrientation;

		private SettingEntry<Point> _settingLoc;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<int> _settingImgWidth;

		public static SettingEntry<float> _settingOpacity;

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
			where m.OrderSetting.get_Value() != 0
			orderby m.OrderSetting.get_Value()
			select m).ToList();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Initialize()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> e)
			{
				HandleCombatChange(sender, e);
			});
			GameService.Input.get_Keyboard().add_KeyStateChanged((EventHandler<KeyboardEventArgs>)delegate(object sender, KeyboardEventArgs e)
			{
				HandleKeyBoardKeyChange(sender, e);
			});
			_helper = new Helper(ContentsManager);
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

		private void MigrateDefaultMountBehaviour()
		{
			if (_settingDefaultMountBehaviour.get_Value().Equals(""))
			{
				_settingDefaultMountBehaviour.set_Value(_settingDefaultMountUseRadial.get_Value() ? "Radial" : "DefaultMount");
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			_mounts = new Collection<Mount>
			{
				new Raptor(settings),
				new Springer(settings),
				new Skimmer(settings),
				new Jackal(settings),
				new Griffon(settings),
				new RollerBeetle(settings),
				new Warclaw(settings),
				new Skyscale(settings),
				new SiegeTurtle(settings)
			};
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), "Default Mount Binding", "", (SettingTypeRendererDelegate)null);
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DoDefaultMountAction();
			});
			_settingDefaultMountChoice = settings.DefineSetting<string>("DefaultMountChoice", "Disabled", "Default Mount Choice", "", (SettingTypeRendererDelegate)null);
			_settingDefaultWaterMountChoice = settings.DefineSetting<string>("DefaultWaterMountChoice", "Disabled", "Default Water Mount Choice", "", (SettingTypeRendererDelegate)null);
			_settingDisplayMountQueueing = settings.DefineSetting<bool>("DisplayMountQueueing", false, "Display Mount queuing", "", (SettingTypeRendererDelegate)null);
			_settingDefaultMountUseRadial = settings.DefineSetting<bool>("DefaultMountUseRadial", false, "Default Mount uses radial", "", (SettingTypeRendererDelegate)null);
			_settingDefaultMountBehaviour = settings.DefineSetting<string>("DefaultMountBehaviour", "", "Default Mount button behaviour", "", (SettingTypeRendererDelegate)null);
			_settingMountRadialSpawnAtMouse = settings.DefineSetting<bool>("MountRadialSpawnAtMouse", false, "Radial spawn at mouse", "", (SettingTypeRendererDelegate)null);
			_settingMountRadialIconSizeModifier = settings.DefineSetting<float>("MountRadialIconSizeModifier", 1f, "Radial Icon Size", "", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(_settingMountRadialIconSizeModifier, 0.05f, 1f);
			_settingMountRadialRadiusModifier = settings.DefineSetting<float>("MountRadialRadiusModifier", 1f, "Radial radius", "", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(_settingMountRadialRadiusModifier, 0.2f, 1f);
			_settingMountRadialCenterMountBehavior = settings.DefineSetting<string>("MountRadialCenterMountBehavior", "Default", "Determines the mount in the center of the radial.", "", (SettingTypeRendererDelegate)null);
			_settingDisplay = settings.DefineSetting<string>("MountDisplay", "Transparent", "Display Type", "", (SettingTypeRendererDelegate)null);
			_settingDisplayCornerIcons = settings.DefineSetting<bool>("MountDisplayCornerIcons", false, "Display corner icons", "", (SettingTypeRendererDelegate)null);
			_settingDisplayManualIcons = settings.DefineSetting<bool>("MountDisplayManualIcons", false, "Display manual icons", "", (SettingTypeRendererDelegate)null);
			_settingOrientation = settings.DefineSetting<string>("Orientation", "Horizontal", "Manual Orientation", "", (SettingTypeRendererDelegate)null);
			_settingLoc = settings.DefineSetting<Point>("MountLoc", new Point(100, 100), "Window Location", "", (SettingTypeRendererDelegate)null);
			_settingDrag = settings.DefineSetting<bool>("MountDrag", false, "Enable Dragging (White Box)", "", (SettingTypeRendererDelegate)null);
			_settingImgWidth = settings.DefineSetting<int>("MountImgWidth", 50, "Manual Icon Width", "", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(_settingImgWidth, 0, 200);
			_settingOpacity = settings.DefineSetting<float>("MountOpacity", 1f, "Manual Opacity", "", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(_settingOpacity, 0f, 1f);
			MigrateDisplaySettings();
			MigrateDefaultMountBehaviour();
			foreach (Mount i in _mounts)
			{
				i.OrderSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
				i.KeybindingSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			}
			_settingDefaultMountBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDefaultMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayMountQueueing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDefaultMountUseRadial.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDefaultMountBehaviour.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
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

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DrawUI();
			GameService.Overlay.get_BlishHudWindow().AddTab("Mounts", AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("514394-grey.png")), (Func<IView>)(() => (IView)(object)new SettingsView()), 0);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
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
			foreach (Mount i in _mounts)
			{
				i.OrderSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
				i.KeybindingSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
				i.DisposeCornerIcon();
			}
			_settingDefaultMountBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDefaultMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDefaultWaterMountChoice.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingDisplayMountQueueing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDefaultMountUseRadial.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingDefaultMountBehaviour.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingMountRadialSpawnAtMouse.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingMountRadialIconSizeModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialRadiusModifier.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
			_settingMountRadialCenterMountBehavior.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
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
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
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
				Image _btnMount = val2;
				((Control)_btnMount).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					mount.DoHotKey();
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
				Panel dragBox = val3;
				((Control)dragBox).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					_dragging = true;
					_dragStart = GameService.Input.get_Mouse().get_Position();
				});
				((Control)dragBox).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
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
				if (mount.OrderSetting.get_Value() != 0)
				{
					mount.CreateCornerIcon(_helper.GetImgFile(mount.ImageFileName));
				}
			}
		}

		private void DrawUI()
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
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

		private void HandleKeyBoardKeyChange(object sender, KeyboardEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Invalid comparison between Unknown and I4
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			Keys key = _settingDefaultMountBinding.get_Value().get_PrimaryKey();
			if (_settingDefaultMountBehaviour.get_Value() == "Radial" && e.get_Key() == key)
			{
				if ((int)e.get_EventType() == 256)
				{
					_radial.Start();
				}
				else if ((int)e.get_EventType() == 257)
				{
					_radial.Stop();
				}
			}
		}

		private void DoDefaultMountAction()
		{
			if (_settingDefaultMountBehaviour.get_Value() == "DefaultMount")
			{
				_helper.GetDefaultMount()?.DoHotKey();
			}
		}

		private void HandleCombatChange(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				return;
			}
			(from m in _mounts
				where m.QueuedTimestamp.HasValue
				orderby m.QueuedTimestamp descending
				select m).FirstOrDefault()?.DoHotKey();
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
