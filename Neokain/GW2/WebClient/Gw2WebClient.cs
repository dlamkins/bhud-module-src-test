using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Alliances.Tags;
using Neokain.GW2.WebClient.Models.Bans;
using Neokain.GW2.WebClient.Models.Connection;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Guilds.Stash;
using Neokain.GW2.WebClient.Models.Maps;
using Neokain.GW2.WebClient.Models.Server;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Tasks;

namespace Neokain.GW2.WebClient
{
	public sealed class Gw2WebClient : IGw2WebClient, IGw2HubReceiver, IDisposable, IGw2HubServer, IGw2HubServerAccount, IGw2HubServerGuild, IGw2HubServerAlliance, IGw2HubServerMap, IGw2HubServerConnection
	{
		private sealed class WebClientLoggerBridge : ILoggerProvider, IDisposable
		{
			private sealed class BridgeLogger : ILogger
			{
				private readonly string _category;

				public BridgeLogger(string category)
				{
					_category = category;
				}

				public IDisposable BeginScope<TState>(TState state)
				{
					return null;
				}

				public bool IsEnabled(LogLevel logLevel)
				{
					if (Logger != null)
					{
						return logLevel >= LogLevel.Information;
					}
					return false;
				}

				public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
				{
					if (IsEnabled(logLevel))
					{
						string text = $"[{logLevel}] {_category}: {formatter(state, exception)}";
						if (exception != null)
						{
							Logger?.LogError(text, exception);
						}
						else
						{
							Logger?.LogEvent("SignalR", text);
						}
					}
				}
			}

			public ILogger CreateLogger(string categoryName)
			{
				return new BridgeLogger(categoryName);
			}

			public void Dispose()
			{
			}
		}

		private const int DefaultTimeoutSeconds = 30;

		private static readonly TimeSpan ClosedLoopAttemptTimeout = TimeSpan.FromMinutes(2.0);

		private HubConnection _connection;

		private Func<Exception, Task> _closedHandler;

		private CancellationTokenSource _lifecycleCts;

		private string _hubEndpoint;

		private string _accessToken;

		private string _clientVersion;

		private string _clientType = "BlishHUD";

		private AuthenticationSource _authenticationSource = AuthenticationSource.None;

		private volatile bool _explicitStop;

		private HubConnection _loopOwner;

		private int _resyncRunning;

		public static IWebClientLogger? Logger { get; set; }

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

		public ConnectionState State { get; private set; }

		public AuthenticationSource AuthenticationSource => _authenticationSource;

		public bool IsVerified
		{
			get
			{
				if (IsConnected)
				{
					return _authenticationSource == AuthenticationSource.CustomApiKey;
				}
				return false;
			}
		}

		public ServerHandshakeResponseDto? HandshakeResponse { get; private set; }

		public AccountDataDto? CurrentAccount { get; private set; }

		public IReadOnlyList<GuildMembershipDto>? CurrentGuilds { get; private set; }

		public IReadOnlyList<AllianceMembershipDto>? CurrentAlliances { get; private set; }

		public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

		public event EventHandler<RetryScheduledEventArgs>? RetryScheduled;

		public event EventHandler<Exception>? Error;

		public event EventHandler<AuthenticatedEventArgs>? Authenticated;

		public event EventHandler<CompatibilityFailedEventArgs>? CompatibilityFailed;

		public event EventHandler<GuildSpamAddedEventArgs>? GuildSpamAdded;

		public event EventHandler<GuildSpamChangedEventArgs>? GuildSpamChanged;

		public event EventHandler<GuildSpamRemovedEventArgs>? GuildSpamRemoved;

		public event EventHandler<GuildSpamUsedEventArgs>? GuildSpamUsed;

		public event EventHandler<GuildBanAddedEventArgs>? GuildBanAdded;

		public event EventHandler<GuildBanChangedEventArgs>? GuildBanChanged;

		public event EventHandler<GuildBanRemovedEventArgs>? GuildBanRemoved;

		public event EventHandler<AllianceSpamAddedEventArgs>? AllianceSpamAdded;

		public event EventHandler<AllianceSpamChangedEventArgs>? AllianceSpamChanged;

		public event EventHandler<AllianceSpamRemovedEventArgs>? AllianceSpamRemoved;

		public event EventHandler<AllianceSpamUsedEventArgs>? AllianceSpamUsed;

		public event EventHandler<AllianceBanAddedEventArgs>? AllianceBanAdded;

		public event EventHandler<AllianceBanChangedEventArgs>? AllianceBanChanged;

		public event EventHandler<AllianceBanRemovedEventArgs>? AllianceBanRemoved;

		public event EventHandler<AllianceTagTypeAddedEventArgs>? AllianceTagTypeAdded;

		public event EventHandler<AllianceTagTypeChangedEventArgs>? AllianceTagTypeChanged;

		public event EventHandler<AllianceTagTypeRemovedEventArgs>? AllianceTagTypeRemoved;

		public event EventHandler<AllianceMemberTagAddedEventArgs>? AllianceMemberTagAdded;

		public event EventHandler<AllianceMemberTagChangedEventArgs>? AllianceMemberTagChanged;

		public event EventHandler<AllianceMemberTagRevokedEventArgs>? AllianceMemberTagRevoked;

		public event EventHandler<TaskProgressUpdatedEventArgs>? TaskProgressUpdated;

		public event EventHandler<TaskCompletedEventArgs>? TaskCompleted;

		public event EventHandler<TaskFailedEventArgs>? TaskFailed;

		public event EventHandler<TaskCancelledEventArgs>? TaskCancelled;

		public event EventHandler<SpamFavoriteAddedEventArgs>? SpamFavoriteAdded;

		public event EventHandler<SpamFavoriteRemovedEventArgs>? SpamFavoriteRemoved;

		public event EventHandler<SpamFavoritesReorderedEventArgs>? SpamFavoritesReordered;

		public event EventHandler<GuildSpamCategoryAddedEventArgs>? GuildSpamCategoryAdded;

		public event EventHandler<GuildSpamCategoryChangedEventArgs>? GuildSpamCategoryChanged;

		public event EventHandler<GuildSpamCategoryRemovedEventArgs>? GuildSpamCategoryRemoved;

		public event EventHandler<AllianceSpamCategoryAddedEventArgs>? AllianceSpamCategoryAdded;

		public event EventHandler<AllianceSpamCategoryChangedEventArgs>? AllianceSpamCategoryChanged;

		public event EventHandler<AllianceSpamCategoryRemovedEventArgs>? AllianceSpamCategoryRemoved;

