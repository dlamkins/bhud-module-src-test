using System.Reflection;

namespace LiteDB
{
	internal class BsonValueResolver : ITypeResolver
	{
		public string ResolveMethod(MethodInfo method)
		{
			return null;
		}

		public string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 7:
				{
					char c = name[5];
					if (c != '3')
					{
						if (c != '6')
						{
							if (c != 'a')
							{
								break;
							}
							if (!(name == "AsArray"))
							{
								if (!(name == "IsArray"))
								{
									break;
								}
								return "IS_ARRAY(#)";
							}
						}
						else if (!(name == "AsInt64"))
						{
							if (!(name == "IsInt64"))
							{
								break;
							}
							return "IS_INT64(#)";
						}
					}
					else if (!(name == "AsInt32"))
					{
						if (!(name == "IsInt32"))
						{
							break;
						}
						return "IS_INT32(#)";
					}
					goto IL_0302;
				}
				case 10:
				{
					char c = name[4];
					if ((uint)c <= 106u)
					{
						if (c != 'c')
						{
							if (c != 'j')
							{
								break;
							}
							if (!(name == "AsObjectId"))
							{
								if (!(name == "IsObjectId"))
								{
									break;
								}
								return "IS_OBJECTID(#)";
							}
						}
						else if (!(name == "AsDocument"))
						{
							if (!(name == "IsDocument"))
							{
								break;
							}
							return "IS_DOCUMENT(#)";
						}
					}
					else
					{
						if (c == 'n')
						{
							if (!(name == "IsMinValue"))
							{
								break;
							}
							return "IS_MINVALUE(#)";
						}
						if (c != 't')
						{
							if (c != 'x' || !(name == "IsMaxValue"))
							{
								break;
							}
							return "IS_MAXVALUE(#)";
						}
						if (!(name == "AsDateTime"))
						{
							if (!(name == "IsDateTime"))
							{
								break;
							}
							return "IS_DATETIME(#)";
						}
					}
					goto IL_0302;
				}
				case 8:
				{
					char c = name[2];
					if ((uint)c <= 68u)
					{
						if (c != 'B')
						{
							if (c != 'D')
							{
								break;
							}
							if (!(name == "AsDouble"))
							{
								if (!(name == "IsDouble"))
								{
									break;
								}
								return "IS_DOUBLE(#)";
							}
						}
						else if (!(name == "AsBinary"))
						{
							if (!(name == "IsBinary"))
							{
								break;
							}
							return "IS_BINARY(#)";
						}
					}
					else
					{
						if (c == 'N')
						{
							if (!(name == "IsNumber"))
							{
								break;
							}
							return "IS_NUMBER(#)";
						}
						if (c != 'S')
						{
							break;
						}
						if (!(name == "AsString"))
						{
							if (!(name == "IsString"))
							{
								break;
							}
							return "IS_STRING(#)";
						}
					}
					goto IL_0302;
				}
				case 9:
				{
					char c = name[0];
					if (c != 'A')
					{
						if (c == 'I')
						{
							if (name == "IsDecimal")
							{
								return "IS_DECIMAL(#)";
							}
							if (name == "IsBoolean")
							{
								return "IS_BOOLEAN(#)";
							}
						}
						break;
					}
					if (!(name == "AsBoolean") && !(name == "AsDecimal"))
					{
						break;
					}
					goto IL_0302;
				}
				case 6:
					{
						char c = name[0];
						if (c != 'A')
						{
							if (c == 'I')
							{
								if (name == "IsNull")
								{
									return "IS_NULL(#)";
								}
								if (name == "IsGuid")
								{
									return "IS_GUID(#)";
								}
							}
							break;
						}
						if (!(name == "AsGuid"))
						{
							break;
						}
						goto IL_0302;
					}
					IL_0302:
					return "#";
				}
			}
			return null;
		}

		public string ResolveCtor(ConstructorInfo ctor)
		{
			return null;
		}
	}
}
