using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLite
{
	public class SQLiteCommand
	{
		private class Binding
		{
			public string Name { get; set; }

			public object Value { get; set; }

			public int Index { get; set; }
		}

		private SQLiteConnection _conn;

		private List<Binding> _bindings;

		private static IntPtr NegativePointer = new IntPtr(-1);

		public string CommandText { get; set; }

		public SQLiteCommand(SQLiteConnection conn)
		{
			_conn = conn;
			_bindings = new List<Binding>();
			CommandText = "";
		}

		public int ExecuteNonQuery()
		{
			if (_conn.Trace)
			{
				_conn.Tracer?.Invoke("Executing: " + this);
			}
			SQLite3.Result r = SQLite3.Result.OK;
			IntPtr stmt = Prepare();
			r = SQLite3.Step(stmt);
			Finalize(stmt);
			switch (r)
			{
			case SQLite3.Result.Done:
				return SQLite3.Changes(_conn.Handle);
			case SQLite3.Result.Error:
			{
				string msg = SQLite3.GetErrmsg(_conn.Handle);
				throw SQLiteException.New(r, msg);
			}
			case SQLite3.Result.Constraint:
				if (SQLite3.ExtendedErrCode(_conn.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
				{
					throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(_conn.Handle));
				}
				break;
			}
			throw SQLiteException.New(r, r.ToString());
		}

		public IEnumerable<T> ExecuteDeferredQuery<T>()
		{
			return ExecuteDeferredQuery<T>(_conn.GetMapping(typeof(T)));
		}

		public List<T> ExecuteQuery<T>()
		{
			return ExecuteDeferredQuery<T>(_conn.GetMapping(typeof(T))).ToList();
		}

		public List<T> ExecuteQuery<T>(TableMapping map)
		{
			return ExecuteDeferredQuery<T>(map).ToList();
		}

		protected virtual void OnInstanceCreated(object obj)
		{
		}

		public IEnumerable<T> ExecuteDeferredQuery<T>(TableMapping map)
		{
			if (_conn.Trace)
			{
				_conn.Tracer?.Invoke("Executing Query: " + this);
			}
			IntPtr stmt = Prepare();
			try
			{
				TableMapping.Column[] cols = new TableMapping.Column[SQLite3.ColumnCount(stmt)];
				for (int i = 0; i < cols.Length; i++)
				{
					string name = SQLite3.ColumnName16(stmt, i);
					cols[i] = map.FindColumn(name);
				}
				while (SQLite3.Step(stmt) == SQLite3.Result.Row)
				{
					object obj = Activator.CreateInstance(map.MappedType);
					for (int j = 0; j < cols.Length; j++)
					{
						if (cols[j] != null)
						{
							SQLite3.ColType colType = SQLite3.ColumnType(stmt, j);
							object val = ReadCol(stmt, j, colType, cols[j].ColumnType);
							cols[j].SetValue(obj, val);
						}
					}
					OnInstanceCreated(obj);
					yield return (T)obj;
				}
			}
			finally
			{
				SQLite3.Finalize(stmt);
			}
		}

		public T ExecuteScalar<T>()
		{
			if (_conn.Trace)
			{
				_conn.Tracer?.Invoke("Executing Query: " + this);
			}
			T val = default(T);
			IntPtr stmt = Prepare();
			try
			{
				SQLite3.Result r = SQLite3.Step(stmt);
				switch (r)
				{
				case SQLite3.Result.Row:
				{
					SQLite3.ColType colType = SQLite3.ColumnType(stmt, 0);
					return (T)ReadCol(stmt, 0, colType, typeof(T));
				}
				default:
					throw SQLiteException.New(r, SQLite3.GetErrmsg(_conn.Handle));
				case SQLite3.Result.Done:
					return val;
				}
			}
			finally
			{
				Finalize(stmt);
			}
		}

		public void Bind(string name, object val)
		{
			_bindings.Add(new Binding
			{
				Name = name,
				Value = val
			});
		}

		public void Bind(object val)
		{
			Bind(null, val);
		}

		public override string ToString()
		{
			string[] parts = new string[1 + _bindings.Count];
			parts[0] = CommandText;
			int i = 1;
			foreach (Binding b in _bindings)
			{
				parts[i] = $"  {i - 1}: {b.Value}";
				i++;
			}
			return string.Join(Environment.NewLine, parts);
		}

		private IntPtr Prepare()
		{
			IntPtr stmt = SQLite3.Prepare2(_conn.Handle, CommandText);
			BindAll(stmt);
			return stmt;
		}

		private void Finalize(IntPtr stmt)
		{
			SQLite3.Finalize(stmt);
		}

		private void BindAll(IntPtr stmt)
		{
			int nextIdx = 1;
			foreach (Binding b in _bindings)
			{
				if (b.Name != null)
				{
					b.Index = SQLite3.BindParameterIndex(stmt, b.Name);
				}
				else
				{
					b.Index = nextIdx++;
				}
				BindParameter(stmt, b.Index, b.Value, _conn.StoreDateTimeAsTicks, _conn.DateTimeStringFormat, _conn.StoreTimeSpanAsTicks);
			}
		}

		internal static void BindParameter(IntPtr stmt, int index, object value, bool storeDateTimeAsTicks, string dateTimeStringFormat, bool storeTimeSpanAsTicks)
		{
			if (value == null)
			{
				SQLite3.BindNull(stmt, index);
				return;
			}
			if (value is int)
			{
				SQLite3.BindInt(stmt, index, (int)value);
				return;
			}
			if (value is string)
			{
				SQLite3.BindText(stmt, index, (string)value, -1, NegativePointer);
				return;
			}
			if (value is byte || value is ushort || value is sbyte || value is short)
			{
				SQLite3.BindInt(stmt, index, Convert.ToInt32(value));
				return;
			}
			if (value is bool)
			{
				SQLite3.BindInt(stmt, index, ((bool)value) ? 1 : 0);
				return;
			}
			if (value is uint || value is long)
			{
				SQLite3.BindInt64(stmt, index, Convert.ToInt64(value));
				return;
			}
			if (value is float || value is double || value is decimal)
			{
				SQLite3.BindDouble(stmt, index, Convert.ToDouble(value));
				return;
			}
			if (value is TimeSpan)
			{
				if (storeTimeSpanAsTicks)
				{
					SQLite3.BindInt64(stmt, index, ((TimeSpan)value).Ticks);
				}
				else
				{
					SQLite3.BindText(stmt, index, ((TimeSpan)value).ToString(), -1, NegativePointer);
				}
				return;
			}
			if (value is DateTime)
			{
				if (storeDateTimeAsTicks)
				{
					SQLite3.BindInt64(stmt, index, ((DateTime)value).Ticks);
				}
				else
				{
					SQLite3.BindText(stmt, index, ((DateTime)value).ToString(dateTimeStringFormat, CultureInfo.InvariantCulture), -1, NegativePointer);
				}
				return;
			}
			if (value is DateTimeOffset)
			{
				SQLite3.BindInt64(stmt, index, ((DateTimeOffset)value).UtcTicks);
				return;
			}
			if (value is byte[])
			{
				SQLite3.BindBlob(stmt, index, (byte[])value, ((byte[])value).Length, NegativePointer);
				return;
			}
			if (value is Guid)
			{
				SQLite3.BindText(stmt, index, ((Guid)value).ToString(), 72, NegativePointer);
				return;
			}
			if (value is Uri)
			{
				SQLite3.BindText(stmt, index, ((Uri)value).ToString(), -1, NegativePointer);
				return;
			}
			if (value is StringBuilder)
			{
				SQLite3.BindText(stmt, index, ((StringBuilder)value).ToString(), -1, NegativePointer);
				return;
			}
			if (value is UriBuilder)
			{
				SQLite3.BindText(stmt, index, ((UriBuilder)value).ToString(), -1, NegativePointer);
				return;
			}
			EnumCacheInfo enumInfo = EnumCache.GetInfo(value.GetType());
			if (enumInfo.IsEnum)
			{
				int enumIntValue = Convert.ToInt32(value);
				if (enumInfo.StoreAsText)
				{
					SQLite3.BindText(stmt, index, enumInfo.EnumValues[enumIntValue], -1, NegativePointer);
				}
				else
				{
					SQLite3.BindInt(stmt, index, enumIntValue);
				}
				return;
			}
			throw new NotSupportedException("Cannot store type: " + Orm.GetType(value));
		}

		private object ReadCol(IntPtr stmt, int index, SQLite3.ColType type, Type clrType)
		{
			if (type == SQLite3.ColType.Null)
			{
				return null;
			}
			TypeInfo clrTypeInfo = clrType.GetTypeInfo();
			if (clrTypeInfo.IsGenericType && clrTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				clrType = clrTypeInfo.GenericTypeArguments[0];
				clrTypeInfo = clrType.GetTypeInfo();
			}
			if (clrType == typeof(string))
			{
				return SQLite3.ColumnString(stmt, index);
			}
			if (clrType == typeof(int))
			{
				return SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(bool))
			{
				return SQLite3.ColumnInt(stmt, index) == 1;
			}
			if (clrType == typeof(double))
			{
				return SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(float))
			{
				return (float)SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(TimeSpan))
			{
				if (_conn.StoreTimeSpanAsTicks)
				{
					return new TimeSpan(SQLite3.ColumnInt64(stmt, index));
				}
				string text2 = SQLite3.ColumnString(stmt, index);
				if (!TimeSpan.TryParseExact(text2, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None, out var resultTime))
				{
					resultTime = TimeSpan.Parse(text2);
				}
				return resultTime;
			}
			if (clrType == typeof(DateTime))
			{
				if (_conn.StoreDateTimeAsTicks)
				{
					return new DateTime(SQLite3.ColumnInt64(stmt, index));
				}
				string text = SQLite3.ColumnString(stmt, index);
				if (!DateTime.TryParseExact(text, _conn.DateTimeStringFormat, CultureInfo.InvariantCulture, _conn.DateTimeStyle, out var resultDate))
				{
					resultDate = DateTime.Parse(text);
				}
				return resultDate;
			}
			if (clrType == typeof(DateTimeOffset))
			{
				return new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
			}
			if (clrTypeInfo.IsEnum)
			{
				if (type == SQLite3.ColType.Text)
				{
					string value = SQLite3.ColumnString(stmt, index);
					return Enum.Parse(clrType, value.ToString(), ignoreCase: true);
				}
				return SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(long))
			{
				return SQLite3.ColumnInt64(stmt, index);
			}
			if (clrType == typeof(uint))
			{
				return (uint)SQLite3.ColumnInt64(stmt, index);
			}
			if (clrType == typeof(decimal))
			{
				return (decimal)SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(byte))
			{
				return (byte)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(ushort))
			{
				return (ushort)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(short))
			{
				return (short)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(sbyte))
			{
				return (sbyte)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(byte[]))
			{
				return SQLite3.ColumnByteArray(stmt, index);
			}
			if (clrType == typeof(Guid))
			{
				return new Guid(SQLite3.ColumnString(stmt, index));
			}
			if (clrType == typeof(Uri))
			{
				return new Uri(SQLite3.ColumnString(stmt, index));
			}
			if (clrType == typeof(StringBuilder))
			{
				return new StringBuilder(SQLite3.ColumnString(stmt, index));
			}
			if (clrType == typeof(UriBuilder))
			{
				return new UriBuilder(SQLite3.ColumnString(stmt, index));
			}
			throw new NotSupportedException("Don't know how to read " + clrType);
		}
	}
}
