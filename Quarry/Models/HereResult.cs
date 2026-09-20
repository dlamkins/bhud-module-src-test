using System;
using System.Collections.Generic;

namespace Quarry.Models
{
	public class HereResult
	{
		public HereResultReason Reason { get; set; }

		public IReadOnlyList<HereCandidate> Candidates { get; set; } = Array.Empty<HereCandidate>();


		public IReadOnlyList<HereCandidate> RankedUncapped { get; set; } = Array.Empty<HereCandidate>();


		public bool Partial { get; set; }

		public bool CategorySupported { get; set; } = true;


		public int FilteredByGuidance { get; set; }

		public int HiddenCount { get; set; }

		public IReadOnlyList<HereCandidate> Opportunistic { get; set; } = Array.Empty<HereCandidate>();


		public bool IndexReady { get; set; }
	}
}
