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

		private Texture2D _homesteadIconHover;

		private Texture2D _homesteadIconUnactive;

		private Texture2D _handiworkTab;

		private Texture2D _scribeTab;

		private Texture2D _iconsTab;

		private Texture2D _imagesTab;

		private Texture2D _info;

		private Texture2D _x;

		private Texture2D _copy;

		private LoadingSpinner _loadingSpinner;

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

		public Texture2D X => _x;

		public Texture2D Info => _info;

		public bool Loaded => _loaded;

		public Texture2D CopyIcon => _copy;

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
			_handiworkTab = ContentsManager.GetTexture("test/handiwork.png");
			_scribeTab = ContentsManager.GetTexture("test/scribe.png");
			_iconsTab = ContentsManager.GetTexture("test/icons.png");
			_imagesTab = ContentsManager.GetTexture("test/images.png");
			_info = ContentsManager.GetTexture("test/info.png");
			_x = ContentsManager.GetTexture("test/x.png");
			_copy = ContentsManager.GetTexture("test/copy.png");
			_cornerIcon = CornerIconHelper.CreateLoadingIcon(_homesteadIconUnactive, _homesteadIconHover, _decorWindow, out _loadingSpinner);
			((Control)_loadingSpinner).set_Visible(true);
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			await CreateGw2StyleWindowThatDisplaysAllDecorations(windowBackgroundTexture);
			await InfoSection.InitializeInfoPanel();
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
			((Control)_loadingSpinner).set_Visible(false);
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
			LoadingSpinner loadingSpinner = _loadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
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
			Texture2D x = _x;
			if (x != null)
			{
				((GraphicsResource)x).Dispose();
			}
			Texture2D copy = _copy;
			if (copy != null)
			{
				((GraphicsResource)copy).Dispose();
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
			CustomTabbedWindow2 customTabbedWindow = new CustomTabbedWindow2(windowBackgroundTexture, new Rectangle(20, 26, 560, 640), new Rectangle(70, 40, 550, 640), new Point(1150, 800));
			((Control)customTabbedWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)customTabbedWindow).set_Title("Decor");
			((WindowBase2)customTabbedWindow).set_Emblem(_homesteadIconMenu);
			((WindowBase2)customTabbedWindow).set_Subtitle("Homestead Decorations");
			((Control)customTabbedWindow).set_Location(new Point(300, 300));
			((WindowBase2)customTabbedWindow).set_SavesPosition(true);
			((WindowBase2)customTabbedWindow).set_Id("DecorModule_Decoration_Window");
			decorModule._decorWindow = customTabbedWindow;
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)_decorWindow);
			((Control)val).set_Location(new Point(20, 0));
			((Control)val).set_Width(240);
			((TextInputBase)val).set_PlaceholderText("Search Decorations...");
			TextBox searchTextBox = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_decorWindow);
			((Control)val2).set_Location(new Point(((Control)searchTextBox).get_Right() - 22, ((Control)searchTextBox).get_Top() + 5));
			((Control)val2).set_Size(new Point(16, 16));
			((Control)val2).set_Visible(false);
			val2.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_x));
			Panel clearButton = val2;
			DecorModule decorModule2 = this;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)_decorWindow);
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val3).set_ShowBorder(true);
			((Control)val3).set_Width(500);
			((Control)val3).set_Height(660);
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val3).set_Visible(true);
			decorModule2._homesteadDecorationsFlowPanel = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)_decorWindow);
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val4).set_ShowBorder(true);
			((Control)val4).set_Width(500);
			((Control)val4).set_Height(660);
			((Panel)val4).set_CanScroll(true);
			((Control)val4).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val4).set_Visible(false);
			FlowPanel guildHallDecorationsFlowPanel = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)_decorWindow);
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val5).set_ShowBorder(true);
			((Control)val5).set_Width(1050);
			((Control)val5).set_Height(640);
			((Panel)val5).set_CanScroll(true);
			((Control)val5).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val5).set_Visible(false);
			FlowPanel homesteadDecorationsBigFlowPanel = val5;
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)_decorWindow);
			val6.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val6).set_ShowBorder(true);
			((Control)val6).set_Width(1050);
			((Control)val6).set_Height(640);
			((Panel)val6).set_CanScroll(true);
			((Control)val6).set_Location(new Point(10, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val6).set_Visible(false);
			FlowPanel guildHallDecorationsBigFlowPanel = val6;
			DecorModule decorModule3 = this;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_decorWindow);
			((Control)val7).set_Width(500);
			((Control)val7).set_Height(120);
			val7.set_WrapText(true);
			val7.set_StrokeText(true);
			val7.set_ShowShadow(true);
			val7.set_ShadowColor(new Color(0, 0, 0));
			val7.set_Font(GameService.Content.get_DefaultFont18());
			decorModule3._decorationRightText = val7;
			DecorModule decorModule4 = this;
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)_decorWindow);
			((Control)val8).set_Size(new Point(40, 40));
			((Control)val8).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationRightText).get_Bottom() + 5));
			decorModule4._decorationIcon = val8;
			DecorModule decorModule5 = this;
			Image val9 = new Image();
			((Control)val9).set_Parent((Container)(object)_decorWindow);
			((Control)val9).set_Size(new Point(400, 400));
			((Control)val9).set_Location(new Point(((Control)_decorationRightText).get_Left(), ((Control)_decorationIcon).get_Bottom() + 5));
			decorModule5._decorationImage = val9;
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
					((Control)_decorationRightText).set_Visible(true);
					((Control)_decorationImage).set_Visible(true);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(true);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					_wikiLicenseManager.UpdateWidthBasedOnFlowPanel(isBigView: false);
					InfoSection.UpdateInfoText("    Click on the name or the image\n            to copy its name.");
				}
				else if (selectedTabGroup == customTab2 && selectedTabGroup2 == customTab3)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Guild Hall Decorations");
					((Control)_decorationRightText).set_Visible(true);
					((Control)_decorationImage).set_Visible(true);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(true);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					_wikiLicenseManager.UpdateWidthBasedOnFlowPanel(isBigView: false);
					InfoSection.UpdateInfoText("    Click on the name or the image\n            to copy its name.");
				}
				else if (selectedTabGroup == customTab1 && selectedTabGroup2 == customTab4)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Homestead Decorations");
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(true);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(false);
					_wikiLicenseManager.UpdateWidthBasedOnFlowPanel(isBigView: true);
					InfoSection.UpdateInfoText("    Click on the image to zoom in.\nCopy icon copies the decoration name.");
				}
				else if (selectedTabGroup == customTab2 && selectedTabGroup2 == customTab4)
				{
					((WindowBase2)_decorWindow).set_Subtitle("Guild Hall Decorations");
					((Control)_decorationRightText).set_Visible(false);
					((Control)_decorationImage).set_Visible(false);
					((Control)_homesteadDecorationsFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsFlowPanel).set_Visible(false);
					((Control)homesteadDecorationsBigFlowPanel).set_Visible(false);
					((Control)guildHallDecorationsBigFlowPanel).set_Visible(true);
					_wikiLicenseManager.UpdateWidthBasedOnFlowPanel(isBigView: true);
					InfoSection.UpdateInfoText("    Click on the image to zoom in.\nCopy icon copies the decoration name.");
				}
			};
			customTab2.Enabled = false;
			customTab4.Enabled = false;
			await LeftSideSection.PopulateHomesteadIconsInFlowPanel(_homesteadDecorationsFlowPanel, _isIconView: true);
			Task guildHallTask = Task.Run(async delegate
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
			Task.WhenAll(guildHallTask);
			Task.WhenAll(imagePreviewTask);
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
