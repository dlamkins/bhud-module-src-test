using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LiteDB
{
	public sealed class BsonExpression
	{
		private BsonExpressionEnumerableDelegate _funcEnumerable;

		private BsonExpressionScalarDelegate _funcScalar;

		private static readonly ConcurrentDictionary<string, BsonExpressionEnumerableDelegate> _cacheEnumerable = new ConcurrentDictionary<string, BsonExpressionEnumerableDelegate>();

		private static readonly ConcurrentDictionary<string, BsonExpressionScalarDelegate> _cacheScalar = new ConcurrentDictionary<string, BsonExpressionScalarDelegate>();

		public static BsonExpression Root = Create("$");

		private static readonly Dictionary<string, MethodInfo> _methods = typeof(BsonExpressionMethods).GetMethods(BindingFlags.Static | BindingFlags.Public).ToDictionary((MethodInfo m) => m.Name.ToUpperInvariant() + "~" + (from p in m.GetParameters()
			where p.ParameterType != typeof(Collation)
			select p).Count());

		private static readonly Dictionary<string, MethodInfo> _functions = typeof(BsonExpressionFunctions).GetMethods(BindingFlags.Static | BindingFlags.Public).ToDictionary((MethodInfo m) => m.Name.ToUpperInvariant() + "~" + m.GetParameters().Skip(5).Count());

		public string Source { get; internal set; }

		public BsonExpressionType Type { get; internal set; }

		public bool IsImmutable { get; internal set; }

		public BsonDocument Parameters { get; internal set; }

		internal BsonExpression Left { get; set; }

		internal BsonExpression Right { get; set; }

		internal bool UseSource { get; set; }

		internal Expression Expression { get; set; }

		public HashSet<string> Fields { get; internal set; }

		public bool IsScalar { get; internal set; }

		internal bool IsPredicate
		{
			get
			{
				if (Type != BsonExpressionType.Equal && Type != BsonExpressionType.Like && Type != BsonExpressionType.Between && Type != BsonExpressionType.GreaterThan && Type != BsonExpressionType.GreaterThanOrEqual && Type != BsonExpressionType.LessThan && Type != BsonExpressionType.LessThanOrEqual && Type != BsonExpressionType.NotEqual)
				{
					return Type == BsonExpressionType.In;
				}
				return true;
			}
		}

		internal bool IsIndexable
		{
			get
			{
				if (Fields.Count > 0 && IsImmutable)
				{
					return Parameters.Count == 0;
				}
				return false;
			}
		}

		internal bool IsValue => Fields.Count == 0;

		internal bool IsANY
		{
			get
			{
				if (IsPredicate)
				{
					return Expression.ToString().Contains("_ANY");
				}
				return false;
			}
		}

		public static IEnumerable<MethodInfo> Methods => _methods.Values;

		public static IEnumerable<MethodInfo> Functions => _functions.Values;

		internal string DefaultFieldName()
		{
			string name = string.Join("_", Fields.Where((string x) => x != "$"));
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			return "expr";
		}

		internal BsonExpression()
		{
		}

		public static implicit operator string(BsonExpression expr)
		{
			return expr.Source;
		}

		public static implicit operator BsonExpression(string expr)
		{
			return Create(expr);
		}

		public IEnumerable<BsonValue> Execute(Collation collation = null)
		{
			BsonDocument root = new BsonDocument();
			BsonDocument[] source = new BsonDocument[1] { root };
			return Execute(source, root, root, collation);
		}

		public IEnumerable<BsonValue> Execute(BsonDocument root, Collation collation = null)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			BsonDocument[] source = new BsonDocument[1] { root };
			return Execute(source, root, root, collation);
		}

		public IEnumerable<BsonValue> Execute(IEnumerable<BsonDocument> source, Collation collation = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Execute(source, null, null, collation);
		}

		internal IEnumerable<BsonValue> Execute(IEnumerable<BsonDocument> source, BsonDocument root, BsonValue current, Collation collation)
		{
			if (IsScalar)
			{
				yield return _funcScalar(source, root, current, collation ?? Collation.Binary, Parameters);
				yield break;
			}
			IEnumerable<BsonValue> values = _funcEnumerable(source, root, current, collation ?? Collation.Binary, Parameters);
			foreach (BsonValue item in values)
			{
				yield return item;
			}
		}

		internal IEnumerable<BsonValue> GetIndexKeys(BsonDocument doc, Collation collation)
		{
			return Execute(doc, collation).Distinct();
		}

		public BsonValue ExecuteScalar(Collation collation = null)
		{
			BsonDocument root = new BsonDocument();
			BsonDocument[] source = new BsonDocument[0];
			return ExecuteScalar(source, root, root, collation);
		}

		public BsonValue ExecuteScalar(BsonDocument root, Collation collation = null)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			BsonDocument[] source = new BsonDocument[1] { root };
			return ExecuteScalar(source, root, root, collation);
		}

		public BsonValue ExecuteScalar(IEnumerable<BsonDocument> source, Collation collation = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return ExecuteScalar(source, null, null, collation);
		}

		internal BsonValue ExecuteScalar(IEnumerable<BsonDocument> source, BsonDocument root, BsonValue current, Collation collation)
		{
			if (IsScalar)
			{
				return _funcScalar(source, root, current, collation ?? Collation.Binary, Parameters);
			}
			throw new LiteException(0, "Expression `" + Source + "` is not a scalar expression and can return more than one result");
		}

		public static BsonExpression Create(string expression)
		{
			return Create(expression, new BsonDocument());
		}

		public static BsonExpression Create(string expression, params BsonValue[] args)
		{
			BsonDocument parameters = new BsonDocument();
			for (int i = 0; i < args.Length; i++)
			{
				parameters[i.ToString()] = args[i];
			}
			return Create(expression, parameters);
		}

		public static BsonExpression Create(string expression, BsonDocument parameters)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw new ArgumentNullException("expression");
			}
			Tokenizer tokenizer = new Tokenizer(expression);
			BsonExpression expr = Create(tokenizer, BsonExpressionParserMode.Full, parameters);
			tokenizer.LookAhead().Expect(TokenType.EOF);
			return expr;
		}

		internal static BsonExpression Create(Tokenizer tokenizer, BsonExpressionParserMode mode, BsonDocument parameters)
		{
			if (tokenizer == null)
			{
				throw new ArgumentNullException("tokenizer");
			}
			return ParseAndCompile(tokenizer, mode, parameters, DocumentScope.Root);
		}

		internal static BsonExpression ParseAndCompile(Tokenizer tokenizer, BsonExpressionParserMode mode, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer == null)
			{
				throw new ArgumentNullException("tokenizer");
			}
			ExpressionContext context = new ExpressionContext();
			object obj = mode switch
			{
				BsonExpressionParserMode.SelectDocument => BsonExpressionParser.ParseSelectDocumentBuilder(tokenizer, context, parameters), 
				BsonExpressionParserMode.Single => BsonExpressionParser.ParseSingleExpression(tokenizer, context, parameters, scope), 
				BsonExpressionParserMode.Full => BsonExpressionParser.ParseFullExpression(tokenizer, context, parameters, scope), 
				_ => BsonExpressionParser.ParseUpdateDocumentBuilder(tokenizer, context, parameters), 
			};
			Compile((BsonExpression)obj, context);
			return (BsonExpression)obj;
		}

		internal static void Compile(BsonExpression expr, ExpressionContext context)
		{
			if (expr.IsScalar)
			{
				BsonExpressionScalarDelegate cached2 = _cacheScalar.GetOrAdd(expr.Source, (string s) => Expression.Lambda<BsonExpressionScalarDelegate>(expr.Expression, new ParameterExpression[5] { context.Source, context.Root, context.Current, context.Collation, context.Parameters }).Compile());
				expr._funcScalar = cached2;
			}
			else
			{
				BsonExpressionEnumerableDelegate cached = _cacheEnumerable.GetOrAdd(expr.Source, (string s) => Expression.Lambda<BsonExpressionEnumerableDelegate>(expr.Expression, new ParameterExpression[5] { context.Source, context.Root, context.Current, context.Collation, context.Parameters }).Compile());
				expr._funcEnumerable = cached;
			}
			if (expr.Left != null)
			{
				Compile(expr.Left, context);
			}
			if (expr.Right != null)
			{
				Compile(expr.Right, context);
			}
		}

		internal static void SetParameters(BsonExpression expr, BsonDocument parameters)
		{
			expr.Parameters = parameters;
			if (expr.Left != null)
			{
				SetParameters(expr.Left, parameters);
			}
			if (expr.Right != null)
			{
				SetParameters(expr.Right, parameters);
			}
		}

		internal static MethodInfo GetMethod(string name, int parameterCount)
		{
			string key = name.ToUpperInvariant() + "~" + parameterCount;
			return _methods.GetOrDefault(key);
		}

		internal static MethodInfo GetFunction(string name, int parameterCount = 0)
		{
			string key = name.ToUpperInvariant() + "~" + parameterCount;
			return _functions.GetOrDefault(key);
		}

		public override string ToString()
		{
			return $"`{Source}` [{Type}]";
		}
	}
}
