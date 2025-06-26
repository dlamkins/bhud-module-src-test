using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Settings.Controls;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class GeoServerWrapper : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<GeoServerWrapper>();

		private static readonly HttpClient HttpClient = new HttpClient();

		private readonly string _baseUrl;

		private HelpScreen? HelpScreenData;

		private readonly ConfigModel _config;

		private readonly GeoGuessWindow _geoGuessWindow;

		private readonly SettingsWindow _settingsWindow;

		public GeoServerWrapper(string baseUrl, Module module, ConfigModel config, GeoGuessWindow geoGuessWindow, SettingsWindow settingsWindow)
		{
			_baseUrl = baseUrl;
			_config = config;
			_geoGuessWindow = geoGuessWindow;
			_settingsWindow = settingsWindow;
		}

		private async Task<T?> Fetch<T>(string url, Action<T?>? action = null)
		{
			_ = 3;
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Get(), url);
				string subtoken = await GW2ApiService.GetValidSubtoken();
				if (!string.IsNullOrEmpty(subtoken))
				{
					request.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", subtoken));
				}
				HttpResponseMessage response = await HttpClient.SendAsync(request);
				if (response.get_StatusCode() == HttpStatusCode.Forbidden)
				{
					string content = await response.get_Content().ReadAsStringAsync();
					Logger.Warn("Received 403 Forbidden from " + url + ": " + content);
					if (url.Contains("/check_user"))
					{
						UserCheckServerResponse obj = new UserCheckServerResponse
						{
							Banned = true,
							VersionCheck = true,
							Account = ""
						};
						Ban ban = new Ban();
						Account? account = Service.UserManager.Account;
						ban.AccountName = ((account != null) ? account!.get_Name() : null) ?? "Unknown";
						ban.Reason = "Account banned by server";
						ban.Moderator = "System";
						ban.BannedOn = DateTime.Now;
						obj.Ban = ban;
						UserCheckServerResponse banResponse = obj;
						try
						{
							Dictionary<string, object> errorResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
							if (errorResponse != null && errorResponse.ContainsKey("error") && errorResponse["error"]?.ToString() == "Account banned")
							{
								banResponse.Ban.Reason = ((!errorResponse.ContainsKey("message")) ? "Account banned by server" : (errorResponse["message"]?.ToString() ?? "Account banned by server"));
							}
						}
						catch
						{
						}
						action?.Invoke((T)(object)banResponse);
						return (T)(object)banResponse;
					}
					response.EnsureSuccessStatusCode();
				}
				else
				{
					response.EnsureSuccessStatusCode();
				}
				T result = JsonConvert.DeserializeObject<T>(await response.get_Content().ReadAsStringAsync());
				action?.Invoke(result);
				return result;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch data from " + url);
				action?.Invoke(default(T));
				return default(T);
			}
		}

		private async Task<T?> Post<T>(string url, object data, Action<T?>? action = null)
		{
			_ = 2;
			try
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
				HttpRequestMessage val = new HttpRequestMessage(HttpMethod.get_Post(), url);
				val.set_Content((HttpContent)(object)content);
				HttpRequestMessage request = val;
				string subtoken = await GW2ApiService.GetValidSubtoken();
				if (!string.IsNullOrEmpty(subtoken))
				{
					request.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", subtoken));
					Logger.Debug("Added authorization header to POST request to " + url);
				}
				else
				{
					Logger.Warn("No valid subtoken available for POST request to " + url);
				}
				HttpResponseMessage obj = await HttpClient.SendAsync(request);
				obj.EnsureSuccessStatusCode();
				T result = JsonConvert.DeserializeObject<T>(await obj.get_Content().ReadAsStringAsync());
				action?.Invoke(result);
				return result;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to post data to " + url);
				action?.Invoke(default(T));
				return default(T);
			}
		}

		private async Task<T?> Delete<T>(string url, Action<T?>? action = null)
		{
			_ = 2;
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.get_Delete(), url);
				string subtoken = await GW2ApiService.GetValidSubtoken();
				if (!string.IsNullOrEmpty(subtoken))
				{
					request.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", subtoken));
				}
				HttpResponseMessage obj = await HttpClient.SendAsync(request);
				obj.EnsureSuccessStatusCode();
				T result = JsonConvert.DeserializeObject<T>(await obj.get_Content().ReadAsStringAsync());
				action?.Invoke(result);
				return result;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to delete data from " + url);
				action?.Invoke(default(T));
				return default(T);
			}
		}

		private async Task<T?> Put<T>(string url, object data, Action<T?>? action = null)
		{
			_ = 2;
			try
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
				HttpRequestMessage val = new HttpRequestMessage(HttpMethod.get_Put(), url);
				val.set_Content((HttpContent)(object)content);
				HttpRequestMessage request = val;
				string subtoken = await GW2ApiService.GetValidSubtoken();
				if (!string.IsNullOrEmpty(subtoken))
				{
					request.get_Headers().set_Authorization(new AuthenticationHeaderValue("Bearer", subtoken));
				}
				HttpResponseMessage obj = await HttpClient.SendAsync(request);
				obj.EnsureSuccessStatusCode();
				T result = JsonConvert.DeserializeObject<T>(await obj.get_Content().ReadAsStringAsync());
				action?.Invoke(result);
				return result;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to put data to " + url);
				action?.Invoke(default(T));
				return default(T);
			}
		}

		public async Task<HelpScreen?> GetHelpScreens()
		{
			if (HelpScreenData != null)
			{
				return HelpScreenData;
			}
			HelpScreenData = await Fetch<HelpScreen>(Service.Config.HelpScreenUrl);
			return HelpScreenData;
		}

		public string GetImageUrl(string uuid)
		{
			return (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/image/" + uuid;
		}

		public string GetPuzzleMapUrl(string puzzleId)
		{
			return (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId + "/map";
		}

		public string GetPuzzleMapWithGuessUrl(string puzzleId, int guessMapId, float[] guessCoords)
		{
			string basePath = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3"));
			return $"{basePath}/p/{puzzleId}/map?guess_map={guessMapId}&continent_x={guessCoords[0]}&continent_y={guessCoords[1]}";
		}

		public string GetGuildEmblemUrl(string guid)
		{
			return (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/emblem/" + guid;
		}

		public async Task<Guild?> GetGuildInfoAsync(string guildId)
		{
			string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/g/" + guildId;
			Logger.GetLogger<Module>().Info(url);
			return await Fetch<Guild>(url);
		}

		public async Task<List<Guild>> GetGuildsInfoAsync()
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/get-guild-list";
				Logger.Info("Requesting guild info from: " + url);
				List<Guild> resp = await Fetch<List<Guild>>(url);
				Logger.Info($"Received {resp?.Count ?? 0} guilds from server");
				return resp ?? new List<Guild>();
			}
			catch (Exception ex)
			{
				string basePath = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3"));
				Logger.Warn("Error fetching guildlist " + basePath + "/get-guild-list", new object[1] { ex.Message });
			}
			return new List<Guild>();
		}

		public async Task<Puzzle?> GetPuzzleInfoAsync(string guildId, string puzzleId)
		{
			string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId;
			return await Fetch<Puzzle>(url);
		}

		public async Task SubmitGeoGuess(string guildId, string puzzleId, GuessCreate guess, Action<Puzzle?>? action = null)
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId + "/guess";
				var v3Guess = new { guess.Location };
				Puzzle newModel = await Post<Puzzle>(url, v3Guess);
				if (newModel != null)
				{
					action?.Invoke(newModel);
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Error submitting puzzle-guess", new object[1] { ex.Message });
			}
			action?.Invoke(null);
		}

		public async Task SubmitPuzzleUpvote(string puzzleId, string accountName, Action<Puzzle?>? action = null)
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId + "/upvote";
				Puzzle newModel = await Post<Puzzle>(url, new object());
				if (newModel != null)
				{
					action?.Invoke(newModel);
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Error submitting puzzle upvote", new object[1] { ex.Message });
			}
			action?.Invoke(null);
		}

		public async Task DeleteGeoGame(string guildId, string puzzleId, Action<bool>? action = null)
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId;
				Puzzle result = await Delete<Puzzle>(url);
				action?.Invoke(result != null);
			}
			catch (Exception ex)
			{
				Logger.Warn("Error deleting puzzle", new object[1] { ex.Message });
				action?.Invoke(obj: false);
			}
		}

		public async Task SubmitNewPuzzle(string guildId, Puzzle game, Bitmap image, Action<string?>? action = null)
		{
			_ = 3;
			try
			{
				string basePath = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3"));
				string url = basePath + "/create";
				MultipartFormDataContent val = new MultipartFormDataContent();
				val.Add((HttpContent)new StringContent(guildId), "GuildId");
				val.Add((HttpContent)new StringContent(game.Duration.ToString()), "Duration");
				val.Add((HttpContent)new StringContent(game.Title), "Title");
				val.Add((HttpContent)new StringContent(JsonConvert.SerializeObject(game.Location)), "Location");
				MultipartFormDataContent formData = val;
				try
				{
					using MemoryStream imageStream = new MemoryStream();
					image.Save(imageStream, ImageFormat.Png);
					imageStream.Position = 0L;
					StreamContent imageContent = new StreamContent((Stream)imageStream);
					((HttpContent)imageContent).get_Headers().set_ContentType(new MediaTypeHeaderValue("image/png"));
					formData.Add((HttpContent)(object)imageContent, "image", "image.png");
					string subtoken = await GW2ApiService.GetValidSubtoken();
					if (!string.IsNullOrEmpty(subtoken))
					{
						HttpClient.get_DefaultRequestHeaders().set_Authorization(new AuthenticationHeaderValue("Bearer", subtoken));
					}
					HttpResponseMessage response = await HttpClient.PostAsync(url, (HttpContent)(object)formData);
					if (response.get_StatusCode() == HttpStatusCode.Forbidden)
					{
						string content = await response.get_Content().ReadAsStringAsync();
						Logger.Warn("Received 403 Forbidden from " + url + ": " + content);
						try
						{
							Dictionary<string, object> errorResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
							if (errorResponse != null && errorResponse.ContainsKey("error") && errorResponse["error"]?.ToString() == "Your account is banned.")
							{
								Logger.Warn("User is banned, cannot submit new puzzle");
								action?.Invoke(null);
								return;
							}
						}
						catch
						{
						}
						response.EnsureSuccessStatusCode();
					}
					else
					{
						response.EnsureSuccessStatusCode();
					}
					Puzzle result = JsonConvert.DeserializeObject<Puzzle>(await response.get_Content().ReadAsStringAsync());
					action?.Invoke(result?.Id);
				}
				finally
				{
					((IDisposable)formData)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Error submitting new puzzle", new object[1] { ex.Message });
				action?.Invoke(null);
			}
		}

		public async Task<UserCheckServerResponse> PerformUserCheck(string account_name, Action<UserCheckServerResponse>? action = null)
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/check_user?version=" + Module.MODULE_VERSION;
				UserCheckServerResponse result = await Fetch<UserCheckServerResponse>(url);
				action?.Invoke(result ?? new UserCheckServerResponse());
				return result ?? new UserCheckServerResponse();
			}
			catch (Exception ex)
			{
				Logger.Warn("Error performing user check", new object[1] { ex.Message });
				UserCheckServerResponse response = new UserCheckServerResponse();
				action?.Invoke(response);
				return response;
			}
		}

		public async Task<LeaderboardStats?> GetLeaderboardStatsAsync()
		{
			string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/leaderboards";
			return await Fetch<LeaderboardStats>(url);
		}

		public async Task RemoveGuessFromPuzzle(string puzzleId, string accountNameToRemove, string requestorAccountName, Action<Puzzle?>? action = null)
		{
			try
			{
				string basePath = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3"));
				string url = basePath + "/p/" + puzzleId + "/remove-guess/" + accountNameToRemove;
				Puzzle result = await Delete<Puzzle>(url);
				if (result != null)
				{
					action?.Invoke(result);
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Error removing guess from puzzle", new object[1] { ex.Message });
			}
			action?.Invoke(null);
		}

		public async Task UpdatePuzzleTitle(string puzzleId, string newTitle, string accountName, Action<Puzzle?>? action = null)
		{
			try
			{
				string url = (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/p/" + puzzleId;
				var updateData = new
				{
					title = newTitle
				};
				Puzzle result = await Put<Puzzle>(url, updateData);
				if (result != null)
				{
					action?.Invoke(result);
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Error updating puzzle title", new object[1] { ex.Message });
			}
			action?.Invoke(null);
		}

		public string GetSolutionMapUrl(string puzzleId, string accountName)
		{
			return (_baseUrl.EndsWith("/module/v3") ? _baseUrl : (_baseUrl + "/module/v3")) + "/solution-map/" + puzzleId;
		}

		public void Dispose()
		{
		}
	}
}
