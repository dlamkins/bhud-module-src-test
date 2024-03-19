using System.Reflection;

namespace LiteDB
{
	internal class RegexResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			string name = method.Name;
			if (!(name == "Split"))
			{
				if (name == "IsMatch")
				{
					return "IS_MATCH(@0, @1)";
				}
				return null;
			}
			return "SPLIT(@0, @1, true)";
		}

		public string ResolveMember(MemberInfo member)
		{
			return null;
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			return null;
		}
	}
}
