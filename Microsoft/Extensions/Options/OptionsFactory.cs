using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal class OptionsFactory<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsFactory<TOptions> where TOptions : class
	{
		private readonly IConfigureOptions<TOptions>[] _setups;

		private readonly IPostConfigureOptions<TOptions>[] _postConfigures;

		private readonly IValidateOptions<TOptions>[] _validations;

		public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures)
			: this(setups, postConfigures, (IEnumerable<IValidateOptions<TOptions>>)Array.Empty<IValidateOptions<TOptions>>())
		{
		}

		public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations)
		{
			_setups = (setups as IConfigureOptions<TOptions>[]) ?? new List<IConfigureOptions<TOptions>>(setups).ToArray();
			_postConfigures = (postConfigures as IPostConfigureOptions<TOptions>[]) ?? new List<IPostConfigureOptions<TOptions>>(postConfigures).ToArray();
			_validations = (validations as IValidateOptions<TOptions>[]) ?? new List<IValidateOptions<TOptions>>(validations).ToArray();
		}

		public TOptions Create(string name)
		{
			TOptions val = CreateInstance(name);
			IConfigureOptions<TOptions>[] setups = _setups;
			foreach (IConfigureOptions<TOptions> configureOptions in setups)
			{
				IConfigureNamedOptions<TOptions> configureNamedOptions = configureOptions as IConfigureNamedOptions<TOptions>;
				if (configureNamedOptions != null)
				{
					configureNamedOptions.Configure(name, val);
				}
				else if (name == Options.DefaultName)
				{
					configureOptions.Configure(val);
				}
			}
			IPostConfigureOptions<TOptions>[] postConfigures = _postConfigures;
			foreach (IPostConfigureOptions<TOptions> postConfigureOptions in postConfigures)
			{
				postConfigureOptions.PostConfigure(name, val);
			}
			if (_validations.Length != 0)
			{
				List<string> list = new List<string>();
				IValidateOptions<TOptions>[] validations = _validations;
				foreach (IValidateOptions<TOptions> validateOptions in validations)
				{
					ValidateOptionsResult validateOptionsResult = validateOptions.Validate(name, val);
					if (validateOptionsResult != null && validateOptionsResult.Failed)
					{
						list.AddRange(validateOptionsResult.Failures);
					}
				}
				if (list.Count > 0)
				{
					throw new OptionsValidationException(name, typeof(TOptions), list);
				}
			}
			return val;
		}

		protected virtual TOptions CreateInstance(string name)
		{
			return Activator.CreateInstance<TOptions>();
		}
	}
}
