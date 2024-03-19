using System.Reflection;

namespace LiteDB
{
	internal class NullableResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			return null;
		}

		public string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (!(name == "HasValue"))
			{
				if (name == "Value")
				{
					return "#";
				}
				return null;
			}
			return "(IS_NULL(#) = false)";
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			return null;
		}
	}
}
