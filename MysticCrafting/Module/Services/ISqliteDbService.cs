using System.Threading.Tasks;

namespace MysticCrafting.Module.Services
{
	public interface ISqliteDbService
	{
		string DatabaseFilePath { get; }

		Task InitializeDatabaseFile();
	}
}
