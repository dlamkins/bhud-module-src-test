using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json.Linq;

namespace CinemaModule.Services.Twitch
{
	public class TwitchAuthService : IDisposable
	{
		private enum TokenPollResult
		{
			Pending,
			Success,
			Failed
		}

		private class DeviceCodeResponse
		{
			public string DeviceCode { get; set; }

			public string UserCode { get; set; }

			public string VerificationUri { get; set; }

			public int ExpiresIn { get; set; }

			public int Interval { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<TwitchAuthService>();

		private const string TwitchDeviceAuthUrl = "https://id.twitch.tv/oauth2/device";

		private const string TwitchTokenUrl = "https://id.twitch.tv/oauth2/token";

		private const string TwitchValidateUrl = "https://id.twitch.tv/oauth2/validate";

		private const string TwitchRevokeUrl = "https://id.twitch.tv/oauth2/revoke";

		public const string ClientId = "8m7h0mxthjx16qofx82mruz640ke67";

		private const string Scopes = "user:read:subscriptions user:read:follows chat:read chat:edit";

		private const int PollIntervalMs = 5000;

		private const int DeviceCodeExpirySeconds = 1800;

		private static readonly TimeSpan HttpTimeout = TimeSpan.FromSeconds(30.0);

		private readonly HttpClient _httpClient;

		private readonly object _pollLock = new object();

		private CancellationTokenSource _pollCts;

		public string AccessToken { get; private set; }

		public string RefreshToken { get; private set; }

		public string Username { get; private set; }

		public string UserId { get; private set; }

		public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

		public event EventHandler<TwitchAuthStatusEventArgs> AuthStatusChanged;

		public event EventHandler<DeviceCodeEventArgs> DeviceCodeReceived;

		public TwitchAuthService()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(HttpTimeout);
			_httpClient = val;
		}

		public void LoadTokens(string accessToken, string refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
			if (!string.IsNullOrEmpty(AccessToken))
			{
				ValidateTokenAsync();
			}
		}

		public async Task StartDeviceAuthFlowAsync()
		{
			CancellationToken cancellationToken;
			lock (_pollLock)
			{
				CancelPendingAuthInternal();
				_pollCts = new CancellationTokenSource();
				cancellationToken = _pollCts.Token;
			}
			try
			{
				DeviceCodeResponse deviceCode = await RequestDeviceCodeAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (deviceCode == null)
				{
					RaiseAuthStatus(TwitchAuthStatus.Failed, "Failed to get device code");
					return;
				}
				this.DeviceCodeReceived?.Invoke(this, new DeviceCodeEventArgs(deviceCode.UserCode, deviceCode.VerificationUri));
				RaiseAuthStatus(TwitchAuthStatus.WaitingForUser, "Enter code: " + deviceCode.UserCode);
				await PollForTokenAsync(deviceCode.DeviceCode, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException)
			{
				RaiseAuthStatus(TwitchAuthStatus.Cancelled, "Authentication cancelled");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Device auth flow failed");
				RaiseAuthStatus(TwitchAuthStatus.Failed, ex.Message);
			}
		}

		public void CancelPendingAuth()
		{
			lock (_pollLock)
			{
				CancelPendingAuthInternal();
			}
		}

		private void CancelPendingAuthInternal()
		{
			_pollCts?.Cancel();
			_pollCts?.Dispose();
			_pollCts = null;
		}

		public async Task<bool> ValidateTokenAsync()
		{
			if (string.IsNullOrEmpty(AccessToken))
			{
				return false;
			}
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), "https://id.twitch.tv/oauth2/validate");
				try
				{
					((HttpHeaders)request.get_Headers()).Add("Authorization", "OAuth " + AccessToken);
					HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
					if (response.get_IsSuccessStatusCode())
					{
						JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
						Username = json["login"]?.ToString();
						UserId = json["user_id"]?.ToString();
						Logger.Info("Twitch token validated for user: " + Username + " (ID: " + UserId + ")");
						RaiseAuthStatus(TwitchAuthStatus.Authenticated, "Logged in as " + Username);
						return true;
					}
					if (response.get_StatusCode() == HttpStatusCode.Unauthorized)
					{
						Logger.Info("Twitch token expired, attempting refresh");
						return await RefreshTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to validate Twitch token");
			}
			ClearTokens();
			RaiseAuthStatus(TwitchAuthStatus.NotAuthenticated, "Not logged in");
			return false;
		}

