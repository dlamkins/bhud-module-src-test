using System;
using System.ComponentModel.Composition;
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
using DecorBlishhudModule.Sections;
using DecorBlishhudModule.Sections.LeftSideTasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule
{
	[Export(typeof(Module))]
	public class DecorModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

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

		private Texture2D _handiworkTab;

		private Texture2D _scribeTab;

		private Texture2D _iconsTab;

		private Texture2D _imagesTab;

		private Texture2D _info;

		private Texture2D _blackTexture;

		private Texture2D _x;

		private Texture2D _x2;

		private Texture2D _x2Active;

		private Texture2D _copy;

		private Texture2D _heart;

		private CustomTabbedWindow2 _decorWindow;

		private Image _decorationIcon;

		private Label _decorationRightText;

		private Image _decorationImage;

		private SignatureSection _signatureLabelManager;

		private WikiLicenseSection _wikiLicenseManager;

		private FlowPanel _homesteadDecorationsFlowPanel;

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

		public HttpClient Client => client;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

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
			_handiworkTab = ContentsManager.GetTexture("test/handiwork.png");
			_scribeTab = ContentsManager.GetTexture("test/scribe.png");
			_iconsTab = ContentsManager.GetTexture("test/icons.png");
			_imagesTab = ContentsManager.GetTexture("test/images.png");
			_info = ContentsManager.GetTexture("test/info.png");
			_blackTexture = ContentsManager.GetTexture("test/black_texture.png");
			_x = ContentsManager.GetTexture("test/x.png");
			_x2 = ContentsManager.GetTexture("test/x2.png");
			_x2Active = ContentsManager.GetTexture("test/x2_active.png");
			_copy = ContentsManager.GetTexture("test/copy.png");
			_heart = ContentsManager.GetTexture("test/heart.png");
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
			Texture2D info = _info;
			if (info != null)
			{
				((GraphicsResource)info).Dispose();
			}
			Texture2D blackTexture = _blackTexture;
			if (blackTexture != null)
			{
				((GraphicsResource)blackTexture).Dispose();
			}
			Texture2D x = _x;
			if (x != null)
			{
				((GraphicsResource)x).Dispose();
			}
			Texture2D x2 = _x2;
			if (x2 != null)
			{
				((GraphicsResource)x2).Dispose();
			}
			Texture2D x2Active = _x2Active;
			if (x2Active != null)
			{
				((GraphicsResource)x2Active).Dispose();
			}
			Texture2D copy = _copy;
			if (copy != null)
			{
				((GraphicsResource)copy).Dispose();
			}
			Texture2D heart = _heart;
			if (heart != null)
			{
				((GraphicsResource)heart).Dispose();
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
			DecorModule decorModule2 = this;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)_decorWindow);
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val4).set_ShowBorder(true);
			((Control)val4).set_Width(500);
			((Control)val4).set_Height(660);
			((Panel)val4).set_CanScroll(true);
			((Control)val4).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val4).set_Visible(true);
			decorModule2._homesteadDecorationsFlowPanel = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)_decorWindow);
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val5).set_ShowBorder(true);
			((Control)val5).set_Width(500);
			((Control)val5).set_Height(660);
			((Panel)val5).set_CanScroll(true);
			((Control)val5).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val5).set_Visible(false);
			FlowPanel guildHallDecorationsFlowPanel = val5;
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)_decorWindow);
			val6.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val6).set_ShowBorder(true);
			((Control)val6).set_Width(1080);
			((Control)val6).set_Height(660);
			((Panel)val6).set_CanScroll(true);
			((Control)val6).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val6).set_Visible(false);
			FlowPanel homesteadDecorationsBigFlowPanel = val6;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)_decorWindow);
			val7.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val7).set_ShowBorder(true);
			((Control)val7).set_Width(1080);
			((Control)val7).set_Height(660);
			((Panel)val7).set_CanScroll(true);
			((Control)val7).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val7).set_Visible(false);
			FlowPanel guildHallDecorationsBigFlowPanel = val7;
			DecorModule decorModule3 = this;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)_decorWindow);
			((Control)val8).set_Width(500);
			((Control)val8).set_Height(120);
			val8.set_WrapText(true);
			val8.set_StrokeText(true);
			val8.set_ShowShadow(true);
			val8.set_ShadowColor(new Color(0, 0, 0));
			val8.set_Font(GameService.Content.get_DefaultFont18());
			decorModule3._decorationRightText = val8;
			DecorModule decorModule4 = this;
			Image val9 = new Image();
			((Control)val9).set_Parent((Container)(object)_decorWindow);
			((Control)val9).set_Size(new Point(40, 40));
			((Control)val9).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationRightText).get_Bottom() + 5));
			decorModule4._decorationIcon = val9;
			DecorModule decorModule5 = this;
			Image val10 = new Image();
			((Control)val10).set_Parent((Container)(object)_decorWindow);
			((Control)val10).set_Size(new Point(400, 400));
			((Control)val10).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationIcon).get_Bottom() + 5));
			decorModule5._decorationImage = val10;
			CustomTab customTab1 = new CustomTab(AsyncTexture2D.op_Implicit(_handiworkTab), "Homestead Handiwork", 4);
			CustomTab customTab2 = new CustomTab(AsyncTexture2D.op_Implicit(_scribeTab), "Guild Hall Scribe", 3);
			CustomTab customTab3 = new CustomTab(AsyncTexture2D.op_Implicit(_iconsTab), "Icons Preview", 2);
			CustomTab customTab4 = new CustomTab(AsyncTexture2D.op_Implicit(_imagesTab), "Images Preview", 1);
			_decorWindow.TabsGroup1.Add(customTab1);
			_decorWindow.TabsGroup1.Add(customTab2);
			_decorWindow.TabsGroup2.Add(customTab3);
			_decorWindow.TabsGroup2.Add(customTab4);
			_decorWindow.SelectedTabGroup1 = _decorWindow.TabsGroup1.FirstOrDefault();
			_decorWindow.SelectedTabGroup2 = _decorWindow.TabsGroup2.FirstOrDefault();
			_ = _decorWindow.SelectedTabGroup1;
			_ = _decorWindow.SelectedTabGroup2;
			_decorWindow.TabChanged += delegate
			{
				CustomTab selectedTabGroup = _decorWindow.SelectedTabGroup1;
				CustomTab selectedTabGroup2 = _decorWindow.SelectedTabGroup2;
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
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: false);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: false);
					InfoSection.UpdateInfoText("    Click on the name or the image\n            to copy its name.");
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
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: false);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: false);
					InfoSection.UpdateInfoText("    Click on the name or the image\n            to copy its name.");
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
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoText("    Click on the image to zoom in.\nCopy icon copies the decoration name.");
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
					_wikiLicenseManager.UpdateFlowPanelPosition(isBigView: true);
					_signatureLabelManager.UpdateFlowPanelPosition(isBigView: true);
					InfoSection.UpdateInfoText("    Click on the image to zoom in.\nCopy icon copies the decoration name.");
				}
			};
			customTab2.Enabled = false;
			customTab4.Enabled = false;
			await LeftSideSection.PopulateHomesteadIconsInFlowPanel(_homesteadDecorationsFlowPanel, _isIconView: true);
			_cornerIcon.set_LoadingMessage((string)null);
			Task task = Task.Run(async delegate
			{
				await LeftSideSection.PopulateGuildHallIconsInFlowPanel(guildHallDecorationsFlowPanel, _isIconView: true);
				customTab2.Enabled = true;
				if (customTab2.Enabled && customTab4.Enabled)
				{
					_loaded = true;
				}
			});
			Task imagePreviewTask = Task.Run(async delegate
			{
				await LeftSideSection.PopulateHomesteadBigIconsInFlowPanel(homesteadDecorationsBigFlowPanel, _isIconView: false);
				await LeftSideSection.PopulateGuildHallBigIconsInFlowPanel(guildHallDecorationsBigFlowPanel, _isIconView: false);
				customTab4.Enabled = true;
				if (customTab2.Enabled && customTab4.Enabled)
				{
					_loaded = true;
				}
			});
			task.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					Logger.Warn((Exception)t.Exception, "Guild Hall task failed.");
				}
			});
			imagePreviewTask.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					Logger.Warn((Exception)t.Exception, "Image Preview task failed.");
				}
			});
			((TextInputBase)searchTextBox).add_TextChanged((EventHandler<EventArgs>)async delegate
			{
				string searchText = ((TextInputBase)searchTextBox).get_Text().ToLower();
				await FilterDecorations.FilterDecorationsAsync(_homesteadDecorationsFlowPanel, searchText, _isIconView: true);
				await FilterDecorations.FilterDecorationsAsync(guildHallDecorationsFlowPanel, searchText, _isIconView: true);
				await FilterDecorations.FilterDecorationsAsync(homesteadDecorationsBigFlowPanel, searchText, _isIconView: false);
				await FilterDecorations.FilterDecorationsAsync(guildHallDecorationsBigFlowPanel, searchText, _isIconView: false);
				((Control)clearButton).set_Visible(!string.IsNullOrEmpty(searchText));
			});
			((Control)clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)searchTextBox).set_Text(string.Empty);
			});
		}
	}
}
