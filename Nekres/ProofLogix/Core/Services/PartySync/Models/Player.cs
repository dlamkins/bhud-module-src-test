using System;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services.PartySync.Models
{
	public class Player
	{
		private Player _arcDpsPlayer;

		public Profile KpProfile { get; private set; }

		public DateTime Created { get; private set; }

		public string AccountName
		{
			get
			{
				if (!HasAgent)
				{
					if (!HasKpProfile)
					{
						return string.Empty;
					}
					return KpProfile.Name;
				}
				return ((Player)(ref _arcDpsPlayer)).get_AccountName();
			}
		}

		public virtual string CharacterName => ((Player)(ref _arcDpsPlayer)).get_CharacterName();

		public bool HasAgent => !string.IsNullOrEmpty(((Player)(ref _arcDpsPlayer)).get_AccountName());

		public bool HasKpProfile
		{
			get
			{
				Profile kpProfile = KpProfile;
				if (kpProfile != null)
				{
					return !kpProfile.NotFound;
				}
				return false;
			}
		}

		public string Class => GetClass();

		public AsyncTexture2D Icon => GetIcon();

		public Player()
		{
			Created = DateTime.UtcNow;
			KpProfile = Profile.Empty;
		}

		public Player(Player agent)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_arcDpsPlayer = agent;
		}

		public Player(Profile profile)
			: this()
		{
			KpProfile = profile;
		}

		public bool Equals(Player other)
		{
			if (!AccountName.Equals(other.AccountName, StringComparison.InvariantCultureIgnoreCase))
			{
				Profile linkedProfile;
				if (HasKpProfile)
				{
					return KpProfile.BelongsTo(other.AccountName, out linkedProfile);
				}
				return false;
			}
			return true;
		}

		public void AttachAgent(Player arcDpsPlayer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_arcDpsPlayer = arcDpsPlayer;
		}

		public void AttachProfile(Profile kpProfile)
		{
			KpProfile = kpProfile;
		}

		protected virtual int GetSpecialization()
		{
			return (int)((Player)(ref _arcDpsPlayer)).get_Elite();
		}

		protected virtual int GetProfession()
		{
			return (int)((Player)(ref _arcDpsPlayer)).get_Profession();
		}

		private string GetClass()
		{
			return ProofLogix.Instance.Resources.GetClassName(GetProfession(), GetSpecialization());
		}

		private AsyncTexture2D GetIcon()
		{
			return ProofLogix.Instance.Resources.GetClassIcon(GetProfession(), GetSpecialization());
		}
	}
}
