using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SL.ChatLinks
{
	internal static class EnumerableExtensions
	{
		[IteratorStateMachine(typeof(_003CChunk_003Ed__0<>))]
		public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size)
		{
			return new _003CChunk_003Ed__0<TSource>(-2)
			{
				_003C_003E3__source = source,
				_003C_003E3__size = size
			};
		}
	}
}
