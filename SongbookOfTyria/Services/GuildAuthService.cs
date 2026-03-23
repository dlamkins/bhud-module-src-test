using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using SongbookOfTyria.Models.Api;

namespace SongbookOfTyria.Services
{
	public sealed class GuildAuthService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<GuildAuthService>();

		private const string BaseUrl = "https://www.gw2opus.com/wp-json/blishhud/v1/";

		private const string AuthVerifyEndpoint = "auth/verify";

		private const int TimeoutSeconds = 30;

		private const int ExpectedApiKeyLength = 72;

		private static readonly Regex ApiKeyPattern = new Regex("^[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{20}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private readonly HttpClient _httpClient;

		private bool _isVerified;

		private bool _isOpusMember;

		private string _accountName;

		private string _currentApiKey;

		public bool IsVerified => _isVerified;

		public bool IsOpusMember => _isOpusMember;

		public string AccountName => _accountName;

		public string CurrentApiKey => _currentApiKey;

		public event EventHandler<GuildAuthStatusChangedEventArgs> AuthStatusChanged;

		public GuildAuthService()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(30.0));
			_httpClient = val;
		}

		public static ApiKeyValidationResult ValidateApiKeyFormat(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				return new ApiKeyValidationResult(isValid: false, "API key cannot be empty.");
			}
			string trimmedKey = apiKey.Trim();
			if (trimmedKey.Length != 72)
			{
				return new ApiKeyValidationResult(isValid: false, $"API key must be {72} characters (got {trimmedKey.Length}).");
			}
			if (!ApiKeyPattern.IsMatch(trimmedKey))
			{
				return new ApiKeyValidationResult(isValid: false, "Invalid API key format. Please copy the full key from https://account.arena.net/applications");
			}
			return new ApiKeyValidationResult(isValid: true, null);
		}

		public async Task<AuthVerifyResponse> VerifyApiKeyAsync(string apiKey)
		{
			ApiKeyValidationResult validation = ValidateApiKeyFormat(apiKey);
			if (!validation.IsValid)
			{
				Logger.Warn("VerifyApiKeyAsync: {ErrorMessage}", new object[1] { validation.ErrorMessage });
				ResetAuthState();
				return new AuthVerifyResponse
				{
					Valid = false,
					Message = validation.ErrorMessage
				};
			}
			string apiKey2 = apiKey.Trim();
			string url = "https://www.gw2opus.com/wp-json/blishhud/v1/auth/verify";
			AuthVerifyRequest request = new AuthVerifyRequest(apiKey2);
			try
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject((object)request), Encoding.UTF8, "application/json");
				string responseBody = await (await _httpClient.PostAsync(url, (HttpContent)(object)content).ConfigureAwait(continueOnCapturedContext: false)).get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrEmpty(responseBody))
				{
					Logger.Warn("VerifyApiKeyAsync: Response was null or empty");
					ResetAuthState();
					return null;
				}
				AuthVerifyResponse authResponse = JsonConvert.DeserializeObject<AuthVerifyResponse>(responseBody);
				if (authResponse != null)
				{
					UpdateAuthState(apiKey, authResponse);
				}
				return authResponse;
			}
			catch (HttpRequestException val)
			{
				HttpRequestException ex4 = val;
				Logger.Warn((Exception)(object)ex4, "VerifyApiKeyAsync: HTTP request failed");
				ResetAuthState();
				return null;
			}
			catch (TaskCanceledException ex3)
			{
				Logger.Warn((Exception)ex3, "VerifyApiKeyAsync: Request timed out");
				ResetAuthState();
				return null;
			}
			catch (JsonException val2)
			{
				JsonException ex2 = val2;
				Logger.Warn((Exception)(object)ex2, "VerifyApiKeyAsync: JSON deserialization failed");
				ResetAuthState();
				return null;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "VerifyApiKeyAsync: Unexpected error");
				ResetAuthState();
				return null;
			}
		}

		private void UpdateAuthState(string apiKey, AuthVerifyResponse response)
		{
			bool isOpusMember = _isOpusMember;
			_currentApiKey = apiKey;
			_isVerified = response.Valid;
			_isOpusMember = response.Valid && response.InOpusGuild;
			_accountName = response.AccountName;
			if (isOpusMember != _isOpusMember)
			{
				OnAuthStatusChanged();
			}
			if (_isOpusMember)
			{
				Logger.Info("GuildAuthService: User {AccountName} verified as OPUS guild member", new object[1] { _accountName });
			}
			else if (_isVerified)
			{
				Logger.Info("GuildAuthService: User {AccountName} verified but not an OPUS member", new object[1] { _accountName });
			}
		}

		private void ResetAuthState()
		{
			bool isOpusMember = _isOpusMember;
			_currentApiKey = null;
			_isVerified = false;
			_isOpusMember = false;
			_accountName = null;
			if (isOpusMember != _isOpusMember)
			{
				OnAuthStatusChanged();
			}
		}

		public void ClearAuth()
		{
			ResetAuthState();
			Logger.Info("GuildAuthService: Authentication cleared");
		}

		private void OnAuthStatusChanged()
		{
			this.AuthStatusChanged?.Invoke(this, new GuildAuthStatusChangedEventArgs(_isOpusMember, _accountName));
		}

		public void Dispose()
		{
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
