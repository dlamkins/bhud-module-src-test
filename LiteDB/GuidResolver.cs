using System;
using System.Reflection;

namespace LiteDB
{
	internal class GuidResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			return method.Name switch
			{
				"ToString" => "STRING(#)", 
				"NewGuid" => "GUID()", 
				"Parse" => "GUID(@0)", 
				"TryParse" => throw new NotSupportedException("There is no TryParse translate. Use Guid.Parse()"), 
				"Equals" => "# = @0", 
				_ => null, 
			};
		}

		public string ResolveMember(MemberInfo member)
		{
			if (member.Name == "Empty")
			{
				return "GUID('00000000-0000-0000-0000-000000000000')";
			}
			return null;
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			ParameterInfo[] pars = ctor.GetParameters();
			if (pars.Length == 1 && pars[0].ParameterType == typeof(string))
			{
				return "GUID(@0)";
			}
			return null;
		}
	}
}
