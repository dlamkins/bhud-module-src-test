using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteDB
{
	internal static class DictionaryExtensions
	{
		public static T GetOrDefault<K, T>(this IDictionary<K, T> dict, K key, T defaultValue = default(T))
		{
			if (dict.TryGetValue(key, out var result))
			{
				return result;
			}
			return defaultValue;
		}

		public static T GetOrAdd<K, T>(this IDictionary<K, T> dict, K key, Func<K, T> valueFactoy)
		{
			if (!dict.TryGetValue(key, out var value))
			{
				value = valueFactoy(key);
				dict.Add(key, value);
			}
			return value;
		}

		public static void ParseKeyValue(this IDictionary<string, string> dict, string connectionString)
		{
			int position = 0;
			while (position < connectionString.Length)
			{
				EatWhitespace();
				string key = ReadKey();
				EatWhitespace();
				string value = (dict[key] = ReadValue());
			}
			void EatWhitespace()
			{
				for (; position < connectionString.Length && (connectionString[position] == ' ' || connectionString[position] == '\t' || connectionString[position] == '\f'); position++)
				{
				}
			}
			string ReadKey()
			{
				StringBuilder sb2 = new StringBuilder();
				for (; position < connectionString.Length; position++)
				{
					char current2 = connectionString[position];
					if (current2 == '=')
					{
						position++;
						return sb2.ToString().Trim();
					}
					sb2.Append(current2);
				}
				return sb2.ToString().Trim();
			}
			string ReadValue()
			{
				StringBuilder sb = new StringBuilder();
				char quote = ((connectionString[position] == '"') ? '"' : ((connectionString[position] == '\'') ? '\'' : ' '));
				if (quote != ' ')
				{
					position++;
				}
				for (; position < connectionString.Length; position++)
				{
					char current = connectionString[position];
					if (quote == ' ')
					{
						if (current == ';')
						{
							position++;
							return sb.ToString().Trim();
						}
					}
					else if (quote != ' ' && current == quote)
					{
						if (connectionString[position - 1] != '\\')
						{
							position++;
							EatWhitespace();
							if (position < connectionString.Length && connectionString[position] == ';')
							{
								position++;
							}
							return sb.ToString();
						}
						sb.Length--;
					}
					sb.Append(current);
				}
				return sb.ToString().Trim();
			}
		}

		public static T GetValue<T>(this Dictionary<string, string> dict, string key, T defaultValue = default(T))
		{
			try
			{
				if (!dict.TryGetValue(key, out var value))
				{
					return defaultValue;
				}
				if (typeof(T) == typeof(TimeSpan))
				{
					if (Regex.IsMatch(value, "^\\d+$", RegexOptions.Compiled))
					{
						return (T)(object)TimeSpan.FromSeconds(Convert.ToInt32(value));
					}
					return (T)(object)TimeSpan.Parse(value);
				}
				if (typeof(T).GetTypeInfo().IsEnum)
				{
					return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
				}
				return (T)Convert.ChangeType(value, typeof(T));
			}
			catch (Exception)
			{
				throw new LiteException(0, "Invalid connection string value type for `" + key + "`");
			}
		}

		public static long GetFileSize(this Dictionary<string, string> dict, string key, long defaultValue)
		{
			string size = dict.GetValue<string>(key);
			if (size == null)
			{
				return defaultValue;
			}
			Match match = Regex.Match(size, "^(\\d+)\\s*([tgmk])?(b|byte|bytes)?$", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				return 0L;
			}
			long num = Convert.ToInt64(match.Groups[1].Value);
			string text = match.Groups[2].Value.ToLower();
			switch (text)
			{
			default:
				if (text.Length != 0)
				{
					break;
				}
				return num;
			case "t":
				return num * 1024 * 1024 * 1024 * 1024;
			case "g":
				return num * 1024 * 1024 * 1024;
			case "m":
				return num * 1024 * 1024;
			case "k":
				return num * 1024;
			case null:
				break;
			}
			return 0L;
		}
	}
}
