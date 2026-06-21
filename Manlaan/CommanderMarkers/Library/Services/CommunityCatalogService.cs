using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class CommunityCatalogService
	{
		private sealed class CommunitySetsPage
		{
			[JsonProperty("total")]
			public int Total { get; set; }

			[JsonProperty("sets")]
			public List<CommunitySetSummary> Sets { get; set; } = new List<CommunitySetSummary>();

		}

		public const string IndexFileName = "community_index.json";

		private readonly CommanderMarkersManifestService _manifestService;

		private readonly string _moduleDirectory;

		private readonly List<CommunitySetSummary> _sets = new List<CommunitySetSummary>();

		private readonly List<CommunityCategoryEntry> _categories = new List<CommunityCategoryEntry>();

		private string _lastEdit = "";

		public IReadOnlyList<CommunitySetSummary> Sets => _sets;

		public IReadOnlyList<CommunityCategoryEntry> Categories => _categories;

		public event EventHandler? CatalogUpdated;

		public CommunityCatalogService(CommanderMarkersManifestService manifestService, string moduleDirectory)
		{
			_manifestService = manifestService;
			_moduleDirectory = moduleDirectory;
		}

		public void LoadCached()
		{
			string path = Path.Combine(_moduleDirectory, "community_index.json");
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				JObject i = JObject.Parse(File.ReadAllText(path));
				_lastEdit = i.Value<string>("lastEdit") ?? "";
				_sets.Clear();
				JArray setsArray = i["sets"] as JArray;
				if (setsArray == null)
				{
					return;
				}
				foreach (JToken item in setsArray)
				{
					CommunitySetSummary summary = item.ToObject<CommunitySetSummary>();
					if (summary != null)
					{
						_sets.Add(summary);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public bool SyncCatalog()
		{
			CommanderMarkersManifest manifest = _manifestService.Manifest;
			try
			{
				using WebClient client = new WebClient();
				string checkUrl = manifest.Absolute(manifest.CommunityCheckUrl);
				string remoteLastEdit = JObject.Parse(client.DownloadString(checkUrl)).Value<string>("lastEdit") ?? "";
				if (!string.IsNullOrEmpty(remoteLastEdit) && remoteLastEdit == _lastEdit && _sets.Count > 0)
				{
					return false;
				}
				List<CommunitySetSummary> fetched = new List<CommunitySetSummary>();
				int offset = 0;
				int total = -1;
				while (total < 0 || offset < total)
				{
					string pageUrl = $"{manifest.Absolute(manifest.SetsUrl)}?limit={200}&offset={offset}";
					CommunitySetsPage page = JsonConvert.DeserializeObject<CommunitySetsPage>(client.DownloadString(pageUrl));
					if (page == null)
					{
						break;
					}
					total = page.Total;
					fetched.AddRange(page.Sets);
					offset += 200;
					if (page.Sets.Count == 0)
					{
						break;
					}
				}
				try
				{
					List<CommunityCategoryEntry> categories = JsonConvert.DeserializeObject<List<CommunityCategoryEntry>>(client.DownloadString(manifest.Absolute(manifest.CategoriesUrl))) ?? new List<CommunityCategoryEntry>();
					_categories.Clear();
					_categories.AddRange(categories);
				}
				catch (Exception)
				{
					_categories.Clear();
				}
				_sets.Clear();
				_sets.AddRange(fetched);
				_lastEdit = remoteLastEdit;
				SaveIndex();
				this.CatalogUpdated?.Invoke(this, EventArgs.Empty);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public MarkerSet? FetchSetDetail(string setId)
		{
			string setId2 = setId;
			if (string.IsNullOrWhiteSpace(setId2))
			{
				return null;
			}
			try
			{
				using WebClient client = new WebClient();
				string url = _manifestService.Manifest.Resolve(_manifestService.Manifest.SetDetailUrl, setId2);
				string value = client.DownloadString(url);
				CommunitySetSummary summary = _sets.FirstOrDefault((CommunitySetSummary s) => s.Id == setId2);
				MarkerSet markerSet = JsonConvert.DeserializeObject<MarkerSet>(value);
				if (markerSet == null)
				{
					return null;
				}
				markerSet.id = Guid.NewGuid().ToString();
				markerSet.communitySetId = setId2;
				markerSet.author = summary?.Author;
				markerSet.communityUpdatedAt = summary?.UpdatedAt;
				markerSet.source = "community";
				markerSet.syncDetached = false;
				markerSet.localModifiedAt = null;
				markerSet.syncBaselineHash = SyncBaselineHash.Compute(markerSet);
				return markerSet;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void SaveIndex()
		{
			Directory.CreateDirectory(_moduleDirectory);
			JObject payload = new JObject
			{
				["lastEdit"] = (JToken)_lastEdit,
				["fetchedAt"] = (JToken)DateTime.UtcNow.ToString("o"),
				["sets"] = JArray.FromObject(_sets)
			};
			File.WriteAllText(Path.Combine(_moduleDirectory, "community_index.json"), payload.ToString(Formatting.Indented));
		}
	}
}
