using System;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceTagTypeRemovedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public Guid TagTypeId { get; }

		public AllianceTagTypeRemovedEventArgs(Guid allianceId, Guid tagTypeId)
		{
			AllianceId = allianceId;
			TagTypeId = tagTypeId;
		}
	}
}
