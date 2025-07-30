using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.WhatRoleAmIPlaying.Models;
using Soeed.WhatRoleAmIPlaying.Services;

namespace Soeed.WhatRoleAmIPlaying
{
	[Export(typeof(Module))]
	public class WhatRoleAmIPlayingModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<WhatRoleAmIPlayingModule>();

		public static string MODULE_VERSION = "0.1.0";

		public static string DIRECTORY_PATH = "WhatRoleAmIPlaying";

		public static string STATIC_HOST_URL = "https://bhm.blishhud.com/Soeed.WhatRoleAmIPlaying";

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _contextMenuStrip;

		private StandardWindow _mainWindow;

		private Panel _roleDisplayPanel;

		private Image _professionIcon;

		private Image _eliteSpecIcon;

		private Image _boonIcon;

		private Label _roleNameLabel;

		private Label _roleDescriptionLabel;

		private StandardButton _buildUrlButton;

		private string _currentBuildUrl = string.Empty;

		internal static WhatRoleAmIPlayingModule WhatRoleAmIPlayingModuleInstance;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public static RoleConfigService RoleConfig { get; set; } = null;


		public static SettingService Settings { get; set; } = null;


		public static Gw2ApiService Gw2Api { get; set; } = null;


		[ImportingConstructor]
		public WhatRoleAmIPlayingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			WhatRoleAmIPlayingModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new SettingService(settings);
		}

		protected override async Task LoadAsync()
		{
			RoleConfig config = await new DynamicConfigService().LoadConfig();
			if (config == null)
			{
				Logger.Error("Failed to load dynamic configuration");
				return;
			}
			RoleConfig = new RoleConfigService(config);
			Gw2Api = new Gw2ApiService(Gw2ApiManager);
			CreateCornerIcon();
			CreateMainWindow();
			Logger.Info("What Role Am I Playing module loaded successfully");
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			ContextMenuStrip contextMenuStrip = _contextMenuStrip;
			if (contextMenuStrip != null)
			{
				((Control)contextMenuStrip).Dispose();
			}
			StandardWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			WhatRoleAmIPlayingModuleInstance = null;
		}

		private void CreateCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.FromAssetId(156720));
			((Control)val).set_BasicTooltipText("What Am I Playing?");
			val.set_Priority(1645843523);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			_contextMenuStrip = new ContextMenuStrip();
			((Control)_contextMenuStrip.AddMenuItem("Full Random")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole();
			});
			_contextMenuStrip.AddMenuItem("-");
			((Control)_contextMenuStrip.AddMenuItem("DPS")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.DPS);
			});
			((Control)_contextMenuStrip.AddMenuItem("DPS - Quickness")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomDPSWithBoon(providesQuickness: true, providesAlacrity: false);
			});
			((Control)_contextMenuStrip.AddMenuItem("DPS - Alacrity")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomDPSWithBoon(providesQuickness: false, providesAlacrity: true);
			});
			((Control)_contextMenuStrip.AddMenuItem("Healer - Any")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.Healer);
			});
			((Control)_contextMenuStrip.AddMenuItem("Healer - Quickness")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomHealerWithBoon(providesQuickness: true, providesAlacrity: false);
			});
			((Control)_contextMenuStrip.AddMenuItem("Healer - Alacrity")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomHealerWithBoon(providesQuickness: false, providesAlacrity: true);
			});
			((Control)_cornerIcon).set_Menu(_contextMenuStrip);
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StandardWindow mainWindow = _mainWindow;
				if (mainWindow != null)
				{
					((WindowBase2)mainWindow).ToggleWindow();
				}
			});
		}

		private void CreateMainWindow()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Expected O, but got Unknown
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Expected O, but got Unknown
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Expected O, but got Unknown
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Expected O, but got Unknown
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Expected O, but got Unknown
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Expected O, but got Unknown
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Expected O, but got Unknown
			//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_0613: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Expected O, but got Unknown
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_0647: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_065a: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Expected O, but got Unknown
			AsyncTexture2D Background = AsyncTexture2D.FromAssetId(155985);
			Rectangle SettingPanelRegion = default(Rectangle);
			((Rectangle)(ref SettingPanelRegion))._002Ector(40, 26, 913, 691);
			Rectangle SettingPanelContentRegion = default(Rectangle);
			((Rectangle)(ref SettingPanelContentRegion))._002Ector(40, 26, 913, 691);
			Point SettingPanelWindowSize = default(Point);
			((Point)(ref SettingPanelWindowSize))._002Ector(425, 425);
			StandardWindow val = new StandardWindow(Background, SettingPanelRegion, SettingPanelContentRegion, SettingPanelWindowSize);
			((WindowBase2)val).set_Title("What Am I Playing?");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("WhatRoleAmIPlayingModule_Main_123456");
			_mainWindow = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val2).set_Parent((Container)(object)_mainWindow);
			((Control)val2).set_Size(new Point(((Container)_mainWindow).get_ContentRegion().Width, ((Container)_mainWindow).get_ContentRegion().Height));
			((Panel)val2).set_CanScroll(true);
			FlowPanel mainContainer = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Random");
			((Control)val3).set_Parent((Container)(object)mainContainer);
			((Control)val3).set_Size(new Point(400, 40));
			((Control)val3).set_Location(new Point((((Container)_mainWindow).get_ContentRegion().Width - 400) / 2, 10));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole();
			});
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)mainContainer);
			((Control)val4).set_Size(new Point(420, 150));
			((Control)val4).set_Location(new Point((((Container)_mainWindow).get_ContentRegion().Width - 420) / 2, 60));
			Panel gridPanel = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("DPS");
			((Control)val5).set_Parent((Container)(object)gridPanel);
			((Control)val5).set_Size(new Point(120, 32));
			((Control)val5).set_Location(new Point(0, 0));
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.DPS);
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Power");
			((Control)val6).set_Parent((Container)(object)gridPanel);
			((Control)val6).set_Size(new Point(120, 32));
			((Control)val6).set_Location(new Point(0, 40));
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.PowerDPS);
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Condi");
			((Control)val7).set_Parent((Container)(object)gridPanel);
			((Control)val7).set_Size(new Point(120, 32));
			((Control)val7).set_Location(new Point(0, 80));
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.ConditionDPS);
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text("BoonDPS");
			((Control)val8).set_Parent((Container)(object)gridPanel);
			((Control)val8).set_Size(new Point(120, 32));
			((Control)val8).set_Location(new Point(140, 0));
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomDPSWithBoon(providesQuickness: true, providesAlacrity: false);
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text("Quickness");
			((Control)val9).set_Parent((Container)(object)gridPanel);
			((Control)val9).set_Size(new Point(120, 32));
			((Control)val9).set_Location(new Point(140, 40));
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomDPSWithBoon(providesQuickness: true, providesAlacrity: false);
			});
			StandardButton val10 = new StandardButton();
			val10.set_Text("Alacrity");
			((Control)val10).set_Parent((Container)(object)gridPanel);
			((Control)val10).set_Size(new Point(120, 32));
			((Control)val10).set_Location(new Point(140, 80));
			((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomDPSWithBoon(providesQuickness: false, providesAlacrity: true);
			});
			StandardButton val11 = new StandardButton();
			val11.set_Text("Healer");
			((Control)val11).set_Parent((Container)(object)gridPanel);
			((Control)val11).set_Size(new Point(120, 32));
			((Control)val11).set_Location(new Point(280, 0));
			((Control)val11).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomRole(RoleType.Healer);
			});
			StandardButton val12 = new StandardButton();
			val12.set_Text("QuickHeal");
			((Control)val12).set_Parent((Container)(object)gridPanel);
			((Control)val12).set_Size(new Point(120, 32));
			((Control)val12).set_Location(new Point(280, 40));
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomHealerWithBoon(providesQuickness: true, providesAlacrity: false);
			});
			StandardButton val13 = new StandardButton();
			val13.set_Text("AlacHeal");
			((Control)val13).set_Parent((Container)(object)gridPanel);
			((Control)val13).set_Size(new Point(120, 32));
			((Control)val13).set_Location(new Point(280, 80));
			((Control)val13).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GetRandomHealerWithBoon(providesQuickness: false, providesAlacrity: true);
			});
			Panel val14 = new Panel();
			((Control)val14).set_Parent((Container)(object)mainContainer);
			((Control)val14).set_Size(new Point(((Container)_mainWindow).get_ContentRegion().Width - 20, 150));
			((Control)val14).set_BackgroundColor(new Color(0, 0, 0, 100));
			((Control)val14).set_Visible(false);
			_roleDisplayPanel = val14;
			FlowPanel val15 = new FlowPanel();
			val15.set_FlowDirection((ControlFlowDirection)0);
			val15.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val15).set_Parent((Container)(object)_roleDisplayPanel);
			((Control)val15).set_Location(new Point(10, 10));
			((Control)val15).set_Size(new Point(80, 80));
			FlowPanel iconsPanel = val15;
			Image val16 = new Image();
			((Control)val16).set_Parent((Container)(object)iconsPanel);
			((Control)val16).set_Size(new Point(32, 32));
			val16.set_Texture(AsyncTexture2D.FromAssetId(156678));
			_professionIcon = val16;
			Image val17 = new Image();
			((Control)val17).set_Parent((Container)(object)iconsPanel);
			((Control)val17).set_Size(new Point(32, 32));
			val17.set_Texture(AsyncTexture2D.FromAssetId(156678));
			_eliteSpecIcon = val17;
			Image val18 = new Image();
			((Control)val18).set_Parent((Container)(object)iconsPanel);
			((Control)val18).set_Size(new Point(32, 32));
			val18.set_Texture(AsyncTexture2D.FromAssetId(156678));
			((Control)val18).set_Visible(false);
			_boonIcon = val18;
			FlowPanel val19 = new FlowPanel();
			val19.set_FlowDirection((ControlFlowDirection)3);
			val19.set_ControlPadding(new Vector2(0f, 5f));
			((Control)val19).set_Parent((Container)(object)_roleDisplayPanel);
			((Control)val19).set_Location(new Point(100, 10));
			((Control)val19).set_Size(new Point(((Control)_roleDisplayPanel).get_Width() - 110, ((Control)_roleDisplayPanel).get_Height() - 20));
			FlowPanel roleInfoPanel = val19;
			Label val20 = new Label();
			((Control)val20).set_Parent((Container)(object)roleInfoPanel);
			val20.set_Text("No role selected");
			val20.set_Font(GameService.Content.get_DefaultFont16());
			val20.set_TextColor(Color.get_White());
			val20.set_AutoSizeHeight(true);
			val20.set_AutoSizeWidth(true);
			_roleNameLabel = val20;
			Label val21 = new Label();
			((Control)val21).set_Parent((Container)(object)roleInfoPanel);
			val21.set_Text("Click a button above to get a role suggestion");
			val21.set_Font(GameService.Content.get_DefaultFont12());
			val21.set_TextColor(Color.get_LightGray());
			val21.set_AutoSizeHeight(true);
			val21.set_AutoSizeWidth(true);
			_roleDescriptionLabel = val21;
			StandardButton val22 = new StandardButton();
			((Control)val22).set_Parent((Container)(object)roleInfoPanel);
			val22.set_Text("View Build");
			((Control)val22).set_Size(new Point(100, 25));
			((Control)val22).set_Visible(false);
			_buildUrlButton = val22;
			((Control)_buildUrlButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenBuildUrl();
			});
		}

		private void DisplayRole(RoleSuggestion role)
		{
			try
			{
				ProfessionInfo profession = RoleConfig.GetProfessionByName(role.Profession);
				EliteSpecInfo eliteSpec = RoleConfig.GetEliteSpecByName(role.EliteSpec);
				if (profession != null && !string.IsNullOrEmpty(profession.Icon) && int.TryParse(profession.Icon, out var professionIconId))
				{
					_professionIcon.set_Texture(AsyncTexture2D.FromAssetId(professionIconId));
				}
				if (eliteSpec != null && !string.IsNullOrEmpty(eliteSpec.Icon) && int.TryParse(eliteSpec.Icon, out var eliteSpecIconId))
				{
					_eliteSpecIcon.set_Texture(AsyncTexture2D.FromAssetId(eliteSpecIconId));
				}
				((Control)_boonIcon).set_Visible(false);
				if (role.ProvidesQuickness && role.ProvidesAlacrity)
				{
					_boonIcon.set_Texture(AsyncTexture2D.FromAssetId(1012835));
					((Control)_boonIcon).set_Visible(true);
				}
				else if (role.ProvidesQuickness)
				{
					_boonIcon.set_Texture(AsyncTexture2D.FromAssetId(1012835));
					((Control)_boonIcon).set_Visible(true);
				}
				else if (role.ProvidesAlacrity)
				{
					_boonIcon.set_Texture(AsyncTexture2D.FromAssetId(1938787));
					((Control)_boonIcon).set_Visible(true);
				}
				_roleNameLabel.set_Text(role.Profession + " - " + role.EliteSpec + " (" + role.Role + ")");
				_roleDescriptionLabel.set_Text(role.Description);
				((Control)_buildUrlButton).set_Visible(!string.IsNullOrEmpty(role.BuildUrl));
				_currentBuildUrl = role.BuildUrl ?? string.Empty;
				((Control)_roleDisplayPanel).set_Visible(true);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to display role");
			}
		}

		private void OpenBuildUrl()
		{
			try
			{
				if (!string.IsNullOrEmpty(_currentBuildUrl))
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = _currentBuildUrl,
						UseShellExecute = true
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to open build URL");
				ScreenNotification.ShowNotification("Failed to open build URL", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async void GetRandomRole(RoleType roleType = RoleType.FullRandom)
		{
			_ = 1;
			try
			{
				if (!(await Gw2Api.HasUnlockedEliteSpecsAsync()))
				{
					ScreenNotification.ShowNotification("No unlocked elite specs found. Please unlock some elite specializations first.", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				List<RoleSuggestion> availableRoles = await Gw2Api.GetAvailableRolesAsync(roleType);
				if (!availableRoles.Any())
				{
					ScreenNotification.ShowNotification($"No available roles for {roleType} with your unlocked elite specs", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				int randomIndex = new Random().Next(availableRoles.Count);
				RoleSuggestion suggestion = availableRoles[randomIndex];
				DisplayRole(suggestion);
				ScreenNotification.ShowNotification("Try: " + suggestion.Description, (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Selected role: " + suggestion.Profession + " - " + suggestion.EliteSpec + " (" + suggestion.Role + ")");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to get random role");
				ScreenNotification.ShowNotification("Failed to get role suggestion", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async void GetRandomDPSWithBoon(bool providesQuickness, bool providesAlacrity)
		{
			try
			{
				if (!(await Gw2Api.HasUnlockedEliteSpecsAsync()))
				{
					ScreenNotification.ShowNotification("No unlocked elite specs found. Please unlock some elite specializations first.", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				List<RoleSuggestion> availableRoles = RoleConfig.GetDPSWithBoon(providesQuickness, providesAlacrity);
				if (!availableRoles.Any())
				{
					string boonText = ((providesQuickness && providesAlacrity) ? "both boons" : (providesQuickness ? "quickness" : "alacrity"));
					ScreenNotification.ShowNotification("No available DPS roles with " + boonText + " and your unlocked elite specs", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				int randomIndex = new Random().Next(availableRoles.Count);
				RoleSuggestion suggestion = availableRoles[randomIndex];
				DisplayRole(suggestion);
				ScreenNotification.ShowNotification("Try: " + suggestion.Profession + " - " + suggestion.EliteSpec + " (" + suggestion.Role + ")", (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Selected DPS role with boons: " + suggestion.Profession + " - " + suggestion.EliteSpec + " (" + suggestion.Role + ")");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to get random DPS role with boon");
				ScreenNotification.ShowNotification("Failed to get role suggestion", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async void GetRandomHealerWithBoon(bool providesQuickness, bool providesAlacrity)
		{
			try
			{
				if (!(await Gw2Api.HasUnlockedEliteSpecsAsync()))
				{
					ScreenNotification.ShowNotification("No unlocked elite specs found. Please unlock some elite specializations first.", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				List<RoleSuggestion> availableRoles = RoleConfig.GetHealerWithBoon(providesQuickness, providesAlacrity);
				if (!availableRoles.Any())
				{
					string boonText = ((providesQuickness && providesAlacrity) ? "both boons" : (providesQuickness ? "quickness" : "alacrity"));
					ScreenNotification.ShowNotification("No available healer roles with " + boonText + " and your unlocked elite specs", (NotificationType)1, (Texture2D)null, 4);
					return;
				}
				int randomIndex = new Random().Next(availableRoles.Count);
				RoleSuggestion suggestion = availableRoles[randomIndex];
				DisplayRole(suggestion);
				ScreenNotification.ShowNotification("Try: " + suggestion.Profession + " - " + suggestion.EliteSpec + " (" + suggestion.Role + ")", (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Selected healer role with boons: " + suggestion.Profession + " - " + suggestion.EliteSpec + " (" + suggestion.Role + ")");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to get random healer role with boon");
				ScreenNotification.ShowNotification("Failed to get role suggestion", (NotificationType)2, (Texture2D)null, 4);
			}
		}
	}
}
