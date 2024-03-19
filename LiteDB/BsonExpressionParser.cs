using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LiteDB
{
	internal class BsonExpressionParser
	{
		private static readonly Dictionary<string, Tuple<string, MethodInfo, BsonExpressionType>> _operators = new Dictionary<string, Tuple<string, MethodInfo, BsonExpressionType>>
		{
			["%"] = Tuple.Create("%", M("MOD"), BsonExpressionType.Modulo),
			["/"] = Tuple.Create("/", M("DIVIDE"), BsonExpressionType.Divide),
			["*"] = Tuple.Create("*", M("MULTIPLY"), BsonExpressionType.Multiply),
			["+"] = Tuple.Create("+", M("ADD"), BsonExpressionType.Add),
			["-"] = Tuple.Create("-", M("MINUS"), BsonExpressionType.Subtract),
			["LIKE"] = Tuple.Create(" LIKE ", M("LIKE"), BsonExpressionType.Like),
			["BETWEEN"] = Tuple.Create(" BETWEEN ", M("BETWEEN"), BsonExpressionType.Between),
			["IN"] = Tuple.Create(" IN ", M("IN"), BsonExpressionType.In),
			[">"] = Tuple.Create(">", M("GT"), BsonExpressionType.GreaterThan),
			[">="] = Tuple.Create(">=", M("GTE"), BsonExpressionType.GreaterThanOrEqual),
			["<"] = Tuple.Create("<", M("LT"), BsonExpressionType.LessThan),
			["<="] = Tuple.Create("<=", M("LTE"), BsonExpressionType.LessThanOrEqual),
			["!="] = Tuple.Create("!=", M("NEQ"), BsonExpressionType.NotEqual),
			["="] = Tuple.Create("=", M("EQ"), BsonExpressionType.Equal),
			["ANY LIKE"] = Tuple.Create(" ANY LIKE ", M("LIKE_ANY"), BsonExpressionType.Like),
			["ANY BETWEEN"] = Tuple.Create(" ANY BETWEEN ", M("BETWEEN_ANY"), BsonExpressionType.Between),
			["ANY IN"] = Tuple.Create(" ANY IN ", M("IN_ANY"), BsonExpressionType.In),
			["ANY >"] = Tuple.Create(" ANY>", M("GT_ANY"), BsonExpressionType.GreaterThan),
			["ANY >="] = Tuple.Create(" ANY>=", M("GTE_ANY"), BsonExpressionType.GreaterThanOrEqual),
			["ANY <"] = Tuple.Create(" ANY<", M("LT_ANY"), BsonExpressionType.LessThan),
			["ANY <="] = Tuple.Create(" ANY<=", M("LTE_ANY"), BsonExpressionType.LessThanOrEqual),
			["ANY !="] = Tuple.Create(" ANY!=", M("NEQ_ANY"), BsonExpressionType.NotEqual),
			["ANY ="] = Tuple.Create(" ANY=", M("EQ_ANY"), BsonExpressionType.Equal),
			["ALL LIKE"] = Tuple.Create(" ALL LIKE ", M("LIKE_ALL"), BsonExpressionType.Like),
			["ALL BETWEEN"] = Tuple.Create(" ALL BETWEEN ", M("BETWEEN_ALL"), BsonExpressionType.Between),
			["ALL IN"] = Tuple.Create(" ALL IN ", M("IN_ALL"), BsonExpressionType.In),
			["ALL >"] = Tuple.Create(" ALL>", M("GT_ALL"), BsonExpressionType.GreaterThan),
			["ALL >="] = Tuple.Create(" ALL>=", M("GTE_ALL"), BsonExpressionType.GreaterThanOrEqual),
			["ALL <"] = Tuple.Create(" ALL<", M("LT_ALL"), BsonExpressionType.LessThan),
			["ALL <="] = Tuple.Create(" ALL<=", M("LTE_ALL"), BsonExpressionType.LessThanOrEqual),
			["ALL !="] = Tuple.Create(" ALL!=", M("NEQ_ALL"), BsonExpressionType.NotEqual),
			["ALL ="] = Tuple.Create(" ALL=", M("EQ_ALL"), BsonExpressionType.Equal),
			["AND"] = Tuple.Create<string, MethodInfo, BsonExpressionType>(" AND ", null, BsonExpressionType.And),
			["OR"] = Tuple.Create<string, MethodInfo, BsonExpressionType>(" OR ", null, BsonExpressionType.Or)
		};

		private static readonly MethodInfo _parameterPathMethod = M("PARAMETER_PATH");

		private static readonly MethodInfo _memberPathMethod = M("MEMBER_PATH");

		private static readonly MethodInfo _arrayIndexMethod = M("ARRAY_INDEX");

		private static readonly MethodInfo _arrayFilterMethod = M("ARRAY_FILTER");

		private static readonly MethodInfo _documentInitMethod = M("DOCUMENT_INIT");

		private static readonly MethodInfo _arrayInitMethod = M("ARRAY_INIT");

		private static readonly MethodInfo _itemsMethod = typeof(BsonExpressionMethods).GetMethod("ITEMS");

		private static readonly MethodInfo _arrayMethod = typeof(BsonExpressionMethods).GetMethod("ARRAY");

		private static MethodInfo M(string s)
		{
			return typeof(BsonExpressionOperators).GetMethod(s);
		}

		public static BsonExpression ParseFullExpression(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			BsonExpression first = ParseSingleExpression(tokenizer, context, parameters, scope);
			List<BsonExpression> values = new List<BsonExpression> { first };
			List<string> ops = new List<string>();
			while (!tokenizer.EOF)
			{
				string op2 = ReadOperant(tokenizer);
				if (op2 == null)
				{
					break;
				}
				BsonExpression expr = ParseSingleExpression(tokenizer, context, parameters, scope);
				if (op2.EndsWith("BETWEEN", StringComparison.OrdinalIgnoreCase))
				{
					tokenizer.ReadToken().Expect("AND");
					BsonExpression expr2 = ParseSingleExpression(tokenizer, context, parameters, scope);
					expr = NewArray(expr, expr2);
				}
				values.Add(expr);
				ops.Add(op2.ToUpperInvariant());
			}
			int order = 0;
			while (values.Count >= 2)
			{
				KeyValuePair<string, Tuple<string, MethodInfo, BsonExpressionType>> op = _operators.ElementAt(order);
				int i = ops.IndexOf(op.Key);
				if (i == -1)
				{
					order++;
					continue;
				}
				BsonExpression left = values.ElementAt(i);
				BsonExpression right = values.ElementAt(i + 1);
				string src = op.Value.Item1;
				MethodInfo method = op.Value.Item2;
				BsonExpressionType type = op.Value.Item3;
				bool num = op.Key.StartsWith("ALL") || op.Key.StartsWith("ANY");
				if (num && left.IsScalar)
				{
					left = ConvertToEnumerable(left);
				}
				if (!num && !left.IsScalar)
				{
					throw new LiteException(0, "Left expression `" + left.Source + "` returns more than one result. Try use ANY or ALL before operant.");
				}
				if (!num && !right.IsScalar)
				{
					throw new LiteException(0, "Left expression `" + right.Source + "` must return a single value");
				}
				if (!right.IsScalar)
				{
					throw new LiteException(0, "Right expression `" + right.Source + "` must return a single value");
				}
				BsonExpression result;
				if (type == BsonExpressionType.And || type == BsonExpressionType.Or)
				{
					result = CreateLogicExpression(type, left, right);
				}
				else
				{
					List<Expression> args = new List<Expression>();
					if (method?.GetParameters().FirstOrDefault()?.ParameterType == typeof(Collation))
					{
						args.Add(context.Collation);
					}
					args.Add(left.Expression);
					args.Add(right.Expression);
					result = new BsonExpression
					{
						Type = type,
						Parameters = parameters,
						IsImmutable = (left.IsImmutable && right.IsImmutable),
						UseSource = (left.UseSource || right.UseSource),
						IsScalar = true,
						Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(left.Fields).AddRange(right.Fields),
						Expression = Expression.Call(method, args.ToArray()),
						Left = left,
						Right = right,
						Source = left.Source + src + right.Source
					};
				}
				values.Insert(i, result);
				values.RemoveRange(i + 1, 2);
				ops.RemoveAt(i);
			}
			return values.Single();
		}

		public static BsonExpression ParseSingleExpression(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			Token token = tokenizer.ReadToken();
			return TryParseDouble(tokenizer, parameters) ?? TryParseInt(tokenizer, parameters) ?? TryParseBool(tokenizer, parameters) ?? TryParseNull(tokenizer, parameters) ?? TryParseString(tokenizer, parameters) ?? TryParseSource(tokenizer, context, parameters, scope) ?? TryParseDocument(tokenizer, context, parameters, scope) ?? TryParseArray(tokenizer, context, parameters, scope) ?? TryParseParameter(tokenizer, context, parameters, scope) ?? TryParseInnerExpression(tokenizer, context, parameters, scope) ?? TryParseFunction(tokenizer, context, parameters, scope) ?? TryParseMethodCall(tokenizer, context, parameters, scope) ?? TryParsePath(tokenizer, context, parameters, scope) ?? throw LiteException.UnexpectedToken(token);
		}

		public static BsonExpression ParseSelectDocumentBuilder(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters)
		{
			List<KeyValuePair<string, BsonExpression>> fields = new List<KeyValuePair<string, BsonExpression>>();
			HashSet<string> names = new HashSet<string>();
			int counter = 1;
			while (true)
			{
				BsonExpression expr2 = ParseFullExpression(tokenizer, context, parameters, DocumentScope.Root);
				Token next = tokenizer.LookAhead();
				if (stop(next))
				{
					Add(expr2.DefaultFieldName(), expr2);
					break;
				}
				if (next.Type == TokenType.Comma)
				{
					tokenizer.ReadToken();
					Add(expr2.DefaultFieldName(), expr2);
					continue;
				}
				if (next.Is("AS"))
				{
					tokenizer.ReadToken();
				}
				Add(tokenizer.ReadToken().Expect(TokenType.Word).Value, expr2);
				next = tokenizer.LookAhead();
				if (stop(next))
				{
					break;
				}
				tokenizer.ReadToken().Expect(TokenType.Comma);
			}
			BsonExpression first = fields[0].Value;
			if (fields.Count == 1)
			{
				if (first.Type == BsonExpressionType.Path && first.Source == "$")
				{
					return BsonExpression.Root;
				}
				if (fields.Count == 1 && first.Type == BsonExpressionType.Document)
				{
					return first;
				}
				if (fields.Count == 1 && first.Type == BsonExpressionType.Call && first.Source.StartsWith("EXTEND"))
				{
					return first;
				}
			}
			Type typeFromHandle = typeof(string);
			Expression[] initializers = fields.Select((KeyValuePair<string, BsonExpression> x) => Expression.Constant(x.Key)).ToArray();
			NewArrayExpression arrKeys = Expression.NewArrayInit(typeFromHandle, initializers);
			NewArrayExpression arrValues = Expression.NewArrayInit(typeof(BsonValue), fields.Select((KeyValuePair<string, BsonExpression> x) => x.Value.Expression).ToArray());
			BsonExpression bsonExpression = new BsonExpression();
			bsonExpression.Type = BsonExpressionType.Document;
			bsonExpression.Parameters = parameters;
			bsonExpression.IsImmutable = fields.All((KeyValuePair<string, BsonExpression> x) => x.Value.IsImmutable);
			bsonExpression.UseSource = fields.Any((KeyValuePair<string, BsonExpression> x) => x.Value.UseSource);
			bsonExpression.IsScalar = true;
			bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(fields.SelectMany((KeyValuePair<string, BsonExpression> x) => x.Value.Fields));
			bsonExpression.Expression = Expression.Call(_documentInitMethod, new Expression[2] { arrKeys, arrValues });
			bsonExpression.Source = "{" + string.Join(",", fields.Select((KeyValuePair<string, BsonExpression> x) => x.Key + ":" + x.Value.Source)) + "}";
			return bsonExpression;
			void Add(string alias, BsonExpression expr)
			{
				if (names.Contains(alias))
				{
					alias += counter++;
				}
				names.Add(alias);
				if (!expr.IsScalar)
				{
					expr = ConvertToArray(expr);
				}
				fields.Add(new KeyValuePair<string, BsonExpression>(alias, expr));
			}
			static bool stop(Token t)
			{
				if (!t.Is("FROM") && !t.Is("INTO") && t.Type != TokenType.EOF)
				{
					return t.Type == TokenType.SemiColon;
				}
				return true;
			}
		}

		public static BsonExpression ParseUpdateDocumentBuilder(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters)
		{
			if (tokenizer.LookAhead().Type == TokenType.OpenBrace)
			{
				tokenizer.ReadToken();
				return TryParseDocument(tokenizer, context, parameters, DocumentScope.Root);
			}
			List<Expression> keys = new List<Expression>();
			List<Expression> values = new List<Expression>();
			StringBuilder src = new StringBuilder();
			bool isImmutable = true;
			bool useSource = false;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			src.Append("{");
			while (!tokenizer.CheckEOF())
			{
				string key = ReadKey(tokenizer, src);
				tokenizer.ReadToken().Expect(TokenType.Equals);
				src.Append(":");
				BsonExpression value = ParseFullExpression(tokenizer, context, parameters, DocumentScope.Root);
				if (!value.IsScalar)
				{
					value = ConvertToArray(value);
				}
				if (!value.IsImmutable)
				{
					isImmutable = false;
				}
				if (value.UseSource)
				{
					useSource = true;
				}
				fields.AddRange(value.Fields);
				keys.Add(Expression.Constant(key));
				values.Add(value.Expression);
				src.Append(value.Source);
				if (tokenizer.LookAhead().Type != TokenType.Comma)
				{
					break;
				}
				src.Append(tokenizer.ReadToken().Value);
			}
			src.Append("}");
			NewArrayExpression arrKeys = Expression.NewArrayInit(typeof(string), keys.ToArray());
			NewArrayExpression arrValues = Expression.NewArrayInit(typeof(BsonValue), values.ToArray());
			MethodCallExpression docExpr = Expression.Call(_documentInitMethod, new Expression[2] { arrKeys, arrValues });
			return new BsonExpression
			{
				Type = BsonExpressionType.Document,
				Parameters = parameters,
				IsImmutable = isImmutable,
				UseSource = useSource,
				IsScalar = true,
				Fields = fields,
				Expression = docExpr,
				Source = src.ToString()
			};
		}

		private static BsonExpression TryParseDouble(Tokenizer tokenizer, BsonDocument parameters)
		{
			string value = null;
			if (tokenizer.Current.Type == TokenType.Double)
			{
				value = tokenizer.Current.Value;
			}
			else if (tokenizer.Current.Type == TokenType.Minus && tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Double)
			{
				value = "-" + tokenizer.ReadToken().Value;
			}
			if (value != null)
			{
				double number = Convert.ToDouble(value, CultureInfo.InvariantCulture.NumberFormat);
				ConstantExpression constant = Expression.Constant(new BsonValue(number));
				return new BsonExpression
				{
					Type = BsonExpressionType.Double,
					Parameters = parameters,
					IsImmutable = true,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = constant,
					Source = number.ToString("0.0########", CultureInfo.InvariantCulture.NumberFormat)
				};
			}
			return null;
		}

		private static BsonExpression TryParseInt(Tokenizer tokenizer, BsonDocument parameters)
		{
			string value = null;
			if (tokenizer.Current.Type == TokenType.Int)
			{
				value = tokenizer.Current.Value;
			}
			else if (tokenizer.Current.Type == TokenType.Minus && tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Int)
			{
				value = "-" + tokenizer.ReadToken().Value;
			}
			if (value != null)
			{
				if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var i32))
				{
					ConstantExpression constant32 = Expression.Constant(new BsonValue(i32));
					return new BsonExpression
					{
						Type = BsonExpressionType.Int,
						Parameters = parameters,
						IsImmutable = true,
						UseSource = false,
						IsScalar = true,
						Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
						Expression = constant32,
						Source = i32.ToString(CultureInfo.InvariantCulture.NumberFormat)
					};
				}
				long i33 = long.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				ConstantExpression constant33 = Expression.Constant(new BsonValue(i33));
				return new BsonExpression
				{
					Type = BsonExpressionType.Int,
					Parameters = parameters,
					IsImmutable = true,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = constant33,
					Source = i33.ToString(CultureInfo.InvariantCulture.NumberFormat)
				};
			}
			return null;
		}

		private static BsonExpression TryParseBool(Tokenizer tokenizer, BsonDocument parameters)
		{
			if (tokenizer.Current.Type == TokenType.Word && (tokenizer.Current.Is("true") || tokenizer.Current.Is("false")))
			{
				bool boolean = Convert.ToBoolean(tokenizer.Current.Value);
				ConstantExpression constant = Expression.Constant(new BsonValue(boolean));
				return new BsonExpression
				{
					Type = BsonExpressionType.Boolean,
					Parameters = parameters,
					IsImmutable = true,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = constant,
					Source = boolean.ToString().ToLower()
				};
			}
			return null;
		}

		private static BsonExpression TryParseNull(Tokenizer tokenizer, BsonDocument parameters)
		{
			if (tokenizer.Current.Type == TokenType.Word && tokenizer.Current.Is("null"))
			{
				ConstantExpression constant = Expression.Constant(BsonValue.Null);
				return new BsonExpression
				{
					Type = BsonExpressionType.Null,
					Parameters = parameters,
					IsImmutable = true,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = constant,
					Source = "null"
				};
			}
			return null;
		}

		private static BsonExpression TryParseString(Tokenizer tokenizer, BsonDocument parameters)
		{
			if (tokenizer.Current.Type == TokenType.String)
			{
				BsonValue bstr = new BsonValue(tokenizer.Current.Value);
				ConstantExpression constant = Expression.Constant(bstr);
				return new BsonExpression
				{
					Type = BsonExpressionType.String,
					Parameters = parameters,
					IsImmutable = true,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = constant,
					Source = JsonSerializer.Serialize(bstr)
				};
			}
			return null;
		}

		private static BsonExpression TryParseDocument(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != 0)
			{
				return null;
			}
			List<Expression> keys = new List<Expression>();
			List<Expression> values = new List<Expression>();
			StringBuilder src = new StringBuilder();
			bool isImmutable = true;
			bool useSource = false;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			src.Append("{");
			BsonExpression bsonExpression;
			if (tokenizer.LookAhead().Type == TokenType.CloseBrace)
			{
				src.Append(tokenizer.ReadToken().Value);
			}
			else
			{
				while (!tokenizer.CheckEOF())
				{
					StringBuilder innerSrc = new StringBuilder();
					string key = ReadKey(tokenizer, innerSrc);
					src.Append(innerSrc);
					tokenizer.ReadToken();
					src.Append(":");
					BsonExpression value;
					if (tokenizer.Current.Type == TokenType.Colon)
					{
						value = ParseFullExpression(tokenizer, context, parameters, scope);
						tokenizer.ReadToken();
					}
					else
					{
						string fname = innerSrc.ToString();
						bsonExpression = new BsonExpression();
						bsonExpression.Type = BsonExpressionType.Path;
						bsonExpression.Parameters = parameters;
						bsonExpression.IsImmutable = isImmutable;
						bsonExpression.UseSource = useSource;
						bsonExpression.IsScalar = true;
						bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(new string[1] { key });
						bsonExpression.Expression = Expression.Call(_memberPathMethod, context.Root, Expression.Constant(key));
						bsonExpression.Source = "$." + (fname.IsWord() ? fname : ("[" + fname + "]"));
						value = bsonExpression;
					}
					if (!value.IsScalar)
					{
						value = ConvertToArray(value);
					}
					if (!value.IsImmutable)
					{
						isImmutable = false;
					}
					if (value.UseSource)
					{
						useSource = true;
					}
					fields.AddRange(value.Fields);
					keys.Add(Expression.Constant(key));
					values.Add(value.Expression);
					src.Append(value.Source);
					tokenizer.Current.Expect(TokenType.Comma, TokenType.CloseBrace);
					src.Append(tokenizer.Current.Value);
					if (tokenizer.Current.Type != TokenType.Comma)
					{
						break;
					}
				}
			}
			NewArrayExpression arrKeys = Expression.NewArrayInit(typeof(string), keys.ToArray());
			NewArrayExpression arrValues = Expression.NewArrayInit(typeof(BsonValue), values.ToArray());
			bsonExpression = new BsonExpression();
			bsonExpression.Type = BsonExpressionType.Document;
			bsonExpression.Parameters = parameters;
			bsonExpression.IsImmutable = isImmutable;
			bsonExpression.UseSource = useSource;
			bsonExpression.IsScalar = true;
			bsonExpression.Fields = fields;
			bsonExpression.Expression = Expression.Call(_documentInitMethod, new Expression[2] { arrKeys, arrValues });
			bsonExpression.Source = src.ToString();
			return bsonExpression;
		}

		private static BsonExpression TryParseSource(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.Asterisk)
			{
				return null;
			}
			BsonExpression sourceExpr = new BsonExpression
			{
				Type = BsonExpressionType.Source,
				Parameters = parameters,
				IsImmutable = true,
				UseSource = true,
				IsScalar = false,
				Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "$" },
				Expression = context.Source,
				Source = "*"
			};
			if (tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Period)
			{
				tokenizer.ReadToken();
				BsonExpression pathExpr = BsonExpression.ParseAndCompile(tokenizer, BsonExpressionParserMode.Single, parameters, DocumentScope.Source);
				if (pathExpr == null)
				{
					throw LiteException.UnexpectedToken(tokenizer.Current);
				}
				return new BsonExpression
				{
					Type = BsonExpressionType.Map,
					Parameters = parameters,
					IsImmutable = pathExpr.IsImmutable,
					UseSource = true,
					IsScalar = false,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(pathExpr.Fields),
					Expression = Expression.Call(BsonExpression.GetFunction("MAP"), context.Root, context.Collation, context.Parameters, sourceExpr.Expression, Expression.Constant(pathExpr)),
					Source = "MAP(*=>" + pathExpr.Source + ")"
				};
			}
			return sourceExpr;
		}

		private static BsonExpression TryParseArray(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.OpenBracket)
			{
				return null;
			}
			List<Expression> values = new List<Expression>();
			StringBuilder src = new StringBuilder();
			bool isImmutable = true;
			bool useSource = false;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			src.Append("[");
			if (tokenizer.LookAhead().Type == TokenType.CloseBracket)
			{
				src.Append(tokenizer.ReadToken().Value);
			}
			else
			{
				while (!tokenizer.CheckEOF())
				{
					BsonExpression value = ParseFullExpression(tokenizer, context, parameters, scope);
					if (!value.IsScalar)
					{
						value = ConvertToArray(value);
					}
					src.Append(value.Source);
					if (!value.IsImmutable)
					{
						isImmutable = false;
					}
					if (value.UseSource)
					{
						useSource = true;
					}
					fields.AddRange(value.Fields);
					values.Add(value.Expression);
					Token next = tokenizer.ReadToken().Expect(TokenType.Comma, TokenType.CloseBracket);
					src.Append(next.Value);
					if (next.Type != TokenType.Comma)
					{
						break;
					}
				}
			}
			NewArrayExpression arrValues = Expression.NewArrayInit(typeof(BsonValue), values.ToArray());
			return new BsonExpression
			{
				Type = BsonExpressionType.Array,
				Parameters = parameters,
				IsImmutable = isImmutable,
				UseSource = useSource,
				IsScalar = true,
				Fields = fields,
				Expression = Expression.Call(_arrayInitMethod, arrValues),
				Source = src.ToString()
			};
		}

		private static BsonExpression TryParseParameter(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.At)
			{
				return null;
			}
			Token ahead = tokenizer.LookAhead(eatWhitespace: false);
			if (ahead.Type == TokenType.Word || ahead.Type == TokenType.Int)
			{
				string parameterName = tokenizer.ReadToken(eatWhitespace: false).Value;
				ConstantExpression name = Expression.Constant(parameterName);
				return new BsonExpression
				{
					Type = BsonExpressionType.Parameter,
					Parameters = parameters,
					IsImmutable = false,
					UseSource = false,
					IsScalar = true,
					Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
					Expression = Expression.Call(_parameterPathMethod, context.Parameters, name),
					Source = "@" + parameterName
				};
			}
			return null;
		}

		private static BsonExpression TryParseInnerExpression(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.OpenParenthesis)
			{
				return null;
			}
			BsonExpression inner = ParseFullExpression(tokenizer, context, parameters, scope);
			tokenizer.ReadToken().Expect(TokenType.CloseParenthesis);
			return new BsonExpression
			{
				Type = inner.Type,
				Parameters = inner.Parameters,
				IsImmutable = inner.IsImmutable,
				UseSource = inner.UseSource,
				IsScalar = inner.IsScalar,
				Fields = inner.Fields,
				Expression = inner.Expression,
				Left = inner.Left,
				Right = inner.Right,
				Source = "(" + inner.Source + ")"
			};
		}

		private static BsonExpression TryParseMethodCall(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			Token token = tokenizer.Current;
			if (tokenizer.Current.Type != TokenType.Word)
			{
				return null;
			}
			if (tokenizer.LookAhead().Type != TokenType.OpenParenthesis)
			{
				return null;
			}
			tokenizer.ReadToken();
			List<BsonExpression> pars = new List<BsonExpression>();
			StringBuilder src = new StringBuilder();
			bool isImmutable = true;
			bool useSource = false;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			src.Append(token.Value.ToUpperInvariant() + "(");
			if (tokenizer.LookAhead().Type == TokenType.CloseParenthesis)
			{
				src.Append(tokenizer.ReadToken().Value);
			}
			else
			{
				while (!tokenizer.CheckEOF())
				{
					BsonExpression parameter2 = ParseFullExpression(tokenizer, context, parameters, scope);
					if (!parameter2.IsImmutable)
					{
						isImmutable = false;
					}
					if (parameter2.UseSource)
					{
						useSource = true;
					}
					fields.AddRange(parameter2.Fields);
					pars.Add(parameter2);
					src.Append(parameter2.Source);
					Token next = tokenizer.ReadToken().Expect(TokenType.Comma, TokenType.CloseParenthesis);
					src.Append(next.Value);
					if (next.Type != TokenType.Comma)
					{
						break;
					}
				}
			}
			MethodInfo method = BsonExpression.GetMethod(token.Value, pars.Count);
			if (method == null)
			{
				throw LiteException.UnexpectedToken("Method '" + token.Value.ToUpperInvariant() + "' does not exist or contains invalid parameters", token);
			}
			if (method.GetCustomAttribute<VolatileAttribute>() != null)
			{
				isImmutable = false;
			}
			List<Expression> args = new List<Expression>();
			if (method.GetParameters().FirstOrDefault()?.ParameterType == typeof(Collation))
			{
				args.Add(context.Collation);
			}
			foreach (var item in (from x in method.GetParameters()
				where x.ParameterType != typeof(Collation)
				select x).Zip(pars, (ParameterInfo parameter, BsonExpression expr) => new { parameter, expr }))
			{
				if (!item.parameter.ParameterType.IsEnumerable() && !item.expr.IsScalar)
				{
					args.Add(ConvertToArray(item.expr).Expression);
				}
				else if (item.parameter.ParameterType.IsEnumerable() && item.expr.IsScalar)
				{
					args.Add(ConvertToEnumerable(item.expr).Expression);
				}
				else
				{
					args.Add(item.expr.Expression);
				}
			}
			if (method.Name == "IIF" && pars.Count == 3)
			{
				return CreateConditionalExpression(pars[0], pars[1], pars[2]);
			}
			return new BsonExpression
			{
				Type = BsonExpressionType.Call,
				Parameters = parameters,
				IsImmutable = isImmutable,
				UseSource = useSource,
				IsScalar = !method.ReturnType.IsEnumerable(),
				Fields = fields,
				Expression = Expression.Call(method, args.ToArray()),
				Source = src.ToString()
			};
		}

		private static BsonExpression TryParsePath(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.At && tokenizer.Current.Type != TokenType.Dollar && tokenizer.Current.Type != TokenType.Word)
			{
				return null;
			}
			TokenType defaultScope = ((scope == DocumentScope.Root) ? TokenType.Dollar : TokenType.At);
			if (tokenizer.Current.Type == TokenType.At || tokenizer.Current.Type == TokenType.Dollar)
			{
				defaultScope = tokenizer.Current.Type;
				if (tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Period)
				{
					tokenizer.ReadToken();
					tokenizer.ReadToken();
				}
			}
			StringBuilder src = new StringBuilder();
			bool isImmutable = true;
			bool useSource = false;
			bool isScalar = true;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			src.Append((defaultScope == TokenType.Dollar) ? "$" : "@");
			string field = ReadField(tokenizer, src);
			ConstantExpression name = Expression.Constant(field);
			Expression expr = Expression.Call(_memberPathMethod, (defaultScope == TokenType.Dollar) ? context.Root : context.Current, name);
			if (defaultScope == TokenType.Dollar || scope == DocumentScope.Source)
			{
				fields.Add((field.Length == 0) ? "$" : field);
			}
			while (!tokenizer.EOF)
			{
				Expression result = ParsePath(tokenizer, expr, context, parameters, fields, ref isImmutable, ref useSource, ref isScalar, src);
				if (!isScalar)
				{
					expr = result;
					break;
				}
				if (result == null)
				{
					break;
				}
				expr = result;
			}
			BsonExpression pathExpr = new BsonExpression
			{
				Type = BsonExpressionType.Path,
				Parameters = parameters,
				IsImmutable = isImmutable,
				UseSource = useSource,
				IsScalar = isScalar,
				Fields = fields,
				Expression = expr,
				Source = src.ToString()
			};
			if (!isScalar && tokenizer.LookAhead(eatWhitespace: false).Type == TokenType.Period)
			{
				tokenizer.ReadToken();
				BsonExpression mapExpr = BsonExpression.ParseAndCompile(tokenizer, BsonExpressionParserMode.Single, parameters, DocumentScope.Current);
				if (mapExpr == null)
				{
					throw LiteException.UnexpectedToken(tokenizer.Current);
				}
				BsonExpression bsonExpression = new BsonExpression();
				bsonExpression.Type = BsonExpressionType.Map;
				bsonExpression.Parameters = parameters;
				bsonExpression.IsImmutable = pathExpr.IsImmutable && mapExpr.IsImmutable;
				bsonExpression.UseSource = pathExpr.UseSource || mapExpr.UseSource;
				bsonExpression.IsScalar = false;
				bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(pathExpr.Fields).AddRange(mapExpr.Fields);
				bsonExpression.Expression = Expression.Call(BsonExpression.GetFunction("MAP"), context.Root, context.Collation, context.Parameters, pathExpr.Expression, Expression.Constant(mapExpr));
				bsonExpression.Source = "MAP(" + pathExpr.Source + "=>" + mapExpr.Source + ")";
				return bsonExpression;
			}
			return pathExpr;
		}

		private static Expression ParsePath(Tokenizer tokenizer, Expression expr, ExpressionContext context, BsonDocument parameters, HashSet<string> fields, ref bool isImmutable, ref bool useSource, ref bool isScalar, StringBuilder src)
		{
			Token ahead = tokenizer.LookAhead(eatWhitespace: false);
			if (ahead.Type == TokenType.Period)
			{
				tokenizer.ReadToken();
				tokenizer.ReadToken(eatWhitespace: false);
				ConstantExpression name = Expression.Constant(ReadField(tokenizer, src));
				return Expression.Call(_memberPathMethod, expr, name);
			}
			if (ahead.Type == TokenType.OpenBracket)
			{
				src.Append("[");
				tokenizer.ReadToken();
				ahead = tokenizer.LookAhead();
				int index = 0;
				BsonExpression inner = new BsonExpression();
				MethodInfo method = _arrayIndexMethod;
				if (ahead.Type == TokenType.Int)
				{
					src.Append(tokenizer.ReadToken().Value);
					index = Convert.ToInt32(tokenizer.Current.Value);
				}
				else if (ahead.Type == TokenType.Minus)
				{
					src.Append(tokenizer.ReadToken().Value + tokenizer.ReadToken().Expect(TokenType.Int).Value);
					index = -Convert.ToInt32(tokenizer.Current.Value);
				}
				else if (ahead.Type == TokenType.Asterisk)
				{
					method = _arrayFilterMethod;
					isScalar = false;
					index = int.MaxValue;
					src.Append(tokenizer.ReadToken().Value);
				}
				else
				{
					inner = BsonExpression.ParseAndCompile(tokenizer, BsonExpressionParserMode.Full, parameters, DocumentScope.Current);
					if (inner == null)
					{
						throw LiteException.UnexpectedToken(tokenizer.Current);
					}
					if (!inner.IsImmutable)
					{
						isImmutable = false;
					}
					if (inner.UseSource)
					{
						useSource = true;
					}
					if (inner.Type != BsonExpressionType.Parameter)
					{
						method = _arrayFilterMethod;
						isScalar = false;
					}
					fields.AddRange(inner.Fields);
					src.Append(inner.Source);
				}
				tokenizer.ReadToken().Expect(TokenType.CloseBracket);
				src.Append("]");
				return Expression.Call(method, expr, Expression.Constant(index), Expression.Constant(inner), context.Root, context.Collation, context.Parameters);
			}
			return null;
		}

		private static BsonExpression TryParseFunction(Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.Current.Type != TokenType.Word)
			{
				return null;
			}
			if (tokenizer.LookAhead().Type != TokenType.OpenParenthesis)
			{
				return null;
			}
			string token = tokenizer.Current.Value.ToUpperInvariant();
			return token switch
			{
				"MAP" => ParseFunction(token, BsonExpressionType.Map, tokenizer, context, parameters, scope), 
				"FILTER" => ParseFunction(token, BsonExpressionType.Filter, tokenizer, context, parameters, scope), 
				"SORT" => ParseFunction(token, BsonExpressionType.Sort, tokenizer, context, parameters, scope), 
				_ => null, 
			};
		}

		private static BsonExpression ParseFunction(string functionName, BsonExpressionType type, Tokenizer tokenizer, ExpressionContext context, BsonDocument parameters, DocumentScope scope)
		{
			if (tokenizer.LookAhead().Type != TokenType.OpenParenthesis)
			{
				return null;
			}
			tokenizer.ReadToken().Expect(TokenType.OpenParenthesis);
			BsonExpression left = ParseSingleExpression(tokenizer, context, parameters, scope);
			if (left.IsScalar)
			{
				left = ConvertToEnumerable(left);
			}
			List<Expression> args = new List<Expression>();
			args.Add(context.Root);
			args.Add(context.Collation);
			args.Add(context.Parameters);
			StringBuilder src = new StringBuilder(functionName + "(" + left.Source);
			bool isImmutable = left.IsImmutable;
			bool useSource = left.UseSource;
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			args.Add(left.Expression);
			fields.AddRange(left.Fields);
			if (tokenizer.LookAhead().Type == TokenType.Equals)
			{
				tokenizer.ReadToken().Expect(TokenType.Equals);
				tokenizer.ReadToken().Expect(TokenType.Greater);
				BsonExpression right = BsonExpression.ParseAndCompile(tokenizer, BsonExpressionParserMode.Full, parameters, (left.Type != BsonExpressionType.Source) ? DocumentScope.Current : DocumentScope.Source);
				src.Append("=>" + right.Source);
				args.Add(Expression.Constant(right));
				fields.AddRange(right.Fields);
			}
			if (tokenizer.LookAhead().Type != TokenType.CloseParenthesis)
			{
				tokenizer.ReadToken().Expect(TokenType.Comma);
				src.Append(",");
				while (!tokenizer.CheckEOF())
				{
					BsonExpression parameter = ParseFullExpression(tokenizer, context, parameters, scope);
					if (!parameter.IsImmutable)
					{
						isImmutable = false;
					}
					if (parameter.UseSource)
					{
						useSource = true;
					}
					args.Add(parameter.Expression);
					src.Append(parameter.Source);
					fields.AddRange(parameter.Fields);
					if (tokenizer.LookAhead().Type != TokenType.Comma)
					{
						break;
					}
					src.Append(tokenizer.ReadToken().Value);
				}
			}
			tokenizer.ReadToken().Expect(TokenType.CloseParenthesis);
			src.Append(")");
			MethodInfo method = BsonExpression.GetFunction(functionName, args.Count - 5);
			return new BsonExpression
			{
				Type = type,
				Parameters = parameters,
				IsImmutable = isImmutable,
				UseSource = useSource,
				IsScalar = false,
				Fields = fields,
				Expression = Expression.Call(method, args.ToArray()),
				Source = src.ToString()
			};
		}

		private static BsonExpression NewArray(BsonExpression item0, BsonExpression item1)
		{
			Expression[] values = new Expression[2] { item0.Expression, item1.Expression };
			if (!item0.IsScalar)
			{
				throw new LiteException(0, "Expression `" + item0.Source + "` must be a scalar expression");
			}
			if (!item1.IsScalar)
			{
				throw new LiteException(0, "Expression `" + item0.Source + "` must be a scalar expression");
			}
			NewArrayExpression arrValues = Expression.NewArrayInit(typeof(BsonValue), values.ToArray());
			BsonExpression bsonExpression = new BsonExpression();
			bsonExpression.Type = BsonExpressionType.Array;
			bsonExpression.Parameters = item0.Parameters;
			bsonExpression.IsImmutable = item0.IsImmutable && item1.IsImmutable;
			bsonExpression.UseSource = item0.UseSource || item1.UseSource;
			bsonExpression.IsScalar = true;
			bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(item0.Fields).AddRange(item1.Fields);
			bsonExpression.Expression = Expression.Call(_arrayInitMethod, new Expression[1] { arrValues });
			bsonExpression.Source = item0.Source + " AND " + item1.Source;
			return bsonExpression;
		}

		private static string ReadField(Tokenizer tokenizer, StringBuilder source)
		{
			string field = "";
			if (tokenizer.Current.Type == TokenType.OpenBracket)
			{
				field = tokenizer.ReadToken().Expect(TokenType.String).Value;
				tokenizer.ReadToken().Expect(TokenType.CloseBracket);
			}
			else if (tokenizer.Current.Type == TokenType.Word)
			{
				field = tokenizer.Current.Value;
			}
			if (field.Length > 0)
			{
				source.Append(".");
				if (field.IsWord())
				{
					source.Append(field);
				}
				else
				{
					source.Append("[");
					JsonSerializer.Serialize(field, source);
					source.Append("]");
				}
			}
			return field;
		}

		public static string ReadKey(Tokenizer tokenizer, StringBuilder source)
		{
			Token token = tokenizer.ReadToken();
			string key = "";
			key = ((token.Type != TokenType.String) ? token.Expect(TokenType.Word, TokenType.Int).Value : token.Value);
			if (key.IsWord())
			{
				source.Append(key);
			}
			else
			{
				JsonSerializer.Serialize(key, source);
			}
			return key;
		}

		private static string ReadOperant(Tokenizer tokenizer)
		{
			Token token = tokenizer.LookAhead();
			if (token.IsOperand)
			{
				tokenizer.ReadToken();
				return token.Value;
			}
			if (token.Is("ALL") || token.Is("ANY"))
			{
				string text = token.Value.ToUpperInvariant();
				tokenizer.ReadToken();
				token = tokenizer.ReadToken();
				if (!token.IsOperand)
				{
					throw LiteException.UnexpectedToken("Expected valid operand", token);
				}
				return text + " " + token.Value;
			}
			return null;
		}

		private static BsonExpression ConvertToEnumerable(BsonExpression expr)
		{
			string src = ((expr.Type == BsonExpressionType.Path) ? (expr.Source + "[*]") : ("ITEMS(" + expr.Source + ")"));
			BsonExpressionType exprType = ((expr.Type == BsonExpressionType.Path) ? BsonExpressionType.Path : BsonExpressionType.Call);
			return new BsonExpression
			{
				Type = exprType,
				Parameters = expr.Parameters,
				IsImmutable = expr.IsImmutable,
				UseSource = expr.UseSource,
				IsScalar = false,
				Fields = expr.Fields,
				Expression = Expression.Call(_itemsMethod, expr.Expression),
				Source = src
			};
		}

		private static BsonExpression ConvertToArray(BsonExpression expr)
		{
			return new BsonExpression
			{
				Type = BsonExpressionType.Call,
				Parameters = expr.Parameters,
				IsImmutable = expr.IsImmutable,
				UseSource = expr.UseSource,
				IsScalar = true,
				Fields = expr.Fields,
				Expression = Expression.Call(_arrayMethod, expr.Expression),
				Source = "ARRAY(" + expr.Source + ")"
			};
		}

		internal static BsonExpression CreateLogicExpression(BsonExpressionType type, BsonExpression left, BsonExpression right)
		{
			MemberExpression boolLeft = Expression.Property(left.Expression, typeof(BsonValue), "AsBoolean");
			MemberExpression boolRight = Expression.Property(right.Expression, typeof(BsonValue), "AsBoolean");
			BinaryExpression expr = ((type == BsonExpressionType.And) ? Expression.AndAlso(boolLeft, boolRight) : Expression.OrElse(boolLeft, boolRight));
			ConstructorInfo ctor = typeof(BsonValue).GetConstructors().First((ConstructorInfo x) => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(bool));
			BsonExpression bsonExpression = new BsonExpression();
			bsonExpression.Type = type;
			bsonExpression.Parameters = left.Parameters;
			bsonExpression.IsImmutable = left.IsImmutable && right.IsImmutable;
			bsonExpression.UseSource = left.UseSource || right.UseSource;
			bsonExpression.IsScalar = left.IsScalar && right.IsScalar;
			bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(left.Fields).AddRange(right.Fields);
			bsonExpression.Expression = Expression.New(ctor, expr);
			bsonExpression.Left = left;
			bsonExpression.Right = right;
			bsonExpression.Source = left.Source + " " + type.ToString().ToUpperInvariant() + " " + right.Source;
			return bsonExpression;
		}

		internal static BsonExpression CreateConditionalExpression(BsonExpression test, BsonExpression ifTrue, BsonExpression ifFalse)
		{
			ConditionalExpression expr = Expression.Condition(Expression.Property(test.Expression, typeof(BsonValue), "AsBoolean"), ifTrue.Expression, ifFalse.Expression);
			BsonExpression bsonExpression = new BsonExpression();
			bsonExpression.Type = BsonExpressionType.Call;
			bsonExpression.Parameters = test.Parameters;
			bsonExpression.IsImmutable = (test.IsImmutable && ifTrue.IsImmutable) || ifFalse.IsImmutable;
			bsonExpression.UseSource = test.UseSource || ifTrue.UseSource || ifFalse.UseSource;
			bsonExpression.IsScalar = test.IsScalar && ifTrue.IsScalar && ifFalse.IsScalar;
			bsonExpression.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase).AddRange(test.Fields).AddRange(ifTrue.Fields).AddRange(ifFalse.Fields);
			bsonExpression.Expression = expr;
			bsonExpression.Source = "IIF(" + test.Source + "," + ifTrue.Source + "," + ifFalse.Source + ")";
			return bsonExpression;
		}
	}
}
