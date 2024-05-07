using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public abstract class SelectablesMenu<TId, TSelectable, TItem> : Panel where TId : notnull where TSelectable : class where TItem : MenuItem
	{
		private TSelectable? _selected;

		private Func<IEnumerable<TSelectable>, IEnumerable<TSelectable>>? _filter;

		private OrderedDictionary<TId, TItem> _items = new OrderedDictionary<TId, TItem>();

		private OrderedDictionary<TId, TSelectable> _selectables = new OrderedDictionary<TId, TSelectable>();

		private readonly Menu _menu;

		private bool _dirty;

		private bool _reflecting;

		private (int frames, float target) _scroll;

		private (int frames, float target) _relativeScroll;

		private (int frames, Control? item) _scrollItem;

		public TSelectable? Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				foreach (TItem value2 in _items.Values)
				{
					((MenuItem)value2).Deselect();
				}
				if (value == null || !_items.TryGetValue(ExtractId(value), out var item))
				{
					_selected = null;
					return;
				}
				_selected = value;
				_reflecting = true;
				((MenuItem)item).Select();
				_reflecting = false;
			}
		}

		public Func<IEnumerable<TSelectable>, IEnumerable<TSelectable>>? Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				_filter = value;
				_dirty = true;
			}
		}

		public Scrollbar? Scrollbar { get; set; }

		public int MenuItemHeight
		{
			get
			{
				return _menu.get_MenuItemHeight();
			}
			set
			{
				_menu.set_MenuItemHeight(value);
			}
		}

		public bool Empty => !Selectables.Any();

		public IReadOnlyDictionary<TId, TItem> Items => _items;

		public IReadOnlyDictionary<TId, TSelectable> Selectables => _selectables;

		public event Action<TSelectable?>? ItemSelected;

		public SelectablesMenu()
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_CanSelect(true);
			_menu = val;
		}

		protected abstract TItem Construct(TItem? item, TSelectable selectable);

		protected abstract TId ExtractId(TSelectable item);

		public void Refresh()
		{
			_dirty = true;
		}

		public void SetSelectable(TId id, TSelectable selectable)
		{
			_selectables.Add(id, selectable);
			_menu.set_CanSelect(true);
			_dirty = true;
		}

		public void RemoveSelectable(TId id)
		{
			_selectables.Remove(id);
			if (_items.TryGetValue(id, out var item))
			{
				_items.Remove(id);
				((Control)(object)item).set_Parent((Container)null);
				((Control)(object)item).Dispose();
			}
		}

		public void Placeholder(string text, string? tooltip = null)
		{
			if (Selected != null)
			{
				Select(null);
			}
			((Container)_menu).ClearChildren();
			_items = new OrderedDictionary<TId, TItem>();
			_menu.set_CanSelect(false);
			MenuItem item = _menu.AddMenuItem(text, (Texture2D)null);
			if (tooltip != null)
			{
				((Control)item).set_BasicTooltipText(tooltip);
			}
		}

		protected void Repopulate(Action repopulate)
		{
			TSelectable oldSelected = Selected;
			if (oldSelected != null)
			{
				Select(null);
			}
			((Container)_menu).ClearChildren();
			_items = new OrderedDictionary<TId, TItem>();
			_selectables = new OrderedDictionary<TId, TSelectable>();
			repopulate();
			if (oldSelected != null && Selectables.TryGetValue(ExtractId(oldSelected), out var selectable))
			{
				Select(selectable);
			}
			_dirty = true;
		}

		public void Select(TSelectable? selectable)
		{
			if (!_reflecting)
			{
				_selected = selectable;
				this.ItemSelected?.Invoke(selectable);
			}
		}

		public void SaveScroll(int frames = 2)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			SaveScroll(frames, (Scrollbar == null) ? 0f : (Scrollbar!.get_ScrollDistance() * (float)(((Control)_menu).get_Bottom() - ((Container)this).get_ContentRegion().Height)));
		}

		private void SaveScroll(int frames, float scroll)
		{
			_scroll = (Math.Max(_scroll.frames, frames), scroll);
		}

		public void SetScroll(float scroll, int frames = 2)
		{
			_relativeScroll = (Math.Max(frames, _relativeScroll.frames), scroll);
		}

		public bool HasScrollbar()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Container)this).get_ContentRegion().Height < ((Control)_menu).get_Bottom();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_dirty && !Empty)
			{
				_dirty = false;
				if (_scroll.frames != 0)
				{
					SaveScroll(2, _scroll.target);
				}
				else
				{
					SaveScroll();
				}
				if (!_items.Any())
				{
					((Container)_menu).ClearChildren();
				}
				OrderedDictionary<TId, TItem> oldItems = _items;
				_items = new OrderedDictionary<TId, TItem>();
				foreach (var (selectable, kv) in (Filter?.Invoke(Selectables.Values) ?? Selectables.Values).ZipLongest(oldItems))
				{
					if (selectable != null)
					{
						TItem item2 = Construct(kv.Value, selectable);
						_items[ExtractId(selectable)] = item2;
						((Control)(object)item2).set_Parent((Container)(object)_menu);
						continue;
					}
					TItem item = kv.Value;
					if (item != null)
					{
						((Control)(object)item).set_Parent((Container)null);
					}
				}
				Selected = Selected;
			}
			if (Scrollbar != null)
			{
				if (_relativeScroll.frames > 0)
				{
					SaveScroll(_relativeScroll.frames, _relativeScroll.target * (float)(((Control)_menu).get_Bottom() - ((Container)this).get_ContentRegion().Height));
					_relativeScroll.frames--;
				}
				if (_scrollItem.frames > 0 && _scrollItem.item != null)
				{
					SaveScroll(_scrollItem.frames, ((Panel)(object)this).NearestScrollTarget(_scrollItem.item));
					_scrollItem.frames--;
				}
				if (_scroll.frames > 0)
				{
					Scrollbar!.set_ScrollDistance(_scroll.target / (float)(((Control)_menu).get_Bottom() - ((Container)this).get_ContentRegion().Height));
					_scroll.frames--;
				}
			}
		}
	}
}
