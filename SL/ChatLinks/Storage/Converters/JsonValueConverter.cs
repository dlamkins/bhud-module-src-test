using System;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SL.ChatLinks.Storage.Converters
{
	public class JsonValueConverter<T> : ValueConverter<T, string>
	{
		public JsonValueConverter(JsonSerializerOptions? options = null)
		{
			JsonSerializerOptions options2 = options;
			base._002Ector((Expression<Func<T, string>>)((T value) => Serialize(value, options2)), (Expression<Func<string, T>>)((string value) => Deserialize(value, options2)), (ConverterMappingHints)null);
		}

		private static string Serialize(T value, JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Serialize(value, options);
		}

		private static T Deserialize(string value, JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Deserialize<T>(value, options);
		}
	}
}
