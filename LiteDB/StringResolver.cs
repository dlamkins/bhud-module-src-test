using System;
using System.Reflection;

namespace LiteDB
{
	internal class StringResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			int qtParams = method.GetParameters().Length;
			string name = method.Name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 4:
					switch (name[0])
					{
					case 'T':
						if (!(name == "Trim"))
						{
							break;
						}
						return "TRIM(#)";
					case 'J':
						if (!(name == "Join"))
						{
							break;
						}
						throw new NotImplementedException();
					}
					break;
				case 9:
					switch (name[0])
					{
					case 'T':
						if (!(name == "TrimStart"))
						{
							break;
						}
						return "LTRIM(#)";
					case 'R':
						if (!(name == "RightLeft"))
						{
							break;
						}
						return "RPAD(#, @0, @1)";
					case 'S':
						if (!(name == "Substring"))
						{
							break;
						}
						if (qtParams != 1)
						{
							return "SUBSTRING(#, @0, @1)";
						}
						return "SUBSTRING(#, @0)";
					}
					break;
				case 7:
					switch (name[3])
					{
					case 'm':
						if (!(name == "TrimEnd"))
						{
							break;
						}
						return "RTRIM(#)";
					case 'p':
						if (!(name == "ToUpper"))
						{
							break;
						}
						return "UPPER(#)";
					case 'o':
						if (!(name == "ToLower"))
						{
							break;
						}
						return "LOWER(#)";
					case 'l':
						if (!(name == "Replace"))
						{
							break;
						}
						return "REPLACE(#, @0, @1)";
					case 'L':
						if (!(name == "PadLeft"))
						{
							break;
						}
						return "LPAD(#, @0, @1)";
					case 'e':
						if (!(name == "IndexOf"))
						{
							break;
						}
						if (qtParams != 1)
						{
							return "INDEXOF(#, @0, @1)";
						}
						return "INDEXOF(#, @0)";
					}
					break;
				case 8:
					switch (name[0])
					{
					case 'C':
						if (!(name == "Contains"))
						{
							break;
						}
						return "# LIKE ('%' + @0 + '%')";
					case 'E':
						if (!(name == "EndsWith"))
						{
							break;
						}
						return "# LIKE ('%' + @0)";
					case 'T':
						if (!(name == "ToString"))
						{
							break;
						}
						return "#";
					}
					break;
				case 6:
					switch (name[0])
					{
					case 'E':
						if (!(name == "Equals"))
						{
							break;
						}
						return "# = @0";
					case 'F':
						if (!(name == "Format"))
						{
							break;
						}
						throw new NotImplementedException();
					}
					break;
				case 5:
					if (!(name == "Count"))
					{
						break;
					}
					return "LENGTH(#)";
				case 10:
					if (!(name == "StartsWith"))
					{
						break;
					}
					return "# LIKE (@0 + '%')";
				case 13:
					if (!(name == "IsNullOrEmpty"))
					{
						break;
					}
					return "(LENGTH(@0) = 0)";
				case 18:
					if (!(name == "IsNullOrWhiteSpace"))
					{
						break;
					}
					return "(LENGTH(TRIM(@0)) = 0)";
				}
			}
			return null;
		}

		public string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (!(name == "Length"))
			{
				if (name == "Empty")
				{
					return "''";
				}
				return null;
			}
			return "LENGTH(#)";
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			return null;
		}
	}
}
