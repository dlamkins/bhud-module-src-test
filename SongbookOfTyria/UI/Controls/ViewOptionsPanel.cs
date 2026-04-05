using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Controls.Notation;

namespace SongbookOfTyria.UI.Controls
{
	public sealed class ViewOptionsPanel : FlowPanel
	{
		private const int AutoScrollButtonSize = 24;

		private const float DefaultScrollSpeed = 30f;

		private const float MinScrollSpeed = 10f;

		private const float MaxScrollSpeed = 100f;

		private static readonly Dictionary<string, NotationFontSize> FontSizeFromString = new Dictionary<string, NotationFontSize>
		{
			{
				"16",
				NotationFontSize.Size16
			},
			{
				"18",
				NotationFontSize.Size18
			},
			{
				"20",
				NotationFontSize.Size20
			},
			{
				"22",
				NotationFontSize.Size22
			},
			{
				"24",
				NotationFontSize.Size24
			},
			{
				"26",
				NotationFontSize.Size26
			},
			{
				"28",
				NotationFontSize.Size28
			}
		};

		private static readonly Dictionary<NotationFontSize, string> FontSizeToString = new Dictionary<NotationFontSize, string>
		{
			{
				NotationFontSize.Size16,
				"16"
			},
			{
				NotationFontSize.Size18,
				"18"
			},
			{
				NotationFontSize.Size20,
				"20"
			},
			{
				NotationFontSize.Size22,
				"22"
			},
			{
				NotationFontSize.Size24,
				"24"
			},
			{
				NotationFontSize.Size26,
				"26"
			},
			{
				NotationFontSize.Size28,
				"28"
			}
		};

		private readonly TextureService _textureService;

		private Dropdown _fontSizeDropdown;

		private GlowButton _autoScrollButton;

		private TrackBar _speedTrackBar;

		private Label _speedLabel;

		private Checkbox _hitDetectionCheckbox;

		private FlowPanel _hitDetectionRow;

		private Panel _hitDetectionSpacer;

		private NotationFontSize _currentFontSize;

		private bool _autoScrollEnabled;

		private float _scrollSpeed;

		private bool _hitDetectionEnabled;

		private bool _lastCollapsedState;

		private bool _isHandlingResize;

		public NotationFontSize FontSize
		{
			get
			{
				return _currentFontSize;
			}
			set
			{
				_currentFontSize = value;
				if (_fontSizeDropdown != null)
				{
					_fontSizeDropdown.set_SelectedItem(GetFontSizeString(value));
				}
			}
		}

		public bool AutoScrollEnabled
		{
			get
			{
				return _autoScrollEnabled;
			}
			set
			{
				_autoScrollEnabled = value;
				if (_autoScrollButton != null)
				{
					_autoScrollButton.set_Checked(value);
				}
			}
		}

		public float ScrollSpeed
		{
			get
			{
				return _scrollSpeed;
			}
			set
			{
				_scrollSpeed = value;
				if (_speedTrackBar != null)
				{
					_speedTrackBar.set_Value(value);
				}
				UpdateSpeedLabel();
			}
		}

		public bool HitDetectionEnabled
		{
			get
			{
				return _hitDetectionEnabled;
			}
			set
			{
				_hitDetectionEnabled = value;
				if (_hitDetectionCheckbox != null)
				{
					_hitDetectionCheckbox.set_Checked(value);
				}
			}
		}

		public event EventHandler<NotationFontSize> FontSizeChanged;

		public event EventHandler<bool> AutoScrollToggled;

		public event EventHandler<float> ScrollSpeedChanged;

		public event EventHandler<bool> HitDetectionToggled;

		public event EventHandler<bool> CollapsedChanged;

