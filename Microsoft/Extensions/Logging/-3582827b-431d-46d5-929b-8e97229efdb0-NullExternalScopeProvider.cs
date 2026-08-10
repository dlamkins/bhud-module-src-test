using System;

namespace Microsoft.Extensions.Logging
{
	internal sealed class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENullExternalScopeProvider : IExternalScopeProvider
	{
		public static IExternalScopeProvider Instance { get; } = new _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENullExternalScopeProvider();


		private _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENullExternalScopeProvider()
		{
		}

		void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
		}

		IDisposable IExternalScopeProvider.Push(object state)
		{
			return _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENullScope.Instance;
		}
	}
}
