using System;
using System.Threading;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal static class CancellationTokenUtils
	{
		internal static IDisposable? CreateLinkedToken(CancellationToken token1, CancellationToken token2, out CancellationToken linkedToken)
		{
			if (!token1.CanBeCanceled)
			{
				linkedToken = token2;
				return null;
			}
			if (!token2.CanBeCanceled)
			{
				linkedToken = token1;
				return null;
			}
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token1, token2);
			linkedToken = cancellationTokenSource.Token;
			return cancellationTokenSource;
		}
	}
}
