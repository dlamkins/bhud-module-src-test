using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB.Engine;

namespace LiteDB
{
	internal class SqlParser
	{
		private readonly ILiteEngine _engine;

		private readonly Tokenizer _tokenizer;

		private readonly BsonDocument _parameters;

		private readonly Lazy<Collation> _collation;

		private BsonDataReader ParseBegin()
		{
			_tokenizer.ReadToken().Expect("BEGIN");
			Token token = _tokenizer.ReadToken().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon);
			if (token.Is("TRANS") || token.Is("TRANSACTION"))
			{
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			}
			return new BsonDataReader(_engine.BeginTrans());
		}

		private BsonDataReader ParseCheckpoint()
		{
			_tokenizer.ReadToken().Expect("CHECKPOINT");
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			return new BsonDataReader(_engine.Checkpoint());
		}

		private BsonDataReader ParseCommit()
		{
			_tokenizer.ReadToken().Expect("COMMIT");
			Token token = _tokenizer.ReadToken().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon);
			if (token.Is("TRANS") || token.Is("TRANSACTION"))
			{
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			}
			return new BsonDataReader(_engine.Commit());
		}

		private BsonDataReader ParseCreate()
		{
			_tokenizer.ReadToken().Expect("CREATE");
			Token token = _tokenizer.ReadToken().Expect(TokenType.Word);
			bool unique = token.Is("UNIQUE");
			if (unique)
			{
				token = _tokenizer.ReadToken();
			}
			token.Expect("INDEX");
			string name = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			_tokenizer.ReadToken().Expect("ON");
			string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			_tokenizer.ReadToken().Expect(TokenType.OpenParenthesis);
			BsonExpression expr = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, new BsonDocument());
			_tokenizer.ReadToken().Expect(TokenType.CloseParenthesis);
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			return new BsonDataReader(_engine.EnsureIndex(collection, name, expr, unique));
		}

		private BsonDataReader ParseDelete()
		{
			_tokenizer.ReadToken().Expect("DELETE");
			string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			BsonExpression where = null;
			if (_tokenizer.LookAhead().Is("WHERE"))
			{
				_tokenizer.ReadToken();
				where = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
			}
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			_tokenizer.ReadToken();
			return new BsonDataReader(_engine.DeleteMany(collection, where));
		}

		private BsonDataReader ParseDrop()
		{
			_tokenizer.ReadToken().Expect("DROP");
			Token token = _tokenizer.ReadToken().Expect(TokenType.Word);
			if (token.Is("INDEX"))
			{
				string collection2 = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
				_tokenizer.ReadToken().Expect(TokenType.Period);
				string name = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
				return new BsonDataReader(_engine.DropIndex(collection2, name));
			}
			if (token.Is("COLLECTION"))
			{
				string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
				return new BsonDataReader(_engine.DropCollection(collection));
			}
			throw LiteException.UnexpectedToken(token, "INDEX|COLLECTION");
		}

		private BsonDataReader ParseInsert()
		{
			_tokenizer.ReadToken().Expect("INSERT");
			_tokenizer.ReadToken().Expect("INTO");
			string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			BsonAutoId autoId = ParseWithAutoId();
			_tokenizer.ReadToken().Expect("VALUES");
			IEnumerable<BsonDocument> docs = ParseListOfDocuments();
			return new BsonDataReader(_engine.Insert(collection, docs, autoId));
		}

		private BsonAutoId ParseWithAutoId()
		{
			if (_tokenizer.LookAhead().Type == TokenType.Colon)
			{
				_tokenizer.ReadToken();
				Token type = _tokenizer.ReadToken().Expect(TokenType.Word);
				return type.Value.ToUpper() switch
				{
					"GUID" => BsonAutoId.Guid, 
					"INT" => BsonAutoId.Int32, 
					"LONG" => BsonAutoId.Int64, 
					"OBJECTID" => BsonAutoId.ObjectId, 
					_ => throw LiteException.UnexpectedToken(type, "DATE, GUID, INT, LONG, OBJECTID"), 
				};
			}
			return BsonAutoId.ObjectId;
		}

		private IEnumerable<BsonExpression> ParseListOfExpressions()
		{
			while (true)
			{
				yield return BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
				if (_tokenizer.LookAhead().Type == TokenType.Comma)
				{
					_tokenizer.ReadToken();
					continue;
				}
				break;
			}
		}

		private IEnumerable<BsonDocument> ParseListOfDocuments()
		{
			JsonReader reader = new JsonReader(_tokenizer);
			Token next;
			while (true)
			{
				BsonValue value = reader.Deserialize();
				if (value.IsDocument)
				{
					yield return value as BsonDocument;
					next = _tokenizer.LookAhead();
					if (next.Type != TokenType.Comma)
					{
						break;
					}
					_tokenizer.ReadToken();
					continue;
				}
				throw LiteException.UnexpectedToken("Value must be a valid document", _tokenizer.Current);
			}
			next.Expect(TokenType.EOF, TokenType.SemiColon);
			_tokenizer.ReadToken();
		}

		private IBsonDataReader ParsePragma()
		{
			_tokenizer.ReadToken().Expect("PRAGMA");
			string name = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			Token eof = _tokenizer.LookAhead();
			if (eof.Type == TokenType.EOF || eof.Type == TokenType.SemiColon)
			{
				_tokenizer.ReadToken();
				return new BsonDataReader(_engine.Pragma(name));
			}
			if (eof.Type == TokenType.Equals)
			{
				_tokenizer.ReadToken().Expect(TokenType.Equals);
				BsonValue value = new JsonReader(_tokenizer).Deserialize();
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
				return new BsonDataReader(_engine.Pragma(name, value));
			}
			throw LiteException.UnexpectedToken(eof);
		}

		private BsonDataReader ParseRebuild()
		{
			_tokenizer.ReadToken().Expect("REBUILD");
			RebuildOptions options = new RebuildOptions();
			Token next = _tokenizer.LookAhead();
			if (next.Type == TokenType.EOF || next.Type == TokenType.SemiColon)
			{
				options = null;
				_tokenizer.ReadToken();
			}
			else
			{
				BsonValue json = new JsonReader(_tokenizer).Deserialize();
				if (!json.IsDocument)
				{
					throw LiteException.UnexpectedToken(next);
				}
				if (json["password"].IsString)
				{
					options.Password = json["password"];
				}
				if (json["collation"].IsString)
				{
					options.Collation = new Collation(json["collation"].AsString);
				}
			}
			return new BsonDataReader((int)_engine.Rebuild(options));
		}

		private BsonDataReader ParseRename()
		{
			_tokenizer.ReadToken().Expect("RENAME");
			_tokenizer.ReadToken().Expect("COLLECTION");
			string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			_tokenizer.ReadToken().Expect("TO");
			string newName = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			return new BsonDataReader(_engine.RenameCollection(collection, newName));
		}

		private BsonDataReader ParseRollback()
		{
			_tokenizer.ReadToken().Expect("ROLLBACK");
			Token token = _tokenizer.ReadToken().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon);
			if (token.Is("TRANS") || token.Is("TRANSACTION"))
			{
				_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			}
			return new BsonDataReader(_engine.Rollback());
		}

		private IBsonDataReader ParseSelect()
		{
			Query query = new Query();
			Token token = _tokenizer.ReadToken();
			query.ExplainPlan = token.Is("EXPLAIN");
			if (query.ExplainPlan)
			{
				token = _tokenizer.ReadToken();
			}
			token.Expect("SELECT");
			query.Select = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.SelectDocument, _parameters);
			Token from = _tokenizer.ReadToken();
			if (from.Type == TokenType.EOF || from.Type == TokenType.SemiColon)
			{
				IEnumerable<BsonValue> source = query.Select.Execute(_collation.Value);
				string defaultName = "expr";
				return new BsonDataReader(source.Select((BsonValue x) => (!x.IsDocument) ? new BsonDocument { [defaultName] = x } : x.AsDocument), null);
			}
			if (from.Is("INTO"))
			{
				query.Into = ParseCollection(_tokenizer);
				query.IntoAutoId = ParseWithAutoId();
				_tokenizer.ReadToken().Expect("FROM");
			}
			else
			{
				from.Expect("FROM");
			}
			string collection = ParseCollection(_tokenizer);
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("INCLUDE"))
			{
				_tokenizer.ReadToken();
				foreach (BsonExpression path in ParseListOfExpressions())
				{
					query.Includes.Add(path);
				}
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("WHERE"))
			{
				_tokenizer.ReadToken();
				BsonExpression where = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
				query.Where.Add(where);
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("GROUP"))
			{
				_tokenizer.ReadToken();
				_tokenizer.ReadToken().Expect("BY");
				BsonExpression groupBy = (query.GroupBy = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters));
				if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("HAVING"))
				{
					_tokenizer.ReadToken();
					BsonExpression having = (query.Having = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters));
				}
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("ORDER"))
			{
				_tokenizer.ReadToken();
				_tokenizer.ReadToken().Expect("BY");
				BsonExpression orderBy = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
				int orderByOrder = 1;
				Token orderByToken = _tokenizer.LookAhead();
				if (orderByToken.Is("ASC") || orderByToken.Is("DESC"))
				{
					orderByOrder = (_tokenizer.ReadToken().Is("ASC") ? 1 : (-1));
				}
				query.OrderBy = orderBy;
				query.Order = orderByOrder;
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("LIMIT"))
			{
				_tokenizer.ReadToken();
				string limit = _tokenizer.ReadToken().Expect(TokenType.Int).Value;
				query.Limit = Convert.ToInt32(limit);
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("OFFSET"))
			{
				_tokenizer.ReadToken();
				string offset = _tokenizer.ReadToken().Expect(TokenType.Int).Value;
				query.Offset = Convert.ToInt32(offset);
			}
			if (_tokenizer.LookAhead().Expect(TokenType.Word, TokenType.EOF, TokenType.SemiColon).Is("FOR"))
			{
				_tokenizer.ReadToken();
				_tokenizer.ReadToken().Expect("UPDATE");
				query.ForUpdate = true;
			}
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			return _engine.Query(collection, query);
		}

		public static string ParseCollection(Tokenizer tokenizer)
		{
			string name;
			BsonValue options;
			return ParseCollection(tokenizer, out name, out options);
		}

		public static string ParseCollection(Tokenizer tokenizer, out string name, out BsonValue options)
		{
			name = tokenizer.ReadToken().Expect(TokenType.Word).Value;
			if (name.StartsWith("$"))
			{
				if (tokenizer.LookAhead().Type == TokenType.OpenParenthesis)
				{
					tokenizer.ReadToken();
					if (tokenizer.LookAhead().Type == TokenType.CloseParenthesis)
					{
						options = null;
					}
					else
					{
						options = new JsonReader(tokenizer).Deserialize();
					}
					tokenizer.ReadToken().Expect(TokenType.CloseParenthesis);
				}
				else
				{
					options = null;
				}
			}
			else
			{
				options = null;
			}
			return name + ((options == null) ? "" : ("(" + JsonSerializer.Serialize(options) + ")"));
		}

		private BsonDataReader ParseUpdate()
		{
			_tokenizer.ReadToken().Expect("UPDATE");
			string collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;
			_tokenizer.ReadToken().Expect("SET");
			BsonExpression transform = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.UpdateDocument, _parameters);
			BsonExpression where = null;
			if (_tokenizer.LookAhead().Is("WHERE"))
			{
				_tokenizer.ReadToken();
				where = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
			}
			_tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);
			return new BsonDataReader(_engine.UpdateMany(collection, transform, where));
		}

		public SqlParser(ILiteEngine engine, Tokenizer tokenizer, BsonDocument parameters)
		{
			_engine = engine;
			_tokenizer = tokenizer;
			_parameters = parameters ?? new BsonDocument();
			_collation = new Lazy<Collation>(() => new Collation(_engine.Pragma("COLLATION")));
		}

		public IBsonDataReader Execute()
		{
			Token ahead = _tokenizer.LookAhead().Expect(TokenType.Word);
			string text = ahead.Value.ToUpper();
			if (text != null)
			{
				switch (text.Length)
				{
				case 6:
					switch (text[0])
					{
					case 'S':
						break;
					case 'I':
						if (!(text == "INSERT"))
						{
							goto end_IL_0030;
						}
						return ParseInsert();
					case 'D':
						if (!(text == "DELETE"))
						{
							goto end_IL_0030;
						}
						return ParseDelete();
					case 'U':
						if (!(text == "UPDATE"))
						{
							goto end_IL_0030;
						}
						return ParseUpdate();
					case 'R':
						if (!(text == "RENAME"))
						{
							goto end_IL_0030;
						}
						return ParseRename();
					case 'C':
						goto IL_0126;
					case 'P':
						if (!(text == "PRAGMA"))
						{
							goto end_IL_0030;
						}
						return ParsePragma();
					default:
						goto end_IL_0030;
					}
					if (!(text == "SELECT"))
					{
						break;
					}
					goto IL_01c3;
				case 7:
				{
					char c = text[0];
					if (c != 'E')
					{
						if (c != 'R' || !(text == "REBUILD"))
						{
							break;
						}
						return ParseRebuild();
					}
					if (!(text == "EXPLAIN"))
					{
						break;
					}
					goto IL_01c3;
				}
				case 4:
					if (!(text == "DROP"))
					{
						break;
					}
					return ParseDrop();
				case 10:
					if (!(text == "CHECKPOINT"))
					{
						break;
					}
					return ParseCheckpoint();
				case 5:
					if (!(text == "BEGIN"))
					{
						break;
					}
					return ParseBegin();
				case 8:
					{
						if (!(text == "ROLLBACK"))
						{
							break;
						}
						return ParseRollback();
					}
					IL_0126:
					if (!(text == "CREATE"))
					{
						if (!(text == "COMMIT"))
						{
							break;
						}
						return ParseCommit();
					}
					return ParseCreate();
					IL_01c3:
					return ParseSelect();
					end_IL_0030:
					break;
				}
			}
			throw LiteException.UnexpectedToken(ahead);
		}
	}
}
