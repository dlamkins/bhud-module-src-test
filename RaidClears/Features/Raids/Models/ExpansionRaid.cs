using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	[Serializable]
	public class ExpansionRaid : EncounterInterface, IExpansion<RaidWing>
	{
		[JsonProperty("asset")]
		public string asset = "missing.png";

		[JsonProperty("wings")]
		public List<RaidWing> Wings = new List<RaidWing>();

		[JsonIgnore]
		public string Asset => asset;

		[JsonIgnore]
		public IReadOnlyList<RaidWing> Children => Wings;

		string IExpansion<RaidWing>.Id => Id;
	}
}
