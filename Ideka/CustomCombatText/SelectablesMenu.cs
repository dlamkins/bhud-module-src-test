using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public abstract class SelectablesMenu<TId, TSelectable> : Panel where TId : notnull where TSelectable : class
	{
		private TSelectable? _selected;

		protected readonly Menu _menu;

		private readonly Dictionary<TId, MenuItem> _items = new Dictionary<TId, MenuItem>();

		private bool _reflecting;

		public TSelectable? Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				foreach (MenuItem value2 in _items.Values)
				{
					value2.Deselect();
				}
				TId id = ExtractId(value);
				if (id == null || !_items.TryGetValue(id, out var item))
				{
					_selected = null;
					return;
				}
				_selected = value;
				_reflecting = true;
				item.Select();
				_reflecting = false;
			}
		}

		public event Action<TSelectable?>? ItemSelected;

		public SelectablesMenu()
			: this()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_CanSelect(true);
			_menu = val;
		}

		protected abstract TId? ExtractId(TSelectable? item);

		protected void Update(TId id, Action<MenuItem> act)
		{
			if (_items.TryGetValue(id, out var item))
			{
				act(item);
			}
		}

		public T SetItem<T>(TId id, T item) where T : MenuItem
		{
			_items[id] = (MenuItem)(object)item;
			return item;
		}

		public void Placeholder(string text, string? tooltip = null)
		{
			if (ExtractId(Selected) != null)
			{
				Select(null);
			}
			_items.Clear();
			((Container)_menu).ClearChildren();
			_menu.set_CanSelect(false);
			MenuItem item = _menu.AddMenuItem(text, (Texture2D)null);
			if (tooltip != null)
			{
				((Control)item).set_BasicTooltipText(tooltip);
			}
		}

		protected void Repopulate(Action repopulate)
		{
			TId oldId = ExtractId(Selected);
			if (oldId != null)
			{
				Select(null);
			}
			((Container)_menu).ClearChildren();
			repopulate();
			if (oldId != null && _items.TryGetValue(oldId, out var item))
			{
				item.Select();
			}
		}

		public void Select(TSelectable? selectable)
		{
			if (!_reflecting)
			{
				_selected = selectable;
				this.ItemSelected?.Invoke(selectable);
			}
		}
	}
}