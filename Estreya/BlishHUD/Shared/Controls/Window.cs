using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Settings;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public abstract class Window : Container, IWindow, IViewContainer
	{
		private const int STANDARD_TITLEBAR_HEIGHT = 40;

		private const int STANDARD_TITLEBAR_VERTICAL_OFFSET = 11;

		private const int STANDARD_LEFTTITLEBAR_HORIZONTAL_OFFSET = 2;

		private const int STANDARD_RIGHTTITLEBAR_HORIZONTAL_OFFSET = 16;

		private const int STANDARD_TITLEOFFSET = 80;

		private const int STANDARD_SUBTITLEOFFSET = 20;

		private const int STANDARD_MARGIN = 16;

		private const int SIDEBAR_WIDTH = 46;

		private const int SIDEBAR_OFFSET = 3;

		private const int RESIZEHANDLE_SIZE = 16;

		private const string WINDOW_SETTINGS = "WindowSettings2";

		private static readonly Texture2D _textureTitleBarLeft = Control.get_Content().GetTexture("titlebar-inactive");

		private static readonly Texture2D _textureTitleBarRight = Control.get_Content().GetTexture("window-topright");

		private static readonly Texture2D _textureTitleBarLeftActive = Control.get_Content().GetTexture("titlebar-active");

		private static readonly Texture2D _textureTitleBarRightActive = Control.get_Content().GetTexture("window-topright-active");

		private static readonly Texture2D _textureExitButton = Control.get_Content().GetTexture("button-exit");

		private static readonly Texture2D _textureExitButtonActive = Control.get_Content().GetTexture("button-exit-active");

		private static readonly Texture2D _textureBlackFade = Control.get_Content().GetTexture("fade-down-46");

		private readonly SettingCollection _windowSettings;

		private readonly AsyncTexture2D _textureWindowCorner = AsyncTexture2D.FromAssetId(156008);

		private readonly AsyncTexture2D _textureWindowResizableCorner = AsyncTexture2D.FromAssetId(156009);

		private readonly AsyncTexture2D _textureWindowResizableCornerActive = AsyncTexture2D.FromAssetId(156010);

		private readonly AsyncTexture2D _textureSplitLine = AsyncTexture2D.FromAssetId(605026);

		private string _title = "No Title";

		private string _subtitle = "";

		private bool _canClose = true;

		private bool _canCloseWithEscape = true;

		private bool _canResize;

		private Point _maxSize = Point.get_Zero();

		private Point _minSize = Point.get_Zero();

		private bool _rebuildViewAfterResize;

		private bool _unloadOnRebuild = true;

		private AsyncTexture2D _emblem;

		private bool _topMost;

		protected bool _savesPosition;

		protected bool _savesSize;

		private string _id;

		private bool _dragging;

		private bool _resizing;

		private readonly Tween _animFade;

		private bool _savedVisibility;

		private bool _showSideBar;

		private int _sideBarHeight = 100;

		private double _lastWindowInteract;

		private Rectangle _leftTitleBarDrawBounds = Rectangle.get_Empty();

		private Rectangle _rightTitleBarDrawBounds = Rectangle.get_Empty();

		private Rectangle _subtitleDrawBounds = Rectangle.get_Empty();

		private Rectangle _emblemDrawBounds = Rectangle.get_Empty();

		private Rectangle _sidebarInactiveDrawBounds = Rectangle.get_Empty();

		private Point _dragStart = Point.get_Zero();

		private Point _resizeStart = Point.get_Zero();

		private Point _contentMargin;

		private float _windowToTextureWidthRatio;

		private float _windowToTextureHeightRatio;

		private float _windowLeftOffsetRatio;

		private float _windowTopOffsetRatio;

		public static IWindow ActiveWindow
		{
			get
			{
				return (from w in GetWindows()
					where w.get_Visible()
					select w).OrderByDescending(GetZIndex).FirstOrDefault();
			}
			set
			{
				value.BringWindowToFront();
			}
		}

		public override int ZIndex
		{
			get
			{
				return ((Control)this)._zIndex + GetZIndex((IWindow)(object)this);
			}
			set
			{
				((Control)this).SetProperty<int>(ref ((Control)this)._zIndex, value, false, "ZIndex");
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _title, value, true, "Title");
			}
		}

		public string Subtitle
		{
			get
			{
				return _subtitle;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _subtitle, value, true, "Subtitle");
			}
		}

		public bool CanClose
		{
			get
			{
				return _canClose;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canClose, value, false, "CanClose");
			}
		}

		public bool CanCloseWithEscape
		{
			get
			{
				return _canCloseWithEscape;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canCloseWithEscape, value, false, "CanCloseWithEscape");
			}
		}

		public bool CanResize
		{
			get
			{
				return _canResize;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canResize, value, false, "CanResize");
			}
		}

		public Point MaxSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _maxSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Point>(ref _maxSize, value, false, "MaxSize");
			}
		}

		public Point MinSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _minSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Point>(ref _minSize, value, true, "MinSize");
			}
		}

		public bool RebuildViewAfterResize
		{
			get
			{
				return _rebuildViewAfterResize;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _rebuildViewAfterResize, value, false, "RebuildViewAfterResize");
			}
		}

		public bool UnloadOnRebuild
		{
			get
			{
				return _unloadOnRebuild;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _unloadOnRebuild, value, false, "UnloadOnRebuild");
			}
		}

		public Texture2D Emblem
		{
			get
			{
				return AsyncTexture2D.op_Implicit(_emblem);
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _emblem, AsyncTexture2D.op_Implicit(value), true, "Emblem");
			}
		}

		public bool TopMost
		{
			get
			{
				return _topMost;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _topMost, value, false, "TopMost");
			}
		}

		public bool SavesPosition
		{
			get
			{
				return _savesPosition;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _savesPosition, value, false, "SavesPosition");
			}
		}

		public bool SavesSize
		{
			get
			{
				return _savesSize;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _savesSize, value, false, "SavesSize");
			}
		}

		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _id, value, false, "Id");
			}
		}

		public bool Dragging
		{
			get
			{
				return _dragging;
			}
			private set
			{
				((Control)this).SetProperty<bool>(ref _dragging, value, false, "Dragging");
			}
		}

		public bool Resizing
		{
			get
			{
				return _resizing;
			}
			private set
			{
				((Control)this).SetProperty<bool>(ref _resizing, value, false, "Resizing");
			}
		}

		public ViewState ViewState { get; protected set; }

		public IView CurrentView { get; protected set; }

		protected bool ShowSideBar
		{
			get
			{
				return _showSideBar;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _showSideBar, value, false, "ShowSideBar");
			}
		}

		protected int SideBarHeight
		{
			get
			{
				return _sideBarHeight;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _sideBarHeight, value, true, "SideBarHeight");
			}
		}

		double LastInteraction => _lastWindowInteract;

		protected Rectangle TitleBarBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle ExitButtonBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle ResizeHandleBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle SidebarActiveBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle BackgroundDestinationBounds { get; private set; } = Rectangle.get_Empty();


		protected bool MouseOverTitleBar { get; private set; }

		protected bool MouseOverExitButton { get; private set; }

		protected bool MouseOverResizeHandle { get; private set; }

		protected AsyncTexture2D WindowBackground { get; set; }

		protected Rectangle WindowRegion { get; set; }

		protected Rectangle WindowRelativeContentRegion { get; set; }

		public event EventHandler ManualResized;

		public static IEnumerable<IWindow> GetWindows()
		{
			return ((Container)GameService.Graphics.get_SpriteScreen()).GetChildrenOfType<IWindow>();
		}

		public static int GetZIndex(IWindow thisWindow)
		{
			IWindow[] source = GetWindows().ToArray();
			if (!source.Contains(thisWindow))
			{
				throw new InvalidOperationException("thisWindow must be a direct child of GameService.Graphics.SpriteScreen before ZIndex can automatically be calculated.");
			}
			return 41 + (from window in source
				orderby window.get_TopMost(), window.get_LastInteraction()
				select window).TakeWhile((IWindow window) => window != thisWindow).Count();
		}

		private Window()
			: this()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(false);
			((Control)this)._zIndex = 41;
			((Control)this).set_ClipsBounds(false);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				UpdateWindowBaseDynamicHUDCombatState(this);
			});
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				UpdateWindowBaseDynamicHUDLoadingState(this);
			});
			_animFade = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Window>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(-1).Reflect();
			_animFade.Pause();
			_animFade.OnComplete((Action)delegate
			{
				_animFade.Pause();
				if (((Control)this)._opacity <= 0f)
				{
					((Control)this).set_Visible(false);
				}
			});
		}

		public Window(BaseModuleSettings settings)
			: this()
		{
			_windowSettings = settings.GlobalSettings.AddSubCollection("WindowSettings", false);
		}

		public static void UpdateWindowBaseDynamicHUDCombatState(Window wb)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)GameService.Overlay.get_DynamicHUDWindows() == 1 && GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				wb._savedVisibility = ((Control)wb).get_Visible();
				if (wb._savedVisibility)
				{
					((Control)wb).Hide();
				}
			}
			else if (wb._savedVisibility)
			{
				((Control)wb).Show();
			}
		}

		public static void UpdateWindowBaseDynamicHUDLoadingState(Window wb)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)GameService.Overlay.get_DynamicHUDLoading() == 3 && !GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				wb._savedVisibility = ((Control)wb).get_Visible();
				if (wb._savedVisibility)
				{
					((Control)wb).Hide();
				}
			}
			else if (wb._savedVisibility)
			{
				((Control)wb).Show();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			if (Dragging)
			{
				Point point = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + point);
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (Resizing)
			{
				Point point2 = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Size(HandleWindowResize(_resizeStart + point2));
			}
		}

		public void ToggleWindow()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
			}
			else
			{
				((Control)this).Show();
			}
		}

		public override void Show()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			BringWindowToFront();
			if (((Control)this).get_Visible())
			{
				return;
			}
			if (Id != null)
			{
				SettingEntry settingEntry = default(SettingEntry);
				if (SavesPosition && _windowSettings.TryGetSetting(Id, ref settingEntry))
				{
					((Control)this).set_Location(((settingEntry as SettingEntry<Point>) ?? new SettingEntry<Point>()).get_Value());
				}
				SettingEntry settingEntry2 = default(SettingEntry);
				if (SavesSize && _windowSettings.TryGetSetting(Id + "_size", ref settingEntry2))
				{
					Point savedSize = ((settingEntry2 as SettingEntry<Point>) ?? new SettingEntry<Point>()).get_Value();
					if (savedSize.X < MinSize.X || savedSize.Y < MinSize.Y)
					{
						((Control)this).set_Size(MinSize);
					}
					else if (MaxSize != Point.get_Zero() && (savedSize.X > MaxSize.X || savedSize.Y > MaxSize.Y))
					{
						((Control)this).set_Size(MaxSize);
					}
					else
					{
						((Control)this).set_Size(savedSize);
					}
				}
			}
			((Control)this).set_Location(new Point(MathHelper.Clamp(((Control)this)._location.X, 0, ((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 64), MathHelper.Clamp(((Control)this)._location.Y, 0, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 64)));
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(true);
			_animFade.Resume();
		}

		public override void Hide()
		{
			if (((Control)this).get_Visible())
			{
				Dragging = false;
				_animFade.Resume();
				Control.get_Content().PlaySoundEffectByName("window-close");
			}
		}

		public void SetView(IView view)
		{
			SetView(view, unloadCurrent: true);
		}

		private void SetView(IView view, bool unloadCurrent = true)
		{
			ClearView(view == null || unloadCurrent);
			if (view != null)
			{
				ViewState = (ViewState)1;
				CurrentView = view;
				Progress<string> progress = new Progress<string>(delegate
				{
				});
				view.add_Loaded((EventHandler<EventArgs>)OnViewBuilt);
				view.DoLoad((IProgress<string>)progress).ContinueWith(BuildView);
			}
		}

		public void Show(IView view)
		{
			SetView(view);
			((Control)this).Show();
		}

		protected void ClearView(bool unload = true)
		{
			if (CurrentView != null)
			{
				CurrentView.remove_Loaded((EventHandler<EventArgs>)OnViewBuilt);
				if (unload)
				{
					CurrentView.DoUnload();
				}
			}
			((Container)this).ClearChildren();
			ViewState = (ViewState)0;
		}

		private void OnViewBuilt(object sender, EventArgs e)
		{
			CurrentView.remove_Loaded((EventHandler<EventArgs>)OnViewBuilt);
			ViewState = (ViewState)2;
		}

		private void BuildView(Task<bool> loadResult)
		{
			if (loadResult.Result)
			{
				CurrentView.DoBuild((Container)(object)this);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			_rightTitleBarDrawBounds = new Rectangle(TitleBarBounds.Width - _textureTitleBarRight.get_Width() + 16, TitleBarBounds.Y - 11, _textureTitleBarRight.get_Width(), _textureTitleBarRight.get_Height());
			Rectangle titleBarBounds = TitleBarBounds;
			int num4 = ((Rectangle)(ref titleBarBounds)).get_Location().X - 2;
			titleBarBounds = TitleBarBounds;
			_leftTitleBarDrawBounds = new Rectangle(num4, ((Rectangle)(ref titleBarBounds)).get_Location().Y - 11, Math.Min(_textureTitleBarLeft.get_Width(), ((Rectangle)(ref _rightTitleBarDrawBounds)).get_Left() - 2), _textureTitleBarLeft.get_Height());
			if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Subtitle))
			{
				int num = (int)Control.get_Content().get_DefaultFont32().MeasureString(Title)
					.Width;
				_subtitleDrawBounds = RectangleExtension.OffsetBy(_leftTitleBarDrawBounds, 80 + num + 20, 0);
			}
			if (_emblem != null)
			{
				_emblemDrawBounds = new Rectangle(_leftTitleBarDrawBounds.X + 40 - _emblem.get_Width() / 2 - 16, ((Rectangle)(ref _leftTitleBarDrawBounds)).get_Bottom() - _textureTitleBarLeft.get_Height() / 2 - _emblem.get_Height() / 2, _emblem.get_Width(), _emblem.get_Height());
			}
			ExitButtonBounds = new Rectangle(((Rectangle)(ref _rightTitleBarDrawBounds)).get_Right() - 32 - _textureExitButton.get_Width(), _rightTitleBarDrawBounds.Y + 16, _textureExitButton.get_Width(), _textureExitButton.get_Height());
			int num2 = ((Rectangle)(ref _leftTitleBarDrawBounds)).get_Bottom() - 11;
			int num3 = ((Control)this).get_Size().Y - num2;
			SidebarActiveBounds = new Rectangle(_leftTitleBarDrawBounds.X + 3, num2 - 3, 46, SideBarHeight);
			_sidebarInactiveDrawBounds = new Rectangle(_leftTitleBarDrawBounds.X + 3, num2 - 3 + SideBarHeight, 46, num3 - SideBarHeight);
			ResizeHandleBounds = new Rectangle(((Control)this).get_Width() - _textureWindowCorner.get_Width(), ((Control)this).get_Height() - _textureWindowCorner.get_Height(), _textureWindowCorner.get_Width(), _textureWindowCorner.get_Height());
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			ResetMouseRegionStates();
			int y = ((Control)this).get_RelativeMousePosition().Y;
			Rectangle val = TitleBarBounds;
			if (y < ((Rectangle)(ref val)).get_Bottom())
			{
				val = ExitButtonBounds;
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					MouseOverExitButton = true;
				}
				else
				{
					MouseOverTitleBar = true;
				}
			}
			else if (_canResize)
			{
				val = ResizeHandleBounds;
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					int x = ((Control)this).get_RelativeMousePosition().X;
					val = ResizeHandleBounds;
					if (x > ((Rectangle)(ref val)).get_Right() - 16)
					{
						int y2 = ((Control)this).get_RelativeMousePosition().Y;
						val = ResizeHandleBounds;
						if (y2 > ((Rectangle)(ref val)).get_Bottom() - 16)
						{
							MouseOverResizeHandle = true;
						}
					}
				}
			}
			((Control)this).OnMouseMoved(e);
		}

		private void OnGlobalMouseRelease(object sender, MouseEventArgs e)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible())
			{
				return;
			}
			if (Id != null)
			{
				if (SavesPosition && Dragging)
				{
					((_windowSettings.get_Item(Id) as SettingEntry<Point>) ?? _windowSettings.DefineSetting<Point>(Id, ((Control)this).get_Location(), (Func<string>)null, (Func<string>)null)).set_Value(((Control)this).get_Location());
				}
				else if (SavesSize && Resizing)
				{
					((_windowSettings.get_Item(Id + "_size") as SettingEntry<Point>) ?? _windowSettings.DefineSetting<Point>(Id + "_size", ((Control)this).get_Size(), (Func<string>)null, (Func<string>)null)).set_Value(((Control)this).get_Size());
				}
			}
			if (Resizing && _resizeStart != ((Control)this).get_Size())
			{
				OnManualResized();
			}
			Dragging = false;
			Resizing = false;
		}

		private void OnManualResized()
		{
			try
			{
				this.ManualResized?.Invoke(this, EventArgs.Empty);
				if (RebuildViewAfterResize && CurrentView != null)
				{
					SetView(CurrentView, UnloadOnRebuild);
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			ResetMouseRegionStates();
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			BringWindowToFront();
			if (MouseOverTitleBar)
			{
				Dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (MouseOverResizeHandle)
			{
				Resizing = true;
				_resizeStart = ((Control)this).get_Size();
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (MouseOverExitButton && CanClose)
			{
				((Control)this).Hide();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (MouseOverResizeHandle && e.get_IsDoubleClick())
			{
				((Control)this).set_Size(new Point(WindowRegion.Width, WindowRegion.Height + 40));
			}
			((Control)this).OnClick(e);
		}

		private void ResetMouseRegionStates()
		{
			MouseOverTitleBar = false;
			MouseOverExitButton = false;
			MouseOverResizeHandle = false;
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			int minX = Math.Max(Math.Max(((Container)this).get_ContentRegion().X + _contentMargin.X + 16, ((Rectangle)(ref _subtitleDrawBounds)).get_Left() + 16), MinSize.X);
			int maxX = ((MaxSize != Point.get_Zero()) ? MaxSize.X : int.MaxValue);
			int minY = Math.Max(ShowSideBar ? (((Rectangle)(ref _sidebarInactiveDrawBounds)).get_Top() + 16) : (((Container)this).get_ContentRegion().Y + _contentMargin.Y + 16), MinSize.Y);
			int maxY = ((MaxSize != Point.get_Zero()) ? MaxSize.Y : int.MaxValue);
			return new Point(MathHelper.Clamp(newSize.X, minX, maxX), MathHelper.Clamp(newSize.Y, minY, maxY));
		}

		public void BringWindowToFront()
		{
			_lastWindowInteract = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		protected void ConstructWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(background, windowRegion, contentRegion, new Point(windowRegion.Width, windowRegion.Height + 40));
		}

		protected void ConstructWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion);
		}

		protected void ConstructWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			WindowBackground = background;
			WindowRegion = windowRegion;
			WindowRelativeContentRegion = contentRegion;
			((Control)this).set_Padding(new Thickness((float)Math.Max(((Rectangle)(ref windowRegion)).get_Top() - 40, 11), (float)(background.get_Width() - ((Rectangle)(ref windowRegion)).get_Right()), (float)(background.get_Height() - ((Rectangle)(ref windowRegion)).get_Bottom() + 40), (float)((Rectangle)(ref windowRegion)).get_Left()));
			int x = contentRegion.X;
			Thickness padding = ((Control)this).get_Padding();
			int num = x - (int)((Thickness)(ref padding)).get_Left();
			int num2 = contentRegion.Y + 40;
			padding = ((Control)this).get_Padding();
			((Container)this).set_ContentRegion(new Rectangle(num, num2 - (int)((Thickness)(ref padding)).get_Top(), contentRegion.Width, contentRegion.Height));
			_contentMargin = new Point(((Rectangle)(ref windowRegion)).get_Right() - ((Rectangle)(ref contentRegion)).get_Right(), ((Rectangle)(ref windowRegion)).get_Bottom() - ((Rectangle)(ref contentRegion)).get_Bottom());
			_windowToTextureWidthRatio = (float)(((Container)this).get_ContentRegion().Width + _contentMargin.X + ((Container)this).get_ContentRegion().X) / (float)background.get_Width();
			_windowToTextureHeightRatio = (float)(((Container)this).get_ContentRegion().Height + _contentMargin.Y + ((Container)this).get_ContentRegion().Y - 40) / (float)background.get_Height();
			_windowLeftOffsetRatio = (float)(-((Rectangle)(ref windowRegion)).get_Left()) / (float)background.get_Width();
			_windowTopOffsetRatio = (float)(-((Rectangle)(ref windowRegion)).get_Top()) / (float)background.get_Height();
			((Control)this).set_Size(windowSize);
		}

		protected void ConstructWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion, windowSize);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_ContentRegion(new Rectangle(((Container)this).get_ContentRegion().X, ((Container)this).get_ContentRegion().Y, ((Control)this).get_Width() - ((Container)this).get_ContentRegion().X - _contentMargin.X, ((Control)this).get_Height() - ((Container)this).get_ContentRegion().Y - _contentMargin.Y));
			CalculateWindow();
			((Container)this).OnResized(e);
		}

		private void CalculateWindow()
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			TitleBarBounds = new Rectangle(0, 0, ((Control)this).get_Size().X, 40);
			int num = (int)((float)(((Container)this).get_ContentRegion().Width + _contentMargin.X + ((Container)this).get_ContentRegion().X) / _windowToTextureWidthRatio);
			int num2 = (int)((float)(((Container)this).get_ContentRegion().Height + _contentMargin.Y + ((Container)this).get_ContentRegion().Y - 40) / _windowToTextureHeightRatio);
			BackgroundDestinationBounds = new Rectangle((int)Math.Floor(_windowLeftOffsetRatio * (float)num), (int)Math.Floor(_windowTopOffsetRatio * (float)num2 + 40f), num, num2);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			PaintWindowBackground(spriteBatch);
			PaintSideBar(spriteBatch);
			PaintTitleBar(spriteBatch);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			PaintEmblem(spriteBatch);
			PaintTitleText(spriteBatch);
			PaintExitButton(spriteBatch);
			PaintCorner(spriteBatch);
		}

		private void PaintCorner(SpriteBatch spriteBatch)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (CanResize)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((MouseOverResizeHandle || Resizing) ? _textureWindowResizableCornerActive : _textureWindowResizableCorner), ResizeHandleBounds);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureWindowCorner), ResizeHandleBounds);
			}
		}

		private void PaintSideBar(SpriteBatch spriteBatch)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			if (ShowSideBar)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), SidebarActiveBounds, Color.get_Black());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureBlackFade, _sidebarInactiveDrawBounds);
				Texture2D obj = AsyncTexture2D.op_Implicit(_textureSplitLine);
				Rectangle sidebarActiveBounds = SidebarActiveBounds;
				int num = ((Rectangle)(ref sidebarActiveBounds)).get_Right() - _textureSplitLine.get_Width() / 2;
				sidebarActiveBounds = SidebarActiveBounds;
				int top = ((Rectangle)(ref sidebarActiveBounds)).get_Top();
				int width = _textureSplitLine.get_Width();
				int bottom = ((Rectangle)(ref _sidebarInactiveDrawBounds)).get_Bottom();
				sidebarActiveBounds = SidebarActiveBounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, new Rectangle(num, top, width, bottom - ((Rectangle)(ref sidebarActiveBounds)).get_Top()));
			}
		}

		private void PaintWindowBackground(SpriteBatch spriteBatch)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(WindowBackground), BackgroundDestinationBounds);
		}

		private void PaintTitleBar(SpriteBatch spriteBatch)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver() && MouseOverTitleBar)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTitleBarLeftActive, _leftTitleBarDrawBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTitleBarRightActive, _rightTitleBarDrawBounds);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTitleBarLeft, _leftTitleBarDrawBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTitleBarRight, _rightTitleBarDrawBounds);
			}
		}

		private void PaintTitleText(SpriteBatch spriteBatch)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrWhiteSpace(Title))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Title, Control.get_Content().get_DefaultFont32(), RectangleExtension.OffsetBy(_leftTitleBarDrawBounds, 80, 0), Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (!string.IsNullOrWhiteSpace(Subtitle))
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Subtitle, Control.get_Content().get_DefaultFont16(), _subtitleDrawBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		private void PaintExitButton(SpriteBatch spriteBatch)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (CanClose)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MouseOverExitButton ? _textureExitButtonActive : _textureExitButton, ExitButtonBounds);
			}
		}

		private void PaintEmblem(SpriteBatch spriteBatch)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (_emblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Emblem, _emblemDrawBounds);
			}
		}

		protected override void DisposeControl()
		{
			if (CurrentView != null)
			{
				CurrentView.remove_Loaded((EventHandler<EventArgs>)OnViewBuilt);
				CurrentView.DoUnload();
			}
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			((Container)this).DisposeControl();
		}
	}
}
