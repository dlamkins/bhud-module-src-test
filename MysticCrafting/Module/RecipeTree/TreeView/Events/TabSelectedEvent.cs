using System;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;

namespace MysticCrafting.Module.RecipeTree.TreeView.Events
{
	public class TabSelectedEvent : EventArgs
	{
		public ItemTab SelectedTab { get; }

		public TabSelectedEvent(ItemTab selectedTab)
		{
			SelectedTab = selectedTab;
		}
	}
}
