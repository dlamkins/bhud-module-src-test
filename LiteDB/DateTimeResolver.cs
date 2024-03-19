using System.Reflection;

namespace LiteDB
{
	internal class DateTimeResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			string name = method.Name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 8:
					switch (name[3])
					{
					case 'Y':
						if (!(name == "AddYears"))
						{
							break;
						}
						return "DATEADD('y', @0, #)";
					case 'H':
						if (!(name == "AddHours"))
						{
							break;
						}
						return "DATEADD('h', @0, #)";
					case 't':
						if (name == "ToString")
						{
							ParameterInfo[] pars = method.GetParameters();
							if (pars.Length == 0)
							{
								return "STRING(#)";
							}
							if (pars.Length == 1 && pars[0].ParameterType == typeof(string))
							{
								return "FORMAT(#, @0)";
							}
						}
						break;
					}
					break;
				case 10:
					switch (name[3])
					{
					case 'M':
						if (!(name == "AddMinutes"))
						{
							break;
						}
						return "DATEADD('m', @0, #)";
					case 'S':
						if (!(name == "AddSeconds"))
						{
							break;
						}
						return "DATEADD('s', @0, #)";
					}
					break;
				case 9:
					if (!(name == "AddMonths"))
					{
						break;
					}
					return "DATEADD('M', @0, #)";
				case 7:
					if (!(name == "AddDays"))
					{
						break;
					}
					return "DATEADD('d', @0, #)";
				case 15:
					if (!(name == "ToUniversalTime"))
					{
						break;
					}
					return "TO_UTC(#)";
				case 5:
					if (!(name == "Parse"))
					{
						break;
					}
					return "DATETIME(@0)";
				case 6:
					if (!(name == "Equals"))
					{
						break;
					}
					return "# = @0";
				}
			}
			return null;
		}

		public string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 3:
					switch (name[0])
					{
					case 'N':
						if (!(name == "Now"))
						{
							break;
						}
						return "NOW()";
					case 'D':
						if (!(name == "Day"))
						{
							break;
						}
						return "DAY(#)";
					}
					break;
				case 6:
					switch (name[0])
					{
					case 'U':
						if (!(name == "UtcNow"))
						{
							break;
						}
						return "NOW_UTC()";
					case 'M':
						if (!(name == "Minute"))
						{
							break;
						}
						return "MINUTE(#)";
					case 'S':
						if (!(name == "Second"))
						{
							break;
						}
						return "SECOND(#)";
					}
					break;
				case 5:
					switch (name[0])
					{
					case 'T':
						if (!(name == "Today"))
						{
							break;
						}
						return "TODAY()";
					case 'M':
						if (!(name == "Month"))
						{
							break;
						}
						return "MONTH(#)";
					}
					break;
				case 4:
					switch (name[0])
					{
					case 'Y':
						if (!(name == "Year"))
						{
							break;
						}
						return "YEAR(#)";
					case 'H':
						if (!(name == "Hour"))
						{
							break;
						}
						return "HOUR(#)";
					case 'D':
						if (!(name == "Date"))
						{
							break;
						}
						return "DATETIME(YEAR(#), MONTH(#), DAY(#))";
					}
					break;
				case 11:
					if (!(name == "ToLocalTime"))
					{
						break;
					}
					return "TO_LOCAL(#)";
				case 15:
					if (!(name == "ToUniversalTime"))
					{
						break;
					}
					return "TO_UTC(#)";
				}
			}
			return null;
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			ParameterInfo[] pars = ctor.GetParameters();
			if (pars.Length == 3 && pars[0].ParameterType == typeof(int) && pars[1].ParameterType == typeof(int) && pars[2].ParameterType == typeof(int))
			{
				return "DATETIME(@0, @1, @2)";
			}
			return null;
		}
	}
}
