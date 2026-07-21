using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using Taskmaster.Models;
using Taskmaster.UI.Controls;

namespace Taskmaster.UI
{
	public class TabStrip : Control
	{
		private const int TabPaddingX = 10;

		private const int TabGap = 4;

		private const int NameBadgeGap = 10;

		private const int BadgePaddingX = 6;

		private const int BadgeHeight = 18;

		private const int TabRadius = 3;

		private const int MinTabWidth = 88;

		private const int MaxTabNameWidth = 140;

		private const int PlusWidth = 28;

		private const int PlusIconSize = 14;

		private const int DragThreshold = 6;

		private const int ScrollArrowWidth = 18;

		private const int ScrollStep = 90;

		private const int ControlGap = 2;

		private const int EdgeFadeWidth = 10;

		private const int ControlRailWidth = 68;

		private IReadOnlyList<TodoTab> _tabs = new List<TodoTab>();

		private readonly List<Rectangle> _tabBounds = new List<Rectangle>();

		private int _hoverIndex = -1;

		private bool _hoverPlus;

		private bool _hoverLeft;

		private bool _hoverRight;

		private int _dragIndex = -1;

		private Point _dragStart;

		private bool _dragging;

		private int _scrollX;

		private int _contentWidth;

		private bool _panning;

		private int _panStartX;

		private int _panStartScrollX;

		private Guid? _activeTabId;

		private Guid? _editingTabId;

		private bool _ensureActiveVisible;

		public Guid? ActiveTabId
		{
			get
			{
				return _activeTabId;
			}
			set
			{
				if (!(_activeTabId == value))
				{
					_activeTabId = value;
					_ensureActiveVisible = true;
					((Control)this).Invalidate();
				}
			}
		}

		public Guid? EditingTabId
		{
			get
			{
				return _editingTabId;
			}
			set
			{
				if (!(_editingTabId == value))
				{
					_editingTabId = value;
					((Control)this).Invalidate();
				}
			}
		}

		public bool Locked { get; set; }

		private int RailStartX => Math.Max(0, ((Control)this).get_Width() - 68);

		private int ViewportWidth => Math.Max(0, RailStartX - 4);

		private bool CanScrollLeft => _scrollX > 0;

		private bool CanScrollRight => _scrollX < MaxScroll();

		private Rectangle LeftArrowBounds => new Rectangle(RailStartX, 2, 18, ((Control)this).get_Height() - 2);

