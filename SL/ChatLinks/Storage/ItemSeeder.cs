using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SL.Common;

namespace SL.ChatLinks.Storage
{
	public sealed class ItemSeeder
	{
		[CompilerGenerated]
		private ILogger<ItemSeeder> _003Clogger_003EP;

		[CompilerGenerated]
		private ChatLinksContext _003Ccontext_003EP;

		[CompilerGenerated]
		private Gw2Client _003Cgw2Client_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		public ItemSeeder(ILogger<ItemSeeder> logger, ChatLinksContext context, Gw2Client gw2Client, IEventAggregator eventAggregator)
		{
			_003Clogger_003EP = logger;
			_003Ccontext_003EP = context;
			_003Cgw2Client_003EP = gw2Client;
			_003CeventAggregator_003EP = eventAggregator;
			base._002Ector();
		}

		public async Task Seed(IProgress<string> progress, CancellationToken cancellationToken)
		{
			IProgress<string> progress2 = progress;
			_003Ccontext_003EP.ChangeTracker.AutoDetectChangesEnabled = false;
			_003Clogger_003EP.LogInformation("Start seeding items.");
			HashSet<int> index = await _003Cgw2Client_003EP.Items.GetItemsIndex(cancellationToken).ValueOnly();
			_003Clogger_003EP.LogDebug("Found {Count} items in the API.", index.Count);
			index.ExceptWith(await _003Ccontext_003EP.Items.Select((Item item) => item.Id).ToListAsync(cancellationToken));
			if (index.Count != 0)
			{
				_003Clogger_003EP.LogDebug("Start seeding {Count} items.", index.Count);
				Progress<BulkProgress> bulkProgress = new Progress<BulkProgress>(delegate(BulkProgress report)
				{
					progress2.Report($"Fetching items... ({report.ResultCount} of {report.ResultTotal})");
				});
				_003Ccontext_003EP.ChangeTracker.AutoDetectChangesEnabled = false;
				int count = 0;
				await foreach (Item item2 in _003Cgw2Client_003EP.Items.GetItemsBulk(index, null, MissingMemberBehavior.Undefined, 3, 200, bulkProgress, cancellationToken).ValueOnly(cancellationToken))
				{
					_003Ccontext_003EP.Add(item2);
					int num = count + 1;
					count = num;
					if (num % 333 == 0)
					{
						await _003Ccontext_003EP.SaveChangesAsync(cancellationToken);
						DetachAllEntities(_003Ccontext_003EP);
					}
				}
				await _003Ccontext_003EP.SaveChangesAsync(cancellationToken);
			}
			_003Clogger_003EP.LogInformation("Finished seeding {Count} items.", index.Count);
			await _003CeventAggregator_003EP.PublishAsync(new DatabaseSyncCompleted(), cancellationToken);
		}

		private static void DetachAllEntities(ChatLinksContext context)
		{
			foreach (EntityEntry item in context.ChangeTracker.Entries().ToList())
			{
				item.State = EntityState.Detached;
			}
		}
	}
}
