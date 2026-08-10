using System;

namespace Microsoft.Extensions.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Parameter)]
	internal class ServiceKeyAttribute : Attribute
	{
	}
}
