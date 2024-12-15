using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Raids.Services
{
	[Serializable]
	public abstract class Labelable
	{
		protected bool _isRaid;

		protected bool _isStrike;

		protected bool _isFractal;

		[JsonProperty("encounterLabels")]
		public Dictionary<string, string> EncounterLabels { get; set; } = new Dictionary<string, string>();


		public abstract void SetEncounterLabel(string encounterApiId, string label);

		public string GetEncounterLabel(string encounterApiId)
		{
			if (EncounterLabels.TryGetValue(encounterApiId, out var value))
			{
				return value;
			}
			if (_isRaid)
			{
				return Service.RaidData.GetRaidEncounterByApiId(encounterApiId).Abbriviation;
			}
			if (_isStrike)
			{
				return Service.StrikeData.GetStrikeMissionById(encounterApiId).Abbriviation;
			}
			if (_isFractal)
			{
				return Service.FractalMapData.GetFractalByApiName(encounterApiId).ShortLabel;
			}
			return "undefined";
		}

		public string GetEncounterLabel(RaidEncounter enc)
		{
			if (EncounterLabels.TryGetValue(enc.ApiId, out var value))
			{
				return value;
			}
			return enc.Abbriviation;
		}

		public string GetEncounterLabel(EncounterInterface enc)
		{
			if (EncounterLabels.TryGetValue(enc.Id, out var value))
			{
				return value;
			}
			return enc.Abbriviation;
		}

		public abstract void Save();
	}
}
