using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.UI.Controls
{
	public sealed class PracticeModePanel : FlowPanel
	{
		private sealed class TrackVolumeControl
		{
			public int TrackIndex { get; set; }

			public GlowButton MuteButton { get; set; }

			public TrackBar VolumeSlider { get; set; }

			public Label PercentLabel { get; set; }

			public float LastVolume { get; set; }

			public bool IsMuted
			{
				get
				{
					TrackBar volumeSlider = VolumeSlider;
					if (volumeSlider == null)
					{
						return false;
					}
					return volumeSlider.get_Value() == 0f;
				}
			}

			public float CurrentVolume
			{
				get
				{
					TrackBar volumeSlider = VolumeSlider;
					return ((volumeSlider != null) ? volumeSlider.get_Value() : 0f) / 100f;
				}
			}

			public void SetMuted(bool muted, AsyncTexture2D volumeIcon, AsyncTexture2D mutedIcon)
			{
				if (muted)
				{
					if (VolumeSlider != null)
					{
						VolumeSlider.set_Value(0f);
					}
					if (PercentLabel != null)
					{
						PercentLabel.set_Text("0%");
					}
				}
				else
				{
					float restoreVolume = ((LastVolume > 0f) ? LastVolume : 1f);
					if (VolumeSlider != null)
					{
						VolumeSlider.set_Value(restoreVolume * 100f);
					}
					if (PercentLabel != null)
					{
						PercentLabel.set_Text($"{(int)(restoreVolume * 100f)}%");
					}
				}
				UpdateMuteButtonVisuals(muted, volumeIcon, mutedIcon);
			}

			public void UpdateMuteButtonVisuals(bool muted, AsyncTexture2D volumeIcon, AsyncTexture2D mutedIcon)
			{
				if (MuteButton != null)
				{
					MuteButton.set_Icon(muted ? mutedIcon : volumeIcon);
					MuteButton.set_ActiveIcon(muted ? mutedIcon : volumeIcon);
					((Control)MuteButton).set_BasicTooltipText(muted ? "Unmute" : "Mute");
				}
			}

			public void UpdateFromVolumeChange(float value, AsyncTexture2D volumeIcon, AsyncTexture2D mutedIcon)
			{
				if (PercentLabel != null)
				{
					PercentLabel.set_Text($"{(int)value}%");
				}
				if (value > 0f)
				{
					LastVolume = value / 100f;
					UpdateMuteButtonVisuals(muted: false, volumeIcon, mutedIcon);
				}
				else
				{
					UpdateMuteButtonVisuals(muted: true, volumeIcon, mutedIcon);
				}
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<PracticeModePanel>();

		private const int PlayButtonSize = 32;

		private const int MuteButtonSize = 20;

		private const int VolumeRowHeight = 24;

		private const int VolumeLabelWidth = 55;

		private const int PercentLabelWidth = 32;

		private const int VolumeColumnWidth = 230;

		private const int ColumnGap = 10;

		private const int PlaybackControlsWidth = 160;

		private const int MinSliderWidth = 40;

		private const int MinSeekBarWidth = 50;

		private const int VolumeHeaderHeight = 28;

		private const int PlaybackHeaderHeight = 28;

		private const int SectionHeaderHeight = 28;

		private const int SectionPanelHeight = 85;

		private const int SeekBarHeight = 76;

		private const int ButtonSize = 32;

		private const int ButtonSpacing = 4;

		private const int TopMargin = 4;

		private const int RightMargin = 20;

		private const int PanelSpacing = 5;

		private const int MinPanelWidthForSideBySide = 200;

		private readonly MidiPlaybackService _playbackService;

		private readonly MidiData _midiData;

		private readonly PracticeSections _practiceSections;

		private readonly UserSettingsService _userSettingsService;

		private readonly int _tabId;

		private int _panelWidth;

		private readonly AsyncTexture2D _playTexture;

		private readonly AsyncTexture2D _pauseTexture;

		private readonly AsyncTexture2D _volumeTexture;

		private readonly AsyncTexture2D _volumeMutedTexture;

		private Panel _contentPanel;

		private Panel _playbackColumn;

		private Panel _playbackPanelContainer;

		private Panel _sectionsPanelContainer;

		private Panel _markersPanelContainer;

		private FlowPanel _sectionsPanel;

		private FlowPanel _markersPanel;

		private Panel _volumePanelContainer;

		private FlowPanel _volumePanel;

		private Panel _speedPanelContainer;

		private TrackBar _speedTrackBar;

		private Label _speedPercentLabel;

		private GlowButton _playPauseButton;

		private SectionedSeekBar _sectionedSeekBar;

		private TrackVolumeControl _masterVolumeControl;

		private readonly List<TrackVolumeControl> _trackControls = new List<TrackVolumeControl>();

		private readonly List<StandardButton> _sectionButtons = new List<StandardButton>();

		private readonly List<Panel> _markerButtons = new List<Panel>();

		private StandardButton _addMarkerButton;

		private Label _loadingLabel;

		private bool _disposed;

		private double _pendingPosition;

		private volatile bool _positionDirty;

		private volatile bool _pendingPlayState;

		private volatile bool _pendingStopState;

		private volatile bool _pendingFinished;

		private volatile bool _pendingSoundFontLoaded;

		private PracticeModeState _savedState;

		private bool _lastCollapsedState;

		private bool _isStackedLayout;

		public int SelectedTrackIndex => _savedState?.SelectedTrackIndex ?? 0;

		public event EventHandler<double> PositionUpdated;

		public event EventHandler<bool> CollapsedChanged;

		public event EventHandler MarkersChanged;

		public PracticeModePanel(MidiPlaybackService playbackService, MidiData midiData, TextureService textureService, int width, UserSettingsService userSettingsService = null, int tabId = 0, PracticeSections practiceSections = null)
			: this()
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			_playbackService = playbackService ?? throw new ArgumentNullException("playbackService");
			_midiData = midiData ?? throw new ArgumentNullException("midiData");
			_practiceSections = practiceSections;
			_userSettingsService = userSettingsService;
			_tabId = tabId;
			_panelWidth = width;
			_playTexture = textureService.GetPlayIcon();
			_pauseTexture = textureService.GetPauseIcon();
			_volumeTexture = textureService.GetVolumeIcon();
			_volumeMutedTexture = textureService.GetVolumeMutedIcon();
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_CanCollapse(true);
			((Panel)this).set_Title("Playback");
			((Control)this).set_Width(width);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 0f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(5f, 5f));
			_savedState = _userSettingsService?.GetPracticeModeState(_tabId);
			RestoreSavedState();
			_playbackService.LoadMidiData(midiData);
			_playbackService.PositionChanged += OnPositionChanged;
			_playbackService.PlaybackStarted += OnPlaybackStarted;
			_playbackService.PlaybackStopped += OnPlaybackStopped;
			_playbackService.PlaybackFinished += OnPlaybackFinished;
			_playbackService.SoundFontLoaded += OnSoundFontLoaded;
			if (_playbackService.IsSoundFontLoaded)
			{
				BuildTwoColumnLayout();
			}
			else
			{
				BuildLoadingMessage();
			}
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnPanelResized);
			_lastCollapsedState = _userSettingsService?.GetGlobalPlaybackCollapsed() ?? false;
			((Panel)this).set_Collapsed(_lastCollapsedState);
		}

		private void RestoreSavedState()
		{
			if (_savedState == null)
			{
				return;
			}
			_playbackService.SetMasterVolume(_savedState.MasterMuted ? 0f : _savedState.MasterVolume);
			_playbackService.SetPlaybackSpeed(_savedState.PlaybackSpeed);
			foreach (KeyValuePair<int, TrackVolumeState> kvp in _savedState.TrackStates)
			{
				_playbackService.SetTrackVolume(kvp.Key, kvp.Value.Volume);
				_playbackService.SetTrackMuted(kvp.Key, kvp.Value.Muted);
			}
		}

		private void BuildLoadingMessage()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Downloading sound data, please wait...");
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(10, 5));
			((Control)val).set_Parent((Container)(object)this);
			_loadingLabel = val;
		}

		private void OnSoundFontLoaded(object sender, EventArgs e)
		{
			_pendingSoundFontLoaded = true;
		}

		private void RestoreMarkers()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (_savedState?.Markers != null && _sectionedSeekBar != null)
			{
				List<MarkerInfo> markers = new List<MarkerInfo>();
				for (int i = 0; i < _savedState.Markers.Count; i++)
				{
					SavedMarker savedMarker = _savedState.Markers[i];
					markers.Add(new MarkerInfo
					{
						Time = savedMarker.Time,
						Color = GetMarkerColor(savedMarker.ColorIndex),
						Id = i + 1
					});
				}
				_sectionedSeekBar.SetMarkers(markers);
			}
		}

		private static Color GetMarkerColor(int index)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			Color[] colors = (Color[])(object)new Color[8]
			{
				new Color(46, 204, 113),
				new Color(52, 152, 219),
				new Color(231, 76, 60),
				new Color(241, 196, 15),
				new Color(155, 89, 182),
				new Color(230, 126, 34),
				new Color(26, 188, 156),
				new Color(236, 100, 165)
			};
			return colors[index % colors.Length];
		}

		private void SaveState()
		{
			if (_userSettingsService == null || _tabId == 0)
			{
				return;
			}
			PracticeModeState obj = new PracticeModeState
			{
				MasterVolume = (_masterVolumeControl?.LastVolume ?? 1f)
			};
			TrackVolumeControl masterVolumeControl = _masterVolumeControl;
			int masterMuted;
			if (masterVolumeControl == null)
			{
				masterMuted = 0;
			}
			else
			{
				TrackBar volumeSlider = masterVolumeControl.VolumeSlider;
				masterMuted = ((((volumeSlider != null) ? new float?(volumeSlider.get_Value()) : null) == 0f) ? 1 : 0);
			}
			obj.MasterMuted = (byte)masterMuted != 0;
			obj.SelectedTrackIndex = _savedState?.SelectedTrackIndex ?? 0;
			obj.PlaybackSpeed = _playbackService?.PlaybackSpeed ?? 1f;
			obj.TrackStates = new Dictionary<int, TrackVolumeState>();
			obj.Markers = new List<SavedMarker>();
			PracticeModeState state = obj;
			foreach (TrackVolumeControl control in _trackControls)
			{
				state.TrackStates[control.TrackIndex] = new TrackVolumeState
				{
					Volume = control.LastVolume,
					Muted = _playbackService.IsTrackMuted(control.TrackIndex)
				};
			}
			if (_sectionedSeekBar != null)
			{
				IReadOnlyList<MarkerInfo> markers = _sectionedSeekBar.GetMarkers();
				for (int i = 0; i < markers.Count; i++)
				{
					state.Markers.Add(new SavedMarker
					{
						Time = markers[i].Time,
						ColorIndex = i % 4
					});
				}
			}
			_savedState = state;
			_userSettingsService.SavePracticeModeState(_tabId, state);
		}

		public void SetSelectedTrackIndex(int trackIndex)
		{
			if (_savedState == null)
			{
				_savedState = new PracticeModeState();
			}
			_savedState.SelectedTrackIndex = trackIndex;
			SaveState();
		}

		public float GetPlaybackSpeed()
		{
			return _savedState?.PlaybackSpeed ?? 1f;
		}

		public void SetPlaybackSpeed(float speed)
		{
			_playbackService?.SetPlaybackSpeed(speed);
			SaveState();
		}

		private void BuildTwoColumnLayout()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Expected O, but got Unknown
			(_midiData?.Tracks?.Count).GetValueOrDefault();
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			_contentPanel = val;
			int volumeColumnWidth = Math.Min(230, _panelWidth / 3);
			int playbackColumnWidth = _panelWidth - volumeColumnWidth - 10 - 20;
			Panel val2 = new Panel();
			((Control)val2).set_Width(playbackColumnWidth);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Location(new Point(0, 4));
			((Control)val2).set_Parent((Container)(object)_contentPanel);
			_playbackColumn = val2;
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			((Control)val3).set_Width(volumeColumnWidth);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Location(new Point(playbackColumnWidth + 10, 4));
			((Control)val3).set_Parent((Container)(object)_contentPanel);
			_volumePanelContainer = val3;
			Label val4 = new Label();
			val4.set_Text("Volume");
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Location(new Point(10, 5));
			((Control)val4).set_Parent((Container)(object)_volumePanelContainer);
			FlowPanel val5 = new FlowPanel();
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_FlowDirection((ControlFlowDirection)3);
			val5.set_ControlPadding(new Vector2(0f, 2f));
			val5.set_OuterControlPadding(new Vector2(5f, 5f));
			((Control)val5).set_Location(new Point(0, 28));
			((Control)val5).set_Parent((Container)(object)_volumePanelContainer);
			_volumePanel = val5;
			BuildPlaybackControls();
			BuildVolumeControls(volumeColumnWidth);
			BuildSpeedControls(volumeColumnWidth);
			int height = ((Control)_playbackColumn).get_Height();
			Panel speedPanelContainer = _speedPanelContainer;
			int maxHeight = Math.Max(height, (speedPanelContainer != null) ? ((Control)speedPanelContainer).get_Bottom() : ((Control)_volumePanelContainer).get_Height());
			((Control)_contentPanel).set_Height(maxHeight + 10);
		}

		private void BuildPlaybackControls()
		{
			BuildSectionedSeekBar();
		}

		private void BuildSectionedSeekBar()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			List<SectionInfo> sections = CalculateSectionTimes();
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(109);
			((Control)val).set_Parent((Container)(object)_playbackColumn);
			_playbackPanelContainer = val;
			Label val2 = new Label();
			val2.set_Text("Control");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(10, 5));
			((Control)val2).set_Parent((Container)(object)_playbackPanelContainer);
			Panel val3 = new Panel();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Height(76);
			((Control)val3).set_Location(new Point(0, 28));
			((Control)val3).set_Parent((Container)(object)_playbackPanelContainer);
			Panel controlsPanel = val3;
			GlowButton val4 = new GlowButton();
			val4.set_Icon(_playTexture);
			val4.set_ActiveIcon(_playTexture);
			((Control)val4).set_BasicTooltipText("Play");
			((Control)val4).set_Size(new Point(32, 32));
			((Control)val4).set_Location(new Point(5, 27));
			((Control)val4).set_Parent((Container)(object)controlsPanel);
			_playPauseButton = val4;
			((Control)_playPauseButton).add_Click((EventHandler<MouseEventArgs>)OnPlayPauseClicked);
			SectionedSeekBar sectionedSeekBar = new SectionedSeekBar();
			((Control)sectionedSeekBar).set_Width(((Control)_playbackColumn).get_Width() - 32 - 30);
			((Control)sectionedSeekBar).set_Location(new Point(47, 0));
			sectionedSeekBar.Duration = _midiData?.Duration ?? 0.0;
			((Control)sectionedSeekBar).set_Parent((Container)(object)controlsPanel);
			_sectionedSeekBar = sectionedSeekBar;
			_sectionedSeekBar.SetSections(sections);
			_sectionedSeekBar.SeekRequested += OnSectionedSeekBarSeekRequested;
			_sectionedSeekBar.MarkerAdded += OnMarkerChanged;
			_sectionedSeekBar.MarkerRemoved += OnMarkerChanged;
			_sectionedSeekBar.MarkerMoved += OnMarkerChanged;
			RestoreMarkers();
			BuildSectionsAndMarkersRow();
		}

		private void BuildSectionsAndMarkersRow()
		{
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Expected O, but got Unknown
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Expected O, but got Unknown
			int rowTop = ((Control)_playbackPanelContainer).get_Bottom() + 5;
			int availableWidth = ((Control)_playbackColumn).get_Width();
			_isStackedLayout = availableWidth < 405;
			int sectionsTop = rowTop;
			IReadOnlyList<SectionInfo> sections = _sectionedSeekBar?.GetSections();
			int markerCount = (_savedState?.Markers?.Count).GetValueOrDefault() + 1;
			int sectionsPanelWidth;
			int markersPanelWidth;
			int markersTop;
			int markersLeft;
			if (_isStackedLayout)
			{
				sectionsPanelWidth = availableWidth;
				markersPanelWidth = availableWidth;
				int sectionsHeight = EstimateSectionsPanelHeight(sectionsPanelWidth, sections);
				markersTop = rowTop + sectionsHeight + 5;
				markersLeft = 0;
			}
			else
			{
				int totalWidth = availableWidth - 5;
				sectionsPanelWidth = Math.Max(80, (int)((double)totalWidth * 0.65));
				markersPanelWidth = Math.Max(80, totalWidth - sectionsPanelWidth);
				markersTop = rowTop;
				markersLeft = sectionsPanelWidth + 5;
			}
			int sectionsHeightFinal = EstimateSectionsPanelHeight(sectionsPanelWidth, sections);
			int markersHeightFinal = CalculatePanelHeight(markersPanelWidth, markerCount);
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Height(sectionsHeightFinal);
			((Control)val).set_Width(sectionsPanelWidth);
			((Control)val).set_Location(new Point(0, sectionsTop));
			((Control)val).set_Parent((Container)(object)_playbackColumn);
			_sectionsPanelContainer = val;
			Label val2 = new Label();
			val2.set_Text("Sections");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(10, 5));
			((Control)val2).set_Parent((Container)(object)_sectionsPanelContainer);
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)0);
			val3.set_ControlPadding(new Vector2(4f, 4f));
			val3.set_OuterControlPadding(new Vector2(4f, 4f));
			((Container)val3).set_HeightSizingMode((SizingMode)2);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Location(new Point(0, 28));
			((Control)val3).set_Parent((Container)(object)_sectionsPanelContainer);
			_sectionsPanel = val3;
			Panel val4 = new Panel();
			val4.set_ShowBorder(true);
			((Control)val4).set_Height(markersHeightFinal);
			((Control)val4).set_Width(markersPanelWidth);
			((Control)val4).set_Location(new Point(markersLeft, markersTop));
			((Control)val4).set_Parent((Container)(object)_playbackColumn);
			_markersPanelContainer = val4;
			Label val5 = new Label();
			val5.set_Text("Markers");
			val5.set_Font(GameService.Content.get_DefaultFont16());
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Location(new Point(10, 5));
			((Control)val5).set_Parent((Container)(object)_markersPanelContainer);
			FlowPanel val6 = new FlowPanel();
			val6.set_FlowDirection((ControlFlowDirection)0);
			val6.set_ControlPadding(new Vector2(4f, 4f));
			val6.set_OuterControlPadding(new Vector2(4f, 4f));
			((Container)val6).set_HeightSizingMode((SizingMode)2);
			((Container)val6).set_WidthSizingMode((SizingMode)2);
			((Control)val6).set_Location(new Point(0, 28));
			((Control)val6).set_Parent((Container)(object)_markersPanelContainer);
			_markersPanel = val6;
			StandardButton val7 = new StandardButton();
			val7.set_Text("Add");
			((Control)val7).set_Width(50);
			((Control)val7).set_Height(32);
			((Control)val7).set_BasicTooltipText("Add marker at current position");
			((Control)val7).set_Parent((Container)(object)_markersPanel);
			_addMarkerButton = val7;
			((Control)_addMarkerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_sectionedSeekBar?.AddMarker(_sectionedSeekBar.CurrentPosition);
			});
			BuildSectionButtons();
			RebuildMarkerButtons();
		}

		private void BuildSectionButtons()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			foreach (StandardButton sectionButton in _sectionButtons)
			{
				((Control)sectionButton).Dispose();
			}
			_sectionButtons.Clear();
			if (_sectionedSeekBar == null)
			{
				return;
			}
			foreach (SectionInfo section in _sectionedSeekBar.GetSections())
			{
				SectionInfo capturedSection = section;
				int buttonWidth = CalculateSectionButtonWidth(section.Label);
				StandardButton val = new StandardButton();
				val.set_Text(section.Label);
				((Control)val).set_Width(buttonWidth);
				((Control)val).set_Height(32);
				((Control)val).set_BasicTooltipText("Jump to " + section.Label);
				((Control)val).set_Parent((Container)(object)_sectionsPanel);
				StandardButton btn = val;
				((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnSectionedSeekBarSeekRequested(this, capturedSection.StartTime);
				});
				_sectionButtons.Add(btn);
			}
		}

		private int CalculateSectionButtonWidth(string label)
		{
			if (string.IsNullOrEmpty(label) || label.Length <= 2)
			{
				return 32;
			}
			int textWidth = label.Length * 8;
			return Math.Max(32, textWidth + 16);
		}

		private void RebuildMarkerButtons()
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			foreach (Panel markerButton in _markerButtons)
			{
				((Control)markerButton).Dispose();
			}
			_markerButtons.Clear();
			if (_addMarkerButton != null)
			{
				((Control)_addMarkerButton).set_Parent((Container)null);
			}
			if (_sectionedSeekBar == null)
			{
				return;
			}
			IReadOnlyList<MarkerInfo> markers = _sectionedSeekBar.GetMarkers();
			for (int i = 0; i < markers.Count; i++)
			{
				MarkerInfo marker = markers[i];
				int index = i;
				Panel val = new Panel();
				((Control)val).set_Width(32);
				((Control)val).set_Height(32);
				((Control)val).set_BackgroundColor(marker.Color * 0.3f);
				((Control)val).set_BasicTooltipText("Left-click: Jump to marker\nRight-click: Remove");
				((Control)val).set_Parent((Container)(object)_markersPanel);
				Panel markerBtn = val;
				Panel val2 = new Panel();
				((Control)val2).set_Width(12);
				((Control)val2).set_Height(12);
				((Control)val2).set_BackgroundColor(marker.Color);
				((Control)val2).set_Location(new Point(10, 10));
				((Control)val2).set_Parent((Container)(object)markerBtn);
				((Control)markerBtn).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					OnSectionedSeekBarSeekRequested(this, marker.Time);
				});
				((Control)markerBtn).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					_sectionedSeekBar?.RemoveMarker(index);
				});
				_markerButtons.Add(markerBtn);
			}
			if (_addMarkerButton != null)
			{
				((Control)_addMarkerButton).set_Parent((Container)(object)_markersPanel);
			}
		}

		private void OnMarkerChanged(object sender, MarkerInfo e)
		{
			RebuildMarkerButtons();
			Panel playbackColumn = _playbackColumn;
			UpdateSectionsAndMarkersLayout((playbackColumn != null) ? ((Control)playbackColumn).get_Width() : _panelWidth);
			SaveState();
			this.MarkersChanged?.Invoke(this, EventArgs.Empty);
		}

		public IReadOnlyList<MarkerInfo> GetMarkers()
		{
			return _sectionedSeekBar?.GetMarkers() ?? Array.Empty<MarkerInfo>();
		}

		public int GetNoteIndexForTime(double time)
		{
			return GetNoteIndexForTime(time, -1);
		}

		public int GetNoteIndexForTime(double time, int trackIndex)
		{
			MidiData midiData = _midiData;
			if (midiData != null && midiData.Duration <= 0.0)
			{
				return 0;
			}
			List<MidiNote> notes = GetTrackNotes(trackIndex);
			if (notes != null && notes.Count > 0)
			{
				for (int i = 0; i < notes.Count; i++)
				{
					if (notes[i].Time >= time)
					{
						return i;
					}
				}
				return notes.Count - 1;
			}
			int totalNotes = GetTotalNoteCount(trackIndex);
			if (totalNotes <= 0)
			{
				return 0;
			}
			return (int)(time / _midiData.Duration * (double)totalNotes);
		}

		private List<MidiNote> GetTrackNotes(int trackIndex)
		{
			if (trackIndex >= 0 && _midiData?.Tracks != null && trackIndex < _midiData.Tracks.Count)
			{
				return _midiData.Tracks[trackIndex].Notes;
			}
			MidiData midiData = _midiData;
			if (midiData != null && midiData.Tracks?.Count > 0)
			{
				return _midiData.Tracks[0].Notes;
			}
			return null;
		}

		private int GetTotalNoteCount(int trackIndex)
		{
			string notation = GetNotationForTrack(trackIndex);
			if (string.IsNullOrEmpty(notation))
			{
				return 0;
			}
			int count = 0;
			string text = notation;
			foreach (char c in text)
			{
				if ((c >= '1' && c <= '8') || (c >= '①' && c <= '⓿'))
				{
					count++;
				}
			}
			return count;
		}

		private string GetNotationForTrack(int trackIndex)
		{
			if (trackIndex < 0 || _midiData?.Tracks == null || trackIndex >= _midiData.Tracks.Count)
			{
				return null;
			}
			return _midiData.Tracks[trackIndex].Notation;
		}

		private List<SectionInfo> CalculateSectionTimes()
		{
			List<SectionInfo> sections = new List<SectionInfo>();
			if (_practiceSections?.Sections == null || _practiceSections.Sections.Count == 0)
			{
				return sections;
			}
			double duration = _midiData?.Duration ?? 0.0;
			if (duration <= 0.0)
			{
				return sections;
			}
			List<double> barTimes = CalculateBarTimes();
			Logger.Debug($"CalculateSectionTimes: duration={duration:F2}s, barTimes.Count={barTimes.Count}");
			for (int i = 0; i < _practiceSections.Sections.Count; i++)
			{
				PracticeSection section = _practiceSections.Sections[i];
				int barIndex = section.Bar;
				double startTime = GetTimeForBar(barIndex, barTimes, duration);
				double endTime;
				if (i + 1 < _practiceSections.Sections.Count)
				{
					int nextBarIndex = _practiceSections.Sections[i + 1].Bar;
					endTime = GetTimeForBar(nextBarIndex, barTimes, duration);
				}
				else
				{
					endTime = duration;
				}
				Logger.Debug($"Section '{section.Label}': Bar={section.Bar} -> barIndex={barIndex}, startTime={startTime:F2}s, endTime={endTime:F2}s");
				sections.Add(new SectionInfo
				{
					Label = section.Label,
					StartTime = Math.Min(startTime, duration),
					EndTime = Math.Min(endTime, duration)
				});
			}
			return sections;
		}

		private double GetTimeForBar(int barIndex, List<double> barTimes, double duration)
		{
			if (barTimes == null || barTimes.Count == 0)
			{
				double timePerBar = CalculateTimePerBar();
				return Math.Min((double)barIndex * timePerBar, duration);
			}
			if (barIndex < barTimes.Count)
			{
				return barTimes[barIndex];
			}
			if (barTimes.Count >= 2)
			{
				double num = barTimes[barTimes.Count - 1];
				double avgBarDuration = num / (double)(barTimes.Count - 1);
				int extraBars = barIndex - (barTimes.Count - 1);
				return Math.Min(num + (double)extraBars * avgBarDuration, duration);
			}
			return Math.Min((double)barIndex * CalculateTimePerBar(), duration);
		}

		private List<double> CalculateBarTimes()
		{
			List<double> barTimes = new List<double>();
			if (_midiData == null)
			{
				return barTimes;
			}
			int ppq = ((_midiData.Ppq > 0) ? _midiData.Ppq : 480);
			double duration = _midiData.Duration;
			List<MidiTempo> tempos = _midiData.Tempos ?? new List<MidiTempo>();
			List<MidiTimeSignature> timeSignatures = _midiData.TimeSignatures ?? new List<MidiTimeSignature>();
			if (tempos.Count == 0)
			{
				tempos = new List<MidiTempo>
				{
					new MidiTempo
					{
						Ticks = 0,
						Bpm = 120.0
					}
				};
			}
			if (timeSignatures.Count == 0)
			{
				timeSignatures = new List<MidiTimeSignature>
				{
					new MidiTimeSignature
					{
						Ticks = 0,
						Numerator = 4,
						Denominator = 4
					}
				};
			}
			int tempoIndex = 0;
			int tsIndex = 0;
			double currentTime = 0.0;
			int currentTick = 0;
			int maxBars = (int)(duration / 0.5) + 10;
			for (int bar = 0; bar < maxBars; bar++)
			{
				if (!(currentTime < duration))
				{
					break;
				}
				barTimes.Add(currentTime);
				for (; tempoIndex + 1 < tempos.Count && tempos[tempoIndex + 1].Ticks <= currentTick; tempoIndex++)
				{
				}
				for (; tsIndex + 1 < timeSignatures.Count && timeSignatures[tsIndex + 1].Ticks <= currentTick; tsIndex++)
				{
				}
				MidiTempo currentTempo = tempos[tempoIndex];
				MidiTimeSignature midiTimeSignature = timeSignatures[tsIndex];
				int beatsPerBar = midiTimeSignature.Numerator;
				int beatUnit = midiTimeSignature.Denominator;
				int ticksPerBeat = ppq * 4 / beatUnit;
				int ticksPerBar = beatsPerBar * ticksPerBeat;
				double secondsPerTick = 60.0 / (currentTempo.Bpm * (double)ppq);
				double barDuration = (double)ticksPerBar * secondsPerTick;
				currentTick += ticksPerBar;
				currentTime += barDuration;
			}
			return barTimes;
		}

		private double CalculateTimePerBar()
		{
			if (_midiData?.Tempos == null || _midiData.Tempos.Count == 0 || _midiData?.TimeSignatures == null || _midiData.TimeSignatures.Count == 0)
			{
				return 2.0;
			}
			MidiTempo tempo = _midiData.Tempos[0];
			int numerator = _midiData.TimeSignatures[0].Numerator;
			double secondsPerBeat = 60.0 / tempo.Bpm;
			return (double)numerator * secondsPerBeat;
		}

		private void OnSectionedSeekBarSeekRequested(object sender, double seekTime)
		{
			_playbackService.Seek(seekTime);
		}

		private void BuildVolumeControls(int columnWidth)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Expected O, but got Unknown
			int sliderWidth = Math.Max(40, columnWidth - 55 - 20 - 32 - 30);
			float savedMasterVolume = _savedState?.MasterVolume ?? 1f;
			bool savedMasterMuted = _savedState?.MasterMuted ?? false;
			float currentMasterValue = (savedMasterMuted ? 0f : (savedMasterVolume * 100f));
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(24);
			((Control)val).set_Parent((Container)(object)_volumePanel);
			Panel masterRow = val;
			Label val2 = new Label();
			val2.set_Text("Master");
			val2.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val2).set_Width(55);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(0, 5));
			((Control)val2).set_Parent((Container)(object)masterRow);
			GlowButton val3 = new GlowButton();
			val3.set_Icon(savedMasterMuted ? _volumeMutedTexture : _volumeTexture);
			val3.set_ActiveIcon(savedMasterMuted ? _volumeMutedTexture : _volumeTexture);
			((Control)val3).set_BasicTooltipText(savedMasterMuted ? "Unmute" : "Mute");
			((Control)val3).set_Size(new Point(20, 20));
			((Control)val3).set_Location(new Point(55, 2));
			((Control)val3).set_Parent((Container)(object)masterRow);
			GlowButton masterMuteButton = val3;
			TrackBar val4 = new TrackBar();
			val4.set_MinValue(0f);
			val4.set_MaxValue(100f);
			val4.set_Value((float)(int)currentMasterValue);
			val4.set_SmallStep(true);
			((Control)val4).set_Width(sliderWidth);
			((Control)val4).set_Height(16);
			((Control)val4).set_Location(new Point(80, 4));
			((Control)val4).set_Parent((Container)(object)masterRow);
			TrackBar masterSlider = val4;
			Label val5 = new Label();
			val5.set_Text($"{(int)masterSlider.get_Value()}%");
			val5.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val5).set_Width(32);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Location(new Point(((Control)masterSlider).get_Right() + 3, 5));
			((Control)val5).set_Parent((Container)(object)masterRow);
			Label masterPercentLabel = val5;
			_masterVolumeControl = new TrackVolumeControl
			{
				TrackIndex = -1,
				MuteButton = masterMuteButton,
				VolumeSlider = masterSlider,
				PercentLabel = masterPercentLabel,
				LastVolume = savedMasterVolume
			};
			((Control)masterMuteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OnMasterMuteClicked();
			});
			masterSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
			{
				OnMasterVolumeChanged(s, e);
			});
			BuildTrackVolumeControls(sliderWidth);
		}

		private void BuildTrackVolumeControls(int sliderWidth)
		{
			if (_midiData?.Tracks == null || _midiData.Tracks.Count == 0)
			{
				return;
			}
			foreach (MidiTrack track in _midiData.Tracks)
			{
				TrackVolumeControl trackControl = CreateTrackVolumeControl(track, sliderWidth);
				_trackControls.Add(trackControl);
			}
		}

		private void BuildSpeedControls(int columnWidth)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Expected O, but got Unknown
			float savedSpeed = _savedState?.PlaybackSpeed ?? 1f;
			int playbackColumnWidth = _panelWidth - columnWidth - 10 - 20;
			int volumePanelHeight = CalculateVolumePanelHeight();
			Panel val = new Panel();
			val.set_ShowBorder(true);
			((Control)val).set_Width(columnWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Location(new Point(playbackColumnWidth + 10, 4 + volumePanelHeight + 5));
			((Control)val).set_Parent((Container)(object)_contentPanel);
			_speedPanelContainer = val;
			Label val2 = new Label();
			val2.set_Text("Speed");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(10, 5));
			((Control)val2).set_Parent((Container)(object)_speedPanelContainer);
			Panel val3 = new Panel();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Height(34);
			((Control)val3).set_Location(new Point(0, 28));
			((Control)val3).set_Parent((Container)(object)_speedPanelContainer);
			Panel speedPanel = val3;
			int sliderWidth = Math.Max(40, columnWidth - 32 - 20);
			TrackBar val4 = new TrackBar();
			val4.set_MinValue(50f);
			val4.set_MaxValue(100f);
			val4.set_Value(savedSpeed * 100f);
			val4.set_SmallStep(true);
			((Control)val4).set_Width(sliderWidth);
			((Control)val4).set_Height(16);
			((Control)val4).set_Location(new Point(5, 4));
			((Control)val4).set_Parent((Container)(object)speedPanel);
			_speedTrackBar = val4;
			_speedTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSpeedTrackBarChanged);
			Label val5 = new Label();
			val5.set_Text($"{(int)(savedSpeed * 100f)}%");
			val5.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val5).set_Width(32);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Location(new Point(((Control)_speedTrackBar).get_Right() + 3, 5));
			((Control)val5).set_Parent((Container)(object)speedPanel);
			_speedPercentLabel = val5;
		}

		private int CalculateVolumePanelHeight()
		{
			int rowCount = (_midiData?.Tracks?.Count).GetValueOrDefault() + 1;
			int flowPanelPadding = 10;
			int rowSpacing = 2;
			int contentHeight = rowCount * 24 + (rowCount - 1) * rowSpacing + flowPanelPadding;
			return 28 + contentHeight;
		}

		private void OnSpeedTrackBarChanged(object sender, ValueEventArgs<float> e)
		{
			float speed = e.get_Value() / 100f;
			_playbackService?.SetPlaybackSpeed(speed);
			if (_speedPercentLabel != null)
			{
				_speedPercentLabel.set_Text($"{(int)e.get_Value()}%");
			}
			SaveState();
		}

		private TrackVolumeControl CreateTrackVolumeControl(MidiTrack track, int sliderWidth)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Expected O, but got Unknown
			int trackIndex = track.Index;
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(24);
			((Control)val).set_Parent((Container)(object)_volumePanel);
			Panel trackRow = val;
			Label val2 = new Label();
			val2.set_Text(track.GetDisplayName());
			val2.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val2).set_Width(55);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(0, 5));
			((Control)val2).set_Parent((Container)(object)trackRow);
			TrackVolumeState savedTrackState = null;
			_savedState?.TrackStates?.TryGetValue(trackIndex, out savedTrackState);
			float savedTrackVolume = savedTrackState?.Volume ?? 1f;
			bool savedTrackMuted = savedTrackState?.Muted ?? false;
			GlowButton val3 = new GlowButton();
			val3.set_Icon(savedTrackMuted ? _volumeMutedTexture : _volumeTexture);
			val3.set_ActiveIcon(savedTrackMuted ? _volumeMutedTexture : _volumeTexture);
			((Control)val3).set_BasicTooltipText(savedTrackMuted ? "Unmute" : "Mute");
			((Control)val3).set_Size(new Point(20, 20));
			((Control)val3).set_Location(new Point(55, 2));
			((Control)val3).set_Parent((Container)(object)trackRow);
			GlowButton muteButton = val3;
			TrackBar val4 = new TrackBar();
			val4.set_MinValue(0f);
			val4.set_MaxValue(100f);
			val4.set_Value(savedTrackVolume * 100f);
			val4.set_SmallStep(true);
			((Control)val4).set_Width(sliderWidth);
			((Control)val4).set_Height(16);
			((Control)val4).set_Location(new Point(80, 4));
			((Control)val4).set_Parent((Container)(object)trackRow);
			TrackBar volumeSlider = val4;
			Label val5 = new Label();
			val5.set_Text($"{(int)volumeSlider.get_Value()}%");
			val5.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val5).set_Width(32);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Location(new Point(((Control)volumeSlider).get_Right() + 3, 5));
			((Control)val5).set_Parent((Container)(object)trackRow);
			Label percentLabel = val5;
			TrackVolumeControl trackControl = new TrackVolumeControl
			{
				TrackIndex = trackIndex,
				MuteButton = muteButton,
				VolumeSlider = volumeSlider,
				PercentLabel = percentLabel,
				LastVolume = savedTrackVolume
			};
			((Control)muteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OnTrackMuteClicked(trackControl);
			});
			volumeSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
			{
				OnTrackVolumeChanged(trackControl, e.get_Value());
			});
			return trackControl;
		}

		private void OnMasterMuteClicked()
		{
			if (_masterVolumeControl != null)
			{
				bool isMuted = _masterVolumeControl.IsMuted;
				if (!isMuted)
				{
					_masterVolumeControl.LastVolume = _masterVolumeControl.CurrentVolume;
				}
				_masterVolumeControl.SetMuted(!isMuted, _volumeTexture, _volumeMutedTexture);
				_playbackService.SetMasterVolume(isMuted ? _masterVolumeControl.CurrentVolume : 0f);
				SaveState();
			}
		}

		private void OnTrackMuteClicked(TrackVolumeControl control)
		{
			bool isMuted = _playbackService.IsTrackMuted(control.TrackIndex);
			if (!isMuted)
			{
				control.LastVolume = control.CurrentVolume;
			}
			_playbackService.SetTrackMuted(control.TrackIndex, !isMuted);
			control.SetMuted(!isMuted, _volumeTexture, _volumeMutedTexture);
			SaveState();
		}

		private void OnTrackVolumeChanged(TrackVolumeControl control, float value)
		{
			float volume = value / 100f;
			_playbackService.SetTrackVolume(control.TrackIndex, volume);
			if (volume > 0f && _playbackService.IsTrackMuted(control.TrackIndex))
			{
				_playbackService.SetTrackMuted(control.TrackIndex, muted: false);
			}
			control.UpdateFromVolumeChange(value, _volumeTexture, _volumeMutedTexture);
			SaveState();
		}

		private void OnMasterVolumeChanged(object sender, ValueEventArgs<float> e)
		{
			_playbackService.SetMasterVolume(e.get_Value() / 100f);
			_masterVolumeControl?.UpdateFromVolumeChange(e.get_Value(), _volumeTexture, _volumeMutedTexture);
			SaveState();
		}

		private void OnPanelResized(object sender, ResizedEventArgs e)
		{
			CheckCollapsedStateChanged();
			if (((Control)this).get_Width() != _panelWidth && ((Control)this).get_Width() > 0)
			{
				_panelWidth = ((Control)this).get_Width();
				UpdateControlLayout();
			}
		}

		private void CheckCollapsedStateChanged()
		{
			if (((Panel)this).get_Collapsed() != _lastCollapsedState)
			{
				_lastCollapsedState = ((Panel)this).get_Collapsed();
				this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
			}
		}

		private void UpdateControlLayout()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			int volumeColumnWidth = Math.Min(230, _panelWidth / 3);
			int playbackColumnWidth = _panelWidth - volumeColumnWidth - 10 - 20;
			if (_playbackColumn != null)
			{
				((Control)_playbackColumn).set_Width(playbackColumnWidth);
			}
			if (_volumePanelContainer != null)
			{
				((Control)_volumePanelContainer).set_Width(volumeColumnWidth);
				((Control)_volumePanelContainer).set_Location(new Point(playbackColumnWidth + 10, 4));
			}
			if (_speedPanelContainer != null)
			{
				int volumePanelHeight = CalculateVolumePanelHeight();
				((Control)_speedPanelContainer).set_Width(volumeColumnWidth);
				((Control)_speedPanelContainer).set_Location(new Point(playbackColumnWidth + 10, 4 + volumePanelHeight + 5));
			}
			if (_sectionedSeekBar != null)
			{
				((Control)_sectionedSeekBar).set_Width(playbackColumnWidth - 32 - 30);
			}
			UpdateSectionsAndMarkersLayout(playbackColumnWidth);
			UpdateVolumeControlLayout(volumeColumnWidth);
			UpdateSpeedControlLayout(volumeColumnWidth);
		}

		private void UpdateSectionsAndMarkersLayout(int availableWidth)
		{
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			if (_sectionsPanelContainer != null && _markersPanelContainer != null && _playbackPanelContainer != null)
			{
				int rowTop = ((Control)_playbackPanelContainer).get_Bottom() + 5;
				_isStackedLayout = availableWidth < 405;
				int markerButtonCount = _markerButtons.Count + 1;
				int sectionsPanelWidth;
				int markersPanelWidth;
				int markersTop;
				int markersLeft;
				if (_isStackedLayout)
				{
					sectionsPanelWidth = availableWidth;
					markersPanelWidth = availableWidth;
					markersTop = rowTop + CalculateSectionsPanelHeight(sectionsPanelWidth) + 5;
					markersLeft = 0;
				}
				else
				{
					int totalWidth = availableWidth - 5;
					sectionsPanelWidth = Math.Max(80, (int)((double)totalWidth * 0.65));
					markersPanelWidth = Math.Max(80, totalWidth - sectionsPanelWidth);
					markersTop = rowTop;
					markersLeft = sectionsPanelWidth + 5;
				}
				int sectionsHeight = CalculateSectionsPanelHeight(sectionsPanelWidth);
				int markersHeight = CalculatePanelHeight(markersPanelWidth, markerButtonCount);
				((Control)_sectionsPanelContainer).set_Width(sectionsPanelWidth);
				((Control)_sectionsPanelContainer).set_Height(sectionsHeight);
				((Control)_sectionsPanelContainer).set_Location(new Point(0, rowTop));
				((Control)_markersPanelContainer).set_Width(markersPanelWidth);
				((Control)_markersPanelContainer).set_Height(markersHeight);
				((Control)_markersPanelContainer).set_Location(new Point(markersLeft, markersTop));
			}
		}

		private int CalculatePanelHeight(int panelWidth, int buttonCount)
		{
			if (buttonCount <= 0)
			{
				return 85;
			}
			int flowPanelPadding = 8;
			int contentWidth = panelWidth - flowPanelPadding;
			int buttonWithSpacing = 36;
			int buttonsPerRow = Math.Max(1, contentWidth / buttonWithSpacing);
			int contentHeight = (int)Math.Ceiling((double)buttonCount / (double)buttonsPerRow) * buttonWithSpacing + flowPanelPadding;
			return Math.Max(85, 28 + contentHeight + 5);
		}

		private int CalculateSectionsPanelHeight(int panelWidth)
		{
			if (_sectionButtons.Count == 0)
			{
				return 85;
			}
			return CalculateFlowPanelHeight(panelWidth, _sectionButtons.Select((StandardButton b) => ((Control)b).get_Width()));
		}

		private int EstimateSectionsPanelHeight(int panelWidth, IReadOnlyList<SectionInfo> sections)
		{
			if (sections == null || sections.Count == 0)
			{
				return 85;
			}
			return CalculateFlowPanelHeight(panelWidth, sections.Select((SectionInfo s) => CalculateSectionButtonWidth(s.Label)));
		}

		private int CalculateFlowPanelHeight(int panelWidth, IEnumerable<int> buttonWidths)
		{
			int flowPanelPadding = 8;
			int contentWidth = panelWidth - flowPanelPadding;
			int currentRowWidth = 0;
			int rowsNeeded = 1;
			foreach (int buttonWidth in buttonWidths)
			{
				int buttonWithSpacing = buttonWidth + 4;
				if (currentRowWidth + buttonWithSpacing > contentWidth && currentRowWidth > 0)
				{
					rowsNeeded++;
					currentRowWidth = buttonWithSpacing;
				}
				else
				{
					currentRowWidth += buttonWithSpacing;
				}
			}
			int contentHeight = rowsNeeded * 36 + flowPanelPadding;
			return Math.Max(85, 28 + contentHeight + 5);
		}

		private void UpdateVolumeControlLayout(int columnWidth)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			int sliderWidth = Math.Max(40, columnWidth - 55 - 20 - 32 - 30);
			if (_masterVolumeControl?.VolumeSlider != null)
			{
				((Control)_masterVolumeControl.VolumeSlider).set_Width(sliderWidth);
				if (_masterVolumeControl.PercentLabel != null)
				{
					((Control)_masterVolumeControl.PercentLabel).set_Location(new Point(((Control)_masterVolumeControl.VolumeSlider).get_Right() + 3, ((Control)_masterVolumeControl.PercentLabel).get_Location().Y));
				}
			}
			foreach (TrackVolumeControl control in _trackControls)
			{
				if (control.VolumeSlider != null)
				{
					((Control)control.VolumeSlider).set_Width(sliderWidth);
					if (control.PercentLabel != null)
					{
						((Control)control.PercentLabel).set_Location(new Point(((Control)control.VolumeSlider).get_Right() + 3, ((Control)control.PercentLabel).get_Location().Y));
					}
				}
			}
		}

		private void UpdateSpeedControlLayout(int columnWidth)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			int sliderWidth = Math.Max(40, columnWidth - 32 - 20);
			if (_speedTrackBar != null)
			{
				((Control)_speedTrackBar).set_Width(sliderWidth);
				if (_speedPercentLabel != null)
				{
					((Control)_speedPercentLabel).set_Location(new Point(((Control)_speedTrackBar).get_Right() + 3, ((Control)_speedPercentLabel).get_Location().Y));
				}
			}
		}

		private void OnPlayPauseClicked(object sender, MouseEventArgs e)
		{
			if (_playbackService.IsPlaying)
			{
				_playbackService.Pause();
			}
			else
			{
				_playbackService.Play();
			}
		}

		private void OnPositionChanged(object sender, double position)
		{
			_pendingPosition = position;
			_positionDirty = true;
		}

		private void OnPlaybackStarted(object sender, EventArgs e)
		{
			_pendingPlayState = true;
		}

		private void OnPlaybackStopped(object sender, EventArgs e)
		{
			_pendingStopState = true;
		}

		private void OnPlaybackFinished(object sender, EventArgs e)
		{
			_pendingFinished = true;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (_pendingSoundFontLoaded)
			{
				_pendingSoundFontLoaded = false;
				Label loadingLabel = _loadingLabel;
				if (loadingLabel != null)
				{
					((Control)loadingLabel).Dispose();
				}
				_loadingLabel = null;
				BuildTwoColumnLayout();
			}
			if (_pendingPlayState)
			{
				_pendingPlayState = false;
				UpdatePlayPauseButton(isPlaying: true);
			}
			if (_pendingStopState)
			{
				_pendingStopState = false;
				UpdatePlayPauseButton(isPlaying: false);
			}
			if (_pendingFinished)
			{
				_pendingFinished = false;
				UpdatePlayPauseButton(isPlaying: false);
				UpdateSeekBar(0.0);
			}
			if (_positionDirty)
			{
				_positionDirty = false;
				double pos = _pendingPosition;
				UpdateSeekBar(pos);
				this.PositionUpdated?.Invoke(this, pos);
			}
		}

		private void UpdatePlayPauseButton(bool isPlaying)
		{
			if (_playPauseButton != null)
			{
				if (isPlaying)
				{
					_playPauseButton.set_Icon(_pauseTexture);
					_playPauseButton.set_ActiveIcon(_pauseTexture);
					((Control)_playPauseButton).set_BasicTooltipText("Pause");
				}
				else
				{
					_playPauseButton.set_Icon(_playTexture);
					_playPauseButton.set_ActiveIcon(_playTexture);
					((Control)_playPauseButton).set_BasicTooltipText("Play");
				}
			}
		}

		private void UpdateSeekBar(double position)
		{
			if (_sectionedSeekBar != null)
			{
				_sectionedSeekBar.CurrentPosition = position;
			}
		}

		protected override void DisposeControl()
		{
			if (!_disposed)
			{
				_disposed = true;
				_playbackService.PositionChanged -= OnPositionChanged;
				_playbackService.PlaybackStarted -= OnPlaybackStarted;
				_playbackService.PlaybackStopped -= OnPlaybackStopped;
				_playbackService.PlaybackFinished -= OnPlaybackFinished;
				_playbackService.SoundFontLoaded -= OnSoundFontLoaded;
				_playbackService.Stop();
				if (_sectionedSeekBar != null)
				{
					_sectionedSeekBar.SeekRequested -= OnSectionedSeekBarSeekRequested;
					_sectionedSeekBar.MarkerAdded -= OnMarkerChanged;
					_sectionedSeekBar.MarkerRemoved -= OnMarkerChanged;
					_sectionedSeekBar.MarkerMoved -= OnMarkerChanged;
				}
				if (_playPauseButton != null)
				{
					((Control)_playPauseButton).remove_Click((EventHandler<MouseEventArgs>)OnPlayPauseClicked);
				}
				((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnPanelResized);
				((FlowPanel)this).DisposeControl();
			}
		}
	}
}
