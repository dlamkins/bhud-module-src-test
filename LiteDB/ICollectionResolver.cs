using System.Reflection;

namespace LiteDB
{
	internal class ICollectionResolver : EnumerableResolver
	{
		public override string ResolveMethod(MethodInfo method)
		{
			if (method.Name == "Contains")
			{
				return "# ANY = @0";
			}
			return base.ResolveMethod(method);
		}
	}
}
