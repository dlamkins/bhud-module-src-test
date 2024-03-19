using System.IO;
using System.Text;

namespace LiteDB
{
	internal class Tokenizer
	{
		private readonly TextReader _reader;

		private char _char;

		private Token _ahead;

		private bool _eof;

		public bool EOF
		{
			get
			{
				if (_eof)
				{
					return _ahead == null;
				}
				return false;
			}
		}

		public long Position { get; private set; }

		public Token Current { get; private set; }

		public bool CheckEOF()
		{
			if (_eof)
			{
				throw LiteException.UnexpectedToken(Current);
			}
			return false;
		}

		public Tokenizer(string source)
			: this(new StringReader(source))
		{
		}

		public Tokenizer(TextReader reader)
		{
			_reader = reader;
			Position = 0L;
			ReadChar();
		}

		public static bool IsWordChar(char c, bool first)
		{
			if (first)
			{
				if (!char.IsLetter(c) && c != '_')
				{
					return c == '$';
				}
				return true;
			}
			if (!char.IsLetterOrDigit(c) && c != '_')
			{
				return c == '$';
			}
			return true;
		}

		private char ReadChar()
		{
			if (_eof)
			{
				return '\0';
			}
			int c = _reader.Read();
			Position++;
			if (c == -1)
			{
				_char = '\0';
				_eof = true;
			}
			else
			{
				_char = (char)c;
			}
			return _char;
		}

		public Token LookAhead(bool eatWhitespace = true)
		{
			if (_ahead != null)
			{
				if (eatWhitespace && _ahead.Type == TokenType.Whitespace)
				{
					_ahead = ReadNext(eatWhitespace);
				}
				return _ahead;
			}
			return _ahead = ReadNext(eatWhitespace);
		}

		public Token ReadToken(bool eatWhitespace = true)
		{
			if (_ahead == null)
			{
				return Current = ReadNext(eatWhitespace);
			}
			if (eatWhitespace && _ahead.Type == TokenType.Whitespace)
			{
				_ahead = ReadNext(eatWhitespace);
			}
			Current = _ahead;
			_ahead = null;
			return Current;
		}

		private Token ReadNext(bool eatWhitespace)
		{
			if (eatWhitespace)
			{
				EatWhitespace();
			}
			if (_eof)
			{
				return new Token(TokenType.EOF, null, Position);
			}
			Token token = null;
			switch (_char)
			{
			case '{':
				token = new Token(TokenType.OpenBrace, "{", Position);
				ReadChar();
				break;
			case '}':
				token = new Token(TokenType.CloseBrace, "}", Position);
				ReadChar();
				break;
			case '[':
				token = new Token(TokenType.OpenBracket, "[", Position);
				ReadChar();
				break;
			case ']':
				token = new Token(TokenType.CloseBracket, "]", Position);
				ReadChar();
				break;
			case '(':
				token = new Token(TokenType.OpenParenthesis, "(", Position);
				ReadChar();
				break;
			case ')':
				token = new Token(TokenType.CloseParenthesis, ")", Position);
				ReadChar();
				break;
			case ',':
				token = new Token(TokenType.Comma, ",", Position);
				ReadChar();
				break;
			case ':':
				token = new Token(TokenType.Colon, ":", Position);
				ReadChar();
				break;
			case ';':
				token = new Token(TokenType.SemiColon, ";", Position);
				ReadChar();
				break;
			case '@':
				token = new Token(TokenType.At, "@", Position);
				ReadChar();
				break;
			case '#':
				token = new Token(TokenType.Hashtag, "#", Position);
				ReadChar();
				break;
			case '~':
				token = new Token(TokenType.Til, "~", Position);
				ReadChar();
				break;
			case '.':
				token = new Token(TokenType.Period, ".", Position);
				ReadChar();
				break;
			case '&':
				token = new Token(TokenType.Ampersand, "&", Position);
				ReadChar();
				break;
			case '$':
				ReadChar();
				token = ((!IsWordChar(_char, first: true)) ? new Token(TokenType.Dollar, "$", Position) : new Token(TokenType.Word, "$" + ReadWord(), Position));
				break;
			case '!':
				ReadChar();
				if (_char == '=')
				{
					token = new Token(TokenType.NotEquals, "!=", Position);
					ReadChar();
				}
				else
				{
					token = new Token(TokenType.Exclamation, "!", Position);
				}
				break;
			case '=':
				token = new Token(TokenType.Equals, "=", Position);
				ReadChar();
				break;
			case '>':
				ReadChar();
				if (_char == '=')
				{
					token = new Token(TokenType.GreaterOrEquals, ">=", Position);
					ReadChar();
				}
				else
				{
					token = new Token(TokenType.Greater, ">", Position);
				}
				break;
			case '<':
				ReadChar();
				if (_char == '=')
				{
					token = new Token(TokenType.LessOrEquals, "<=", Position);
					ReadChar();
				}
				else
				{
					token = new Token(TokenType.Less, "<", Position);
				}
				break;
			case '-':
				ReadChar();
				if (_char == '-')
				{
					ReadLine();
					token = ReadNext(eatWhitespace);
				}
				else
				{
					token = new Token(TokenType.Minus, "-", Position);
				}
				break;
			case '+':
				token = new Token(TokenType.Plus, "+", Position);
				ReadChar();
				break;
			case '*':
				token = new Token(TokenType.Asterisk, "*", Position);
				ReadChar();
				break;
			case '/':
				token = new Token(TokenType.Slash, "/", Position);
				ReadChar();
				break;
			case '\\':
				token = new Token(TokenType.Backslash, "\\", Position);
				ReadChar();
				break;
			case '%':
				token = new Token(TokenType.Percent, "%", Position);
				ReadChar();
				break;
			case '"':
			case '\'':
				token = new Token(TokenType.String, ReadString(_char), Position);
				break;
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
			{
				bool dbl = false;
				string number = ReadNumber(ref dbl);
				token = new Token(dbl ? TokenType.Double : TokenType.Int, number, Position);
				break;
			}
			case '\t':
			case '\n':
			case '\r':
			case ' ':
			{
				StringBuilder sb = new StringBuilder();
				while (char.IsWhiteSpace(_char) && !_eof)
				{
					sb.Append(_char);
					ReadChar();
				}
				token = new Token(TokenType.Whitespace, sb.ToString(), Position);
				break;
			}
			default:
				if (IsWordChar(_char, first: true))
				{
					token = new Token(TokenType.Word, ReadWord(), Position);
				}
				else
				{
					ReadChar();
				}
				break;
			}
			return token ?? new Token(TokenType.Unknown, _char.ToString(), Position);
		}

