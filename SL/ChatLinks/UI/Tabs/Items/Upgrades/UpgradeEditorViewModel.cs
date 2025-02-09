using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeEditorViewModel : ViewModel, IDisposable
	{
		private bool _customizing;

		private readonly IStringLocalizer<UpgradeEditor> _localizer;

		private readonly IEventAggregator _eventAggregator;

		private readonly IClipBoard _clipboard;

		private readonly UpgradeSelectorViewModelFactory _upgradeComponentListViewModelFactory;

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

		public bool IsCustomizable => TargetItem is IUpgradable;

		public string CustomizeLabel => (string)_localizer["Customize"];

		public string CancelLabel => (string)_localizer["Cancel"];

		public RelayCommand CustomizeCommand => new RelayCommand(delegate
		{
			Customizing = !Customizing;
		});

		public RelayCommand RemoveCommand => new RelayCommand(delegate
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = null;
			_eventAggregator.Publish(new UpgradeSlotChanged());
		}, () => (object)UpgradeSlotViewModel.SelectedUpgradeComponent != null);

		public string CopyNameLabel => (string)_localizer["Copy Name"];

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.Name;
			if (text != null)
			{
				_clipboard.SetText(text);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public string CopyChatLinkLabel => (string)_localizer["Copy Chat Link"];

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.ChatLink;
			if (text != null)
			{
				_clipboard.SetText(text);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public string OpenWikiLabel => (string)_localizer["Open Wiki"];

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			string text = EffectiveUpgradeComponent?.ChatLink;
			if (text != null)
			{
				Process.Start(_localizer["Wiki search", new object[1] { WebUtility.UrlEncode(text) }]);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public string OpenApiLabel => (string)_localizer["Open API"];

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			int? num = EffectiveUpgradeComponent?.Id;
			if (num.HasValue)
			{
				Process.Start(_localizer["Item API", new object[1] { num }]);
			}
		}, () => (object)EffectiveUpgradeComponent != null);

		public RelayCommand HideCommand => new RelayCommand(delegate
		{
			Customizing = false;
		});

		public UpgradeSlotViewModel UpgradeSlotViewModel { get; }

		public Item TargetItem { get; }

		public UpgradeComponent? EffectiveUpgradeComponent => UpgradeSlotViewModel.SelectedUpgradeComponent ?? UpgradeSlotViewModel.DefaultUpgradeComponent;

		public string RemoveItemLabel
		{
			get
			{
				UpgradeSlotViewModel upgradeSlotViewModel = UpgradeSlotViewModel;
				if (upgradeSlotViewModel == null)
				{
					goto IL_0074;
				}
				LocalizedString localizedString;
				if ((object)upgradeSlotViewModel.SelectedUpgradeComponent == null)
				{
					if ((object)upgradeSlotViewModel.DefaultUpgradeComponent == null)
					{
						goto IL_0074;
					}
					localizedString = _localizer["Remove upgrade", new object[1] { UpgradeSlotViewModel.DefaultUpgradeComponent!.Name }];
				}
				else
				{
					localizedString = _localizer["Remove upgrade", new object[1] { UpgradeSlotViewModel.SelectedUpgradeComponent!.Name }];
				}
				goto IL_0085;
				IL_0085:
				return (string)localizedString;
				IL_0074:
				localizedString = _localizer["Remove"];
				goto IL_0085;
			}
		}

		public UpgradeSlotType UpgradeSlotType => UpgradeSlotViewModel.Type;

		public UpgradeEditorViewModel(IStringLocalizer<UpgradeEditor> localizer, IEventAggregator eventAggregator, IClipBoard clipboard, UpgradeSlotViewModel upgradeSlotViewModel, UpgradeSelectorViewModelFactory upgradeComponentListViewModelFactory, Item target)
		{
			_localizer = localizer;
			_eventAggregator = eventAggregator;
			_clipboard = clipboard;
			_upgradeComponentListViewModelFactory = upgradeComponentListViewModelFactory;
			UpgradeSlotViewModel = upgradeSlotViewModel;
			TargetItem = target;
			eventAggregator.Subscribe(new Action<LocaleChanged>(OnLocaleChanged));
			upgradeSlotViewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
			{
				string propertyName = args.PropertyName;
				if (propertyName == "DefaultUpgradeComponent" || propertyName == "SelectedUpgradeComponent")
				{
					OnPropertyChanged("EffectiveUpgradeComponent");
				}
			};
		}

		private void OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("CustomizeLabel");
			OnPropertyChanged("CancelLabel");
			OnPropertyChanged("CopyNameLabel");
			OnPropertyChanged("CopyChatLinkLabel");
			OnPropertyChanged("OpenWikiLabel");
			OnPropertyChanged("OpenApiLabel");
			OnPropertyChanged("RemoveItemLabel");
		}

		public UpgradeSelectorViewModel CreateUpgradeComponentListViewModel()
		{
			UpgradeSelectorViewModel upgradeSelectorViewModel = _upgradeComponentListViewModelFactory.Create(TargetItem, UpgradeSlotViewModel.Type, UpgradeSlotViewModel.SelectedUpgradeComponent);
			upgradeSelectorViewModel.Selected += new EventHandler<UpgradeComponent>(Selected);
			upgradeSelectorViewModel.Deselected += new EventHandler(Deselected);
			return upgradeSelectorViewModel;
		}

		private void Selected(object sender, UpgradeComponent args)
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = args;
			Customizing = false;
			_eventAggregator.Publish(new UpgradeSlotChanged());
		}

		private void Deselected(object sender, EventArgs args)
		{
			UpgradeSlotViewModel.SelectedUpgradeComponent = null;
			Customizing = false;
			_eventAggregator.Publish(new UpgradeSlotChanged());
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Action<LocaleChanged>(OnLocaleChanged));
		}
	}
}
