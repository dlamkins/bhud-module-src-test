namespace Ideka.CustomCombatText
{
	public static class AreaTypeExtensions
	{
		public static IAreaModelType? GetModelType(this AreaType type)
		{
			return type switch
			{
				AreaType.Container => new ModelTypeContainer(), 
				AreaType.Scroll => new ModelTypeScroll(), 
				AreaType.Top => new ModelTypeTop(), 
				_ => null, 
			};
		}
	}
}
