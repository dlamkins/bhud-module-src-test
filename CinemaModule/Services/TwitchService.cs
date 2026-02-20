using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using CinemaModule.Models;
using Newtonsoft.Json.Linq;

namespace CinemaModule.Services
{
	public class TwitchService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<TwitchService>();

		private const string TwitchGqlUrl = "https://gql.twitch.tv/gql";

		private const string TwitchHelixUrl = "https://api.twitch.tv/helix";

		private const string TwitchClientId = "kimne78kx3ncx6brgo4mv6wki5h1ko";

		private const string TwitchAuthClientId = "8m7h0mxthjx16qofx82mruz640ke67";

		private const string TwitchUsherUrl = "https://usher.ttvnw.net/api/channel/hls";

		private const string AvatarCacheSubfolder = "avatars";

		private readonly HttpClient _httpClient;

		private readonly ImageCacheService _imageCache;

		private List<TwitchStreamQuality> _cachedQualities = new List<TwitchStreamQuality>();

		private string _cachedQualitiesChannel;

		private int _selectedQualityIndex;

		private bool _isFetchingQualities;

		private string _authToken;

		private string _userId;

		public ImageCacheService ImageCache => _imageCache;

		public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);

		public IReadOnlyList<TwitchStreamQuality> CachedQualities => _cachedQualities;

		public event EventHandler<TwitchQualitiesEventArgs> QualitiesChanged;

		public event EventHandler ScopeError;

		public TwitchService(string cacheDirectory)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			_httpClient = new HttpClient();
			string avatarCacheDir = Path.Combine(cacheDirectory, "avatars");
			_imageCache = new ImageCacheService(avatarCacheDir, _httpClient);
		}

		public void SetAuthToken(string token, string userId = null)
		{
			_authToken = token;
			_userId = userId;
		}

		public async Task<TwitchStreamInfo> GetStreamInfoAsync(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName))
			{
				return null;
			}
			try
			{
				JObject query = BuildStreamInfoQuery(channelName);
				JObject json = await ExecuteGqlRequestAsync(query, "GetStreamInfo");
				if (json == null)
				{
					return null;
				}
				return ParseStreamInfo(json, channelName);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get stream info for channel: " + channelName);
				return null;
			}
		}

		public async Task<Dictionary<string, TwitchStreamInfo>> GetMultipleStreamInfoAsync(List<string> channelNames)
		{
			Dictionary<string, TwitchStreamInfo> result = new Dictionary<string, TwitchStreamInfo>(StringComparer.OrdinalIgnoreCase);
			if (channelNames == null || channelNames.Count == 0)
			{
				return result;
			}
			List<string> validChannels = (from name in channelNames
				where !string.IsNullOrWhiteSpace(name)
				select name.ToLowerInvariant()).Distinct().ToList();
			if (validChannels.Count == 0)
			{
				return result;
			}
			try
			{
				JObject query = BuildMultipleStreamInfoQuery(validChannels);
				JObject json = await ExecuteGqlRequestAsync(query, "GetMultipleStreamInfo");
				if (json == null)
				{
					return result;
				}
				return ParseMultipleStreamInfo(json, validChannels);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Failed to get stream info for {validChannels.Count} channels");
				return result;
			}
		}

		public async Task<List<TwitchStreamInfo>> GetFollowedChannelsAsync()
		{
			if (!IsAuthenticated || string.IsNullOrEmpty(_userId))
			{
				return new List<TwitchStreamInfo>();
			}
			try
			{
				return await GetFollowedLiveStreamsHelixAsync();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get followed channels");
				return new List<TwitchStreamInfo>();
			}
		}

		private async Task<List<TwitchStreamInfo>> GetFollowedLiveStreamsHelixAsync()
		{
			string url = "https://api.twitch.tv/helix/streams/followed?user_id=" + _userId + "&first=50";
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), url);
			try
			{
				((HttpHeaders)request.get_Headers()).Add("Client-ID", "8m7h0mxthjx16qofx82mruz640ke67");
				((HttpHeaders)request.get_Headers()).Add("Authorization", "Bearer " + _authToken);
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				if (!response.get_IsSuccessStatusCode())
				{
					string error = await response.get_Content().ReadAsStringAsync();
					Logger.Warn("Failed to get followed streams: " + error);
					if (error.Contains("Missing scope"))
					{
						Logger.Info("Token missing required scope - triggering re-authentication");
						this.ScopeError?.Invoke(this, EventArgs.Empty);
					}
					return new List<TwitchStreamInfo>();
				}
				JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
				List<TwitchStreamInfo> streams = ParseHelixFollowedStreams(json);
				return await EnrichWithUserAvatars(streams);
			}
			finally
			{
				((IDisposable)request)?.Dispose();
			}
		}

		private async Task<List<TwitchStreamInfo>> EnrichWithUserAvatars(List<TwitchStreamInfo> streams)
		{
			if (streams.Count == 0)
			{
				return streams;
			}
			List<string> userIds = (from s in streams
				select s.UserId into id
				where !string.IsNullOrEmpty(id)
				select id).Distinct().ToList();
			if (userIds.Count == 0)
			{
				return streams;
			}
			string url = "https://api.twitch.tv/helix/users?" + string.Join("&", userIds.Select((string id) => "id=" + id));
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), url);
			try
			{
				((HttpHeaders)request.get_Headers()).Add("Client-ID", "8m7h0mxthjx16qofx82mruz640ke67");
				((HttpHeaders)request.get_Headers()).Add("Authorization", "Bearer " + _authToken);
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				if (!response.get_IsSuccessStatusCode())
				{
					return streams;
				}
				JToken obj = JObject.Parse(await response.get_Content().ReadAsStringAsync()).get_Item("data");
				JArray users = (JArray)(object)((obj is JArray) ? obj : null);
				if (users == null)
				{
					return streams;
				}
				Dictionary<string, string> avatarMap = new Dictionary<string, string>();
				foreach (JToken item in users)
				{
					string id2 = ((object)item.get_Item((object)"id"))?.ToString();
					string avatar = ((object)item.get_Item((object)"profile_image_url"))?.ToString();
					if (!string.IsNullOrEmpty(id2) && !string.IsNullOrEmpty(avatar))
					{
						avatarMap[id2] = avatar;
					}
				}
				foreach (TwitchStreamInfo stream in streams)
				{
					if (!string.IsNullOrEmpty(stream.UserId) && avatarMap.TryGetValue(stream.UserId, out var avatarUrl))
					{
						stream.AvatarUrl = avatarUrl;
					}
				}
			}
			finally
			{
				((IDisposable)request)?.Dispose();
			}
			return streams;
		}

		private List<TwitchStreamInfo> ParseHelixFollowedStreams(JObject json)
		{
			List<TwitchStreamInfo> result = new List<TwitchStreamInfo>();
			JToken obj = json.get_Item("data");
			JArray data = (JArray)(object)((obj is JArray) ? obj : null);
			if (data == null)
			{
				return result;
			}
			foreach (JToken stream in data)
			{
				string login = ((object)stream.get_Item((object)"user_login"))?.ToString();
				if (!string.IsNullOrEmpty(login))
				{
					TwitchStreamInfo obj2 = new TwitchStreamInfo
					{
						ChannelName = login,
						UserId = ((object)stream.get_Item((object)"user_id"))?.ToString(),
						IsLive = true,
						Title = ((object)stream.get_Item((object)"title"))?.ToString(),
						GameName = ((object)stream.get_Item((object)"game_name"))?.ToString()
					};
					JToken obj3 = stream.get_Item((object)"viewer_count");
					obj2.ViewerCount = ((obj3 != null) ? Extensions.Value<int>((IEnumerable<JToken>)obj3) : 0);
					result.Add(obj2);
				}
			}
			return result;
		}

		public async Task<string> GetPlayableStreamUrlAsync(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName))
			{
				return null;
			}
			try
			{
				StreamAccessToken accessToken = await GetStreamAccessTokenAsync(channelName);
				if (accessToken == null)
				{
					Logger.Warn("Could not get access token for channel: " + channelName);
					return null;
				}
				return BuildHlsUrl(channelName, accessToken.Token, accessToken.Signature);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get playable stream URL for channel: " + channelName);
				return null;
			}
		}

		public async Task<List<TwitchStreamQuality>> GetStreamQualitiesAsync(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName))
			{
				return new List<TwitchStreamQuality>();
			}
			try
			{
				string masterPlaylistUrl = await GetPlayableStreamUrlAsync(channelName);
				if (string.IsNullOrEmpty(masterPlaylistUrl))
				{
					Logger.Warn("Could not get master playlist URL for channel: " + channelName);
					return new List<TwitchStreamQuality>();
				}
				return ParseM3U8Playlist(await _httpClient.GetStringAsync(masterPlaylistUrl));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get stream qualities for channel: " + channelName);
				return new List<TwitchStreamQuality>();
			}
		}

		public async void FetchAndCacheQualitiesAsync(string channelName)
		{
			if (_isFetchingQualities || string.IsNullOrWhiteSpace(channelName))
			{
				return;
			}
			_isFetchingQualities = true;
			try
			{
				List<TwitchStreamQuality> qualities = await GetStreamQualitiesAsync(channelName);
				if (qualities.Count > 0)
				{
					_cachedQualities = qualities;
					_cachedQualitiesChannel = channelName;
					_selectedQualityIndex = 0;
					List<string> qualityNames = _cachedQualities.Select((TwitchStreamQuality q) => q.DisplayName).ToList();
					this.QualitiesChanged?.Invoke(this, new TwitchQualitiesEventArgs(qualityNames, _selectedQualityIndex));
				}
				else
				{
					Logger.Warn("No quality options found for Twitch channel: " + channelName);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to fetch Twitch qualities for channel: " + channelName);
			}
			finally
			{
				_isFetchingQualities = false;
			}
		}

		public string SelectQuality(int qualityIndex)
		{
			if (qualityIndex < 0 || qualityIndex >= _cachedQualities.Count)
			{
				Logger.Warn($"Invalid quality index: {qualityIndex}");
				return null;
			}
			_selectedQualityIndex = qualityIndex;
			return _cachedQualities[qualityIndex].StreamUrl;
		}

		public void ClearCachedQualities()
		{
			_cachedQualities.Clear();
			_cachedQualitiesChannel = null;
			_selectedQualityIndex = 0;
		}

		private List<TwitchStreamQuality> ParseM3U8Playlist(string playlistContent)
		{
			List<TwitchStreamQuality> qualities = new List<TwitchStreamQuality>();
			if (string.IsNullOrEmpty(playlistContent))
			{
				return qualities;
			}
			string[] lines = playlistContent.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			Regex resolutionRegex = new Regex("RESOLUTION=(\\d+x\\d+)", RegexOptions.Compiled);
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i].Trim();
				if (!line.StartsWith("#EXT-X-STREAM-INF:"))
				{
					continue;
				}
				string attributes = line.Substring("#EXT-X-STREAM-INF:".Length);
				string streamUrl = null;
				for (int j = i + 1; j < lines.Length; j++)
				{
					string nextLine = lines[j].Trim();
					if (!nextLine.StartsWith("#") && !string.IsNullOrEmpty(nextLine))
					{
						streamUrl = nextLine;
						break;
					}
				}
				if (string.IsNullOrEmpty(streamUrl))
				{
					continue;
				}
				int height = 0;
				int frameRate = 0;
				string name = null;
				bool isAudioOnly = false;
				bool isSource = false;
				Match resolutionMatch = resolutionRegex.Match(attributes);
				if (resolutionMatch.Success)
				{
					string[] parts = resolutionMatch.Groups[1].Value.Split('x');
					if (parts.Length == 2 && int.TryParse(parts[1], out var h))
					{
						height = h;
					}
				}
				Match frameRateMatch = Regex.Match(attributes, "FRAME-RATE=([\\d.]+)");
				if (frameRateMatch.Success && double.TryParse(frameRateMatch.Groups[1].Value, out var fps))
				{
					frameRate = (int)Math.Round(fps);
				}
				Match bandwidthMatch = Regex.Match(attributes, "BANDWIDTH=(\\d+)");
				if (bandwidthMatch.Success)
				{
					long.Parse(bandwidthMatch.Groups[1].Value);
				}
				Match videoMatch = Regex.Match(attributes, "VIDEO=\"([^\"]+)\"");
				if (videoMatch.Success)
				{
					name = videoMatch.Groups[1].Value;
				}
				isAudioOnly = (name != null && name.IndexOf("audio", StringComparison.OrdinalIgnoreCase) >= 0) || height == 0;
				isSource = name != null && name.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0;
				string displayName = BuildQualityDisplayName(isAudioOnly, isSource, height, frameRate, name);
				qualities.Add(new TwitchStreamQuality(displayName, streamUrl));
			}
			return (from q in qualities
				orderby q.DisplayName.StartsWith("Source") descending, ExtractHeightFromDisplayName(q.DisplayName) descending
				select q).ToList();
		}

		private string BuildQualityDisplayName(bool isAudioOnly, bool isSource, int height, int frameRate, string name)
		{
			if (isAudioOnly)
			{
				return "Audio Only";
			}
			if (isSource)
			{
				if (height > 0 && frameRate > 0)
				{
					return $"Source ({height}p{frameRate})";
				}
				if (height > 0)
				{
					return $"Source ({height}p)";
				}
				return "Source";
			}
			if (height > 0)
			{
				if (frameRate > 0 && frameRate != 30)
				{
					return $"{height}p{frameRate}";
				}
				return $"{height}p";
			}
			return name ?? "Unknown";
		}

		private int ExtractHeightFromDisplayName(string displayName)
		{
			Match match = Regex.Match(displayName, "(\\d+)p");
			if (!match.Success)
			{
				return 0;
			}
			return int.Parse(match.Groups[1].Value);
		}

		private async Task<StreamAccessToken> GetStreamAccessTokenAsync(string channelName)
		{
			JObject query = BuildPlaybackAccessTokenQuery(channelName);
			JObject json = await ExecuteGqlRequestAsync(query, "PlaybackAccessToken", useAuth: true);
			if (json == null)
			{
				return null;
			}
			return ParseAccessToken(json, channelName);
		}

		private string BuildHlsUrl(string channelName, string token, string signature)
		{
			int random = new Random().Next(1000000, 9999999);
			return "https://usher.ttvnw.net/api/channel/hls/" + channelName.ToLowerInvariant() + ".m3u8?allow_source=true&allow_audio_only=true&fast_bread=true" + $"&p={random}" + "&player_backend=mediaplayer&playlist_include_framerate=true&reassignments_supported=true&sig=" + signature + "&supported_codecs=avc1&token=" + Uri.EscapeDataString(token) + "&cdm=wv";
		}

		public string GetChannelUrl(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName))
			{
				return null;
			}
			return "https://www.twitch.tv/" + channelName.ToLowerInvariant();
		}

		public bool OpenTwitchChat(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName))
			{
				Logger.Warn("Cannot open Twitch chat - channel name is empty");
				return false;
			}
			string chatUrl = "https://www.twitch.tv/popout/" + channelName.ToLowerInvariant() + "/chat?popout=";
			try
			{
				Process.Start(chatUrl);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to open Twitch chat URL: {0}", new object[1] { chatUrl });
				return false;
			}
		}

		public async Task<UrlAvailabilityResult> CheckUrlAvailabilityAsync(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return new UrlAvailabilityResult
				{
					IsAvailable = false,
					StatusMessage = "No URL"
				};
			}
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Head(), url);
				try
				{
					HttpResponseMessage response = await _httpClient.SendAsync(request, (HttpCompletionOption)1);
					try
					{
						return response.get_IsSuccessStatusCode() ? new UrlAvailabilityResult
						{
							IsAvailable = true,
							StatusMessage = "Available"
						} : new UrlAvailabilityResult
						{
							IsAvailable = false,
							StatusMessage = $"Unavailable ({(int)response.get_StatusCode()})"
						};
					}
					finally
					{
						((IDisposable)response)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception)
			{
				return new UrlAvailabilityResult
				{
					IsAvailable = null,
					StatusMessage = "Unknown"
				};
			}
		}

		public async Task<AsyncTexture2D> GetAvatarTextureAsync(string cacheKey, string avatarUrl)
		{
			return await _imageCache.GetImageAsync(cacheKey, avatarUrl);
		}

		private JObject BuildStreamInfoQuery(string channelName)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0041: Expected O, but got Unknown
			JObject val = new JObject();
			val.set_Item("query", JToken.op_Implicit("\n                    query GetStreamInfo($login: String!) {\n                        user(login: $login) {\n                            id\n                            login\n                            displayName\n                            profileImageURL(width: 600)\n                            stream {\n                                id\n                                title\n                                viewersCount\n                                game {\n                                    id\n                                    name\n                                }\n                            }\n                        }\n                    }"));
			JObject val2 = new JObject();
			val2.set_Item("login", JToken.op_Implicit(channelName.ToLowerInvariant()));
			val.set_Item("variables", (JToken)val2);
			return val;
		}

		private JObject BuildMultipleStreamInfoQuery(List<string> channelNames)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Expected O, but got Unknown
			JArray loginsArray = new JArray((object)channelNames);
			JObject val = new JObject();
			val.set_Item("query", JToken.op_Implicit("\n                    query GetMultipleStreamInfo($logins: [String!]!) {\n                        users(logins: $logins) {\n                            id\n                            login\n                            displayName\n                            profileImageURL(width: 600)\n                            stream {\n                                id\n                                title\n                                viewersCount\n                                game {\n                                    id\n                                    name\n                                }\n                            }\n                        }\n                    }"));
			JObject val2 = new JObject();
			val2.set_Item("logins", (JToken)(object)loginsArray);
			val.set_Item("variables", (JToken)val2);
			return val;
		}

		private JObject BuildPlaybackAccessTokenQuery(string channelName)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a2: Expected O, but got Unknown
			JObject val = new JObject();
			val.set_Item("operationName", JToken.op_Implicit("PlaybackAccessToken_Template"));
			val.set_Item("query", JToken.op_Implicit("\n                    query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, $vodID: ID!, $isVod: Boolean!, $playerType: String!) {\n                        streamPlaybackAccessToken(channelName: $login, params: {platform: \"web\", playerBackend: \"mediaplayer\", playerType: $playerType}) @include(if: $isLive) {\n                            value\n                            signature\n                            __typename\n                        }\n                        videoPlaybackAccessToken(id: $vodID, params: {platform: \"web\", playerBackend: \"mediaplayer\", playerType: $playerType}) @include(if: $isVod) {\n                            value\n                            signature\n                            __typename\n                        }\n                    }"));
			JObject val2 = new JObject();
			val2.set_Item("isLive", JToken.op_Implicit(true));
			val2.set_Item("login", JToken.op_Implicit(channelName.ToLowerInvariant()));
			val2.set_Item("isVod", JToken.op_Implicit(false));
			val2.set_Item("vodID", JToken.op_Implicit(""));
			val2.set_Item("playerType", JToken.op_Implicit("site"));
			val.set_Item("variables", (JToken)val2);
			return val;
		}

		private Task<JObject> ExecuteGqlRequestAsync(JObject query, string operationName)
		{
			return ExecuteGqlRequestAsync(query, operationName, useAuth: false);
		}

		private async Task<JObject> ExecuteGqlRequestAsync(JObject query, string operationName, bool useAuth)
		{
			HttpRequestMessage val = new HttpRequestMessage(HttpMethod.get_Post(), "https://gql.twitch.tv/gql");
			val.set_Content((HttpContent)new StringContent(((object)query).ToString(), Encoding.UTF8, "application/json"));
			HttpRequestMessage request = val;
			((HttpHeaders)request.get_Headers()).Add("Client-ID", "kimne78kx3ncx6brgo4mv6wki5h1ko");
			if (useAuth && !string.IsNullOrEmpty(_authToken))
			{
				((HttpHeaders)request.get_Headers()).Add("Authorization", "Bearer " + _authToken);
			}
			HttpResponseMessage response = await _httpClient.SendAsync(request);
			if (!response.get_IsSuccessStatusCode())
			{
				string errorContent = await response.get_Content().ReadAsStringAsync();
				Logger.Warn("GQL " + operationName + " request failed: " + errorContent);
				return null;
			}
			JObject json = JObject.Parse(await response.get_Content().ReadAsStringAsync());
			LogGqlErrors(json, operationName);
			return json;
		}

		private void LogGqlErrors(JObject json, string operationName)
		{
			JToken obj = json.get_Item("errors");
			JArray errors = (JArray)(object)((obj is JArray) ? obj : null);
			if (errors != null && ((JContainer)errors).get_Count() > 0)
			{
				Logger.Warn($"GQL {operationName} returned errors: {errors}");
			}
		}

		private TwitchStreamInfo ParseStreamInfo(JObject json, string channelName)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Invalid comparison between Unknown and I4
			JToken obj = json.get_Item("data");
			JToken user = ((obj != null) ? obj.get_Item((object)"user") : null);
			if (user == null || (int)user.get_Type() == 10)
			{
				return new TwitchStreamInfo
				{
					ChannelName = channelName,
					IsLive = false
				};
			}
			JToken stream = user.get_Item((object)"stream");
			bool isLive = stream != null && (int)stream.get_Type() != 10;
			TwitchStreamInfo obj2 = new TwitchStreamInfo
			{
				ChannelName = channelName,
				IsLive = isLive,
				Title = ((!isLive) ? null : ((object)stream.get_Item((object)"title"))?.ToString())
			};
			object gameName;
			if (!isLive)
			{
				gameName = null;
			}
			else
			{
				JToken obj3 = stream.get_Item((object)"game");
				gameName = ((obj3 == null) ? null : ((object)obj3.get_Item((object)"name"))?.ToString());
			}
			obj2.GameName = (string)gameName;
			int viewerCount;
			if (!isLive)
			{
				viewerCount = 0;
			}
			else
			{
				JToken obj4 = stream.get_Item((object)"viewersCount");
				viewerCount = ((obj4 != null) ? Extensions.Value<int>((IEnumerable<JToken>)obj4) : 0);
			}
			obj2.ViewerCount = viewerCount;
			obj2.AvatarUrl = ((object)user.get_Item((object)"profileImageURL"))?.ToString();
			return obj2;
		}

		private Dictionary<string, TwitchStreamInfo> ParseMultipleStreamInfo(JObject json, List<string> requestedChannels)
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Invalid comparison between Unknown and I4
			Dictionary<string, TwitchStreamInfo> result = new Dictionary<string, TwitchStreamInfo>(StringComparer.OrdinalIgnoreCase);
			JToken obj = json.get_Item("data");
			JToken obj2 = ((obj != null) ? obj.get_Item((object)"users") : null);
			JArray users = (JArray)(object)((obj2 is JArray) ? obj2 : null);
			if (users != null)
			{
				foreach (JToken user in users)
				{
					string login = ((object)user.get_Item((object)"login"))?.ToString();
					if (!string.IsNullOrEmpty(login))
					{
						JToken stream = user.get_Item((object)"stream");
						bool isLive = stream != null && (int)stream.get_Type() != 10;
						TwitchStreamInfo obj3 = new TwitchStreamInfo
						{
							ChannelName = login,
							IsLive = isLive,
							Title = ((!isLive) ? null : ((object)stream.get_Item((object)"title"))?.ToString())
						};
						object gameName;
						if (!isLive)
						{
							gameName = null;
						}
						else
						{
							JToken obj4 = stream.get_Item((object)"game");
							gameName = ((obj4 == null) ? null : ((object)obj4.get_Item((object)"name"))?.ToString());
						}
						obj3.GameName = (string)gameName;
						int viewerCount;
						if (!isLive)
						{
							viewerCount = 0;
						}
						else
						{
							JToken obj5 = stream.get_Item((object)"viewersCount");
							viewerCount = ((obj5 != null) ? Extensions.Value<int>((IEnumerable<JToken>)obj5) : 0);
						}
						obj3.ViewerCount = viewerCount;
						obj3.AvatarUrl = ((object)user.get_Item((object)"profileImageURL"))?.ToString();
						result[login] = obj3;
					}
				}
			}
			foreach (string channelName in requestedChannels)
			{
				if (!result.ContainsKey(channelName))
				{
					result[channelName] = new TwitchStreamInfo
					{
						ChannelName = channelName,
						IsLive = false
					};
				}
			}
			return result;
		}

		private StreamAccessToken ParseAccessToken(JObject json, string channelName)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			JToken obj = json.get_Item("data");
			JToken tokenData = ((obj != null) ? obj.get_Item((object)"streamPlaybackAccessToken") : null);
			if (tokenData == null || (int)tokenData.get_Type() == 10)
			{
				Logger.Warn("streamPlaybackAccessToken is null for channel: " + channelName);
				return null;
			}
			string token = ((object)tokenData.get_Item((object)"value"))?.ToString();
			string signature = ((object)tokenData.get_Item((object)"signature"))?.ToString();
			if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(signature))
			{
				Logger.Warn("Token or signature is empty for channel: " + channelName);
				return null;
			}
			return new StreamAccessToken
			{
				Token = token,
				Signature = signature
			};
		}

		public void Dispose()
		{
			_imageCache?.Dispose();
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
