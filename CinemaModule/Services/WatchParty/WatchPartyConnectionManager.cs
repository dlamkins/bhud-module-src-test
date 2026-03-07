using System;
using System.Net;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaModule.Services.WatchParty
{
	public sealed class WatchPartyConnectionManager : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WatchPartyConnectionManager>();

		private const string HubUrl = "https://stream.gw2music.com/hubs/watchparty";

		private static readonly TimeSpan[] ReconnectDelays = new TimeSpan[5]
		{
			TimeSpan.FromSeconds(0.0),
			TimeSpan.FromSeconds(2.0),
			TimeSpan.FromSeconds(5.0),
			TimeSpan.FromSeconds(10.0),
			TimeSpan.FromSeconds(30.0)
		};

		private HubConnection _connection;

		private bool _isDisposed;

		private Action<HubConnection> _handlerRegistration;

		private static bool _tlsConfigured;

		public HubConnection Connection => _connection;

		public bool IsConnected
		{
			get
			{
				HubConnection connection = _connection;
				if (connection == null)
				{
					return false;
				}
				return connection.State == HubConnectionState.Connected;
			}
		}

		public event Func<Exception, Task> ConnectionClosed;

		public event Func<Exception, Task> ConnectionReconnecting;

		public event Func<string, Task> ConnectionReconnected;

		public void SetHandlerRegistration(Action<HubConnection> registration)
		{
			_handlerRegistration = registration;
		}

		public async Task EnsureConnectedAsync()
		{
			if (!IsConnected)
			{
				ConfigureTls();
				if (_connection != null)
				{
					await DisposeConnectionAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				_connection = new HubConnectionBuilder().WithUrl("https://stream.gw2music.com/hubs/watchparty").AddNewtonsoftJsonProtocol().WithAutomaticReconnect(ReconnectDelays)
					.Build();
				_connection.Closed += OnConnectionClosed;
				_connection.Reconnecting += OnConnectionReconnecting;
				_connection.Reconnected += OnConnectionReconnected;
				_handlerRegistration?.Invoke(_connection);
				try
				{
					Logger.Info("Connecting to watch party server...");
					await _connection.StartAsync().ConfigureAwait(continueOnCapturedContext: false);
					Logger.Info("Connected to watch party server");
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to connect to watch party server");
					await DisposeConnectionAsync().ConfigureAwait(continueOnCapturedContext: false);
					throw new InvalidOperationException("Unable to reach the watch party server.", ex);
				}
			}
		}

		public async Task<T> InvokeAsync<T>(string method, params object[] args)
		{
			HubConnection connection = _connection;
			if (connection == null || connection.State != HubConnectionState.Connected)
			{
				throw new InvalidOperationException("Not connected to server.");
			}
			try
			{
				return await connection.InvokeCoreAsync<T>(method, args).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Hub invoke failed: {method}", new object[1] { method });
				throw;
			}
		}

		public async Task InvokeAsync(string method, params object[] args)
		{
			HubConnection connection = _connection;
			if (connection == null || connection.State != HubConnectionState.Connected)
			{
				throw new InvalidOperationException("Not connected to server.");
			}
			try
			{
				await connection.InvokeCoreAsync(method, args).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Hub invoke failed: {method}", new object[1] { method });
				throw;
			}
		}

		private Task OnConnectionClosed(Exception error)
		{
			if (_isDisposed)
			{
				return Task.CompletedTask;
			}
			if (error != null)
			{
				Logger.Error(error, "Connection closed unexpectedly");
			}
			else
			{
				Logger.Info("Connection closed");
			}
			return this.ConnectionClosed?.Invoke(error) ?? Task.CompletedTask;
		}

		private Task OnConnectionReconnecting(Exception error)
		{
			if (_isDisposed)
			{
				return Task.CompletedTask;
			}
			Logger.Warn("Connection lost, reconnecting...");
			return this.ConnectionReconnecting?.Invoke(error) ?? Task.CompletedTask;
		}

		private Task OnConnectionReconnected(string connectionId)
		{
			if (_isDisposed)
			{
				return Task.CompletedTask;
			}
			Logger.Info("Reconnected to watch party server");
			return this.ConnectionReconnected?.Invoke(connectionId) ?? Task.CompletedTask;
		}

		private static void ConfigureTls()
		{
			if (!_tlsConfigured)
			{
				_tlsConfigured = true;
				try
				{
					ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to configure TLS");
				}
			}
		}

		private async Task DisposeConnectionAsync()
		{
			HubConnection connection = _connection;
			if (connection != null)
			{
				_connection = null;
				connection.Closed -= OnConnectionClosed;
				connection.Reconnecting -= OnConnectionReconnecting;
				connection.Reconnected -= OnConnectionReconnected;
				try
				{
					await connection.StopAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex2)
				{
					Logger.Debug(ex2, "Error stopping connection during dispose");
				}
				try
				{
					await connection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Error disposing connection");
				}
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				HubConnection connection = _connection;
				if (connection != null)
				{
					_connection = null;
					connection.Closed -= OnConnectionClosed;
					connection.Reconnecting -= OnConnectionReconnecting;
					connection.Reconnected -= OnConnectionReconnected;
					DisposeConnectionFireAndForgetAsync(connection);
				}
			}
		}

		private static async Task DisposeConnectionFireAndForgetAsync(HubConnection connection)
		{
			try
			{
				await connection.StopAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex2)
			{
				Logger.Debug(ex2, "Error stopping connection during fire-and-forget dispose");
			}
			try
			{
				await connection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Error disposing connection during fire-and-forget dispose");
			}
		}
	}
}
