using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;

namespace Estreya.BlishHUD.Shared.Threading
{
	public class AsyncFileDialog<T> where T : FileDialog
	{
		private readonly TaskCompletionSource<DialogResult> _result;

		private readonly Thread _thread;

		public T Dialog { get; }

		public AsyncFileDialog(T dialog)
		{
			Dialog = dialog;
			_result = new TaskCompletionSource<DialogResult>(TaskCreationOptions.RunContinuationsAsynchronously);
			_thread = new Thread((ThreadStart)delegate
			{
				try
				{
					GameService.GameIntegration.get_Gw2Instance().add_Gw2AcquiredFocus((EventHandler<EventArgs>)abort);
					DialogResult result = Dialog.ShowDialog();
					_result.SetResult(result);
					void abort(object sender, EventArgs e)
					{
						AbortThread();
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

		private void AbortThread()
		{
		}

		public Task<DialogResult> ShowAsync()
		{
			_thread.Start();
			return _result.Task;
		}
	}
}
