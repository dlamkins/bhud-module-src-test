using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Alliances.Tags
{
	public class AllianceMemberTagDto
	{
		public Guid Id { get; set; }

		public Guid AllianceId { get; set; }

		public string MemberName { get; set; } = string.Empty;


		public Guid TagTypeId { get; set; }

		public string TagTypeName { get; set; } = string.Empty;


		public string? TagTypeColor { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset? EndDate { get; set; }

		public string? Note { get; set; }

		public decimal? GoldAmount { get; set; }

		public bool IsActive { get; set; }

		public bool IsExpired
		{
			get
			{
				if (EndDate.HasValue)
				{
					return EndDate.Value < DateTimeOffset.UtcNow;
				}
				return false;
			}
		}

		public int? DaysRemaining
		{
			get
			{
				if (!EndDate.HasValue)
				{
					return null;
				}
				return (int)Math.Max(0.0, Math.Ceiling((EndDate.Value - DateTimeOffset.UtcNow).TotalDays));
			}
		}

		public List<AllianceMemberTagExtensionDto> Extensions { get; set; } = new List<AllianceMemberTagExtensionDto>();

	}
}
