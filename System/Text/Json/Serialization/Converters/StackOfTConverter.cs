using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization.Converters
{
	internal sealed class StackOfTConverter<TCollection, TElement> : IEnumerableDefaultConverter<TCollection, TElement> where TCollection : Stack<TElement>
	{
		internal override bool CanPopulate => true;

		protected override void Add(in TElement value, ref ReadStack state)
		{
			((TCollection)state.Current.ReturnValue).Push(value);
		}

		protected override void CreateCollection(ref Utf8JsonReader reader, [ScopedRef] ref ReadStack state, JsonSerializerOptions options)
		{
			if (!(state.ParentProperty?.TryGetPrePopulatedValue(ref state) ?? false))
			{
				if (state.Current.JsonTypeInfo.CreateObject == null)
				{
					ThrowHelper.ThrowNotSupportedException_SerializationNotSupported(state.Current.JsonTypeInfo.Type);
				}
				state.Current.ReturnValue = state.Current.JsonTypeInfo.CreateObject!();
			}
		}
	}
}
