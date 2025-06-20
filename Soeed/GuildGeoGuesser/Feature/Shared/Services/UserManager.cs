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

		public event EventHandler<Account>? AccountUpdated;

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

		public void SubtokenUpdated()
		{
			if (Service.Gw2ApiManager.get_HasSubtoken() && !AccountQueryInProgress)
			{
				Task.Run(async delegate
				{
					_ = 2;
					try
					{
						AccountQueryInProgress = true;
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
			else
			{
				Logger.GetLogger<Module>().Debug($"SubtokenUpdated called but conditions not met: HasSubtoken={Service.Gw2ApiManager.get_HasSubtoken()}, InProgress={AccountQueryInProgress}");
			}
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}
	}
}
