using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class AsyncTexture2DExtensions
	{
		public static Task WaitUntilSwappedAsync(this AsyncTexture2D texture, TimeSpan timeout)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			CancellationTokenSource ct = new CancellationTokenSource((int)timeout.TotalMilliseconds);
			CancellationTokenRegistration cancelAction = ct.Token.Register(delegate
			{
				tcs.TrySetCanceled();
			}, useSynchronizationContext: false);
			texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate
			{
				if (!ct.IsCancellationRequested)
				{
					cancelAction.Dispose();
					tcs.SetResult(null);
				}
			});
			return tcs.Task;
		}
	}
}
