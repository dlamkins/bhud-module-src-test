using System;
using System.Net;
using Manlaan.CommanderMarkers.Library.Models;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class CommanderMarkersManifestService
	{
		private CommanderMarkersManifest _manifest = new CommanderMarkersManifest();

		private bool _loaded;

		public static string ManifestUrl => "https://addons.soeed.com/commander_markers_v1.json";

		public CommanderMarkersManifest Manifest => _manifest;

		public bool IsLoaded => _loaded;

		public void LoadOrFetch()
		{
			try
			{
				using WebClient client = ModuleHttp.CreateClient();
				JObject i = JObject.Parse(client.DownloadString(ManifestUrl));
				_manifest.ServerUrl = i.Value<string>("server_url") ?? _manifest.ServerUrl;
				_manifest.CommunityCheckUrl = i.Value<string>("community_check_url") ?? _manifest.CommunityCheckUrl;
				_manifest.CommunityMarkersUrl = i.Value<string>("community_markers_url") ?? _manifest.CommunityMarkersUrl;
				_manifest.SetsUrl = i.Value<string>("sets_url") ?? _manifest.SetsUrl;
				_manifest.SetDetailUrl = i.Value<string>("set_detail_url") ?? _manifest.SetDetailUrl;
				_manifest.ThumbUrl = i.Value<string>("thumb_url") ?? _manifest.ThumbUrl;
				_manifest.CategoriesUrl = i.Value<string>("categories_url") ?? _manifest.CategoriesUrl;
				_manifest.SubmissionsUrl = i.Value<string>("submissions_url") ?? _manifest.SubmissionsUrl;
				_manifest.SubmissionsMineUrl = i.Value<string>("submissions_mine_url") ?? _manifest.SubmissionsMineUrl;
				_manifest.SubtokenUrl = i.Value<string>("subtoken_url") ?? _manifest.SubtokenUrl;
				_manifest.LibraryUrl = i.Value<string>("library_url") ?? _manifest.LibraryUrl;
			}
			catch (Exception)
			{
			}
			_loaded = true;
		}
	}
}
