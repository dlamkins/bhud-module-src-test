using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using SL.ChatLinks.UI.Tabs.Items.Collections;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabViewModelFactory
	{
		[CompilerGenerated]
		private ILoggerFactory _003CloggerFactory_003EP;

		[CompilerGenerated]
		private ItemSearch _003Csearch_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private ItemsListViewModelFactory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private ChatLinkEditorViewModelFactory _003CchatLinkEditorViewModelFactory_003EP;

		public ItemsTabViewModelFactory(ILoggerFactory loggerFactory, ItemSearch search, Customizer customizer, ItemsListViewModelFactory itemsListViewModelFactory, ChatLinkEditorViewModelFactory chatLinkEditorViewModelFactory)
		{
			_003CloggerFactory_003EP = loggerFactory;
			_003Csearch_003EP = search;
			_003Ccustomizer_003EP = customizer;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CchatLinkEditorViewModelFactory_003EP = chatLinkEditorViewModelFactory;
			base._002Ector();
		}

		public ItemsTabViewModel Create()
		{
			return new ItemsTabViewModel(_003CloggerFactory_003EP.CreateLogger<ItemsTabViewModel>(), _003Csearch_003EP, _003Ccustomizer_003EP, _003CitemsListViewModelFactory_003EP, _003CchatLinkEditorViewModelFactory_003EP);
		}
	}
}
