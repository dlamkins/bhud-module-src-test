using System;

namespace Neokain.GW2.WebClient.Models.System
{
	internal class SystemVersionInfoDto
	{
		public string Version { get; set; }

		public string CommitHash { get; set; }

		public string StackName { get; set; }

		public DateTimeOffset BuildTimestamp { get; set; }
	}
}
