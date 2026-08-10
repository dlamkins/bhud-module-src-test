using System;

namespace Microsoft.Extensions.Options
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	internal sealed class OptionsValidatorAttribute : Attribute
	{
	}
}
