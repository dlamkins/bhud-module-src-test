using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingChoiceRepository : LoggingRepository, IChoiceRepository, IRepository
	{
		private readonly IChoiceRepository _choiceRepository;

		public LoggingChoiceRepository(IChoiceRepository choiceRepository)
			: base(choiceRepository)
		{
			_choiceRepository = choiceRepository;
		}

		public NodeChoice GetChoice(string uniqueId, ChoiceType type)
		{
			return _choiceRepository.GetChoice(uniqueId, type);
		}

		public void SaveChoice(string itemSourceId, string value, ChoiceType type)
		{
			_choiceRepository.SaveChoice(itemSourceId, value, type);
		}
	}
}
