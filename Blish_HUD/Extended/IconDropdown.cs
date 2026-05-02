using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public class IconDropdown<T> : BaseDropdown<T>
	{
		private readonly SortedList<T, AsyncTexture2D> _itemIcons;

		private AsyncTexture2D _selectedItemIcon;

		private int _itemsPerRow = 5;

		private readonly Texture2D _textureEmptySlot;

		public int ItemsPerRow
		{
			get
			{
				return _itemsPerRow;
			}
			set
			{
				if (((Control)this).SetProperty<int>(ref _itemsPerRow, value, false, "ItemsPerRow"))
				{
					((Control)this).Invalidate();
				}
			}
		}

		private static Rectangle GetInner(Rectangle bounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Rectangle shrink = bounds;
			((Rectangle)(ref shrink)).Inflate(-7, -7);
			return shrink;
		}

		public IconDropdown()
		{
			base.Spacing = 2;
			_itemIcons = new SortedList<T, AsyncTexture2D>();
			_textureEmptySlot = EmbeddedResourceLoader.LoadTexture("156900.png");
		}

		protected override void DisposeControl()
		{
			Texture2D textureEmptySlot = _textureEmptySlot;
			if (textureEmptySlot != null)
			{
				((GraphicsResource)textureEmptySlot).Dispose();
			}
			DisposeIcons();
			((Control)this).DisposeControl();
		}

		private void DisposeIcons()
		{
			foreach (AsyncTexture2D value in _itemIcons.Values)
			{
				if (value != null)
				{
					value.Dispose();
				}
			}
			_itemIcons.Clear();
		}

		public void AddItem(T value, Func<string> tooltip, AsyncTexture2D icon)
		{
			if (base.AddItem(value, tooltip))
			{
				_itemIcons.Add(value, icon);
				OnItemsUpdated();
			}
		}

		public void AddItem(T value, AsyncTexture2D icon)
		{
			AddItem(value, null, icon);
		}

		protected override void OnDropdownMenuShown(DropdownMenu menu)
		{
			((FlowPanel)menu).set_FlowDirection((ControlFlowDirection)0);
		}

		protected override void OnItemRemoved(T item)
		{
			if (_itemIcons.TryGetValue(item, out var icon))
			{
				if (icon != null)
				{
					icon.Dispose();
				}
				_itemIcons.Remove(item);
			}
		}

		protected override void OnItemsCleared()
		{
			DisposeIcons();
		}

		protected override void OnItemsUpdated()
		{
			if (base.HasSelected && _itemIcons != null)
			{
				_selectedItemIcon = (_itemIcons.TryGetValue(base.SelectedItem, out var displayIcon) ? displayIcon : null);
			}
			else
			{
				_selectedItemIcon = null;
			}
		}

		private AsyncTexture2D GetItemIcon(T value)
		{
			if (_itemIcons.TryGetValue(value, out var icon))
			{
				return icon;
			}
			return null;
		}

		protected override void OnSelectedItemChanged(T previous, T current)
		{
			_selectedItemIcon = GetItemIcon(current);
		}

		private Point GetMaxItemSize()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			if (_itemIcons.Count == 0 || _itemsPerRow <= 0)
			{
				return Point.get_Zero();
			}
			int maxIconWidth = 0;
			int maxIconHeight = 0;
			foreach (AsyncTexture2D icon in _itemIcons.Values)
			{
				if (icon != null && icon.get_HasTexture())
				{
					if (icon.get_Width() > maxIconWidth)
					{
						maxIconWidth = icon.get_Width();
					}
					if (icon.get_Height() > maxIconHeight)
					{
						maxIconHeight = icon.get_Height();
					}
				}
			}
			return new Point(maxIconWidth, maxIconHeight);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureEmptySlot, bounds, Color.get_White());
			if (_selectedItemIcon != null && _selectedItemIcon.get_HasTexture())
			{
				Texture2D obj = AsyncTexture2D.op_Implicit(_selectedItemIcon);
				Rectangle inner = GetInner(bounds);
				Rectangle bounds2 = _selectedItemIcon.get_Bounds();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, inner.GetCenteredFit(((Rectangle)(ref bounds2)).get_Size()));
			}
		}

		protected override void PaintDropdownItem(DropdownMenu.DropdownItem ctrl, SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D icon = GetItemIcon(ctrl.Item);
			if (icon != null && icon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)ctrl, _textureEmptySlot, bounds, Color.get_White());
				Rectangle inner = GetInner(bounds);
				Rectangle bounds2 = icon.get_Bounds();
				Rectangle centered = inner.GetCenteredFit(((Rectangle)(ref bounds2)).get_Size());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)ctrl, AsyncTexture2D.op_Implicit(icon), centered);
				if (((Control)ctrl).get_MouseOver())
				{
					spriteBatch.DrawBorderOnCtrl((Control)(object)ctrl, bounds, Color.get_White() * 0.7f, 2);
				}
				else if (!base.HasSelected || !object.Equals(ctrl.Item, base.SelectedItem))
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)ctrl, Textures.get_Pixel(), bounds, Color.get_Black() * 0.4f);
				}
			}
		}

		protected override Point GetDropdownSize()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			if (_itemIcons.Count == 0 || _itemsPerRow <= 0)
			{
				return Point.get_Zero();
			}
			Point itemSize = GetDropdownItemSize();
			int columns = Math.Min(_itemsPerRow, _itemIcons.Count);
			int rows = (_itemIcons.Count + _itemsPerRow - 1) / _itemsPerRow;
			int num = columns * itemSize.X + (columns - 1) * base.Spacing + 2 * base.Margin;
			int totalHeight = rows * itemSize.Y + (rows - 1) * base.Spacing + 2 * base.Margin;
			return new Point(num, (base.MenuHeight > itemSize.Y + 2 * base.Margin) ? base.MenuHeight : totalHeight);
		}

		protected override Point GetDropdownItemSize()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return GetMaxItemSize();
		}
	}
}
