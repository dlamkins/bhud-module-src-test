using System;
using System.Net;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Controls;
using SongbookOfTyria.UI.Controls.Notation;

namespace SongbookOfTyria.UI.Windows
{
	public class TabDetailWindow : StandardWindow
	{
		private static class PanelScrollHelper
		{
			private static readonly FieldInfo ScrollbarField = typeof(Panel).GetField("_panelScrollbar", BindingFlags.Instance | BindingFlags.NonPublic);

			private static readonly FieldInfo ScrollDistanceField = typeof(Scrollbar).GetField("_scrollDistance", BindingFlags.Instance | BindingFlags.NonPublic);

			private static readonly FieldInfo TargetScrollDistanceField = typeof(Scrollbar).GetField("_targetScrollDistance", BindingFlags.Instance | BindingFlags.NonPublic);

			public static Scrollbar GetScrollbar(Panel panel)
			{
				object obj = ScrollbarField?.GetValue(panel);
				return (Scrollbar)((obj is Scrollbar) ? obj : null);
			}

			public static void SetScrollPosition(Scrollbar scrollbar, float scrollPercent)
			{
				if (scrollbar != null)
				{
					ScrollDistanceField?.SetValue(scrollbar, scrollPercent);
					TargetScrollDistanceField?.SetValue(scrollbar, scrollPercent);
				}
			}
		}

		private const int WindowWidth = 935;

		private const int WindowHeight = 920;

		private const int WindowMaxWidth = 1920;

		private const int WindowMaxHeight = 1440;

		private const int LeftPanelWidth = 260;

		private const int LeftPanelCollapsedWidth = 45;

		private const int DefaultSpacing = 10;

		private const float ScrollSpeedMultiplier = 0.21f;

		private const int WindowBackgroundAssetId = 155985;

		private const string TabDetailWindowIdPrefix = "SongbookOfTyria_TabDetail_";

		private const int AudioSectionExpandedHeight = 105;

		private const int PianoKeybindsSectionExpandedHeight = 120;

		private const int CollapsedSectionHeight = 35;

		private const int NotationSectionHeaderHeight = 28;

		private const int SectionWidthPadding = 15;

		private const int NotationPanelPadding = 3;

		private static readonly Logger Logger = Logger.GetLogger<TabDetailWindow>();

		private readonly MusicTab _musicTab;

		private readonly TextureService _textureService;

		private readonly AudioService _audioService;

		private readonly UserSettingsService _userSettingsService;

		private FlowPanel _leftPanel;

		private FlowPanel _rightPanel;

		private TabDetailsPanel _detailsSection;

		private ViewOptionsPanel _controlsSection;

		private FlowPanel _audioSection;

		private PianoKeybindsPanel _pianoKeybindsSection;

		private FlowPanel _notationSection;

		private Panel _notationContentPanel;

		private PianoKeybinds _pianoKeybinds;

		private NotationFontSize _currentFontSize = NotationFontSize.Size20;

		private bool _detailsCollapsed;

		private bool _viewOptionsCollapsed;

		private bool _audioPlayerCollapsed;

		private bool _pianoKeybindsCollapsed;

		private bool _autoScrollEnabled;

		private float _scrollSpeed = 30f;

		private float _accumulatedScrollOffset;

		private Scrollbar _cachedScrollbar;

		private NotationRenderer _notationRenderer;

