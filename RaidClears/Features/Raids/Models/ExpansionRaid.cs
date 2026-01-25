using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Models
{
	[Serializable]
	public class ExpansionRaid : EncounterInterface
	{
		[JsonProperty("asset")]
		public string asset = "missing.png";

		[JsonProperty("wings")]
		public List<RaidWing> Wings = new List<RaidWing>();

		[JsonProperty("name")]
		private string _name = "undefined";

		[JsonProperty("abbriviation")]
		private string _abbriviation = "undefined";

		public new string Name
		{
			get
			{
				return GetLocalizedName(_name);
			}
			set
			{
				_name = value;
			}
		}

		public new string Abbriviation
		{
			get
			{
				return GetLocalizedAbbreviation(_abbriviation);
			}
			set
			{
				_abbriviation = value;
			}
		}
	}
}
