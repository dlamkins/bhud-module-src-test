using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using GuildWars2.Items;
using Microsoft.Extensions.Logging;
using SL.ChatLinks.UI.Tabs.Items.Services;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemsTabPresenter : Presenter<IItemsTabView, ItemsTabModel>
	{
		[CompilerGenerated]
		private ILogger<ItemsTabPresenter> _003Clogger_003EP;

		[CompilerGenerated]
		private ItemSearch _003Csearch_003EP;

		private readonly CancellationTokenSource _loading;

		private readonly SemaphoreSlim _searchLock;

		private EventHandler? _searchCancelled;

		public ItemsTabPresenter(IItemsTabView view, ILogger<ItemsTabPresenter> logger, ItemSearch search)
		{
			_003Clogger_003EP = logger;
			_003Csearch_003EP = search;
			_loading = new CancellationTokenSource(TimeSpan.FromMinutes(5.0));
			_searchLock = new SemaphoreSlim(1, 1);
			base._002Ector(view, Objects.Create<ItemsTabModel>());
		}

		public void CancelPendingSearches()
		{
			_searchCancelled?.Invoke(this, EventArgs.Empty);
		}

		public async Task Search(string text)
		{
			string text2 = text;
			if (Program.get_IsMainThread())
			{
				await Task.Run(() => Search(text2));
				return;
			}
			CancelPendingSearches();
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			try
			{
				_searchCancelled = (EventHandler)Delegate.Combine(_searchCancelled, new EventHandler(SearchCancelled));
				try
				{
					await Task.Delay(1000, cancellationTokenSource.Token);
					await _searchLock.WaitAsync(cancellationTokenSource.Token);
					try
					{
						await DoSearch(text2, cancellationTokenSource.Token);
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

		public void ViewOptionSelected(Item item)
		{
			base.get_View().Select(item);
		}

		public async Task RefreshUpgrades()
		{
			base.get_Model().ClearUpgrades();
			await foreach (UpgradeComponent upgrade in _003Csearch_003EP.OfType<UpgradeComponent>())
			{
				base.get_Model().AddUpgrade(upgrade);
			}
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await foreach (UpgradeComponent upgrade in _003Csearch_003EP.OfType<UpgradeComponent>())
			{
				base.get_Model().AddUpgrade(upgrade);
				progress.Report($"Loading upgrade components ({base.get_Model().Upgrades.Count})");
			}
			await foreach (Item item in _003Csearch_003EP.NewItems(50).WithCancellation(_loading.Token))
			{
				base.get_Model().AddDefaultOption(item);
				progress.Report($"Loading newest items ({base.get_Model().DefaultOptions.Count()})");
			}
			return true;
		}

		protected override void UpdateView()
		{
			base.get_View().SetOptions(base.get_Model().DefaultOptions);
		}

		protected override void Unload()
		{
			_loading.Cancel();
			_loading.Dispose();
		}

		private async Task DoSearch(string text, CancellationToken cancellationToken)
		{
			string query = text.Trim();
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
			base.get_View().SetSearchLoading(loading: true);
			try
			{
				base.get_View().ClearOptions();
				await foreach (Item item in _003Csearch_003EP.Search(text, 100, cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						base.get_View().AddOption(item);
						continue;
					}
					break;
				}
			}
			finally
			{
				base.get_View().SetSearchLoading(loading: false);
			}
		}

		private async Task NewItems(CancellationToken cancellationToken)
		{
			base.get_View().SetSearchLoading(loading: true);
			try
			{
				base.get_Model().ClearDefaultOptions();
				base.get_View().ClearOptions();
				await foreach (Item item in _003Csearch_003EP.NewItems(50).WithCancellation(cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						base.get_Model().AddDefaultOption(item);
						base.get_View().AddOption(item);
						continue;
					}
					break;
				}
			}
			finally
			{
				base.get_View().SetSearchLoading(loading: false);
			}
		}
	}
}
