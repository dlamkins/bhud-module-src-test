using System;
using Neokain.GW2.WebClient.Models.Alliances.Tags;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceTagTypeChangedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public AllianceTagTypeDto TagType { get; }

		public AllianceTagTypeChangedEventArgs(Guid allianceId, AllianceTagTypeDto tagType)
		{
			AllianceId = allianceId;
			TagType = tagType;
		}
	}
}
