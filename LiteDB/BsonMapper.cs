using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LiteDB
{
	public class BsonMapper
	{
		private readonly Dictionary<Type, EntityMapper> _entities = new Dictionary<Type, EntityMapper>();

		private readonly ConcurrentDictionary<Type, Func<object, BsonValue>> _customSerializer = new ConcurrentDictionary<Type, Func<object, BsonValue>>();

		private readonly ConcurrentDictionary<Type, Func<BsonValue, object>> _customDeserializer = new ConcurrentDictionary<Type, Func<BsonValue, object>>();

		private readonly Func<Type, object> _typeInstantiator;

		private readonly ITypeNameBinder _typeNameBinder;

		public static BsonMapper Global = new BsonMapper();

		public Func<string, string> ResolveFieldName;

		public Action<Type, MemberInfo, MemberMapper> ResolveMember;

		public Func<Type, string> ResolveCollectionName;

		private readonly Regex _lowerCaseDelimiter = new Regex("(?!(^[A-Z]))([A-Z])", RegexOptions.Compiled);

		private readonly HashSet<Type> _bsonTypes = new HashSet<Type>
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(bool),
			typeof(Guid),
			typeof(DateTime),
			typeof(byte[]),
			typeof(ObjectId),
			typeof(double),
			typeof(decimal)
		};

		private readonly HashSet<Type> _basicTypes = new HashSet<Type>
		{
			typeof(short),
			typeof(ushort),
			typeof(uint),
			typeof(float),
			typeof(char),
			typeof(byte),
			typeof(sbyte)
		};

		public bool SerializeNullValues { get; set; }

		public bool TrimWhitespace { get; set; }

		public bool EmptyStringToNull { get; set; }

		public bool EnumAsInteger { get; set; }

		public bool IncludeFields { get; set; }

		public bool IncludeNonPublic { get; set; }

		public int MaxDepth { get; set; }

		public BsonMapper(Func<Type, object> customTypeInstantiator = null, ITypeNameBinder typeNameBinder = null)
		{
			SerializeNullValues = false;
			TrimWhitespace = true;
			EmptyStringToNull = true;
			EnumAsInteger = false;
			ResolveFieldName = (string s) => s;
			ResolveMember = delegate
			{
			};
			ResolveCollectionName = (Type t) => (!Reflection.IsEnumerable(t)) ? t.Name : Reflection.GetListItemType(t).Name;
			IncludeFields = false;
			MaxDepth = 20;
			_typeInstantiator = customTypeInstantiator ?? ((Func<Type, object>)((Type t) => null));
			_typeNameBinder = typeNameBinder ?? DefaultTypeNameBinder.Instance;
			RegisterType((Uri uri) => uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString(), (BsonValue bson) => new Uri(bson.AsString));
			RegisterType((DateTimeOffset value) => new BsonValue(value.UtcDateTime), (BsonValue bson) => bson.AsDateTime.ToUniversalTime());
			RegisterType((TimeSpan value) => new BsonValue(value.Ticks), (BsonValue bson) => new TimeSpan(bson.AsInt64));
			RegisterType((Regex r) => (r.Options != 0) ? new BsonDocument
			{
				{
					"p",
					r.ToString()
				},
				{
					"o",
					(int)r.Options
				}
			} : new BsonValue(r.ToString()), (BsonValue value) => (!value.IsString) ? new Regex(value.AsDocument["p"].AsString, (RegexOptions)value.AsDocument["o"].AsInt32) : new Regex(value));
		}

		public void RegisterType<T>(Func<T, BsonValue> serialize, Func<BsonValue, T> deserialize)
		{
			_customSerializer[typeof(T)] = (object o) => serialize((T)o);
			_customDeserializer[typeof(T)] = (BsonValue b) => deserialize(b);
		}

		public void RegisterType(Type type, Func<object, BsonValue> serialize, Func<BsonValue, object> deserialize)
		{
			_customSerializer[type] = (object o) => serialize(o);
			_customDeserializer[type] = (BsonValue b) => deserialize(b);
		}

		public EntityBuilder<T> Entity<T>()
		{
			return new EntityBuilder<T>(this, _typeNameBinder);
		}

		public BsonExpression GetExpression<T, K>(Expression<Func<T, K>> predicate)
		{
			return new LinqExpressionVisitor(this, predicate).Resolve(typeof(K) == typeof(bool));
		}

		public BsonExpression GetIndexExpression<T, K>(Expression<Func<T, K>> predicate)
		{
			return new LinqExpressionVisitor(this, predicate).Resolve(predicate: false);
		}

		public BsonMapper UseCamelCase()
		{
			ResolveFieldName = (string s) => char.ToLower(s[0]) + s.Substring(1);
			return this;
		}

		public BsonMapper UseLowerCaseDelimiter(char delimiter = '_')
		{
			ResolveFieldName = (string s) => _lowerCaseDelimiter.Replace(s, delimiter + "$2").ToLower();
			return this;
		}

		internal EntityMapper GetEntityMapper(Type type)
		{
			if (!_entities.TryGetValue(type, out var mapper))
			{
				lock (_entities)
				{
					if (!_entities.TryGetValue(type, out mapper))
					{
						return _entities[type] = BuildEntityMapper(type);
					}
					return mapper;
				}
			}
			return mapper;
		}

		protected virtual EntityMapper BuildEntityMapper(Type type)
		{
			EntityMapper mapper = new EntityMapper(type);
			Type idAttr = typeof(BsonIdAttribute);
			Type ignoreAttr = typeof(BsonIgnoreAttribute);
			Type fieldAttr = typeof(BsonFieldAttribute);
			Type dbrefAttr = typeof(BsonRefAttribute);
			IEnumerable<MemberInfo> members = GetTypeMembers(type);
			MemberInfo id = GetIdMember(members);
			foreach (MemberInfo memberInfo in members)
			{
				if (!CustomAttributeExtensions.IsDefined(memberInfo, ignoreAttr, inherit: true))
				{
					string name = ResolveFieldName(memberInfo.Name);
					BsonFieldAttribute field = (BsonFieldAttribute)CustomAttributeExtensions.GetCustomAttributes(memberInfo, fieldAttr, inherit: true).FirstOrDefault();
					if (field != null && field.Name != null)
					{
						name = field.Name;
					}
					if (memberInfo == id)
					{
						name = "_id";
					}
					GenericGetter getter = Reflection.CreateGenericGetter(type, memberInfo);
					GenericSetter setter = Reflection.CreateGenericSetter(type, memberInfo);
					BsonIdAttribute autoId = (BsonIdAttribute)CustomAttributeExtensions.GetCustomAttributes(memberInfo, idAttr, inherit: true).FirstOrDefault();
					Type dataType = ((memberInfo is PropertyInfo) ? (memberInfo as PropertyInfo).PropertyType : (memberInfo as FieldInfo).FieldType);
					bool isEnumerable = Reflection.IsEnumerable(dataType);
					MemberMapper member = new MemberMapper
					{
						AutoId = (autoId?.AutoId ?? true),
						FieldName = name,
						MemberName = memberInfo.Name,
						DataType = dataType,
						IsEnumerable = isEnumerable,
						UnderlyingType = (isEnumerable ? Reflection.GetListItemType(dataType) : dataType),
						Getter = getter,
						Setter = setter
					};
					BsonRefAttribute dbRef = (BsonRefAttribute)CustomAttributeExtensions.GetCustomAttributes(memberInfo, dbrefAttr, inherit: false).FirstOrDefault();
					if (dbRef != null && memberInfo is PropertyInfo)
					{
						RegisterDbRef(this, member, _typeNameBinder, dbRef.Collection ?? ResolveCollectionName((memberInfo as PropertyInfo).PropertyType));
					}
					ResolveMember?.Invoke(type, memberInfo, member);
					if (member.FieldName != null && !mapper.Members.Any((MemberMapper x) => x.FieldName.Equals(name, StringComparison.OrdinalIgnoreCase)))
					{
						mapper.Members.Add(member);
					}
				}
			}
			return mapper;
		}

		protected virtual MemberInfo GetIdMember(IEnumerable<MemberInfo> members)
		{
			return Reflection.SelectMember(members, (MemberInfo x) => CustomAttributeExtensions.IsDefined(x, typeof(BsonIdAttribute), inherit: true), (MemberInfo x) => x.Name.Equals("Id", StringComparison.OrdinalIgnoreCase), (MemberInfo x) => x.Name.Equals(x.DeclaringType.Name + "Id", StringComparison.OrdinalIgnoreCase));
		}

		protected virtual IEnumerable<MemberInfo> GetTypeMembers(Type type)
		{
			List<MemberInfo> members = new List<MemberInfo>();
			BindingFlags flags = (IncludeNonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public));
			members.AddRange((from x in type.GetProperties(flags)
				where x.CanRead && x.GetIndexParameters().Length == 0
				select x).Select((Func<PropertyInfo, MemberInfo>)((PropertyInfo x) => x)));
			if (IncludeFields)
			{
				members.AddRange((from x in type.GetFields(flags)
					where !x.Name.EndsWith("k__BackingField") && !x.IsStatic
					select x).Select((Func<FieldInfo, MemberInfo>)((FieldInfo x) => x)));
			}
			return members;
		}

		protected virtual CreateObject GetTypeCtor(EntityMapper mapper)
		{
			ConstructorInfo[] ctors = mapper.ForType.GetConstructors();
			ConstructorInfo ctor = ctors.FirstOrDefault((ConstructorInfo x) => x.GetCustomAttribute<BsonCtorAttribute>() != null && x.GetParameters().All((ParameterInfo p) => Reflection.ConvertType.ContainsKey(p.ParameterType) || _basicTypes.Contains(p.ParameterType) || p.ParameterType.GetTypeInfo().IsEnum)) ?? ctors.FirstOrDefault((ConstructorInfo x) => x.GetParameters().Length == 0) ?? ctors.FirstOrDefault((ConstructorInfo x) => x.GetParameters().All((ParameterInfo p) => Reflection.ConvertType.ContainsKey(p.ParameterType) || _customDeserializer.ContainsKey(p.ParameterType) || _basicTypes.Contains(p.ParameterType) || p.ParameterType.GetTypeInfo().IsEnum));
			if (ctor == null)
			{
				return null;
			}
			List<Expression> pars = new List<Expression>();
			ParameterExpression pDoc = Expression.Parameter(typeof(BsonDocument), "_doc");
			ParameterInfo[] parameters = ctor.GetParameters();
			foreach (ParameterInfo p2 in parameters)
			{
				string name = mapper.Members.FirstOrDefault((MemberMapper x) => x.MemberName.Equals(p2.Name, StringComparison.OrdinalIgnoreCase))?.FieldName ?? p2.Name;
				IndexExpression expr = Expression.MakeIndex(pDoc, Reflection.DocumentItemProperty, new ConstantExpression[1] { Expression.Constant(name) });
				if (_customDeserializer.TryGetValue(p2.ParameterType, out var func))
				{
					UnaryExpression cast4 = Expression.Convert(Expression.Invoke(Expression.Constant(func), expr), p2.ParameterType);
					pars.Add(cast4);
				}
				else if (_basicTypes.Contains(p2.ParameterType))
				{
					ConstantExpression typeExpr3 = Expression.Constant(p2.ParameterType);
					MemberExpression rawValue3 = Expression.Property(expr, typeof(BsonValue).GetProperty("RawValue"));
					UnaryExpression cast3 = Expression.Convert(Expression.Call(typeof(Convert).GetMethod("ChangeType", new Type[2]
					{
						typeof(object),
						typeof(Type)
					}), rawValue3, typeExpr3), p2.ParameterType);
					pars.Add(cast3);
				}
				else if (p2.ParameterType.GetTypeInfo().IsEnum && EnumAsInteger)
				{
					ConstantExpression typeExpr2 = Expression.Constant(p2.ParameterType);
					MemberExpression rawValue2 = Expression.PropertyOrField(expr, "AsInt32");
					UnaryExpression cast2 = Expression.Convert(Expression.Call(typeof(Enum).GetMethod("ToObject", new Type[2]
					{
						typeof(Type),
						typeof(int)
					}), typeExpr2, rawValue2), p2.ParameterType);
					pars.Add(cast2);
				}
				else if (p2.ParameterType.GetTypeInfo().IsEnum)
				{
					ConstantExpression typeExpr = Expression.Constant(p2.ParameterType);
					MemberExpression rawValue = Expression.PropertyOrField(expr, "AsString");
					UnaryExpression cast = Expression.Convert(Expression.Call(typeof(Enum).GetMethod("Parse", new Type[2]
					{
						typeof(Type),
						typeof(string)
					}), typeExpr, rawValue), p2.ParameterType);
					pars.Add(cast);
				}
				else
				{
					PropertyInfo propInfo = Reflection.ConvertType[p2.ParameterType];
					MemberExpression prop = Expression.Property(expr, propInfo);
					pars.Add(prop);
				}
			}
			NewExpression newExpr = Expression.New(ctor, pars.ToArray());
			if (!mapper.ForType.GetTypeInfo().IsClass)
			{
				return Expression.Lambda<CreateObject>(Expression.Convert(newExpr, typeof(object)), new ParameterExpression[1] { pDoc }).Compile();
			}
			return Expression.Lambda<CreateObject>(newExpr, new ParameterExpression[1] { pDoc }).Compile();
		}

		internal static void RegisterDbRef(BsonMapper mapper, MemberMapper member, ITypeNameBinder typeNameBinder, string collection)
		{
			member.IsDbRef = true;
			if (member.IsEnumerable)
			{
				RegisterDbRefList(mapper, member, typeNameBinder, collection);
			}
			else
			{
				RegisterDbRefItem(mapper, member, typeNameBinder, collection);
			}
		}

		private static void RegisterDbRefItem(BsonMapper mapper, MemberMapper member, ITypeNameBinder typeNameBinder, string collection)
		{
			EntityMapper entity = mapper.GetEntityMapper(member.DataType);
			member.Serialize = delegate(object obj, BsonMapper m)
			{
				if (obj == null)
				{
					return BsonValue.Null;
				}
				object obj2 = (entity.Id ?? throw new LiteException(0, "There is no _id field mapped in your type: " + member.DataType.FullName)).Getter(obj);
				BsonDocument bsonDocument = new BsonDocument
				{
					["$id"] = m.Serialize(obj2.GetType(), obj2, 0),
					["$ref"] = collection
				};
				if (member.DataType != obj.GetType())
				{
					bsonDocument["$type"] = typeNameBinder.GetName(obj.GetType());
				}
				return bsonDocument;
			};
			member.Deserialize = delegate(BsonValue bson, BsonMapper m)
			{
				if (bson == null || !bson.IsDocument)
				{
					return null;
				}
				BsonDocument asDocument = bson.AsDocument;
				BsonValue value = asDocument["$id"];
				bool num = asDocument["$missing"] == true;
				bool flag = !asDocument.ContainsKey("$ref");
				if (num)
				{
					return null;
				}
				if (flag)
				{
					asDocument["_id"] = value;
					if (asDocument.ContainsKey("$type"))
					{
						asDocument["_type"] = bson["$type"];
					}
					return m.Deserialize(entity.ForType, asDocument);
				}
				return m.Deserialize(entity.ForType, asDocument.ContainsKey("$type") ? new BsonDocument
				{
					["_id"] = value,
					["_type"] = bson["$type"]
				} : new BsonDocument { ["_id"] = value });
			};
		}

		private static void RegisterDbRefList(BsonMapper mapper, MemberMapper member, ITypeNameBinder typeNameBinder, string collection)
		{
			EntityMapper entity = mapper.GetEntityMapper(member.UnderlyingType);
			member.Serialize = delegate(object list, BsonMapper m)
			{
				if (list == null)
				{
					return BsonValue.Null;
				}
				BsonArray bsonArray2 = new BsonArray();
				MemberMapper id = entity.Id;
				foreach (object current2 in (IEnumerable)list)
				{
					if (current2 != null)
					{
						object obj = id.Getter(current2);
						BsonDocument bsonDocument2 = new BsonDocument
						{
							["$id"] = m.Serialize(obj.GetType(), obj, 0),
							["$ref"] = collection
						};
						if (member.UnderlyingType != current2.GetType())
						{
							bsonDocument2["$type"] = typeNameBinder.GetName(current2.GetType());
						}
						bsonArray2.Add(bsonDocument2);
					}
				}
				return bsonArray2;
			};
			member.Deserialize = delegate(BsonValue bson, BsonMapper m)
			{
				if (!bson.IsArray)
				{
					return null;
				}
				BsonArray asArray = bson.AsArray;
				if (asArray.Count == 0)
				{
					return m.Deserialize(member.DataType, asArray);
				}
				BsonArray bsonArray = new BsonArray();
				foreach (BsonValue current in asArray)
				{
					if (current.IsDocument)
					{
						BsonDocument asDocument = current.AsDocument;
						BsonValue value = asDocument["$id"];
						bool flag = asDocument["$missing"] == true;
						bool flag2 = !asDocument.ContainsKey("$ref");
						if (!flag)
						{
							if (flag2)
							{
								current["_id"] = value;
								if (current.AsDocument.ContainsKey("$type"))
								{
									current["_type"] = current["$type"];
								}
								bsonArray.Add(current);
							}
							else
							{
								BsonDocument bsonDocument = new BsonDocument { ["_id"] = value };
								if (current.AsDocument.ContainsKey("$type"))
								{
									bsonDocument["_type"] = current["$type"];
								}
								bsonArray.Add(bsonDocument);
							}
						}
					}
				}
				return m.Deserialize(member.DataType, bsonArray);
			};
		}

		public virtual object ToObject(Type type, BsonDocument doc)
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			if (type == typeof(BsonDocument))
			{
				return doc;
			}
			return Deserialize(type, doc);
		}

		public virtual T ToObject<T>(BsonDocument doc)
		{
			return (T)ToObject(typeof(T), doc);
		}

		public T Deserialize<T>(BsonValue value)
		{
			if (value == null)
			{
				return default(T);
			}
			return (T)Deserialize(typeof(T), value);
		}

		public object Deserialize(Type type, BsonValue value)
		{
			if (value.IsNull)
			{
				return null;
			}
			if (Reflection.IsNullable(type))
			{
				type = Reflection.UnderlyingTypeOf(type);
			}
			if (_customDeserializer.TryGetValue(type, out var custom))
			{
				return custom(value);
			}
			TypeInfo typeInfo = type.GetTypeInfo();
			if (type == typeof(BsonValue))
			{
				return value;
			}
			if (type == typeof(BsonDocument))
			{
				return value.AsDocument;
			}
			if (type == typeof(BsonArray))
			{
				return value.AsArray;
			}
			if (_bsonTypes.Contains(type))
			{
				return value.RawValue;
			}
			if (_basicTypes.Contains(type))
			{
				return Convert.ChangeType(value.RawValue, type);
			}
			if (type == typeof(ulong))
			{
				return (ulong)value.AsInt64;
			}
			if (typeInfo.IsEnum)
			{
				if (value.IsString)
				{
					return Enum.Parse(type, value.AsString);
				}
				if (value.IsNumber)
				{
					return Enum.ToObject(type, value.AsInt32);
				}
			}
			else
			{
				if (value.IsArray)
				{
					if (type == typeof(object))
					{
						return DeserializeArray(typeof(object), value.AsArray);
					}
					if (type.IsArray)
					{
						return DeserializeArray(type.GetElementType(), value.AsArray);
					}
					return DeserializeList(type, value.AsArray);
				}
				if (value.IsDocument)
				{
					if (type.IsAnonymousType())
					{
						return DeserializeAnonymousType(type, value.AsDocument);
					}
					BsonDocument doc = value.AsDocument;
					if (doc.TryGetValue("_type", out var typeField) && typeField.IsString)
					{
						Type actualType = _typeNameBinder.GetType(typeField.AsString);
						if (actualType == null)
						{
							throw LiteException.InvalidTypedName(typeField.AsString);
						}
						if (!type.IsAssignableFrom(actualType))
						{
							throw LiteException.DataTypeNotAssignable(type.FullName, actualType.FullName);
						}
						if (actualType.FullName.Equals("System.Diagnostics.Process", StringComparison.OrdinalIgnoreCase))
						{
							throw LiteException.AvoidUseOfProcess();
						}
						type = actualType;
					}
					else if (type == typeof(object))
					{
						type = typeof(Dictionary<string, object>);
					}
					EntityMapper entity = GetEntityMapper(type);
					if (entity.CreateInstance == null)
					{
						entity.CreateInstance = GetTypeCtor(entity) ?? ((CreateObject)((BsonDocument v) => Reflection.CreateInstance(entity.ForType)));
					}
					object o = _typeInstantiator(type) ?? entity.CreateInstance(doc);
					IDictionary dict = o as IDictionary;
					if (dict != null)
					{
						if (o.GetType().GetTypeInfo().IsGenericType)
						{
							Type i = type.GetGenericArguments()[0];
							Type t = type.GetGenericArguments()[1];
							DeserializeDictionary(i, t, dict, value.AsDocument);
						}
						else
						{
							DeserializeDictionary(typeof(object), typeof(object), dict, value.AsDocument);
						}
					}
					else
					{
						DeserializeObject(entity, o, doc);
					}
					return o;
				}
			}
			return value.RawValue;
		}

		private object DeserializeArray(Type type, BsonArray array)
		{
			Array arr = Array.CreateInstance(type, array.Count);
			int idx = 0;
			foreach (BsonValue item in array)
			{
				arr.SetValue(Deserialize(type, item), idx++);
			}
			return arr;
		}

		private object DeserializeList(Type type, BsonArray value)
		{
			Type itemType = Reflection.GetListItemType(type);
			IEnumerable enumerable = (IEnumerable)Reflection.CreateInstance(type);
			IList list = enumerable as IList;
			if (list != null)
			{
				foreach (BsonValue item2 in value)
				{
					list.Add(Deserialize(itemType, item2));
				}
				return enumerable;
			}
			MethodInfo addMethod = type.GetMethod("Add", new Type[1] { itemType });
			foreach (BsonValue item in value)
			{
				addMethod.Invoke(enumerable, new object[1] { Deserialize(itemType, item) });
			}
			return enumerable;
		}

		private void DeserializeDictionary(Type K, Type T, IDictionary dict, BsonDocument value)
		{
			bool isKEnum = K.GetTypeInfo().IsEnum;
			foreach (KeyValuePair<string, BsonValue> el in value.GetElements())
			{
				object i = (isKEnum ? Enum.Parse(K, el.Key) : ((K == typeof(Uri)) ? new Uri(el.Key) : Convert.ChangeType(el.Key, K)));
				object v = Deserialize(T, el.Value);
				dict.Add(i, v);
			}
		}

		private void DeserializeObject(EntityMapper entity, object obj, BsonDocument value)
		{
			foreach (MemberMapper member in entity.Members.Where((MemberMapper x) => x.Setter != null))
			{
				if (value.TryGetValue(member.FieldName, out var val))
				{
					if (member.Deserialize != null)
					{
						member.Setter(obj, member.Deserialize(val, this));
					}
					else
					{
						member.Setter(obj, Deserialize(member.DataType, val));
					}
				}
			}
		}

		private object DeserializeAnonymousType(Type type, BsonDocument value)
		{
			List<object> args = new List<object>();
			ParameterInfo[] parameters = type.GetConstructors()[0].GetParameters();
			foreach (ParameterInfo par in parameters)
			{
				object arg = Deserialize(par.ParameterType, value[par.Name]);
				args.Add(arg);
			}
			return Activator.CreateInstance(type, args.ToArray());
		}

		public virtual BsonDocument ToDocument(Type type, object entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (entity is BsonDocument)
			{
				return (BsonDocument)entity;
			}
			return Serialize(type, entity, 0).AsDocument;
		}

		public virtual BsonDocument ToDocument<T>(T entity)
		{
			return ToDocument(typeof(T), entity)?.AsDocument;
		}

		public BsonValue Serialize<T>(T obj)
		{
			return Serialize(typeof(T), obj, 0);
		}

		public BsonValue Serialize(Type type, object obj)
		{
			return Serialize(type, obj, 0);
		}

		internal BsonValue Serialize(Type type, object obj, int depth)
		{
			if (++depth > MaxDepth)
			{
				throw LiteException.DocumentMaxDepth(MaxDepth, type);
			}
			if (obj == null)
			{
				return BsonValue.Null;
			}
			BsonValue bsonValue = obj as BsonValue;
			if ((object)bsonValue != null)
			{
				return bsonValue;
			}
			if (_customSerializer.TryGetValue(type, out var custom) || _customSerializer.TryGetValue(obj.GetType(), out custom))
			{
				return custom(obj);
			}
			if (obj is string)
			{
				string str = (TrimWhitespace ? (obj as string).Trim() : ((string)obj));
				if (EmptyStringToNull && str.Length == 0)
				{
					return BsonValue.Null;
				}
				return new BsonValue(str);
			}
			if (obj is int)
			{
				return new BsonValue((int)obj);
			}
			if (obj is long)
			{
				return new BsonValue((long)obj);
			}
			if (obj is double)
			{
				return new BsonValue((double)obj);
			}
			if (obj is decimal)
			{
				return new BsonValue((decimal)obj);
			}
			if (obj is byte[])
			{
				return new BsonValue((byte[])obj);
			}
			if (obj is ObjectId)
			{
				return new BsonValue((ObjectId)obj);
			}
			if (obj is Guid)
			{
				return new BsonValue((Guid)obj);
			}
			if (obj is bool)
			{
				return new BsonValue((bool)obj);
			}
			if (obj is DateTime)
			{
				return new BsonValue((DateTime)obj);
			}
			if (obj is short || obj is ushort || obj is byte || obj is sbyte)
			{
				return new BsonValue(Convert.ToInt32(obj));
			}
			if (obj is uint)
			{
				return new BsonValue(Convert.ToInt64(obj));
			}
			if (obj is ulong)
			{
				return new BsonValue((long)(ulong)obj);
			}
			if (obj is float)
			{
				return new BsonValue(Convert.ToDouble(obj));
			}
			if (obj is char)
			{
				return new BsonValue(obj.ToString());
			}
			if (obj is Enum)
			{
				if (EnumAsInteger)
				{
					return new BsonValue((int)obj);
				}
				return new BsonValue(obj.ToString());
			}
			IDictionary dict = obj as IDictionary;
			if (dict != null)
			{
				if (type == typeof(object))
				{
					type = obj.GetType();
				}
				Type itemType = (type.GetTypeInfo().IsGenericType ? type.GetGenericArguments()[1] : typeof(object));
				return SerializeDictionary(itemType, dict, depth);
			}
			if (obj is IEnumerable)
			{
				return SerializeArray(Reflection.GetListItemType(type), obj as IEnumerable, depth);
			}
			return SerializeObject(type, obj, depth);
		}

		private BsonArray SerializeArray(Type type, IEnumerable array, int depth)
		{
			BsonArray arr = new BsonArray();
			foreach (object item in array)
			{
				arr.Add(Serialize(type, item, depth));
			}
			return arr;
		}

		private BsonDocument SerializeDictionary(Type type, IDictionary dict, int depth)
		{
			BsonDocument o = new BsonDocument();
			foreach (object key in dict.Keys)
			{
				object value = dict[key];
				string skey = key.ToString();
				if (key is DateTime)
				{
					skey = ((DateTime)key).ToString("o");
				}
				o[skey] = Serialize(type, value, depth);
			}
			return o;
		}

		private BsonDocument SerializeObject(Type type, object obj, int depth)
		{
			Type t = obj.GetType();
			BsonDocument doc = new BsonDocument();
			EntityMapper entityMapper = GetEntityMapper(t);
			if (type != t)
			{
				doc["_type"] = new BsonValue(_typeNameBinder.GetName(t));
			}
			foreach (MemberMapper member in entityMapper.Members.Where((MemberMapper x) => x.Getter != null))
			{
				object value = member.Getter(obj);
				if (value != null || SerializeNullValues || !(member.FieldName != "_id"))
				{
					if (member.Serialize != null)
					{
						doc[member.FieldName] = member.Serialize(value, this);
					}
					else
					{
						doc[member.FieldName] = Serialize(member.DataType, value, depth);
					}
				}
			}
			return doc;
		}
	}
}
