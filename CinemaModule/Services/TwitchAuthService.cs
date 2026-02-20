using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json.Linq;

namespace CinemaModule.Services
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

		private const string ClientId = "8m7h0mxthjx16qofx82mruz640ke67";

		private const string Scopes = "user:read:subscriptions user:read:follows chat:read chat:edit";

		private const int PollIntervalMs = 5000;

		private const int DeviceCodeExpirySeconds = 1800;

		private readonly HttpClient _httpClient;

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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			_httpClient = new HttpClient();
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
			CancelPendingAuth();
			_pollCts = new CancellationTokenSource();
			try
			{
				DeviceCodeResponse deviceCode = await RequestDeviceCodeAsync();
				if (deviceCode == null)
				{
					RaiseAuthStatus(TwitchAuthStatus.Failed, "Failed to get device code");
					return;
				}
				this.DeviceCodeReceived?.Invoke(this, new DeviceCodeEventArgs(deviceCode.UserCode, deviceCode.VerificationUri));
				RaiseAuthStatus(TwitchAuthStatus.WaitingForUser, "Enter code: " + deviceCode.UserCode);
				await PollForTokenAsync(deviceCode.DeviceCode, _pollCts.Token);
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
					HttpResponseMessage response = await _httpClient.SendAsync(request);
					if (response.get_IsSuccessStatusCode())
					{
						JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
						Username = ((object)json.get_Item("login"))?.ToString();
						UserId = ((object)json.get_Item("user_id"))?.ToString();
						Logger.Info("Twitch token validated for user: " + Username + " (ID: " + UserId + ")");
						RaiseAuthStatus(TwitchAuthStatus.Authenticated, "Logged in as " + Username);
						return true;
					}
					if (response.get_StatusCode() == HttpStatusCode.Unauthorized)
					{
						Logger.Info("Twitch token expired, attempting refresh");
						return await RefreshTokenAsync();
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
				FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[3]
				{
					new KeyValuePair<string, string>("grant_type", "refresh_token"),
					new KeyValuePair<string, string>("refresh_token", RefreshToken),
					new KeyValuePair<string, string>("client_id", "8m7h0mxthjx16qofx82mruz640ke67")
				});
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", (HttpContent)(object)content);
				if (response.get_IsSuccessStatusCode())
				{
					JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
					AccessToken = ((object)json.get_Item("access_token"))?.ToString();
					RefreshToken = ((object)json.get_Item("refresh_token"))?.ToString();
					await ValidateTokenAsync();
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
					FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[2]
					{
						new KeyValuePair<string, string>("client_id", "8m7h0mxthjx16qofx82mruz640ke67"),
						new KeyValuePair<string, string>("token", AccessToken)
					});
					await _httpClient.PostAsync("https://id.twitch.tv/oauth2/revoke", (HttpContent)(object)content);
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
				FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[2]
				{
					new KeyValuePair<string, string>("client_id", "8m7h0mxthjx16qofx82mruz640ke67"),
					new KeyValuePair<string, string>("scopes", "user:read:subscriptions user:read:follows chat:read chat:edit")
				});
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/device", (HttpContent)(object)content);
				if (!response.get_IsSuccessStatusCode())
				{
					string error = await response.get_Content().ReadAsStringAsync();
					Logger.Warn("Failed to get device code: " + error);
					return null;
				}
				JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
				DeviceCodeResponse obj = new DeviceCodeResponse
				{
					DeviceCode = ((object)json.get_Item("device_code"))?.ToString(),
					UserCode = ((object)json.get_Item("user_code"))?.ToString(),
					VerificationUri = ((object)json.get_Item("verification_uri"))?.ToString()
				};
				JToken obj2 = json.get_Item("expires_in");
				obj.ExpiresIn = ((obj2 != null) ? Extensions.Value<int>((IEnumerable<JToken>)obj2) : 1800);
				JToken obj3 = json.get_Item("interval");
				obj.Interval = ((obj3 != null) ? Extensions.Value<int>((IEnumerable<JToken>)obj3) : 5);
				return obj;
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
				await Task.Delay(5000, cancellationToken);
				switch (await TryGetTokenAsync(deviceCode))
				{
				case TokenPollResult.Success:
					await ValidateTokenAsync();
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
				FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[4]
				{
					new KeyValuePair<string, string>("client_id", "8m7h0mxthjx16qofx82mruz640ke67"),
					new KeyValuePair<string, string>("scopes", "user:read:subscriptions user:read:follows chat:read chat:edit"),
					new KeyValuePair<string, string>("device_code", deviceCode),
					new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:device_code")
				});
				HttpResponseMessage response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", (HttpContent)(object)content);
				JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
				if (response.get_IsSuccessStatusCode())
				{
					AccessToken = ((object)json.get_Item("access_token"))?.ToString();
					RefreshToken = ((object)json.get_Item("refresh_token"))?.ToString();
					return TokenPollResult.Success;
				}
				string error = ((object)json.get_Item("message"))?.ToString() ?? ((object)json.get_Item("error"))?.ToString();
				if (error == "authorization_pending")
				{
					return TokenPollResult.Pending;
				}
				if (error == "slow_down")
				{
					await Task.Delay(5000);
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
