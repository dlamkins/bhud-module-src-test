namespace Ideka.CustomCombatText
{
	public interface IAreaModelType
	{
		AreaType Type { get; }

		AreaViewType CreateView(AreaModel model);
	}
}
