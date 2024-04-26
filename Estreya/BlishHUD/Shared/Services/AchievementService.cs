using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Flurl.Http;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class AchievementService : FilesystemAPIService<Achievement>
	{
		protected override string BASE_FOLDER_STRUCTURE => "achievements";

		protected override string FILE_NAME => "achievements.json";

		public List<Achievement> Achievements => base.APIObjectList;

		public AchievementService(Gw2ApiManager apiManager, APIServiceConfiguration configuration, string baseFolderPath, IFlurlClient flurlClient, string fileRootUrl)
			: base(apiManager, configuration, baseFolderPath, flurlClient, fileRootUrl)
		{
		}

		protected override async Task<List<Achievement>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			progress.Report("Loading achievement ids...");
			IApiV2ObjectList<int> ids = await ((IBulkExpandableClient<Achievement, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Achievements()).IdsAsync(cancellationToken);
			progress.Report($"Loading {((IReadOnlyCollection<int>)ids).Count} achievements...");
			IEnumerable<IEnumerable<int>> enumerable = ((IEnumerable<int>)ids).ChunkBy(200);
			int loadedCount = 0;
			List<Task<IReadOnlyList<Achievement>>> tasks = new List<Task<IReadOnlyList<Achievement>>>();
			foreach (IEnumerable<int> chunk in enumerable)
			{
				tasks.Add(((IBulkExpandableClient<Achievement, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Achievements()).ManyAsync(chunk, cancellationToken).ContinueWith(delegate(Task<IReadOnlyList<Achievement>> t)
				{
					int num = Interlocked.Add(ref loadedCount, chunk.Count());
					progress.Report($"Loading achievements... {num}/{((IReadOnlyCollection<int>)ids).Count}");
					if (t.IsFaulted)
					{
						Logger.Warn((Exception)t.Exception, $"Failed to load achievement chunk {chunk.First()} - {chunk.Last()}");
						return new List<Achievement>();
					}
					return t.Result;
				}));
			}
			IEnumerable<Achievement> source = (await Task.WhenAll(tasks)).SelectMany((IReadOnlyList<Achievement> a) => a);
			progress.Report("Finished");
			return source.ToList();
		}
	}
}