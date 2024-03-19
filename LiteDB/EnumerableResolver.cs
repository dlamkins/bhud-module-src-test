using System.Reflection;

namespace LiteDB
{
	internal class EnumerableResolver : ITypeResolver
	{
		public virtual string ResolveMethod(MethodInfo method)
		{
			string name = Reflection.MethodName(method, 1);
			if (name != null)
			{
				switch (name.Length)
				{
				case 14:
					switch (name[0])
					{
					case 'A':
						if (!(name == "AsEnumerable()"))
						{
							break;
						}
						return "@0[*]";
					case 'E':
						if (!(name == "ElementAt(int)"))
						{
							break;
						}
						return "@0[@1]";
					}
					break;
				case 8:
				{
					char c = name[0];
					if (c != 'S')
					{
						if (c != 'T' || !(name == "ToList()"))
						{
							break;
						}
						goto IL_04cd;
					}
					if (!(name == "Single()"))
					{
						break;
					}
					goto IL_046d;
				}
				case 7:
				{
					char c = name[0];
					if (c != 'C')
					{
						if (c != 'F' || !(name == "First()"))
						{
							break;
						}
						goto IL_046d;
					}
					if (!(name == "Count()"))
					{
						break;
					}
					return "COUNT(@0)";
				}
				case 23:
				{
					char c = name[1];
					if (c != 'e')
					{
						if (c != 'i' || !(name == "Single(Func<T,TResult>)"))
						{
							break;
						}
						goto IL_0479;
					}
					if (!(name == "Select(Func<T,TResult>)"))
					{
						break;
					}
					return "MAP(@0 => @1)";
				}
				case 22:
				{
					char c = name[0];
					if (c != 'C')
					{
						if (c != 'F')
						{
							if (c != 'W' || !(name == "Where(Func<T,TResult>)"))
							{
								break;
							}
							return "FILTER(@0 => @1)";
						}
						if (!(name == "First(Func<T,TResult>)"))
						{
							break;
						}
						goto IL_0479;
					}
					if (!(name == "Count(Func<T,TResult>)"))
					{
						break;
					}
					return "COUNT(FILTER(@0 => @1))";
				}
				case 5:
					switch (name[1])
					{
					case 'u':
						if (!(name == "Sum()"))
						{
							break;
						}
						return "SUM(@0)";
					case 'a':
						if (!(name == "Max()"))
						{
							break;
						}
						return "MAX(@0)";
					case 'i':
						if (!(name == "Min()"))
						{
							break;
						}
						return "MIN(@0)";
					case 'n':
						if (!(name == "Any()"))
						{
							break;
						}
						return "COUNT(@0) > 0";
					}
					break;
				case 9:
				{
					char c = name[0];
					if (c != 'A')
					{
						if (c != 'T' || !(name == "ToArray()"))
						{
							break;
						}
						goto IL_04cd;
					}
					if (!(name == "Average()"))
					{
						break;
					}
					return "AVG(@0)";
				}
				case 20:
					switch (name[1])
					{
					case 'u':
						if (!(name == "Sum(Func<T,TResult>)"))
						{
							break;
						}
						return "SUM(MAP(@0 => @1))";
					case 'a':
						if (!(name == "Max(Func<T,TResult>)"))
						{
							break;
						}
						return "MAX(MAP(@0 => @1))";
					case 'i':
						if (!(name == "Min(Func<T,TResult>)"))
						{
							break;
						}
						return "MIN(MAP(@0 => @1))";
					case 'n':
						if (!(name == "Any(Func<T,TResult>)"))
						{
							break;
						}
						return "@0 ANY %";
					case 'l':
						if (!(name == "All(Func<T,TResult>)"))
						{
							break;
						}
						return "@0 ALL %";
					}
					break;
				case 13:
					if (!(name == "get_Item(int)"))
					{
						break;
					}
					return "#[@0]";
				case 17:
					if (!(name == "SingleOrDefault()"))
					{
						break;
					}
					goto IL_046d;
				case 16:
					if (!(name == "FirstOrDefault()"))
					{
						break;
					}
					goto IL_046d;
				case 6:
					if (!(name == "Last()"))
					{
						break;
					}
					goto IL_0473;
				case 15:
					if (!(name == "LastOrDefault()"))
					{
						break;
					}
					goto IL_0473;
				case 32:
					if (!(name == "SingleOrDefault(Func<T,TResult>)"))
					{
						break;
					}
					goto IL_0479;
				case 31:
					if (!(name == "FirstOrDefault(Func<T,TResult>)"))
					{
						break;
					}
					goto IL_0479;
				case 21:
					if (!(name == "Last(Func<T,TResult>)"))
					{
						break;
					}
					goto IL_047f;
				case 30:
					if (!(name == "LastOrDefault(Func<T,TResult>)"))
					{
						break;
					}
					goto IL_047f;
				case 24:
					{
						if (!(name == "Average(Func<T,TResult>)"))
						{
							break;
						}
						return "AVG(MAP(@0 => @1))";
					}
					IL_046d:
					return "@0[0]";
					IL_047f:
					return "LAST(FILTER(@0 => @1))";
					IL_0473:
					return "@0[-1]";
					IL_04cd:
					return "ARRAY(@0)";
					IL_0479:
					return "FIRST(FILTER(@0 => @1))";
				}
			}
			if (method.Name == "Contains")
			{
				return "@0 ANY = @1";
			}
			return null;
		}

		public virtual string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (!(name == "Length"))
			{
				if (name == "Count")
				{
					return "COUNT(#)";
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
