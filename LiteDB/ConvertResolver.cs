using System.Reflection;

namespace LiteDB
{
	internal class ConvertResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			string name = method.Name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 7:
					switch (name[5])
					{
					case '3':
						if (!(name == "ToInt32"))
						{
							break;
						}
						return "INT32(@0)";
					case '6':
						if (!(name == "ToInt64"))
						{
							break;
						}
						return "INT64(@0)";
					}
					break;
				case 8:
					switch (name[2])
					{
					case 'D':
						if (!(name == "ToDouble"))
						{
							break;
						}
						return "DOUBLE(@0)";
					case 'S':
						if (!(name == "ToString"))
						{
							break;
						}
						return "STRING(@0)";
					}
					break;
				case 9:
					switch (name[2])
					{
					case 'D':
						if (!(name == "ToDecimal"))
						{
							break;
						}
						return "DECIMAL(@0)";
					case 'B':
						if (!(name == "ToBoolean"))
						{
							break;
						}
						return "BOOL(@0)";
					}
					break;
				case 10:
					if (!(name == "ToDateTime"))
					{
						break;
					}
					return "DATE(@0)";
				case 16:
					if (!(name == "FromBase64String"))
					{
						break;
					}
					return "BINARY(@0)";
				}
			}
			return null;
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
