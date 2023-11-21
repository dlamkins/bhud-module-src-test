using System.Collections.Generic;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		public override bool Equals(object x, object y)
		{
			return x == y;
		}

		public override int GetHashCode(object obj)
		{
			return obj?.GetHashCode() ?? 0;
		}
	}
}
