using System;
using Neokain.GW2.WebClient.Models.Alliances.Tags;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceMemberTagChangedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public AllianceMemberTagDto MemberTag { get; }

		public AllianceMemberTagChangedEventArgs(Guid allianceId, AllianceMemberTagDto memberTag)
		{
			AllianceId = allianceId;
			MemberTag = memberTag;
		}
	}
}
