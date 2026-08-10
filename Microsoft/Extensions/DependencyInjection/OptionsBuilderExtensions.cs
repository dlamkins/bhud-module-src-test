using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2091:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. Outer method has been annotated with DynamicallyAccessedMembers.")]
	internal static class OptionsBuilderExtensions
	{
		public static OptionsBuilder<TOptions> ValidateOnStart<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
		{
			OptionsBuilder<TOptions> optionsBuilder2 = optionsBuilder;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(optionsBuilder2, "optionsBuilder");
			optionsBuilder2.Services.AddTransient<IStartupValidator, StartupValidator>();
			optionsBuilder2.Services.AddOptions<StartupValidatorOptions>().Configure(delegate(StartupValidatorOptions vo, IOptionsMonitor<TOptions> options)
			{
				vo._validators[(typeof(TOptions), optionsBuilder2.Name)] = delegate
				{
					options.Get(optionsBuilder2.Name);
				};
			});
			return optionsBuilder2;
		}
	}
}
