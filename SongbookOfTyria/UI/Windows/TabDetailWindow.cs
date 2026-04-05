using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;
using SongbookOfTyria.Settings;
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

		private readonly MidiPlaybackService _midiPlaybackService;

		private readonly MidiFileParser _midiFileParser;

		private readonly ModuleSettings _moduleSettings;

		private PracticeFeedbackService _practiceFeedbackService;

		private FlowPanel _leftPanel;

		private FlowPanel _rightPanel;

		private TabDetailsPanel _detailsSection;

		private ViewOptionsPanel _controlsSection;

		private Dropdown _modeDropdown;

		private FlowPanel _audioSection;

		private PracticeModePanel _practiceModePanel;

		private PianoKeybindsPanel _pianoKeybindsSection;

		private Panel _notationSection;

		private Panel _notationHeaderPanel;

		private Panel _notationContentPanel;

		private TrackSelectionPanel _trackSelectionPanel;

		private PianoKeybinds _pianoKeybinds;

		private NotationFontSize _currentFontSize = NotationFontSize.Size20;

		private TabViewMode _currentMode;

		private bool _detailsCollapsed;

		private bool _viewOptionsCollapsed;

		private bool _audioPlayerCollapsed;

		private bool _pianoKeybindsCollapsed;

		private bool _autoScrollEnabled;

		private float _scrollSpeed = 30f;

		private bool _hitDetectionEnabled;

		private float _accumulatedScrollOffset;

		private Scrollbar _cachedScrollbar;

		private NotationRenderer _notationRenderer;

		private List<ActiveNoteInfo> _pendingActiveNotes;

		private volatile bool _activeNotesDirty;

		private readonly Dictionary<int, DateTime> _noteHighlightStartTimes = new Dictionary<int, DateTime>();

		private const double HighlightMaxDurationMs = 500.0;

		private Dictionary<int, NoteFeedbackType> _pendingFeedback;

		private volatile bool _feedbackDirty;

		private volatile bool _markersDirty;

		public TabDetailWindow(MusicTab musicTab, TextureService textureService, AudioService audioService, UserSettingsService userSettingsService, ModuleSettings moduleSettings, MidiPlaybackService midiPlaybackService = null, string cacheDirectory = null)
			: this(AsyncTexture2D.FromAssetId(155985), new Rectangle(45, 25, 900, 700), new Rectangle(40, 25, 890, 650))
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			_musicTab = musicTab;
			_textureService = textureService;
			_audioService = audioService;
			_userSettingsService = userSettingsService;
			_moduleSettings = moduleSettings;
			_midiPlaybackService = midiPlaybackService;
			if (cacheDirectory != null)
			{
				_midiFileParser = new MidiFileParser(cacheDirectory);
			}
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(_textureService.GetEmblem()));
			RestoreSavedState();
			InitializeWindow();
			BuildLeftPanel();
			BuildRightPanel();
			ForceLayoutRefresh();
			if (_musicTab.HasPracticeMode && _midiPlaybackService != null)
			{
				InitializePracticeModeAsync();
			}
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
			_hitDetectionEnabled = false;
			_pianoKeybinds = _userSettingsService?.GetPianoKeybinds() ?? new PianoKeybinds();
			TabWindowState savedState = _userSettingsService?.GetTabWindowState(_musicTab.Id);
			if (savedState != null)
			{
				_currentFontSize = savedState.FontSize;
				_autoScrollEnabled = savedState.AutoScrollEnabled;
				_scrollSpeed = savedState.ScrollSpeed;
				if (savedState.IsPracticeMode && _musicTab.HasPracticeMode)
				{
					_currentMode = TabViewMode.Practice;
				}
			}
		}

		private void SaveWindowState()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			TabWindowState windowState = new TabWindowState
			{
				FontSize = _currentFontSize,
				AutoScrollEnabled = _autoScrollEnabled,
				ScrollSpeed = _scrollSpeed,
				IsPracticeMode = (_currentMode == TabViewMode.Practice)
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
			_midiPlaybackService?.Stop();
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
			BuildPracticeModeSection();
			BuildPianoKeybindsSection();
			BuildNotationSection();
			BuildNotationContent();
			UpdateModeVisibility();
		}

		private void BuildModeDropdown()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			if (_musicTab.HasPracticeMode && _notationHeaderPanel != null)
			{
				Dropdown val = new Dropdown();
				((Control)val).set_Width(110);
				((Control)val).set_Location(new Point(75, 1));
				((Control)val).set_Parent((Container)(object)_notationHeaderPanel);
				_modeDropdown = val;
				_modeDropdown.get_Items().Add("Normal");
				_modeDropdown.get_Items().Add("Practice");
				_modeDropdown.set_SelectedItem((_currentMode == TabViewMode.Normal) ? "Normal" : "Practice");
				_modeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnModeDropdownChanged);
			}
		}

		private void OnModeDropdownChanged(object sender, ValueChangedEventArgs e)
		{
			TabViewMode newMode = ((e.get_CurrentValue() == "Practice") ? TabViewMode.Practice : TabViewMode.Normal);
			if (newMode != _currentMode)
			{
				_currentMode = newMode;
				if (_currentMode == TabViewMode.Normal)
				{
					_midiPlaybackService?.Stop();
				}
				else
				{
					_audioService?.Stop();
				}
				_accumulatedScrollOffset = 0f;
				_cachedScrollbar = null;
				UpdateModeVisibility();
				ForceLayoutRefresh();
			}
		}

		private void UpdateModeVisibility()
		{
			bool isNormalMode = _currentMode == TabViewMode.Normal;
			bool isPracticeMode = !isNormalMode;
			SetControlVisible((Control)(object)_audioSection, isNormalMode);
			SetControlVisible((Control)(object)_practiceModePanel, isPracticeMode);
			_controlsSection?.SetPracticeModeActive(isPracticeMode);
			FlowPanel leftPanel = _leftPanel;
			if (leftPanel != null)
			{
				((Control)leftPanel).Invalidate();
			}
			if (_practiceFeedbackService != null)
			{
				_practiceFeedbackService.IsEnabled = isPracticeMode && _hitDetectionEnabled;
				if (isNormalMode)
				{
					_practiceFeedbackService.ClearFeedback();
					_notationRenderer?.Control?.ClearNoteFeedback();
				}
			}
			if (_trackSelectionPanel != null)
			{
				if (isPracticeMode)
				{
					AddTrackSelectionToNotationSection();
				}
				else
				{
					((Control)_trackSelectionPanel).set_Parent((Container)null);
					UpdateNotationContentPanelHeight();
				}
			}
			ReorderSectionsForMode(isNormalMode);
			UpdatePanelSizes();
		}

		private static void SetControlVisible(Control control, bool visible)
		{
			if (control != null)
			{
				control.set_Visible(visible);
			}
		}

		private void ReorderSectionsForMode(bool isNormalMode)
		{
			if (_pianoKeybindsSection != null && _practiceModePanel != null)
			{
				((Control)_pianoKeybindsSection).set_Parent((Container)null);
				((Control)_practiceModePanel).set_Parent((Container)null);
				((Control)_notationSection).set_Parent((Container)null);
				if (isNormalMode)
				{
					((Control)_practiceModePanel).set_Parent((Container)(object)_rightPanel);
					((Control)_pianoKeybindsSection).set_Parent((Container)(object)_rightPanel);
				}
				else
				{
					((Control)_pianoKeybindsSection).set_Parent((Container)(object)_rightPanel);
					((Control)_practiceModePanel).set_Parent((Container)(object)_rightPanel);
				}
				((Control)_notationSection).set_Parent((Container)(object)_rightPanel);
			}
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

		private void BuildPracticeModeSection()
		{
			if (_musicTab.HasPracticeMode && _midiPlaybackService != null)
			{
				MidiData midiData = _musicTab.MidiData;
				if (midiData != null && midiData.Tracks?.Count > 0)
				{
					CreatePracticeModePanel(_musicTab.MidiData);
				}
			}
		}

		private async Task InitializePracticeModeAsync()
		{
			try
			{
				MidiData midiData2 = _musicTab.MidiData;
				if ((midiData2 == null || !(midiData2.Tracks?.Count > 0)) && !string.IsNullOrEmpty(_musicTab.MidiFile) && _midiFileParser != null)
				{
					Logger.Debug("Downloading and parsing MIDI file: {0}", new object[1] { _musicTab.MidiFile });
					MidiData midiData = await _midiFileParser.ParseFromUrlAsync(_musicTab.MidiFile).ConfigureAwait(continueOnCapturedContext: false);
					if (midiData != null && midiData.Tracks?.Count > 0)
					{
						_musicTab.MidiData = midiData;
						CreatePracticeModePanel(midiData);
						UpdateModeVisibility();
						ForceLayoutRefresh();
					}
					else
					{
						Logger.Warn("MIDI file parsed but contained no tracks");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to initialize practice mode");
			}
		}

		private void CreatePracticeModePanel(MidiData midiData)
		{
			PracticeModePanel practiceModePanel = new PracticeModePanel(_midiPlaybackService, midiData, _textureService, ((Control)_rightPanel).get_Width() - 15, _userSettingsService, _musicTab.Id, _musicTab.PracticeSections);
			((Control)practiceModePanel).set_Parent((Container)(object)_rightPanel);
			((Control)practiceModePanel).set_Visible(_currentMode == TabViewMode.Practice);
			_practiceModePanel = practiceModePanel;
			_midiPlaybackService.ActiveNotesChanged += OnActiveNotesChanged;
			_practiceModePanel.CollapsedChanged += OnPlaybackCollapsedChanged;
			_practiceModePanel.MarkersChanged += OnMarkersChanged;
			_practiceFeedbackService = new PracticeFeedbackService(_midiPlaybackService, _moduleSettings);
			_practiceFeedbackService.FeedbackChanged += OnPracticeFeedbackChanged;
			_practiceFeedbackService.IsEnabled = _currentMode == TabViewMode.Practice && _hitDetectionEnabled;
			int savedTrackIndex = _practiceModePanel.SelectedTrackIndex;
			if (midiData != null && midiData.Tracks?.Count > 0)
			{
				int trackIndex = Math.Min(savedTrackIndex, midiData.Tracks.Count - 1);
				MidiTrack track = midiData.Tracks.FirstOrDefault((MidiTrack t) => t.Index == trackIndex) ?? midiData.Tracks[0];
				_practiceFeedbackService.SetActiveTrack(track);
			}
			CreateTrackSelectionPanel(midiData);
			if (_currentMode == TabViewMode.Practice && _trackSelectionPanel != null)
			{
				AddTrackSelectionToNotationSection();
			}
			if (_notationSection != null)
			{
				((Control)_notationSection).set_Parent((Container)null);
				((Control)_notationSection).set_Parent((Container)(object)_rightPanel);
			}
		}

		private void OnPlaybackCollapsedChanged(object sender, bool collapsed)
		{
			_userSettingsService?.SaveGlobalPlaybackCollapsed(collapsed);
		}

		private void CreateTrackSelectionPanel(MidiData midiData)
		{
			if (midiData?.Tracks != null && midiData.Tracks.Count != 0)
			{
				Panel notationSection = _notationSection;
				_trackSelectionPanel = new TrackSelectionPanel(midiData, (notationSection != null) ? ((Control)notationSection).get_Width() : (((Control)_rightPanel).get_Width() - 15 - 3));
				_trackSelectionPanel.TrackChanged += OnTrackSelectionChanged;
				int savedTrackIndex = _practiceModePanel?.SelectedTrackIndex ?? 0;
				if (savedTrackIndex > 0 && savedTrackIndex < midiData.Tracks.Count)
				{
					_trackSelectionPanel.SelectTrack(savedTrackIndex);
				}
			}
		}

		private void OnActiveNotesChanged(object sender, ActiveNoteEventArgs e)
		{
			_pendingActiveNotes = e.ActiveNotes;
			_activeNotesDirty = true;
		}

		private void OnMarkersChanged(object sender, EventArgs e)
		{
			_markersDirty = true;
		}

		private void OnPracticeFeedbackChanged(object sender, PracticeFeedbackEventArgs e)
		{
			_pendingFeedback = e.NoteFeedback?.ToDictionary((KeyValuePair<int, NoteFeedbackState> kvp) => kvp.Key, (KeyValuePair<int, NoteFeedbackState> kvp) => ConvertFeedbackState(kvp.Value));
			_feedbackDirty = true;
		}

		private static NoteFeedbackType ConvertFeedbackState(NoteFeedbackState state)
		{
			return state switch
			{
				NoteFeedbackState.Correct => NoteFeedbackType.Correct, 
				NoteFeedbackState.Wrong => NoteFeedbackType.Wrong, 
				NoteFeedbackState.Missed => NoteFeedbackType.Missed, 
				_ => NoteFeedbackType.None, 
			};
		}

		private void OnTrackSelectionChanged(object sender, int trackIndex)
		{
			_practiceModePanel?.SetSelectedTrackIndex(trackIndex);
			if (_practiceFeedbackService != null && _musicTab.MidiData?.Tracks != null)
			{
				MidiTrack track = _musicTab.MidiData.Tracks.FirstOrDefault((MidiTrack t) => t.Index == trackIndex);
				_practiceFeedbackService.SetActiveTrack(track);
			}
			RefreshNotationContent();
		}

		private int GetPracticeModeSectionHeight()
		{
			if (_currentMode != TabViewMode.Practice)
			{
				return 0;
			}
			PracticeModePanel practiceModePanel = _practiceModePanel;
			if (practiceModePanel == null)
			{
				return 0;
			}
			return ((Control)practiceModePanel).get_Height();
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

		private int GetTrackSelectionHeight()
		{
			return 0;
		}

		private void BuildNotationSection()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			int notationWidth = ((Control)_rightPanel).get_Width() - 15;
			int notationHeight = ((Control)_rightPanel).get_Height() - GetAudioSectionHeight() - GetPracticeModeSectionHeight() - GetPianoKeybindsSectionHeight();
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Width(notationWidth);
			((Control)val).set_Height(notationHeight);
			((Control)val).set_Parent((Container)(object)_rightPanel);
			_notationSection = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(notationWidth - 3);
			((Control)val2).set_Height(28);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Parent((Container)(object)_notationSection);
			_notationHeaderPanel = val2;
			Label val3 = new Label();
			val3.set_Text("Notation");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(10, 5));
			((Control)val3).set_Parent((Container)(object)_notationHeaderPanel);
			BuildModeDropdown();
			Panel val4 = new Panel();
			((Control)val4).set_Width(notationWidth - 3);
			((Control)val4).set_Height(notationHeight - 28);
			((Control)val4).set_Location(new Point(0, 28));
			val4.set_CanScroll(true);
			((Control)val4).set_BackgroundColor(Color.get_Black() * 0.3f);
			((Control)val4).set_Parent((Container)(object)_notationSection);
			_notationContentPanel = val4;
		}

		private void AddTrackSelectionToNotationSection()
		{
			if (_trackSelectionPanel != null && _notationHeaderPanel != null)
			{
				((Control)_trackSelectionPanel).set_Parent((Container)null);
				((Control)_trackSelectionPanel).remove_Resized((EventHandler<ResizedEventArgs>)OnTrackSelectionPanelResized);
				((Container)_trackSelectionPanel).set_HeightSizingMode((SizingMode)0);
				((Control)_trackSelectionPanel).set_Height(24);
				((Container)_trackSelectionPanel).set_WidthSizingMode((SizingMode)1);
				((Control)_trackSelectionPanel).add_Resized((EventHandler<ResizedEventArgs>)OnTrackSelectionPanelResized);
				((Control)_trackSelectionPanel).set_Parent((Container)(object)_notationHeaderPanel);
				RepositionTrackSelectionPanel();
			}
		}

		private void OnTrackSelectionPanelResized(object sender, ResizedEventArgs e)
		{
			RepositionTrackSelectionPanel();
		}

		private void RepositionTrackSelectionPanel()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_trackSelectionPanel != null && _notationHeaderPanel != null)
			{
				int rightX = ((Control)_notationHeaderPanel).get_Width() - ((Control)_trackSelectionPanel).get_Width() - 5;
				((Control)_trackSelectionPanel).set_Location(new Point(Math.Max(80, rightX), 2));
			}
		}

		private void UpdateNotationContentPanelHeight()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (_notationContentPanel != null && _notationSection != null)
			{
				((Control)_notationContentPanel).set_Height(((Control)_notationSection).get_Height() - 28);
				((Control)_notationContentPanel).set_Location(new Point(0, 28));
			}
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
			Panel notationContentPanel2 = _notationContentPanel;
			if (notationContentPanel2 != null)
			{
				((Control)notationContentPanel2).Invalidate();
			}
			RestoreScrollPosition(savedScrollOffset);
		}

		private void ClearNotationPanel()
		{
			if (_notationContentPanel != null)
			{
				Control[] array = ((Container)_notationContentPanel).get_Children().ToArray();
				foreach (Control obj in array)
				{
					obj.set_Parent((Container)null);
					obj.Dispose();
				}
				_notationRenderer = null;
			}
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
			if (_currentMode != 0)
			{
				return 0;
			}
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
			ViewOptionsPanel viewOptionsPanel = new ViewOptionsPanel(GetCurrentLeftPanelContentWidth(), _viewOptionsCollapsed, _currentFontSize, _autoScrollEnabled, _scrollSpeed, _hitDetectionEnabled, _textureService);
			((Control)viewOptionsPanel).set_Parent((Container)(object)_leftPanel);
			((Control)viewOptionsPanel).set_Visible(!_detailsCollapsed);
			_controlsSection = viewOptionsPanel;
			_controlsSection.FontSizeChanged += OnFontSizeChanged;
			_controlsSection.AutoScrollToggled += OnAutoScrollToggled;
			_controlsSection.ScrollSpeedChanged += OnScrollSpeedChanged;
			_controlsSection.HitDetectionToggled += OnHitDetectionToggled;
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

		private void OnHitDetectionToggled(object sender, bool enabled)
		{
			_hitDetectionEnabled = enabled;
			_userSettingsService?.SaveHitDetectionFeedbackEnabled(enabled);
			if (_practiceFeedbackService != null)
			{
				bool shouldBeEnabled = _currentMode == TabViewMode.Practice && enabled;
				_practiceFeedbackService.IsEnabled = shouldBeEnabled;
				if (!shouldBeEnabled)
				{
					_practiceFeedbackService.ClearFeedback();
					_notationRenderer?.Control?.ClearNoteFeedback();
				}
			}
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
			if (_notationContentPanel != null)
			{
				string notationText = GetActiveNotation();
				if (string.IsNullOrEmpty(notationText))
				{
					CreateNoNotationMessage();
				}
				else
				{
					RenderNotation(notationText, explicitWidth, explicitHeight);
				}
			}
		}

		private string GetActiveNotation()
		{
			if (_currentMode == TabViewMode.Practice && _trackSelectionPanel != null)
			{
				string trackNotation = _trackSelectionPanel.GetSelectedTrackNotation();
				if (!string.IsNullOrEmpty(trackNotation))
				{
					return ConvertTrackNotationToBlishHud(trackNotation, _musicTab.PracticeSections);
				}
			}
			return _musicTab.NotationBlishhud;
		}

		private void CreateNoNotationMessage()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (_notationContentPanel != null)
			{
				Label val = new Label();
				val.set_Text("No notation available for this tab.");
				val.set_Font(GameService.Content.get_DefaultFont14());
				val.set_AutoSizeWidth(true);
				val.set_AutoSizeHeight(true);
				((Control)val).set_Location(new Point(10, 10));
				((Control)val).set_Parent((Container)(object)_notationContentPanel);
			}
		}

		private void RenderNotation(string notation, int? explicitWidth = null, int? explicitHeight = null)
		{
			if (_notationContentPanel == null)
			{
				return;
			}
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
				if (_currentMode == TabViewMode.Practice)
				{
					UpdateNotationMarkers();
				}
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
			int notationSectionHeight = rightPanelHeight - GetAudioSectionHeight() - GetPracticeModeSectionHeight() - GetPianoKeybindsSectionHeight();
			if (_audioSection != null)
			{
				((Control)_audioSection).set_Width(sectionWidth);
			}
			if (_practiceModePanel != null)
			{
				((Control)_practiceModePanel).set_Width(sectionWidth);
			}
			if (_pianoKeybindsSection != null)
			{
				((Control)_pianoKeybindsSection).set_Width(sectionWidth);
			}
			UpdateNotationSection(sectionWidth, notationSectionHeight);
		}

		private void UpdateNotationSection(int sectionWidth, int notationSectionHeight)
		{
			if (_notationSection == null)
			{
				return;
			}
			((Control)_notationSection).set_Width(sectionWidth);
			((Control)_notationSection).set_Height(notationSectionHeight);
			int notationPanelWidth = sectionWidth - 3;
			if (_notationHeaderPanel != null)
			{
				((Control)_notationHeaderPanel).set_Width(notationPanelWidth);
			}
			if (_notationContentPanel != null)
			{
				int notationPanelHeight = notationSectionHeight - 28;
				((Control)_notationContentPanel).set_Width(notationPanelWidth);
				((Control)_notationContentPanel).set_Height(notationPanelHeight);
				if (_trackSelectionPanel != null && _currentMode == TabViewMode.Practice)
				{
					RepositionTrackSelectionPanel();
				}
				RefreshNotationContent(notationPanelWidth, notationPanelHeight);
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
			if (_currentMode == TabViewMode.Practice)
			{
				_practiceFeedbackService?.Update();
			}
			if (_feedbackDirty && _notationRenderer?.Control != null && _currentMode == TabViewMode.Practice)
			{
				_feedbackDirty = false;
				_notationRenderer.Control.SetNoteFeedback(_pendingFeedback);
			}
			if (_markersDirty && _notationRenderer?.Control != null && _currentMode == TabViewMode.Practice)
			{
				_markersDirty = false;
				UpdateNotationMarkers();
			}
			if (!_activeNotesDirty || _notationRenderer?.Control == null || _currentMode != TabViewMode.Practice)
			{
				return;
			}
			_activeNotesDirty = false;
			List<ActiveNoteInfo> notes = _pendingActiveNotes;
			DateTime now = DateTime.UtcNow;
			HashSet<int> filteredNoteIndices = new HashSet<int>();
			if (notes != null)
			{
				int selectedTrack = _trackSelectionPanel?.SelectedTrackIndex ?? 0;
				HashSet<int> currentTrackNotes = new HashSet<int>();
				foreach (ActiveNoteInfo note in notes)
				{
					if (note.TrackIndex == selectedTrack)
					{
						currentTrackNotes.Add(note.NoteIndex);
					}
				}
				foreach (int noteIdx in currentTrackNotes)
				{
					if (!_noteHighlightStartTimes.TryGetValue(noteIdx, out var startTime))
					{
						_noteHighlightStartTimes[noteIdx] = now;
						startTime = now;
					}
					if ((now - startTime).TotalMilliseconds < 500.0)
					{
						filteredNoteIndices.Add(noteIdx);
					}
				}
				List<int> expiredKeys = new List<int>();
				foreach (KeyValuePair<int, DateTime> kvp in _noteHighlightStartTimes)
				{
					if (!currentTrackNotes.Contains(kvp.Key))
					{
						expiredKeys.Add(kvp.Key);
					}
				}
				foreach (int key in expiredKeys)
				{
					_noteHighlightStartTimes.Remove(key);
				}
			}
			else
			{
				_noteHighlightStartTimes.Clear();
			}
			_notationRenderer.Control.SetHighlightedNoteIndices(filteredNoteIndices);
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

		private void UpdateNotationMarkers()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (_notationRenderer?.Control == null || _practiceModePanel == null)
			{
				return;
			}
			IReadOnlyList<MarkerInfo> markers = _practiceModePanel.GetMarkers();
			if (markers == null || markers.Count == 0)
			{
				_notationRenderer.Control.SetMarkers(null);
				return;
			}
			int selectedTrackIndex = _trackSelectionPanel?.SelectedTrackIndex ?? (-1);
			List<NotationMarker> notationMarkers = new List<NotationMarker>();
			foreach (MarkerInfo marker in markers)
			{
				int noteIndex = _practiceModePanel.GetNoteIndexForTime(marker.Time, selectedTrackIndex);
				notationMarkers.Add(new NotationMarker
				{
					NoteIndex = noteIndex,
					Color = marker.Color
				});
			}
			_notationRenderer.Control.SetMarkers(notationMarkers);
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
				_controlsSection.HitDetectionToggled -= OnHitDetectionToggled;
				_controlsSection.CollapsedChanged -= OnControlsSectionCollapsedChanged;
			}
			if (_audioSection != null)
			{
				((Control)_audioSection).remove_Resized((EventHandler<ResizedEventArgs>)OnAudioSectionResized);
			}
			if (_modeDropdown != null)
			{
				_modeDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnModeDropdownChanged);
			}
			if (_pianoKeybindsSection != null)
			{
				_pianoKeybindsSection.CollapsedChanged -= OnPianoKeybindsSectionCollapsedChanged;
			}
			if (_trackSelectionPanel != null)
			{
				_trackSelectionPanel.TrackChanged -= OnTrackSelectionChanged;
				((Control)_trackSelectionPanel).remove_Resized((EventHandler<ResizedEventArgs>)OnTrackSelectionPanelResized);
			}
			if (_midiPlaybackService != null)
			{
				_midiPlaybackService.ActiveNotesChanged -= OnActiveNotesChanged;
			}
			if (_practiceFeedbackService != null)
			{
				_practiceFeedbackService.FeedbackChanged -= OnPracticeFeedbackChanged;
				_practiceFeedbackService.Dispose();
			}
			_audioService?.Stop();
			_midiPlaybackService?.Stop();
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

		private static string ConvertTrackNotationToBlishHud(string trackNotation, PracticeSections practiceSections)
		{
			string[] array = trackNotation.Replace("\u200b", "").Split('\n');
			List<string> contentLines = new List<string>();
			bool skippedHeader = false;
			string[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				string trimmed = array2[j].Trim();
				if (!skippedHeader && (trimmed.EndsWith(":") || trimmed.Length == 0))
				{
					if (trimmed.EndsWith(":"))
					{
						skippedHeader = true;
					}
					continue;
				}
				skippedHeader = true;
				if (!string.IsNullOrEmpty(trimmed))
				{
					contentLines.Add(trimmed);
				}
			}
			string allContent = string.Join(" ", contentLines).Trim();
			allContent = Regex.Replace(allContent, "\\|\\s*\\|", "|");
			if (!allContent.StartsWith("|"))
			{
				allContent = "|" + allContent;
			}
			if (!allContent.EndsWith("|"))
			{
				allContent += "|";
			}
			List<string> bars = SplitIntoBars(allContent);
			if (bars.Count == 0)
			{
				return string.Empty;
			}
			int barsPerRow = ((practiceSections != null && practiceSections.BarsPerRow > 0) ? practiceSections.BarsPerRow : 4);
			Dictionary<int, string> sectionLookup = BuildSectionLookup(practiceSections);
			StringBuilder sb = new StringBuilder();
			int barsInCurrentRow = 0;
			for (int i = 0; i < bars.Count; i++)
			{
				string label;
				bool num = sectionLookup.TryGetValue(i, out label);
				bool isLastBar = i == bars.Count - 1;
				bool nextIsNewSection = !isLastBar && sectionLookup.ContainsKey(i + 1);
				if (num)
				{
					if (i > 0)
					{
						sb.AppendLine();
					}
					sb.AppendLine(FormatSectionLabel(label));
					barsInCurrentRow = 0;
				}
				else if (barsInCurrentRow == barsPerRow)
				{
					sb.AppendLine();
					barsInCurrentRow = 0;
				}
				sb.Append("<c=#6bff6b>|</c>");
				sb.Append(ColorizeNotationLine(bars[i]));
				barsInCurrentRow++;
				bool isEndOfRow = barsInCurrentRow == barsPerRow;
				if (isLastBar || isEndOfRow || nextIsNewSection)
				{
					sb.Append("<c=#6bff6b>|</c>");
				}
			}
			return sb.ToString().TrimEnd();
		}

		private static List<string> SplitIntoBars(string content)
		{
			List<string> bars = new List<string>();
			string[] parts = content.Split(new char[1] { '|' }, StringSplitOptions.None);
			for (int i = 0; i < parts.Length; i++)
			{
				if (!string.IsNullOrEmpty(parts[i]))
				{
					bars.Add(parts[i]);
				}
			}
			return bars;
		}

		private static Dictionary<int, string> BuildSectionLookup(PracticeSections practiceSections)
		{
			Dictionary<int, string> lookup = new Dictionary<int, string>();
			if (practiceSections?.Sections == null)
			{
				return lookup;
			}
			foreach (PracticeSection section in practiceSections.Sections)
			{
				int barIndex = section.Bar;
				if (!lookup.ContainsKey(barIndex))
				{
					lookup[barIndex] = section.Label;
				}
			}
			return lookup;
		}

		private static string FormatSectionLabel(string label)
		{
			return "<c=#ffffff>" + label + "</c>";
		}

		private static string ColorizeNotationLine(string line)
		{
			StringBuilder sb = new StringBuilder(line.Length * 2);
			int i = 0;
			while (i < line.Length)
			{
				char c = line[i];
				switch (c)
				{
				case '|':
					sb.Append("<c=#6bff6b>|</c>");
					i++;
					break;
				case '[':
				{
					int end2 = line.IndexOf(']', i);
					if (end2 > i)
					{
						string content2 = line.Substring(i, end2 - i + 1);
						sb.Append("<c=#6bb5ff>").Append(content2).Append("</c>");
						i = end2 + 1;
					}
					else
					{
						sb.Append(c);
						i++;
					}
					break;
				}
				case '(':
				{
					int end = line.IndexOf(')', i);
					if (end > i)
					{
						string content = line.Substring(i, end - i + 1);
						sb.Append("<c=#ff6b6b>").Append(content).Append("</c>");
						i = end + 1;
					}
					else
					{
						sb.Append(c);
						i++;
					}
					break;
				}
				default:
					sb.Append(c);
					i++;
					break;
				}
			}
			return sb.ToString();
		}
	}
}
