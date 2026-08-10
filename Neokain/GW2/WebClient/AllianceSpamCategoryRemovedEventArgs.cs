using System;

namespace Neokain.GW2.WebClient
{
	public sealed class AllianceSpamCategoryRemovedEventArgs : EventArgs
	{
		public Guid AllianceId { get; }

		public Guid CategoryId { get; }

		public AllianceSpamCategoryRemovedEventArgs(Guid allianceId, Guid categoryId)
		{
			AllianceId = allianceId;
			CategoryId = categoryId;
		}
	}
}
