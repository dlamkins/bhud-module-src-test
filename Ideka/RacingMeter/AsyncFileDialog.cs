using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;

namespace Ideka.RacingMeter
{
	public class AsyncFileDialog<T> : IDisposable where T : FileDialog, new()
	{
		private readonly TaskCompletionSource<DialogResult> _result;

		private readonly Thread _thread;

		public T Dialog { get; }

		public AsyncFileDialog()
		{
			Dialog = new T();
			_result = new TaskCompletionSource<DialogResult>(TaskCreationOptions.RunContinuationsAsynchronously);
			_thread = new Thread((ThreadStart)delegate
			{
				try
				{
					GameService.GameIntegration.get_Gw2Instance().add_Gw2AcquiredFocus((EventHandler<EventArgs>)abort);
					_result.SetResult(Dialog.ShowDialog());
					void abort(object sender, EventArgs e)
					{
						_thread?.Abort();
					}
				}
				catch (ThreadAbortException)
				{
					_result.SetResult(DialogResult.Cancel);
				}
				catch (Exception exception)
				{
					_result.SetException(exception);
				}
				finally
				{
					GameService.GameIntegration.get_Gw2Instance().remove_Gw2AcquiredFocus((EventHandler<EventArgs>)abort);
				}
			});
			_thread.SetApartmentState(ApartmentState.STA);
		}

		public Task<DialogResult> Show()
		{
			_thread.Start();
			return _result.Task;
		}

		public void Dispose()
		{
			_thread.Abort();
			Dialog?.Dispose();
		}
	}
}
