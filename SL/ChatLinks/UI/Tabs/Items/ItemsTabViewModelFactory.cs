using System.Runtime.CompilerServices;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabViewModelFactory
	{
		[CompilerGenerated]
		private ILoggerFactory _003CloggerFactory_003EP;

		[CompilerGenerated]
		private IStringLocalizer<ItemsTabView> _003Clocalizer_003EP;

		[CompilerGenerated]
		private IOptionsMonitor<ChatLinkOptions> _003Coptions_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private ItemSearch _003Csearch_003EP;

		[CompilerGenerated]
		private ItemsListViewModelFactory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private ChatLinkEditorViewModelFactory _003CchatLinkEditorViewModelFactory_003EP;

		public ItemsTabViewModelFactory(ILoggerFactory loggerFactory, IStringLocalizer<ItemsTabView> localizer, IOptionsMonitor<ChatLinkOptions> options, IEventAggregator eventAggregator, ItemSearch search, ItemsListViewModelFactory itemsListViewModelFactory, ChatLinkEditorViewModelFactory chatLinkEditorViewModelFactory)
		{
			_003CloggerFactory_003EP = loggerFactory;
			_003Clocalizer_003EP = localizer;
			_003Coptions_003EP = options;
			_003CeventAggregator_003EP = eventAggregator;
			_003Csearch_003EP = search;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CchatLinkEditorViewModelFactory_003EP = chatLinkEditorViewModelFactory;
			base._002Ector();
		}

		public ItemsTabViewModel Create()
		{
			return new ItemsTabViewModel(_003CloggerFactory_003EP.CreateLogger<ItemsTabViewModel>(), _003Clocalizer_003EP, _003Coptions_003EP, _003CeventAggregator_003EP, _003Csearch_003EP, _003CitemsListViewModelFactory_003EP, _003CchatLinkEditorViewModelFactory_003EP);
		}
	}
}