		public ViewOptionsPanel(int panelWidth, bool collapsed, NotationFontSize initialFontSize, bool initialAutoScroll, float initialScrollSpeed, bool initialHitDetection, TextureService textureService)
			: this()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			_textureService = textureService;
			_currentFontSize = initialFontSize;
			_autoScrollEnabled = initialAutoScroll;
			_scrollSpeed = ((initialScrollSpeed > 0f) ? initialScrollSpeed : 30f);
			_hitDetectionEnabled = initialHitDetection;
			_lastCollapsedState = collapsed;
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_Title("View Options");
			((Panel)this).set_CanCollapse(true);
			((Control)this).set_Width(panelWidth);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 8f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(10f, 10f));
			BuildContent(panelWidth);
			((Panel)this).set_Collapsed(collapsed);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void BuildContent(int panelWidth)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Expected O, but got Unknown
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Expected O, but got Unknown
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Expected O, but got Unknown
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Expected O, but got Unknown
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Expected O, but got Unknown
			int contentWidth = panelWidth - 20;
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(contentWidth);
			((Control)val).set_Height(26);
			val.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel fontSizeRow = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(70);
			((Control)val2).set_Height(26);
			((Control)val2).set_Parent((Container)(object)fontSizeRow);
			Panel textSizeLabelContainer = val2;
			Label val3 = new Label();
			val3.set_Text("Text Size:");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Width(70);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(0, 2));
			((Control)val3).set_Parent((Container)(object)textSizeLabelContainer);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Width(100);
			((Control)val4).set_Parent((Container)(object)fontSizeRow);
			_fontSizeDropdown = val4;
			foreach (string size in FontSizeFromString.Keys)
			{
				_fontSizeDropdown.get_Items().Add(size);
			}
			_fontSizeDropdown.set_SelectedItem(GetFontSizeString(_currentFontSize));
			_fontSizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnFontSizeChanged);
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)2);
			((Control)val5).set_Width(contentWidth);
			((Control)val5).set_Height(26);
			val5.set_ControlPadding(new Vector2(3f, 0f));
			((Control)val5).set_Parent((Container)(object)this);
			FlowPanel autoscrollRow = val5;
			Label val6 = new Label();
			val6.set_Text("Scroll:");
			val6.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val6).set_Width(45);
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Parent((Container)(object)autoscrollRow);
			Panel val7 = new Panel();
			((Control)val7).set_Width(24);
			((Control)val7).set_Height(26);
			((Control)val7).set_Parent((Container)(object)autoscrollRow);
			Panel buttonContainer = val7;
			GlowButton val8 = new GlowButton();
			val8.set_Icon(_textureService.GetPlayIcon());
			val8.set_ActiveIcon(_textureService.GetPauseIcon());
			((Control)val8).set_BasicTooltipText("Toggle Autoscroll");
			val8.set_ToggleGlow(true);
			val8.set_Checked(_autoScrollEnabled);
			((Control)val8).set_Size(new Point(24, 24));
			((Control)val8).set_Location(new Point(0, -1));
			((Control)val8).set_Parent((Container)(object)buttonContainer);
			_autoScrollButton = val8;
			((Control)_autoScrollButton).add_Click((EventHandler<MouseEventArgs>)OnAutoScrollButtonClicked);
			Panel val9 = new Panel();
			((Control)val9).set_Width(100);
			((Control)val9).set_Height(26);
			((Control)val9).set_Parent((Container)(object)autoscrollRow);
			Panel trackBarContainer = val9;
			TrackBar val10 = new TrackBar();
			val10.set_MinValue(10f);
			val10.set_MaxValue(100f);
			val10.set_Value(_scrollSpeed);
			((Control)val10).set_Width(100);
			((Control)val10).set_Location(new Point(0, 3));
			((Control)val10).set_Parent((Container)(object)trackBarContainer);
			_speedTrackBar = val10;
			_speedTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSpeedTrackBarChanged);
			Panel val11 = new Panel();
			((Control)val11).set_Width(30);
			((Control)val11).set_Height(26);
			((Control)val11).set_Parent((Container)(object)autoscrollRow);
			Panel speedLabelContainer = val11;
			Label val12 = new Label();
			val12.set_Text($"{(int)_scrollSpeed}");
			val12.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val12).set_Width(30);
			val12.set_AutoSizeHeight(true);
			val12.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val12).set_Location(new Point(0, 3));
			((Control)val12).set_Parent((Container)(object)speedLabelContainer);
			_speedLabel = val12;
		}

		private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
		{
			if (FontSizeFromString.TryGetValue(e.get_CurrentValue(), out var fontSize))
			{
				_currentFontSize = fontSize;
				this.FontSizeChanged?.Invoke(this, fontSize);
			}
		}

		private void OnAutoScrollButtonClicked(object sender, MouseEventArgs e)
		{
			_autoScrollEnabled = !_autoScrollEnabled;
			_autoScrollButton.set_Checked(_autoScrollEnabled);
			this.AutoScrollToggled?.Invoke(this, _autoScrollEnabled);
		}

		private void OnSpeedTrackBarChanged(object sender, ValueEventArgs<float> e)
		{
			_scrollSpeed = e.get_Value();
			UpdateSpeedLabel();
			this.ScrollSpeedChanged?.Invoke(this, _scrollSpeed);
		}

		private void OnHitDetectionCheckboxChanged(object sender, CheckChangedEvent e)
		{
			_hitDetectionEnabled = e.get_Checked();
			this.HitDetectionToggled?.Invoke(this, _hitDetectionEnabled);
		}

		public void SetPracticeModeActive(bool isPracticeMode)
		{
			if (_hitDetectionRow != null)
			{
				((Control)_hitDetectionRow).set_Parent((Container)null);
			}
			if (_hitDetectionSpacer != null)
			{
				((Control)_hitDetectionSpacer).set_Parent((Container)null);
			}
			if (isPracticeMode)
			{
				if (_hitDetectionRow != null)
				{
					((Control)_hitDetectionRow).set_Visible(true);
					((Control)_hitDetectionRow).set_Parent((Container)(object)this);
				}
				if (_hitDetectionSpacer != null)
				{
					((Control)_hitDetectionSpacer).set_Visible(true);
					((Control)_hitDetectionSpacer).set_Parent((Container)(object)this);
				}
			}
			((Control)this).Invalidate();
		}

		private void UpdateSpeedLabel()
		{
			if (_speedLabel != null)
			{
				_speedLabel.set_Text($"{(int)_scrollSpeed}");
			}
		}

		private static string GetFontSizeString(NotationFontSize fontSize)
		{
			if (!FontSizeToString.TryGetValue(fontSize, out var sizeString))
			{
				return "20";
			}
			return sizeString;
		}

		public void RebuildContent()
		{
			if (_fontSizeDropdown != null)
			{
				_fontSizeDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnFontSizeChanged);
			}
			if (_autoScrollButton != null)
			{
				((Control)_autoScrollButton).remove_Click((EventHandler<MouseEventArgs>)OnAutoScrollButtonClicked);
			}
			if (_speedTrackBar != null)
			{
				_speedTrackBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSpeedTrackBarChanged);
			}
			if (_hitDetectionCheckbox != null)
			{
				_hitDetectionCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnHitDetectionCheckboxChanged);
			}
			DisposeOrphanedControls();
			Control[] array = ((Container)this).get_Children().ToArray();
			foreach (Control obj in array)
			{
				obj.set_Parent((Container)null);
				obj.Dispose();
			}
			_fontSizeDropdown = null;
			_autoScrollButton = null;
			_speedTrackBar = null;
			_speedLabel = null;
			_hitDetectionCheckbox = null;
			_hitDetectionRow = null;
			_hitDetectionSpacer = null;
			BuildContent(((Control)this).get_Width());
			((Control)this).Invalidate();
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			if (_isHandlingResize)
			{
				return;
			}
			_isHandlingResize = true;
			try
			{
				if (((Panel)this).get_Collapsed() != _lastCollapsedState)
				{
					_lastCollapsedState = ((Panel)this).get_Collapsed();
					this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
				}
			}
			finally
			{
				_isHandlingResize = false;
			}
		}

		private void DisposeOrphanedControls()
		{
			if (_hitDetectionRow != null && ((Control)_hitDetectionRow).get_Parent() == null)
			{
				((Control)_hitDetectionRow).Dispose();
				_hitDetectionRow = null;
			}
			if (_hitDetectionSpacer != null && ((Control)_hitDetectionSpacer).get_Parent() == null)
			{
				((Control)_hitDetectionSpacer).Dispose();
				_hitDetectionSpacer = null;
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			if (_fontSizeDropdown != null)
			{
				_fontSizeDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnFontSizeChanged);
			}
			if (_autoScrollButton != null)
			{
				((Control)_autoScrollButton).remove_Click((EventHandler<MouseEventArgs>)OnAutoScrollButtonClicked);
			}
			if (_speedTrackBar != null)
			{
				_speedTrackBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSpeedTrackBarChanged);
			}
			if (_hitDetectionCheckbox != null)
			{
				_hitDetectionCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnHitDetectionCheckboxChanged);
			}
			DisposeOrphanedControls();
			((FlowPanel)this).DisposeControl();
		}
	}
}
