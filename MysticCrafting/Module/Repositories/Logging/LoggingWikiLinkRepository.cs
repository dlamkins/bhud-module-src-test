using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingWikiLinkRepository : LoggingRepository, IWikiLinkRepository, IRepository
	{
		private readonly IWikiLinkRepository _wikiLinkRepository;

		public LoggingWikiLinkRepository(IWikiLinkRepository wikiLinkRepository)
			: base(wikiLinkRepository)
		{
			_wikiLinkRepository = wikiLinkRepository;
		}

		public MysticWikiLink GetLink(int itemId)
		{
			return _wikiLinkRepository.GetLink(itemId);
		}
	}
}
