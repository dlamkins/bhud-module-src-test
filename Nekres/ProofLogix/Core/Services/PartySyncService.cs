using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Microsoft.Xna.Framework;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.PartySync.Models;

namespace Nekres.ProofLogix.Core.Services
{
	public class PartySyncService : IDisposable
	{
		public enum ColorGradingMode
		{
			LocalPlayerComparison,
			MedianComparison,
			LargestComparison,
			AverageComparison
		}

		public readonly MumblePlayer LocalPlayer;

		private readonly ConcurrentDictionary<string, Player> _members;

		private readonly Color _redShift = new Color(255, 57, 57);

		private readonly Color _unknownColor = new Color(127, 128, 127);

		private readonly Color _awayColor = new Color(255, 165, 0);

		private readonly Color _onlineColor = new Color(0, 255, 0);

		public IReadOnlyList<Player> PlayerList => _members.Values.ToList();

		public event EventHandler<ValueEventArgs<Player>> PlayerAdded;

		public event EventHandler<ValueEventArgs<Player>> PlayerRemoved;

		public event EventHandler<ValueEventArgs<Player>> PlayerChanged;

		public PartySyncService()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			LocalPlayer = new MumblePlayer();
			_members = new ConcurrentDictionary<string, Player>();
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnPlayerCharacterNameChanged);
			GameService.ArcDps.get_Common().add_PlayerAdded(new PresentPlayersChange(OnPlayerJoin));
			GameService.ArcDps.get_Common().add_PlayerRemoved(new PresentPlayersChange(OnPlayerLeft));
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}

		public Color GetTokenAmountColor(int id, int amount, ColorGradingMode gradingMode)
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			float maxAmount = amount;
			switch (gradingMode)
			{
			case ColorGradingMode.LocalPlayerComparison:
				maxAmount = LocalPlayer.KpProfile.GetToken(id).Amount;
				break;
			case ColorGradingMode.MedianComparison:
				maxAmount = ((_members.Count > 0) ? ((float)_members.Values.Median((Player member) => member.KpProfile.GetToken(id).Amount)) : ((float)amount));
				break;
			case ColorGradingMode.LargestComparison:
				maxAmount = GetLargestAmount(id);
				break;
			case ColorGradingMode.AverageComparison:
				maxAmount = ((_members.Count > 0) ? ((float)_members.Values.Average((Player member) => member.KpProfile.GetToken(id).Amount)) : ((float)amount));
				break;
			}
			float diff = maxAmount - (float)amount;
			if (!(diff <= 0f))
			{
				return Color.Lerp(Color.get_White(), _redShift, diff / maxAmount);
			}
			return Color.get_White();
		}

		private int GetLargestAmount(int id)
		{
			if (_members.Count <= 0)
			{
				return LocalPlayer.KpProfile.GetToken(id).Amount;
			}
			return _members.Values.Max((Player x) => x.KpProfile.GetToken(id).Amount);
		}

		public Color GetStatusColor(Player.OnlineStatus status)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(status switch
			{
				Player.OnlineStatus.Unknown => _unknownColor, 
				Player.OnlineStatus.Away => _awayColor, 
				Player.OnlineStatus.Online => _onlineColor, 
				_ => throw new ArgumentOutOfRangeException(), 
			});
		}

		public async Task InitSquad()
		{
			await GetLocalPlayerProfile();
			foreach (Player player in GameService.ArcDps.get_Common().get_PlayersInSquad().Values)
			{
				AddArcDpsAgent(player);
			}
			foreach (string id in (Collection<string>)(object)ProofLogix.Instance.TableConfig.get_Value().ProfileIds)
			{
				AddKpProfile(await ProofLogix.Instance.KpWebApi.GetProfile(id));
			}
		}

		public void RemovePlayer(string accountName)
		{
			string key = accountName.ToLowerInvariant();
			if (!string.IsNullOrEmpty(key) && _members.TryRemove(key, out var member))
			{
				member.Status = Player.OnlineStatus.Away;
				this.PlayerRemoved?.Invoke(this, new ValueEventArgs<Player>(member));
			}
		}

		public void Dispose()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnPlayerCharacterNameChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			GameService.ArcDps.get_Common().remove_PlayerAdded(new PresentPlayersChange(OnPlayerJoin));
			GameService.ArcDps.get_Common().remove_PlayerRemoved(new PresentPlayersChange(OnPlayerLeft));
		}

		public void AddKpProfile(Profile kpProfile)
		{
			string key = kpProfile.Name;
			if (string.IsNullOrWhiteSpace(key))
			{
				return;
			}
			if (HasAccountInParty(key, out var existingAccount))
			{
				key = existingAccount;
			}
			if ((LocalPlayer.HasKpProfile && LocalPlayer.KpProfile.BelongsTo(key, out var _)) || LocalPlayer.AccountName.ToLowerInvariant().Equals(key.ToLowerInvariant()))
			{
				LocalPlayer.AttachProfile(kpProfile);
				this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>((Player)LocalPlayer));
				return;
			}
			_members.AddOrUpdate(key.ToLowerInvariant(), delegate
			{
				Player player = new Player(kpProfile);
				this.PlayerAdded?.Invoke(this, new ValueEventArgs<Player>(player));
				return player;
			}, delegate(string _, Player member)
			{
				member.AttachProfile(kpProfile);
				this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>(member));
				return member;
			});
		}

		private async void OnPlayerCharacterNameChanged(object sender, ValueEventArgs<string> e)
		{
			await GetLocalPlayerProfile();
		}

		private async void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			foreach (Player member in _members.Values)
			{
				Player player = member;
				player.AttachProfile(await ProofLogix.Instance.KpWebApi.GetProfile(member.AccountName));
				this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>(member));
			}
		}

		private async Task GetLocalPlayerProfile()
		{
			Profile profile = await ProofLogix.Instance.KpWebApi.GetProfileByCharacter(GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
			LocalPlayer.AttachProfile(profile);
		}

		private void AddArcDpsAgent(Player arcDpsPlayer)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			string key = ((Player)(ref arcDpsPlayer)).get_AccountName();
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			if (HasAccountInParty(key, out var existingAccount))
			{
				key = existingAccount;
			}
			if (((Player)(ref arcDpsPlayer)).get_Self())
			{
				LocalPlayer.AttachAgent(arcDpsPlayer);
				this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>((Player)LocalPlayer));
				return;
			}
			_members.AddOrUpdate(key.ToLowerInvariant(), delegate
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				Player player = new Player(arcDpsPlayer);
				this.PlayerAdded?.Invoke(this, new ValueEventArgs<Player>(player));
				return player;
			}, delegate(string _, Player member)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				member.AttachAgent(arcDpsPlayer);
				this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>(member));
				return member;
			}).Status = Player.OnlineStatus.Online;
		}

		private bool HasAccountInParty(string account, out string existingAccount)
		{
			Profile linkedProfile;
			Player existingMember = _members.Values.FirstOrDefault((Player member) => member.HasKpProfile && member.KpProfile.BelongsTo(account, out linkedProfile));
			if (existingMember != null)
			{
				existingAccount = existingMember.AccountName;
				return true;
			}
			existingAccount = string.Empty;
			return false;
		}

		private async void OnPlayerJoin(Player player)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			AddArcDpsAgent(player);
			AddKpProfile(await ProofLogix.Instance.KpWebApi.GetProfile(((Player)(ref player)).get_AccountName()));
		}

		private void OnPlayerLeft(Player player)
		{
			if (!((Player)(ref player)).get_Self())
			{
				RemovePlayer(((Player)(ref player)).get_AccountName());
			}
		}
	}
}
