using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	internal class AllianceHealthCheckDto
	{
		public Dictionary<string, List<string>> DuplicateRegulars { get; set; } = new Dictionary<string, List<string>>();


		public Dictionary<string, List<string>> MissingOfficers { get; set; } = new Dictionary<string, List<string>>();


		public List<string> Regular { get; set; } = new List<string>();


		public List<string> Officers { get; set; } = new List<string>();

	}
}
