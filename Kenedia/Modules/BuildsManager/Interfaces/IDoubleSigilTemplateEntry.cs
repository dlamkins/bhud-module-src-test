using Kenedia.Modules.BuildsManager.DataModels.Items;

namespace Kenedia.Modules.BuildsManager.Interfaces
{
	public interface IDoubleSigilTemplateEntry : ISingleSigilTemplateEntry
	{
		Sigil Sigil2 { get; }
	}
}
