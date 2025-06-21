using System;
using System.Linq.Expressions;
using GuildWars2;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SL.ChatLinks.Storage.Converters
{
	public class ExtensibleEnumConverter<TEnum> : ValueConverter<Extensible<TEnum>, string> where TEnum : struct, Enum
	{
		public ExtensibleEnumConverter()
			: base((Expression<Func<Extensible<TEnum>, string>>)((Extensible<TEnum> value) => ((object)value).ToString()), (Expression<Func<string, Extensible<TEnum>>>)((string value) => new Extensible<TEnum>(value)), (ConverterMappingHints)null)
		{
		}
	}
}
