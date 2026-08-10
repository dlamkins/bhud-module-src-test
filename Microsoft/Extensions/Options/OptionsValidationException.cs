using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
	internal class OptionsValidationException : Exception
	{
		public string OptionsName { get; }

		public Type OptionsType { get; }

		public IEnumerable<string> Failures { get; }

		public override string Message => string.Join("; ", Failures);

		public OptionsValidationException(string optionsName, Type optionsType, IEnumerable<string>? failureMessages)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(optionsName, "optionsName");
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(optionsType, "optionsType");
			Failures = failureMessages ?? new List<string>();
			OptionsType = optionsType;
			OptionsName = optionsName;
		}
	}
}
