using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RaidClears.Features.Shared;
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
			string storageKey = StorageKeyPrefixes.NormalizeStorageKey(encounterApiId);
			if (storageKey != encounterApiId && EncounterLabels.TryGetValue(storageKey, out value))
			{
				return value;
			}
			string lookupId = storageKey;
			if (_isRaid)
			{
				return Service.RaidData.GetRaidEncounterByApiId(lookupId).Abbriviation;
			}
			if (_isStrike)
			{
				return Service.StrikeData.GetBossEncounterById(lookupId).Abbriviation;
			}
			if (_isFractal)
			{
				return Service.FractalMapData.GetFractalByApiName(encounterApiId).ShortLabel;
			}
			return "undefined";
		}

		public string GetEncounterLabel(BossEncounter enc)
		{
			if (EncounterLabels.TryGetValue(enc.EncounterId, out var value))
			{
				return value;
			}
			string storageKey = StorageKeyPrefixes.NormalizeStorageKey(enc.EncounterId);
			if (storageKey != enc.EncounterId && EncounterLabels.TryGetValue(storageKey, out value))
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
