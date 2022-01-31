using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
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

		public static int[] _mountOrder = new int[11]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10
		};

		public static string[] _mountDisplay = new string[5] { "Transparent Corner", "Solid Corner", "Transparent Manual", "Solid Manual", "Solid Manual Text" };

		public static string[] _mountOrientation = new string[2] { "Horizontal", "Vertical" };

		private SettingEntry<KeyBinding> InputQueuingKeybindSetting = null;

		public static string[] _defaultMountChoices = new string[9] { "Disabled", "Raptor", "Springer", "Skimmer", "Jackal", "Griffon", "Roller Beetle", "Skyscale", "Warclaw" };

		private Dictionary<string, Action> defaultMountChoicesToActions;

		private MapType[] warclawOnlyMaps;

		public static SettingEntry<int> _settingGriffonOrder;

		public static SettingEntry<int> _settingJackalOrder;

		public static SettingEntry<int> _settingRaptorOrder;

		public static SettingEntry<int> _settingRollerOrder;

		public static SettingEntry<int> _settingSkimmerOrder;

		public static SettingEntry<int> _settingSkyscaleOrder;

		public static SettingEntry<int> _settingSpringerOrder;

		public static SettingEntry<int> _settingWarclawOrder;

		public static SettingEntry<string> _settingDefaultMountChoice;

		public static SettingEntry<KeyBinding> _settingGriffonBinding;

		public static SettingEntry<KeyBinding> _settingJackalBinding;

		public static SettingEntry<KeyBinding> _settingRaptorBinding;

		public static SettingEntry<KeyBinding> _settingRollerBinding;

		public static SettingEntry<KeyBinding> _settingSkimmerBinding;

		public static SettingEntry<KeyBinding> _settingSkyscaleBinding;

		public static SettingEntry<KeyBinding> _settingSpringerBinding;

		public static SettingEntry<KeyBinding> _settingWarclawBinding;

		public static SettingEntry<KeyBinding> _settingDefaultMountBinding;

		public static SettingEntry<string> _settingDisplay;

		public static SettingEntry<string> _settingOrientation;

		private SettingEntry<Point> _settingLoc;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<int> _settingImgWidth;

		public static SettingEntry<float> _settingOpacity;

		private Panel _mountPanel;

		private CornerIcon _cornerGriffon;

		private CornerIcon _cornerJackal;

		private CornerIcon _cornerRaptor;

		private CornerIcon _cornerRoller;

		private CornerIcon _cornerSkimmer;

		private CornerIcon _cornerSkyscale;

		private CornerIcon _cornerSpringer;

		private CornerIcon _cornerWarclaw;

		private bool _dragging;

		private Point _dragStart;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			MapType[] array = new MapType[8];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			warclawOnlyMaps = (MapType[])(object)array;
			_dragStart = Point.get_Zero();
			((Module)this)._002Ector(moduleParameters);
		}

		protected override void Initialize()
		{
			InitializeDefaultMountActions();
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> e)
			{
				HandleCombatChange(sender, e);
			});
		}

		private void InitializeDefaultMountActions()
		{
			defaultMountChoicesToActions = new Dictionary<string, Action>();
			Action[] defaultMountActions = new Action[9]
			{
				delegate
				{
				},
				delegate
				{
					DoHotKey(_settingRaptorBinding);
				},
				delegate
				{
					DoHotKey(_settingSpringerBinding);
				},
				delegate
				{
					DoHotKey(_settingSkimmerBinding);
				},
				delegate
				{
					DoHotKey(_settingJackalBinding);
				},
				delegate
				{
					DoHotKey(_settingGriffonBinding);
				},
				delegate
				{
					DoHotKey(_settingRollerBinding);
				},
				delegate
				{
					DoHotKey(_settingSkyscaleBinding);
				},
				delegate
				{
					DoHotKey(_settingWarclawBinding);
				}
			};
			for (int index = 0; index < defaultMountActions.Length; index++)
			{
				defaultMountChoicesToActions.Add(_defaultMountChoices[index], defaultMountActions[index]);
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected O, but got Unknown
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Expected O, but got Unknown
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			_settingRaptorOrder = settings.DefineSetting<int>("MountRaptorOrder2", 1, "Raptor Order", "", (SettingTypeRendererDelegate)null);
			_settingSpringerOrder = settings.DefineSetting<int>("MountSpringerOrder2", 2, "Springer Order", "", (SettingTypeRendererDelegate)null);
			_settingSkimmerOrder = settings.DefineSetting<int>("MountSkimmerOrder2", 3, "Skimmer Order", "", (SettingTypeRendererDelegate)null);
			_settingJackalOrder = settings.DefineSetting<int>("MountJackalOrder2", 4, "Jackal Order", "", (SettingTypeRendererDelegate)null);
			_settingGriffonOrder = settings.DefineSetting<int>("MountGriffonOrder2", 5, "Griffon Order", "", (SettingTypeRendererDelegate)null);
			_settingRollerOrder = settings.DefineSetting<int>("MountRollerOrder2", 6, "Roller Order", "", (SettingTypeRendererDelegate)null);
			_settingWarclawOrder = settings.DefineSetting<int>("MountWarclawOrder2", 7, "Warclaw Order", "", (SettingTypeRendererDelegate)null);
			_settingSkyscaleOrder = settings.DefineSetting<int>("MountSkyscaleOrder2", 8, "Skyscale Order", "", (SettingTypeRendererDelegate)null);
			_settingDefaultMountChoice = settings.DefineSetting<string>("DefaultMountChoice", "Disabled", "Default Mount Choice", "", (SettingTypeRendererDelegate)null);
			_settingRaptorBinding = settings.DefineSetting<KeyBinding>("MountRaptorBinding", new KeyBinding((Keys)0), "Raptor Binding", "", (SettingTypeRendererDelegate)null);
			_settingSpringerBinding = settings.DefineSetting<KeyBinding>("MountSpringerBinding", new KeyBinding((Keys)0), "Springer Binding", "", (SettingTypeRendererDelegate)null);
			_settingSkimmerBinding = settings.DefineSetting<KeyBinding>("MountSkimmerBinding", new KeyBinding((Keys)0), "Skimmer Binding", "", (SettingTypeRendererDelegate)null);
			_settingJackalBinding = settings.DefineSetting<KeyBinding>("MountJackalBinding", new KeyBinding((Keys)0), "Jackal Binding", "", (SettingTypeRendererDelegate)null);
			_settingGriffonBinding = settings.DefineSetting<KeyBinding>("MountGriffonBinding", new KeyBinding((Keys)0), "Griffon Binding", "", (SettingTypeRendererDelegate)null);
			_settingRollerBinding = settings.DefineSetting<KeyBinding>("MountRollerBinding", new KeyBinding((Keys)0), "Roller Binding", "", (SettingTypeRendererDelegate)null);
			_settingWarclawBinding = settings.DefineSetting<KeyBinding>("MountWarclawBinding", new KeyBinding((Keys)0), "Warclaw Binding", "", (SettingTypeRendererDelegate)null);
			_settingSkyscaleBinding = settings.DefineSetting<KeyBinding>("MountSkyscaleBinding", new KeyBinding((Keys)0), "Skyscale Binding", "", (SettingTypeRendererDelegate)null);
			_settingDefaultMountBinding = settings.DefineSetting<KeyBinding>("DefaultMountBinding", new KeyBinding((Keys)0), "Default Mount Binding", "", (SettingTypeRendererDelegate)null);
			_settingDisplay = settings.DefineSetting<string>("MountDisplay", "Transparent Corner", "Display Type", "", (SettingTypeRendererDelegate)null);
			_settingOrientation = settings.DefineSetting<string>("Orientation", "Horizontal", "Manual Orientation", "", (SettingTypeRendererDelegate)null);
			_settingLoc = settings.DefineSetting<Point>("MountLoc", new Point(100, 100), "Window Location", "", (SettingTypeRendererDelegate)null);
			_settingDrag = settings.DefineSetting<bool>("MountDrag", false, "Enable Dragging (White Box)", "", (SettingTypeRendererDelegate)null);
			_settingImgWidth = settings.DefineSetting<int>("MountImgWidth", 50, "Manual Icon Width", "", (SettingTypeRendererDelegate)null);
			_settingOpacity = settings.DefineSetting<float>("MountOpacity", 1f, "Manual Opacity", "", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(_settingImgWidth, 0, 200);
			SettingComplianceExtensions.SetRange(_settingOpacity, 0f, 1f);
			_settingGriffonOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingJackalOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingRaptorOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingRollerOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSkimmerOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSkyscaleOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSpringerOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingWarclawOrder.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingGriffonBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingJackalBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingRaptorBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingRollerBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSkimmerBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSkyscaleBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSpringerBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingWarclawBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDefaultMountBinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDefaultMountBinding.get_Value().set_Enabled(true);
			_settingDefaultMountBinding.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DoDefaultMountAction();
			});
			_settingDisplay.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingOrientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingImgWidth.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DrawIcons();
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
		}

		protected override void Unload()
		{
			CornerIcon cornerGriffon = _cornerGriffon;
			if (cornerGriffon != null)
			{
				((Control)cornerGriffon).Dispose();
			}
			CornerIcon cornerJackal = _cornerJackal;
			if (cornerJackal != null)
			{
				((Control)cornerJackal).Dispose();
			}
			CornerIcon cornerRaptor = _cornerRaptor;
			if (cornerRaptor != null)
			{
				((Control)cornerRaptor).Dispose();
			}
			CornerIcon cornerRoller = _cornerRoller;
			if (cornerRoller != null)
			{
				((Control)cornerRoller).Dispose();
			}
			CornerIcon cornerSkimmer = _cornerSkimmer;
			if (cornerSkimmer != null)
			{
				((Control)cornerSkimmer).Dispose();
			}
			CornerIcon cornerSkyscale = _cornerSkyscale;
			if (cornerSkyscale != null)
			{
				((Control)cornerSkyscale).Dispose();
			}
			CornerIcon cornerSpringer = _cornerSpringer;
			if (cornerSpringer != null)
			{
				((Control)cornerSpringer).Dispose();
			}
			CornerIcon cornerWarclaw = _cornerWarclaw;
			if (cornerWarclaw != null)
			{
				((Control)cornerWarclaw).Dispose();
			}
			Panel mountPanel = _mountPanel;
			if (mountPanel != null)
			{
				((Control)mountPanel).Dispose();
			}
			_settingGriffonOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingJackalOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingRaptorOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingRollerOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSkimmerOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSkyscaleOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingSpringerOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingWarclawOrder.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingGriffonBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingJackalBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingRaptorBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingRollerBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSkimmerBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSkyscaleBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingSpringerBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingWarclawBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDefaultMountBinding.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)UpdateSettings);
			_settingDisplay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingOrientation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings);
			_settingLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateSettings);
			_settingDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings);
			_settingImgWidth.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateSettings);
			_settingOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings);
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			DrawIcons();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<KeyBinding> e = null)
		{
			DrawIcons();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			DrawIcons();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			DrawIcons();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<float> e = null)
		{
			DrawIcons();
		}

		private void UpdateSettings(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			DrawIcons();
		}

		public void DrawManualIcons()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Expected O, but got Unknown
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Expected O, but got Unknown
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Expected O, but got Unknown
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Expected O, but got Unknown
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Expected O, but got Unknown
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Expected O, but got Unknown
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_070b: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0719: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Expected O, but got Unknown
			//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Expected O, but got Unknown
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Unknown result type (might be due to invalid IL or missing references)
			int curX = 0;
			int curY = 0;
			int totalMounts = 0;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(_settingLoc.get_Value());
			((Control)val).set_Size(new Point(_settingImgWidth.get_Value() * 8, _settingImgWidth.get_Value() * 8));
			_mountPanel = val;
			int[] mountOrder = _mountOrder;
			foreach (int i in mountOrder)
			{
				if (i == 0)
				{
					continue;
				}
				if (_settingGriffonOrder.get_Value() == i)
				{
					Texture2D img8 = GetImgFile("griffon");
					Image val2 = new Image();
					((Control)val2).set_Parent((Container)(object)_mountPanel);
					val2.set_Texture(AsyncTexture2D.op_Implicit(img8));
					((Control)val2).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val2).set_Location(new Point(curX, curY));
					((Control)val2).set_Opacity(_settingOpacity.get_Value());
					((Control)val2).set_BasicTooltipText("Griffon");
					Image _btnGriffon = val2;
					((Control)_btnGriffon).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingGriffonBinding);
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
				if (_settingJackalOrder.get_Value() == i)
				{
					Texture2D img7 = GetImgFile("jackal");
					Image val3 = new Image();
					((Control)val3).set_Parent((Container)(object)_mountPanel);
					val3.set_Texture(AsyncTexture2D.op_Implicit(img7));
					((Control)val3).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val3).set_Location(new Point(curX, curY));
					((Control)val3).set_Opacity(_settingOpacity.get_Value());
					((Control)val3).set_BasicTooltipText("Jackal");
					Image _btnJackal = val3;
					((Control)_btnJackal).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingJackalBinding);
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
				if (_settingRaptorOrder.get_Value() == i)
				{
					Texture2D img6 = GetImgFile("raptor");
					Image val4 = new Image();
					((Control)val4).set_Parent((Container)(object)_mountPanel);
					val4.set_Texture(AsyncTexture2D.op_Implicit(img6));
					((Control)val4).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val4).set_Location(new Point(curX, curY));
					((Control)val4).set_Opacity(_settingOpacity.get_Value());
					((Control)val4).set_BasicTooltipText("Raptor");
					Image _btnRaptor = val4;
					((Control)_btnRaptor).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingRaptorBinding);
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
				if (_settingRollerOrder.get_Value() == i)
				{
					Texture2D img5 = GetImgFile("roller");
					Image val5 = new Image();
					((Control)val5).set_Parent((Container)(object)_mountPanel);
					val5.set_Texture(AsyncTexture2D.op_Implicit(img5));
					((Control)val5).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val5).set_Location(new Point(curX, curY));
					((Control)val5).set_Opacity(_settingOpacity.get_Value());
					((Control)val5).set_BasicTooltipText("Roller");
					Image _btnRoller = val5;
					((Control)_btnRoller).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingRollerBinding);
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
				if (_settingSkimmerOrder.get_Value() == i)
				{
					Texture2D img4 = GetImgFile("skimmer");
					Image val6 = new Image();
					((Control)val6).set_Parent((Container)(object)_mountPanel);
					val6.set_Texture(AsyncTexture2D.op_Implicit(img4));
					((Control)val6).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val6).set_Location(new Point(curX, curY));
					((Control)val6).set_Opacity(_settingOpacity.get_Value());
					((Control)val6).set_BasicTooltipText("Skimmer");
					Image _btnSkimmer = val6;
					((Control)_btnSkimmer).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSkimmerBinding);
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
				if (_settingSkyscaleOrder.get_Value() == i)
				{
					Texture2D img3 = GetImgFile("skyscale");
					Image val7 = new Image();
					((Control)val7).set_Parent((Container)(object)_mountPanel);
					val7.set_Texture(AsyncTexture2D.op_Implicit(img3));
					((Control)val7).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val7).set_Location(new Point(curX, curY));
					((Control)val7).set_Opacity(_settingOpacity.get_Value());
					((Control)val7).set_BasicTooltipText("Skyscale");
					Image _btnSkyscale = val7;
					((Control)_btnSkyscale).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSkyscaleBinding);
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
				if (_settingSpringerOrder.get_Value() == i)
				{
					Texture2D img2 = GetImgFile("springer");
					Image val8 = new Image();
					((Control)val8).set_Parent((Container)(object)_mountPanel);
					val8.set_Texture(AsyncTexture2D.op_Implicit(img2));
					((Control)val8).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val8).set_Location(new Point(curX, curY));
					((Control)val8).set_Opacity(_settingOpacity.get_Value());
					((Control)val8).set_BasicTooltipText("Springer");
					Image _btnSpringer = val8;
					((Control)_btnSpringer).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSpringerBinding);
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
				if (_settingWarclawOrder.get_Value() == i)
				{
					Texture2D img = GetImgFile("warclaw");
					Image val9 = new Image();
					((Control)val9).set_Parent((Container)(object)_mountPanel);
					val9.set_Texture(AsyncTexture2D.op_Implicit(img));
					((Control)val9).set_Size(new Point(_settingImgWidth.get_Value(), _settingImgWidth.get_Value()));
					((Control)val9).set_Location(new Point(curX, curY));
					((Control)val9).set_Opacity(_settingOpacity.get_Value());
					((Control)val9).set_BasicTooltipText("Warclaw");
					Image _btnWarclaw = val9;
					((Control)_btnWarclaw).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingWarclawBinding);
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
			}
			if (_settingDrag.get_Value())
			{
				Panel val10 = new Panel();
				((Control)val10).set_Parent((Container)(object)_mountPanel);
				((Control)val10).set_Location(new Point(0, 0));
				((Control)val10).set_Size(new Point(_settingImgWidth.get_Value() / 2, _settingImgWidth.get_Value() / 2));
				((Control)val10).set_BackgroundColor(Color.get_White());
				((Control)val10).set_ZIndex(10);
				Panel dragBox = val10;
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
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Expected O, but got Unknown
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Expected O, but got Unknown
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Expected O, but got Unknown
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Expected O, but got Unknown
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Expected O, but got Unknown
			int[] mountOrder = _mountOrder;
			foreach (int i in mountOrder)
			{
				if (i == 0)
				{
					continue;
				}
				if (_settingGriffonOrder.get_Value() == i)
				{
					Texture2D img8 = GetImgFile("griffon");
					CornerIcon val = new CornerIcon();
					val.set_IconName("Griffon");
					val.set_Icon(AsyncTexture2D.op_Implicit(img8));
					val.set_HoverIcon(AsyncTexture2D.op_Implicit(img8));
					val.set_Priority(10);
					_cornerGriffon = val;
					((Control)_cornerGriffon).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingGriffonBinding);
					});
				}
				if (_settingJackalOrder.get_Value() == i)
				{
					Texture2D img7 = GetImgFile("jackal");
					CornerIcon val2 = new CornerIcon();
					val2.set_IconName("Jackal");
					val2.set_Icon(AsyncTexture2D.op_Implicit(img7));
					val2.set_HoverIcon(AsyncTexture2D.op_Implicit(img7));
					val2.set_Priority(10);
					_cornerJackal = val2;
					((Control)_cornerJackal).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingJackalBinding);
					});
				}
				if (_settingRaptorOrder.get_Value() == i)
				{
					Texture2D img6 = GetImgFile("raptor");
					CornerIcon val3 = new CornerIcon();
					val3.set_IconName("Raptor");
					val3.set_Icon(AsyncTexture2D.op_Implicit(img6));
					val3.set_HoverIcon(AsyncTexture2D.op_Implicit(img6));
					val3.set_Priority(10);
					_cornerRaptor = val3;
					((Control)_cornerRaptor).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingRaptorBinding);
					});
				}
				if (_settingRollerOrder.get_Value() == i)
				{
					Texture2D img5 = GetImgFile("roller");
					CornerIcon val4 = new CornerIcon();
					val4.set_IconName("Roller");
					val4.set_Icon(AsyncTexture2D.op_Implicit(img5));
					val4.set_HoverIcon(AsyncTexture2D.op_Implicit(img5));
					val4.set_Priority(10);
					_cornerRoller = val4;
					((Control)_cornerRoller).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingRollerBinding);
					});
				}
				if (_settingSkimmerOrder.get_Value() == i)
				{
					Texture2D img4 = GetImgFile("skimmer");
					CornerIcon val5 = new CornerIcon();
					val5.set_IconName("Skimmer");
					val5.set_Icon(AsyncTexture2D.op_Implicit(img4));
					val5.set_HoverIcon(AsyncTexture2D.op_Implicit(img4));
					val5.set_Priority(10);
					_cornerSkimmer = val5;
					((Control)_cornerSkimmer).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSkimmerBinding);
					});
				}
				if (_settingSkyscaleOrder.get_Value() == i)
				{
					Texture2D img3 = GetImgFile("skyscale");
					CornerIcon val6 = new CornerIcon();
					val6.set_IconName("Skyscale");
					val6.set_Icon(AsyncTexture2D.op_Implicit(img3));
					val6.set_HoverIcon(AsyncTexture2D.op_Implicit(img3));
					val6.set_Priority(10);
					_cornerSkyscale = val6;
					((Control)_cornerSkyscale).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSkyscaleBinding);
					});
				}
				if (_settingSpringerOrder.get_Value() == i)
				{
					Texture2D img2 = GetImgFile("springer");
					CornerIcon val7 = new CornerIcon();
					val7.set_IconName("Springer");
					val7.set_Icon(AsyncTexture2D.op_Implicit(img2));
					val7.set_HoverIcon(AsyncTexture2D.op_Implicit(img2));
					val7.set_Priority(10);
					_cornerSpringer = val7;
					((Control)_cornerSpringer).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingSpringerBinding);
					});
				}
				if (_settingWarclawOrder.get_Value() == i)
				{
					Texture2D img = GetImgFile("warclaw");
					CornerIcon val8 = new CornerIcon();
					val8.set_IconName("Warclaw");
					val8.set_Icon(AsyncTexture2D.op_Implicit(img));
					val8.set_HoverIcon(AsyncTexture2D.op_Implicit(img));
					val8.set_Priority(10);
					_cornerWarclaw = val8;
					((Control)_cornerWarclaw).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						DoHotKey(_settingWarclawBinding);
					});
				}
			}
		}

		private void DrawIcons()
		{
			Panel mountPanel = _mountPanel;
			if (mountPanel != null)
			{
				((Control)mountPanel).Dispose();
			}
			CornerIcon cornerGriffon = _cornerGriffon;
			if (cornerGriffon != null)
			{
				((Control)cornerGriffon).Dispose();
			}
			CornerIcon cornerJackal = _cornerJackal;
			if (cornerJackal != null)
			{
				((Control)cornerJackal).Dispose();
			}
			CornerIcon cornerRaptor = _cornerRaptor;
			if (cornerRaptor != null)
			{
				((Control)cornerRaptor).Dispose();
			}
			CornerIcon cornerRoller = _cornerRoller;
			if (cornerRoller != null)
			{
				((Control)cornerRoller).Dispose();
			}
			CornerIcon cornerSkimmer = _cornerSkimmer;
			if (cornerSkimmer != null)
			{
				((Control)cornerSkimmer).Dispose();
			}
			CornerIcon cornerSkyscale = _cornerSkyscale;
			if (cornerSkyscale != null)
			{
				((Control)cornerSkyscale).Dispose();
			}
			CornerIcon cornerSpringer = _cornerSpringer;
			if (cornerSpringer != null)
			{
				((Control)cornerSpringer).Dispose();
			}
			CornerIcon cornerWarclaw = _cornerWarclaw;
			if (cornerWarclaw != null)
			{
				((Control)cornerWarclaw).Dispose();
			}
			if (_settingDisplay.get_Value().Equals("Solid Corner") || _settingDisplay.get_Value().Equals("Transparent Corner"))
			{
				DrawCornerIcons();
			}
			else
			{
				DrawManualIcons();
			}
		}

		private Texture2D GetImgFile(string filename)
		{
			switch (_settingDisplay.get_Value())
			{
			default:
				return ContentsManager.GetTexture(filename + ".png");
			case "Transparent Manual":
			case "Transparent Corner":
				return ContentsManager.GetTexture(filename + "-trans.png");
			case "Solid Manual Text":
				return ContentsManager.GetTexture(filename + "-text.png");
			}
		}

		private void DoDefaultMountAction()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (Array.Exists(warclawOnlyMaps, (MapType mapType) => mapType == GameService.Gw2Mumble.get_CurrentMap().get_Type()))
			{
				DoHotKey(_settingWarclawBinding);
			}
			else if (GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z <= 0f)
			{
				DoHotKey(_settingSkimmerBinding);
			}
			else
			{
				defaultMountChoicesToActions[_settingDefaultMountChoice.get_Value()]();
			}
		}

		private void HandleCombatChange(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				DoHotKey(InputQueuingKeybindSetting);
				InputQueuingKeybindSetting = null;
			}
		}

		protected void DoHotKey(SettingEntry<KeyBinding> setting)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Invalid comparison between Unknown and I4
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			if (setting == null)
			{
				return;
			}
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				InputQueuingKeybindSetting = setting;
				return;
			}
			if ((int)setting.get_Value().get_ModifierKeys() > 0)
			{
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Press((VirtualKeyShort)18, true);
				}
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Press((VirtualKeyShort)17, true);
				}
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Press((VirtualKeyShort)16, true);
				}
			}
			Keyboard.Press(ToVirtualKey(setting.get_Value().get_PrimaryKey()), true);
			Thread.Sleep(50);
			Keyboard.Release(ToVirtualKey(setting.get_Value().get_PrimaryKey()), true);
			if ((int)setting.get_Value().get_ModifierKeys() > 0)
			{
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Release((VirtualKeyShort)16, true);
				}
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Release((VirtualKeyShort)17, true);
				}
				if (((Enum)setting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Release((VirtualKeyShort)18, true);
				}
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return (VirtualKeyShort)(short)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}
	}
}
