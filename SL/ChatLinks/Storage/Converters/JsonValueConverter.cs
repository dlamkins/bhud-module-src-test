using System;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SL.ChatLinks.Storage.Converters
{
	public class JsonValueConverter<T> : ValueConverter<T, string>
	{
		public JsonValueConverter()
			: base((Expression<Func<T, string>>)((T value) => Serialize(value)), (Expression<Func<string, T>>)((string value) => Deserialize(value)), (ConverterMappingHints)null)
		{
		}

		private static string Serialize(T value)
		{
			return JsonSerializer.Serialize(value);
		}

		private static T Deserialize(string value)
		{
			return JsonSerializer.Deserialize<T>(value);
		}
	}
}
