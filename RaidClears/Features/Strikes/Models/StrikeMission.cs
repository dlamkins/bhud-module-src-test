using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Models
{
	public class StrikeMission : EncounterInterface
	{
		[JsonProperty("mapIds")]
		public List<int> MapIds = new List<int>();

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
