using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
	internal sealed class StartupValidatorOptions
	{
		public Dictionary<(Type, string), Action> _validators { get; } = new Dictionary<(Type, string), Action>();

	}
}
