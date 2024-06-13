using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services
{
	internal class KpWebApiService : IDisposable
	{
		private readonly KpV1Client _v1Client;

		private readonly KpV2Client _v2Client;

		public readonly IReadOnlyList<TokenPermission> RequiredPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)5,
			(TokenPermission)3,
			(TokenPermission)10,
			(TokenPermission)9,
			(TokenPermission)6
		};

		public static event EventHandler<ValueEventArgs<bool>> SubtokenUpdated;

		public KpWebApiService()
		{
			_v1Client = new KpV1Client();
			_v2Client = new KpV2Client();
			ProofLogix.Instance.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
		}

		public void Dispose()
		{
			ProofLogix.Instance.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
		}

		private void OnSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			bool valid = e.get_Value().Intersect(RequiredPermissions).Count() == RequiredPermissions.Count;
			KpWebApiService.SubtokenUpdated?.Invoke(this, new ValueEventArgs<bool>(valid));
		}

		public async Task<AddKey> AddKey(string key, bool opener)
		{
			return await _v1Client.AddKey(key, opener);
		}

		public async Task<Opener> GetOpener(string encounterId, Opener.ServerRegion region)
		{
			return await _v1Client.GetOpener(encounterId, region);
		}

		public async Task<Resources> GetResources()
		{
			return await _v2Client.GetResources();
		}

		public async Task<bool> Refresh(string id)
		{
			return await _v1Client.Refresh(id);
		}

		public async Task<bool> IsProofBusy(string id)
		{
			return await _v1Client.CheckProofBusy(id);
		}

		public async Task<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile> GetProfile(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile.Empty;
			}
			return await ExpandProfile(await _v2Client.GetProfile(id));
		}

		public async Task<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile> GetProfileByCharacter(string characterName)
		{
			if (string.IsNullOrEmpty(characterName))
			{
				return Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile.Empty;
			}
			return await ExpandProfile(await _v2Client.GetProfileByCharacter(characterName));
		}

		private async Task<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile> ExpandProfile(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
		{
			if (profile.NotFound)
			{
				return profile;
			}
			HandleOriginalUce(profile);
			AddLdToLi(profile);
			Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile2 = profile;
			profile2.Clears = await _v1Client.GetClears(profile.Id);
			if (profile.Linked == null)
			{
				return profile;
			}
			foreach (Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile link in profile.Linked)
			{
				HandleOriginalUce(link);
				AddLdToLi(link);
				profile2 = link;
				profile2.Clears = await _v1Client.GetClears(link.Id);
			}
			return profile;
		}

		private void HandleOriginalUce(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
		{
			if (!profile.IsEmpty && profile.OriginalUce != null && !profile.OriginalUce.IsEmpty)
			{
				Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token uce = profile.GetToken(81743);
				if (uce == null || uce.IsEmpty)
				{
					profile.Killproofs.Add(profile.OriginalUce);
					uce = profile.OriginalUce;
				}
				uce.Amount = Math.Max(uce.Amount, profile.OriginalUce.Amount);
			}
		}

		private void AddUceToUfe(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
		{
			if (profile.IsEmpty)
			{
				return;
			}
			Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token uce = profile.GetToken(81743);
			if (!uce.IsEmpty && uce.Amount > 0)
			{
				Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token ufe = profile.GetToken(94020);
				if (ufe == null || ufe.IsEmpty)
				{
					string ufeRes = ProofLogix.Instance.Resources.GetItem(94020).Name;
					ufe = new Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token
					{
						Id = 94020,
						Name = (string.IsNullOrEmpty(ufeRes) ? "Unstable Fractal Essence" : ufeRes)
					};
					profile.Killproofs.Add(ufe);
				}
				ufe.Amount += 5 * uce.Amount;
			}
		}

		private void AddLdToLi(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
		{
			if (profile.IsEmpty)
			{
				return;
			}
			Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token ld = profile.GetToken(88485);
			if (!ld.IsEmpty && ld.Amount > 0)
			{
				Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token li = profile.GetToken(77302);
				if (li == null || li.IsEmpty)
				{
					string liRes = ProofLogix.Instance.Resources.GetItem(77302).Name;
					li = new Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token
					{
						Id = 77302,
						Name = (string.IsNullOrEmpty(liRes) ? "Legendary Insight" : liRes)
					};
					profile.Killproofs.Add(li);
				}
				li.Amount += ld.Amount;
			}
		}
	}
}
