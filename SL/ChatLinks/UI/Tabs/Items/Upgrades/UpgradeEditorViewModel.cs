using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using GuildWars2.Items;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeEditorViewModel : ViewModel
	{
		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IClipBoard _003Cclipboard_003EP;

		[CompilerGenerated]
		private UpgradeSelectorViewModelFactory _003CupgradeComponentListViewModelFactory_003EP;

		private bool _customizing;

		public bool Customizing
		{
			get
			{
				return _customizing;
			}
			set
			{
				SetField(ref _customizing, value, "Customizing");
			}
		}

		public RelayCommand CustomizeCommand => new RelayCommand(delegate
		{
			Customizing = !Customizing;
		});

		public RelayCommand RemoveCommand => new RelayCommand(delegate
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = null;
			OnPropertyChanged("EffectiveUpgradeComponent");
			_003CeventAggregator_003EP.Publish(new UpgradeSlotChanged());
		}, () => (object)UpgradeSlotViewModel.SelectedUpgradeComponent != null);

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.Name;
			if (text != null)
			{
				_003Cclipboard_003EP.SetText(text);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.ChatLink;
			if (text != null)
			{
				_003Cclipboard_003EP.SetText(text);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.ChatLink;
			if (text != null)
			{
				Process.Start("https://wiki.guildwars2.com/wiki/?search=" + WebUtility.UrlEncode(text));
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			int? num = EffectiveUpgradeComponent?.Id;
			if (num.HasValue)
			{
				Process.Start($"https://api.guildwars2.com/v2/items/{num}?v=latest");
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public RelayCommand HideCommand => new RelayCommand(delegate
		{
			Customizing = false;
		});

		public UpgradeSlotViewModel UpgradeSlotViewModel { get; }

		public Item TargetItem { get; }

		public UpgradeComponent? EffectiveUpgradeComponent => UpgradeSlotViewModel.SelectedUpgradeComponent ?? UpgradeSlotViewModel.DefaultUpgradeComponent;

		public string RemoveItemText
		{
			get
			{
				UpgradeSlotViewModel upgradeSlotViewModel = UpgradeSlotViewModel;
				if (upgradeSlotViewModel != null)
				{
					if ((object)upgradeSlotViewModel.SelectedUpgradeComponent != null)
					{
						return "Remove " + UpgradeSlotViewModel.SelectedUpgradeComponent!.Name;
					}
					if ((object)upgradeSlotViewModel.DefaultUpgradeComponent != null)
					{
						return "Remove " + UpgradeSlotViewModel.DefaultUpgradeComponent!.Name;
					}
				}
				return "Remove";
			}
		}

		public UpgradeSlotType UpgradeSlotType => UpgradeSlotViewModel.Type;

		public UpgradeEditorViewModel(IEventAggregator eventAggregator, IClipBoard clipboard, UpgradeSlotViewModel upgradeSlotViewModel, UpgradeSelectorViewModelFactory upgradeComponentListViewModelFactory, Item target)
		{
			_003CeventAggregator_003EP = eventAggregator;
			_003Cclipboard_003EP = clipboard;
			_003CupgradeComponentListViewModelFactory_003EP = upgradeComponentListViewModelFactory;
			UpgradeSlotViewModel = upgradeSlotViewModel;
			TargetItem = target;
			base._002Ector();
		}

		public UpgradeSelectorViewModel CreateUpgradeComponentListViewModel()
		{
			UpgradeSelectorViewModel upgradeSelectorViewModel = _003CupgradeComponentListViewModelFactory_003EP.Create(TargetItem, UpgradeSlotViewModel.Type, UpgradeSlotViewModel.SelectedUpgradeComponent);
			upgradeSelectorViewModel.Selected += new EventHandler<UpgradeComponent>(Selected);
			upgradeSelectorViewModel.Deselected += new EventHandler(Deselected);
			return upgradeSelectorViewModel;
		}

		private void Selected(object sender, UpgradeComponent args)
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = args;
			Customizing = false;
			OnPropertyChanged("EffectiveUpgradeComponent");
			_003CeventAggregator_003EP.Publish(new UpgradeSlotChanged());
		}

		private void Deselected(object sender, EventArgs args)
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = null;
			Customizing = false;
			OnPropertyChanged("EffectiveUpgradeComponent");
			_003CeventAggregator_003EP.Publish(new UpgradeSlotChanged());
		}
	}
}
