using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLite
{
	public class TableMapping
	{
		public class Column
		{
			private PropertyInfo _prop;

			public string Name { get; private set; }

			public PropertyInfo PropertyInfo => _prop;

			public string PropertyName => _prop.Name;

			public Type ColumnType { get; private set; }

			public string Collation { get; private set; }

			public bool IsAutoInc { get; private set; }

			public bool IsAutoGuid { get; private set; }

			public bool IsPK { get; private set; }

			public IEnumerable<IndexedAttribute> Indices { get; set; }

			public bool IsNullable { get; private set; }

			public int? MaxStringLength { get; private set; }

			public bool StoreAsText { get; private set; }

			public Column(PropertyInfo prop, CreateFlags createFlags = CreateFlags.None)
			{
				CustomAttributeData colAttr = prop.CustomAttributes.FirstOrDefault((CustomAttributeData x) => x.AttributeType == typeof(ColumnAttribute));
				_prop = prop;
				Name = ((colAttr == null || colAttr.ConstructorArguments.Count <= 0) ? prop.Name : colAttr.ConstructorArguments[0].Value?.ToString());
				ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
				Collation = Orm.Collation(prop);
				IsPK = Orm.IsPK(prop) || ((createFlags & CreateFlags.ImplicitPK) == CreateFlags.ImplicitPK && string.Compare(prop.Name, "Id", StringComparison.OrdinalIgnoreCase) == 0);
				bool isAuto = Orm.IsAutoInc(prop) || (IsPK && (createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK);
				IsAutoGuid = isAuto && ColumnType == typeof(Guid);
				IsAutoInc = isAuto && !IsAutoGuid;
				Indices = Orm.GetIndices(prop);
				if (!Indices.Any() && !IsPK && (createFlags & CreateFlags.ImplicitIndex) == CreateFlags.ImplicitIndex && Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
				{
					Indices = new IndexedAttribute[1]
					{
						new IndexedAttribute()
					};
				}
				IsNullable = !IsPK && !Orm.IsMarkedNotNull(prop);
				MaxStringLength = Orm.MaxStringLength(prop);
				StoreAsText = prop.PropertyType.GetTypeInfo().CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(StoreAsTextAttribute));
			}

			public void SetValue(object obj, object val)
			{
				if (val != null && ColumnType.GetTypeInfo().IsEnum)
				{
					_prop.SetValue(obj, Enum.ToObject(ColumnType, val));
				}
				else
				{
					_prop.SetValue(obj, val, null);
				}
			}

			public object GetValue(object obj)
			{
				return _prop.GetValue(obj, null);
			}
		}

		private readonly Column _autoPk;

		private readonly Column[] _insertColumns;

		private readonly Column[] _insertOrReplaceColumns;

		public Type MappedType { get; private set; }

		public string TableName { get; private set; }

		public bool WithoutRowId { get; private set; }

		public Column[] Columns { get; private set; }

		public Column PK { get; private set; }

		public string GetByPrimaryKeySql { get; private set; }

		public CreateFlags CreateFlags { get; private set; }

		public bool HasAutoIncPK { get; private set; }

		public Column[] InsertColumns => _insertColumns;

		public Column[] InsertOrReplaceColumns => _insertOrReplaceColumns;

		public TableMapping(Type type, CreateFlags createFlags = CreateFlags.None)
		{
			MappedType = type;
			CreateFlags = createFlags;
			TableAttribute tableAttr = (from x in type.GetTypeInfo().CustomAttributes
				where x.AttributeType == typeof(TableAttribute)
				select (TableAttribute)Orm.InflateAttribute(x)).FirstOrDefault();
			TableName = ((tableAttr != null && !string.IsNullOrEmpty(tableAttr.Name)) ? tableAttr.Name : MappedType.Name);
			WithoutRowId = tableAttr?.WithoutRowId ?? false;
			List<PropertyInfo> props = new List<PropertyInfo>();
			Type baseType = type;
			HashSet<string> propNames = new HashSet<string>();
			while (baseType != typeof(object))
			{
				TypeInfo ti = baseType.GetTypeInfo();
				List<PropertyInfo> newProps = ti.DeclaredProperties.Where((PropertyInfo p) => !propNames.Contains(p.Name) && p.CanRead && p.CanWrite && p.GetMethod != null && p.SetMethod != null && p.GetMethod.IsPublic && p.SetMethod.IsPublic && !p.GetMethod.IsStatic && !p.SetMethod.IsStatic).ToList();
				foreach (PropertyInfo p2 in newProps)
				{
					propNames.Add(p2.Name);
				}
				props.AddRange(newProps);
				baseType = ti.BaseType;
			}
			List<Column> cols = new List<Column>();
			foreach (PropertyInfo p3 in props)
			{
				if (!p3.IsDefined(typeof(IgnoreAttribute), inherit: true))
				{
					cols.Add(new Column(p3, createFlags));
				}
			}
			Columns = cols.ToArray();
			Column[] columns = Columns;
			foreach (Column c2 in columns)
			{
				if (c2.IsAutoInc && c2.IsPK)
				{
					_autoPk = c2;
				}
				if (c2.IsPK)
				{
					PK = c2;
				}
			}
			HasAutoIncPK = _autoPk != null;
			if (PK != null)
			{
				GetByPrimaryKeySql = $"select * from \"{TableName}\" where \"{PK.Name}\" = ?";
			}
			else
			{
				GetByPrimaryKeySql = $"select * from \"{TableName}\" limit 1";
			}
			_insertColumns = Columns.Where((Column c) => !c.IsAutoInc).ToArray();
			_insertOrReplaceColumns = Columns.ToArray();
		}

		public void SetAutoIncPK(object obj, long id)
		{
			if (_autoPk != null)
			{
				_autoPk.SetValue(obj, Convert.ChangeType(id, _autoPk.ColumnType, null));
			}
		}

		public Column FindColumnWithPropertyName(string propertyName)
		{
			return Columns.FirstOrDefault((Column c) => c.PropertyName == propertyName);
		}

		public Column FindColumn(string columnName)
		{
			return Columns.FirstOrDefault((Column c) => c.Name.ToLower() == columnName.ToLower());
		}
	}
}
