using System.Linq;
using JsonFlatFileDataStore;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class WikiLinkRepository : IWikiLinkRepository
	{
		private IDocumentCollection<MysticWikiLink> WikiLinks { get; } = ServiceContainer.Store.GetCollection<MysticWikiLink>();


		public MysticWikiLink GetLink(int itemId)
		{
			return WikiLinks.AsQueryable().FirstOrDefault((MysticWikiLink v) => v.GameId == itemId);
		}
	}
}
