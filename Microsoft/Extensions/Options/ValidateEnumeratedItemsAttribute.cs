using System;

namespace Microsoft.Extensions.Options
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal sealed class ValidateEnumeratedItemsAttribute : Attribute
	{
		public Type? Validator { get; }

		public ValidateEnumeratedItemsAttribute()
		{
		}

		public ValidateEnumeratedItemsAttribute(Type validator)
		{
			Validator = validator;
		}
	}
}
