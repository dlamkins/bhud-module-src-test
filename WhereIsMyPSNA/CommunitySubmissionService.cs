using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;

namespace WhereIsMyPSNA
{
	internal class CommunitySubmissionService
	{
		private class DisclaimerState
		{
			[JsonProperty("acknowledgedDate")]
			public string AcknowledgedDate { get; set; }
		}

		private class SubmitResponse
		{
			[JsonProperty("ok")]
			public bool Ok { get; set; }

			[JsonProperty("error")]
			public string Error { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<CommunitySubmissionService>();

		private static readonly HttpClient Http;

		private const string DisclaimerFileName = "community_disclaimer.json";

		private readonly string _disclaimerFilePath;

		private const string WebAppUrl = "https://script.google.com/macros/s/AKfycbyw8Z1rsiPY9a84jGuktdIwobguD4g23adUqTQ-oyoCI5GsSvBUxo31P4aPfKR4EUnr/exec";

		private const string SheetCsvUrl = "https://docs.google.com/spreadsheets/d/1HhqiYwfJU6JLihX0-OhDUpGxsuHKdXRzGbAn17o5c_c/export?format=csv&gid=0";

		private static readonly Dictionary<string, string> ShortNpcNames;

		private static string TodayUtc => DateTime.UtcNow.ToString("yyyy-MM-dd");

		public bool DisclaimerAcknowledged { get; private set; }

		public bool IsConfigured => !string.IsNullOrEmpty("https://script.google.com/macros/s/AKfycbyw8Z1rsiPY9a84jGuktdIwobguD4g23adUqTQ-oyoCI5GsSvBUxo31P4aPfKR4EUnr/exec");

		public CommunitySubmissionService(DirectoriesManager directoriesManager)
		{
			string dir = ((directoriesManager != null) ? directoriesManager.GetFullDirectoryPath("wimpsna") : null);
			if (string.IsNullOrEmpty(dir))
			{
				return;
			}
			_disclaimerFilePath = Path.Combine(dir, "community_disclaimer.json");
			try
			{
				if (File.Exists(_disclaimerFilePath))
				{
					DisclaimerAcknowledged = JsonConvert.DeserializeObject<DisclaimerState>(File.ReadAllText(_disclaimerFilePath))?.AcknowledgedDate == TodayUtc;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to read community disclaimer state.");
			}
		}

		public void AcknowledgeDisclaimer()
		{
			DisclaimerAcknowledged = true;
			if (_disclaimerFilePath != null)
			{
				try
				{
					File.WriteAllText(_disclaimerFilePath, JsonConvert.SerializeObject((object)new DisclaimerState
					{
						AcknowledgedDate = TodayUtc
					}));
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to persist community disclaimer state.");
				}
			}
		}

		public bool TryGetShortName(string npc, out string shortNpc)
		{
			return ShortNpcNames.TryGetValue(npc, out shortNpc);
		}

		public async Task<Dictionary<string, int>> FetchCurrentSubmissionsAsync()
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			try
			{
				string[] lines = (await Http.GetStringAsync("https://docs.google.com/spreadsheets/d/1HhqiYwfJU6JLihX0-OhDUpGxsuHKdXRzGbAn17o5c_c/export?format=csv&gid=0")).Split('\n');
				if (lines.Length < 2)
				{
					return result;
				}
				string[] names = lines[0].Split(',');
				string[] ids = lines[1].Split(',');
				for (int col = 0; col < names.Length && col < ids.Length; col++)
				{
					string npc = names[col].Trim().Trim('"');
					if (int.TryParse(ids[col].Trim().Trim('"'), out var id) && id > 0)
					{
						result[npc] = id;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch current community submissions.");
			}
			return result;
		}

		public async Task<bool> SubmitAsync(string npc, int recipeSheetId)
		{
			if (!IsConfigured)
			{
				return false;
			}
			if (!ShortNpcNames.TryGetValue(npc, out var shortNpc))
			{
				return false;
			}
			var payload = new
			{
				npc = shortNpc,
				itemId = recipeSheetId
			};
			try
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject((object)payload), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await Http.PostAsync("https://script.google.com/macros/s/AKfycbyw8Z1rsiPY9a84jGuktdIwobguD4g23adUqTQ-oyoCI5GsSvBUxo31P4aPfKR4EUnr/exec", (HttpContent)(object)content);
				if (!response.get_IsSuccessStatusCode())
				{
					Logger.Warn($"Community submission failed: {(int)response.get_StatusCode()} {response.get_ReasonPhrase()}");
					return false;
				}
				SubmitResponse result = JsonConvert.DeserializeObject<SubmitResponse>(await response.get_Content().ReadAsStringAsync());
				if (result == null || !result.Ok)
				{
					Logger.Warn("Community submission rejected: " + (result?.Error ?? "unknown error"));
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to submit community recipe report.");
				return false;
			}
		}

		static CommunitySubmissionService()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			HttpClientHandler val = new HttpClientHandler();
			val.set_UseProxy(false);
			HttpClient val2 = new HttpClient((HttpMessageHandler)val);
			val2.set_Timeout(TimeSpan.FromSeconds(10.0));
			Http = val2;
			ShortNpcNames = new Dictionary<string, string>
			{
				["Mehem the Traveled"] = "Mehem",
				["The Fox"] = "Fox",
				["Lady Derwena"] = "Derwena",
				["Specialist Yana"] = "Yana",
				["Despina Katelyn"] = "Katelyn",
				["Verma Giftrender"] = "Verma"
			};
		}
	}
}
