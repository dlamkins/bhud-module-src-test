using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;

namespace KillProofModule.Models
{
	public class PlayerProfile
	{
		public readonly bool IsSelf;

		private Player _player;

		private KillProof _killProof;

		public string AccountName
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				object obj = KillProof?.AccountName;
				if (obj == null)
				{
					Player player = Player;
					obj = ((Player)(ref player)).get_AccountName() ?? "";
				}
				return (string)obj;
			}
		}

		public string CharacterName
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Player player = Player;
				string text = ((Player)(ref player)).get_CharacterName();
				if (text == null)
				{
					if (!IsSelf)
					{
						return Nickname();
					}
					text = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
				}
				return text;
			}
		}

		public string KpId => KillProof?.KpId ?? "";

		public Player Player
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _player;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				if (!((object)(Player)(ref _player)).Equals((object)value))
				{
					_player = value;
					this.PlayerChanged?.Invoke(this, new ValueEventArgs<Player>(value));
				}
			}
		}

		public KillProof KillProof
		{
			get
			{
				return _killProof;
			}
			set
			{
				if (_killProof == null || !_killProof.Equals(value))
				{
					_killProof = value;
					this.KillProofChanged?.Invoke(this, new ValueEventArgs<KillProof>(value));
				}
			}
		}

		public event EventHandler<ValueEventArgs<Player>> PlayerChanged;

		public event EventHandler<ValueEventArgs<KillProof>> KillProofChanged;

		public PlayerProfile(bool isSelf = false)
		{
			IsSelf = isSelf;
			if (isSelf)
			{
				GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			}
		}

		private void OnUserLocaleChanged(object o, ValueEventArgs<CultureInfo> e)
		{
			KillProofModule.ModuleInstance.PartyManager.RequestSelf();
		}

		public bool IsOwner(string accountNameOrKpId)
		{
			if (string.IsNullOrEmpty(accountNameOrKpId))
			{
				return false;
			}
			if (string.IsNullOrEmpty(AccountName) || !AccountName.Equals(accountNameOrKpId, StringComparison.InvariantCultureIgnoreCase))
			{
				if (!string.IsNullOrEmpty(KpId))
				{
					return KpId.Equals(accountNameOrKpId, StringComparison.InvariantCulture);
				}
				return false;
			}
			return true;
		}

		public bool Equals(PlayerProfile other)
		{
			if (!IsOwner(other.AccountName))
			{
				return IsOwner(other.KpId);
			}
			return true;
		}

		public bool HasKillProof()
		{
			if (KillProof != null)
			{
				return string.IsNullOrEmpty(KillProof.Error);
			}
			return false;
		}

		public string Nickname()
		{
			int index = AccountName.IndexOf('.');
			return AccountName.Substring(0, (index >= 0) ? index : 0);
		}
	}
}
