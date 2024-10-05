using Kenedia.Modules.BuildsManager.DataModels.Items;

namespace Kenedia.Modules.BuildsManager.Interfaces
{
	public interface ITripleInfusionTemplateEntry : IDoubleInfusionTemplateEntry, ISingleInfusionTemplateEntry
	{
		Infusion Infusion3 { get; }
	}
}
