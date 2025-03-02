using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListViewModelFactory
	{
		[CompilerGenerated]
		private IStringLocalizer<ItemsList> _003Clocalizer_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IClipBoard _003Cclipboard_003EP;

		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private ItemTooltipViewModelFactory _003CtooltipViewModelFactory_003EP;

		public ItemsListViewModelFactory(IStringLocalizer<ItemsList> localizer, IEventAggregator eventAggregator, IClipBoard clipboard, ItemIcons icons, Customizer customizer, ItemTooltipViewModelFactory tooltipViewModelFactory)
		{
			_003Clocalizer_003EP = localizer;
			_003CeventAggregator_003EP = eventAggregator;
			_003Cclipboard_003EP = clipboard;
			_003Cicons_003EP = icons;
			_003Ccustomizer_003EP = customizer;
			_003CtooltipViewModelFactory_003EP = tooltipViewModelFactory;
			base._002Ector();
		}

		public ItemsListViewModel Create(Item item, bool isSelected)
		{
			return new ItemsListViewModel(_003Clocalizer_003EP, _003CeventAggregator_003EP, _003Cclipboard_003EP, _003Cicons_003EP, _003Ccustomizer_003EP, item, _003CtooltipViewModelFactory_003EP, isSelected);
		}
	}
}
