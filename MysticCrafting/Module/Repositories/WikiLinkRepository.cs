using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class WikiLinkRepository : IWikiLinkRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<MysticWikiLink> WikiLinks { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "wiki_links_data.json";

		public WikiLinkRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			WikiLinks = (await _dataService.LoadFromFileAsync<List<MysticWikiLink>>(FileName)) ?? new List<MysticWikiLink>();
			Loaded = true;
			return $"{WikiLinks.Count} wiki links loaded";
		}

		public MysticWikiLink GetLink(int itemId)
		{
			return WikiLinks.FirstOrDefault((MysticWikiLink v) => v.GameId == itemId);
		}
	}
}
