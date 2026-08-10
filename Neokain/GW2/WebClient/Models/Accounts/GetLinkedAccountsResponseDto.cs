using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class GetLinkedAccountsResponseDto
	{
		public List<LinkedAccountSummaryDto> Accounts { get; set; } = new List<LinkedAccountSummaryDto>();

	}
}
