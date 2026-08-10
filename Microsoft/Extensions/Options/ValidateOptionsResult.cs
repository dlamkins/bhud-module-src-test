using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal class ValidateOptionsResult
	{
		public static readonly ValidateOptionsResult Skip = new ValidateOptionsResult
		{
			Skipped = true
		};

		public static readonly ValidateOptionsResult Success = new ValidateOptionsResult
		{
			Succeeded = true
		};

		public bool Succeeded { get; protected set; }

		public bool Skipped { get; protected set; }

		[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "Failures")]
		[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "FailureMessage")]
		public bool Failed
		{
			[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "Failures")]
			[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "FailureMessage")]
			get;
			[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "Failures")]
			[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EMemberNotNullWhen(true, "FailureMessage")]
			protected set;
		}

		public string? FailureMessage { get; protected set; }

		public IEnumerable<string>? Failures { get; protected set; }

		public static ValidateOptionsResult Fail(string failureMessage)
		{
			ValidateOptionsResult validateOptionsResult = new ValidateOptionsResult();
			validateOptionsResult.Failed = true;
			validateOptionsResult.FailureMessage = failureMessage;
			validateOptionsResult.Failures = new string[1] { failureMessage };
			return validateOptionsResult;
		}

		public static ValidateOptionsResult Fail(IEnumerable<string> failures)
		{
			return new ValidateOptionsResult
			{
				Failed = true,
				FailureMessage = string.Join("; ", failures),
				Failures = failures
			};
		}
	}
}
