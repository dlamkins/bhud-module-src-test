using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
	internal readonly struct ReferenceEqualsWrapper : IEquatable<ReferenceEqualsWrapper>
	{
		private readonly object _object;

		public ReferenceEqualsWrapper(object obj)
		{
			_object = obj;
		}

		public override bool Equals([_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullWhen(true)] object obj)
		{
			if (obj is ReferenceEqualsWrapper)
			{
				ReferenceEqualsWrapper obj2 = (ReferenceEqualsWrapper)obj;
				return Equals(obj2);
			}
			return false;
		}

		public bool Equals(ReferenceEqualsWrapper obj)
		{
			return _object == obj._object;
		}

		public override int GetHashCode()
		{
			return RuntimeHelpers.GetHashCode(_object);
		}
	}
}
