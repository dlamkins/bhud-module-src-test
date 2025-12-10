using System.Threading.Tasks;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public interface IDataDictionary
	{
		Version Version { get; set; }

		string FilePath { get; }

		string FileName { get; }

		bool IsOutdated(Version version);

		Task Update(Version version);

		Task<bool> Load();

		Task Save();
	}
}
