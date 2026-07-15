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
		private const int TabPaddingX = 12;

		private const int TabGap = 2;

		private const int PlusWidth = 28;

		private const int PlusIconSize = 14;

		private const int DragThreshold = 6;

		private const int ScrollArrowWidth = 18;

		private const int ScrollStep = 90;

		private IReadOnlyList<TodoTab> _tabs = new List<TodoTab>();

		private readonly List<Rectangle> _tabBounds = new List<Rectangle>();

		private Rectangle _plusBounds;

		private int _hoverIndex = -1;

		private bool _hoverPlus;

		private int _dragIndex = -1;

		private Point _dragStart;

		private bool _dragging;

		private int _scrollX;

		private int _contentWidth;

		private bool _panning;

		private int _panStartX;

		private int _panStartScrollX;

		private int _lastWheelValue;

		private bool _wheelInitialized;

		public Guid? ActiveTabId { get; set; }

		public bool Locked { get; set; }

		private int ViewportWidth => Math.Max(0, ((Control)this).get_Width() - 28 - 2);

		private bool CanScrollLeft => _scrollX > 0;

		private bool CanScrollRight => _scrollX < MaxScroll();

		private Rectangle LeftArrowBounds => new Rectangle(0, 2, 18, ((Control)this).get_Height() - 2);

		private Rectangle RightArrowBounds => new Rectangle(ViewportWidth - 18, 2, 18, ((Control)this).get_Height() - 2);

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
			((Control)this).Invalidate();
		}

		private static string BadgeText(TodoTab tab)
		{
			return $"{tab.DoneCount}/{tab.TotalCount}";
		}

		private void RecomputeLayout()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			_tabBounds.Clear();
			BitmapFont font = GameService.Content.get_DefaultFont14();
			BitmapFont badgeFont = GameService.Content.get_DefaultFont12();
			int x = 0;
			foreach (TodoTab tab in _tabs)
			{
				int w = (int)font.MeasureString(tab.Name).Width + 6 + (int)badgeFont.MeasureString(BadgeText(tab)).Width + 24;
				_tabBounds.Add(new Rectangle(x, 2, w, ((Control)this).get_Height() - 2));
				x += w + 2;
			}
			_contentWidth = x;
			_scrollX = ClampScroll(_scrollX);
			for (int i = 0; i < _tabBounds.Count; i++)
			{
				Rectangle r = _tabBounds[i];
				_tabBounds[i] = new Rectangle(r.X - _scrollX, r.Y, r.Width, r.Height);
			}
			_plusBounds = new Rectangle(((Control)this).get_Width() - 28, 2, 28, ((Control)this).get_Height() - 2);
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
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
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

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			Point p = ((Control)this).get_RelativeMousePosition();
			_hoverIndex = IndexAt(p);
			_hoverPlus = ((Rectangle)(ref _plusBounds)).Contains(p);
			((Control)this).set_BasicTooltipText((_hoverPlus && Locked) ? "Locked - unlock to add tabs" : null);
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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseWheelScrolled(e);
			MouseState state = Mouse.GetState();
			int current = ((MouseState)(ref state)).get_ScrollWheelValue();
			if (_wheelInitialized)
			{
				int delta = current - _lastWheelValue;
				if (delta != 0)
				{
					_scrollX = ClampScroll(_scrollX - delta / 4);
				}
			}
			_lastWheelValue = current;
			_wheelInitialized = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			_hoverIndex = -1;
			_hoverPlus = false;
			_panning = false;
			((Control)this).set_BasicTooltipText((string)null);
			((Control)this).Invalidate();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point p = ((Control)this).get_RelativeMousePosition();
			Rectangle val;
			if (CanScrollLeft)
			{
				val = LeftArrowBounds;
				if (((Rectangle)(ref val)).Contains(p))
				{
					_scrollX = ClampScroll(_scrollX - 90);
					((Control)this).Invalidate();
					return;
				}
			}
			if (CanScrollRight)
			{
				val = RightArrowBounds;
				if (((Rectangle)(ref val)).Contains(p))
				{
					_scrollX = ClampScroll(_scrollX + 90);
					((Control)this).Invalidate();
					return;
				}
			}
			if (((Rectangle)(ref _plusBounds)).Contains(p))
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
			else if (MaxScroll() > 0)
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
							target = ((((Control)this).get_RelativeMousePosition().X > _plusBounds.X) ? (_tabs.Count - 1) : _dragIndex);
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
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			RecomputeLayout();
			BitmapFont font = GameService.Content.get_DefaultFont14();
			BitmapFont badgeFont = GameService.Content.get_DefaultFont12();
			Texture2D pixel = Textures.get_Pixel();
			Rectangle accentBar = default(Rectangle);
			Rectangle nameRect = default(Rectangle);
			for (int i = 0; i < _tabs.Count; i++)
			{
				TodoTab tab = _tabs[i];
				Rectangle r = _tabBounds[i];
				bool num = ActiveTabId.HasValue && tab.Id == ActiveTabId.Value;
				bool hover = i == _hoverIndex;
				if (num)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, r, TaskmasterTheme.TabActiveFill);
				}
				else if (hover)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, r, TaskmasterTheme.RowHover);
				}
				Color accent = (Color)(((_003F?)TaskmasterTheme.ParseAccentHex(tab.AccentColorHex)) ?? TaskmasterTheme.Gold);
				((Rectangle)(ref accentBar))._002Ector(r.X, r.Y, 3, r.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, accentBar, accent);
				Color nameColor = (num ? TaskmasterTheme.CreamWhite : TaskmasterTheme.TabInactiveText);
				((Rectangle)(ref nameRect))._002Ector(r.X + 12, r.Y, r.Width - 24, r.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, tab.Name, font, nameRect, nameColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				string badge = BadgeText(tab);
				Color badgeColor = ((tab.TotalCount > 0 && tab.DoneCount == tab.TotalCount) ? TaskmasterTheme.Success : TaskmasterTheme.Gold);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, badge, badgeFont, nameRect, badgeColor, false, (HorizontalAlignment)2, (VerticalAlignment)1);
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
			if (CanScrollLeft)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, LeftArrowBounds, TaskmasterTheme.ChipFill);
				DrawBorder(spriteBatch, LeftArrowBounds, TaskmasterTheme.ChipBorder);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "<", font, LeftArrowBounds, TaskmasterTheme.CreamWhite, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			if (CanScrollRight)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, RightArrowBounds, TaskmasterTheme.ChipFill);
				DrawBorder(spriteBatch, RightArrowBounds, TaskmasterTheme.ChipBorder);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ">", font, RightArrowBounds, TaskmasterTheme.CreamWhite, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			bool plusHover = _hoverPlus && !Locked;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, _plusBounds, plusHover ? TaskmasterTheme.RowHover : TaskmasterTheme.ChipFill);
			DrawBorder(spriteBatch, _plusBounds, plusHover ? TaskmasterTheme.Gold : TaskmasterTheme.ChipBorder);
			Rectangle plusIconBounds = default(Rectangle);
			((Rectangle)(ref plusIconBounds))._002Ector(_plusBounds.X + (_plusBounds.Width - 14) / 2, _plusBounds.Y + (_plusBounds.Height - 14) / 2, 14, 14);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TaskmasterIcons.Plus, plusIconBounds, Locked ? TaskmasterTheme.DimText : (plusHover ? TaskmasterTheme.CreamWhite : TaskmasterTheme.MutedCream));
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
