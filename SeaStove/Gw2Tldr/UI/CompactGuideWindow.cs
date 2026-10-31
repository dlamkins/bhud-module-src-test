using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	public sealed class CompactGuideWindow : IDisposable
	{
		private readonly OverlayPanel _panel;

		private readonly OverlayBackground _background;

		private readonly Label _title;

		private readonly GuideCatalog _catalog;

		private readonly Action<string> _rememberGuide;

		private readonly ToolbarIcon _previousButton;

		private readonly ToolbarIcon _nextButton;

		private readonly ToolbarIcon _collapseButton;

		private bool _collapsed;

		private Point _expandedSize;

		private readonly Func<bool> _readAutoHeight;

		private readonly Action<bool> _writeAutoHeight;

		private bool _lastAutoHeight;

		private int _manualHeight;

		private readonly ToolbarIcon _menuButton;

		private readonly ToolbarIcon _editButton;

		private readonly ToolbarIcon _searchButton;

		private readonly ToolbarIcon _guidesButton;

		private readonly ToolbarIcon _copyAllButton;

		private readonly ToolbarIcon _lockButton;

		private readonly ToolbarIcon _autoButton;

		private readonly ToolbarIcon _closeButton;

		private readonly Action<int> _setOpacity;

		private readonly Action<bool> _setColorCoding;

		private readonly Func<bool> _readFadeHeader;

		private readonly Action<bool> _writeFadeHeader;

		private readonly Func<bool> _readLightMode;

		private readonly Action<bool> _writeLightMode;

		private bool _lightMode;

		private readonly Func<bool> _readShowCopy;

		private readonly Func<bool> _readColorText;

		private readonly Action<bool> _writeShowCopy;

		private readonly Action<bool> _writeColorText;

		private bool _showCopy = true;

		private bool _colorText = true;

		private readonly Func<bool> _readHideUnmapped;

		private readonly Func<bool> _readShowMapped;

		private readonly Func<bool> _readHideLoading;

		private readonly Action<bool> _writeHideLoading;

		private bool _hiddenForLoading;

		private readonly Action<bool> _writeHideUnmapped;

		private readonly Action<bool> _writeShowMapped;

		private int _lastVisibilityMap = -1;

		private GuideSettingsWindow _settingsWindow;

		private ContributorsWindow _contributorsWindow;

		private readonly Label _cmButton;

		private readonly Label _mapProgress;

		private readonly ToolbarIcon _resizeHandle;

		private readonly ContextMenuStrip _guidesMenu;

		private readonly Panel _body;

		private readonly ContextMenuStrip _menu;

		private readonly GuideWindow _picker;

		private readonly Action<int> _rememberTextSize;

		private readonly Action<Point> _rememberPosition;

		private readonly Action<Point> _rememberSize;

		private readonly Action<bool> _rememberLocked;

		private GuideEntry _guide;

		private Label _metaTimerLabel;

		private long _lastMetaTimerMinute = -1L;

		private bool _metaTimerVisible;

		private GuideEntry _originalGuide;

		private GuideEditor _editor;

		private GuideComparison _comparison;

		private GuideImportWindow _importWindow;

		private readonly Func<string> _readCustom;

		private readonly Action<string> _writeCustom;

		private readonly Label _subtitle;

		private int _fontIndex;

		private int _lastFontStyle;

		private readonly Func<int> _readFontStyle;

		private readonly Action<int> _writeFontStyle;

		private bool _challenge;

		private bool _locked;

		private int _dragging;

		private bool _positionDirty;

		private bool _sizeDirty;

		private Point _lastMouse;

		private bool _disposed;

		private readonly Func<bool> _readAutoSelect;

		private readonly Action<bool> _writeAutoSelect;

		private bool _lastAutoSelect;

		private int _lastMapId = -1;

		private List<GuideEntry> _mapGuides = new List<GuideEntry>();

		private bool _colorCoding = true;

		private int _backgroundOpacity = -1;

		private bool UseLightText
		{
			get
			{
				if (_lightMode)
				{
					return _backgroundOpacity < 50;
				}
				return true;
			}
		}

		public CompactGuideWindow(GuideCatalog catalog, string error, string selectedId, int fontIndex, Point position, Point size, bool locked, Action<string> rememberGuide, Action<int> rememberTextSize, Action<Point> rememberPosition, Action<Point> rememberSize, Action<bool> rememberLocked, Func<string> readCustom, Action<string> writeCustom, Func<bool> readAutoSelect, Action<bool> writeAutoSelect, Action<int> setOpacity, Action<bool> setColorCoding, Func<bool> readFadeHeader, Action<bool> writeFadeHeader, Func<bool> readLightMode, Action<bool> writeLightMode, Func<bool> readShowCopy, Action<bool> writeShowCopy, Func<bool> readColorText, Action<bool> writeColorText, Func<bool> readHideUnmapped, Action<bool> writeHideUnmapped, Func<bool> readShowMapped, Action<bool> writeShowMapped, Func<int> readFontStyle, Action<int> writeFontStyle, Func<bool> readHideLoading, Action<bool> writeHideLoading, Func<bool> readAutoHeight, Action<bool> writeAutoHeight)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Expected O, but got Unknown
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Expected O, but got Unknown
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Expected O, but got Unknown
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Expected O, but got Unknown
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Expected O, but got Unknown
			//IL_051a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Expected O, but got Unknown
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Expected O, but got Unknown
			//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
			_catalog = catalog;
			_readAutoHeight = readAutoHeight;
			_writeAutoHeight = writeAutoHeight;
			_lastAutoHeight = readAutoHeight();
			_manualHeight = ClampSize(size).Y;
			_readHideLoading = readHideLoading;
			_writeHideLoading = writeHideLoading;
			_readFontStyle = readFontStyle;
			_writeFontStyle = writeFontStyle;
			_lastFontStyle = readFontStyle();
			_readAutoSelect = readAutoSelect;
			_writeAutoSelect = writeAutoSelect;
			_setOpacity = setOpacity;
			_setColorCoding = setColorCoding;
			_readFadeHeader = readFadeHeader;
			_writeFadeHeader = writeFadeHeader;
			_readLightMode = readLightMode;
			_writeLightMode = writeLightMode;
			_readShowCopy = readShowCopy;
			_writeShowCopy = writeShowCopy;
			_readColorText = readColorText;
			_writeColorText = writeColorText;
			_readHideUnmapped = readHideUnmapped;
			_writeHideUnmapped = writeHideUnmapped;
			_readShowMapped = readShowMapped;
			_writeShowMapped = writeShowMapped;
			_rememberGuide = rememberGuide;
			_expandedSize = ClampSize(size);
			_readCustom = readCustom;
			_writeCustom = writeCustom;
			_fontIndex = Math.Max(0, Math.Min(2, fontIndex));
			_locked = locked;
			_rememberTextSize = rememberTextSize;
			_rememberPosition = rememberPosition;
			_rememberSize = rememberSize;
			_rememberLocked = rememberLocked;
			OverlayPanel overlayPanel = new OverlayPanel();
			((Control)overlayPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)overlayPanel).set_Location(position);
			((Control)overlayPanel).set_Size(ClampSize(size));
			((Control)overlayPanel).set_Visible(false);
			overlayPanel.DrawBackground = false;
			_panel = overlayPanel;
			OverlayBackground overlayBackground = new OverlayBackground();
			((Control)overlayBackground).set_Parent((Container)(object)_panel);
			((Control)overlayBackground).set_Size(((Control)_panel).get_Size());
			((Control)overlayBackground).set_ZIndex(-100);
			_background = overlayBackground;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_panel);
			((Control)val).set_Location(new Point(8, 3));
			((Control)val).set_Size(new Point(120, 25));
			val.set_Text("GW2 TLDR");
			((Control)val).set_BasicTooltipText("Drag to move");
			_title = val;
			((Control)_title).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginDrag);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_panel);
			((Control)val2).set_Location(new Point(8, 27));
			((Control)val2).set_Size(new Point(180, 20));
			val2.set_TextColor(Color.get_LightGray());
			_subtitle = val2;
			((Control)_subtitle).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginDrag);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_panel);
			((Control)val3).set_Size(new Point(50, 20));
			((Control)val3).set_Visible(false);
			((Control)val3).set_BasicTooltipText("Current encounter on this map; use the arrows to switch");
			_mapProgress = val3;
			_previousButton = MakeButton("previous", "Previous encounter", delegate
			{
				Navigate(-1);
			});
			_nextButton = MakeButton("next", "Next encounter", delegate
			{
				Navigate(1);
			});
			_collapseButton = MakeButton("collapse", "Collapse or expand guide", delegate
			{
				ToggleCollapsed();
			});
			_searchButton = MakeButton("search", "Search encounters", delegate
			{
				_writeAutoSelect(obj: false);
				_lastAutoSelect = false;
				_mapGuides.Clear();
				_picker?.Toggle();
			});
			_guidesButton = MakeButton("book", "External guides", ShowGuides);
			_copyAllButton = MakeButton("copy", "Copy all visible mechanics", delegate
			{
				CopyAll();
			});
			_closeButton = MakeButton("close", "Hide guide", delegate
			{
				((Control)_panel).Hide();
			});
			_autoButton = MakeButton("auto", "Toggle auto-select", delegate
			{
				_writeAutoSelect(!_readAutoSelect());
				_lastAutoSelect = false;
			});
			_lockButton = MakeButton("lock", "Lock or unlock movement and resizing", delegate
			{
				ToggleLock();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_panel);
			((Control)val4).set_Size(new Point(28, 25));
			val4.set_Text("CM");
			((Control)val4).set_BasicTooltipText("Toggle challenge mode");
			_cmButton = val4;
			((Control)_cmButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_guide?.HasChallengeMode ?? false)
				{
					_challenge = !_challenge;
					Render();
				}
			});
			_editButton = MakeButton("edit", "Edit this guide", delegate
			{
				OpenEditor();
			});
			_menuButton = MakeButton("settings", "Settings", ShowMenu);
			_resizeHandle = MakeButton("resize", "Drag to resize the overlay", delegate
			{
			});
			((Control)_resizeHandle).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginResize);
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)_panel);
			val5.set_CanScroll(true);
			_body = val5;
			_menu = new ContextMenuStrip();
			_guidesMenu = new ContextMenuStrip();
			_lockButton.Active = _locked;
			((Control)_resizeHandle).set_Visible(!_locked);
			((Control)_cmButton).set_Visible(false);
			Layout();
			((Control)_panel).add_Hidden((EventHandler<EventArgs>)OnHidden);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)EndDrag);
			if (catalog != null)
			{
				_picker = new GuideWindow(catalog, selectedId, rememberGuide, OnManualGuideSelected);
				_originalGuide = _picker.CurrentGuide;
				_guide = ApplyCustom(_originalGuide);
				Render();
			}
			else
			{
				AddText(error ?? "The bundled guides could not be loaded.", Color.get_White(), 6);
			}
		}

		public void Toggle()
		{
			if (((Control)_panel).get_Visible())
			{
				((Control)_panel).Hide();
			}
			else
			{
				((Control)_panel).Show();
			}
		}

		public void Update()
		{
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			Gw2InstanceIntegration game = GameService.GameIntegration.get_Gw2Instance();
			if (_readHideLoading() && game.get_Gw2IsRunning() && !game.get_IsInGame())
			{
				if (((Control)_panel).get_Visible())
				{
					((Control)_panel).Hide();
					_hiddenForLoading = true;
				}
			}
			else
			{
				if (_hiddenForLoading)
				{
					_hiddenForLoading = false;
					((Control)_panel).Show();
					_lastVisibilityMap = -1;
				}
				UpdateMapVisibility();
				UpdateAutoSelection();
			}
			UpdateMapProgress();
			_picker?.Update();
			_editor?.Update();
			_settingsWindow?.Update();
			bool timerVisible = ((Control)_panel).get_Visible() && !_collapsed && _metaTimerLabel != null;
			if (timerVisible)
			{
				long currentMinute = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60;
				if (!_metaTimerVisible || _lastMetaTimerMinute != currentMinute)
				{
					_lastMetaTimerMinute = currentMinute;
					_metaTimerLabel.set_Text(MetaTimerText());
				}
			}
			_metaTimerVisible = timerVisible;
			_contributorsWindow?.Update();
			if (_lastFontStyle != _readFontStyle())
			{
				_lastFontStyle = _readFontStyle();
				Render();
			}
			if (_lastAutoHeight != _readAutoHeight())
			{
				_lastAutoHeight = _readAutoHeight();
				if (!_lastAutoHeight && !_collapsed)
				{
					((Control)_panel).set_Height(_manualHeight);
					Layout();
				}
				Render();
			}
			UpdateHeaderFade();
			int gesture = Volatile.Read(ref _dragging);
			if (gesture != 0 && !_locked && ((Control)_panel).get_Visible())
			{
				Point current = GameService.Input.get_Mouse().get_Position();
				Point delta = current - _lastMouse;
				_lastMouse = current;
				if (gesture == 1 && delta != Point.get_Zero())
				{
					OverlayPanel panel = _panel;
					((Control)panel).set_Location(((Control)panel).get_Location() + delta);
					_positionDirty = true;
				}
				else
				{
					if (gesture != 2 || !(delta != Point.get_Zero()))
					{
						return;
					}
					Point size = ClampSize(((Control)_panel).get_Size() + (Point)(_readAutoHeight() ? new Point(delta.X, 0) : delta));
					if (size != ((Control)_panel).get_Size())
					{
						((Control)_panel).set_Size(size);
						if (!_readAutoHeight())
						{
							_manualHeight = size.Y;
						}
						_sizeDirty = true;
						Layout();
						Render();
					}
				}
			}
			else
			{
				if (_positionDirty)
				{
					_rememberPosition(((Control)_panel).get_Location());
					_positionDirty = false;
				}
				if (_sizeDirty)
				{
					_rememberSize(new Point(((Control)_panel).get_Width(), _manualHeight));
					_sizeDirty = false;
				}
			}
		}

		private void UpdateHeaderFade()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Point mouse = GameService.Input.get_Mouse().get_Position();
			Rectangle bounds = ((Control)_panel).get_AbsoluteBounds();
			Rectangle val = new Rectangle(bounds.X, bounds.Y, bounds.Width, 50);
			bool inHeader = ((Rectangle)(ref val)).Contains(mouse);
			float opacity = ((_readFadeHeader() && !inHeader) ? 0f : 1f);
			Control[] array = (Control[])(object)new Control[12]
			{
				_previousButton,
				_nextButton,
				_collapseButton,
				_editButton,
				_copyAllButton,
				(Control)_cmButton,
				_guidesButton,
				_searchButton,
				_lockButton,
				_menuButton,
				_autoButton,
				_closeButton
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].set_Opacity(opacity);
			}
		}

		public void OpenSettings()
		{
			_settingsWindow?.Dispose();
			_settingsWindow = new GuideSettingsWindow(() => Math.Max(0, _backgroundOpacity), delegate(int value)
			{
				_setOpacity(value);
				SetAppearance(value, _colorCoding, _lightMode, _showCopy, _colorText);
			}, () => _fontIndex, SetTextSizeFromPicker, () => _colorCoding, delegate(bool value)
			{
				_setColorCoding(value);
				SetAppearance(_backgroundOpacity, value, _lightMode, _showCopy, _colorText);
			}, _readFadeHeader, _writeFadeHeader, _readAutoSelect, delegate(bool enabled)
			{
				_writeAutoSelect(enabled);
				_lastAutoSelect = false;
			}, _readLightMode, delegate(bool value)
			{
				_writeLightMode(value);
				SetAppearance(_backgroundOpacity, _colorCoding, value, _showCopy, _colorText);
			}, _readShowCopy, delegate(bool value)
			{
				_writeShowCopy(value);
				SetAppearance(_backgroundOpacity, _colorCoding, _lightMode, value, _colorText);
			}, _readColorText, delegate(bool value)
			{
				_writeColorText(value);
				SetAppearance(_backgroundOpacity, _colorCoding, _lightMode, _showCopy, value);
			}, _readHideUnmapped, delegate(bool value)
			{
				_writeHideUnmapped(value);
				if (value)
				{
					_lastVisibilityMap = -1;
				}
			}, _readShowMapped, _writeShowMapped, _readFontStyle, delegate(int value)
			{
				_writeFontStyle(value);
				_lastFontStyle = value;
				Render();
			}, _readHideLoading, _writeHideLoading, _readAutoHeight, delegate(bool value)
			{
				_writeAutoHeight(value);
				_lastAutoHeight = value;
				if (!value && !_collapsed)
				{
					((Control)_panel).set_Height(_manualHeight);
					Layout();
				}
				Render();
			});
		}

		private static Point ClampSize(Point size)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			return new Point(Math.Max(360, Math.Min(1100, size.X)), Math.Max(130, Math.Min(900, size.Y)));
		}

		private void Layout()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			int width = ((Control)_panel).get_Width();
			int right = width - 8;
			Place(_closeButton);
			Place(_collapseButton);
			Place(_menuButton);
			Place(_lockButton);
			Place(_searchButton);
			Place(_guidesButton);
			if (_showCopy)
			{
				Place(_copyAllButton);
			}
			Place(_editButton);
			((Control)_title).set_Width(Math.Max(60, right - 8));
			((Control)_background).set_Size(((Control)_panel).get_Size());
			((Control)_subtitle).set_Width(Math.Max(100, width - (((Control)_mapProgress).get_Visible() ? 175 : ((_guide?.HasChallengeMode ?? false) ? 125 : 100))));
			((Control)_mapProgress).set_Location(new Point(width - 160, 27));
			((Control)_cmButton).set_Location(new Point(width - 105, 26));
			((Control)_previousButton).set_Location(new Point(width - 78, 27));
			((Control)_nextButton).set_Location(new Point(width - 53, 27));
			((Control)_autoButton).set_Location(new Point(width - 28, 27));
			((Control)_resizeHandle).set_Location(new Point(width - 20, ((Control)_panel).get_Height() - 20));
			((Control)_body).set_Location(new Point(5, 50));
			((Control)_body).set_Size(new Point(width - 10, Math.Max(1, ((Control)_panel).get_Height() - 67)));
			void Place(ToolbarIcon button)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				right -= 22;
				((Control)button).set_Location(new Point(right, 4));
				right -= 3;
			}
		}

		private ToolbarIcon MakeButton(string kind, string tooltip, EventHandler<MouseEventArgs> click)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			ToolbarIcon toolbarIcon = new ToolbarIcon(kind);
			((Control)toolbarIcon).set_Parent((Container)(object)_panel);
			((Control)toolbarIcon).set_Size(new Point(22, 22));
			((Control)toolbarIcon).set_BasicTooltipText(tooltip);
			((Control)toolbarIcon).add_Click(click);
			return toolbarIcon;
		}

		public void SetAppearance(int opacity, bool colorCoding, bool lightMode, bool showCopy, bool colorText)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			opacity = Math.Max(0, Math.Min(100, opacity));
			if (_backgroundOpacity != opacity || _colorCoding != colorCoding || _lightMode != lightMode || _showCopy != showCopy || _colorText != colorText)
			{
				_backgroundOpacity = opacity;
				_colorCoding = colorCoding;
				_lightMode = lightMode;
				_showCopy = showCopy;
				_colorText = colorText;
				((Control)_copyAllButton).set_Visible(showCopy);
				OverlayTheme.Light = lightMode;
				_title.set_TextColor(UseLightText ? Color.get_White() : Color.get_Black());
				_subtitle.set_TextColor(UseLightText ? Color.get_White() : Color.get_DarkSlateGray());
				_mapProgress.set_TextColor(UseLightText ? Color.get_White() : Color.get_DarkSlateGray());
				_mapProgress.set_ShowShadow(true);
				_mapProgress.set_ShadowColor(UseLightText ? Color.get_Black() : Color.get_White());
				Label title = _title;
				bool showShadow;
				_subtitle.set_ShowShadow(showShadow = true);
				title.set_ShowShadow(showShadow);
				Label title2 = _title;
				Color shadowColor;
				_subtitle.set_ShadowColor(shadowColor = (UseLightText ? Color.get_Black() : Color.get_White()));
				title2.set_ShadowColor(shadowColor);
				((Control)_panel).set_Opacity(1f);
				((Control)_body).set_Opacity(1f);
				_background.BackgroundOpacity = (float)opacity / 100f;
				((Control)_background).Invalidate();
				Render();
			}
		}

		public void SetTextSize(int index)
		{
			index = Math.Max(0, Math.Min(2, index));
			if (_fontIndex != index)
			{
				_fontIndex = index;
				Render();
			}
		}

		private void SetTextSizeFromPicker(int index)
		{
			SetTextSize(index);
			_rememberTextSize(index);
		}

		private void OnManualGuideSelected(GuideEntry guide)
		{
			_writeAutoSelect(obj: false);
			_lastAutoSelect = false;
			_mapGuides.Clear();
			OnGuideSelected(guide);
		}

		private void UpdateMapVisibility()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (mapId > 0 && _lastVisibilityMap != mapId)
			{
				_lastVisibilityMap = mapId;
				switch (MapVisibility.Decide(MapGuideIndex.Find(_catalog, mapId).Count > 0, _readHideUnmapped(), _readShowMapped(), ((Control)_panel).get_Visible()))
				{
				case VisibilityAction.Hide:
					((Control)_panel).Hide();
					break;
				case VisibilityAction.Show:
					((Control)_panel).Show();
					break;
				}
			}
		}

		private void UpdateMapProgress()
		{
			int index = ((_originalGuide == null) ? (-1) : _mapGuides.FindIndex((GuideEntry item) => item.Id == _originalGuide.Id));
			bool visible = _readAutoSelect() && _mapGuides.Count > 1 && index >= 0;
			if (visible)
			{
				_mapProgress.set_Text(index + 1 + " / " + _mapGuides.Count);
			}
			if (((Control)_mapProgress).get_Visible() != visible)
			{
				((Control)_mapProgress).set_Visible(visible);
				Layout();
			}
		}

		private void UpdateAutoSelection()
		{
			bool enabled = _readAutoSelect();
			_autoButton.Active = enabled;
			((Control)_autoButton).set_BasicTooltipText(enabled ? "Auto-select on — click to turn off" : "Auto-select off — click to turn on");
			if (!enabled)
			{
				_lastAutoSelect = false;
				return;
			}
			int mapId = (GameService.Gw2Mumble.get_IsAvailable() ? GameService.Gw2Mumble.get_CurrentMap().get_Id() : 0);
			if (!_lastAutoSelect || _lastMapId != mapId)
			{
				_lastAutoSelect = true;
				_lastMapId = mapId;
				_mapGuides = MapGuideIndex.Find(_catalog, mapId);
				if (_mapGuides.Count != 0)
				{
					_editor?.Dispose();
					_editor = null;
					_picker?.Hide();
					_rememberGuide(_mapGuides[0].Id);
					_picker?.SetCurrentGuide(_mapGuides[0]);
					OnGuideSelected(_mapGuides[0]);
				}
			}
		}

		private void OnGuideSelected(GuideEntry guide)
		{
			_originalGuide = guide;
			_guide = ApplyCustom(guide);
			if (!_guide.HasChallengeMode)
			{
				_challenge = false;
			}
			Render();
		}

		private void Navigate(int direction)
		{
			if (_originalGuide != null)
			{
				List<GuideEntry> group = ((_readAutoSelect() && _mapGuides.Any((GuideEntry item) => item.Id == _originalGuide.Id)) ? _mapGuides : _catalog.Guides.Where((GuideEntry item) => item.Category == _originalGuide.Category && item.Group == _originalGuide.Group).ToList());
				int num = group.FindIndex((GuideEntry item) => item.Id == _originalGuide.Id);
				int next = num + direction;
				if (num >= 0 && next >= 0 && next < group.Count)
				{
					_editor?.Dispose();
					_editor = null;
					_rememberGuide(group[next].Id);
					_picker?.SetCurrentGuide(group[next]);
					OnGuideSelected(group[next]);
				}
			}
		}

		private void ToggleCollapsed()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			Interlocked.Exchange(ref _dragging, 0);
			if (!_collapsed)
			{
				_expandedSize = ((Control)_panel).get_Size();
				if (_sizeDirty)
				{
					_rememberSize(new Point(_expandedSize.X, _manualHeight));
					_sizeDirty = false;
				}
				_collapsed = true;
				((Control)_panel).set_Height(50);
			}
			else
			{
				_collapsed = false;
				((Control)_panel).set_Size((Point)(_readAutoHeight() ? _expandedSize : new Point(_expandedSize.X, _manualHeight)));
			}
			((Control)_body).set_Visible(!_collapsed);
			((Control)_resizeHandle).set_Visible(!_collapsed && !_locked);
			((Control)_collapseButton).set_BasicTooltipText(_collapsed ? "Expand guide" : "Collapse guide");
			_collapseButton.Expanded = !_collapsed;
			Layout();
			if (!_collapsed && _readAutoHeight())
			{
				Render();
			}
		}

		private bool BundledGuideChanged()
		{
			if (_originalGuide == null)
			{
				return false;
			}
			JToken obj = ReadCustom().get_Item(_originalGuide.Id);
			JObject edit = (JObject)(object)((obj is JObject) ? obj : null);
			if (edit == null)
			{
				return false;
			}
			JToken obj2 = edit.get_Item("baseline");
			JObject baseline = (JObject)(object)((obj2 is JObject) ? obj2 : null);
			if (baseline == null)
			{
				return true;
			}
			return !JToken.DeepEquals((JToken)(object)baseline, (JToken)(object)GuideBaseline(_originalGuide));
		}

		private static JObject GuideBaseline(GuideEntry guide)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			JObject val = new JObject();
			val.set_Item("mechanics", (JToken)(object)JArray.FromObject((object)guide.GetMechanics(challengeMode: false)));
			val.set_Item("cm_mechanics", (JToken)(object)JArray.FromObject((object)guide.GetMechanics(challengeMode: true)));
			return val;
		}

		private JObject ReadCustom()
		{
			//IL_001d: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			try
			{
				return JObject.Parse(_readCustom() ?? "{}");
			}
			catch (JsonException val)
			{
				JsonException exception = val;
				Logger.GetLogger<CompactGuideWindow>().Warn((Exception)(object)exception, "Could not read local guide edits.");
				return new JObject();
			}
		}

		private GuideEntry ApplyCustom(GuideEntry original)
		{
			return GuideEdits.Apply(original, ReadCustom());
		}

		private void OpenEditor()
		{
			if (_guide == null)
			{
				return;
			}
			_editor?.Dispose();
			string guideId = _originalGuide.Id;
			JToken obj = ReadCustom().get_Item(guideId);
			JObject previousEdit = (JObject)(object)((obj is JObject) ? obj : null);
			object obj3;
			if (previousEdit != null)
			{
				JToken obj2 = previousEdit.get_Item("baseline");
				obj3 = ((obj2 != null) ? obj2.DeepClone() : null);
			}
			else
			{
				obj3 = GuideBaseline(_originalGuide);
			}
			JToken baseline = (JToken)obj3;
			_editor = new GuideEditor(_guide, _challenge, delegate(List<Mechanic> normal, List<Mechanic> cm)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Expected O, but got Unknown
				JObject val2 = ReadCustom();
				string text = guideId;
				JObject val3 = new JObject();
				val3.set_Item("mechanics", (JToken)(object)JArray.FromObject((object)normal));
				val3.set_Item("cm_mechanics", (JToken)(object)JArray.FromObject((object)cm));
				val3.set_Item("baseline", (JToken)(((object)baseline) ?? ((object)JValue.CreateNull())));
				val2.set_Item(text, (JToken)val3);
				_writeCustom(((JToken)val2).ToString((Formatting)0, Array.Empty<JsonConverter>()));
				_guide = ApplyCustom(_originalGuide);
				if (!_guide.HasChallengeMode)
				{
					_challenge = false;
				}
				Render();
			}, delegate
			{
				JObject val = ReadCustom();
				val.Remove(guideId);
				_writeCustom(((JToken)val).ToString((Formatting)0, Array.Empty<JsonConverter>()));
				_guide = ApplyCustom(_originalGuide);
				if (!_guide.HasChallengeMode)
				{
					_challenge = false;
				}
				Render();
			}, _colorCoding || _colorText);
		}

		private void ShowComparison()
		{
			if (_guide != null && _originalGuide != null)
			{
				_comparison?.Dispose();
				_comparison = new GuideComparison(_guide, _originalGuide);
			}
		}

		private void ExportGuide()
		{
			if (_guide != null)
			{
				CopyText(GuideEdits.Export(_guide));
			}
		}

		private JObject AllNotesExport()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Expected O, but got Unknown
			JArray guides = new JArray();
			foreach (JProperty entry in ReadCustom().Properties())
			{
				JToken value = entry.get_Value();
				JObject notes = (JObject)(object)((value is JObject) ? value : null);
				if (notes != null)
				{
					GuideEntry guide = _catalog?.Guides.FirstOrDefault((GuideEntry item) => item.Id == entry.get_Name());
					if (guide != null)
					{
						JObject val = new JObject();
						val.set_Item("guideId", JToken.op_Implicit(entry.get_Name()));
						val.set_Item("name", JToken.op_Implicit(guide.Name));
						val.set_Item("category", JToken.op_Implicit(guide.Category));
						val.set_Item("group", JToken.op_Implicit(guide.Group));
						JToken obj = notes.get_Item("mechanics");
						val.set_Item("mechanics", (obj != null) ? obj.DeepClone() : null);
						JToken obj2 = notes.get_Item("cm_mechanics");
						val.set_Item("cm_mechanics", (obj2 != null) ? obj2.DeepClone() : null);
						guides.Add((JToken)val);
					}
				}
			}
			JObject val2 = new JObject();
			val2.set_Item("format", JToken.op_Implicit("gw2tldr-guide-submissions"));
			val2.set_Item("schemaVersion", JToken.op_Implicit(1));
			val2.set_Item("guides", (JToken)(object)guides);
			return val2;
		}

		private async void ExportAllNotes()
		{
			JObject payload = AllNotesExport();
			if (((JContainer)(JArray)payload.get_Item("guides")).get_Count() == 0)
			{
				ScreenNotification.ShowNotification("No saved guide edits to export.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(((JToken)payload).ToString((Formatting)0, Array.Empty<JsonConverter>()));
				ScreenNotification.ShowNotification("All saved guide edits copied to clipboard.", (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception exception)
			{
				Logger.GetLogger<CompactGuideWindow>().Warn(exception, "Could not copy guide edits.");
				ScreenNotification.ShowNotification("Could not copy guide edits. Please try again.", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private async void SubmitAllNotes()
		{
			JObject payload = AllNotesExport();
			if (((JContainer)(JArray)payload.get_Item("guides")).get_Count() == 0)
			{
				ScreenNotification.ShowNotification("No saved guide edits could be exported.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(((JToken)payload).ToString((Formatting)0, Array.Empty<JsonConverter>()));
				ScreenNotification.ShowNotification("Guide edits copied. Paste them in the feedback form in your browser.", (NotificationType)0, (Texture2D)null, 4);
				OpenLink("https://forms.gle/YgpeijBaASgATMD1A");
			}
			catch (Exception exception)
			{
				Logger.GetLogger<CompactGuideWindow>().Warn(exception, "Could not copy guide edits for submission.");
				ScreenNotification.ShowNotification("Could not copy guide edits. Please try again.", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private void ImportGuide()
		{
			if (_originalGuide == null)
			{
				return;
			}
			_importWindow?.Dispose();
			_importWindow = new GuideImportWindow(_catalog, delegate(JObject imported)
			{
				JObject val = ReadCustom();
				foreach (JProperty current in imported.Properties())
				{
					val.set_Item(current.get_Name(), current.get_Value().DeepClone());
				}
				_writeCustom(((JToken)val).ToString((Formatting)0, Array.Empty<JsonConverter>()));
				if (_originalGuide != null && imported.get_Item(_originalGuide.Id) != null)
				{
					_guide = ApplyCustom(_originalGuide);
					if (!_guide.HasChallengeMode)
					{
						_challenge = false;
					}
					Render();
				}
			});
		}

		private void BeginDrag(object sender, MouseEventArgs args)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (!_locked)
			{
				_lastMouse = GameService.Input.get_Mouse().get_Position();
				Interlocked.Exchange(ref _dragging, 1);
			}
		}

		private void BeginResize(object sender, MouseEventArgs args)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (!_locked && !_collapsed)
			{
				_lastMouse = GameService.Input.get_Mouse().get_Position();
				Interlocked.Exchange(ref _dragging, 2);
			}
		}

		private void EndDrag(object sender, MouseEventArgs args)
		{
			Interlocked.Exchange(ref _dragging, 0);
		}

		private void ToggleLock()
		{
			_locked = !_locked;
			Interlocked.Exchange(ref _dragging, 0);
			_lockButton.Active = _locked;
			((Control)_resizeHandle).set_Visible(!_locked && !_collapsed);
			((Control)_lockButton).set_BasicTooltipText(_locked ? "Unlock movement and resizing" : "Lock movement and resizing");
			_rememberLocked(_locked);
		}

		private async void CopyText(string text)
		{
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
			}
			catch (Exception exception)
			{
				Logger.GetLogger<CompactGuideWindow>().Warn(exception, "Could not copy guide text.");
			}
		}

		private void CopyAll()
		{
			if (_guide != null)
			{
				IReadOnlyList<Mechanic> mechanics = _guide.GetMechanics(challengeMode: false);
				if (_challenge)
				{
					mechanics = mechanics.Concat(_guide.GetMechanics(challengeMode: true)).ToList();
				}
				CopyText(string.Join("\n", mechanics.Select((Mechanic mechanic) => mechanic.Text)));
			}
		}

		private void ShowGuides(object sender, MouseEventArgs args)
		{
			((Control)_menu).Hide();
			((Control)_guidesMenu).Hide();
			Control[] array = ((Container)_guidesMenu).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			if (_guide != null)
			{
				foreach (KeyValuePair<string, string> source in _guide.GetSourceLinks(_challenge))
				{
					KeyValuePair<string, string> link = source;
					((Control)_guidesMenu.AddMenuItem("Open " + link.Key + " guide")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						((Control)_guidesMenu).Hide();
						OpenLink(link.Value);
					});
				}
			}
			if (((Container)_guidesMenu).get_Children().get_Count() == 0)
			{
				((Control)_guidesMenu.AddMenuItem("No external guides available")).set_Enabled(false);
			}
			_guidesMenu.Show((Control)(object)_guidesButton);
		}

		private void ShowMenu(object sender, MouseEventArgs args)
		{
			((Control)_guidesMenu).Hide();
			((Control)_menu).Hide();
			Control[] array = ((Container)_menu).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			AddMenuItem("Settings", OpenSettings);
			AddMenuItem("Contributors", OpenContributors);
			AddMenuItem("Import notes", ImportGuide);
			AddMenuItem("Export current notes to clipboard", ExportGuide);
			AddMenuItem("Export all custom notes to clipboard", ExportAllNotes);
			AddMenuItem("Submit all custom edits to help improve base data", SubmitAllNotes);
			AddMenuItem("Provide feedback", delegate
			{
				OpenLink("https://forms.gle/YgpeijBaASgATMD1A");
			});
			if (BundledGuideChanged())
			{
				AddMenuItem("Compare with updated bundled guide", ShowComparison);
			}
			_menu.Show((Control)(object)_menuButton);
		}

		private void OpenContributors()
		{
			_contributorsWindow?.Dispose();
			_contributorsWindow = new ContributorsWindow(OpenLink);
		}

		private void AddMenuItem(string text, Action action)
		{
			((Control)_menu.AddMenuItem(text)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_menu).Hide();
				action();
			});
		}

		private void Render()
		{
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Expected O, but got Unknown
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Expected O, but got Unknown
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			_metaTimerLabel = null;
			Control[] array = ((Container)_body).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			if (_guide == null)
			{
				return;
			}
			_title.set_Text(_guide.Name + (BundledGuideChanged() ? " *" : ""));
			_subtitle.set_Text((_guide.Category == "raids") ? (_guide.Group + " • " + _guide.Section) : _guide.Group);
			if (BundledGuideChanged())
			{
				Label subtitle = _subtitle;
				subtitle.set_Text(subtitle.get_Text() + " • Updated guide available");
			}
			((Control)_cmButton).set_Visible(_guide.HasChallengeMode);
			Layout();
			_cmButton.set_TextColor((Color)((!_challenge) ? (_lightMode ? Color.get_DarkSlateGray() : Color.get_LightGray()) : (_lightMode ? new Color(145, 70, 0) : new Color(255, 180, 90))));
			_lockButton.Active = _locked;
			((Control)_title).set_BasicTooltipText(_guide.DisplayName + (BundledGuideChanged() ? " — bundled guide changed; compare using the options menu." : " — drag to move; use the toolbar for options."));
			int top = 6;
			string levels = _catalog?.FractalLevels(_guide);
			if (!string.IsNullOrEmpty(levels))
			{
				top = AddText("Fractal levels: " + levels, Color.get_LightGray(), top);
			}
			if (_guide.Category == "metas")
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_body);
				((Control)val).set_Location(new Point(6, top));
				((Control)val).set_Width(Math.Max(80, ((Control)_body).get_Width() - 35));
				val.set_Font(Font());
				val.set_TextColor(UseLightText ? Color.get_White() : Color.get_DarkSlateGray());
				val.set_ShowShadow(true);
				val.set_ShadowColor(UseLightText ? Color.get_Black() : Color.get_White());
				val.set_AutoSizeHeight(true);
				val.set_WrapText(true);
				val.set_Text(MetaTimerText());
				_metaTimerLabel = val;
				top += ((Control)_metaTimerLabel).get_Height() + 8;
			}
			string zone = (string)_guide.Details.get_Item("zone");
			JToken obj = _guide.Details.get_Item("waypoints");
			string[] waypoints = ((obj != null) ? obj.ToObject<string[]>() : null);
			bool combineZone = _guide.Category == "metas" && !string.IsNullOrWhiteSpace(zone) && waypoints != null && waypoints.Length != 0;
			if (!string.IsNullOrEmpty(zone) && !combineZone)
			{
				top = AddText(zone, Color.get_LightGray(), top);
			}
			if (waypoints != null)
			{
				string[] array2 = waypoints;
				foreach (string waypoint in array2)
				{
					string waypointText = (combineZone ? (zone + ": " + waypoint) : ("Waypoint: " + waypoint));
					Label val2 = new Label();
					((Control)val2).set_Parent((Container)(object)_body);
					((Control)val2).set_Location(new Point(6, top));
					((Control)val2).set_Width(Math.Max(80, ((Control)_body).get_Width() - (_showCopy ? 83 : 35)));
					val2.set_Font(Font());
					val2.set_TextColor(UseLightText ? Color.get_White() : Color.get_DarkSlateGray());
					val2.set_ShowShadow(true);
					val2.set_ShadowColor(UseLightText ? Color.get_Black() : Color.get_White());
					val2.set_WrapText(true);
					val2.set_AutoSizeHeight(true);
					val2.set_Text(waypointText);
					((Control)val2).set_BasicTooltipText(_showCopy ? "Click to copy waypoint" : "Waypoint");
					Label label = val2;
					if (_showCopy)
					{
						((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							CopyText(waypoint);
						});
						ToolbarIcon toolbarIcon = new ToolbarIcon("copy");
						((Control)toolbarIcon).set_Parent((Container)(object)_body);
						((Control)toolbarIcon).set_Location(new Point(((Control)_body).get_Width() - 45, top));
						((Control)toolbarIcon).set_Size(new Point(20, 20));
						((Control)toolbarIcon).set_BasicTooltipText("Copy waypoint");
						((Control)toolbarIcon).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							CopyText(waypoint);
						});
					}
					top += Math.Max(20, ((Control)label).get_Height()) + 8;
				}
			}
			top = AddMechanics(_guide.GetMechanics(challengeMode: false), top);
			if (_challenge)
			{
				top = AddText("Challenge mode changes", Color.get_White(), top + 3);
				top = AddMechanics(_guide.GetMechanics(challengeMode: true), top);
			}
			if (_readAutoHeight() && !_collapsed)
			{
				int availableHeight = Math.Max(130, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - Math.Max(0, ((Control)_panel).get_Top()));
				int height = Math.Max(130, Math.Min(Math.Min(900, availableHeight), top + 67));
				if (((Control)_panel).get_Height() != height)
				{
					((Control)_panel).set_Height(height);
					Layout();
				}
			}
		}

		private string MetaTimerText()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;
			MetaSchedule.Occurrence occurrence = _catalog?.NextMeta(_guide, now);
			if (occurrence == null)
			{
				return "No fixed timer available";
			}
			if (occurrence.Active)
			{
				return "Active (started " + (int)Math.Floor((now - occurrence.Start).TotalMinutes) + "m ago)";
			}
			int minutes = Math.Max(0, (int)Math.Ceiling((occurrence.Start - now).TotalMinutes));
			return "Next scheduled: " + occurrence.Start.ToLocalTime().ToString("t") + " (in " + minutes + "m)";
		}

		private int AddMechanics(IReadOnlyList<Mechanic> mechanics, int top)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			foreach (Mechanic mechanic in mechanics)
			{
				Color color = (_colorCoding ? IconColor(mechanic.Color, !UseLightText) : (UseLightText ? Color.get_White() : Color.get_DarkSlateGray()));
				MechanicIcon mechanicIcon = new MechanicIcon(mechanic.Type, color);
				((Control)mechanicIcon).set_Parent((Container)(object)_body);
				((Control)mechanicIcon).set_Location(new Point(5, top + 3));
				((Control)mechanicIcon).set_Size(new Point(18, 18));
				((Control)mechanicIcon).set_BasicTooltipText(mechanic.Type);
				MechanicIcon icon = mechanicIcon;
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_body);
				((Control)val).set_Location(new Point(29, top));
				((Control)val).set_Width(Math.Max(80, ((Control)_body).get_Width() - (_showCopy ? 83 : 48)));
				val.set_Font(Font());
				val.set_TextColor(_colorText ? IconColor(TypeColor(mechanic.Type), !UseLightText) : (UseLightText ? Color.get_White() : Color.get_Black()));
				val.set_ShowShadow(true);
				val.set_ShadowColor(UseLightText ? Color.get_Black() : Color.get_White());
				val.set_WrapText(true);
				val.set_AutoSizeHeight(true);
				val.set_Text(mechanic.Text);
				Label label = val;
				if (_showCopy)
				{
					ToolbarIcon toolbarIcon = new ToolbarIcon("copy");
					((Control)toolbarIcon).set_Parent((Container)(object)_body);
					((Control)toolbarIcon).set_Location(new Point(((Control)_body).get_Width() - 45, top));
					((Control)toolbarIcon).set_Size(new Point(20, 20));
					((Control)toolbarIcon).set_BasicTooltipText("Copy mechanic");
					((Control)toolbarIcon).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CopyText(mechanic.Text);
					});
				}
				top += Math.Max(((Control)icon).get_Height(), ((Control)label).get_Height()) + 8;
			}
			return top;
		}

		private int AddText(string text, Color color, int top)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_body);
			((Control)val).set_Location(new Point(6, top));
			((Control)val).set_Width(Math.Max(80, ((Control)_body).get_Width() - 35));
			val.set_Font(Font());
			val.set_TextColor(UseLightText ? Color.get_White() : Color.get_DarkSlateGray());
			val.set_ShowShadow(true);
			val.set_ShadowColor(UseLightText ? Color.get_Black() : Color.get_White());
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			val.set_Text(text);
			Label label = val;
			return top + ((Control)label).get_Height() + 8;
		}

		private BitmapFont Font()
		{
			ContentService content = GameService.Content;
			FontSize[] array = new FontSize[3];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			return content.GetFont((FontFace)0, array[_fontIndex], (new FontStyle[3]
			{
				default(FontStyle),
				(FontStyle)2,
				(FontStyle)1
			})[Math.Max(0, Math.Min(2, _readFontStyle()))]);
		}

		internal static string TypeColor(string type)
		{
			return type switch
			{
				"movement" => "blue", 
				"group" => "green", 
				"combat" => "orange", 
				"danger" => "red", 
				"timer" => "purple", 
				"shield" => "teal", 
				_ => "gray", 
			};
		}

		internal static Color IconColor(string color, bool lightBackground = false)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			if (lightBackground)
			{
				return (Color)(color switch
				{
					"blue" => new Color(30, 90, 180), 
					"green" => new Color(35, 120, 65), 
					"orange" => new Color(160, 80, 0), 
					"red" => new Color(170, 45, 45), 
					"purple" => new Color(115, 65, 160), 
					"teal" => new Color(0, 105, 100), 
					_ => Color.get_DarkSlateGray(), 
				});
			}
			return (Color)(color switch
			{
				"blue" => new Color(165, 215, 255), 
				"green" => new Color(175, 255, 185), 
				"orange" => new Color(255, 215, 155), 
				"red" => new Color(255, 175, 175), 
				"purple" => new Color(215, 165, 255), 
				"teal" => new Color(120, 225, 220), 
				_ => Color.get_LightGray(), 
			});
		}

		private static void OpenLink(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo(url)
				{
					UseShellExecute = true
				});
			}
			catch (Exception exception)
			{
				Logger.GetLogger<CompactGuideWindow>().Warn(exception, "Could not open guide source.");
			}
		}

		private void OnHidden(object sender, EventArgs args)
		{
			Interlocked.Exchange(ref _dragging, 0);
			((Control)_menu).Hide();
			((Control)_guidesMenu).Hide();
			_picker?.Hide();
			_editor?.Dispose();
			_editor = null;
			_comparison?.Dispose();
			_comparison = null;
			_importWindow?.Dispose();
			_importWindow = null;
			_settingsWindow?.Dispose();
			_settingsWindow = null;
			_contributorsWindow?.Dispose();
			_contributorsWindow = null;
		}

		public void Dispose()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (!_disposed)
			{
				_disposed = true;
				Interlocked.Exchange(ref _dragging, 0);
				if (_positionDirty)
				{
					_rememberPosition(((Control)_panel).get_Location());
				}
				if (_sizeDirty)
				{
					_rememberSize(new Point(((Control)_panel).get_Width(), _manualHeight));
				}
				GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)EndDrag);
				((Control)_panel).remove_Hidden((EventHandler<EventArgs>)OnHidden);
				((Control)_menu).Hide();
				((Control)_menu).Dispose();
				((Control)_guidesMenu).Hide();
				((Control)_guidesMenu).Dispose();
				_editor?.Dispose();
				_comparison?.Dispose();
				_importWindow?.Dispose();
				_picker?.Dispose();
				_settingsWindow?.Dispose();
				_contributorsWindow?.Dispose();
				((Control)_panel).Dispose();
			}
		}
	}
}
