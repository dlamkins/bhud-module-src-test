using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Quarry.UserInterface.Controls
{
	public class CardGrid : Panel
	{
		private readonly struct Item
		{
			public Control Control { get; }

			public bool FullWidth { get; }

			public Item(Control control, bool fullWidth)
			{
				Control = control;
				FullWidth = fullWidth;
			}
		}

		public const int MinCardWidth = 380;

		public const int CardHeight = 92;

		public const int Gutter = 8;

		public const int ScrollbarAllowance = 12;

		private const int AbsoluteMinCardWidth = 120;

		private readonly List<Item> items = new List<Item>();

		public int ContentHeight { get; private set; }

		public int Columns => ColumnsFor(UsableWidth);

		public Point CardSize => new Point(CardWidth(UsableWidth, Columns), 92);

		private int UsableWidth => ((Container)this).get_ContentRegion().Width - 12 - 8;

		private static int ColumnsFor(int usableWidth)
		{
			return Math.Max(1, (usableWidth + 8) / 388);
		}

		public static int CardWidth(int usableWidth, int columns)
		{
			if (columns <= 0)
			{
				return 120;
			}
			return Math.Max(120, (usableWidth - (columns - 1) * 8) / columns);
		}

		public void Add(Control control, bool fullWidth = false)
		{
			control.set_Parent((Container)(object)this);
			items.Add(new Item(control, fullWidth));
			((Control)this).Invalidate();
		}

		public void ClearItems()
		{
			foreach (Item item in items)
			{
				item.Control.Dispose();
			}
			items.Clear();
			ContentHeight = 0;
			((Control)this).Invalidate();
		}

		public override void RecalculateLayout()
		{
			((Panel)this).RecalculateLayout();
			LayoutItems();
		}

		private void LayoutItems()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			if (items.Count == 0)
			{
				ContentHeight = 0;
				return;
			}
			int gutter = 8;
			int outer = gutter / 2;
			int usable = UsableWidth;
			if (usable <= 0)
			{
				return;
			}
			int columns = ColumnsFor(usable);
			int cardWidth = CardWidth(usable, columns);
			int x = ((Container)this).get_ContentRegion().X + outer;
			int y = ((Container)this).get_ContentRegion().Y + outer;
			int column = 0;
			foreach (Item item in items.ToList())
			{
				if (item.FullWidth)
				{
					if (column > 0)
					{
						y += 92 + gutter;
						column = 0;
					}
					item.Control.set_Location(new Point(x, y));
					item.Control.set_Width(usable);
					y += item.Control.get_Height() + gutter;
				}
				else
				{
					if (column == columns)
					{
						column = 0;
						y += 92 + gutter;
					}
					item.Control.set_Location(new Point(x + column * (cardWidth + gutter), y));
					item.Control.set_Size(new Point(cardWidth, 92));
					column++;
				}
			}
			if (column > 0)
			{
				y += 92 + gutter;
			}
			ContentHeight = y - ((Container)this).get_ContentRegion().Y + outer;
		}

		public CardGrid()
			: this()
		{
		}
	}
}
