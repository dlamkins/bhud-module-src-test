using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SL.ChatLinks.Storage;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemSearch
	{
		[CompilerGenerated]
		private IDbContextFactory _003CcontextFactory_003EP;

		[CompilerGenerated]
		private ILocale _003Clocale_003EP;

		[CompilerGenerated]
		private ILogger<ItemSearch> _003Clogger_003EP;

		public ItemSearch(IDbContextFactory contextFactory, ILocale locale, ILogger<ItemSearch> logger)
		{
			_003CcontextFactory_003EP = contextFactory;
			_003Clocale_003EP = locale;
			_003Clogger_003EP = logger;
			base._002Ector();
		}

		public async ValueTask<int> CountItems()
		{
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				return await context.Items.CountAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		public async IAsyncEnumerable<Item> NewItems(int limit)
		{
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				await foreach (Item item in context.Items.OrderByDescending((Item item) => item.Id).Take(limit).AsAsyncEnumerable()
					.ConfigureAwait(continueOnCapturedContext: false))
				{
					yield return item;
				}
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		[AsyncIteratorStateMachine(typeof(_003CFilterItems_003Ed__6))]
		public IAsyncEnumerable<Item> FilterItems(ItemsFilter filter, int limit, ResultContext resultContext, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			return new _003CFilterItems_003Ed__6(-2)
			{
				_003C_003E4__this = this,
				_003C_003E3__filter = filter,
				_003C_003E3__limit = limit,
				_003C_003E3__resultContext = resultContext,
				_003C_003E3__cancellationToken = cancellationToken
			};
		}
	}
}
