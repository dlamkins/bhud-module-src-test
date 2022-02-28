using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using KillProofModule.Controls;
using KillProofModule.Models;
using KillProofModule.Properties;

namespace KillProofModule.Manager
{
	public class PartyManager
	{
		private Logger Logger = Logger.GetLogger<PartyManager>();

		public List<PlayerProfile> Players;

		public PlayerProfile Self;

		public event EventHandler<ValueEventArgs<PlayerProfile>> PlayerAdded;

		public event EventHandler<ValueEventArgs<PlayerProfile>> PlayerLeft;

		public PartyManager()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			Self = new PlayerProfile(isSelf: true);
			Players = new List<PlayerProfile>();
			RequestSelf();
			GameService.ArcDps.get_Common().Activate();
			GameService.ArcDps.get_Common().add_PlayerAdded(new PresentPlayersChange(PlayerAddedEvent));
			GameService.ArcDps.get_Common().add_PlayerRemoved(new PresentPlayersChange(PlayerLeavesEvent));
		}

		public async void RequestSelf()
		{
			if (!string.IsNullOrEmpty(Self.AccountName))
			{
				PlayerProfile self = Self;
				self.KillProof = await KillProofApi.GetKillProofContent(Self.AccountName);
			}
			else
			{
				if (!KillProofModule.ModuleInstance.Gw2ApiManager.HavePermission((TokenPermission)1))
				{
					return;
				}
				await ((IBlobClient<Account>)(object)KillProofModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> result)
				{
					if (result.IsCompleted && !result.IsFaulted)
					{
						PlayerProfile self2 = Self;
						self2.KillProof = await KillProofApi.GetKillProofContent(result.Result.get_Name());
					}
				});
			}
		}

		private async void PlayerAddedEvent(Player player)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (((Player)(ref player)).get_Self())
			{
				Self.Player = player;
				RequestSelf();
				return;
			}
			PlayerProfile profile = Players.FirstOrDefault((PlayerProfile p) => p.IsOwner(((Player)(ref player)).get_AccountName()));
			if (profile == null)
			{
				profile = new PlayerProfile
				{
					Player = player
				};
				Players.Add(profile);
				this.PlayerAdded?.Invoke(this, new ValueEventArgs<PlayerProfile>(profile));
				await KillProofApi.ProfileAvailable(((Player)(ref player)).get_AccountName()).ContinueWith(delegate(Task<bool> response)
				{
					if (response.IsCompleted && !response.IsFaulted && response.Result)
					{
						PlayerNotification.ShowNotification(profile, global::KillProofModule.Properties.Resources.profile_available, 10f);
					}
				});
			}
			else
			{
				profile.Player = player;
			}
		}

		private void PlayerLeavesEvent(Player player)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (!((Player)(ref player)).get_Self() && KillProofModule.ModuleInstance.AutomaticClearEnabled.get_Value())
			{
				PlayerProfile profile = Players.FirstOrDefault((PlayerProfile p) => p.IsOwner(((Player)(ref player)).get_AccountName()));
				Players.Remove(profile);
				this.PlayerLeft?.Invoke(this, new ValueEventArgs<PlayerProfile>(profile));
			}
		}
	}
}
