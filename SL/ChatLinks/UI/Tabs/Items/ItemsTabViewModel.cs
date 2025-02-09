using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Blish_HUD;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabViewModel : ViewModel
	{
		[CompilerGenerated]
		private ILogger<ItemsTabViewModel> _003Clogger_003EP;

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

		private string _searchText;

		private bool _searching;

		private int _resultTotal;

		private string _resultText;

		private EventHandler? _searchCancelled;

		private Item? _selectedItem;

		private readonly SemaphoreSlim _searchLock;

		public Item? SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				SetField(ref _selectedItem, value, "SelectedItem");
			}
		}

		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				SetField(ref _searchText, value, "SearchText");
			}
		}

		public bool Searching
		{
			get
			{
				return _searching;
			}
			set
			{
				SetField(ref _searching, value, "Searching");
			}
		}

		public string ResultText
		{
			get
			{
				return _resultText;
			}
			set
			{
				SetField(ref _resultText, value, "ResultText");
			}
		}

		public ObservableCollection<ItemsListViewModel> SearchResults { get; }

		public int ResultTotal
		{
			get
			{
				return _resultTotal;
			}
			private set
			{
				SetField(ref _resultTotal, value, "ResultTotal");
			}
		}

		public ICommand SearchCommand => new AsyncRelayCommand(async delegate
		{
			await Task.Run((Func<Task>)OnSearch);
		});

		public string SearchPlaceholderText => (string)_003Clocalizer_003EP["Search placeholder"];

		public ItemsTabViewModel(ILogger<ItemsTabViewModel> logger, IStringLocalizer<ItemsTabView> localizer, IOptionsMonitor<ChatLinkOptions> options, IEventAggregator eventAggregator, ItemSearch search, ItemsListViewModelFactory itemsListViewModelFactory, ChatLinkEditorViewModelFactory chatLinkEditorViewModelFactory)
		{
			_003Clogger_003EP = logger;
			_003Clocalizer_003EP = localizer;
			_003Coptions_003EP = options;
			_003CeventAggregator_003EP = eventAggregator;
			_003Csearch_003EP = search;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CchatLinkEditorViewModelFactory_003EP = chatLinkEditorViewModelFactory;
			_searchText = "";
			_resultText = "";
			_searchLock = new SemaphoreSlim(1, 1);
			SearchResults = new ObservableCollection<ItemsListViewModel>();
			base._002Ector();
		}

		public void Initialize()
		{
			_003CeventAggregator_003EP.Subscribe(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseDownloaded, ValueTask>(OnDatabaseDownloaded));
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseSyncCompleted, ValueTask>(OnDatabaseSyncCompleted));
		}

		private async ValueTask OnLocaleChanged(LocaleChanged args)
		{
			OnPropertyChanged("SearchPlaceholderText");
			await Task.Run((Func<Task>)OnSearch);
		}

		private async ValueTask OnDatabaseDownloaded(DatabaseDownloaded downloaded)
		{
			await Task.Run((Func<Task>)OnSearch);
		}

		private async ValueTask OnDatabaseSyncCompleted(DatabaseSyncCompleted args)
		{
			if (args.Updated["items"] > 0)
			{
				await Task.Run((Func<Task>)OnSearch);
			}
		}

		public async Task LoadAsync()
		{
			await Task.Run(async delegate
			{
				await NewItems(CancellationToken.None);
			});
		}

		public ChatLinkEditorViewModel CreateChatLinkEditorViewModel(Item item)
		{
			return _003CchatLinkEditorViewModelFactory_003EP.Create(item);
		}

		public void CancelPendingSearches()
		{
			_searchCancelled?.Invoke(this, EventArgs.Empty);
		}

		public async Task OnSearch()
		{
			CancelPendingSearches();
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			try
			{
				_searchCancelled = (EventHandler)Delegate.Combine(_searchCancelled, new EventHandler(SearchCancelled));
				try
				{
					await Task.Delay(300, cancellationTokenSource.Token);
					await _searchLock.WaitAsync(cancellationTokenSource.Token);
					try
					{
						await DoSearch(cancellationTokenSource.Token);
					}
					finally
					{
						_searchLock.Release();
					}
				}
				catch (OperationCanceledException)
				{
					_003Clogger_003EP.LogDebug("Previous search was canceled");
				}
				finally
				{
					_searchCancelled = (EventHandler)Delegate.Remove(_searchCancelled, new EventHandler(SearchCancelled));
				}
			}
			finally
			{
				if (cancellationTokenSource != null)
				{
					((IDisposable)cancellationTokenSource).Dispose();
				}
			}
			void SearchCancelled(object o, EventArgs a)
			{
				try
				{
					cancellationTokenSource.Cancel();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		private async Task DoSearch(CancellationToken cancellationToken)
		{
			Program.get_IsMainThread();
			string query = SearchText.Trim();
			int length = query.Length;
			if (length < 3)
			{
				if (length == 0)
				{
					await NewItems(cancellationToken);
				}
			}
			else
			{
				await Query(query, cancellationToken);
			}
		}

		private async Task Query(string text, CancellationToken cancellationToken)
		{
			Searching = true;
			try
			{
				SearchResults.Clear();
				int maxResults = _003Coptions_003EP.CurrentValue.MaxResultCount;
				ResultContext context = new ResultContext();
				await foreach (Item item in _003Csearch_003EP.Search(text, maxResults, context, cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						ItemsListViewModel viewModel = _003CitemsListViewModelFactory_003EP.Create(item, item.Id == SelectedItem?.Id);
						SearchResults.Add(viewModel);
						continue;
					}
					break;
				}
				ResultTotal = context.ResultTotal;
				ResultText = (string)((ResultTotal <= maxResults) ? _003Clocalizer_003EP["Total results", new object[1] { ResultTotal }] : _003Clocalizer_003EP["Partial results", new object[2] { maxResults, ResultTotal }]);
			}
			finally
			{
				Searching = false;
			}
		}

		private async Task NewItems(CancellationToken cancellationToken)
		{
			Searching = true;
			try
			{
				SearchResults.Clear();
				int maxResults = _003Coptions_003EP.CurrentValue.MaxResultCount;
				await foreach (Item item in _003Csearch_003EP.NewItems(maxResults).WithCancellation(cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						ItemsListViewModel viewModel = _003CitemsListViewModelFactory_003EP.Create(item, item.Id == SelectedItem?.Id);
						SearchResults.Add(viewModel);
						continue;
					}
					break;
				}
				ResultTotal = await _003Csearch_003EP.CountItems();
				ResultText = (string)((ResultTotal <= maxResults) ? _003Clocalizer_003EP["Total results", new object[1] { ResultTotal }] : _003Clocalizer_003EP["Partial results", new object[2] { maxResults, ResultTotal }]);
			}
			finally
			{
				Searching = false;
			}
		}

		public void Unload()
		{
			_003CeventAggregator_003EP.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
			_003CeventAggregator_003EP.Unsubscribe<DatabaseDownloaded>(new Func<DatabaseDownloaded, ValueTask>(OnDatabaseDownloaded));
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSyncCompleted>(new Func<DatabaseSyncCompleted, ValueTask>(OnDatabaseSyncCompleted));
		}
	}
}
