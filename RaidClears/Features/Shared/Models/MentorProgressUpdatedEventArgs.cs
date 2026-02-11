using System;
using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models
{
	public sealed class MentorProgressUpdatedEventArgs : EventArgs
	{
		public IReadOnlyList<MentorProgressChange> Changes { get; set; } = Array.Empty<MentorProgressChange>();

	}
}