		private Rectangle RightArrowBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle leftArrowBounds = LeftArrowBounds;
				return new Rectangle(((Rectangle)(ref leftArrowBounds)).get_Right() + 2, 2, 18, ((Control)this).get_Height() - 2);
			}
		}

		private Rectangle PlusBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rightArrowBounds = RightArrowBounds;
				return new Rectangle(((Rectangle)(ref rightArrowBounds)).get_Right() + 2, 2, 28, ((Control)this).get_Height() - 2);
			}
		}

		public event Action<TodoTab> TabClicked;

		public event Action<TodoTab> TabRightClicked;

		public event Action AddClicked;

		public event Action<TodoTab, int> TabReordered;

		public TabStrip()
			: this()
		{
			((Control)this).set_Height(32);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		public void SetTabs(IReadOnlyList<TodoTab> tabs)
		{
			_tabs = tabs ?? new List<TodoTab>();
			_ensureActiveVisible = true;
			((Control)this).Invalidate();
		}

		private static string BadgeText(TodoTab tab)
		{
			return $"{tab.DoneCount}/{tab.TotalCount}";
		}

		private void RecomputeLayout()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			_tabBounds.Clear();
			BitmapFont font = GameService.Content.get_DefaultFont14();
			BitmapFont badgeFont = GameService.Content.get_DefaultFont12();
			int x = 0;
			foreach (TodoTab tab in _tabs)
			{
				int nameWidth = Math.Min(140, (int)font.MeasureString(tab.Name).Width);
				int badgeWidth = (int)badgeFont.MeasureString(BadgeText(tab)).Width + 12;
				int w = Math.Max(88, nameWidth + badgeWidth + 10 + 20);
				_tabBounds.Add(new Rectangle(x, 2, w, ((Control)this).get_Height() - 3));
				x += w + 4;
			}
			_contentWidth = x;
			if (_ensureActiveVisible && ActiveTabId.HasValue)
			{
				int activeIndex = -1;
				for (int j = 0; j < _tabs.Count; j++)
				{
					if (_tabs[j].Id == ActiveTabId.Value)
					{
						activeIndex = j;
						break;
					}
				}
				if (activeIndex >= 0 && ViewportWidth > 0)
				{
					Rectangle activeBounds = _tabBounds[activeIndex];
					if (activeBounds.Width >= ViewportWidth || activeBounds.X < _scrollX)
					{
						_scrollX = activeBounds.X;
					}
					else if (((Rectangle)(ref activeBounds)).get_Right() > _scrollX + ViewportWidth)
					{
						_scrollX = ((Rectangle)(ref activeBounds)).get_Right() - ViewportWidth;
					}
				}
			}
			_ensureActiveVisible = false;
			_scrollX = ClampScroll(_scrollX);
			for (int i = 0; i < _tabBounds.Count; i++)
			{
				Rectangle r = _tabBounds[i];
				_tabBounds[i] = new Rectangle(r.X - _scrollX, r.Y, r.Width, r.Height);
			}
		}

		private int MaxScroll()
		{
			return Math.Max(0, _contentWidth - ViewportWidth);
		}

		private int ClampScroll(int value)
		{
			return Math.Max(0, Math.Min(value, MaxScroll()));
		}

		private int IndexAt(Point p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (p.X < 0 || p.X >= ViewportWidth)
			{
				return -1;
			}
			for (int i = 0; i < _tabBounds.Count; i++)
			{
				Rectangle val = _tabBounds[i];
				if (((Rectangle)(ref val)).Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		public bool TryGetTabEditBounds(Guid tabId, out Rectangle bounds)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			RecomputeLayout();
			for (int i = 0; i < _tabs.Count; i++)
			{
				if (!(_tabs[i].Id != tabId))
				{
					Rectangle tabBounds = _tabBounds[i];
					if (((Rectangle)(ref tabBounds)).get_Right() <= 0 || tabBounds.X >= ViewportWidth)
					{
						break;
					}
					Rectangle visualBounds = (Rectangle)((ActiveTabId == tabId) ? new Rectangle(tabBounds.X, tabBounds.Y - 1, tabBounds.Width, tabBounds.Height + 1) : tabBounds);
					bounds = new Rectangle(visualBounds.X + 6, visualBounds.Y + 3, Math.Max(40, visualBounds.Width - 12), 24);
					return true;
				}
			}
			bounds = Rectangle.get_Empty();
			return false;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			Point p = ((Control)this).get_RelativeMousePosition();
			_hoverIndex = IndexAt(p);
			Rectangle val = PlusBounds;
			_hoverPlus = ((Rectangle)(ref val)).Contains(p);
			val = LeftArrowBounds;
			_hoverLeft = ((Rectangle)(ref val)).Contains(p);
			val = RightArrowBounds;
			_hoverRight = ((Rectangle)(ref val)).Contains(p);
			((Control)this).set_BasicTooltipText((_hoverPlus && Locked) ? "Locked - unlock to add tabs" : ((_hoverIndex >= 0 && _hoverIndex < _tabs.Count && GameService.Content.get_DefaultFont14().MeasureString(_tabs[_hoverIndex].Name).Width > 140f) ? _tabs[_hoverIndex].Name : null));
			if (_panning)
			{
				_scrollX = ClampScroll(_panStartScrollX + (_panStartX - p.X));
			}
			else if (_dragIndex >= 0 && !_dragging && Math.Abs(p.X - _dragStart.X) > 6)
			{
				_dragging = true;
			}
			((Control)this).Invalidate();
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseWheelScrolled(e);
			MouseState state = GameService.Input.get_Mouse().get_State();
			int delta = ((MouseState)(ref state)).get_ScrollWheelValue();
			if (delta != 0)
			{
				_scrollX = ClampScroll(_scrollX - Math.Sign(delta) * 90);
			}
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			_hoverIndex = -1;
			_hoverPlus = false;
			_hoverLeft = false;
			_hoverRight = false;
			_panning = false;
			((Control)this).set_BasicTooltipText((string)null);
			((Control)this).Invalidate();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point p = ((Control)this).get_RelativeMousePosition();
			Rectangle val = LeftArrowBounds;
			if (((Rectangle)(ref val)).Contains(p))
			{
				if (CanScrollLeft)
				{
					_scrollX = ClampScroll(_scrollX - 90);
				}
				((Control)this).Invalidate();
				return;
			}
			val = RightArrowBounds;
			if (((Rectangle)(ref val)).Contains(p))
			{
				if (CanScrollRight)
				{
					_scrollX = ClampScroll(_scrollX + 90);
				}
				((Control)this).Invalidate();
				return;
			}
			val = PlusBounds;
			if (((Rectangle)(ref val)).Contains(p))
			{
				if (!Locked)
				{
					this.AddClicked?.Invoke();
				}
				return;
			}
			int i = IndexAt(p);
			if (i >= 0)
			{
				_dragIndex = i;
				_dragStart = p;
				_dragging = false;
			}
			else if (p.X < ViewportWidth && MaxScroll() > 0)
			{
				_panning = true;
				_panStartX = p.X;
				_panStartScrollX = _scrollX;
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonReleased(e);
			if (_dragIndex >= 0)
			{
				TodoTab tab = ((_dragIndex < _tabs.Count) ? _tabs[_dragIndex] : null);
				if (tab != null)
				{
					if (_dragging)
					{
						int target = IndexAt(((Control)this).get_RelativeMousePosition());
						if (target < 0)
						{
							target = ((((Control)this).get_RelativeMousePosition().X >= ViewportWidth) ? (_tabs.Count - 1) : _dragIndex);
						}
						if (target != _dragIndex)
						{
							this.TabReordered?.Invoke(tab, target);
						}
					}
					else
					{
						this.TabClicked?.Invoke(tab);
					}
				}
			}
			_dragIndex = -1;
			_dragging = false;
			_panning = false;
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnRightMouseButtonPressed(e);
			int i = IndexAt(((Control)this).get_RelativeMousePosition());
			if (i >= 0 && i < _tabs.Count)
			{
				this.TabRightClicked?.Invoke(_tabs[i]);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			RecomputeLayout();
			BitmapFont font = GameService.Content.get_DefaultFont14();
			BitmapFont badgeFont = GameService.Content.get_DefaultFont12();
			Texture2D pixel = Textures.get_Pixel();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, ((Control)this).get_Height() - 1, ViewportWidth, 1), TaskmasterTheme.SubtleBorder);
			Rectangle accentLine = default(Rectangle);
			Rectangle badgeBounds = default(Rectangle);
			Rectangle nameRect = default(Rectangle);
			for (int i = 0; i < _tabs.Count; i++)
			{
				TodoTab tab = _tabs[i];
				Rectangle r = _tabBounds[i];
				if (((Rectangle)(ref r)).get_Right() > 0 && r.X < ViewportWidth)
				{
					bool active = ActiveTabId.HasValue && tab.Id == ActiveTabId.Value;
					bool hover = i == _hoverIndex;
					Rectangle visualBounds = (Rectangle)(active ? new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1) : r);
					if (active)
					{
						DrawRoundedBorder(spriteBatch, visualBounds, TaskmasterTheme.TabActiveBorder, TaskmasterTheme.TabActiveFill);
					}
					else if (hover)
					{
						DrawRoundedRect(spriteBatch, visualBounds, TaskmasterTheme.TabHoverFill);
					}
					Color accent = (Color)(((_003F?)TaskmasterTheme.ParseAccentHex(tab.AccentColorHex)) ?? TaskmasterTheme.Gold);
					((Rectangle)(ref accentLine))._002Ector(visualBounds.X + 10, ((Rectangle)(ref visualBounds)).get_Bottom() - 2, Math.Max(8, visualBounds.Width - 20), 2);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, accentLine, active ? accent : (accent * (hover ? 0.72f : 0.48f)));
					if (!EditingTabId.HasValue || !(tab.Id == EditingTabId.Value))
					{
						Color nameColor = (active ? TaskmasterTheme.CreamWhite : TaskmasterTheme.TabInactiveText);
						string badge = BadgeText(tab);
						int badgeWidth = (int)badgeFont.MeasureString(badge).Width + 12;
						((Rectangle)(ref badgeBounds))._002Ector(((Rectangle)(ref visualBounds)).get_Right() - 10 - badgeWidth, visualBounds.Y + (visualBounds.Height - 18) / 2 + 1, badgeWidth, 18);
						((Rectangle)(ref nameRect))._002Ector(visualBounds.X + 10, visualBounds.Y + 2, Math.Max(0, badgeBounds.X - 10 - visualBounds.X - 10), visualBounds.Height - 2);
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, FitText(tab.Name, font, nameRect.Width), font, nameRect, nameColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
						bool complete = tab.TotalCount > 0 && tab.DoneCount == tab.TotalCount;
						Color badgeAccent = (complete ? TaskmasterTheme.Success : accent);
						DrawRoundedBorder(spriteBatch, badgeBounds, active ? (badgeAccent * 0.72f) : TaskmasterTheme.TabBadgeBorder, active ? TaskmasterTheme.TabBadgeActiveFill : TaskmasterTheme.TabBadgeFill, 2);
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, badge, badgeFont, badgeBounds, active ? TaskmasterTheme.CreamWhite : (complete ? badgeAccent : TaskmasterTheme.MutedCream), false, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}
			if (_dragging && _dragIndex >= 0 && _dragIndex < _tabBounds.Count)
			{
				int target = IndexAt(((Control)this).get_RelativeMousePosition());
				if (target >= 0 && target < _tabBounds.Count)
				{
					Rectangle t = _tabBounds[target];
					Rectangle marker = default(Rectangle);
					((Rectangle)(ref marker))._002Ector((target > _dragIndex) ? (((Rectangle)(ref t)).get_Right() - 2) : t.X, t.Y, 2, t.Height);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, marker, TaskmasterTheme.Gold);
				}
			}
			DrawEdgeFades(spriteBatch);
			Rectangle railBounds = default(Rectangle);
			((Rectangle)(ref railBounds))._002Ector(RailStartX, 2, 68, ((Control)this).get_Height() - 2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, railBounds, TaskmasterTheme.ChipFill);
			DrawScrollButton(spriteBatch, font, LeftArrowBounds, "<", CanScrollLeft, _hoverLeft);
			DrawScrollButton(spriteBatch, font, RightArrowBounds, ">", CanScrollRight, _hoverRight);
			bool plusHover = _hoverPlus && !Locked;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, PlusBounds, plusHover ? TaskmasterTheme.RowHover : TaskmasterTheme.ChipFill);
			DrawBorder(spriteBatch, PlusBounds, plusHover ? TaskmasterTheme.Gold : TaskmasterTheme.ChipBorder);
			Rectangle plusIconBounds = default(Rectangle);
			((Rectangle)(ref plusIconBounds))._002Ector(PlusBounds.X + (PlusBounds.Width - 14) / 2, PlusBounds.Y + (PlusBounds.Height - 14) / 2, 14, 14);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Plus, plusIconBounds, Locked ? TaskmasterTheme.DimText : (plusHover ? TaskmasterTheme.CreamWhite : TaskmasterTheme.MutedCream));
		}

		private void DrawScrollButton(SpriteBatch spriteBatch, BitmapFont font, Rectangle buttonBounds, string label, bool enabled, bool hovered)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			Color fill = ((enabled && hovered) ? TaskmasterTheme.RowHover : TaskmasterTheme.ChipFill);
			Color border = ((enabled && hovered) ? TaskmasterTheme.Gold : TaskmasterTheme.ChipBorder);
			Color text = (enabled ? TaskmasterTheme.MutedCream : TaskmasterTheme.DimText);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), buttonBounds, fill);
			DrawBorder(spriteBatch, buttonBounds, border);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, font, buttonBounds, text, false, (HorizontalAlignment)1, (VerticalAlignment)1);
		}

		private void DrawEdgeFades(SpriteBatch spriteBatch)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			int fadeWidth = Math.Min(10, ViewportWidth);
			if (fadeWidth <= 0)
			{
				return;
			}
			Texture2D pixel = Textures.get_Pixel();
			if (CanScrollLeft)
			{
				for (int j = 0; j < fadeWidth; j++)
				{
					float strength2 = (float)(fadeWidth - j) / (float)fadeWidth * 0.65f;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(j, 2, 1, ((Control)this).get_Height() - 2), TaskmasterTheme.ChipFill * strength2);
				}
			}
			if (CanScrollRight)
			{
				for (int i = 0; i < fadeWidth; i++)
				{
					float strength = (float)(i + 1) / (float)fadeWidth * 0.65f;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(ViewportWidth - fadeWidth + i, 2, 1, ((Control)this).get_Height() - 2), TaskmasterTheme.ChipFill * strength);
				}
			}
		}

		private static string FitText(string text, BitmapFont font, int maxWidth)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (maxWidth <= 0)
			{
				return "";
			}
			if (font.MeasureString(text).Width <= (float)maxWidth)
			{
				return text;
			}
			int length = text.Length;
			while (length > 0 && font.MeasureString(text.Substring(0, length) + "...").Width > (float)maxWidth)
			{
				length--;
			}
			if (length <= 0)
			{
				return "...";
			}
			return text.Substring(0, length) + "...";
		}

		private void DrawRoundedBorder(SpriteBatch spriteBatch, Rectangle bounds, Color border, Color fill, int radius = 3)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			DrawRoundedRect(spriteBatch, bounds, border, radius);
			if (bounds.Width > 2 && bounds.Height > 2)
			{
				DrawRoundedRect(spriteBatch, new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2), fill, Math.Max(1, radius - 1));
			}
		}

		private void DrawRoundedRect(SpriteBatch spriteBatch, Rectangle bounds, Color color, int radius = 3)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			radius = Math.Max(1, Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2));
			Texture2D pixel = Textures.get_Pixel();
			if (bounds.Height > radius * 2)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bounds.X, bounds.Y + radius, bounds.Width, bounds.Height - radius * 2), color);
			}
			for (int row = 0; row < radius; row++)
			{
				int inset = radius - row - 1;
				int width = Math.Max(0, bounds.Width - inset * 2);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bounds.X + inset, bounds.Y + row, width, 1), color);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bounds.X + inset, ((Rectangle)(ref bounds)).get_Bottom() - row - 1, width, 1), color);
			}
		}

		private void DrawBorder(SpriteBatch spriteBatch, Rectangle r, Color color)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, r.Y, r.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, ((Rectangle)(ref r)).get_Bottom() - 1, r.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(r.X, r.Y, 1, r.Height), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(((Rectangle)(ref r)).get_Right() - 1, r.Y, 1, r.Height), color);
		}
	}
}
