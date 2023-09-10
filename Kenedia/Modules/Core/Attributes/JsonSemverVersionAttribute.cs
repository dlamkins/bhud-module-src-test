using System;

namespace Kenedia.Modules.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	internal sealed class JsonSemverVersionAttribute : Attribute
	{
	}
}
