using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Microsoft.Extensions.Internal
{
	internal static class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper
	{
		private readonly struct DisplayNameOptions
		{
			public bool FullName { get; }

			public bool IncludeGenericParameters { get; }

			public bool IncludeGenericParameterNames { get; }

			public char NestedTypeDelimiter { get; }

			public DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
			{
				FullName = fullName;
				IncludeGenericParameters = includeGenericParameters;
				IncludeGenericParameterNames = includeGenericParameterNames;
				NestedTypeDelimiter = nestedTypeDelimiter;
			}
		}

		private const char DefaultNestedTypeDelimiter = '+';

		private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
		{
			{
				typeof(void),
				"void"
			},
			{
				typeof(bool),
				"bool"
			},
			{
				typeof(byte),
				"byte"
			},
			{
				typeof(char),
				"char"
			},
			{
				typeof(decimal),
				"decimal"
			},
			{
				typeof(double),
				"double"
			},
			{
				typeof(float),
				"float"
			},
			{
				typeof(int),
				"int"
			},
			{
				typeof(long),
				"long"
			},
			{
				typeof(object),
				"object"
			},
			{
				typeof(sbyte),
				"sbyte"
			},
			{
				typeof(short),
				"short"
			},
			{
				typeof(string),
				"string"
			},
			{
				typeof(uint),
				"uint"
			},
			{
				typeof(ulong),
				"ulong"
			},
			{
				typeof(ushort),
				"ushort"
			}
		};

		[return: _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ENotNullIfNotNull("item")]
		public static string? GetTypeDisplayName(object? item, bool fullName = true)
		{
			if (item != null)
			{
				return GetTypeDisplayName(item!.GetType(), fullName);
			}
			return null;
		}

		public static string GetTypeDisplayName(Type type, bool fullName = true, bool includeGenericParameterNames = false, bool includeGenericParameters = true, char nestedTypeDelimiter = '+')
		{
			StringBuilder builder = null;
			DisplayNameOptions options = new DisplayNameOptions(fullName, includeGenericParameterNames, includeGenericParameters, nestedTypeDelimiter);
			string text = ProcessType(ref builder, type, in options);
			return text ?? builder?.ToString() ?? string.Empty;
		}

		private static string ProcessType(ref StringBuilder builder, Type type, in DisplayNameOptions options)
		{
			string value;
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				if (builder == null)
				{
					builder = new StringBuilder();
				}
				ProcessGenericType(builder, type, genericArguments, genericArguments.Length, in options);
			}
			else if (type.IsArray)
			{
				if (builder == null)
				{
					builder = new StringBuilder();
				}
				ProcessArrayType(builder, type, in options);
			}
			else if (_builtInTypeNames.TryGetValue(type, out value))
			{
				if (builder == null)
				{
					return value;
				}
				builder.Append(value);
			}
			else if (type.IsGenericParameter)
			{
				if (options.IncludeGenericParameterNames)
				{
					if (builder == null)
					{
						return type.Name;
					}
					builder.Append(type.Name);
				}
			}
			else
			{
				string text = (options.FullName ? type.FullName : type.Name);
				if (builder == null)
				{
					if (options.NestedTypeDelimiter != '+')
					{
						return text.Replace('+', options.NestedTypeDelimiter);
					}
					return text;
				}
				builder.Append(text);
				if (options.NestedTypeDelimiter != '+')
				{
					builder.Replace('+', options.NestedTypeDelimiter, builder.Length - text.Length, text.Length);
				}
			}
			return null;
		}

		private static void ProcessArrayType(StringBuilder builder, Type type, in DisplayNameOptions options)
		{
			Type type2 = type;
			while (type2.IsArray)
			{
				type2 = type2.GetElementType();
			}
			ProcessType(ref builder, type2, in options);
			while (type.IsArray)
			{
				builder.Append('[');
				builder.Append(',', type.GetArrayRank() - 1);
				builder.Append(']');
				type = type.GetElementType();
			}
		}

		private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, in DisplayNameOptions options)
		{
			int num = 0;
			if (type.IsNested)
			{
				num = type.DeclaringType.GetGenericArguments().Length;
			}
			if (options.FullName)
			{
				if (type.IsNested)
				{
					ProcessGenericType(builder, type.DeclaringType, genericArguments, num, in options);
					builder.Append(options.NestedTypeDelimiter);
				}
				else if (!string.IsNullOrEmpty(type.Namespace))
				{
					builder.Append(type.Namespace);
					builder.Append('.');
				}
			}
			int num2 = type.Name.IndexOf('`');
			if (num2 <= 0)
			{
				builder.Append(type.Name);
				return;
			}
			builder.Append(type.Name, 0, num2);
			if (!options.IncludeGenericParameters)
			{
				return;
			}
			builder.Append('<');
			for (int i = num; i < length; i++)
			{
				ProcessType(ref builder, genericArguments[i], in options);
				if (i + 1 != length)
				{
					builder.Append(',');
					if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
					{
						builder.Append(' ');
					}
				}
			}
			builder.Append('>');
		}
	}
}
