using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace MysticCrafting.Module.Menu
{
	public interface ICategoryMenuItem
	{
		int MenuItemHeight { get; set; }

		bool Selected { get; }

		CategoryMenuItem SelectedMenuItem { get; }

		bool ShouldShift { get; set; }

		event EventHandler<ControlActivatedEventArgs> ItemSelected;

		void Select();

		void Select(CategoryMenuItem menuItem);

		void Select(CategoryMenuItem menuItem, List<CategoryMenuItem> itemPath);

		void Deselect();

		void Collapse();
	}
}
