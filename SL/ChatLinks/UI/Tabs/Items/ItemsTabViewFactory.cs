using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabViewFactory
	{
		[CompilerGenerated]
		private ILoggerFactory _003CloggerFactory_003EP;

		[CompilerGenerated]
		private ItemsTabViewModelFactory _003CitemsTabViewModelFactory_003EP;

		public ItemsTabViewFactory(ILoggerFactory loggerFactory, ItemsTabViewModelFactory itemsTabViewModelFactory)
		{
			_003CloggerFactory_003EP = loggerFactory;
			_003CitemsTabViewModelFactory_003EP = itemsTabViewModelFactory;
			base._002Ector();
		}

		public ItemsTabView Create()
		{
			ItemsTabViewModel itemsTabViewModel = _003CitemsTabViewModelFactory_003EP.Create();
			return new ItemsTabView(_003CloggerFactory_003EP.CreateLogger<ItemsTabView>(), itemsTabViewModel);
		}
	}
}
