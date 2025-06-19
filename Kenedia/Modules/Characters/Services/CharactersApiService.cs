using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kenedia.Modules.Characters.Services
{
	public class CharactersApiService
	{
		private readonly HttpListener _listener;

		private CancellationTokenSource _cts;

		private string HostUrl = "http://localhost:5001/";

		public CharactersApiService(string? url = null)
		{
			if (url != null)
			{
				HostUrl = url;
			}
			_listener = new HttpListener();
			_listener.Prefixes.Add(HostUrl);
		}

		public void Start()
		{
			_cts = new CancellationTokenSource();
			_listener.Start();
			Task.Run(() => RunServerLoop(_cts.Token));
		}

		public void Stop()
		{
			_cts.Cancel();
			_listener.Stop();
		}

		private async Task RunServerLoop(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					HttpListenerContext context = await _listener.GetContextAsync();
					using StreamReader reader = new StreamReader(context.Request.InputStream);
					await reader.ReadToEndAsync();
					await Task.Delay(3000, cancellationToken);
					string jsonResponse = JsonSerializer.Serialize(new
					{
						message = "Processing complete",
						timestamp = DateTime.UtcNow
					});
					byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
					context.Response.ContentType = "application/json";
					context.Response.ContentLength64 = buffer.Length;
					await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
					context.Response.Close();
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
