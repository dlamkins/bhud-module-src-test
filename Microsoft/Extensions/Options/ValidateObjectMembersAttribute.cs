using System;

namespace Microsoft.Extensions.Options
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal sealed class ValidateObjectMembersAttribute : Attribute
	{
		public Type? Validator { get; }

		public ValidateObjectMembersAttribute()
		{
		}

		public ValidateObjectMembersAttribute(Type validator)
		{
			Validator = validator;
		}
	}
}
