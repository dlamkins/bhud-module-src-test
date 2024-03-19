using System.Security.Cryptography;
using System.Text;

namespace LiteDB
{
	internal static class StringExtensions
	{
		public static bool IsNullOrWhiteSpace(this string str)
		{
			if (str != null)
			{
				return str.Trim().Length == 0;
			}
			return true;
		}

		public static bool IsWord(this string str)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (!Tokenizer.IsWordChar(str[i], i == 0))
				{
					return false;
				}
			}
			return true;
		}

		public static string TrimToNull(this string str)
		{
			string v = str.Trim();
			if (v.Length != 0)
			{
				return v;
			}
			return null;
		}

		public static string Sha1(this string value)
		{
			byte[] data = Encoding.UTF8.GetBytes(value);
			using SHA1 sha = SHA1.Create();
			byte[] array = sha.ComputeHash(data);
			StringBuilder hash = new StringBuilder();
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				hash.Append(b.ToString("X2"));
			}
			return hash.ToString();
		}

		public static bool SqlLike(this string str, string pattern, Collation collation)
		{
			bool isMatch = true;
			bool isWildCardOn = false;
			bool isCharWildCardOn = false;
			bool isCharSetOn = false;
			bool isNotCharSetOn = false;
			bool endOfPattern = false;
			int lastWildCard = -1;
			int patternIndex = 0;
			char p = '\0';
			for (int j = 0; j < str.Length; j++)
			{
				char c = str[j];
				if (patternIndex < pattern.Length)
				{
					p = pattern[patternIndex];
					if (!isWildCardOn && p == '%')
					{
						lastWildCard = patternIndex;
						isWildCardOn = true;
						for (; patternIndex < pattern.Length && pattern[patternIndex] == '%'; patternIndex++)
						{
						}
						p = ((patternIndex < pattern.Length) ? pattern[patternIndex] : '\0');
					}
					else if (p == '_')
					{
						isCharWildCardOn = true;
						patternIndex++;
					}
				}
				if (isWildCardOn)
				{
					if (collation.Compare(c.ToString(), p.ToString()) == 0)
					{
						isWildCardOn = false;
						patternIndex++;
					}
				}
				else if (isCharWildCardOn)
				{
					isCharWildCardOn = false;
				}
				else if (isCharSetOn || isNotCharSetOn)
				{
					if (isCharSetOn)
					{
						if (lastWildCard < 0)
						{
							isMatch = false;
							break;
						}
						patternIndex = lastWildCard;
					}
					isNotCharSetOn = (isCharSetOn = false);
				}
				else if (collation.Compare(c.ToString(), p.ToString()) == 0)
				{
					patternIndex++;
				}
				else
				{
					if (lastWildCard < 0)
					{
						isMatch = false;
						break;
					}
					int back = patternIndex - lastWildCard - 1;
					j -= back;
					patternIndex = lastWildCard;
				}
			}
			endOfPattern = patternIndex >= pattern.Length;
			if (isMatch && !endOfPattern)
			{
				bool isOnlyWildCards = true;
				for (int i = patternIndex; i < pattern.Length; i++)
				{
					if (pattern[i] != '%')
					{
						isOnlyWildCards = false;
						break;
					}
				}
				if (isOnlyWildCards)
				{
					endOfPattern = true;
				}
			}
			return isMatch && endOfPattern;
		}

		public static string SqlLikeStartsWith(this string str, out bool hasMore)
		{
			int i = 0;
			int len = str.Length;
			char c = '\0';
			for (; i < len; i++)
			{
				c = str[i];
				if (c == '%' || c == '_')
				{
					break;
				}
			}
			hasMore = i != len && i != len - 1;
			return str.Substring(0, i);
		}
	}
}
