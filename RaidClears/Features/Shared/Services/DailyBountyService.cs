using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Shared.Services
{
	public static class DailyBountyService
	{
		public static IEnumerable<Encounter> GetDailyBounties()
		{
			DailyBountyData bountyData = Service.DailyBountyData;
			RaidData raidData = Service.RaidData;
			StrikeData strikeData = Service.StrikeData;
			if (bountyData == null || !bountyData.Enabled)
			{
				return Enumerable.Empty<Encounter>();
			}
			return GetDayOfYearBounties(PriorityRotationService.DayOfYearIndex(), bountyData, raidData, strikeData);
		}

		public static IEnumerable<Encounter> GetTomorrowBounties()
		{
			DailyBountyData bountyData = Service.DailyBountyData;
			RaidData raidData = Service.RaidData;
			StrikeData strikeData = Service.StrikeData;
			if (bountyData == null || !bountyData.Enabled)
			{
				return Enumerable.Empty<Encounter>();
			}
			return GetDayOfYearBounties(PriorityRotationService.DayOfYearIndex() + 1, bountyData, raidData, strikeData);
		}

		private static IEnumerable<Encounter> GetDayOfYearBounties(int dayIndex, DailyBountyData bountyData, RaidData raidData, StrikeData strikeData)
		{
			if (bountyData.BossSlots != null && bountyData.BossSlots.Count > 0)
			{
				List<BountyEncounterReference> references = new List<BountyEncounterReference>();
				foreach (BossSlotRotation slot in bountyData.BossSlots)
				{
					if (slot.Encounters != null && slot.Encounters.Count != 0)
					{
						int modulo = slot.Encounters.Count;
						int indexForSlot = (dayIndex + slot.Offset) % modulo;
						if (indexForSlot < 0)
						{
							indexForSlot += modulo;
						}
						string encounterId = slot.Encounters[indexForSlot];
						if (!string.IsNullOrWhiteSpace(encounterId))
						{
							references.Add(new BountyEncounterReference
							{
								EncounterId = encounterId
							});
						}
					}
				}
				return ResolveBountyEncounters(references, raidData, strikeData);
			}
			int legacyIndex = (dayIndex + bountyData.Offset) % bountyData.Modulo;
			if (legacyIndex < bountyData.Rotation.Count)
			{
				return ResolveBountyEncounters(bountyData.Rotation[legacyIndex], raidData, strikeData);
			}
			return Enumerable.Empty<Encounter>();
		}

		private static IEnumerable<Encounter> ResolveBountyEncounters(List<BountyEncounterReference> references, RaidData raidData, StrikeData strikeData)
		{
			List<Encounter> encounters = new List<Encounter>();
			foreach (BountyEncounterReference reference in references)
			{
				RaidEncounter raidEncounter = raidData.GetRaidEncounterByApiId(reference.EncounterId);
				if (raidEncounter != null)
				{
					Encounter encounter = new Encounter(raidEncounter);
					encounters.Add(encounter);
				}
				else
				{
					Module.ModuleLogger.Warn("Could not resolve bounty encounter: " + reference.EncounterId);
				}
			}
			return encounters;
		}
	}
}