		public TabDetailWindow(MusicTab musicTab, TextureService textureService, AudioService audioService, UserSettingsService userSettingsService)
			: this(AsyncTexture2D.FromAssetId(155985), new Rectangle(45, 25, 900, 700), new Rectangle(40, 25, 890, 650))
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			_musicTab = musicTab;
			_textureService = textureService;
			_audioService = audioService;
			_userSettingsService = userSettingsService;
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(_textureService.GetEmblem()));
			RestoreSavedState();
			InitializeWindow();
			BuildLeftPanel();
			BuildRightPanel();
			ForceLayoutRefresh();
		}

		private void ForceLayoutRefresh()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y + 1));
			((Control)this).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 1));
		}

		private void RestoreSavedState()
		{
			_detailsCollapsed = _userSettingsService?.GetGlobalDetailsCollapsed() ?? false;
			_viewOptionsCollapsed = _userSettingsService?.GetGlobalViewOptionsCollapsed() ?? false;
			_audioPlayerCollapsed = _userSettingsService?.GetGlobalAudioPlayerCollapsed() ?? false;
			_pianoKeybindsCollapsed = _userSettingsService?.GetGlobalPianoKeybindsCollapsed() ?? false;
			_pianoKeybinds = _userSettingsService?.GetPianoKeybinds() ?? new PianoKeybinds();
			TabWindowState savedState = _userSettingsService?.GetTabWindowState(_musicTab.Id);
			if (savedState != null)
			{
				_currentFontSize = savedState.FontSize;
				_autoScrollEnabled = savedState.AutoScrollEnabled;
				_scrollSpeed = savedState.ScrollSpeed;
			}
		}

		private void SaveWindowState()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			TabWindowState windowState = new TabWindowState
			{
				FontSize = _currentFontSize,
				AutoScrollEnabled = _autoScrollEnabled,
				ScrollSpeed = _scrollSpeed
			};
			windowState.SetLocation(((Control)this).get_Location());
			windowState.SetSize(((Control)this).get_Size());
			_userSettingsService?.SaveTabWindowState(_musicTab.Id, windowState);
		}

		private void InitializeWindow()
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title(WebUtility.HtmlDecode(_musicTab.Name) ?? "Tab Details");
			((WindowBase2)this).set_Subtitle(string.Empty);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_Id(string.Format("{0}{1}", "SongbookOfTyria_TabDetail_", _musicTab.Id));
			TabWindowState savedState = _userSettingsService?.GetTabWindowState(_musicTab.Id);
			if (savedState != null && savedState.Width > 0 && savedState.Height > 0)
			{
				((Control)this).set_Location(savedState.GetLocation());
				((Control)this).set_Size(savedState.GetSize());
				((WindowBase2)this).set_SavesPosition(false);
			}
			else
			{
				((Control)this).set_Location(new Point(200, 150));
				((Control)this).set_Size(new Point(935, 920));
				((WindowBase2)this).set_SavesPosition(true);
			}
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnWindowResized);
			((Control)this).add_Hidden((EventHandler<EventArgs>)OnWindowHidden);
		}

		private void OnWindowHidden(object sender, EventArgs e)
		{
			_audioService?.Stop();
			SaveWindowState();
		}

		private void BuildLeftPanel()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			int initialWidth = (_detailsCollapsed ? 45 : 260);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(initialWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Location(new Point(10, 10));
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(0f, 5f));
			((Control)val).set_Parent((Container)(object)this);
			_leftPanel = val;
			BuildDetailsSection();
			BuildControlsSection();
		}

		private void BuildRightPanel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			int contentWidth = ((Container)this).get_ContentRegion().Width;
			int contentHeight = ((Container)this).get_ContentRegion().Height;
			int currentLeftWidth = (_detailsCollapsed ? 45 : 260);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(contentWidth - currentLeftWidth - 30);
			((Control)val).set_Height(contentHeight - 10);
			((Control)val).set_Location(new Point(currentLeftWidth + 20, 10));
			val.set_ControlPadding(new Vector2(0f, 0f));
			val.set_OuterControlPadding(new Vector2(5f, 0f));
			((Control)val).set_Parent((Container)(object)this);
			_rightPanel = val;
			BuildAudioSection();
			BuildPianoKeybindsSection();
			BuildNotationSection();
			BuildNotationContent();
		}

		private void BuildAudioSection()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(_musicTab.SongMp3))
			{
				FlowPanel val = new FlowPanel();
				((Panel)val).set_ShowBorder(true);
				((Panel)val).set_Title("Audio Player");
				((Panel)val).set_CanCollapse(true);
				((Control)val).set_Width(((Control)_rightPanel).get_Width() - 15);
				((Control)val).set_Height(105);
				((Container)val).set_HeightSizingMode((SizingMode)0);
				((Control)val).set_Parent((Container)(object)_rightPanel);
				val.set_FlowDirection((ControlFlowDirection)3);
				val.set_ControlPadding(new Vector2(0f, 0f));
				val.set_OuterControlPadding(new Vector2(10f, 20f));
				((Panel)val).set_Collapsed(_audioPlayerCollapsed);
				_audioSection = val;
				((Control)new Mp3PlayerControl(_audioService, _textureService, _musicTab.SongMp3)).set_Parent((Container)(object)_audioSection);
				((Control)_audioSection).add_Resized((EventHandler<ResizedEventArgs>)OnAudioSectionResized);
			}
		}

		private void BuildPianoKeybindsSection()
		{
			if (_musicTab.Piano)
			{
				PianoKeybindsPanel pianoKeybindsPanel = new PianoKeybindsPanel(((Control)_rightPanel).get_Width() - 15, _pianoKeybindsCollapsed, _pianoKeybinds, _userSettingsService, RefreshNotationContent);
				((Control)pianoKeybindsPanel).set_Parent((Container)(object)_rightPanel);
				_pianoKeybindsSection = pianoKeybindsPanel;
				_pianoKeybindsSection.CollapsedChanged += OnPianoKeybindsSectionCollapsedChanged;
			}
		}

		private void OnPianoKeybindsSectionCollapsedChanged(object sender, bool isCollapsed)
		{
			if (isCollapsed != _pianoKeybindsCollapsed)
			{
				_pianoKeybindsCollapsed = isCollapsed;
				_userSettingsService?.SaveGlobalPianoKeybindsCollapsed(_pianoKeybindsCollapsed);
			}
			_pianoKeybinds = _pianoKeybindsSection?.Keybinds ?? _pianoKeybinds;
			UpdatePanelSizes();
		}

		private int GetPianoKeybindsSectionHeight()
		{
			if (_pianoKeybindsSection != null)
			{
				if (!_pianoKeybindsCollapsed)
				{
					return 120;
				}
				return 35;
			}
			return 0;
		}

		private void BuildNotationSection()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			int notationWidth = ((Control)_rightPanel).get_Width() - 15;
			int notationHeight = ((Control)_rightPanel).get_Height() - GetAudioSectionHeight() - GetPianoKeybindsSectionHeight();
			FlowPanel val = new FlowPanel();
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_Title("Notation");
			((Control)val).set_Width(notationWidth);
			((Control)val).set_Height(notationHeight);
			((Control)val).set_Parent((Container)(object)_rightPanel);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 0f));
			val.set_OuterControlPadding(new Vector2(0f, 0f));
			_notationSection = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(notationWidth - 3);
			((Control)val2).set_Height(notationHeight - 28);
			val2.set_CanScroll(true);
			((Control)val2).set_BackgroundColor(Color.get_Black() * 0.3f);
			((Control)val2).set_Parent((Container)(object)_notationSection);
			_notationContentPanel = val2;
		}

		private void RefreshNotationContent(int? explicitWidth = null, int? explicitHeight = null)
		{
			_pianoKeybinds = _pianoKeybindsSection?.Keybinds ?? _pianoKeybinds;
			float num;
			if (!_autoScrollEnabled)
			{
				Panel notationContentPanel = _notationContentPanel;
				num = ((notationContentPanel != null) ? ((Container)notationContentPanel).get_VerticalScrollOffset() : 0);
			}
			else
			{
				num = _accumulatedScrollOffset;
			}
			float savedScrollOffset = num;
			_cachedScrollbar = null;
			ClearNotationPanel();
			BuildNotationContent(explicitWidth, explicitHeight);
			((Control)_notationContentPanel).Invalidate();
			RestoreScrollPosition(savedScrollOffset);
		}

		private void ClearNotationPanel()
		{
			Control[] array = ((Container)_notationContentPanel).get_Children().ToArray();
			foreach (Control obj in array)
			{
				obj.set_Parent((Container)null);
				obj.Dispose();
			}
			_notationRenderer = null;
		}

		private void RestoreScrollPosition(float savedScrollOffset)
		{
			if (savedScrollOffset <= 0f)
			{
				return;
			}
			if (_autoScrollEnabled)
			{
				_accumulatedScrollOffset = savedScrollOffset;
			}
			_cachedScrollbar = PanelScrollHelper.GetScrollbar(_notationContentPanel);
			if (_cachedScrollbar != null)
			{
				int scrollableHeight = GetScrollableHeight();
				if (scrollableHeight > 0)
				{
					PanelScrollHelper.SetScrollPosition(_cachedScrollbar, savedScrollOffset / (float)scrollableHeight);
				}
			}
		}

		private void RefreshNotationContent()
		{
			RefreshNotationContent(null, null);
		}

		private void UpdateLayoutForDetailsState()
		{
			int currentLeftWidth = (_detailsCollapsed ? 45 : 260);
			int contentWidth = GetCurrentLeftPanelContentWidth();
			((Control)_leftPanel).set_Width(currentLeftWidth);
			if (_detailsSection != null)
			{
				((Control)_detailsSection).set_Width(contentWidth);
				if (!_detailsCollapsed)
				{
					_detailsSection.RebuildContent();
				}
			}
			if (_controlsSection != null)
			{
				((Control)_controlsSection).set_Width(contentWidth);
				if (!_detailsCollapsed)
				{
					_controlsSection.RebuildContent();
				}
			}
			((Control)_leftPanel).Invalidate();
			UpdatePanelSizes();
		}

		private void OnAudioSectionResized(object sender, ResizedEventArgs e)
		{
			bool isNowCollapsed = ((Panel)_audioSection).get_Collapsed();
			if (isNowCollapsed != _audioPlayerCollapsed)
			{
				_audioPlayerCollapsed = isNowCollapsed;
				_userSettingsService?.SaveGlobalAudioPlayerCollapsed(_audioPlayerCollapsed);
			}
			UpdatePanelSizes();
		}

		private int GetAudioSectionHeight()
		{
			if (_audioSection != null)
			{
				if (!_audioPlayerCollapsed)
				{
					return 105;
				}
				return 35;
			}
			return 0;
		}

		private void BuildDetailsSection()
		{
			TabDetailsPanel tabDetailsPanel = new TabDetailsPanel(_musicTab, _textureService, GetCurrentLeftPanelContentWidth(), _detailsCollapsed);
			((Control)tabDetailsPanel).set_Parent((Container)(object)_leftPanel);
			_detailsSection = tabDetailsPanel;
			_detailsSection.CollapsedChanged += OnDetailsSectionCollapsedChanged;
		}

		private int GetCurrentLeftPanelContentWidth()
		{
			return (_detailsCollapsed ? 45 : 260) - 10;
		}

		private void OnDetailsSectionCollapsedChanged(object sender, bool isCollapsed)
		{
			if (isCollapsed != _detailsCollapsed)
			{
				_detailsCollapsed = isCollapsed;
				_userSettingsService?.SaveGlobalDetailsCollapsed(_detailsCollapsed);
				if (_controlsSection != null)
				{
					((Control)_controlsSection).set_Visible(!_detailsCollapsed);
				}
				UpdateLayoutForDetailsState();
			}
		}

		private void BuildControlsSection()
		{
			ViewOptionsPanel viewOptionsPanel = new ViewOptionsPanel(GetCurrentLeftPanelContentWidth(), _viewOptionsCollapsed, _currentFontSize, _autoScrollEnabled, _scrollSpeed, _textureService);
			((Control)viewOptionsPanel).set_Parent((Container)(object)_leftPanel);
			((Control)viewOptionsPanel).set_Visible(!_detailsCollapsed);
			_controlsSection = viewOptionsPanel;
			_controlsSection.FontSizeChanged += OnFontSizeChanged;
			_controlsSection.AutoScrollToggled += OnAutoScrollToggled;
			_controlsSection.ScrollSpeedChanged += OnScrollSpeedChanged;
			_controlsSection.CollapsedChanged += OnControlsSectionCollapsedChanged;
		}

		private void OnFontSizeChanged(object sender, NotationFontSize fontSize)
		{
			_currentFontSize = fontSize;
			RefreshNotationContent();
		}

		private void OnAutoScrollToggled(object sender, bool enabled)
		{
			_autoScrollEnabled = enabled;
			if (_notationRenderer?.Control != null)
			{
				_notationRenderer.Control.SmoothScrolling = _autoScrollEnabled;
			}
			if (_autoScrollEnabled && _notationContentPanel != null)
			{
				_accumulatedScrollOffset = ((Container)_notationContentPanel).get_VerticalScrollOffset();
				_cachedScrollbar = PanelScrollHelper.GetScrollbar(_notationContentPanel);
			}
		}

		private void OnScrollSpeedChanged(object sender, float speed)
		{
			_scrollSpeed = speed;
		}

		private void OnControlsSectionCollapsedChanged(object sender, bool isCollapsed)
		{
			if (isCollapsed != _viewOptionsCollapsed)
			{
				_viewOptionsCollapsed = isCollapsed;
				_userSettingsService?.SaveGlobalViewOptionsCollapsed(_viewOptionsCollapsed);
			}
		}

		private void BuildNotationContent(int? explicitWidth = null, int? explicitHeight = null)
		{
			string notationText = _musicTab.NotationBlishhud;
			if (string.IsNullOrEmpty(notationText))
			{
				CreateNoNotationMessage();
			}
			else
			{
				RenderNotation(notationText, explicitWidth, explicitHeight);
			}
		}

		private void CreateNoNotationMessage()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("No notation available for this tab.");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Parent((Container)(object)_notationContentPanel);
		}

		private void RenderNotation(string notation, int? explicitWidth = null, int? explicitHeight = null)
		{
			int width = explicitWidth ?? ((Control)_notationContentPanel).get_Width();
			int height = explicitHeight ?? ((Control)_notationContentPanel).get_Height();
			if (_musicTab.Piano && _pianoKeybinds != null)
			{
				notation = _pianoKeybinds.ApplyToNotation(notation);
			}
			_notationRenderer = new NotationRenderer(_notationContentPanel, _currentFontSize, width, height);
			_notationRenderer.Render(notation);
			if (_notationRenderer.Control != null)
			{
				_notationRenderer.Control.SmoothScrolling = _autoScrollEnabled;
				((Control)_notationContentPanel).Invalidate();
			}
		}

		private void OnWindowResized(object sender, ResizedEventArgs e)
		{
			UpdatePanelSizes();
		}

		private void UpdatePanelSizes()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			int contentWidth = ((Container)this).get_ContentRegion().Width;
			int contentHeight = ((Container)this).get_ContentRegion().Height;
			if (contentWidth > 0 && contentHeight > 0)
			{
				int currentLeftWidth = (_detailsCollapsed ? 45 : 260);
				int rightPanelWidth = contentWidth - currentLeftWidth - 30;
				int rightPanelHeight = contentHeight - 10;
				UpdateRightPanelLayout(currentLeftWidth, rightPanelWidth, rightPanelHeight);
				UpdateSectionSizes(rightPanelWidth, rightPanelHeight);
			}
		}

		private void UpdateRightPanelLayout(int currentLeftWidth, int rightPanelWidth, int rightPanelHeight)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (_rightPanel != null)
			{
				((Control)_rightPanel).set_Width(rightPanelWidth);
				((Control)_rightPanel).set_Height(rightPanelHeight);
				((Control)_rightPanel).set_Location(new Point(currentLeftWidth + 20, 10));
			}
		}

		private void UpdateSectionSizes(int rightPanelWidth, int rightPanelHeight)
		{
			int sectionWidth = rightPanelWidth - 15;
			int notationSectionHeight = rightPanelHeight - GetAudioSectionHeight() - GetPianoKeybindsSectionHeight();
			if (_audioSection != null)
			{
				((Control)_audioSection).set_Width(sectionWidth);
			}
			if (_pianoKeybindsSection != null)
			{
				((Control)_pianoKeybindsSection).set_Width(sectionWidth);
			}
			UpdateNotationSection(sectionWidth, notationSectionHeight);
		}

		private void UpdateNotationSection(int sectionWidth, int notationSectionHeight)
		{
			if (_notationSection != null)
			{
				((Control)_notationSection).set_Width(sectionWidth);
				((Control)_notationSection).set_Height(notationSectionHeight);
				if (_notationContentPanel != null)
				{
					int notationPanelWidth = sectionWidth - 3;
					int notationPanelHeight = notationSectionHeight - 28;
					((Control)_notationContentPanel).set_Width(notationPanelWidth);
					((Control)_notationContentPanel).set_Height(notationPanelHeight);
					RefreshNotationContent(notationPanelWidth, notationPanelHeight);
				}
			}
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, 935, 1920), MathHelper.Clamp(newSize.Y, 920, 1440));
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((WindowBase2)this).UpdateContainer(gameTime);
			SyncAudioSectionCollapseState();
			if (_autoScrollEnabled && _notationContentPanel != null)
			{
				UpdateAutoScroll(gameTime);
			}
		}

		private void SyncAudioSectionCollapseState()
		{
			if (_audioSection != null && ((Panel)_audioSection).get_Collapsed() != _audioPlayerCollapsed)
			{
				_audioPlayerCollapsed = ((Panel)_audioSection).get_Collapsed();
				_userSettingsService?.SaveGlobalAudioPlayerCollapsed(_audioPlayerCollapsed);
				UpdatePanelSizes();
			}
		}

		private int GetScrollableHeight()
		{
			int maxChildBottom = 0;
			foreach (Control child in ((Container)_notationContentPanel).get_Children())
			{
				if (child.get_Visible())
				{
					maxChildBottom = Math.Max(maxChildBottom, child.get_Bottom());
				}
			}
			return maxChildBottom - ((Control)_notationContentPanel).get_Height();
		}

		private void UpdateAutoScroll(GameTime gameTime)
		{
			int scrollableHeight = GetScrollableHeight();
			if (scrollableHeight > 0)
			{
				_accumulatedScrollOffset += _scrollSpeed * 0.21f * (float)gameTime.get_ElapsedGameTime().TotalSeconds;
				if (_accumulatedScrollOffset >= (float)scrollableHeight)
				{
					_accumulatedScrollOffset = 0f;
				}
				if (_cachedScrollbar == null)
				{
					_cachedScrollbar = PanelScrollHelper.GetScrollbar(_notationContentPanel);
				}
				if (_cachedScrollbar != null)
				{
					float scrollPercent = _accumulatedScrollOffset / (float)scrollableHeight;
					PanelScrollHelper.SetScrollPosition(_cachedScrollbar, scrollPercent);
				}
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnWindowResized);
			((Control)this).remove_Hidden((EventHandler<EventArgs>)OnWindowHidden);
			if (_detailsSection != null)
			{
				_detailsSection.CollapsedChanged -= OnDetailsSectionCollapsedChanged;
			}
			if (_controlsSection != null)
			{
				_controlsSection.FontSizeChanged -= OnFontSizeChanged;
				_controlsSection.AutoScrollToggled -= OnAutoScrollToggled;
				_controlsSection.ScrollSpeedChanged -= OnScrollSpeedChanged;
				_controlsSection.CollapsedChanged -= OnControlsSectionCollapsedChanged;
			}
			if (_audioSection != null)
			{
				((Control)_audioSection).remove_Resized((EventHandler<ResizedEventArgs>)OnAudioSectionResized);
			}
			if (_pianoKeybindsSection != null)
			{
				_pianoKeybindsSection.CollapsedChanged -= OnPianoKeybindsSectionCollapsedChanged;
			}
			_audioService?.Stop();
			FlowPanel leftPanel = _leftPanel;
			if (leftPanel != null)
			{
				((Control)leftPanel).Dispose();
			}
			FlowPanel rightPanel = _rightPanel;
			if (rightPanel != null)
			{
				((Control)rightPanel).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
