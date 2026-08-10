using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Microsoft.Extensions.Logging
{
	internal sealed class LogValuesFormatter
	{
		private const string NullValue = "(null)";

		private static readonly char[] FormatDelimiters = new char[2] { ',', ':' };

		private readonly List<string> _valueNames = new List<string>();

		private readonly string _format;

		public string OriginalFormat { get; private set; }

		public List<string> ValueNames => _valueNames;

		public LogValuesFormatter(string format)
		{
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper.ThrowIfNull(format, "format");
			OriginalFormat = format;
			Span<char> initialBuffer = stackalloc char[256];
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder = new _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder(initialBuffer);
			int num = 0;
			int length = format.Length;
			while (num < length)
			{
				int num2 = FindBraceIndex(format, '{', num, length);
				if (num == 0 && num2 == length)
				{
					_format = format;
					return;
				}
				int num3 = FindBraceIndex(format, '}', num2, length);
				if (num3 == length)
				{
					_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append(format.AsSpan(num, length - num));
					num = length;
					continue;
				}
				int num4 = FindIndexOfAny(format, FormatDelimiters, num2, num3);
				_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append(format.AsSpan(num, num2 - num + 1));
				_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append(_valueNames.Count.ToString());
				_valueNames.Add(format.Substring(num2 + 1, num4 - num2 - 1));
				_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append(format.AsSpan(num4, num3 - num4 + 1));
				num = num3 + 1;
			}
			_format = _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.ToString();
		}

		private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
		{
			int result = endIndex;
			int i = startIndex;
			int num = 0;
			for (; i < endIndex; i++)
			{
				if (num > 0 && format[i] != brace)
				{
					if (num % 2 != 0)
					{
						break;
					}
					num = 0;
					result = endIndex;
				}
				else
				{
					if (format[i] != brace)
					{
						continue;
					}
					if (brace == '}')
					{
						if (num == 0)
						{
							result = i;
						}
					}
					else
					{
						result = i;
					}
					num++;
				}
			}
			return result;
		}

		private static int FindIndexOfAny(string format, char[] chars, int startIndex, int endIndex)
		{
			int num = format.IndexOfAny(chars, startIndex, endIndex - startIndex);
			if (num != -1)
			{
				return num;
			}
			return endIndex;
		}

		public string Format(object?[]? values)
		{
			object[] array = values;
			if (values != null)
			{
				for (int i = 0; i < values!.Length; i++)
				{
					object obj = FormatArgument(values[i]);
					if (obj != values[i])
					{
						array = new object[values!.Length];
						Array.Copy(values, array, i);
						array[i++] = obj;
						for (; i < values!.Length; i++)
						{
							array[i] = FormatArgument(values[i]);
						}
						break;
					}
				}
			}
			return string.Format(CultureInfo.InvariantCulture, _format, array ?? Array.Empty<object>());
		}

		internal string FormatWithOverwrite(object?[]? values)
		{
			if (values != null)
			{
				for (int i = 0; i < values!.Length; i++)
				{
					values[i] = FormatArgument(values[i]);
				}
			}
			return string.Format(CultureInfo.InvariantCulture, _format, values ?? Array.Empty<object>());
		}

		internal string Format()
		{
			return _format;
		}

		internal string Format(object? arg0)
		{
			return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0));
		}

		internal string Format(object? arg0, object? arg1)
		{
			return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0), FormatArgument(arg1));
		}

		internal string Format(object? arg0, object? arg1, object? arg2)
		{
			return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0), FormatArgument(arg1), FormatArgument(arg2));
		}

		public KeyValuePair<string, object?> GetValue(object?[] values, int index)
		{
			if (index < 0 || index > _valueNames.Count)
			{
				throw new IndexOutOfRangeException("index");
			}
			if (_valueNames.Count > index)
			{
				return new KeyValuePair<string, object>(_valueNames[index], values[index]);
			}
			return new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
		}

		public IEnumerable<KeyValuePair<string, object?>> GetValues(object[] values)
		{
			KeyValuePair<string, object>[] array = new KeyValuePair<string, object>[values.Length + 1];
			for (int i = 0; i != _valueNames.Count; i++)
			{
				array[i] = new KeyValuePair<string, object>(_valueNames[i], values[i]);
			}
			array[array.Length - 1] = new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
			return array;
		}

		private static object FormatArgument(object value)
		{
			if (!TryFormatArgumentIfNullOrEnumerable(value, out var stringValue))
			{
				return value;
			}
			return stringValue;
		}

		private static bool TryFormatArgumentIfNullOrEnumerable<T>(T value, [_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ENotNullWhen(true)] out object stringValue)
		{
			if (value == null)
			{
				stringValue = "(null)";
				return true;
			}
			if (!(value is string))
			{
				IEnumerable enumerable = value as IEnumerable;
				if (enumerable != null)
				{
					Span<char> initialBuffer = stackalloc char[256];
					_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder = new _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder(initialBuffer);
					bool flag = true;
					foreach (object item in enumerable)
					{
						if (!flag)
						{
							_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append(", ");
						}
						_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.Append((item != null) ? item.ToString() : "(null)");
						flag = false;
					}
					stringValue = _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EValueStringBuilder.ToString();
					return true;
				}
			}
			stringValue = null;
			return false;
		}
	}
}
