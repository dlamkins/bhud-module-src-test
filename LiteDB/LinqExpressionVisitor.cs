using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteDB
{
	internal class LinqExpressionVisitor : ExpressionVisitor
	{
		private static readonly Dictionary<Type, ITypeResolver> _resolver = new Dictionary<Type, ITypeResolver>
		{
			[typeof(BsonValue)] = new BsonValueResolver(),
			[typeof(BsonArray)] = new BsonValueResolver(),
			[typeof(BsonDocument)] = new BsonValueResolver(),
			[typeof(Convert)] = new ConvertResolver(),
			[typeof(DateTime)] = new DateTimeResolver(),
			[typeof(int)] = new NumberResolver("INT32"),
			[typeof(long)] = new NumberResolver("INT64"),
			[typeof(decimal)] = new NumberResolver("DECIMAL"),
			[typeof(double)] = new NumberResolver("DOUBLE"),
			[typeof(ICollection)] = new ICollectionResolver(),
			[typeof(Enumerable)] = new EnumerableResolver(),
			[typeof(Guid)] = new GuidResolver(),
			[typeof(Math)] = new MathResolver(),
			[typeof(Regex)] = new RegexResolver(),
			[typeof(ObjectId)] = new ObjectIdResolver(),
			[typeof(string)] = new StringResolver(),
			[typeof(Nullable)] = new NullableResolver()
		};

		private readonly BsonMapper _mapper;

		private readonly Expression _expr;

		private readonly ParameterExpression _rootParameter;

		private readonly BsonDocument _parameters = new BsonDocument();

		private int _paramIndex;

		private Type _dbRefType;

		private readonly StringBuilder _builder = new StringBuilder();

		private readonly Stack<Expression> _nodes = new Stack<Expression>();

		public LinqExpressionVisitor(BsonMapper mapper, Expression expr)
		{
			_mapper = mapper;
			_expr = expr;
			LambdaExpression lambda = expr as LambdaExpression;
			if (lambda != null)
			{
				_rootParameter = lambda.Parameters.First();
				return;
			}
			throw new NotSupportedException("Expression " + expr.ToString() + " must be a lambda expression");
		}

		public BsonExpression Resolve(bool predicate)
		{
			Visit(_expr);
			Constants.ENSURE(_nodes.Count == 0, "node stack must be empty when finish expression resolve");
			string expression = _builder.ToString();
			try
			{
				BsonExpression e = BsonExpression.Create(expression, _parameters);
				if (predicate && (e.Type == BsonExpressionType.Path || e.Type == BsonExpressionType.Call || e.Type == BsonExpressionType.Parameter))
				{
					expression = "(" + expression + " = true)";
					e = BsonExpression.Create(expression, _parameters);
				}
				return e;
			}
			catch (Exception ex)
			{
				throw new NotSupportedException("Invalid BsonExpression when converted from Linq expression: " + _expr.ToString() + " - `" + expression + "`", ex);
			}
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			Expression result = base.VisitLambda(node);
			_builder.Length--;
			return result;
		}

		protected override Expression VisitInvocation(InvocationExpression node)
		{
			Expression result = base.VisitInvocation(node);
			_builder.Length--;
			return result;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			_builder.Append(_rootParameter.Equals(node) ? "$" : "@");
			return base.VisitParameter(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			bool isParam = ParameterExpressionVisitor.Test(node);
			MemberInfo member = node.Member;
			if (TryGetResolver(member.DeclaringType, out var type))
			{
				string pattern = type.ResolveMember(member);
				if (pattern == null)
				{
					throw new NotSupportedException("Member " + member.Name + " are not support in " + member.DeclaringType.Name + " when convert to BsonExpression (" + node.ToString() + ").");
				}
				ResolvePattern(pattern, node.Expression, new Expression[0]);
			}
			else if (node.Expression != null)
			{
				_nodes.Push(node);
				base.Visit(node.Expression);
				if (isParam)
				{
					string name = ResolveMember(member);
					_builder.Append(name);
				}
			}
			else
			{
				object value = Evaluate(node);
				base.Visit(Expression.Constant(value));
			}
			if (_nodes.Count > 0)
			{
				_nodes.Pop();
			}
			return node;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (IsMethodIndexEval(node, out var obj, out var idx))
			{
				Visit(obj);
				object index = Evaluate(idx, typeof(string), typeof(int));
				if (index is string)
				{
					_builder.Append(".");
					_builder.Append($"['{index}']");
				}
				else
				{
					_builder.Append($"[{index}]");
				}
				return node;
			}
			if (!TryGetResolver(node.Method.DeclaringType, out var type))
			{
				if (ParameterExpressionVisitor.Test(node))
				{
					throw new NotSupportedException("Method " + node.Method.Name + " not available to convert to BsonExpression (" + node.ToString() + ").");
				}
				object value = Evaluate(node);
				base.Visit(Expression.Constant(value));
				return node;
			}
			string pattern = type.ResolveMethod(node.Method);
			if (pattern == null)
			{
				throw new NotSupportedException("Method " + Reflection.MethodName(node.Method) + " in " + node.Method.DeclaringType.Name + " are not supported when convert to BsonExpression (" + node.ToString() + ").");
			}
			ResolvePattern(pattern, node.Object, node.Arguments);
			return node;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			object value = node.Value;
			MemberExpression prevNode;
			while (_nodes.Count > 0 && (prevNode = _nodes.Peek() as MemberExpression) != null)
			{
				FieldInfo fieldInfo = prevNode.Member as FieldInfo;
				if ((object)fieldInfo != null)
				{
					value = fieldInfo.GetValue(value);
				}
				else
				{
					PropertyInfo propertyInfo = prevNode.Member as PropertyInfo;
					if ((object)propertyInfo != null)
					{
						value = propertyInfo.GetValue(value);
					}
				}
				_nodes.Pop();
			}
			Constants.ENSURE(_nodes.Count == 0, "counter stack must be zero to eval all properties/field over object");
			string parameter = "p" + _paramIndex++;
			_builder.AppendFormat("@" + parameter);
			Type type = value?.GetType();
			BsonValue arg = ((type == null) ? BsonValue.Null : ((type == typeof(string)) ? new BsonValue((string)value) : _mapper.Serialize(value.GetType(), value)));
			_parameters[parameter] = arg;
			return node;
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType == ExpressionType.Not)
			{
				if (node.Operand.NodeType == ExpressionType.MemberAccess)
				{
					_builder.Append("(");
					Visit(node.Operand);
					_builder.Append(" = false)");
				}
				else
				{
					_builder.Append("(");
					Visit(node.Operand);
					_builder.Append(")");
					_builder.Append(" = false");
				}
			}
			else if (node.NodeType == ExpressionType.Convert)
			{
				Type fromType = node.Operand.Type;
				Type toType = node.Type;
				if ((fromType == typeof(double) || fromType == typeof(decimal)) && (toType == typeof(int) || toType == typeof(long)))
				{
					string methodName = "To" + toType.Name.ToString();
					MethodInfo convert = (from x in typeof(Convert).GetMethods()
						where x.Name == methodName
						where x.GetParameters().Length == 1 && x.GetParameters().Any((ParameterInfo z) => z.ParameterType == fromType)
						select x).FirstOrDefault();
					if (convert == null)
					{
						throw new NotSupportedException("Cast from " + fromType.Name + " are not supported when convert to BsonExpression");
					}
					MethodCallExpression method = Expression.Call(null, convert, node.Operand);
					VisitMethodCall(method);
				}
				else
				{
					base.VisitUnary(node);
				}
			}
			else if (node.NodeType == ExpressionType.ArrayLength)
			{
				_builder.Append("LENGTH(");
				Visit(node.Operand);
				_builder.Append(")");
			}
			else
			{
				base.VisitUnary(node);
			}
			return node;
		}

		protected override Expression VisitNew(NewExpression node)
		{
			if (node.Members == null)
			{
				if (!TryGetResolver(node.Type, out var type))
				{
					throw new NotSupportedException($"New instance are not supported for {node.Type} when convert to BsonExpression ({node.ToString()}).");
				}
				string pattern = type.ResolveCtor(node.Constructor);
				if (pattern == null)
				{
					throw new NotSupportedException("Constructor for " + node.Type.Name + " are not supported when convert to BsonExpression (" + node.ToString() + ").");
				}
				ResolvePattern(pattern, null, node.Arguments);
			}
			else
			{
				_builder.Append("{ ");
				for (int i = 0; i < node.Members.Count; i++)
				{
					MemberInfo member = node.Members[i];
					_builder.Append((i > 0) ? ", " : "");
					_builder.AppendFormat("'{0}': ", member.Name);
					Visit(node.Arguments[i]);
				}
				_builder.Append(" }");
			}
			return node;
		}

		protected override Expression VisitMemberInit(MemberInitExpression node)
		{
			if (node.NewExpression.Constructor.GetParameters().Length != 0)
			{
				throw new NotSupportedException($"New instance of {node.Type} are not supported because contains ctor with parameter. Try use only property initializers: `new {node.Type.Name} {{ PropA = 1, PropB == \"John\" }}`.");
			}
			_builder.Append("{");
			for (int i = 0; i < node.Bindings.Count; i++)
			{
				MemberAssignment bind = node.Bindings[i] as MemberAssignment;
				string member = ResolveMember(bind.Member);
				_builder.Append((i > 0) ? ", " : "");
				_builder.Append(member.Substring(1));
				_builder.Append(":");
				Visit(bind.Expression);
			}
			_builder.Append("}");
			return node;
		}

		protected override Expression VisitNewArray(NewArrayExpression node)
		{
			_builder.Append("[ ");
			for (int i = 0; i < node.Expressions.Count; i++)
			{
				_builder.Append((i > 0) ? ", " : "");
				Visit(node.Expressions[i]);
			}
			_builder.Append(" ]");
			return node;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			bool andOr = node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse;
			if (node.NodeType == ExpressionType.Coalesce)
			{
				return VisitCoalesce(node);
			}
			if (node.NodeType == ExpressionType.ArrayIndex)
			{
				return VisitArrayIndex(node);
			}
			string op = GetOperator(node.NodeType);
			_builder.Append("(");
			VisitAsPredicate(node.Left, andOr);
			_builder.Append(op);
			if (!_mapper.EnumAsInteger && node.Left.NodeType == ExpressionType.Convert)
			{
				UnaryExpression unex = node.Left as UnaryExpression;
				if (unex != null && unex.Operand.Type.GetTypeInfo().IsEnum && unex.Type == typeof(int))
				{
					VisitAsPredicate(Expression.Constant(Enum.GetName(unex.Operand.Type, Evaluate(node.Right))), andOr);
					goto IL_0106;
				}
			}
			VisitAsPredicate(node.Right, andOr);
			goto IL_0106;
			IL_0106:
			_builder.Append(")");
			return node;
		}

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			_builder.Append("IIF(");
			Visit(node.Test);
			_builder.Append(", ");
			Visit(node.IfTrue);
			_builder.Append(", ");
			Visit(node.IfFalse);
			_builder.Append(")");
			return node;
		}

		private Expression VisitCoalesce(BinaryExpression node)
		{
			_builder.Append("COALESCE(");
			Visit(node.Left);
			_builder.Append(", ");
			Visit(node.Right);
			_builder.Append(")");
			return node;
		}

		private Expression VisitArrayIndex(BinaryExpression node)
		{
			Visit(node.Left);
			_builder.Append("[");
			object index = Evaluate(node.Right, typeof(int));
			_builder.Append(index);
			_builder.Append("]");
			return node;
		}

		private void ResolvePattern(string pattern, Expression obj, IList<Expression> args)
		{
			Tokenizer tokenizer = new Tokenizer(pattern);
			while (!tokenizer.EOF)
			{
				Token token = tokenizer.ReadToken(eatWhitespace: false);
				if (token.Type == TokenType.Hashtag)
				{
					Visit(obj);
				}
				else if (token.Type == TokenType.At && tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Int)
				{
					int i = Convert.ToInt32(tokenizer.ReadToken(eatWhitespace: false).Expect(TokenType.Int).Value);
					Visit(args[i]);
				}
				else if (token.Type == TokenType.Percent)
				{
					VisitEnumerablePredicate(args[1] as LambdaExpression);
				}
				else
				{
					_builder.Append((token.Type == TokenType.String) ? ("'" + token.Value + "'") : token.Value);
				}
			}
		}

		private void VisitEnumerablePredicate(LambdaExpression lambda)
		{
			Expression expression = lambda.Body;
			BinaryExpression bin = expression as BinaryExpression;
			if (bin != null)
			{
				if (bin.Left.NodeType != ExpressionType.Parameter)
				{
					throw new LiteException(0, "Any/All requires simple parameter on left side. Eg: `x => x.Phones.Select(p => p.Number).Any(n => n > 5)`");
				}
				string op = GetOperator(bin.NodeType);
				_builder.Append(op);
				VisitAsPredicate(bin.Right, ensurePredicate: false);
				return;
			}
			MethodCallExpression met = expression as MethodCallExpression;
			if (met != null)
			{
				if (met.Object.NodeType != ExpressionType.Parameter)
				{
					throw new NotSupportedException("Any/All requires simple parameter on left side. Eg: `x.Customers.Select(c => c.Name).Any(n => n.StartsWith('J'))`");
				}
				if (!TryGetResolver(met.Method.DeclaringType, out var type))
				{
					throw new NotSupportedException("Method " + met.Method.Name + " not available to convert to BsonExpression inside Any/All call.");
				}
				string pattern = type.ResolveMethod(met.Method);
				if (pattern == null || !pattern.StartsWith("#"))
				{
					throw new NotSupportedException("Method " + met.Method.Name + " not available to convert to BsonExpression inside Any/All call.");
				}
				ResolvePattern(pattern.Substring(1), met.Object, met.Arguments);
				return;
			}
			throw new LiteException(0, "When using Any/All method test do only simple predicate variable. Eg: `x => x.Phones.Select(p => p.Number).Any(n => n > 5)`");
		}

		private string GetOperator(ExpressionType nodeType)
		{
			return nodeType switch
			{
				ExpressionType.Add => " + ", 
				ExpressionType.Multiply => " * ", 
				ExpressionType.Subtract => " - ", 
				ExpressionType.Divide => " / ", 
				ExpressionType.Equal => " = ", 
				ExpressionType.NotEqual => " != ", 
				ExpressionType.GreaterThan => " > ", 
				ExpressionType.GreaterThanOrEqual => " >= ", 
				ExpressionType.LessThan => " < ", 
				ExpressionType.LessThanOrEqual => " <= ", 
				ExpressionType.And => " AND ", 
				ExpressionType.AndAlso => " AND ", 
				ExpressionType.Or => " OR ", 
				ExpressionType.OrElse => " OR ", 
				_ => throw new NotSupportedException($"Operator not supported {nodeType}"), 
			};
		}

		private string ResolveMember(MemberInfo member)
		{
			string name = member.Name;
			bool isParentDbRef = _dbRefType != null && member.DeclaringType.IsAssignableFrom(_dbRefType);
			MemberMapper field = _mapper.GetEntityMapper(member.DeclaringType).Members.FirstOrDefault((MemberMapper x) => x.MemberName == name);
			if (field == null)
			{
				throw new NotSupportedException($"Member {name} not found on BsonMapper for type {member.DeclaringType}.");
			}
			_dbRefType = (field.IsDbRef ? field.UnderlyingType : null);
			return "." + ((isParentDbRef && field.FieldName == "_id") ? "$id" : field.FieldName);
		}

		private bool IsMethodIndexEval(MethodCallExpression node, out Expression obj, out Expression idx)
		{
			MethodInfo method = node.Method;
			_ = method.DeclaringType;
			ParameterInfo[] pars = method.GetParameters();
			if (method.Name == "get_Item" && pars.Length == 1 && (pars[0].ParameterType == typeof(int) || pars[0].ParameterType == typeof(string)))
			{
				obj = node.Object;
				idx = node.Arguments[0];
				return true;
			}
			obj = null;
			idx = null;
			return false;
		}

		private void VisitAsPredicate(Expression expr, bool ensurePredicate)
		{
			ensurePredicate = ensurePredicate && (expr.NodeType == ExpressionType.MemberAccess || expr.NodeType == ExpressionType.Call || expr.NodeType == ExpressionType.Invoke || expr.NodeType == ExpressionType.Constant);
			if (ensurePredicate)
			{
				_builder.Append("(");
				_builder.Append("(");
				base.Visit(expr);
				_builder.Append(")");
				_builder.Append(" = true)");
			}
			else
			{
				base.Visit(expr);
			}
		}

		private object Evaluate(Expression expr, params Type[] validTypes)
		{
			object value = null;
			if (expr.NodeType == ExpressionType.Constant)
			{
				ConstantExpression constant = (ConstantExpression)expr;
				value = constant.Value;
			}
			else
			{
				Delegate func = Expression.Lambda(expr).Compile();
				value = func.DynamicInvoke();
			}
			if (validTypes.Length != 0 && value == null)
			{
				throw new NotSupportedException($"Expression {expr} can't return null value");
			}
			if (validTypes.Length != 0 && !validTypes.Any((Type x) => x == value.GetType()))
			{
				throw new NotSupportedException(string.Format("Expression {0} must return on of this types: {1}", expr, string.Join(", ", validTypes.Select((Type x) => "`" + x.Name + "`"))));
			}
			return value;
		}

		private bool TryGetResolver(Type declaringType, out ITypeResolver typeResolver)
		{
			bool num = Reflection.IsCollection(declaringType);
			bool isEnumerable = Reflection.IsEnumerable(declaringType);
			bool isNullable = Reflection.IsNullable(declaringType);
			Type type = (num ? typeof(ICollection) : (isEnumerable ? typeof(Enumerable) : (isNullable ? typeof(Nullable) : declaringType)));
			return _resolver.TryGetValue(type, out typeResolver);
		}
	}
}
