using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nekres.Musician
{
	internal static class DictionaryExtension
	{
		public static async Task<Dictionary<TKey, TResult>> ToResults<TKey, TResult>(this IEnumerable<KeyValuePair<TKey, Task<TResult>>> input)
		{
			return (await Task.WhenAll(input.Select(async delegate(KeyValuePair<TKey, Task<TResult>> pair)
			{
				TKey key = pair.Key;
				return new
				{
					Key = key,
					Value = await pair.Value
				};
			}))).ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}
