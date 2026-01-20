using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using GuildWars2.Collections;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SL.ChatLinks.Storage.Converters
{
	public class ImmutableValueDictionaryConverter<TKey, TValue> : ValueConverter<IImmutableValueDictionary<TKey, TValue>, string> where TKey : notnull
	{
		public ImmutableValueDictionaryConverter(Func<string, TKey> keyToProvider, Func<TKey, string> keyFromProvider)
		{
			Func<TKey, string> keyFromProvider2 = keyFromProvider;
			Func<string, TKey> keyToProvider2 = keyToProvider;
			base._002Ector((Expression<Func<IImmutableValueDictionary<TKey, TValue>, string>>)((IImmutableValueDictionary<TKey, TValue> value) => Serialize(value, keyFromProvider2)), (Expression<Func<string, IImmutableValueDictionary<TKey, TValue>>>)((string value) => Deserialize(value, keyToProvider2)), (ConverterMappingHints)null);
		}

		private static string Serialize(IImmutableValueDictionary<TKey, TValue> value, Func<TKey, string> keyMapper)
		{
			Func<TKey, string> keyMapper2 = keyMapper;
			return JsonSerializer.Serialize(value.ToDictionary<KeyValuePair<TKey, TValue>, string, TValue>((KeyValuePair<TKey, TValue> kvp) => keyMapper2(kvp.Key), (KeyValuePair<TKey, TValue> kvp) => kvp.Value));
		}

		private static ImmutableValueDictionary<TKey, TValue> Deserialize(string value, Func<string, TKey> keyMapper)
		{
			Dictionary<string, TValue> intermediate = JsonSerializer.Deserialize<Dictionary<string, TValue>>(value);
			ImmutableDictionary<TKey, TValue>.Builder builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
			if (intermediate != null)
			{
				foreach (KeyValuePair<string, TValue> kvp in intermediate)
				{
					builder.Add(keyMapper(kvp.Key), kvp.Value);
				}
			}
			return new ImmutableValueDictionary<TKey, TValue>(builder.ToImmutable());
		}
	}
}
