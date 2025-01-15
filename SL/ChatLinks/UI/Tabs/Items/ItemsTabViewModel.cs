using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GuildWars2.Items;
using Microsoft.Extensions.Logging;
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
		private ItemSearch _003Csearch_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private ItemsListViewModelFactory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private ChatLinkEditorViewModelFactory _003CchatLinkEditorViewModelFactory_003EP;

		private string _searchText;

		private bool _searching;

		private EventHandler? _searchCancelled;

		private readonly SemaphoreSlim _searchLock;

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

		public ObservableCollection<ItemsListViewModel> SearchResults { get; }

		public ICommand SearchCommand => new AsyncRelayCommand(new Func<Task>(OnSearch));

		public ItemsTabViewModel(ILogger<ItemsTabViewModel> logger, ItemSearch search, Customizer customizer, ItemsListViewModelFactory itemsListViewModelFactory, ChatLinkEditorViewModelFactory chatLinkEditorViewModelFactory)
		{
			_003Clogger_003EP = logger;
			_003Csearch_003EP = search;
			_003Ccustomizer_003EP = customizer;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CchatLinkEditorViewModelFactory_003EP = chatLinkEditorViewModelFactory;
			_searchText = "";
			_searchLock = new SemaphoreSlim(1, 1);
			SearchResults = new ObservableCollection<ItemsListViewModel>();
			base._002Ector();
		}

		public async Task LoadAsync()
		{
			await _003Ccustomizer_003EP.LoadAsync();
			await NewItems(CancellationToken.None);
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
				await foreach (Item item in _003Csearch_003EP.Search(text, 100, cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						ItemsListViewModel viewModel = _003CitemsListViewModelFactory_003EP.Create(item, isSelected: false);
						SearchResults.Add(viewModel);
						continue;
					}
					break;
				}
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
				await foreach (Item item in _003Csearch_003EP.NewItems(50).WithCancellation(cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						ItemsListViewModel viewModel = _003CitemsListViewModelFactory_003EP.Create(item, isSelected: false);
						SearchResults.Add(viewModel);
						continue;
					}
					break;
				}
			}
			finally
			{
				Searching = false;
			}
		}
	}
}