		private void RegisterReceiverEvents()
		{
			_connection.On("GuildSpamAdded", delegate(Guid guildId, SpamDetailDto spam)
			{
				Logger?.LogEvent("GuildSpamAdded", new { guildId, spam });
				this.GuildSpamAdded?.Invoke(this, new GuildSpamAddedEventArgs(guildId, spam));
			});
			_connection.On("GuildSpamChanged", delegate(Guid guildId, SpamDetailDto spam)
			{
				Logger?.LogEvent("GuildSpamChanged", new { guildId, spam });
				this.GuildSpamChanged?.Invoke(this, new GuildSpamChangedEventArgs(guildId, spam));
			});
			_connection.On("GuildSpamRemoved", delegate(Guid guildId, Guid spamId)
			{
				Logger?.LogEvent("GuildSpamRemoved", new { guildId, spamId });
				this.GuildSpamRemoved?.Invoke(this, new GuildSpamRemovedEventArgs(guildId, spamId));
			});
			_connection.On("GuildSpamUsed", delegate(Guid guildId, SpamUsedNotificationDto notification)
			{
				Logger?.LogEvent("GuildSpamUsed", new { guildId, notification });
				this.GuildSpamUsed?.Invoke(this, new GuildSpamUsedEventArgs(guildId, notification));
			});
			_connection.On("GuildBanAdded", delegate(Guid guildId, BanDto ban)
			{
				Logger?.LogEvent("GuildBanAdded", new { guildId, ban });
				this.GuildBanAdded?.Invoke(this, new GuildBanAddedEventArgs(guildId, ban));
			});
			_connection.On("GuildBanChanged", delegate(Guid guildId, BanDto ban)
			{
				Logger?.LogEvent("GuildBanChanged", new { guildId, ban });
				this.GuildBanChanged?.Invoke(this, new GuildBanChangedEventArgs(guildId, ban));
			});
			_connection.On("GuildBanRemoved", delegate(Guid guildId, Guid banId)
			{
				Logger?.LogEvent("GuildBanRemoved", new { guildId, banId });
				this.GuildBanRemoved?.Invoke(this, new GuildBanRemovedEventArgs(guildId, banId));
			});
			_connection.On("AllianceSpamAdded", delegate(Guid allianceId, SpamDetailDto spam)
			{
				Logger?.LogEvent("AllianceSpamAdded", new { allianceId, spam });
				this.AllianceSpamAdded?.Invoke(this, new AllianceSpamAddedEventArgs(allianceId, spam));
			});
			_connection.On("AllianceSpamChanged", delegate(Guid allianceId, SpamDetailDto spam)
			{
				Logger?.LogEvent("AllianceSpamChanged", new { allianceId, spam });
				this.AllianceSpamChanged?.Invoke(this, new AllianceSpamChangedEventArgs(allianceId, spam));
			});
			_connection.On("AllianceSpamRemoved", delegate(Guid allianceId, Guid spamId)
			{
				Logger?.LogEvent("AllianceSpamRemoved", new { allianceId, spamId });
				this.AllianceSpamRemoved?.Invoke(this, new AllianceSpamRemovedEventArgs(allianceId, spamId));
			});
			_connection.On("AllianceSpamUsed", delegate(Guid allianceId, SpamUsedNotificationDto notification)
			{
				Logger?.LogEvent("AllianceSpamUsed", new { allianceId, notification });
				this.AllianceSpamUsed?.Invoke(this, new AllianceSpamUsedEventArgs(allianceId, notification));
			});
			_connection.On("AllianceBanAdded", delegate(Guid allianceId, BanDto ban)
			{
				Logger?.LogEvent("AllianceBanAdded", new { allianceId, ban });
				this.AllianceBanAdded?.Invoke(this, new AllianceBanAddedEventArgs(allianceId, ban));
			});
			_connection.On("AllianceBanChanged", delegate(Guid allianceId, BanDto ban)
			{
				Logger?.LogEvent("AllianceBanChanged", new { allianceId, ban });
				this.AllianceBanChanged?.Invoke(this, new AllianceBanChangedEventArgs(allianceId, ban));
			});
			_connection.On("AllianceBanRemoved", delegate(Guid allianceId, Guid banId)
			{
				Logger?.LogEvent("AllianceBanRemoved", new { allianceId, banId });
				this.AllianceBanRemoved?.Invoke(this, new AllianceBanRemovedEventArgs(allianceId, banId));
			});
			_connection.On("AllianceTagTypeAdded", delegate(Guid allianceId, AllianceTagTypeDto tagType)
			{
				Logger?.LogEvent("AllianceTagTypeAdded", new { allianceId, tagType });
				this.AllianceTagTypeAdded?.Invoke(this, new AllianceTagTypeAddedEventArgs(allianceId, tagType));
			});
			_connection.On("AllianceTagTypeChanged", delegate(Guid allianceId, AllianceTagTypeDto tagType)
			{
				Logger?.LogEvent("AllianceTagTypeChanged", new { allianceId, tagType });
				this.AllianceTagTypeChanged?.Invoke(this, new AllianceTagTypeChangedEventArgs(allianceId, tagType));
			});
			_connection.On("AllianceTagTypeRemoved", delegate(Guid allianceId, Guid tagTypeId)
			{
				Logger?.LogEvent("AllianceTagTypeRemoved", new { allianceId, tagTypeId });
				this.AllianceTagTypeRemoved?.Invoke(this, new AllianceTagTypeRemovedEventArgs(allianceId, tagTypeId));
			});
			_connection.On("AllianceMemberTagAdded", delegate(Guid allianceId, AllianceMemberTagDto memberTag)
			{
				Logger?.LogEvent("AllianceMemberTagAdded", new { allianceId, memberTag });
				this.AllianceMemberTagAdded?.Invoke(this, new AllianceMemberTagAddedEventArgs(allianceId, memberTag));
			});
			_connection.On("AllianceMemberTagChanged", delegate(Guid allianceId, AllianceMemberTagDto memberTag)
			{
				Logger?.LogEvent("AllianceMemberTagChanged", new { allianceId, memberTag });
				this.AllianceMemberTagChanged?.Invoke(this, new AllianceMemberTagChangedEventArgs(allianceId, memberTag));
			});
			_connection.On("AllianceMemberTagRevoked", delegate(Guid allianceId, Guid tagId)
			{
				Logger?.LogEvent("AllianceMemberTagRevoked", new { allianceId, tagId });
				this.AllianceMemberTagRevoked?.Invoke(this, new AllianceMemberTagRevokedEventArgs(allianceId, tagId));
			});
			_connection.On("TaskProgressUpdated", delegate(Guid taskId, TaskProgressDto progress)
			{
				Logger?.LogEvent("TaskProgressUpdated", new { taskId, progress });
				this.TaskProgressUpdated?.Invoke(this, new TaskProgressUpdatedEventArgs(taskId, progress));
			});
			_connection.On("TaskCompleted", delegate(Guid taskId, TaskDto task)
			{
				Logger?.LogEvent("TaskCompleted", new { taskId, task });
				this.TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(taskId, task));
			});
			_connection.On("TaskFailed", delegate(Guid taskId, TaskDto task)
			{
				Logger?.LogEvent("TaskFailed", new { taskId, task });
				this.TaskFailed?.Invoke(this, new TaskFailedEventArgs(taskId, task));
			});
			_connection.On("TaskCancelled", delegate(Guid taskId)
			{
				Logger?.LogEvent("TaskCancelled", new { taskId });
				this.TaskCancelled?.Invoke(this, new TaskCancelledEventArgs(taskId));
			});
			_connection.On("SpamFavoriteAdded", delegate(Guid accountId, SpamFavoriteDto favorite)
			{
				Logger?.LogEvent("SpamFavoriteAdded", new { accountId, favorite });
				this.SpamFavoriteAdded?.Invoke(this, new SpamFavoriteAddedEventArgs(accountId, favorite));
			});
			_connection.On("SpamFavoriteRemoved", delegate(Guid accountId, Guid favoriteId)
			{
				Logger?.LogEvent("SpamFavoriteRemoved", new { accountId, favoriteId });
				this.SpamFavoriteRemoved?.Invoke(this, new SpamFavoriteRemovedEventArgs(accountId, favoriteId));
			});
			_connection.On("SpamFavoritesReordered", delegate(Guid accountId)
			{
				Logger?.LogEvent("SpamFavoritesReordered", new { accountId });
				this.SpamFavoritesReordered?.Invoke(this, new SpamFavoritesReorderedEventArgs(accountId));
			});
			_connection.On("GuildSpamCategoryAdded", delegate(Guid guildId, SpamCategoryDto category)
			{
				Logger?.LogEvent("GuildSpamCategoryAdded", new { guildId, category });
				this.GuildSpamCategoryAdded?.Invoke(this, new GuildSpamCategoryAddedEventArgs(guildId, category));
			});
			_connection.On("GuildSpamCategoryChanged", delegate(Guid guildId, SpamCategoryDto category)
			{
				Logger?.LogEvent("GuildSpamCategoryChanged", new { guildId, category });
				this.GuildSpamCategoryChanged?.Invoke(this, new GuildSpamCategoryChangedEventArgs(guildId, category));
			});
			_connection.On("GuildSpamCategoryRemoved", delegate(Guid guildId, Guid categoryId)
			{
				Logger?.LogEvent("GuildSpamCategoryRemoved", new { guildId, categoryId });
				this.GuildSpamCategoryRemoved?.Invoke(this, new GuildSpamCategoryRemovedEventArgs(guildId, categoryId));
			});
			_connection.On("AllianceSpamCategoryAdded", delegate(Guid allianceId, SpamCategoryDto category)
			{
				Logger?.LogEvent("AllianceSpamCategoryAdded", new { allianceId, category });
				this.AllianceSpamCategoryAdded?.Invoke(this, new AllianceSpamCategoryAddedEventArgs(allianceId, category));
			});
			_connection.On("AllianceSpamCategoryChanged", delegate(Guid allianceId, SpamCategoryDto category)
			{
				Logger?.LogEvent("AllianceSpamCategoryChanged", new { allianceId, category });
				this.AllianceSpamCategoryChanged?.Invoke(this, new AllianceSpamCategoryChangedEventArgs(allianceId, category));
			});
			_connection.On("AllianceSpamCategoryRemoved", delegate(Guid allianceId, Guid categoryId)
			{
				Logger?.LogEvent("AllianceSpamCategoryRemoved", new { allianceId, categoryId });
				this.AllianceSpamCategoryRemoved?.Invoke(this, new AllianceSpamCategoryRemovedEventArgs(allianceId, categoryId));
			});
		}

