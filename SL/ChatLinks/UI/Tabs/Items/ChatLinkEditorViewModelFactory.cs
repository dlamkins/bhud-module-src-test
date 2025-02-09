using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ChatLinkEditorViewModelFactory
	{
		[CompilerGenerated]
		private ItemTooltipViewModelFactory _003CitemTooltipViewModelFactory_003EP;

		[CompilerGenerated]
		private UpgradeEditorViewModelFactory _003CupgradeEditorViewModelFactory_003EP;

		[CompilerGenerated]
		private IOptionsMonitor<ChatLinkOptions> _003Coptions_003EP;

		[CompilerGenerated]
		private IStringLocalizer<ChatLinkEditor> _003Clocalizer_003EP;

		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private IClipBoard _003Cclipboard_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IDbContextFactory _003CcontextFactory_003EP;

		public ChatLinkEditorViewModelFactory(ItemTooltipViewModelFactory itemTooltipViewModelFactory, UpgradeEditorViewModelFactory upgradeEditorViewModelFactory, IOptionsMonitor<ChatLinkOptions> options, IStringLocalizer<ChatLinkEditor> localizer, ItemIcons icons, Customizer customizer, IClipBoard clipboard, IEventAggregator eventAggregator, IDbContextFactory contextFactory)
		{
			_003CitemTooltipViewModelFactory_003EP = itemTooltipViewModelFactory;
			_003CupgradeEditorViewModelFactory_003EP = upgradeEditorViewModelFactory;
			_003Coptions_003EP = options;
			_003Clocalizer_003EP = localizer;
			_003Cicons_003EP = icons;
			_003Ccustomizer_003EP = customizer;
			_003Cclipboard_003EP = clipboard;
			_003CeventAggregator_003EP = eventAggregator;
			_003CcontextFactory_003EP = contextFactory;
			base._002Ector();
		}

		public ChatLinkEditorViewModel Create(Item item)
		{
			return new ChatLinkEditorViewModel(_003Coptions_003EP, _003Clocalizer_003EP, _003CeventAggregator_003EP, _003CcontextFactory_003EP, _003CitemTooltipViewModelFactory_003EP, _003CupgradeEditorViewModelFactory_003EP, _003Cicons_003EP, _003Ccustomizer_003EP, _003Cclipboard_003EP, item);
		}
	}
}
