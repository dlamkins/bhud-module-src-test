using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Services
{
	public interface IChoiceRepository : IRepository
	{
		void SaveChoice(string itemSourceId, string value, ChoiceType type);

		NodeChoice GetChoice(string uniqueId, ChoiceType type);
	}
}