		public async Task InitializeAsync(string hubEndpoint, string accessToken)
		{
			if (string.IsNullOrWhiteSpace(hubEndpoint))
			{
				throw new ArgumentException("Hub endpoint cannot be null or empty", "hubEndpoint");
			}
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				throw new ArgumentException("Access token cannot be null or empty", "accessToken");
			}
			await DisposeConnectionAsync().ConfigureAwait(continueOnCapturedContext: false);
			_hubEndpoint = NormalizeEndpoint(hubEndpoint);
			_accessToken = accessToken;
			_authenticationSource = DetectAuthenticationSource(accessToken);
			_explicitStop = false;
			SetState(ConnectionState.Connecting);
			try
			{
				CancellationTokenSource cancellationTokenSource = (_lifecycleCts = new CancellationTokenSource());
				HubConnection connection = new HubConnectionBuilder().WithUrl(_hubEndpoint, delegate(HttpConnectionOptions options)
				{
					options.Transports = HttpTransportType.WebSockets;
					options.AccessTokenProvider = () => Task.FromResult(_accessToken);
				}).ConfigureLogging(delegate(ILoggingBuilder logging)
				{
					logging.SetMinimumLevel(LogLevel.Information);
					logging.AddProvider(new WebClientLoggerBridge());
				}).Build();
				CancellationToken lifecycleToken = cancellationTokenSource.Token;
				Func<Exception, Task> func = (Exception error) => OnClosedAsync(connection, lifecycleToken, error);
				connection.Closed += func;
				_connection = connection;
				_closedHandler = func;
				RegisterReceiverEvents();
				await connection.StartAsync().ConfigureAwait(continueOnCapturedContext: false);
				SetState(ConnectionState.Connected);
			}
			catch (Exception error2)
			{
				RaiseError(error2);
				await DisposeConnectionAsync().ConfigureAwait(continueOnCapturedContext: false);
				SetState(ConnectionState.Disconnected);
				throw;
			}
		}

		internal static string NormalizeEndpoint(string url)
		{
			string text = url.Trim();
			if (text.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
			{
				return "https://" + text.Substring(6);
			}
			if (text.StartsWith("ws://", StringComparison.OrdinalIgnoreCase))
			{
				return "http://" + text.Substring(5);
			}
			if (text.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
			{
				return text;
			}
			return "https://" + text;
		}

		public async Task DisconnectAsync()
		{
			_explicitStop = true;
			CancelLifecycle();
			HubConnection connection = _connection;
			if (connection != null)
			{
				try
				{
					await connection.StopAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger?.LogError("DisconnectAsync", ex);
				}
			}
			SetState(ConnectionState.Disconnected);
		}

		private async Task DisposeConnectionAsync()
		{
			CancelLifecycle();
			HubConnection connection = _connection;
			Func<Exception, Task> closedHandler = _closedHandler;
			_connection = null;
			_closedHandler = null;
			if (connection != null)
			{
				if (closedHandler != null)
				{
					connection.Closed -= closedHandler;
				}
				try
				{
					await connection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger?.LogError("DisposeConnection", ex);
				}
			}
		}

		private void CancelLifecycle()
		{
			CancellationTokenSource lifecycleCts = _lifecycleCts;
			_lifecycleCts = null;
			if (lifecycleCts != null)
			{
				try
				{
					lifecycleCts.Cancel();
				}
				catch
				{
				}
				try
				{
					lifecycleCts.Dispose();
				}
				catch
				{
				}
			}
		}

		private void SetState(ConnectionState state)
		{
			if (State != state)
			{
				State = state;
				try
				{
					this.ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(state, _authenticationSource));
				}
				catch (Exception ex)
				{
					Logger?.LogError("ConnectionStateChangedHandler", ex);
				}
			}
		}

		private void RaiseError(Exception error)
		{
			try
			{
				this.Error?.Invoke(this, error);
			}
			catch (Exception ex)
			{
				Logger?.LogError("ErrorHandler", ex);
			}
		}

		private void RaiseRetryScheduled(TimeSpan delay)
		{
			try
			{
				this.RetryScheduled?.Invoke(this, new RetryScheduledEventArgs(DateTimeOffset.UtcNow + delay));
			}
			catch (Exception ex)
			{
				Logger?.LogError("RetryScheduledHandler", ex);
			}
		}

		private AuthenticationSource DetectAuthenticationSource(string accessToken)
		{
			if (accessToken.StartsWith("am_", StringComparison.Ordinal))
			{
				return AuthenticationSource.CustomApiKey;
			}
			if (Guid.TryParse(accessToken, out var _))
			{
				return AuthenticationSource.CustomApiKey;
			}
			return AuthenticationSource.Gw2ApiSubtoken;
		}

		private async Task OnClosedAsync(HubConnection captured, CancellationToken lifecycleToken, Exception error)
		{
			if (error != null)
			{
				Logger?.LogError("ConnectionClosed", error);
				RaiseError(error);
			}
			if (_connection != captured)
			{
				return;
			}
			if (_explicitStop)
			{
				SetState(ConnectionState.Disconnected);
				return;
			}
			SetState(ConnectionState.Reconnecting);
			while (Interlocked.CompareExchange(ref _loopOwner, captured, null) != null)
			{
				if (_loopOwner == captured)
				{
					return;
				}
				try
				{
					await Task.Delay(50, lifecycleToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
				}
				if (_explicitStop || _connection != captured)
				{
					return;
				}
			}
			try
			{
				int attempt = 0;
				while (!_explicitStop && _connection == captured)
				{
					TimeSpan[] effectiveDelays = InfiniteRetryPolicy.EffectiveDelays;
					TimeSpan timeSpan = effectiveDelays[Math.Min(attempt, effectiveDelays.Length - 1)];
					attempt++;
					if (timeSpan > TimeSpan.Zero)
					{
						RaiseRetryScheduled(timeSpan);
						try
						{
							await Task.Delay(timeSpan, lifecycleToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						catch (OperationCanceledException)
						{
						}
					}
					if (_explicitStop || _connection != captured)
					{
						break;
					}
					try
					{
						using (CancellationTokenSource attemptCts = CancellationTokenSource.CreateLinkedTokenSource(lifecycleToken))
						{
							attemptCts.CancelAfter(ClosedLoopAttemptTimeout);
							await captured.StartAsync(attemptCts.Token).ConfigureAwait(continueOnCapturedContext: false);
						}
						if (_explicitStop)
						{
							try
							{
								await captured.StopAsync().ConfigureAwait(continueOnCapturedContext: false);
							}
							catch (Exception ex3)
							{
								Logger?.LogError("ClosedRestartStop", ex3);
							}
							SetState(ConnectionState.Disconnected);
							return;
						}
						if (_connection != captured)
						{
							return;
						}
						SetState(ConnectionState.Connected);
						await HandshakeAndResyncWithRetryAsync(captured, lifecycleToken).ConfigureAwait(continueOnCapturedContext: false);
						if (_explicitStop)
						{
							SetState(ConnectionState.Disconnected);
							return;
						}
						if (_connection != captured || captured.State == HubConnectionState.Connected)
						{
							return;
						}
						SetState(ConnectionState.Reconnecting);
					}
					catch (OperationCanceledException) when (lifecycleToken.IsCancellationRequested)
					{
					}
					catch (Exception ex5)
					{
						if (_connection != captured)
						{
							break;
						}
						Logger?.LogError("ClosedRestartAttempt", ex5);
					}
				}
				if (_explicitStop && _connection == captured)
				{
					SetState(ConnectionState.Disconnected);
				}
			}
			finally
			{
				Interlocked.Exchange(ref _loopOwner, null);
			}
		}

		public async Task<ServerHandshakeResponseDto> PerformHandshakeAsync(string clientVersion, string clientType = "BlishHUD")
		{
			if (!IsConnected)
			{
				throw new InvalidOperationException("Connection must be established before performing handshake.");
			}
			ClientHandshakeDto clientHandshakeDto = new ClientHandshakeDto
			{
				ClientVersion = clientVersion,
				ClientType = clientType
			};
			HandshakeResponse = await InvokeAsync<ServerHandshakeResponseDto>("Handshake", new object[1] { clientHandshakeDto }, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			return HandshakeResponse;
		}

		public async Task InitializeAndAuthenticateAsync(string hubEndpoint, string accessToken, string clientVersion, string clientType = "BlishHUD")
		{
			_clientVersion = clientVersion;
			_clientType = clientType;
			await InitializeAsync(hubEndpoint, accessToken).ConfigureAwait(continueOnCapturedContext: false);
			await HandshakeAndResyncAsync(clientVersion, clientType).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task HandshakeAndResyncAsync(string clientVersion, string clientType)
		{
			ServerHandshakeResponseDto serverHandshakeResponseDto = await PerformHandshakeAsync(clientVersion, clientType).ConfigureAwait(continueOnCapturedContext: false);
			if (!serverHandshakeResponseDto.CurrentVersionCompatible)
			{
				CompatibilityFailedEventArgs compatibilityFailedEventArgs = new CompatibilityFailedEventArgs(serverHandshakeResponseDto);
				try
				{
					this.CompatibilityFailed?.Invoke(this, compatibilityFailedEventArgs);
				}
				catch (Exception ex)
				{
					Logger?.LogError("CompatibilityFailedHandler", ex);
				}
				throw new IncompatibleVersionException(compatibilityFailedEventArgs.CompatibilityMessage);
			}
			CurrentAccount = await GetMyAccount().ConfigureAwait(continueOnCapturedContext: false);
			if (CurrentAccount == null)
			{
				throw new HubException("Failed to get account data");
			}
			Task<List<GuildMembershipDto>> guildsTask = GetGuilds(CurrentAccount!.Id);
			Task<List<AllianceMembershipDto>> alliancesTask = GetAccountAllianceMemberships(CurrentAccount!.Id);
			await Task.WhenAll(guildsTask, alliancesTask).ConfigureAwait(continueOnCapturedContext: false);
			CurrentGuilds = (await guildsTask.ConfigureAwait(continueOnCapturedContext: false))?.ToList().AsReadOnly();
			CurrentAlliances = (await alliancesTask.ConfigureAwait(continueOnCapturedContext: false))?.ToList().AsReadOnly();
			AuthenticatedEventArgs e = new AuthenticatedEventArgs(CurrentAccount, CurrentGuilds ?? Array.Empty<GuildMembershipDto>(), CurrentAlliances ?? Array.Empty<AllianceMembershipDto>());
			try
			{
				this.Authenticated?.Invoke(this, e);
			}
			catch (Exception ex2)
			{
				Logger?.LogError("AuthenticatedHandler", ex2);
			}
		}

		private async Task HandshakeAndResyncWithRetryAsync(HubConnection captured, CancellationToken lifecycleToken)
		{
			if (_clientVersion == null || Interlocked.CompareExchange(ref _resyncRunning, 1, 0) != 0)
			{
				return;
			}
			try
			{
				int attempt = 0;
				while (!_explicitStop && _connection == captured && captured.State == HubConnectionState.Connected)
				{
					try
					{
						await HandshakeAndResyncAsync(_clientVersion, _clientType).ConfigureAwait(continueOnCapturedContext: false);
						return;
					}
					catch (IncompatibleVersionException)
					{
						await DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
						return;
					}
					catch (Exception ex2)
					{
						if (_connection != captured)
						{
							return;
						}
						Logger?.LogError("HandshakeResync", ex2);
						RaiseError(ex2);
						TimeSpan[] effectiveDelays = InfiniteRetryPolicy.EffectiveDelays;
						TimeSpan timeSpan = effectiveDelays[Math.Min(attempt, effectiveDelays.Length - 1)];
						attempt++;
						TimeSpan delay = ((timeSpan > TimeSpan.Zero) ? timeSpan : TimeSpan.FromSeconds(2.0));
						RaiseRetryScheduled(delay);
						try
						{
							await Task.Delay(delay, lifecycleToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						catch (OperationCanceledException)
						{
							return;
						}
					}
				}
			}
			finally
			{
				Interlocked.Exchange(ref _resyncRunning, 0);
			}
		}

		internal async Task<TResult> InvokeAsync<TResult>(string method, object?[] args, CancellationToken ct)
		{
			HubConnection hubConnection = _connection ?? throw new InvalidOperationException("Client is not initialized. Call InitializeAsync first.");
			using CancellationTokenSource timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30.0));
			using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
			try
			{
				object obj = await hubConnection.InvokeCoreAsync(method, typeof(TResult), args, linkedCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				if (obj == null)
				{
					return default(TResult);
				}
				return (TResult)obj;
			}
			catch (Microsoft.AspNetCore.SignalR.HubException ex)
			{
				throw new HubException(ex.Message);
			}
			catch (OperationCanceledException) when (linkedCts.IsCancellationRequested)
			{
				throw;
			}
			catch (OperationCanceledException ex3)
			{
				throw new HubException("Connection closed: " + ex3.Message);
			}
			catch (Exception ex4) when (!IsConnected)
			{
				throw new HubException("Connection closed: " + ex4.Message);
			}
		}

		internal Task InvokeVoidAsync(string method, object?[] args, CancellationToken ct)
		{
			return InvokeAsync<object>(method, args, ct);
		}

		public void Dispose()
		{
			_explicitStop = true;
			try
			{
				DisposeConnectionAsync().Wait(TimeSpan.FromSeconds(5.0));
			}
			catch
			{
			}
			SetState(ConnectionState.Disconnected);
		}

		public async Task<AccountDataDto> GetMyAccount()
		{
			Logger?.LogCall("GetMyAccount", Array.Empty<object>());
			try
			{
				AccountDataDto result = await InvokeAsync<AccountDataDto>("GetMyAccount", Array.Empty<object>(), CancellationToken.None);
				Logger?.LogResult("GetMyAccount", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetMyAccount", ex);
				throw;
			}
		}

		public async Task<AccountDataDto> GetAccountData(Guid accountId)
		{
			Logger?.LogCall("GetAccountData", new object[1] { accountId });
			try
			{
				AccountDataDto result = await InvokeAsync<AccountDataDto>("GetAccountData", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountData", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountData", ex);
				throw;
			}
		}

		public async Task<List<SpamDto>> GetAccountSpams(Guid accountId)
		{
			Logger?.LogCall("GetAccountSpams", new object[1] { accountId });
			try
			{
				List<SpamDto> result = await InvokeAsync<List<SpamDto>>("GetAccountSpams", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountSpams", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountSpams", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> GetAccountSpam(Guid accountId, Guid spamId)
		{
			Logger?.LogCall("GetAccountSpam", new object[2] { accountId, spamId });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("GetAccountSpam", new object[2] { accountId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetAccountSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> AddAccountSpam(Guid accountId, SpamCreateDto dto)
		{
			Logger?.LogCall("AddAccountSpam", new object[2] { accountId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("AddAccountSpam", new object[2] { accountId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAccountSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAccountSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UpdateAccountSpam(Guid accountId, Guid spamId, SpamUpdateDto dto)
		{
			Logger?.LogCall("UpdateAccountSpam", new object[3] { accountId, spamId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UpdateAccountSpam", new object[3] { accountId, spamId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAccountSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAccountSpam", ex);
				throw;
			}
		}

		public async Task DeleteAccountSpam(Guid accountId, Guid spamId)
		{
			Logger?.LogCall("DeleteAccountSpam", new object[2] { accountId, spamId });
			try
			{
				await InvokeVoidAsync("DeleteAccountSpam", new object[2] { accountId, spamId }, CancellationToken.None);
				Logger?.LogResult("DeleteAccountSpam", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAccountSpam", ex);
				throw;
			}
		}

		public async Task<bool> CanUseAccountSpam(Guid accountId, Guid spamId, int? mapId)
		{
			Logger?.LogCall("CanUseAccountSpam", new object[3] { accountId, spamId, mapId });
			try
			{
				bool flag = await InvokeAsync<bool>("CanUseAccountSpam", new object[3] { accountId, spamId, mapId }, CancellationToken.None);
				Logger?.LogResult("CanUseAccountSpam", flag);
				return flag;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CanUseAccountSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UseAccountSpam(Guid accountId, Guid spamId, SpamUsageRequestDto? request)
		{
			Logger?.LogCall("UseAccountSpam", new object[3] { accountId, spamId, request });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UseAccountSpam", new object[3] { accountId, spamId, request }, CancellationToken.None);
				Logger?.LogResult("UseAccountSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UseAccountSpam", ex);
				throw;
			}
		}

		public async Task<List<SpamCategoryDto>> GetAccountSpamCategories(Guid accountId)
		{
			Logger?.LogCall("GetAccountSpamCategories", new object[1] { accountId });
			try
			{
				List<SpamCategoryDto> result = await InvokeAsync<List<SpamCategoryDto>>("GetAccountSpamCategories", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountSpamCategories", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountSpamCategories", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> AddAccountSpamCategory(Guid accountId, SpamCategoryCreateDto dto)
		{
			Logger?.LogCall("AddAccountSpamCategory", new object[2] { accountId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("AddAccountSpamCategory", new object[2] { accountId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAccountSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAccountSpamCategory", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> UpdateAccountSpamCategory(Guid accountId, Guid categoryId, SpamCategoryUpdateDto dto)
		{
			Logger?.LogCall("UpdateAccountSpamCategory", new object[3] { accountId, categoryId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("UpdateAccountSpamCategory", new object[3] { accountId, categoryId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAccountSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAccountSpamCategory", ex);
				throw;
			}
		}

		public async Task DeleteAccountSpamCategory(Guid accountId, Guid categoryId)
		{
			Logger?.LogCall("DeleteAccountSpamCategory", new object[2] { accountId, categoryId });
			try
			{
				await InvokeVoidAsync("DeleteAccountSpamCategory", new object[2] { accountId, categoryId }, CancellationToken.None);
				Logger?.LogResult("DeleteAccountSpamCategory", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAccountSpamCategory", ex);
				throw;
			}
		}

		public async Task<List<SpamMapHistoryDto>> GetAccountSpamMapHistory(Guid accountId, Guid spamId)
		{
			Logger?.LogCall("GetAccountSpamMapHistory", new object[2] { accountId, spamId });
			try
			{
				List<SpamMapHistoryDto> result = await InvokeAsync<List<SpamMapHistoryDto>>("GetAccountSpamMapHistory", new object[2] { accountId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetAccountSpamMapHistory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountSpamMapHistory", ex);
				throw;
			}
		}

		public async Task<List<SpamUsageLogDto>> GetAccountSpamUsageLogs(Guid accountId, Guid spamId, int? limit)
		{
			Logger?.LogCall("GetAccountSpamUsageLogs", new object[3] { accountId, spamId, limit });
			try
			{
				List<SpamUsageLogDto> result = await InvokeAsync<List<SpamUsageLogDto>>("GetAccountSpamUsageLogs", new object[3] { accountId, spamId, limit }, CancellationToken.None);
				Logger?.LogResult("GetAccountSpamUsageLogs", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountSpamUsageLogs", ex);
				throw;
			}
		}

		public async Task<List<BanDto>> GetAccountBans(Guid accountId)
		{
			Logger?.LogCall("GetAccountBans", new object[1] { accountId });
			try
			{
				List<BanDto> result = await InvokeAsync<List<BanDto>>("GetAccountBans", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountBans", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountBans", ex);
				throw;
			}
		}

		public async Task<BanDto> GetAccountBan(Guid accountId, Guid banId)
		{
			Logger?.LogCall("GetAccountBan", new object[2] { accountId, banId });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("GetAccountBan", new object[2] { accountId, banId }, CancellationToken.None);
				Logger?.LogResult("GetAccountBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountBan", ex);
				throw;
			}
		}

		public async Task<BanDto> AddAccountBan(Guid accountId, BanCreateDto dto)
		{
			Logger?.LogCall("AddAccountBan", new object[2] { accountId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("AddAccountBan", new object[2] { accountId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAccountBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAccountBan", ex);
				throw;
			}
		}

		public async Task<BanDto> UpdateAccountBan(Guid accountId, Guid banId, BanUpdateDto dto)
		{
			Logger?.LogCall("UpdateAccountBan", new object[3] { accountId, banId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("UpdateAccountBan", new object[3] { accountId, banId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAccountBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAccountBan", ex);
				throw;
			}
		}

		public async Task DeleteAccountBan(Guid accountId, Guid banId)
		{
			Logger?.LogCall("DeleteAccountBan", new object[2] { accountId, banId });
			try
			{
				await InvokeVoidAsync("DeleteAccountBan", new object[2] { accountId, banId }, CancellationToken.None);
				Logger?.LogResult("DeleteAccountBan", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAccountBan", ex);
				throw;
			}
		}

		public async Task<List<GuildMembershipDto>> GetAccountGuildMemberships(Guid accountId)
		{
			Logger?.LogCall("GetAccountGuildMemberships", new object[1] { accountId });
			try
			{
				List<GuildMembershipDto> result = await InvokeAsync<List<GuildMembershipDto>>("GetAccountGuildMemberships", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountGuildMemberships", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountGuildMemberships", ex);
				throw;
			}
		}

		public async Task<GuildMembershipDto> GetAccountGuildMembership(Guid accountId, Guid guildId)
		{
			Logger?.LogCall("GetAccountGuildMembership", new object[2] { accountId, guildId });
			try
			{
				GuildMembershipDto result = await InvokeAsync<GuildMembershipDto>("GetAccountGuildMembership", new object[2] { accountId, guildId }, CancellationToken.None);
				Logger?.LogResult("GetAccountGuildMembership", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountGuildMembership", ex);
				throw;
			}
		}

		public async Task<List<GuildLeadershipDto>> GetAccountGuildLeaderships(Guid accountId)
		{
			Logger?.LogCall("GetAccountGuildLeaderships", new object[1] { accountId });
			try
			{
				List<GuildLeadershipDto> result = await InvokeAsync<List<GuildLeadershipDto>>("GetAccountGuildLeaderships", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountGuildLeaderships", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountGuildLeaderships", ex);
				throw;
			}
		}

		public async Task<List<AllianceMembershipDto>> GetAccountAllianceMemberships(Guid accountId)
		{
			Logger?.LogCall("GetAccountAllianceMemberships", new object[1] { accountId });
			try
			{
				List<AllianceMembershipDto> result = await InvokeAsync<List<AllianceMembershipDto>>("GetAccountAllianceMemberships", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountAllianceMemberships", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountAllianceMemberships", ex);
				throw;
			}
		}

		public async Task<AllianceMembershipDto> GetAccountAllianceMembership(Guid accountId, Guid allianceId)
		{
			Logger?.LogCall("GetAccountAllianceMembership", new object[2] { accountId, allianceId });
			try
			{
				AllianceMembershipDto result = await InvokeAsync<AllianceMembershipDto>("GetAccountAllianceMembership", new object[2] { accountId, allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAccountAllianceMembership", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountAllianceMembership", ex);
				throw;
			}
		}

		public async Task<List<AllianceLeadershipDto>> GetAccountAllianceLeaderships(Guid accountId)
		{
			Logger?.LogCall("GetAccountAllianceLeaderships", new object[1] { accountId });
			try
			{
				List<AllianceLeadershipDto> result = await InvokeAsync<List<AllianceLeadershipDto>>("GetAccountAllianceLeaderships", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAccountAllianceLeaderships", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountAllianceLeaderships", ex);
				throw;
			}
		}

		public async Task<TaskDto> CreateAccountTask(string taskType, Guid accountId, object? payload)
		{
			Logger?.LogCall("CreateAccountTask", new object[3] { taskType, accountId, payload });
			try
			{
				TaskDto result = await InvokeAsync<TaskDto>("CreateAccountTask", new object[3] { taskType, accountId, payload }, CancellationToken.None);
				Logger?.LogResult("CreateAccountTask", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CreateAccountTask", ex);
				throw;
			}
		}

		public async Task<TaskDto> CreateGuildTask(string taskType, Guid guildId, object? payload)
		{
			Logger?.LogCall("CreateGuildTask", new object[3] { taskType, guildId, payload });
			try
			{
				TaskDto result = await InvokeAsync<TaskDto>("CreateGuildTask", new object[3] { taskType, guildId, payload }, CancellationToken.None);
				Logger?.LogResult("CreateGuildTask", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CreateGuildTask", ex);
				throw;
			}
		}

		public async Task<TaskDto> CreateAllianceTask(string taskType, Guid allianceId, object? payload)
		{
			Logger?.LogCall("CreateAllianceTask", new object[3] { taskType, allianceId, payload });
			try
			{
				TaskDto result = await InvokeAsync<TaskDto>("CreateAllianceTask", new object[3] { taskType, allianceId, payload }, CancellationToken.None);
				Logger?.LogResult("CreateAllianceTask", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CreateAllianceTask", ex);
				throw;
			}
		}

		public async Task<TaskDto?> GetTask(Guid taskId)
		{
			Logger?.LogCall("GetTask", new object[1] { taskId });
			try
			{
				TaskDto result = await InvokeAsync<TaskDto>("GetTask", new object[1] { taskId }, CancellationToken.None);
				Logger?.LogResult("GetTask", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetTask", ex);
				throw;
			}
		}

		public async Task<List<TaskDto>> GetMyTasks(int? status)
		{
			Logger?.LogCall("GetMyTasks", new object[1] { status });
			try
			{
				List<TaskDto> result = await InvokeAsync<List<TaskDto>>("GetMyTasks", new object[1] { status }, CancellationToken.None);
				Logger?.LogResult("GetMyTasks", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetMyTasks", ex);
				throw;
			}
		}

		public async Task<List<TaskDto>> GetAccountTasks(Guid accountId, int? status)
		{
			Logger?.LogCall("GetAccountTasks", new object[2] { accountId, status });
			try
			{
				List<TaskDto> result = await InvokeAsync<List<TaskDto>>("GetAccountTasks", new object[2] { accountId, status }, CancellationToken.None);
				Logger?.LogResult("GetAccountTasks", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAccountTasks", ex);
				throw;
			}
		}

		public async Task<bool> CancelTask(Guid taskId)
		{
			Logger?.LogCall("CancelTask", new object[1] { taskId });
			try
			{
				bool flag = await InvokeAsync<bool>("CancelTask", new object[1] { taskId }, CancellationToken.None);
				Logger?.LogResult("CancelTask", flag);
				return flag;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CancelTask", ex);
				throw;
			}
		}

		public async Task<List<SpamFavoriteDto>> GetSpamFavorites(Guid accountId)
		{
			Logger?.LogCall("GetSpamFavorites", new object[1] { accountId });
			try
			{
				List<SpamFavoriteDto> result = await InvokeAsync<List<SpamFavoriteDto>>("GetSpamFavorites", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetSpamFavorites", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetSpamFavorites", ex);
				throw;
			}
		}

		public async Task<SpamFavoriteDto> AddSpamFavorite(Guid accountId, SpamFavoriteCreateDto dto)
		{
			Logger?.LogCall("AddSpamFavorite", new object[2] { accountId, dto });
			try
			{
				SpamFavoriteDto result = await InvokeAsync<SpamFavoriteDto>("AddSpamFavorite", new object[2] { accountId, dto }, CancellationToken.None);
				Logger?.LogResult("AddSpamFavorite", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddSpamFavorite", ex);
				throw;
			}
		}

		public async Task RemoveSpamFavorite(Guid accountId, Guid favoriteId)
		{
			Logger?.LogCall("RemoveSpamFavorite", new object[2] { accountId, favoriteId });
			try
			{
				await InvokeVoidAsync("RemoveSpamFavorite", new object[2] { accountId, favoriteId }, CancellationToken.None);
				Logger?.LogResult("RemoveSpamFavorite", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("RemoveSpamFavorite", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UseSpamFavorite(Guid accountId, Guid favoriteId, SpamUsageRequestDto? request)
		{
			Logger?.LogCall("UseSpamFavorite", new object[3] { accountId, favoriteId, request });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UseSpamFavorite", new object[3] { accountId, favoriteId, request }, CancellationToken.None);
				Logger?.LogResult("UseSpamFavorite", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UseSpamFavorite", ex);
				throw;
			}
		}

		public async Task ReorderSpamFavorites(Guid accountId, SpamFavoriteReorderDto dto)
		{
			Logger?.LogCall("ReorderSpamFavorites", new object[2] { accountId, dto });
			try
			{
				await InvokeVoidAsync("ReorderSpamFavorites", new object[2] { accountId, dto }, CancellationToken.None);
				Logger?.LogResult("ReorderSpamFavorites", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("ReorderSpamFavorites", ex);
				throw;
			}
		}

		public async Task<List<GuildMembershipDto>> GetGuilds(Guid accountId)
		{
			Logger?.LogCall("GetGuilds", new object[1] { accountId });
			try
			{
				List<GuildMembershipDto> result = await InvokeAsync<List<GuildMembershipDto>>("GetGuilds", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetGuilds", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuilds", ex);
				throw;
			}
		}

		public async Task<GuildDetailDto> GetGuildDetail(Guid guildId)
		{
			Logger?.LogCall("GetGuildDetail", new object[1] { guildId });
			try
			{
				GuildDetailDto result = await InvokeAsync<GuildDetailDto>("GetGuildDetail", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildDetail", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildDetail", ex);
				throw;
			}
		}

		public async Task<byte[]?> GetGuildEmblemAsync(Guid guildId, int size)
		{
			Logger?.LogCall("GetGuildEmblemAsync", new object[2] { guildId, size });
			try
			{
				byte[] result = await InvokeAsync<byte[]>("GetGuildEmblemAsync", new object[2] { guildId, size }, CancellationToken.None);
				Logger?.LogResult("GetGuildEmblemAsync", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildEmblemAsync", ex);
				throw;
			}
		}

		public async Task<List<GuildRankDto>> GetGuildRanks(Guid guildId)
		{
			Logger?.LogCall("GetGuildRanks", new object[1] { guildId });
			try
			{
				List<GuildRankDto> result = await InvokeAsync<List<GuildRankDto>>("GetGuildRanks", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildRanks", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildRanks", ex);
				throw;
			}
		}

		public async Task<GuildRankDto> GetGuildRank(Guid guildId, string rankId)
		{
			Logger?.LogCall("GetGuildRank", new object[2] { guildId, rankId });
			try
			{
				GuildRankDto result = await InvokeAsync<GuildRankDto>("GetGuildRank", new object[2] { guildId, rankId }, CancellationToken.None);
				Logger?.LogResult("GetGuildRank", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildRank", ex);
				throw;
			}
		}

		public async Task<List<GuildMemberDetailDto>> GetGuildMembers(Guid guildId)
		{
			Logger?.LogCall("GetGuildMembers", new object[1] { guildId });
			try
			{
				List<GuildMemberDetailDto> result = await InvokeAsync<List<GuildMemberDetailDto>>("GetGuildMembers", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildMembers", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildMembers", ex);
				throw;
			}
		}

		public async Task<GuildMemberDetailDto> GetGuildMember(Guid guildId, string memberName)
		{
			Logger?.LogCall("GetGuildMember", new object[2] { guildId, memberName });
			try
			{
				GuildMemberDetailDto result = await InvokeAsync<GuildMemberDetailDto>("GetGuildMember", new object[2] { guildId, memberName }, CancellationToken.None);
				Logger?.LogResult("GetGuildMember", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildMember", ex);
				throw;
			}
		}

		public async Task<List<SpamDto>> GetGuildSpams(Guid guildId)
		{
			Logger?.LogCall("GetGuildSpams", new object[1] { guildId });
			try
			{
				List<SpamDto> result = await InvokeAsync<List<SpamDto>>("GetGuildSpams", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildSpams", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildSpams", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> GetGuildSpam(Guid guildId, Guid spamId)
		{
			Logger?.LogCall("GetGuildSpam", new object[2] { guildId, spamId });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("GetGuildSpam", new object[2] { guildId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetGuildSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildSpam", ex);
				throw;
			}
		}

		public async Task<bool> CanUseGuildSpam(Guid guildId, Guid spamId, int? mapId)
		{
			Logger?.LogCall("CanUseGuildSpam", new object[3] { guildId, spamId, mapId });
			try
			{
				bool flag = await InvokeAsync<bool>("CanUseGuildSpam", new object[3] { guildId, spamId, mapId }, CancellationToken.None);
				Logger?.LogResult("CanUseGuildSpam", flag);
				return flag;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CanUseGuildSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UseGuildSpam(Guid guildId, Guid spamId, SpamUsageRequestDto? request)
		{
			Logger?.LogCall("UseGuildSpam", new object[3] { guildId, spamId, request });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UseGuildSpam", new object[3] { guildId, spamId, request }, CancellationToken.None);
				Logger?.LogResult("UseGuildSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UseGuildSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> AddGuildSpam(Guid guildId, SpamCreateDto dto)
		{
			Logger?.LogCall("AddGuildSpam", new object[2] { guildId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("AddGuildSpam", new object[2] { guildId, dto }, CancellationToken.None);
				Logger?.LogResult("AddGuildSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddGuildSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UpdateGuildSpam(Guid guildId, Guid spamId, SpamUpdateDto dto)
		{
			Logger?.LogCall("UpdateGuildSpam", new object[3] { guildId, spamId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UpdateGuildSpam", new object[3] { guildId, spamId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateGuildSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateGuildSpam", ex);
				throw;
			}
		}

		public async Task DeleteGuildSpam(Guid guildId, Guid spamId)
		{
			Logger?.LogCall("DeleteGuildSpam", new object[2] { guildId, spamId });
			try
			{
				await InvokeVoidAsync("DeleteGuildSpam", new object[2] { guildId, spamId }, CancellationToken.None);
				Logger?.LogResult("DeleteGuildSpam", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteGuildSpam", ex);
				throw;
			}
		}

		public async Task<List<BanDto>> GetGuildBans(Guid guildId)
		{
			Logger?.LogCall("GetGuildBans", new object[1] { guildId });
			try
			{
				List<BanDto> result = await InvokeAsync<List<BanDto>>("GetGuildBans", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildBans", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildBans", ex);
				throw;
			}
		}

		public async Task<BanDto> GetGuildBan(Guid guildId, Guid banId)
		{
			Logger?.LogCall("GetGuildBan", new object[2] { guildId, banId });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("GetGuildBan", new object[2] { guildId, banId }, CancellationToken.None);
				Logger?.LogResult("GetGuildBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildBan", ex);
				throw;
			}
		}

		public async Task<BanDto> AddGuildBan(Guid guildId, BanCreateDto dto)
		{
			Logger?.LogCall("AddGuildBan", new object[2] { guildId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("AddGuildBan", new object[2] { guildId, dto }, CancellationToken.None);
				Logger?.LogResult("AddGuildBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddGuildBan", ex);
				throw;
			}
		}

		public async Task<BanDto> UpdateGuildBan(Guid guildId, Guid banId, BanUpdateDto dto)
		{
			Logger?.LogCall("UpdateGuildBan", new object[3] { guildId, banId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("UpdateGuildBan", new object[3] { guildId, banId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateGuildBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateGuildBan", ex);
				throw;
			}
		}

		public async Task DeleteGuildBan(Guid guildId, Guid banId)
		{
			Logger?.LogCall("DeleteGuildBan", new object[2] { guildId, banId });
			try
			{
				await InvokeVoidAsync("DeleteGuildBan", new object[2] { guildId, banId }, CancellationToken.None);
				Logger?.LogResult("DeleteGuildBan", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteGuildBan", ex);
				throw;
			}
		}

		public async Task<List<GuildStashTabDto>> GetGuildStash(Guid guildId)
		{
			Logger?.LogCall("GetGuildStash", new object[1] { guildId });
			try
			{
				List<GuildStashTabDto> result = await InvokeAsync<List<GuildStashTabDto>>("GetGuildStash", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildStash", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildStash", ex);
				throw;
			}
		}

		public async Task<List<GuildTreasuryItemDto>> GetGuildTreasury(Guid guildId)
		{
			Logger?.LogCall("GetGuildTreasury", new object[1] { guildId });
			try
			{
				List<GuildTreasuryItemDto> result = await InvokeAsync<List<GuildTreasuryItemDto>>("GetGuildTreasury", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildTreasury", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildTreasury", ex);
				throw;
			}
		}

		public async Task<List<GuildStashChangeDto>> GetGuildStashHistory(Guid guildId, GuildStashChangeFilterDto? filter)
		{
			Logger?.LogCall("GetGuildStashHistory", new object[2] { guildId, filter });
			try
			{
				List<GuildStashChangeDto> result = await InvokeAsync<List<GuildStashChangeDto>>("GetGuildStashHistory", new object[2] { guildId, filter }, CancellationToken.None);
				Logger?.LogResult("GetGuildStashHistory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildStashHistory", ex);
				throw;
			}
		}

		public async Task<List<StashSearchResultDto>> SearchGuildStash(Guid guildId, StashSearchFilterDto? filter)
		{
			Logger?.LogCall("SearchGuildStash", new object[2] { guildId, filter });
			try
			{
				List<StashSearchResultDto> result = await InvokeAsync<List<StashSearchResultDto>>("SearchGuildStash", new object[2] { guildId, filter }, CancellationToken.None);
				Logger?.LogResult("SearchGuildStash", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("SearchGuildStash", ex);
				throw;
			}
		}

		public async Task<List<SpamCategoryDto>> GetGuildSpamCategories(Guid guildId)
		{
			Logger?.LogCall("GetGuildSpamCategories", new object[1] { guildId });
			try
			{
				List<SpamCategoryDto> result = await InvokeAsync<List<SpamCategoryDto>>("GetGuildSpamCategories", new object[1] { guildId }, CancellationToken.None);
				Logger?.LogResult("GetGuildSpamCategories", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildSpamCategories", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> AddGuildSpamCategory(Guid guildId, SpamCategoryCreateDto dto)
		{
			Logger?.LogCall("AddGuildSpamCategory", new object[2] { guildId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("AddGuildSpamCategory", new object[2] { guildId, dto }, CancellationToken.None);
				Logger?.LogResult("AddGuildSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddGuildSpamCategory", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> UpdateGuildSpamCategory(Guid guildId, Guid categoryId, SpamCategoryUpdateDto dto)
		{
			Logger?.LogCall("UpdateGuildSpamCategory", new object[3] { guildId, categoryId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("UpdateGuildSpamCategory", new object[3] { guildId, categoryId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateGuildSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateGuildSpamCategory", ex);
				throw;
			}
		}

		public async Task DeleteGuildSpamCategory(Guid guildId, Guid categoryId)
		{
			Logger?.LogCall("DeleteGuildSpamCategory", new object[2] { guildId, categoryId });
			try
			{
				await InvokeVoidAsync("DeleteGuildSpamCategory", new object[2] { guildId, categoryId }, CancellationToken.None);
				Logger?.LogResult("DeleteGuildSpamCategory", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteGuildSpamCategory", ex);
				throw;
			}
		}

		public async Task<List<SpamMapHistoryDto>> GetGuildSpamMapHistory(Guid guildId, Guid spamId)
		{
			Logger?.LogCall("GetGuildSpamMapHistory", new object[2] { guildId, spamId });
			try
			{
				List<SpamMapHistoryDto> result = await InvokeAsync<List<SpamMapHistoryDto>>("GetGuildSpamMapHistory", new object[2] { guildId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetGuildSpamMapHistory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildSpamMapHistory", ex);
				throw;
			}
		}

		public async Task<List<SpamUsageLogDto>> GetGuildSpamUsageLogs(Guid guildId, Guid spamId, int? limit)
		{
			Logger?.LogCall("GetGuildSpamUsageLogs", new object[3] { guildId, spamId, limit });
			try
			{
				List<SpamUsageLogDto> result = await InvokeAsync<List<SpamUsageLogDto>>("GetGuildSpamUsageLogs", new object[3] { guildId, spamId, limit }, CancellationToken.None);
				Logger?.LogResult("GetGuildSpamUsageLogs", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetGuildSpamUsageLogs", ex);
				throw;
			}
		}

		public async Task<List<AllianceMembershipDto>> GetAllianceMemberships(Guid accountId)
		{
			Logger?.LogCall("GetAllianceMemberships", new object[1] { accountId });
			try
			{
				List<AllianceMembershipDto> result = await InvokeAsync<List<AllianceMembershipDto>>("GetAllianceMemberships", new object[1] { accountId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceMemberships", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceMemberships", ex);
				throw;
			}
		}

		public async Task<AllianceDetailDto> GetAllianceDetail(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceDetail", new object[1] { allianceId });
			try
			{
				AllianceDetailDto result = await InvokeAsync<AllianceDetailDto>("GetAllianceDetail", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceDetail", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceDetail", ex);
				throw;
			}
		}

		public async Task<AllianceDetailDto> UpdateAllianceDetail(Guid allianceId, AllianceDetailUpdateDto dto)
		{
			Logger?.LogCall("UpdateAllianceDetail", new object[2] { allianceId, dto });
			try
			{
				AllianceDetailDto result = await InvokeAsync<AllianceDetailDto>("UpdateAllianceDetail", new object[2] { allianceId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAllianceDetail", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAllianceDetail", ex);
				throw;
			}
		}

		public async Task<List<AllianceRankDto>> GetAllianceRanks(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceRanks", new object[1] { allianceId });
			try
			{
				List<AllianceRankDto> result = await InvokeAsync<List<AllianceRankDto>>("GetAllianceRanks", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceRanks", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceRanks", ex);
				throw;
			}
		}

		public async Task<AllianceRankDto> GetAllianceRank(Guid allianceId, Guid rankId)
		{
			Logger?.LogCall("GetAllianceRank", new object[2] { allianceId, rankId });
			try
			{
				AllianceRankDto result = await InvokeAsync<AllianceRankDto>("GetAllianceRank", new object[2] { allianceId, rankId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceRank", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceRank", ex);
				throw;
			}
		}

		public async Task<List<AllianceMemberDetailDto>> GetAllianceMembers(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceMembers", new object[1] { allianceId });
			try
			{
				List<AllianceMemberDetailDto> result = await InvokeAsync<List<AllianceMemberDetailDto>>("GetAllianceMembers", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceMembers", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceMembers", ex);
				throw;
			}
		}

		public async Task<AllianceMemberDetailDto> GetAllianceMember(Guid allianceId, string memberName)
		{
			Logger?.LogCall("GetAllianceMember", new object[2] { allianceId, memberName });
			try
			{
				AllianceMemberDetailDto result = await InvokeAsync<AllianceMemberDetailDto>("GetAllianceMember", new object[2] { allianceId, memberName }, CancellationToken.None);
				Logger?.LogResult("GetAllianceMember", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceMember", ex);
				throw;
			}
		}

		public async Task<List<SpamDto>> GetAllianceSpams(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceSpams", new object[1] { allianceId });
			try
			{
				List<SpamDto> result = await InvokeAsync<List<SpamDto>>("GetAllianceSpams", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceSpams", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceSpams", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> GetAllianceSpam(Guid allianceId, Guid spamId)
		{
			Logger?.LogCall("GetAllianceSpam", new object[2] { allianceId, spamId });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("GetAllianceSpam", new object[2] { allianceId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceSpam", ex);
				throw;
			}
		}

		public async Task<bool> CanUseAllianceSpam(Guid allianceId, Guid spamId, int? mapId)
		{
			Logger?.LogCall("CanUseAllianceSpam", new object[3] { allianceId, spamId, mapId });
			try
			{
				bool flag = await InvokeAsync<bool>("CanUseAllianceSpam", new object[3] { allianceId, spamId, mapId }, CancellationToken.None);
				Logger?.LogResult("CanUseAllianceSpam", flag);
				return flag;
			}
			catch (Exception ex)
			{
				Logger?.LogError("CanUseAllianceSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UseAllianceSpam(Guid allianceId, Guid spamId, SpamUsageRequestDto? request)
		{
			Logger?.LogCall("UseAllianceSpam", new object[3] { allianceId, spamId, request });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UseAllianceSpam", new object[3] { allianceId, spamId, request }, CancellationToken.None);
				Logger?.LogResult("UseAllianceSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UseAllianceSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> AddAllianceSpam(Guid allianceId, SpamCreateDto dto)
		{
			Logger?.LogCall("AddAllianceSpam", new object[2] { allianceId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("AddAllianceSpam", new object[2] { allianceId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAllianceSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAllianceSpam", ex);
				throw;
			}
		}

		public async Task<SpamDetailDto> UpdateAllianceSpam(Guid allianceId, Guid spamId, SpamUpdateDto dto)
		{
			Logger?.LogCall("UpdateAllianceSpam", new object[3] { allianceId, spamId, dto });
			try
			{
				SpamDetailDto result = await InvokeAsync<SpamDetailDto>("UpdateAllianceSpam", new object[3] { allianceId, spamId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAllianceSpam", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAllianceSpam", ex);
				throw;
			}
		}

		public async Task DeleteAllianceSpam(Guid allianceId, Guid spamId)
		{
			Logger?.LogCall("DeleteAllianceSpam", new object[2] { allianceId, spamId });
			try
			{
				await InvokeVoidAsync("DeleteAllianceSpam", new object[2] { allianceId, spamId }, CancellationToken.None);
				Logger?.LogResult("DeleteAllianceSpam", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAllianceSpam", ex);
				throw;
			}
		}

		public async Task<List<BanDto>> GetAllianceBans(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceBans", new object[1] { allianceId });
			try
			{
				List<BanDto> result = await InvokeAsync<List<BanDto>>("GetAllianceBans", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceBans", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceBans", ex);
				throw;
			}
		}

		public async Task<BanDto> GetAllianceBan(Guid allianceId, Guid banId)
		{
			Logger?.LogCall("GetAllianceBan", new object[2] { allianceId, banId });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("GetAllianceBan", new object[2] { allianceId, banId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceBan", ex);
				throw;
			}
		}

		public async Task<BanDto> AddAllianceBan(Guid allianceId, BanCreateDto dto)
		{
			Logger?.LogCall("AddAllianceBan", new object[2] { allianceId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("AddAllianceBan", new object[2] { allianceId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAllianceBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAllianceBan", ex);
				throw;
			}
		}

		public async Task<BanDto> UpdateAllianceBan(Guid allianceId, Guid banId, BanUpdateDto dto)
		{
			Logger?.LogCall("UpdateAllianceBan", new object[3] { allianceId, banId, dto });
			try
			{
				BanDto result = await InvokeAsync<BanDto>("UpdateAllianceBan", new object[3] { allianceId, banId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAllianceBan", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAllianceBan", ex);
				throw;
			}
		}

		public async Task DeleteAllianceBan(Guid allianceId, Guid banId)
		{
			Logger?.LogCall("DeleteAllianceBan", new object[2] { allianceId, banId });
			try
			{
				await InvokeVoidAsync("DeleteAllianceBan", new object[2] { allianceId, banId }, CancellationToken.None);
				Logger?.LogResult("DeleteAllianceBan", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAllianceBan", ex);
				throw;
			}
		}

		public async Task<List<AllianceTagTypeDto>> GetAllianceTagTypes(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceTagTypes", new object[1] { allianceId });
			try
			{
				List<AllianceTagTypeDto> result = await InvokeAsync<List<AllianceTagTypeDto>>("GetAllianceTagTypes", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceTagTypes", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceTagTypes", ex);
				throw;
			}
		}

		public async Task<AllianceTagTypeDto?> GetAllianceTagType(Guid allianceId, Guid tagTypeId)
		{
			Logger?.LogCall("GetAllianceTagType", new object[2] { allianceId, tagTypeId });
			try
			{
				AllianceTagTypeDto result = await InvokeAsync<AllianceTagTypeDto>("GetAllianceTagType", new object[2] { allianceId, tagTypeId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceTagType", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceTagType", ex);
				throw;
			}
		}

		public async Task<AllianceTagTypeDto> AddAllianceTagType(Guid allianceId, AllianceTagTypeCreateDto dto)
		{
			Logger?.LogCall("AddAllianceTagType", new object[2] { allianceId, dto });
			try
			{
				AllianceTagTypeDto result = await InvokeAsync<AllianceTagTypeDto>("AddAllianceTagType", new object[2] { allianceId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAllianceTagType", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAllianceTagType", ex);
				throw;
			}
		}

		public async Task<AllianceTagTypeDto> UpdateAllianceTagType(Guid allianceId, Guid tagTypeId, AllianceTagTypeUpdateDto dto)
		{
			Logger?.LogCall("UpdateAllianceTagType", new object[3] { allianceId, tagTypeId, dto });
			try
			{
				AllianceTagTypeDto result = await InvokeAsync<AllianceTagTypeDto>("UpdateAllianceTagType", new object[3] { allianceId, tagTypeId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAllianceTagType", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAllianceTagType", ex);
				throw;
			}
		}

		public async Task DeleteAllianceTagType(Guid allianceId, Guid tagTypeId)
		{
			Logger?.LogCall("DeleteAllianceTagType", new object[2] { allianceId, tagTypeId });
			try
			{
				await InvokeVoidAsync("DeleteAllianceTagType", new object[2] { allianceId, tagTypeId }, CancellationToken.None);
				Logger?.LogResult("DeleteAllianceTagType", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAllianceTagType", ex);
				throw;
			}
		}

		public async Task<List<AllianceMemberTagDto>> GetAllianceMemberTags(Guid allianceId, string memberName)
		{
			Logger?.LogCall("GetAllianceMemberTags", new object[2] { allianceId, memberName });
			try
			{
				List<AllianceMemberTagDto> result = await InvokeAsync<List<AllianceMemberTagDto>>("GetAllianceMemberTags", new object[2] { allianceId, memberName }, CancellationToken.None);
				Logger?.LogResult("GetAllianceMemberTags", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceMemberTags", ex);
				throw;
			}
		}

		public async Task<List<AllianceMemberTagDto>> GetAllianceActiveTagsByType(Guid allianceId, Guid tagTypeId)
		{
			Logger?.LogCall("GetAllianceActiveTagsByType", new object[2] { allianceId, tagTypeId });
			try
			{
				List<AllianceMemberTagDto> result = await InvokeAsync<List<AllianceMemberTagDto>>("GetAllianceActiveTagsByType", new object[2] { allianceId, tagTypeId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceActiveTagsByType", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceActiveTagsByType", ex);
				throw;
			}
		}

		public async Task<AllianceMemberTagDto> AddAllianceMemberTag(Guid allianceId, string memberName, AllianceMemberTagCreateDto dto)
		{
			Logger?.LogCall("AddAllianceMemberTag", new object[3] { allianceId, memberName, dto });
			try
			{
				AllianceMemberTagDto result = await InvokeAsync<AllianceMemberTagDto>("AddAllianceMemberTag", new object[3] { allianceId, memberName, dto }, CancellationToken.None);
				Logger?.LogResult("AddAllianceMemberTag", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAllianceMemberTag", ex);
				throw;
			}
		}

		public async Task<AllianceMemberTagDto> ExtendAllianceMemberTag(Guid allianceId, Guid tagId, AllianceMemberTagExtendDto dto)
		{
			Logger?.LogCall("ExtendAllianceMemberTag", new object[3] { allianceId, tagId, dto });
			try
			{
				AllianceMemberTagDto result = await InvokeAsync<AllianceMemberTagDto>("ExtendAllianceMemberTag", new object[3] { allianceId, tagId, dto }, CancellationToken.None);
				Logger?.LogResult("ExtendAllianceMemberTag", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("ExtendAllianceMemberTag", ex);
				throw;
			}
		}

		public async Task RevokeAllianceMemberTag(Guid allianceId, Guid tagId)
		{
			Logger?.LogCall("RevokeAllianceMemberTag", new object[2] { allianceId, tagId });
			try
			{
				await InvokeVoidAsync("RevokeAllianceMemberTag", new object[2] { allianceId, tagId }, CancellationToken.None);
				Logger?.LogResult("RevokeAllianceMemberTag", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("RevokeAllianceMemberTag", ex);
				throw;
			}
		}

		public async Task<List<AllianceTagHealthCheckResultDto>> GetAllianceTagHealthCheck(Guid allianceId, int expiringInDays)
		{
			Logger?.LogCall("GetAllianceTagHealthCheck", new object[2] { allianceId, expiringInDays });
			try
			{
				List<AllianceTagHealthCheckResultDto> result = await InvokeAsync<List<AllianceTagHealthCheckResultDto>>("GetAllianceTagHealthCheck", new object[2] { allianceId, expiringInDays }, CancellationToken.None);
				Logger?.LogResult("GetAllianceTagHealthCheck", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceTagHealthCheck", ex);
				throw;
			}
		}

		public async Task<int> DeactivateExpiredAllianceTags(Guid allianceId)
		{
			Logger?.LogCall("DeactivateExpiredAllianceTags", new object[1] { allianceId });
			try
			{
				int num = await InvokeAsync<int>("DeactivateExpiredAllianceTags", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("DeactivateExpiredAllianceTags", num);
				return num;
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeactivateExpiredAllianceTags", ex);
				throw;
			}
		}

		public async Task<List<StashSearchResultDto>> SearchAllianceStash(Guid allianceId, StashSearchFilterDto? filter)
		{
			Logger?.LogCall("SearchAllianceStash", new object[2] { allianceId, filter });
			try
			{
				List<StashSearchResultDto> result = await InvokeAsync<List<StashSearchResultDto>>("SearchAllianceStash", new object[2] { allianceId, filter }, CancellationToken.None);
				Logger?.LogResult("SearchAllianceStash", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("SearchAllianceStash", ex);
				throw;
			}
		}

		public async Task<List<SpamCategoryDto>> GetAllianceSpamCategories(Guid allianceId)
		{
			Logger?.LogCall("GetAllianceSpamCategories", new object[1] { allianceId });
			try
			{
				List<SpamCategoryDto> result = await InvokeAsync<List<SpamCategoryDto>>("GetAllianceSpamCategories", new object[1] { allianceId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceSpamCategories", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceSpamCategories", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> AddAllianceSpamCategory(Guid allianceId, SpamCategoryCreateDto dto)
		{
			Logger?.LogCall("AddAllianceSpamCategory", new object[2] { allianceId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("AddAllianceSpamCategory", new object[2] { allianceId, dto }, CancellationToken.None);
				Logger?.LogResult("AddAllianceSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("AddAllianceSpamCategory", ex);
				throw;
			}
		}

		public async Task<SpamCategoryDto> UpdateAllianceSpamCategory(Guid allianceId, Guid categoryId, SpamCategoryUpdateDto dto)
		{
			Logger?.LogCall("UpdateAllianceSpamCategory", new object[3] { allianceId, categoryId, dto });
			try
			{
				SpamCategoryDto result = await InvokeAsync<SpamCategoryDto>("UpdateAllianceSpamCategory", new object[3] { allianceId, categoryId, dto }, CancellationToken.None);
				Logger?.LogResult("UpdateAllianceSpamCategory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("UpdateAllianceSpamCategory", ex);
				throw;
			}
		}

		public async Task DeleteAllianceSpamCategory(Guid allianceId, Guid categoryId)
		{
			Logger?.LogCall("DeleteAllianceSpamCategory", new object[2] { allianceId, categoryId });
			try
			{
				await InvokeVoidAsync("DeleteAllianceSpamCategory", new object[2] { allianceId, categoryId }, CancellationToken.None);
				Logger?.LogResult("DeleteAllianceSpamCategory", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("DeleteAllianceSpamCategory", ex);
				throw;
			}
		}

		public async Task<List<SpamMapHistoryDto>> GetAllianceSpamMapHistory(Guid allianceId, Guid spamId)
		{
			Logger?.LogCall("GetAllianceSpamMapHistory", new object[2] { allianceId, spamId });
			try
			{
				List<SpamMapHistoryDto> result = await InvokeAsync<List<SpamMapHistoryDto>>("GetAllianceSpamMapHistory", new object[2] { allianceId, spamId }, CancellationToken.None);
				Logger?.LogResult("GetAllianceSpamMapHistory", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceSpamMapHistory", ex);
				throw;
			}
		}

		public async Task<List<SpamUsageLogDto>> GetAllianceSpamUsageLogs(Guid allianceId, Guid spamId, int? limit)
		{
			Logger?.LogCall("GetAllianceSpamUsageLogs", new object[3] { allianceId, spamId, limit });
			try
			{
				List<SpamUsageLogDto> result = await InvokeAsync<List<SpamUsageLogDto>>("GetAllianceSpamUsageLogs", new object[3] { allianceId, spamId, limit }, CancellationToken.None);
				Logger?.LogResult("GetAllianceSpamUsageLogs", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetAllianceSpamUsageLogs", ex);
				throw;
			}
		}

		public async Task<Gw2MapDto?> GetMap(int mapId)
		{
			Logger?.LogCall("GetMap", new object[1] { mapId });
			try
			{
				Gw2MapDto result = await InvokeAsync<Gw2MapDto>("GetMap", new object[1] { mapId }, CancellationToken.None);
				Logger?.LogResult("GetMap", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetMap", ex);
				throw;
			}
		}

		public async Task<List<Gw2MapDto>> GetMaps(List<int> mapIds)
		{
			Logger?.LogCall("GetMaps", new object[1] { mapIds });
			try
			{
				List<Gw2MapDto> result = await InvokeAsync<List<Gw2MapDto>>("GetMaps", new object[1] { mapIds }, CancellationToken.None);
				Logger?.LogResult("GetMaps", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetMaps", ex);
				throw;
			}
		}

		public async Task<string?> GetMapName(int mapId)
		{
			Logger?.LogCall("GetMapName", new object[1] { mapId });
			try
			{
				string result = await InvokeAsync<string>("GetMapName", new object[1] { mapId }, CancellationToken.None);
				Logger?.LogResult("GetMapName", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetMapName", ex);
				throw;
			}
		}

		public async Task EnsureMapsExist(List<int> mapIds)
		{
			Logger?.LogCall("EnsureMapsExist", new object[1] { mapIds });
			try
			{
				await InvokeVoidAsync("EnsureMapsExist", new object[1] { mapIds }, CancellationToken.None);
				Logger?.LogResult("EnsureMapsExist", null);
			}
			catch (Exception ex)
			{
				Logger?.LogError("EnsureMapsExist", ex);
				throw;
			}
		}

		public async Task<Gw2ContinentDto?> GetContinent(int continentId)
		{
			Logger?.LogCall("GetContinent", new object[1] { continentId });
			try
			{
				Gw2ContinentDto result = await InvokeAsync<Gw2ContinentDto>("GetContinent", new object[1] { continentId }, CancellationToken.None);
				Logger?.LogResult("GetContinent", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetContinent", ex);
				throw;
			}
		}

		public async Task<List<Gw2ContinentDto>> GetContinents()
		{
			Logger?.LogCall("GetContinents", Array.Empty<object>());
			try
			{
				List<Gw2ContinentDto> result = await InvokeAsync<List<Gw2ContinentDto>>("GetContinents", Array.Empty<object>(), CancellationToken.None);
				Logger?.LogResult("GetContinents", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("GetContinents", ex);
				throw;
			}
		}

		public async Task<CoordinateDto> ConvertMapToContinent(int mapId, double mapX, double mapY)
		{
			Logger?.LogCall("ConvertMapToContinent", new object[3] { mapId, mapX, mapY });
			try
			{
				CoordinateDto result = await InvokeAsync<CoordinateDto>("ConvertMapToContinent", new object[3] { mapId, mapX, mapY }, CancellationToken.None);
				Logger?.LogResult("ConvertMapToContinent", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("ConvertMapToContinent", ex);
				throw;
			}
		}

		public async Task<CoordinateDto> ConvertContinentToMap(int mapId, double continentX, double continentY)
		{
			Logger?.LogCall("ConvertContinentToMap", new object[3] { mapId, continentX, continentY });
			try
			{
				CoordinateDto result = await InvokeAsync<CoordinateDto>("ConvertContinentToMap", new object[3] { mapId, continentX, continentY }, CancellationToken.None);
				Logger?.LogResult("ConvertContinentToMap", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("ConvertContinentToMap", ex);
				throw;
			}
		}

		public async Task<ServerHandshakeResponseDto> Handshake(ClientHandshakeDto request)
		{
			Logger?.LogCall("Handshake", new object[1] { request });
			try
			{
				ServerHandshakeResponseDto result = await InvokeAsync<ServerHandshakeResponseDto>("Handshake", new object[1] { request }, CancellationToken.None);
				Logger?.LogResult("Handshake", result);
				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogError("Handshake", ex);
				throw;
			}
		}

		Task IGw2HubReceiver.GuildSpamAdded(Guid guildId, SpamDetailDto spam)
		{
			Logger?.LogEvent("GuildSpamAdded", new { guildId, spam });
			this.GuildSpamAdded?.Invoke(this, new GuildSpamAddedEventArgs(guildId, spam));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamChanged(Guid guildId, SpamDetailDto spam)
		{
			Logger?.LogEvent("GuildSpamChanged", new { guildId, spam });
			this.GuildSpamChanged?.Invoke(this, new GuildSpamChangedEventArgs(guildId, spam));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamRemoved(Guid guildId, Guid spamId)
		{
			Logger?.LogEvent("GuildSpamRemoved", new { guildId, spamId });
			this.GuildSpamRemoved?.Invoke(this, new GuildSpamRemovedEventArgs(guildId, spamId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamUsed(Guid guildId, SpamUsedNotificationDto notification)
		{
			Logger?.LogEvent("GuildSpamUsed", new { guildId, notification });
			this.GuildSpamUsed?.Invoke(this, new GuildSpamUsedEventArgs(guildId, notification));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildBanAdded(Guid guildId, BanDto ban)
		{
			Logger?.LogEvent("GuildBanAdded", new { guildId, ban });
			this.GuildBanAdded?.Invoke(this, new GuildBanAddedEventArgs(guildId, ban));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildBanChanged(Guid guildId, BanDto ban)
		{
			Logger?.LogEvent("GuildBanChanged", new { guildId, ban });
			this.GuildBanChanged?.Invoke(this, new GuildBanChangedEventArgs(guildId, ban));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildBanRemoved(Guid guildId, Guid banId)
		{
			Logger?.LogEvent("GuildBanRemoved", new { guildId, banId });
			this.GuildBanRemoved?.Invoke(this, new GuildBanRemovedEventArgs(guildId, banId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamAdded(Guid allianceId, SpamDetailDto spam)
		{
			Logger?.LogEvent("AllianceSpamAdded", new { allianceId, spam });
			this.AllianceSpamAdded?.Invoke(this, new AllianceSpamAddedEventArgs(allianceId, spam));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamChanged(Guid allianceId, SpamDetailDto spam)
		{
			Logger?.LogEvent("AllianceSpamChanged", new { allianceId, spam });
			this.AllianceSpamChanged?.Invoke(this, new AllianceSpamChangedEventArgs(allianceId, spam));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamRemoved(Guid allianceId, Guid spamId)
		{
			Logger?.LogEvent("AllianceSpamRemoved", new { allianceId, spamId });
			this.AllianceSpamRemoved?.Invoke(this, new AllianceSpamRemovedEventArgs(allianceId, spamId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamUsed(Guid allianceId, SpamUsedNotificationDto notification)
		{
			Logger?.LogEvent("AllianceSpamUsed", new { allianceId, notification });
			this.AllianceSpamUsed?.Invoke(this, new AllianceSpamUsedEventArgs(allianceId, notification));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceBanAdded(Guid allianceId, BanDto ban)
		{
			Logger?.LogEvent("AllianceBanAdded", new { allianceId, ban });
			this.AllianceBanAdded?.Invoke(this, new AllianceBanAddedEventArgs(allianceId, ban));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceBanChanged(Guid allianceId, BanDto ban)
		{
			Logger?.LogEvent("AllianceBanChanged", new { allianceId, ban });
			this.AllianceBanChanged?.Invoke(this, new AllianceBanChangedEventArgs(allianceId, ban));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceBanRemoved(Guid allianceId, Guid banId)
		{
			Logger?.LogEvent("AllianceBanRemoved", new { allianceId, banId });
			this.AllianceBanRemoved?.Invoke(this, new AllianceBanRemovedEventArgs(allianceId, banId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceTagTypeAdded(Guid allianceId, AllianceTagTypeDto tagType)
		{
			Logger?.LogEvent("AllianceTagTypeAdded", new { allianceId, tagType });
			this.AllianceTagTypeAdded?.Invoke(this, new AllianceTagTypeAddedEventArgs(allianceId, tagType));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceTagTypeChanged(Guid allianceId, AllianceTagTypeDto tagType)
		{
			Logger?.LogEvent("AllianceTagTypeChanged", new { allianceId, tagType });
			this.AllianceTagTypeChanged?.Invoke(this, new AllianceTagTypeChangedEventArgs(allianceId, tagType));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceTagTypeRemoved(Guid allianceId, Guid tagTypeId)
		{
			Logger?.LogEvent("AllianceTagTypeRemoved", new { allianceId, tagTypeId });
			this.AllianceTagTypeRemoved?.Invoke(this, new AllianceTagTypeRemovedEventArgs(allianceId, tagTypeId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceMemberTagAdded(Guid allianceId, AllianceMemberTagDto memberTag)
		{
			Logger?.LogEvent("AllianceMemberTagAdded", new { allianceId, memberTag });
			this.AllianceMemberTagAdded?.Invoke(this, new AllianceMemberTagAddedEventArgs(allianceId, memberTag));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceMemberTagChanged(Guid allianceId, AllianceMemberTagDto memberTag)
		{
			Logger?.LogEvent("AllianceMemberTagChanged", new { allianceId, memberTag });
			this.AllianceMemberTagChanged?.Invoke(this, new AllianceMemberTagChangedEventArgs(allianceId, memberTag));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceMemberTagRevoked(Guid allianceId, Guid tagId)
		{
			Logger?.LogEvent("AllianceMemberTagRevoked", new { allianceId, tagId });
			this.AllianceMemberTagRevoked?.Invoke(this, new AllianceMemberTagRevokedEventArgs(allianceId, tagId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.TaskProgressUpdated(Guid taskId, TaskProgressDto progress)
		{
			Logger?.LogEvent("TaskProgressUpdated", new { taskId, progress });
			this.TaskProgressUpdated?.Invoke(this, new TaskProgressUpdatedEventArgs(taskId, progress));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.TaskCompleted(Guid taskId, TaskDto task)
		{
			Logger?.LogEvent("TaskCompleted", new { taskId, task });
			this.TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(taskId, task));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.TaskFailed(Guid taskId, TaskDto task)
		{
			Logger?.LogEvent("TaskFailed", new { taskId, task });
			this.TaskFailed?.Invoke(this, new TaskFailedEventArgs(taskId, task));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.TaskCancelled(Guid taskId)
		{
			Logger?.LogEvent("TaskCancelled", new { taskId });
			this.TaskCancelled?.Invoke(this, new TaskCancelledEventArgs(taskId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.SpamFavoriteAdded(Guid accountId, SpamFavoriteDto favorite)
		{
			Logger?.LogEvent("SpamFavoriteAdded", new { accountId, favorite });
			this.SpamFavoriteAdded?.Invoke(this, new SpamFavoriteAddedEventArgs(accountId, favorite));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.SpamFavoriteRemoved(Guid accountId, Guid favoriteId)
		{
			Logger?.LogEvent("SpamFavoriteRemoved", new { accountId, favoriteId });
			this.SpamFavoriteRemoved?.Invoke(this, new SpamFavoriteRemovedEventArgs(accountId, favoriteId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.SpamFavoritesReordered(Guid accountId)
		{
			Logger?.LogEvent("SpamFavoritesReordered", new { accountId });
			this.SpamFavoritesReordered?.Invoke(this, new SpamFavoritesReorderedEventArgs(accountId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamCategoryAdded(Guid guildId, SpamCategoryDto category)
		{
			Logger?.LogEvent("GuildSpamCategoryAdded", new { guildId, category });
			this.GuildSpamCategoryAdded?.Invoke(this, new GuildSpamCategoryAddedEventArgs(guildId, category));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamCategoryChanged(Guid guildId, SpamCategoryDto category)
		{
			Logger?.LogEvent("GuildSpamCategoryChanged", new { guildId, category });
			this.GuildSpamCategoryChanged?.Invoke(this, new GuildSpamCategoryChangedEventArgs(guildId, category));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.GuildSpamCategoryRemoved(Guid guildId, Guid categoryId)
		{
			Logger?.LogEvent("GuildSpamCategoryRemoved", new { guildId, categoryId });
			this.GuildSpamCategoryRemoved?.Invoke(this, new GuildSpamCategoryRemovedEventArgs(guildId, categoryId));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamCategoryAdded(Guid allianceId, SpamCategoryDto category)
		{
			Logger?.LogEvent("AllianceSpamCategoryAdded", new { allianceId, category });
			this.AllianceSpamCategoryAdded?.Invoke(this, new AllianceSpamCategoryAddedEventArgs(allianceId, category));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamCategoryChanged(Guid allianceId, SpamCategoryDto category)
		{
			Logger?.LogEvent("AllianceSpamCategoryChanged", new { allianceId, category });
			this.AllianceSpamCategoryChanged?.Invoke(this, new AllianceSpamCategoryChangedEventArgs(allianceId, category));
			return Task.CompletedTask;
		}

		Task IGw2HubReceiver.AllianceSpamCategoryRemoved(Guid allianceId, Guid categoryId)
		{
			Logger?.LogEvent("AllianceSpamCategoryRemoved", new { allianceId, categoryId });
			this.AllianceSpamCategoryRemoved?.Invoke(this, new AllianceSpamCategoryRemovedEventArgs(allianceId, categoryId));
			return Task.CompletedTask;
		}
	}
}
