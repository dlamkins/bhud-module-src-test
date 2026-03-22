using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using DecorBlishhudModule.CustomControls.CustomTab;
using DecorBlishhudModule.Model;
using DecorBlishhudModule.Refinement;
using DecorBlishhudModule.Sections;
using DecorBlishhudModule.Sections.LeftSideTasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule
{
	[Export(typeof(Module))]
	public class DecorModule : Module
	{
		public readonly Logger Logger = Logger.GetLogger<DecorModule>();

		private static readonly HttpClient client = new HttpClient();

		private CornerIcon _cornerIcon;

		private Texture2D _homesteadIconMenu;

		private Texture2D _homesteadIconMenuLunar;

		private Texture2D _homesteadIconMenuSAB;

		private Texture2D _homesteadIconMenuDragonBash;

		private Texture2D _homesteadIconMenuFOTFW;

		private Texture2D _homesteadIconMenuHalloween;

		private Texture2D _homesteadIconMenuWintersday;

		private Texture2D _currentEmblem;

		private Texture2D _homesteadIconHover;

		private Texture2D _homesteadIconUnactive;

		private Texture2D _homesteadScreen;

		private Texture2D _guildhallScreen;

		private Texture2D _farmScreen;

		private Texture2D _lumberScreen;

		private Texture2D _metalScreen;

		private Texture2D _handiworkTab;

		private Texture2D _scribeTab;

		private Texture2D _iconsTab;

		private Texture2D _imagesTab;

		private Texture2D _farmTab;

		private Texture2D _lumberTab;

		private Texture2D _metalTab;

		private Texture2D _info;

		private Texture2D _blackTexture;

		private Texture2D _x;

		private Texture2D _x2;

		private Texture2D _x2Active;

		private Texture2D _copy;

		private Texture2D _zoom;

		private Texture2D _click;

		private Texture2D _empty;

		private Texture2D _heart;

		private Texture2D _copperCoin;

		private Texture2D _silverCoin;

		private Texture2D _arrowUp;

		private Texture2D _arrowDown;

		private Texture2D _arrowNeutral;

		private Texture2D _efficiancy;

		private CustomTabbedWindow2 _decorWindow;

		private Image _decorationIcon;

		private Label _decorationRightText;

		private Image _decorationImage;

		private SignatureSection _signatureLabelManager;

		private WikiLicenseSection _wikiLicenseManager;

		private FlowPanel _homesteadDecorationsFlowPanel;

		private FlowPanel _farmPanel;

		private FlowPanel _lumberPanel;

		private FlowPanel _metalPanel;

		private bool _loaded;

		internal static DecorModule DecorModuleInstance;

		public CustomTabbedWindow2 DecorWindow => _decorWindow;

		public Label DecorationRightText => _decorationRightText;

		public Image DecorationImage => _decorationImage;

		public Texture2D BlackTexture => _blackTexture;

		public Texture2D X => _x;

		public Texture2D X2 => _x2;

		public Texture2D X2Active => _x2Active;

		public Texture2D Info => _info;

		public bool Loaded => _loaded;

		public Texture2D CopyIcon => _copy;

		public Texture2D Heart => _heart;

		public Texture2D CopperCoin => _copperCoin;

		public Texture2D SilverCoin => _silverCoin;

		public Texture2D ArrowUp => _arrowUp;

		public Texture2D ArrowDown => _arrowDown;

		public Texture2D ArrowNeutral => _arrowNeutral;

		public Texture2D Efficiency => _efficiancy;

		public HttpClient Client => client;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		public ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public DecorModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			DecorModuleInstance = this;
		}

		protected override async Task LoadAsync()
		{
			foreach (string directoryName in DirectoriesManager.get_RegisteredDirectories())
			{
				string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath(directoryName);
				List<string> allFiles = Directory.EnumerateFiles(fullDirectoryPath, "*", SearchOption.AllDirectories).ToList();
				Logger.Info($"'{directoryName}' can be found at '{fullDirectoryPath}' and has {allFiles.Count} total files within it.");
			}
			_homesteadIconUnactive = ContentsManager.GetTexture("test/homesteadIconUnactive.png");
			_homesteadIconHover = ContentsManager.GetTexture("test/homesteadIconHover.png");
			_homesteadIconMenu = ContentsManager.GetTexture("test/homesteadIconMenu.png");
			_homesteadIconMenuLunar = ContentsManager.GetTexture("test/homesteadIconMenuLunar.png");
			_homesteadIconMenuSAB = ContentsManager.GetTexture("test/homesteadIconMenuSAB.png");
			_homesteadIconMenuDragonBash = ContentsManager.GetTexture("test/homesteadIconMenuDragonBash.png");
			_homesteadIconMenuFOTFW = ContentsManager.GetTexture("test/homesteadIconMenuFOTFW.png");
			_homesteadIconMenuHalloween = ContentsManager.GetTexture("test/homesteadIconMenuHalloween.png");
			_homesteadIconMenuWintersday = ContentsManager.GetTexture("test/homesteadIconMenuWinterstay.png");
			_homesteadScreen = ContentsManager.GetTexture("test/homestead_screen.png");
			_guildhallScreen = ContentsManager.GetTexture("test/guildhall_screen.png");
			_farmScreen = ContentsManager.GetTexture("test/farm_screen.png");
			_lumberScreen = ContentsManager.GetTexture("test/lumber_screen.png");
			_metalScreen = ContentsManager.GetTexture("test/metal_screen.png");
			_handiworkTab = ContentsManager.GetTexture("test/handiwork.png");
			_scribeTab = ContentsManager.GetTexture("test/scribe.png");
			_iconsTab = ContentsManager.GetTexture("test/icons.png");
			_imagesTab = ContentsManager.GetTexture("test/images.png");
			_farmTab = ContentsManager.GetTexture("test/farm.png");
			_lumberTab = ContentsManager.GetTexture("test/lumber.png");
			_metalTab = ContentsManager.GetTexture("test/metal.png");
			_info = ContentsManager.GetTexture("test/info.png");
			_blackTexture = ContentsManager.GetTexture("test/black_texture.png");
			_x = ContentsManager.GetTexture("test/x.png");
			_x2 = ContentsManager.GetTexture("test/x2.png");
			_x2Active = ContentsManager.GetTexture("test/x2_active.png");
			_copy = ContentsManager.GetTexture("test/copy.png");
			_zoom = ContentsManager.GetTexture("test/zoom.png");
			_click = ContentsManager.GetTexture("test/click.png");
			_empty = ContentsManager.GetTexture("test/empty.png");
			_heart = ContentsManager.GetTexture("test/heart.png");
			_copperCoin = ContentsManager.GetTexture("test/coin_copper.png");
			_silverCoin = ContentsManager.GetTexture("test/coin_silver.png");
			_arrowUp = ContentsManager.GetTexture("test/arrow_up.png");
			_arrowDown = ContentsManager.GetTexture("test/arrow_down.png");
			_arrowNeutral = ContentsManager.GetTexture("test/arrow_neutral.png");
			_efficiancy = ContentsManager.GetTexture("test/efficiency.png");
			DecorModule decorModule = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_homesteadIconUnactive));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(_homesteadIconHover));
			val.set_IconName("Decor");
			val.set_Priority(1645843523);
			val.set_LoadingMessage("Decor is fetching data...");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			decorModule._cornerIcon = val;
			_currentEmblem = await MainIconTheme.GetThemeIconAsync(_homesteadIconMenu, _homesteadIconMenuLunar, _homesteadIconMenuSAB, _homesteadIconMenuDragonBash, _homesteadIconMenuFOTFW, _homesteadIconMenuHalloween, _homesteadIconMenuWintersday);
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			await CreateGw2StyleWindowThatDisplaysAllDecorations(windowBackgroundTexture);
			InfoSection.InitializeInfoPanel();
			await RightSideSection.UpdateDecorationImageAsync(new Decoration
			{
				Name = "Welcome to Decor. Enjoy your stay!",
				IconUrl = null,
				ImageUrl = "https://i.imgur.com/VBAy1WA.jpeg"
			}, (Container)(object)_decorWindow, _decorationImage);
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_decorWindow != null)
				{
					if (((Control)_decorWindow).get_Visible())
					{
						((Control)_decorWindow).Hide();
					}
					else
					{
						((Control)_decorWindow).Show();
					}
				}
			});
			_signatureLabelManager = new SignatureSection((Container)(object)_decorWindow);
			_wikiLicenseManager = new WikiLicenseSection((Container)(object)_decorWindow);
			while (!_loaded)
			{
				await Task.Delay(100);
			}
		}

		protected override void Unload()
		{
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Texture2D homesteadIconMenu = _homesteadIconMenu;
			if (homesteadIconMenu != null)
			{
				((GraphicsResource)homesteadIconMenu).Dispose();
			}
			Texture2D homesteadIconMenuLunar = _homesteadIconMenuLunar;
			if (homesteadIconMenuLunar != null)
			{
				((GraphicsResource)homesteadIconMenuLunar).Dispose();
			}
			Texture2D homesteadIconMenuSAB = _homesteadIconMenuSAB;
			if (homesteadIconMenuSAB != null)
			{
				((GraphicsResource)homesteadIconMenuSAB).Dispose();
			}
			Texture2D homesteadIconMenuDragonBash = _homesteadIconMenuDragonBash;
			if (homesteadIconMenuDragonBash != null)
			{
				((GraphicsResource)homesteadIconMenuDragonBash).Dispose();
			}
			((GraphicsResource)_homesteadIconMenuFOTFW).Dispose();
			((GraphicsResource)_homesteadIconMenuHalloween).Dispose();
			Texture2D homesteadIconMenuWintersday = _homesteadIconMenuWintersday;
			if (homesteadIconMenuWintersday != null)
			{
				((GraphicsResource)homesteadIconMenuWintersday).Dispose();
			}
			Texture2D homesteadIconHover = _homesteadIconHover;
			if (homesteadIconHover != null)
			{
				((GraphicsResource)homesteadIconHover).Dispose();
			}
			Texture2D homesteadIconUnactive = _homesteadIconUnactive;
			if (homesteadIconUnactive != null)
			{
				((GraphicsResource)homesteadIconUnactive).Dispose();
			}
			Texture2D homesteadScreen = _homesteadScreen;
			if (homesteadScreen != null)
			{
				((GraphicsResource)homesteadScreen).Dispose();
			}
			Texture2D guildhallScreen = _guildhallScreen;
			if (guildhallScreen != null)
			{
				((GraphicsResource)guildhallScreen).Dispose();
			}
			Texture2D farmScreen = _farmScreen;
			if (farmScreen != null)
			{
				((GraphicsResource)farmScreen).Dispose();
			}
			Texture2D lumberScreen = _lumberScreen;
			if (lumberScreen != null)
			{
				((GraphicsResource)lumberScreen).Dispose();
			}
			Texture2D metalScreen = _metalScreen;
			if (metalScreen != null)
			{
				((GraphicsResource)metalScreen).Dispose();
			}
			CustomTabbedWindow2 decorWindow = _decorWindow;
			if (decorWindow != null)
			{
				((Control)decorWindow).Dispose();
			}
			Texture2D handiworkTab = _handiworkTab;
			if (handiworkTab != null)
			{
				((GraphicsResource)handiworkTab).Dispose();
			}
			Texture2D scribeTab = _scribeTab;
			if (scribeTab != null)
			{
				((GraphicsResource)scribeTab).Dispose();
			}
			Texture2D iconsTab = _iconsTab;
			if (iconsTab != null)
			{
				((GraphicsResource)iconsTab).Dispose();
			}
			Texture2D imagesTab = _imagesTab;
			if (imagesTab != null)
			{
				((GraphicsResource)imagesTab).Dispose();
			}
			Texture2D farmTab = _farmTab;
			if (farmTab != null)
			{
				((GraphicsResource)farmTab).Dispose();
			}
			Texture2D lumberTab = _lumberTab;
			if (lumberTab != null)
			{
				((GraphicsResource)lumberTab).Dispose();
			}
			Texture2D metalTab = _metalTab;
			if (metalTab != null)
			{
				((GraphicsResource)metalTab).Dispose();
			}
			Image decorationIcon = _decorationIcon;
			if (decorationIcon != null)
			{
				((Control)decorationIcon).Dispose();
			}
			Image decorationImage = _decorationImage;
			if (decorationImage != null)
			{
				((Control)decorationImage).Dispose();
			}
			DecorModuleInstance = null;
		}

		private async Task CreateGw2StyleWindowThatDisplaysAllDecorations(AsyncTexture2D windowBackgroundTexture)
		{
			DecorModule decorModule = this;
			CustomTabbedWindow2 customTabbedWindow = new CustomTabbedWindow2(windowBackgroundTexture, new Rectangle(0, 26, 590, 640), new Rectangle(50, 40, 550, 640), new Point(1150, 800));
			((Control)customTabbedWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)customTabbedWindow).set_Title("Decor");
			((WindowBase2)customTabbedWindow).set_Emblem(_currentEmblem);
			((WindowBase2)customTabbedWindow).set_Subtitle("Homestead Decorations");
			((Control)customTabbedWindow).set_Location(new Point(300, 300));
			((WindowBase2)customTabbedWindow).set_SavesPosition(true);
			((WindowBase2)customTabbedWindow).set_Id("DecorModule_Decoration_Window");
			decorModule._decorWindow = customTabbedWindow;
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)_decorWindow);
			((Control)val).set_Size(new Point(((Control)_decorWindow).get_Size().X - 75, ((Control)_decorWindow).get_Size().Y - 75));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Opacity(0.3f);
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 10));
			val.set_Texture(AsyncTexture2D.op_Implicit(_homesteadScreen));
			((Control)val).set_Visible(true);
			Image backgroundImage = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)_decorWindow);
			((Control)val2).set_Location(new Point(20, 0));
			((Control)val2).set_Size(new Point(280, 30));
			((TextInputBase)val2).set_Font(GameService.Content.get_DefaultFont16());
			((TextInputBase)val2).set_PlaceholderText("Search Decorations...");
			TextBox searchTextBox = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)_decorWindow);
			((Control)val3).set_Location(new Point(((Control)searchTextBox).get_Right() - 23, ((Control)searchTextBox).get_Top() + 5));
			((Control)val3).set_Size(new Point(18, 18));
			((Control)val3).set_Visible(false);
			val3.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_x));
			Panel clearButton = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)_decorWindow);
			((Control)val4).set_Location(new Point(((Control)searchTextBox).get_Right() + 20, ((Control)searchTextBox).get_Top()));
			((Control)val4).set_Width(200);
			((Control)val4).set_Height(50);
			Panel checkboxFlowPanel = val4;
			CustomLoadingSpinner customLoadingSpinner = new CustomLoadingSpinner();
			((Control)customLoadingSpinner).set_Parent((Container)(object)checkboxFlowPanel);
			((Control)customLoadingSpinner).set_Location(new Point(0, 0));
			CustomLoadingSpinner checkboxSpinner = customLoadingSpinner;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)checkboxFlowPanel);
			((Control)val5).set_Location(new Point(((Control)checkboxSpinner).get_Right(), -8));
			val5.set_Text("Loading... Please wait");
			((Control)val5).set_Size(new Point(200, 50));
			Label checkboxLoading = val5;
			Checkbox val6 = new Checkbox();
			((Control)val6).set_Parent((Container)(object)checkboxFlowPanel);
			((Control)val6).set_Location(new Point(15, 9));
			val6.set_Text("Display Categories");
			val6.set_Checked(true);
			((Control)val6).set_Visible(false);
			Checkbox checkbox = val6;
			DecorModule decorModule2 = this;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)_decorWindow);
			val7.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val7).set_ShowBorder(true);
			((Control)val7).set_Width(500);
			((Control)val7).set_Height(660);
			((Panel)val7).set_CanScroll(true);
			((Control)val7).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val7).set_Visible(true);
			decorModule2._homesteadDecorationsFlowPanel = val7;
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)_decorWindow);
			val8.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val8).set_ShowBorder(true);
			((Control)val8).set_Width(500);
			((Control)val8).set_Height(660);
			((Panel)val8).set_CanScroll(true);
			((Control)val8).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val8).set_Visible(false);
			FlowPanel guildHallDecorationsFlowPanel = val8;
			FlowPanel val9 = new FlowPanel();
			((Control)val9).set_Parent((Container)(object)_decorWindow);
			val9.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val9).set_ShowBorder(true);
			((Control)val9).set_Width(1080);
			((Control)val9).set_Height(660);
			((Panel)val9).set_CanScroll(true);
			((Control)val9).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val9).set_Visible(false);
			FlowPanel homesteadDecorationsBigFlowPanel = val9;
			FlowPanel val10 = new FlowPanel();
			((Control)val10).set_Parent((Container)(object)_decorWindow);
			val10.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val10).set_ShowBorder(true);
			((Control)val10).set_Width(1080);
			((Control)val10).set_Height(660);
			((Panel)val10).set_CanScroll(true);
			((Control)val10).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val10).set_Visible(false);
			FlowPanel guildHallDecorationsBigFlowPanel = val10;
			DecorModule decorModule3 = this;
			FlowPanel val11 = new FlowPanel();
			((Control)val11).set_Parent((Container)(object)_decorWindow);
			val11.set_FlowDirection((ControlFlowDirection)2);
			((Control)val11).set_Width(1080);
			((Control)val11).set_Height(700);
			((Panel)val11).set_CanScroll(true);
			((Control)val11).set_Visible(false);
			decorModule3._farmPanel = val11;
			DecorModule decorModule4 = this;
			FlowPanel val12 = new FlowPanel();
			((Control)val12).set_Parent((Container)(object)_decorWindow);
			val12.set_FlowDirection((ControlFlowDirection)2);
			((Control)val12).set_Width(1080);
			((Control)val12).set_Height(700);
			((Panel)val12).set_CanScroll(true);
			((Control)val12).set_Visible(false);
			decorModule4._lumberPanel = val12;
			DecorModule decorModule5 = this;
			FlowPanel val13 = new FlowPanel();
			((Control)val13).set_Parent((Container)(object)_decorWindow);
			val13.set_FlowDirection((ControlFlowDirection)2);
			((Control)val13).set_Width(1080);
			((Control)val13).set_Height(700);
			((Panel)val13).set_CanScroll(true);
			((Control)val13).set_Visible(false);
			decorModule5._metalPanel = val13;
			DecorModule decorModule6 = this;
			Label val14 = new Label();
			((Control)val14).set_Parent((Container)(object)_decorWindow);
			((Control)val14).set_Width(500);
			((Control)val14).set_Height(120);
			val14.set_WrapText(true);
			val14.set_StrokeText(true);
			val14.set_ShowShadow(true);
			val14.set_ShadowColor(new Color(0, 0, 0));
			val14.set_Font(GameService.Content.get_DefaultFont18());
			decorModule6._decorationRightText = val14;
			DecorModule decorModule7 = this;
			Image val15 = new Image();
			((Control)val15).set_Parent((Container)(object)_decorWindow);
			((Control)val15).set_Size(new Point(40, 40));
			((Control)val15).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationRightText).get_Bottom() + 5));
			decorModule7._decorationIcon = val15;
			DecorModule decorModule8 = this;
			Image val16 = new Image();
			((Control)val16).set_Parent((Container)(object)_decorWindow);
			((Control)val16).set_Size(new Point(400, 400));
			((Control)val16).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationIcon).get_Bottom() + 5));
			decorModule8._decorationImage = val16;
			CustomTab customTab1 = new CustomTab(AsyncTexture2D.op_Implicit(_handiworkTab), "Homestead Handiwork", 7);
			CustomTab customTab2 = new CustomTab(AsyncTexture2D.op_Implicit(_scribeTab), "Guild Hall Scribe", 6);
			CustomTab customTab3 = new CustomTab(AsyncTexture2D.op_Implicit(_iconsTab), "Icons Preview", 5);
			CustomTab customTab4 = new CustomTab(AsyncTexture2D.op_Implicit(_imagesTab), "Images Preview", 4);
			CustomTab customTab5 = new CustomTab(AsyncTexture2D.op_Implicit(_farmTab), "Refinement - Farm", 3);
			CustomTab customTab6 = new CustomTab(AsyncTexture2D.op_Implicit(_lumberTab), "Refinement - Lumber Mill", 2);
			CustomTab customTab7 = new CustomTab(AsyncTexture2D.op_Implicit(_metalTab), "Refinement - Metal Forge", 1);
			_decorWindow.TabsGroup1.Add(customTab1);
			_decorWindow.TabsGroup1.Add(customTab2);
			_decorWindow.TabsGroup2.Add(customTab3);
			_decorWindow.TabsGroup2.Add(customTab4);
			_decorWindow.TabsGroup3.Add(customTab5);
			_decorWindow.TabsGroup3.Add(customTab6);
			_decorWindow.TabsGroup3.Add(customTab7);
			_decorWindow.SelectedTabGroup1 = _decorWindow.TabsGroup1.FirstOrDefault();
			_decorWindow.SelectedTabGroup2 = _decorWindow.TabsGroup2.FirstOrDefault();
			_ = _decorWindow.SelectedTabGroup1;
			_ = _decorWindow.SelectedTabGroup2;
			_ = _decorWindow.SelectedTabGroup3;
			_decorWindow.TabChanged += delegate
			{
				CustomTab selectedTabGroup = _decorWindow.SelectedTabGroup1;
				CustomTab selectedTabGroup2 = _decorWindow.SelectedTabGroup2;
				CustomTab selectedTabGroup3 = _decorWindow.SelectedTabGroup3;
				if (selectedTabGroup == customTab1 && selectedTabGroup2 == customTab3)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Homestead Decorations");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_homesteadScreen));
					((Control)backgroundImage).set_Visible(true);
					((Control)_decorationRightText).set_Visible(true);
					((Control)_decorationImage).set_Visible(true);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(true);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(true);
					((Control)clearButton).set_Visible(!string.IsNullOrEmpty(((TextInputBase)searchTextBox).get_Text()));
					((Control)checkboxFlowPanel).set_Visible(true);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: false);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: false);
					InfoSection.UpdateInfoVisible(visible: true);
					InfoSection.SetInfo(new(string, string)[3]
					{
						("test/empty.png", "                                                     "),
						("test/click.png", "Double-click on an icon to go to its wiki page."),
						("test/copy.png", "Click on the name or the image to copy its name.")
					});
				}
				else if (selectedTabGroup == customTab2 && selectedTabGroup2 == customTab3)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Guild Hall Decorations");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_guildhallScreen));
					((Control)backgroundImage).set_Visible(true);
					((Control)_decorationRightText).set_Visible(true);
					((Control)_decorationImage).set_Visible(true);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(true);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(true);
					((Control)clearButton).set_Visible(!string.IsNullOrEmpty(((TextInputBase)searchTextBox).get_Text()));
					((Control)checkboxFlowPanel).set_Visible(true);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: false);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: false);
					InfoSection.UpdateInfoVisible(visible: true);
					InfoSection.SetInfo(new(string, string)[3]
					{
						("test/empty.png", "                                                  "),
						("test/click.png", "Double-click on an icon to go to its wiki page."),
						("test/copy.png", "Click on the name or the image to copy its name.")
					});
				}
				else if (selectedTabGroup == customTab1 && selectedTabGroup2 == customTab4)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Homestead Decorations");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_homesteadScreen));
					((Control)backgroundImage).set_Visible(true);
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(true);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(true);
					((Control)clearButton).set_Visible(!string.IsNullOrEmpty(((TextInputBase)searchTextBox).get_Text()));
					((Control)checkboxFlowPanel).set_Visible(true);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoVisible(visible: true);
					InfoSection.SetInfo(new(string, string)[3]
					{
						("test/zoom.png", "Click on the image to zoom in."),
						("test/click.png", "Double-click on an icon to go to its wiki page."),
						("test/copy.png", "The copy icon copies the decoration name.")
					});
				}
				else if (selectedTabGroup == customTab2 && selectedTabGroup2 == customTab4)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Guild Hall Decorations");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_guildhallScreen));
					((Control)backgroundImage).set_Visible(true);
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(true);
					((Control)searchTextBox).set_Visible(true);
					((Control)clearButton).set_Visible(!string.IsNullOrEmpty(((TextInputBase)searchTextBox).get_Text()));
					((Control)checkboxFlowPanel).set_Visible(true);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoVisible(visible: true);
					InfoSection.SetInfo(new(string, string)[3]
					{
						("test/zoom.png", "Click on the image to zoom in."),
						("test/click.png", "Double-click on an icon to go to its wiki page."),
						("test/copy.png", "The copy icon copies the decoration name.")
					});
				}
				else if (selectedTabGroup3 == customTab5)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Refinement - Farm");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_farmScreen));
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(false);
					((Control)clearButton).set_Visible(false);
					((Control)checkboxFlowPanel).set_Visible(false);
					((Control)_farmPanel).set_Visible(true);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoVisible(visible: false);
				}
				else if (selectedTabGroup3 == customTab6)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Refinement - Lumber Mill");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_lumberScreen));
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(false);
					((Control)clearButton).set_Visible(false);
					((Control)checkboxFlowPanel).set_Visible(false);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(true);
					((Control)_metalPanel).set_Visible(false);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoVisible(visible: false);
				}
				else if (selectedTabGroup3 == customTab7)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Refinement - Metal Forge");
					backgroundImage.set_Texture(AsyncTexture2D.op_Implicit(_metalScreen));
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					((Control)searchTextBox).set_Visible(false);
					((Control)clearButton).set_Visible(false);
					((Control)checkboxFlowPanel).set_Visible(false);
					((Control)_farmPanel).set_Visible(false);
					((Control)_lumberPanel).set_Visible(false);
					((Control)_metalPanel).set_Visible(true);
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoVisible(visible: false);
				}
			};
			customTab2.Enabled = false;
			customTab4.Enabled = false;
			Dictionary<Control, int> _originalHeightsHomestead = new Dictionary<Control, int>();
			Dictionary<Control, int> _originalHeightsGuildHall = new Dictionary<Control, int>();
			Dictionary<Control, int> _originalHeightsHomesteadBig = new Dictionary<Control, int>();
			Dictionary<Control, int> _originalHeightsGuildHallBig = new Dictionary<Control, int>();
			await LeftSideSection.PopulateHomesteadIconsInFlowPanel(_homesteadDecorationsFlowPanel, _isIconView: true);
			LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)_homesteadDecorationsFlowPanel, _originalHeightsHomestead);
			await ApplySearchFilterAsync();
			await CustomTableFarm.Initialize(_farmPanel, "farm");
			await CustomTableLumber.Initialize(_lumberPanel, "lumber");
			await CustomTableMetal.Initialize(_metalPanel, "metal");
			_cornerIcon.set_LoadingMessage((string)null);
			Task.Run(async delegate
			{
				await LeftSideSection.PopulateGuildHallIconsInFlowPanel(guildHallDecorationsFlowPanel, _isIconView: true);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsFlowPanel, _originalHeightsGuildHall);
				await ApplySearchFilterAsync();
				customTab2.Enabled = true;
				if (customTab2.Enabled && customTab4.Enabled)
				{
					_loaded = true;
				}
			}).ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					Logger.Warn((Exception)t.Exception, "Guild Hall task failed.");
				}
			});
			Task.Run(async delegate
			{
				await LeftSideSection.PopulateHomesteadBigIconsInFlowPanel(homesteadDecorationsBigFlowPanel, _isIconView: false);
				await LeftSideSection.PopulateGuildHallBigIconsInFlowPanel(guildHallDecorationsBigFlowPanel, _isIconView: false);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)homesteadDecorationsBigFlowPanel, _originalHeightsHomesteadBig);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsBigFlowPanel, _originalHeightsGuildHallBig);
				await ApplySearchFilterAsync();
				customTab4.Enabled = true;
				if (customTab2.Enabled && customTab4.Enabled)
				{
					_loaded = true;
					((Control)checkbox).set_Visible(true);
					((Control)checkboxLoading).set_Visible(false);
					((Control)checkboxSpinner).set_Visible(false);
				}
			}).ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					Logger.Warn((Exception)t.Exception, "Image Preview task failed.");
				}
			});
			((TextInputBase)searchTextBox).add_TextChanged((EventHandler<EventArgs>)async delegate
			{
				await ApplySearchFilterAsync();
			});
			((Control)clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)searchTextBox).set_Text(string.Empty);
			});
			((Control)checkbox).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (_originalHeightsHomestead.Count != ((Container)_homesteadDecorationsFlowPanel).get_Children().get_Count())
				{
					LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)_homesteadDecorationsFlowPanel, _originalHeightsHomestead);
				}
				if (_originalHeightsGuildHall.Count != ((Container)guildHallDecorationsFlowPanel).get_Children().get_Count())
				{
					LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsFlowPanel, _originalHeightsGuildHall);
				}
				if (_originalHeightsHomesteadBig.Count != ((Container)homesteadDecorationsBigFlowPanel).get_Children().get_Count())
				{
					LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)homesteadDecorationsBigFlowPanel, _originalHeightsHomesteadBig);
				}
				if (_originalHeightsGuildHallBig.Count != ((Container)guildHallDecorationsBigFlowPanel).get_Children().get_Count())
				{
					LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsBigFlowPanel, _originalHeightsGuildHallBig);
				}
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)_homesteadDecorationsFlowPanel, _originalHeightsHomestead, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)guildHallDecorationsFlowPanel, _originalHeightsGuildHall, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)homesteadDecorationsBigFlowPanel, _originalHeightsHomesteadBig, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)guildHallDecorationsBigFlowPanel, _originalHeightsGuildHallBig, checkbox.get_Checked());
			});
			async Task ApplySearchFilterAsync()
			{
				string searchText = ((TextInputBase)searchTextBox).get_Text().ToLower();
				await FilterDecorations.FilterDecorationsAsync(_homesteadDecorationsFlowPanel, searchText, _isIconView: true);
				await FilterDecorations.FilterDecorationsAsync(guildHallDecorationsFlowPanel, searchText, _isIconView: true);
				await FilterDecorations.FilterDecorationsAsync(homesteadDecorationsBigFlowPanel, searchText, _isIconView: false);
				await FilterDecorations.FilterDecorationsAsync(guildHallDecorationsBigFlowPanel, searchText, _isIconView: false);
				((Control)clearButton).set_Visible(!string.IsNullOrEmpty(searchText));
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)_homesteadDecorationsFlowPanel, _originalHeightsHomestead);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsFlowPanel, _originalHeightsGuildHall);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)homesteadDecorationsBigFlowPanel, _originalHeightsHomesteadBig);
				LeftSideSection.StoreCurrentHeightsForPanel((Panel)(object)guildHallDecorationsBigFlowPanel, _originalHeightsGuildHallBig);
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)_homesteadDecorationsFlowPanel, _originalHeightsHomestead, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)guildHallDecorationsFlowPanel, _originalHeightsGuildHall, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)homesteadDecorationsBigFlowPanel, _originalHeightsHomesteadBig, checkbox.get_Checked());
				LeftSideSection.ApplyCheckboxLogic((Panel)(object)guildHallDecorationsBigFlowPanel, _originalHeightsGuildHallBig, checkbox.get_Checked());
			}
		}
	}
}
