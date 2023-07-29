using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Flurl;
using Flurl.Http;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.Utils;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2
{
	internal class KpV2Client
	{
		private readonly string _uri = "https://killproof.me/api/";

		public async Task<Profile> GetProfile(string id)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => _uri.AppendPathSegments("kp", id).SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().Code()).GetAsync(default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<Profile> GetProfileByCharacter(string characterName)
		{
			return (await HttpUtil.RetryAsync<Profile>(() => _uri.AppendPathSegments("character", characterName, "kp").SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().Code()).GetAsync(default(CancellationToken), (HttpCompletionOption)0))) ?? Profile.Empty;
		}

		public async Task<Resources> GetResources()
		{
			return (await HttpUtil.RetryAsync<Resources>(() => _uri.AppendPathSegment("resources").SetQueryParam("lang=" + GameService.Overlay.get_UserLocale().get_Value().Code()).GetAsync(default(CancellationToken), (HttpCompletionOption)0))) ?? Resources.Empty;
		}
	}
}
