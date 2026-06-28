using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class UserManager : IDisposable
	{
		protected bool AccountQueryInProgress;

		public List<Guild> Guilds { get; protected set; } = new List<Guild>();


		public Account? Account { get; set; }

		public TutorialProgress TutorialState { get; private set; } = new TutorialProgress();


		public event EventHandler<Account>? AccountUpdated;

		public event EventHandler<TutorialProgress>? TutorialStateUpdated;

		public event EventHandler<IEnumerable<Guild>>? GuildsUpdated;

		public UserManager()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			SubtokenUpdated();
		}

		public Guild GetGuildByGuid(string guildGuid)
		{
			string guildGuid2 = guildGuid;
			return Guilds.Find((Guild g) => g.Id.ToString() == guildGuid2);
		}

		public Guild GetGuildByName(string guildName)
		{
			string guildName2 = guildName;
			return Guilds.Find((Guild g) => g.Name.ToString() == guildName2);
		}

		public void SetTutorialState(TutorialProgress state)
		{
			TutorialState = state;
			this.TutorialStateUpdated?.Invoke(this, state);
		}

		public async Task RefreshGuildsAsync()
		{
			if (await GW2ApiService.GenerateSubtoken() != null)
			{
				Guilds = await GW2ApiService.QueryGuilds();
				this.GuildsUpdated?.Invoke(this, Guilds);
			}
		}

		public void SubtokenUpdated()
		{
			if (!Service.Gw2ApiManager.get_HasSubtoken() || AccountQueryInProgress)
			{
				Logger.GetLogger<Module>().Debug($"SubtokenUpdated called but conditions not met: HasSubtoken={Service.Gw2ApiManager.get_HasSubtoken()}, InProgress={AccountQueryInProgress}");
				return;
			}
			AccountQueryInProgress = true;
			Task.Run(async delegate
			{
				_ = 3;
				try
				{
					Logger.GetLogger<Module>().Info("Starting account and guild update process");
					Account account = await GW2ApiService.QueryAccount();
					if (account != null)
					{
						Account = account;
						this.AccountUpdated?.Invoke(this, Account);
						Logger.GetLogger<Module>().Info("Account updated: " + account.get_Name());
						if (await GW2ApiService.GenerateSubtoken() != null)
						{
							Logger.GetLogger<Module>().Info("Subtoken generated successfully, loading guilds");
							Guilds = await GW2ApiService.QueryGuilds();
							Logger.GetLogger<Module>().Info($"Loaded {Guilds.Count} guilds");
							this.GuildsUpdated?.Invoke(this, Guilds);
							UserCheckServerResponse check = await Service.GeoServerWrapper.PerformUserCheck(account.get_Name());
							if (check?.Tutorial != null)
							{
								SetTutorialState(check.Tutorial);
							}
						}
						else
						{
							Logger.GetLogger<Module>().Warn("Failed to generate subtoken, skipping guild loading");
						}
					}
					else
					{
						Logger.GetLogger<Module>().Warn("Failed to query account");
					}
				}
				catch (Exception ex)
				{
					Logger.GetLogger<Module>().Warn(ex, "Error in SubtokenUpdated");
				}
				finally
				{
					AccountQueryInProgress = false;
				}
			});
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}
	}
}
