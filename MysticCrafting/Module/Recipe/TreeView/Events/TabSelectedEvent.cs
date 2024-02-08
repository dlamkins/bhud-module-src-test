using System;
using MysticCrafting.Module.Recipe.TreeView.Controls;

namespace MysticCrafting.Module.Recipe.TreeView.Events
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
