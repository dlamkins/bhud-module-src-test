using Blish_HUD;

namespace Nekres.ProofLogix.Core.Services.PartySync.Models
{
	public sealed class MumblePlayer : Player
	{
		public override string CharacterName => GameService.Gw2Mumble.get_PlayerCharacter().get_Name();

		public override OnlineStatus Status => GetStatus();

		protected override int GetSpecialization()
		{
			return GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization();
		}

		protected override int GetProfession()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected I4, but got Unknown
			return (int)GameService.Gw2Mumble.get_PlayerCharacter().get_Profession();
		}

		public OnlineStatus GetStatus()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return OnlineStatus.Online;
			}
			if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				return OnlineStatus.Unknown;
			}
			return OnlineStatus.Away;
		}
	}
}
