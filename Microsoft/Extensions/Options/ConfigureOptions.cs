using System;

namespace Microsoft.Extensions.Options
{
	internal class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
	{
		public Action<TOptions>? Action { get; }

		public ConfigureOptions(Action<TOptions>? action)
		{
			Action = action;
		}

		public virtual void Configure(TOptions options)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(options, "options");
			Action?.Invoke(options);
		}
	}
}
