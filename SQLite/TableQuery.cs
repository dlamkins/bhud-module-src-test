using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SQLite
{
	public class TableQuery<T> : BaseTableQuery, IEnumerable<T>, IEnumerable
	{
		private class CompileResult
		{
			public string CommandText { get; set; }

			public object Value { get; set; }
		}

		private Expression _where;

		private List<Ordering> _orderBys;

		private int? _limit;

		private int? _offset;

		private BaseTableQuery _joinInner;

		private Expression _joinInnerKeySelector;

		private BaseTableQuery _joinOuter;

		private Expression _joinOuterKeySelector;

		private Expression _joinSelector;

		private Expression _selector;

		private bool _deferred;

		public SQLiteConnection Connection { get; private set; }

		public TableMapping Table { get; private set; }

		private TableQuery(SQLiteConnection conn, TableMapping table)
		{
			Connection = conn;
			Table = table;
		}

		public TableQuery(SQLiteConnection conn)
		{
			Connection = conn;
			Table = Connection.GetMapping(typeof(T));
		}

		public TableQuery<U> Clone<U>()
		{
			TableQuery<U> q = new TableQuery<U>(Connection, Table);
			q._where = _where;
			q._deferred = _deferred;
			if (_orderBys != null)
			{
				q._orderBys = new List<Ordering>(_orderBys);
			}
			q._limit = _limit;
			q._offset = _offset;
			q._joinInner = _joinInner;
			q._joinInnerKeySelector = _joinInnerKeySelector;
			q._joinOuter = _joinOuter;
			q._joinOuterKeySelector = _joinOuterKeySelector;
			q._joinSelector = _joinSelector;
			q._selector = _selector;
			return q;
		}

		public TableQuery<T> Where(Expression<Func<T, bool>> predExpr)
		{
			if (predExpr.NodeType == ExpressionType.Lambda)
			{
				Expression pred = predExpr.Body;
				TableQuery<T> tableQuery = Clone<T>();
				tableQuery.AddWhere(pred);
				return tableQuery;
			}
			throw new NotSupportedException("Must be a predicate");
		}

		public int Delete()
		{
			return Delete(null);
		}

		public int Delete(Expression<Func<T, bool>> predExpr)
		{
			if (_limit.HasValue || _offset.HasValue)
			{
				throw new InvalidOperationException("Cannot delete with limits or offsets");
			}
			if (_where == null && predExpr == null)
			{
				throw new InvalidOperationException("No condition specified");
			}
			Expression pred = _where;
			if (predExpr != null && predExpr.NodeType == ExpressionType.Lambda)
			{
				pred = ((pred != null) ? Expression.AndAlso(pred, predExpr.Body) : predExpr.Body);
			}
			List<object> args = new List<object>();
			string cmdText = "delete from \"" + Table.TableName + "\"";
			CompileResult w = CompileExpr(pred, args);
			cmdText = cmdText + " where " + w.CommandText;
			return Connection.CreateCommand(cmdText, args.ToArray()).ExecuteNonQuery();
		}

		public TableQuery<T> Take(int n)
		{
			TableQuery<T> tableQuery = Clone<T>();
			tableQuery._limit = n;
			return tableQuery;
		}

		public TableQuery<T> Skip(int n)
		{
			TableQuery<T> tableQuery = Clone<T>();
			tableQuery._offset = n;
			return tableQuery;
		}

		public T ElementAt(int index)
		{
			return Skip(index).Take(1).First();
		}

		public TableQuery<T> Deferred()
		{
			TableQuery<T> tableQuery = Clone<T>();
			tableQuery._deferred = true;
			return tableQuery;
		}

		public TableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return AddOrderBy(orderExpr, asc: true);
		}

		public TableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return AddOrderBy(orderExpr, asc: false);
		}

		public TableQuery<T> ThenBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return AddOrderBy(orderExpr, asc: true);
		}

		public TableQuery<T> ThenByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return AddOrderBy(orderExpr, asc: false);
		}

		private TableQuery<T> AddOrderBy<U>(Expression<Func<T, U>> orderExpr, bool asc)
		{
			if (orderExpr.NodeType == ExpressionType.Lambda)
			{
				MemberExpression mem = null;
				UnaryExpression unary = orderExpr.Body as UnaryExpression;
				mem = ((unary == null || unary.NodeType != ExpressionType.Convert) ? (orderExpr.Body as MemberExpression) : (unary.Operand as MemberExpression));
				if (mem != null && mem.Expression.NodeType == ExpressionType.Parameter)
				{
					TableQuery<T> q = Clone<T>();
					if (q._orderBys == null)
					{
						q._orderBys = new List<Ordering>();
					}
					q._orderBys.Add(new Ordering
					{
						ColumnName = Table.FindColumnWithPropertyName(mem.Member.Name).Name,
						Ascending = asc
					});
					return q;
				}
				throw new NotSupportedException("Order By does not support: " + orderExpr);
			}
			throw new NotSupportedException("Must be a predicate");
		}

		private void AddWhere(Expression pred)
		{
			if (_where == null)
			{
				_where = pred;
			}
			else
			{
				_where = Expression.AndAlso(_where, pred);
			}
		}

		private SQLiteCommand GenerateCommand(string selectionList)
		{
			if (_joinInner != null && _joinOuter != null)
			{
				throw new NotSupportedException("Joins are not supported.");
			}
			string cmdText = "select " + selectionList + " from \"" + Table.TableName + "\"";
			List<object> args = new List<object>();
			if (_where != null)
			{
				CompileResult w = CompileExpr(_where, args);
				cmdText = cmdText + " where " + w.CommandText;
			}
			if (_orderBys != null && _orderBys.Count > 0)
			{
				string t = string.Join(", ", _orderBys.Select((Ordering o) => "\"" + o.ColumnName + "\"" + (o.Ascending ? "" : " desc")).ToArray());
				cmdText = cmdText + " order by " + t;
			}
			if (_limit.HasValue)
			{
				cmdText = cmdText + " limit " + _limit.Value;
			}
			if (_offset.HasValue)
			{
				if (!_limit.HasValue)
				{
					cmdText += " limit -1 ";
				}
				cmdText = cmdText + " offset " + _offset.Value;
			}
			return Connection.CreateCommand(cmdText, args.ToArray());
		}

		private CompileResult CompileExpr(Expression expr, List<object> queryArgs)
		{
			if (expr == null)
			{
				throw new NotSupportedException("Expression is NULL");
			}
			if (expr is BinaryExpression)
			{
				BinaryExpression bin = (BinaryExpression)expr;
				if (bin.Left.NodeType == ExpressionType.Call)
				{
					MethodCallExpression call2 = (MethodCallExpression)bin.Left;
					if (call2.Method.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators" && call2.Method.Name == "CompareString")
					{
						bin = Expression.MakeBinary(bin.NodeType, call2.Arguments[0], call2.Arguments[1]);
					}
				}
				CompileResult leftr = CompileExpr(bin.Left, queryArgs);
				CompileResult rightr = CompileExpr(bin.Right, queryArgs);
				string text = ((leftr.CommandText == "?" && leftr.Value == null) ? CompileNullBinaryExpression(bin, rightr) : ((!(rightr.CommandText == "?") || rightr.Value != null) ? ("(" + leftr.CommandText + " " + GetSqlName(bin) + " " + rightr.CommandText + ")") : CompileNullBinaryExpression(bin, leftr)));
				return new CompileResult
				{
					CommandText = text
				};
			}
			if (expr.NodeType == ExpressionType.Not)
			{
				Expression operandExpr = ((UnaryExpression)expr).Operand;
				CompileResult opr = CompileExpr(operandExpr, queryArgs);
				object val2 = opr.Value;
				if (val2 is bool)
				{
					val2 = !(bool)val2;
				}
				return new CompileResult
				{
					CommandText = "NOT(" + opr.CommandText + ")",
					Value = val2
				};
			}
			if (expr.NodeType == ExpressionType.Call)
			{
				MethodCallExpression call = (MethodCallExpression)expr;
				CompileResult[] args = new CompileResult[call.Arguments.Count];
				CompileResult obj2 = ((call.Object != null) ? CompileExpr(call.Object, queryArgs) : null);
				for (int i = 0; i < args.Length; i++)
				{
					args[i] = CompileExpr(call.Arguments[i], queryArgs);
				}
				string sqlCall = "";
				if (call.Method.Name == "Like" && args.Length == 2)
				{
					sqlCall = "(" + args[0].CommandText + " like " + args[1].CommandText + ")";
				}
				else if (call.Method.Name == "Contains" && args.Length == 2)
				{
					sqlCall = "(" + args[1].CommandText + " in " + args[0].CommandText + ")";
				}
				else if (call.Method.Name == "Contains" && args.Length == 1)
				{
					sqlCall = ((call.Object == null || !(call.Object.Type == typeof(string))) ? ("(" + args[0].CommandText + " in " + obj2.CommandText + ")") : ("( instr(" + obj2.CommandText + "," + args[0].CommandText + ") >0 )"));
				}
				else if (call.Method.Name == "StartsWith" && args.Length >= 1)
				{
					StringComparison startsWithCmpOp = StringComparison.CurrentCulture;
					if (args.Length == 2)
					{
						startsWithCmpOp = (StringComparison)args[1].Value;
					}
					switch (startsWithCmpOp)
					{
					case StringComparison.CurrentCulture:
					case StringComparison.Ordinal:
						sqlCall = "( substr(" + obj2.CommandText + ", 1, " + args[0].Value.ToString().Length + ") =  " + args[0].CommandText + ")";
						break;
					case StringComparison.CurrentCultureIgnoreCase:
					case StringComparison.OrdinalIgnoreCase:
						sqlCall = "(" + obj2.CommandText + " like (" + args[0].CommandText + " || '%'))";
						break;
					}
				}
				else if (!(call.Method.Name == "EndsWith") || args.Length < 1)
				{
					sqlCall = ((call.Method.Name == "Equals" && args.Length == 1) ? ("(" + obj2.CommandText + " = (" + args[0].CommandText + "))") : ((call.Method.Name == "ToLower") ? ("(lower(" + obj2.CommandText + "))") : ((call.Method.Name == "ToUpper") ? ("(upper(" + obj2.CommandText + "))") : ((!(call.Method.Name == "Replace") || args.Length != 2) ? (call.Method.Name.ToLower() + "(" + string.Join(",", args.Select((CompileResult a) => a.CommandText).ToArray()) + ")") : ("(replace(" + obj2.CommandText + "," + args[0].CommandText + "," + args[1].CommandText + "))")))));
				}
				else
				{
					StringComparison endsWithCmpOp = StringComparison.CurrentCulture;
					if (args.Length == 2)
					{
						endsWithCmpOp = (StringComparison)args[1].Value;
					}
					switch (endsWithCmpOp)
					{
					case StringComparison.CurrentCulture:
					case StringComparison.Ordinal:
						sqlCall = "( substr(" + obj2.CommandText + ", length(" + obj2.CommandText + ") - " + args[0].Value.ToString().Length + "+1, " + args[0].Value.ToString().Length + ") =  " + args[0].CommandText + ")";
						break;
					case StringComparison.CurrentCultureIgnoreCase:
					case StringComparison.OrdinalIgnoreCase:
						sqlCall = "(" + obj2.CommandText + " like ('%' || " + args[0].CommandText + "))";
						break;
					}
				}
				return new CompileResult
				{
					CommandText = sqlCall
				};
			}
			if (expr.NodeType == ExpressionType.Constant)
			{
				ConstantExpression c = (ConstantExpression)expr;
				queryArgs.Add(c.Value);
				return new CompileResult
				{
					CommandText = "?",
					Value = c.Value
				};
			}
			if (expr.NodeType == ExpressionType.Convert)
			{
				UnaryExpression u = (UnaryExpression)expr;
				Type ty = u.Type;
				CompileResult valr = CompileExpr(u.Operand, queryArgs);
				return new CompileResult
				{
					CommandText = valr.CommandText,
					Value = ((valr.Value != null) ? ConvertTo(valr.Value, ty) : null)
				};
			}
			if (expr.NodeType == ExpressionType.MemberAccess)
			{
				MemberExpression mem = (MemberExpression)expr;
				ParameterExpression paramExpr = mem.Expression as ParameterExpression;
				if (paramExpr == null)
				{
					UnaryExpression convert = mem.Expression as UnaryExpression;
					if (convert != null && convert.NodeType == ExpressionType.Convert)
					{
						paramExpr = convert.Operand as ParameterExpression;
					}
				}
				if (paramExpr != null)
				{
					string columnName = Table.FindColumnWithPropertyName(mem.Member.Name).Name;
					return new CompileResult
					{
						CommandText = "\"" + columnName + "\""
					};
				}
				object obj = null;
				if (mem.Expression != null)
				{
					CompileResult compileResult = CompileExpr(mem.Expression, queryArgs);
					if (compileResult.Value == null)
					{
						throw new NotSupportedException("Member access failed to compile expression");
					}
					if (compileResult.CommandText == "?")
					{
						queryArgs.RemoveAt(queryArgs.Count - 1);
					}
					obj = compileResult.Value;
				}
				object val = null;
				if (mem.Member is PropertyInfo)
				{
					val = ((PropertyInfo)mem.Member).GetValue(obj, null);
				}
				else
				{
					if (!(mem.Member is FieldInfo))
					{
						throw new NotSupportedException("MemberExpr: " + mem.Member.GetType());
					}
					val = ((FieldInfo)mem.Member).GetValue(obj);
				}
				if (val != null && val is IEnumerable && !(val is string) && !(val is IEnumerable<byte>))
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("(");
					string head = "";
					foreach (object a2 in (IEnumerable)val)
					{
						queryArgs.Add(a2);
						sb.Append(head);
						sb.Append("?");
						head = ",";
					}
					sb.Append(")");
					return new CompileResult
					{
						CommandText = sb.ToString(),
						Value = val
					};
				}
				queryArgs.Add(val);
				return new CompileResult
				{
					CommandText = "?",
					Value = val
				};
			}
			throw new NotSupportedException("Cannot compile: " + expr.NodeType);
		}

		private static object ConvertTo(object obj, Type t)
		{
			Type nut = Nullable.GetUnderlyingType(t);
			if (nut != null)
			{
				if (obj == null)
				{
					return null;
				}
				return Convert.ChangeType(obj, nut);
			}
			return Convert.ChangeType(obj, t);
		}

		private string CompileNullBinaryExpression(BinaryExpression expression, CompileResult parameter)
		{
			if (expression.NodeType == ExpressionType.Equal)
			{
				return "(" + parameter.CommandText + " is ?)";
			}
			if (expression.NodeType == ExpressionType.NotEqual)
			{
				return "(" + parameter.CommandText + " is not ?)";
			}
			if (expression.NodeType == ExpressionType.GreaterThan || expression.NodeType == ExpressionType.GreaterThanOrEqual || expression.NodeType == ExpressionType.LessThan || expression.NodeType == ExpressionType.LessThanOrEqual)
			{
				return "(" + parameter.CommandText + " < ?)";
			}
			throw new NotSupportedException("Cannot compile Null-BinaryExpression with type " + expression.NodeType);
		}

		private string GetSqlName(Expression expr)
		{
			ExpressionType i = expr.NodeType;
			return i switch
			{
				ExpressionType.GreaterThan => ">", 
				ExpressionType.GreaterThanOrEqual => ">=", 
				ExpressionType.LessThan => "<", 
				ExpressionType.LessThanOrEqual => "<=", 
				ExpressionType.And => "&", 
				ExpressionType.AndAlso => "and", 
				ExpressionType.Or => "|", 
				ExpressionType.OrElse => "or", 
				ExpressionType.Equal => "=", 
				ExpressionType.NotEqual => "!=", 
				_ => throw new NotSupportedException("Cannot get SQL for: " + i), 
			};
		}

		public int Count()
		{
			return GenerateCommand("count(*)").ExecuteScalar<int>();
		}

		public int Count(Expression<Func<T, bool>> predExpr)
		{
			return Where(predExpr).Count();
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (!_deferred)
			{
				return GenerateCommand("*").ExecuteQuery<T>().GetEnumerator();
			}
			return GenerateCommand("*").ExecuteDeferredQuery<T>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public List<T> ToList()
		{
			return GenerateCommand("*").ExecuteQuery<T>();
		}

		public T[] ToArray()
		{
			return GenerateCommand("*").ExecuteQuery<T>().ToArray();
		}

		public T First()
		{
			return Take(1).ToList().First();
		}

		public T FirstOrDefault()
		{
			return Take(1).ToList().FirstOrDefault();
		}

		public T First(Expression<Func<T, bool>> predExpr)
		{
			return Where(predExpr).First();
		}

		public T FirstOrDefault(Expression<Func<T, bool>> predExpr)
		{
			return Where(predExpr).FirstOrDefault();
		}
	}
}
