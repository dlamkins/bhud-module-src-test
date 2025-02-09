using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items;
using SL.Common;

namespace SL.ChatLinks.UI
{
	public sealed class MainWindowViewModel : ViewModel
	{
		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private ItemsTabViewModelFactory _003CitemsTabViewModelFactory_003EP;

		[CompilerGenerated]
		private IStringLocalizer<MainWindow> _003Clocalizer_003EP;

		private bool _visible;

		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				SetField(ref _visible, value, "Visible");
			}
		}

		public string Id => "sliekens.chat-links.main-window";

		public string Title => (string)_003Clocalizer_003EP["Title"];

		public string ItemsTabName => (string)_003Clocalizer_003EP["Items"];

		public AsyncTexture2D BackgroundTexture => AsyncTexture2D.FromAssetId(155985);

		public AsyncTexture2D EmblemTexture => AsyncTexture2D.FromAssetId(2237584);

		public MainWindowViewModel(IEventAggregator eventAggregator, ItemsTabViewModelFactory itemsTabViewModelFactory, IStringLocalizer<MainWindow> localizer)
		{
			_003CeventAggregator_003EP = eventAggregator;
			_003CitemsTabViewModelFactory_003EP = itemsTabViewModelFactory;
			_003Clocalizer_003EP = localizer;
			base._002Ector();
		}

		public void Initialize()
		{
			_003CeventAggregator_003EP.Subscribe(new Action<LocaleChanged>(OnLocaleChanged));
			_003CeventAggregator_003EP.Subscribe(new Action<MainIconClicked>(MainIconClicked));
			_003CeventAggregator_003EP.Subscribe(new Action<ModuleUnloading>(ModuleUnloading));
		}

		private void OnLocaleChanged(LocaleChanged obj)
		{
			OnPropertyChanged("Title");
			OnPropertyChanged("ItemsTabName");
		}

		private void MainIconClicked(MainIconClicked obj)
		{
			Visible = !Visible;
		}

		public ItemsTabViewModel CreateItemsTabViewModel()
		{
			return _003CitemsTabViewModelFactory_003EP.Create();
		}

		private void ModuleUnloading(ModuleUnloading obj)
		{
			_003CeventAggregator_003EP.Unsubscribe<LocaleChanged>(new Action<LocaleChanged>(OnLocaleChanged));
			_003CeventAggregator_003EP.Unsubscribe<MainIconClicked>(new Action<MainIconClicked>(MainIconClicked));
			_003CeventAggregator_003EP.Unsubscribe<ModuleUnloading>(new Action<ModuleUnloading>(ModuleUnloading));
		}
	}
}
