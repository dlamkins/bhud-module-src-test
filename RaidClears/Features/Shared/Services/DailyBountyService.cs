using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
			if (bountyData == null || !bountyData.Enabled)
			{
				return Enumerable.Empty<Encounter>();
			}
			return GetDayOfYearBounties(PriorityRotationService.DayOfYearIndex(), bountyData, raidData, "priority_");
		}

		public static IEnumerable<Encounter> GetTomorrowBounties()
		{
			DailyBountyData bountyData = Service.DailyBountyData;
			RaidData raidData = Service.RaidData;
			_ = Service.StrikeData;
			if (bountyData == null || !bountyData.Enabled)
			{
				return Enumerable.Empty<Encounter>();
			}
			return GetDayOfYearBounties(PriorityRotationService.DayOfYearIndex() + 1, bountyData, raidData, "tomorrow_");
		}

		[IteratorStateMachine(typeof(_003CGetBountyEncounterApiIdsForDay_003Ed__2))]
		public static IEnumerable<string> GetBountyEncounterApiIdsForDay(int dayIndex)
		{
			return new _003CGetBountyEncounterApiIdsForDay_003Ed__2(-2)
			{
				_003C_003E3__dayIndex = dayIndex
			};
		}

		private static IEnumerable<Encounter> GetDayOfYearBounties(int dayIndex, DailyBountyData bountyData, RaidData raidData, string prefix)
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
				return ResolveBountyEncounters(references, raidData, prefix);
			}
			int legacyIndex = (dayIndex + bountyData.Offset) % bountyData.Modulo;
			if (legacyIndex < bountyData.Rotation.Count)
			{
				return ResolveBountyEncounters(bountyData.Rotation[legacyIndex], raidData, prefix);
			}
			return Enumerable.Empty<Encounter>();
		}

		private static IEnumerable<Encounter> ResolveBountyEncounters(List<BountyEncounterReference> references, RaidData raidData, string prefix)
		{
			List<Encounter> encounters = new List<Encounter>();
			foreach (BountyEncounterReference reference in references)
			{
				BossEncounter raidEncounter = raidData.GetRaidEncounterByApiId(reference.EncounterId);
				if (raidEncounter != null)
				{
					string baseApiId = ((raidEncounter.ApiId != null && raidEncounter.ApiId != "undefined") ? raidEncounter.ApiId : raidEncounter.Id);
					BossEncounter copy = new BossEncounter
					{
						Id = prefix + raidEncounter.Id,
						ApiId = prefix + baseApiId,
						Name = raidEncounter.Name,
						Abbriviation = raidEncounter.Abbriviation,
						AssetId = raidEncounter.AssetId,
						MapIds = (raidEncounter.MapIds ?? new List<int>()),
						DailyBountyAchievementId = raidEncounter.DailyBountyAchievementId,
						MentorAchievementId = raidEncounter.MentorAchievementId,
						PowerFavored = raidEncounter.PowerFavored,
						CondiFavored = raidEncounter.CondiFavored,
						NeedsDefianceBreak = raidEncounter.NeedsDefianceBreak
					};
					encounters.Add(new Encounter(copy, raidEncounter.IsStrike));
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
