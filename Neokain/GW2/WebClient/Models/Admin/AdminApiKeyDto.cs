using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminApiKeyDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string MaskedKey { get; set; }

		public List<string> Permissions { get; set; } = new List<string>();


		public bool IsActive { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? LastUsedAt { get; set; }
	}
}
