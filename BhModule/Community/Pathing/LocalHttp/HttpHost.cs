using System;
using System.Net;
using System.Threading;

namespace BhModule.Community.Pathing.LocalHttp
{
	public class HttpHost
	{
		private readonly HttpListener _listener;

		private readonly RouteFactory _router;

		private readonly int _threads;

		private int _threadGeneration;

		public HttpHost(int port, int threads = 4)
		{
			_threads = threads;
			ServicePointManager.DefaultConnectionLimit = 100;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.MaxServicePoints = 100;
			_listener = new HttpListener();
			_router = new RouteFactory();
			_listener.Prefixes.Add($"http://localhost:{port}/");
		}

		public void Start()
		{
			_listener.Start();
			for (int i = 0; i < _threads; i++)
			{
				Thread thread = new Thread(ListenerThread);
				thread.Priority = ThreadPriority.Normal;
				thread.IsBackground = true;
				thread.Start();
			}
		}

		public void Close()
		{
			_threadGeneration++;
			_listener.Close();
		}

		private void ListenerThread()
		{
			int threadGeneration = _threadGeneration;
			while (threadGeneration == _threadGeneration)
			{
				_listener.BeginGetContext(ListenerCallback, _listener).AsyncWaitHandle.WaitOne();
			}
		}

		private async void ListenerCallback(IAsyncResult result)
		{
			try
			{
				HttpListener listener = result.AsyncState as HttpListener;
				if (listener != null)
				{
					HttpListenerContext context = listener.EndGetContext(result);
					await _router.HandleRequest(context);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
