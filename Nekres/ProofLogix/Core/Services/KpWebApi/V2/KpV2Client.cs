using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Flurl;
using Flurl.Http;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2
{
	internal class KpV2Client
	{
		private readonly string _uri = "https://killproof.me/api/";

		public async Task<Profile> GetProfile(string id)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => GeneratedExtensions.GetAsync(StringExtensions.AppendPathSegments(_uri, new object[2] { "kp", id }).SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().TwoLetterISOLanguageName()), default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<Profile> GetProfileByCharacter(string characterName)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => GeneratedExtensions.GetAsync(StringExtensions.AppendPathSegments(_uri, new object[3] { "character", characterName, "kp" }).SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().TwoLetterISOLanguageName()), default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<Resources> GetResources()
		{
			return (await HttpUtil.RetryAsync<Resources>(() => GeneratedExtensions.GetAsync(StringExtensions.AppendPathSegment(_uri, (object)"resources", false).SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().TwoLetterISOLanguageName()), default(CancellationToken), (HttpCompletionOption)0))) ?? Resources.Empty;
		}
	}
}
