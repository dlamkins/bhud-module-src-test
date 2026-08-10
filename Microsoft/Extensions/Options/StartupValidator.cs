using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace Microsoft.Extensions.Options
{
	internal sealed class StartupValidator : IStartupValidator
	{
		private readonly StartupValidatorOptions _validatorOptions;

		public StartupValidator(IOptions<StartupValidatorOptions> validators)
		{
			_validatorOptions = validators.Value;
		}

		public void Validate()
		{
			List<Exception> list = null;
			foreach (Action value in _validatorOptions._validators.Values)
			{
				try
				{
					value();
				}
				catch (OptionsValidationException item)
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(item);
				}
			}
			if (list != null)
			{
				if (list.Count == 1)
				{
					ExceptionDispatchInfo.Capture(list[0]).Throw();
				}
				if (list.Count > 1)
				{
					throw new AggregateException(list);
				}
			}
		}
	}
}
