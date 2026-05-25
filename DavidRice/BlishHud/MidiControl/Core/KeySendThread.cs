using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public class KeySendThread : IDisposable
	{
		private readonly BlockingCollection<SendAction> _queue;

		private readonly Thread _thread;

		private readonly Action<uint> _sendTap;

		private bool _disposed;

		public bool IsAlive => _thread.IsAlive;

		public KeySendThread(Action<uint> sendTap)
		{
			_sendTap = sendTap;
			_queue = new BlockingCollection<SendAction>();
			_thread = new Thread(Run)
			{
				IsBackground = true,
				Name = "MIDI Key Send"
			};
		}

		public void Start()
		{
			_thread.Start();
		}

		public void Enqueue(SendAction action)
		{
			if (!_disposed && !_queue.IsAddingCompleted)
			{
				try
				{
					_queue.Add(action);
				}
				catch (InvalidOperationException)
				{
				}
			}
		}

		public void Shutdown(TimeSpan? timeout = null)
		{
			if (!_disposed)
			{
				_queue.CompleteAdding();
				_disposed = true;
				TimeSpan wait = timeout ?? TimeSpan.FromSeconds(5.0);
				_thread.Join(wait);
			}
		}

		public void Dispose()
		{
			Shutdown();
		}

		private void Run()
		{
			foreach (SendAction action in _queue.GetConsumingEnumerable())
			{
				_sendTap(action.ScanCode);
				if (action.DelayAfterMs > 0)
				{
					Thread.Sleep(action.DelayAfterMs);
				}
			}
		}
	}
}