		public async Task<bool> RefreshTokenAsync()
		{
			if (string.IsNullOrEmpty(RefreshToken))
			{
				ClearTokens();
				return false;
			}
			try
			{
				FormUrlEncodedContent content = CreateFormContent(("grant_type", "refresh_token"), ("refresh_token", RefreshToken), ("client_id", "8m7h0mxthjx16qofx82mruz640ke67"));
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				if (response.get_IsSuccessStatusCode())
				{
					JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
					AccessToken = json["access_token"]?.ToString();
					RefreshToken = json["refresh_token"]?.ToString();
					await ValidateTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
					return true;
				}
				Logger.Warn($"Failed to refresh Twitch token: {response.get_StatusCode()}");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh Twitch token");
			}
			ClearTokens();
			RaiseAuthStatus(TwitchAuthStatus.NotAuthenticated, "Session expired");
			return false;
		}

		public async Task LogoutAsync()
		{
			CancelPendingAuth();
			if (!string.IsNullOrEmpty(AccessToken))
			{
				try
				{
					FormUrlEncodedContent content = CreateFormContent(("client_id", "8m7h0mxthjx16qofx82mruz640ke67"), ("token", AccessToken));
					await _httpClient.PostAsync("https://id.twitch.tv/oauth2/revoke", (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to revoke Twitch token");
				}
			}
			ClearTokens();
			RaiseAuthStatus(TwitchAuthStatus.NotAuthenticated, "Logged out");
		}

		private async Task<DeviceCodeResponse> RequestDeviceCodeAsync()
		{
			_ = 2;
			try
			{
				FormUrlEncodedContent content = CreateFormContent(("client_id", "8m7h0mxthjx16qofx82mruz640ke67"), ("scopes", "user:read:subscriptions user:read:follows chat:read chat:edit"));
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/device", (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				if (!response.get_IsSuccessStatusCode())
				{
					string error = await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
					Logger.Warn("Failed to get device code: " + error);
					return null;
				}
				JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
				return new DeviceCodeResponse
				{
					DeviceCode = json["device_code"]?.ToString(),
					UserCode = json["user_code"]?.ToString(),
					VerificationUri = json["verification_uri"]?.ToString(),
					ExpiresIn = (json["expires_in"]?.Value<int>() ?? 1800),
					Interval = (json["interval"]?.Value<int>() ?? 5)
				};
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to request device code");
				return null;
			}
		}

		private async Task PollForTokenAsync(string deviceCode, CancellationToken cancellationToken)
		{
			DateTime startTime = DateTime.UtcNow;
			TimeSpan timeout = TimeSpan.FromSeconds(1800.0);
			while (!cancellationToken.IsCancellationRequested)
			{
				if (DateTime.UtcNow - startTime > timeout)
				{
					RaiseAuthStatus(TwitchAuthStatus.Failed, "Code expired");
					break;
				}
				await Task.Delay(5000, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				switch (await TryGetTokenAsync(deviceCode).ConfigureAwait(continueOnCapturedContext: false))
				{
				case TokenPollResult.Success:
					await ValidateTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
					return;
				case TokenPollResult.Failed:
					RaiseAuthStatus(TwitchAuthStatus.Failed, "Authentication denied");
					return;
				}
			}
		}

		private async Task<TokenPollResult> TryGetTokenAsync(string deviceCode)
		{
			_ = 2;
			try
			{
				FormUrlEncodedContent content = CreateFormContent(("client_id", "8m7h0mxthjx16qofx82mruz640ke67"), ("scopes", "user:read:subscriptions user:read:follows chat:read chat:edit"), ("device_code", deviceCode), ("grant_type", "urn:ietf:params:oauth:grant-type:device_code"));
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false);
				JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
				if (response.get_IsSuccessStatusCode())
				{
					AccessToken = json["access_token"]?.ToString();
					RefreshToken = json["refresh_token"]?.ToString();
					return TokenPollResult.Success;
				}
				string error = json["message"]?.ToString() ?? json["error"]?.ToString();
				if (error == "authorization_pending")
				{
					return TokenPollResult.Pending;
				}
				if (error == "slow_down")
				{
					await Task.Delay(5000).ConfigureAwait(continueOnCapturedContext: false);
					return TokenPollResult.Pending;
				}
				Logger.Warn("Token poll failed: " + error);
				return TokenPollResult.Failed;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to poll for token");
				return TokenPollResult.Pending;
			}
		}

		private void ClearTokens()
		{
			AccessToken = null;
			RefreshToken = null;
			Username = null;
			UserId = null;
		}

		private void RaiseAuthStatus(TwitchAuthStatus status, string message)
		{
			this.AuthStatusChanged?.Invoke(this, new TwitchAuthStatusEventArgs(status, message, Username, UserId, AccessToken, RefreshToken));
		}

		private static FormUrlEncodedContent CreateFormContent(params (string key, string value)[] pairs)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			List<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>(pairs.Length);
			for (int i = 0; i < pairs.Length; i++)
			{
				var (key, value) = pairs[i];
				content.Add(new KeyValuePair<string, string>(key, value));
			}
			return new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content);
		}

		public void Dispose()
		{
			CancelPendingAuth();
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
