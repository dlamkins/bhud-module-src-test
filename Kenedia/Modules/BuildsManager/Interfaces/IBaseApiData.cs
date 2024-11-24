using Kenedia.Modules.BuildsManager.DataModels.Items;

namespace Kenedia.Modules.BuildsManager.Interfaces
{
	public interface IBaseApiData : IDataMember
	{
		string Name { get; set; }

		string Description { get; set; }

		int Id { get; set; }
	}
}
