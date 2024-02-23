namespace Ideka.CustomCombatText
{
	public class ModelTypeContainer : IAreaModelType
	{
		public AreaType Type => AreaType.Container;

		public AreaViewType CreateView(AreaModel model)
		{
			return new ViewTypeContainer(model);
		}
	}
}
