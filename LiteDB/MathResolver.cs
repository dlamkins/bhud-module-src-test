using System;
using System.Reflection;

namespace LiteDB
{
	internal class MathResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			int qtParams = method.GetParameters().Length;
			switch (method.Name)
			{
			case "Abs":
				return "ABS(@0)";
			case "Pow":
				return "POW(@0, @1)";
			case "Round":
				if (qtParams != 2)
				{
					throw new ArgumentOutOfRangeException("Method Round need 2 arguments when convert to BsonExpression");
				}
				return "ROUND(@0, @1)";
			default:
				return null;
			}
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
