using System.Threading.Tasks;

namespace MysticCrafting.Module.Repositories
{
	public interface IRepository
	{
		string FileName { get; }

		bool Loaded { get; }

		bool LocalOnly { get; }

		Task<string> LoadAsync();
	}
}
