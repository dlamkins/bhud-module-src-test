using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Quarry.Interfaces;
using Quarry.Models;

namespace Quarry.Services
{
	public class WikiSubpageDataService : IWikiSubpageDataService
	{
		private const string FileName = "derived_subpages.json";

		private readonly ContentsManager contentsManager;

		private readonly Logger logger;

		public IReadOnlyDictionary<string, DerivedSubpage> ByLink { get; private set; } = new Dictionary<string, DerivedSubpage>();


		public WikiSubpageDataService(ContentsManager contentsManager, Logger logger)
		{
			this.contentsManager = contentsManager;
			this.logger = logger;
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				using (Stream stream = contentsManager.GetFileStream("derived_subpages.json"))
				{
					JsonSerializerOptions options = new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					};
					ByLink = new Dictionary<string, DerivedSubpage>((await JsonSerializer.DeserializeAsync<Dictionary<string, DerivedSubpage>>(stream, options, cancellationToken)) ?? new Dictionary<string, DerivedSubpage>(), StringComparer.OrdinalIgnoreCase);
				}
				logger.Info(string.Format("WikiSubpageDataService: loaded {0} derived subpage(s) from {1}.", ByLink.Count, "derived_subpages.json"));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to load derived_subpages.json; wiki-sourced locations and Inspector prose/images will be unavailable this session.");
			}
		}
	}
}