		private void EatWhitespace()
		{
			while (char.IsWhiteSpace(_char) && !_eof)
			{
				ReadChar();
			}
		}

		private string ReadWord()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(_char);
			ReadChar();
			while (!_eof && IsWordChar(_char, first: false))
			{
				sb.Append(_char);
				ReadChar();
			}
			return sb.ToString();
		}

		private string ReadNumber(ref bool dbl)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(_char);
			bool canDot = true;
			bool canE = true;
			bool canSign = false;
			ReadChar();
			while (!_eof && (char.IsDigit(_char) || _char == '+' || _char == '-' || _char == '.' || _char == 'e' || _char == 'E'))
			{
				if (_char == '.')
				{
					if (!canDot)
					{
						break;
					}
					dbl = true;
					canDot = false;
				}
				else if (_char == 'e' || _char == 'E')
				{
					if (!canE)
					{
						break;
					}
					canE = false;
					canSign = true;
					dbl = true;
				}
				else if (_char == '-' || _char == '+')
				{
					if (!canSign)
					{
						break;
					}
					canSign = false;
				}
				sb.Append(_char);
				ReadChar();
			}
			return sb.ToString();
		}

		private string ReadString(char quote)
		{
			StringBuilder sb = new StringBuilder();
			ReadChar();
			while (_char != quote && !_eof)
			{
				if (_char == '\\')
				{
					ReadChar();
					if (_char == quote)
					{
						sb.Append(quote);
					}
					switch (_char)
					{
					case '\\':
						sb.Append('\\');
						break;
					case '/':
						sb.Append('/');
						break;
					case 'b':
						sb.Append('\b');
						break;
					case 'f':
						sb.Append('\f');
						break;
					case 'n':
						sb.Append('\n');
						break;
					case 'r':
						sb.Append('\r');
						break;
					case 't':
						sb.Append('\t');
						break;
					case 'u':
					{
						uint codePoint = ParseUnicode(ReadChar(), ReadChar(), ReadChar(), ReadChar());
						sb.Append((char)codePoint);
						break;
					}
					}
				}
				else
				{
					sb.Append(_char);
				}
				ReadChar();
			}
			ReadChar();
			return sb.ToString();
		}

		private void ReadLine()
		{
			while (_char != '\n' && !_eof)
			{
				ReadChar();
			}
			if (_char == '\n')
			{
				ReadChar();
			}
		}

		public static uint ParseUnicode(char c1, char c2, char c3, char c4)
		{
			uint num = ParseSingleChar(c1, 4096u);
			uint p2 = ParseSingleChar(c2, 256u);
			uint p3 = ParseSingleChar(c3, 16u);
			uint p4 = ParseSingleChar(c4, 1u);
			return num + p2 + p3 + p4;
		}

		public static uint ParseSingleChar(char c1, uint multiplier)
		{
			uint p1 = 0u;
			if (c1 >= '0' && c1 <= '9')
			{
				p1 = (uint)(c1 - 48) * multiplier;
			}
			else if (c1 >= 'A' && c1 <= 'F')
			{
				p1 = (uint)(c1 - 65 + 10) * multiplier;
			}
			else if (c1 >= 'a' && c1 <= 'f')
			{
				p1 = (uint)(c1 - 97 + 10) * multiplier;
			}
			return p1;
		}

		public override string ToString()
		{
			return Current?.ToString() + " [ahead: " + _ahead?.ToString() + "] - position: " + Position;
		}
	}
}
