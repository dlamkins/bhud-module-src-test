using Kenedia.Modules.BuildsManager.DataModels.Items;

namespace Kenedia.Modules.BuildsManager.Interfaces
{
	public interface IDoubleInfusionTemplateEntry : ISingleInfusionTemplateEntry
	{
		Infusion Infusion2 { get; }
	}
}
