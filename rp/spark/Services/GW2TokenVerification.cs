using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace rp.spark.Services
{
	public class GW2TokenVerification : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<GW2TokenVerification>();

		private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30.0);

		private static readonly TimeSpan RefreshSkew = TimeSpan.FromMinutes(5.0);

		private static readonly TimeSpan TokenRequestTimeout = TimeSpan.FromSeconds(3.0);

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly SemaphoreSlim _tokenGate = new SemaphoreSlim(1, 1);

		private string _cachedToken = string.Empty;

		private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;

		private readonly object _cacheLock = new object();

		private int _cacheVersion;

		public GW2TokenVerification(Gw2ApiManager gw2ApiManager)
		{
			_gw2ApiManager = gw2ApiManager;
		}

		public void Clear()
		{
			lock (_cacheLock)
			{
				_cacheVersion++;
				_cachedToken = string.Empty;
				_expiresAt = DateTimeOffset.MinValue;
			}
		}

		public bool HasValidApiKey()
		{
			return HasRequiredPermissions();
		}

		public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!HasRequiredPermissions())
			{
				return string.Empty;
			}
			if (TryGetFreshToken(out var cachedToken))
			{
				return cachedToken;
			}
			bool hasTokenGate = false;
			using CancellationTokenSource tokenTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			tokenTimeout.CancelAfter(TokenRequestTimeout);
			try
			{
				await _tokenGate.WaitAsync(tokenTimeout.Token);
				hasTokenGate = true;
				if (!HasRequiredPermissions())
				{
					Clear();
					return string.Empty;
				}
				if (TryGetFreshToken(out cachedToken))
				{
					return cachedToken;
				}
				int cacheVersion = GetCacheVersion();
				DateTimeOffset expiresAt = DateTimeOffset.UtcNow.Add(TokenLifetime);
				CreateSubtoken obj = await ((IBlobClient<CreateSubtoken>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_CreateSubtoken()
					.Expires(expiresAt)
					.WithPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
					{
						(TokenPermission)1,
						(TokenPermission)3
					})).GetAsync(tokenTimeout.Token);
				string token = ((obj == null) ? null : obj.get_Subtoken()?.Trim()) ?? string.Empty;
				lock (_cacheLock)
				{
					if (cacheVersion != _cacheVersion)
					{
						return string.Empty;
					}
					_cachedToken = token;
					_expiresAt = (string.IsNullOrWhiteSpace(_cachedToken) ? DateTimeOffset.MinValue : expiresAt);
					return _cachedToken;
				}
			}
			catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
			{
				Logger.Warn("Timed out while creating a GW2 API verification subtoken for SPARK.");
				return string.Empty;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Clear();
				BlishWarnings.HttpBlocked(ex, "create a temporary GW2 API verification token");
				Logger.Warn(ex, "Failed to create a GW2 API verification subtoken for SPARK.");
				return string.Empty;
			}
			finally
			{
				if (hasTokenGate)
				{
					_tokenGate.Release();
				}
			}
		}

		private bool HasRequiredPermissions()
		{
			if (_gw2ApiManager != null && _gw2ApiManager.get_HasSubtoken())
			{
				return _gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)3
				});
			}
			return false;
		}

		private int GetCacheVersion()
		{
			lock (_cacheLock)
			{
				return _cacheVersion;
			}
		}

		private bool TryGetFreshToken(out string token)
		{
			lock (_cacheLock)
			{
				if (!string.IsNullOrWhiteSpace(_cachedToken) && DateTimeOffset.UtcNow < _expiresAt.Subtract(RefreshSkew))
				{
					token = _cachedToken;
					return true;
				}
			}
			token = string.Empty;
			return false;
		}

		public void Dispose()
		{
			Clear();
		}
	}
}
