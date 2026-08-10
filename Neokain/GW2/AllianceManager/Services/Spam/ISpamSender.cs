using System.Threading.Tasks;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Services.Spam
{
	public interface ISpamSender
	{
		Task<bool> SendAsync(SpamDetailDto spam, SpamContext context, int messageDelay, IGw2WebClient webClient);
	}
}
