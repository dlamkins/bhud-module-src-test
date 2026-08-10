using System;
using Neokain.GW2.WebClient.Models.Enums;

namespace Neokain.GW2.WebClient.Models.Spams.SpamLines
{
	public class SpamLineDto
	{
		public Guid Id { get; set; }

		public Guid SpamId { get; set; }

		public int Order { get; set; }

		public ChatType Target { get; set; }

		public string TargetInfo1 { get; set; }

		public string LineText { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ModifiedAt { get; set; }

		public Guid? CreatedById { get; set; }

		public Guid? ModifiedById { get; set; }
	}
}
