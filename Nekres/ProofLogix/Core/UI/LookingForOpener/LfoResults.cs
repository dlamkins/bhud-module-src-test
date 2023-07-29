using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;

namespace Nekres.ProofLogix.Core.UI.LookingForOpener
{
	public class LfoResults
	{
		public string EncounterId { get; init; }

		public Opener Opener { get; init; }

		public LfoResults(string encounterId, Opener openerTask)
		{
			EncounterId = encounterId;
			Opener = openerTask;
		}
	}
}
