using System.Reflection;

namespace LiteDB
{
	internal class ObjectIdResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			string name = method.Name;
			if (!(name == "ToString"))
			{
				if (name == "Equals")
				{
					return "# = @0";
				}
				return null;
			}
			return "STRING(#)";
		}

		public string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (!(name == "Empty"))
			{
				if (name == "CreationTime")
				{
					return "OID_CREATIONTIME(#)";
				}
				return null;
			}
			return "OBJECTID('000000000000000000000000')";
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			ParameterInfo[] pars = ctor.GetParameters();
			if (pars.Length == 1 && pars[0].ParameterType == typeof(string))
			{
				return "OBJECTID(@0)";
			}
			return null;
		}
	}
}
