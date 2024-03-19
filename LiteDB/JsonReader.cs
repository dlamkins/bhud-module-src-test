using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LiteDB
{
	public class JsonReader
	{
		private static readonly IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

		private readonly Tokenizer _tokenizer;

		public long Position => _tokenizer.Position;

		public JsonReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			_tokenizer = new Tokenizer(reader);
		}

		internal JsonReader(Tokenizer tokenizer)
		{
			_tokenizer = tokenizer ?? throw new ArgumentNullException("tokenizer");
		}

		public BsonValue Deserialize()
		{
			Token token = _tokenizer.ReadToken();
			if (token.Type == TokenType.EOF)
			{
				return BsonValue.Null;
			}
			return ReadValue(token);
		}

		public IEnumerable<BsonValue> DeserializeArray()
		{
			Token token = _tokenizer.ReadToken();
			if (token.Type == TokenType.EOF)
			{
				yield break;
			}
			token.Expect(TokenType.OpenBracket);
			token = _tokenizer.ReadToken();
			while (token.Type != TokenType.CloseBracket)
			{
				yield return ReadValue(token);
				token = _tokenizer.ReadToken();
				if (token.Type == TokenType.Comma)
				{
					token = _tokenizer.ReadToken();
				}
			}
			token.Expect(TokenType.CloseBracket);
		}

		internal BsonValue ReadValue(Token token)
		{
			string value = token.Value;
			switch (token.Type)
			{
			case TokenType.String:
				return value;
			case TokenType.OpenBrace:
				return ReadObject();
			case TokenType.OpenBracket:
				return ReadArray();
			case TokenType.Minus:
			{
				Token number = _tokenizer.ReadToken(eatWhitespace: false).Expect(TokenType.Int, TokenType.Double);
				value = "-" + number.Value;
				if (number.Type == TokenType.Int)
				{
					goto case TokenType.Int;
				}
				if (number.Type != TokenType.Double)
				{
					break;
				}
				goto case TokenType.Double;
			}
			case TokenType.Int:
			{
				if (int.TryParse(value, NumberStyles.Any, _numberFormat, out var result))
				{
					return new BsonValue(result);
				}
				return new BsonValue(long.Parse(value, NumberStyles.Any, _numberFormat));
			}
			case TokenType.Double:
				return new BsonValue(Convert.ToDouble(value, _numberFormat));
			case TokenType.Word:
				return value.ToLower() switch
				{
					"null" => BsonValue.Null, 
					"true" => true, 
					"false" => false, 
					_ => throw LiteException.UnexpectedToken(token), 
				};
			}
			throw LiteException.UnexpectedToken(token);
		}

		private BsonValue ReadObject()
		{
			BsonDocument obj = new BsonDocument();
			Token token = _tokenizer.ReadToken();
			while (token.Type != TokenType.CloseBrace)
			{
				token.Expect(TokenType.String, TokenType.Word);
				string key = token.Value;
				token = _tokenizer.ReadToken();
				token.Expect(TokenType.Colon);
				token = _tokenizer.ReadToken();
				if (key[0] == '$' && obj.Count == 0)
				{
					BsonValue val = ReadExtendedDataType(key, token.Value);
					if (!val.IsNull)
					{
						return val;
					}
				}
				obj[key] = ReadValue(token);
				token = _tokenizer.ReadToken();
				if (token.Type == TokenType.Comma)
				{
					token = _tokenizer.ReadToken();
				}
			}
			return obj;
		}

		private BsonArray ReadArray()
		{
			BsonArray arr = new BsonArray();
			Token token = _tokenizer.ReadToken();
			while (token.Type != TokenType.CloseBracket)
			{
				BsonValue value = ReadValue(token);
				arr.Add(value);
				token = _tokenizer.ReadToken();
				if (token.Type == TokenType.Comma)
				{
					token = _tokenizer.ReadToken();
				}
			}
			return arr;
		}

		private BsonValue ReadExtendedDataType(string key, string value)
		{
			if (key != null)
			{
				BsonValue val;
				switch (key.Length)
				{
				case 5:
				{
					char c = key[1];
					if (c != 'd')
					{
						if (c != 'g' || !(key == "$guid"))
						{
							break;
						}
						val = new BsonValue(new Guid(value));
					}
					else
					{
						if (!(key == "$date"))
						{
							break;
						}
						val = new BsonValue(DateTime.Parse(value).ToLocalTime());
					}
					goto IL_0183;
				}
				case 9:
				{
					char c = key[2];
					if (c != 'a')
					{
						if (c != 'i' || !(key == "$minValue"))
						{
							break;
						}
						val = BsonValue.MinValue;
					}
					else
					{
						if (!(key == "$maxValue"))
						{
							break;
						}
						val = BsonValue.MaxValue;
					}
					goto IL_0183;
				}
				case 7:
					if (!(key == "$binary"))
					{
						break;
					}
					val = new BsonValue(Convert.FromBase64String(value));
					goto IL_0183;
				case 4:
					if (!(key == "$oid"))
					{
						break;
					}
					val = new BsonValue(new ObjectId(value));
					goto IL_0183;
				case 11:
					if (!(key == "$numberLong"))
					{
						break;
					}
					val = new BsonValue(Convert.ToInt64(value, _numberFormat));
					goto IL_0183;
				case 14:
					{
						if (!(key == "$numberDecimal"))
						{
							break;
						}
						val = new BsonValue(Convert.ToDecimal(value, _numberFormat));
						goto IL_0183;
					}
					IL_0183:
					_tokenizer.ReadToken().Expect(TokenType.CloseBrace);
					return val;
				}
			}
			return BsonValue.Null;
		}
	}
}
