using System;
using System.Collections.Generic;

namespace LiteDB
{
	internal class Token
	{
		private static readonly HashSet<string> _keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "BETWEEN", "LIKE", "IN", "AND", "OR" };

		public TokenType Type { get; private set; }

		public string Value { get; private set; }

		public long Position { get; private set; }

		public bool IsOperand
		{
			get
			{
				switch (Type)
				{
				case TokenType.NotEquals:
				case TokenType.Equals:
				case TokenType.Greater:
				case TokenType.GreaterOrEquals:
				case TokenType.Less:
				case TokenType.LessOrEquals:
				case TokenType.Minus:
				case TokenType.Plus:
				case TokenType.Asterisk:
				case TokenType.Slash:
				case TokenType.Percent:
					return true;
				case TokenType.Word:
					return _keywords.Contains(Value);
				default:
					return false;
				}
			}
		}

		public Token(TokenType tokenType, string value, long position)
		{
			Position = position;
			Value = value;
			Type = tokenType;
		}

		public Token Expect(TokenType type)
		{
			if (Type != type)
			{
				throw LiteException.UnexpectedToken(this);
			}
			return this;
		}

		public Token Expect(TokenType type1, TokenType type2)
		{
			if (Type != type1 && Type != type2)
			{
				throw LiteException.UnexpectedToken(this);
			}
			return this;
		}

		public Token Expect(TokenType type1, TokenType type2, TokenType type3)
		{
			if (Type != type1 && Type != type2 && Type != type3)
			{
				throw LiteException.UnexpectedToken(this);
			}
			return this;
		}

		public Token Expect(string value, bool ignoreCase = true)
		{
			if (!Is(value, ignoreCase))
			{
				throw LiteException.UnexpectedToken(this, value);
			}
			return this;
		}

		public bool Is(string value, bool ignoreCase = true)
		{
			if (Type == TokenType.Word)
			{
				return value.Equals(Value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			}
			return false;
		}

		public override string ToString()
		{
			return Value + " (" + Type.ToString() + ")";
		}
	}
}
