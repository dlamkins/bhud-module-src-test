using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Library.Services;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class CommunityShareHelper
	{
		public static List<string> CategoryNames()
		{
			List<string> names = new List<string>();
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (CommunityCategoryEntry category in Service.CommunityCatalog.Categories)
			{
				if (!string.IsNullOrWhiteSpace(category.Name) && seen.Add(category.Name))
				{
					names.Add(category.Name);
				}
			}
			if (names.Count == 0)
			{
				foreach (CommunitySetSummary summary in Service.CommunityCatalog.Sets)
				{
					if (!string.IsNullOrWhiteSpace(summary.CategoryName) && seen.Add(summary.CategoryName))
					{
						names.Add(summary.CategoryName);
					}
				}
				return names;
			}
			return names;
		}

		public static string ResolveCategory(int categoryIndex, string customCategory, IReadOnlyList<string> categoryNames)
		{
			int customIndex = categoryNames.Count;
			if (categoryNames.Count == 0 || categoryIndex == customIndex)
			{
				return customCategory.Trim();
			}
			if (categoryIndex >= 0 && categoryIndex < categoryNames.Count)
			{
				return categoryNames[categoryIndex];
			}
			return customCategory.Trim();
		}

		public static async Task<CommunityShareResult> SubmitAsync(MarkerSet markerSet, string category)
		{
			if (string.IsNullOrWhiteSpace(category))
			{
				return new CommunityShareResult
				{
					Success = false,
					Message = "Enter a category name."
				};
			}
			HttpWebResponse response = default(HttpWebResponse);
			try
			{
				string subtoken = await Service.SubtokenService.GetValidSubtokenAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrEmpty(subtoken))
				{
					return new CommunityShareResult
					{
						Success = false,
						Message = "Account API permission required."
					};
				}
				JObject payload = MarkerSetSubmission.ToSubmissionPayload(markerSet, category);
				using WebClient client = ModuleHttp.CreateClient();
				client.Headers[HttpRequestHeader.Authorization] = "Bearer " + subtoken;
				client.Headers[HttpRequestHeader.ContentType] = "application/json";
				string url = Service.ManifestService.Manifest.Absolute(Service.ManifestService.Manifest.SubmissionsUrl);
				client.UploadString(url, payload.ToString(Formatting.None));
				return new CommunityShareResult
				{
					Success = true,
					Message = "Sent for moderator review."
				};
			}
			catch (WebException ex) when (((Func<bool>)delegate
			{
				// Could not convert BlockContainer to single expression
				response = ex.Response as HttpWebResponse;
				return response != null;
			}).Invoke())
			{
				return new CommunityShareResult
				{
					Success = false,
					Message = $"Share failed ({(int)response.StatusCode})."
				};
			}
			catch (Exception)
			{
				return new CommunityShareResult
				{
					Success = false,
					Message = "Share failed."
				};
			}
		}
	}
}
