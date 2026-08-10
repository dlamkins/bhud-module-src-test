using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Microsoft.Extensions.Options
{
	[DebuggerDisplay("{ErrorsCount} errors")]
	internal class ValidateOptionsResultBuilder
	{
		private const string MemberSeparatorString = ", ";

		private List<string> _errors;

		private int ErrorsCount
		{
			get
			{
				if (_errors != null)
				{
					return _errors.Count;
				}
				return 0;
			}
		}

		private List<string> Errors => _errors ?? (_errors = new List<string>());

		public void AddError(string error, string? propertyName = null)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(error, "error");
			Errors.Add((propertyName == null) ? error : ("Property " + propertyName + ": " + error));
		}

		public void AddResult(ValidationResult? result)
		{
			if (result?.ErrorMessage != null)
			{
				string text = string.Join(", ", result!.MemberNames);
				Errors.Add((text.Length != 0) ? (text + ": " + result!.ErrorMessage) : result!.ErrorMessage);
			}
		}

		public void AddResults(IEnumerable<ValidationResult?>? results)
		{
			if (results == null)
			{
				return;
			}
			foreach (ValidationResult item in results!)
			{
				AddResult(item);
			}
		}

		public void AddResult(ValidateOptionsResult result)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(result, "result");
			if (!result.Failed)
			{
				return;
			}
			if (result.Failures == null)
			{
				Errors.Add(result.FailureMessage);
				return;
			}
			foreach (string item in result.Failures!)
			{
				if (item != null)
				{
					Errors.Add(item);
				}
			}
		}

		public ValidateOptionsResult Build()
		{
			List<string> errors = _errors;
			if (errors != null && errors.Count > 0)
			{
				return ValidateOptionsResult.Fail(_errors);
			}
			return ValidateOptionsResult.Success;
		}

		public void Clear()
		{
			_errors?.Clear();
		}
	}
}
