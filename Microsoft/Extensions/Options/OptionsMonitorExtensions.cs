using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal static class OptionsMonitorExtensions
	{
		public static IDisposable? OnChange<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(this IOptionsMonitor<TOptions> monitor, Action<TOptions> listener)
		{
			Action<TOptions> listener2 = listener;
			return monitor.OnChange(delegate(TOptions o, string _)
			{
				listener2(o);
			});
		}
	}
}
