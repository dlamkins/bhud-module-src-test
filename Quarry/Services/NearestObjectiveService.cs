using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Models.Markers;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class NearestObjectiveService : INearestObjectiveService
	{
		private const float WaypointWorthTakingMarginMetres = 150f;

		private readonly IMarkerPackIndexService markerPackIndexService;

		private readonly IAchievementService achievementService;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly IWikiLocationService wikiLocationService;

		private readonly ICurrentMapService currentMapService;

		private readonly Logger logger;

		public NearestObjectiveService(IMarkerPackIndexService markerPackIndexService, IAchievementService achievementService, IBitAlignmentService bitAlignmentService, IWikiLocationService wikiLocationService, ICurrentMapService currentMapService, Logger logger)
		{
			this.currentMapService = currentMapService;
			this.logger = logger;
			this.markerPackIndexService = markerPackIndexService;
			this.achievementService = achievementService;
			this.bitAlignmentService = bitAlignmentService;
			this.wikiLocationService = wikiLocationService;
		}

		public IReadOnlyList<RemainingObjective> GetRemaining(int achievementId, int mapId, Vector3 player)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			markerPackIndexService.TryGet(achievementId, out var route);
			Dictionary<int, RemainingObjective> nearestByBit = new Dictionary<int, RemainingObjective>();
			RemainingObjective nearestUntagged = null;
			foreach (AchievementObjective objective in RemainingOnMap(route, achievementId, mapId))
			{
				float distance = ObjectiveGeometry.DistanceMetres(player, objective);
				RemainingObjective existing;
				if (objective.Bit < 0)
				{
					if (nearestUntagged == null || (double)distance < nearestUntagged.DistanceMetres)
					{
						nearestUntagged = new RemainingObjective
						{
							Bit = -1,
							Row = -1,
							Name = GetAchievementName(achievementId),
							DistanceMetres = distance,
							Namespace = objective.Namespace,
							IsTrail = objective.IsTrail,
							Waypoint = objective.Waypoint,
							GroundDistanceOnly = objective.HeightUnknown,
							AreaHint = objective.SectorName
						};
					}
				}
				else if (!nearestByBit.TryGetValue(objective.Bit, out existing) || !(existing.DistanceMetres <= (double)distance))
				{
					int row = bitAlignmentService.MapBitToRow(achievementId, objective.Bit);
					string rowDisplayName = GetRowDisplayName(achievementId, row);
					string name = ((rowDisplayName != null) ? $"{row + 1}. {rowDisplayName}" : $"objective #{objective.Bit}");
					nearestByBit[objective.Bit] = new RemainingObjective
					{
						Bit = objective.Bit,
						Row = row,
						Name = name,
						DistanceMetres = distance,
						Namespace = objective.Namespace,
						IsTrail = objective.IsTrail,
						Waypoint = objective.Waypoint,
						GroundDistanceOnly = objective.HeightUnknown,
						AreaHint = objective.SectorName
					};
				}
			}
			List<RemainingObjective> result = nearestByBit.Values.ToList();
			if (nearestUntagged != null)
			{
				result.Add(nearestUntagged);
			}
			return result.OrderBy((RemainingObjective r) => r.DistanceMetres).ToList();
		}

		public GuidanceInfo GetGuidance(int achievementId, int mapId)
		{
			int remainingTagged = 0;
			bool hasRoute = false;
			bool hasTrail = false;
			if (markerPackIndexService.TryGet(achievementId, out var route))
			{
				foreach (AchievementObjective objective in route.Objectives)
				{
					if (objective.MapId == mapId)
					{
						if (objective.Bit < 0)
						{
							hasRoute = true;
							hasTrail |= objective.IsTrail;
						}
						else if (!achievementService.HasFinishedBitIndex(achievementId, objective.Bit))
						{
							remainingTagged++;
						}
					}
				}
			}
			if (remainingTagged > 0)
			{
				return new GuidanceInfo
				{
					Tier = GuidanceTier.Tagged,
					RemainingTagged = remainingTagged,
					HasRoute = hasRoute,
					HasTrail = hasTrail
				};
			}
			if (wikiLocationService.GetRemainingOnMap(achievementId, mapId).Count > 0)
			{
				return new GuidanceInfo
				{
					Tier = GuidanceTier.Coordinate,
					HasRoute = hasRoute,
					HasTrail = hasTrail
				};
			}
			if (hasRoute)
			{
				return new GuidanceInfo
				{
					Tier = GuidanceTier.Route,
					HasRoute = true,
					HasTrail = hasTrail
				};
			}
			if (!wikiLocationService.HasAreaOnlyRemaining(achievementId, mapId))
			{
				return GuidanceInfo.None;
			}
			return new GuidanceInfo
			{
				Tier = GuidanceTier.Area
			};
		}

		public bool HasAnyObjectives(int achievementId)
		{
			if (!markerPackIndexService.TryGet(achievementId, out var _))
			{
				return wikiLocationService.HasAnyLocations(achievementId);
			}
			return true;
		}

		public IReadOnlyList<string> Waypoints(int achievementId)
		{
			if (!markerPackIndexService.TryGet(achievementId, out var route))
			{
				return Array.Empty<string>();
			}
			return route.Waypoints.ToList();
		}

		public WaypointSuggestion NearestWaypoint(int achievementId, int mapId, Vector3 player)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			markerPackIndexService.TryGet(achievementId, out var route);
			List<AchievementObjective> remaining = RemainingOnMap(route, achievementId, mapId);
			if (remaining.Count == 0)
			{
				return null;
			}
			float onFoot = remaining.Min((AchievementObjective o) => ObjectiveGeometry.DistanceMetres(player, o));
			if (currentMapService.TryGetWaypoints(mapId, out var mapWaypoints))
			{
				MapWaypoint best = null;
				float bestDistance = float.MaxValue;
				foreach (MapWaypoint waypoint in mapWaypoints)
				{
					Vector3 from = new Vector3(waypoint.World.X, waypoint.World.Y, 0f);
					float distance2 = remaining.Min((AchievementObjective o) => GroundDistance(from, o));
					if (distance2 < bestDistance)
					{
						bestDistance = distance2;
						best = waypoint;
					}
				}
				if (best != null)
				{
					return new WaypointSuggestion
					{
						Code = best.ChatLink,
						Name = best.Name,
						MetresFromObjective = bestDistance,
						WorthTaking = (bestDistance + 150f < onFoot),
						MetresOnFoot = onFoot
					};
				}
			}
			if (route == null)
			{
				return null;
			}
			string nearestCode = null;
			float nearestPackDistance = float.MaxValue;
			Vector3 anchor = NearestRemainingPosition(route, achievementId, mapId, player).GetValueOrDefault(player);
			foreach (AchievementObjective objective in route.Objectives)
			{
				if (objective.MapId == mapId && !string.IsNullOrEmpty(objective.Waypoint))
				{
					float distance = ObjectiveGeometry.DistanceMetres(anchor, objective);
					if (distance < nearestPackDistance)
					{
						nearestPackDistance = distance;
						nearestCode = objective.Waypoint;
					}
				}
			}
			if (nearestCode != null)
			{
				return new WaypointSuggestion
				{
					Code = nearestCode,
					WorthTaking = true
				};
			}
			return null;
		}

		private static float GroundDistance(Vector3 from, AchievementObjective objective)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 world = ObjectiveGeometry.ToWorld(objective);
			float num = from.X - world.X;
			float dy = from.Y - world.Y;
			return (float)Math.Sqrt(num * num + dy * dy);
		}

		private List<AchievementObjective> RemainingOnMap(AchievementRoute route, int achievementId, int mapId)
		{
			if (achievementService.HasFinishedAchievement(achievementId))
			{
				return new List<AchievementObjective>();
			}
			List<AchievementObjective> tagged = new List<AchievementObjective>();
			List<AchievementObjective> untagged = new List<AchievementObjective>();
			if (route != null)
			{
				foreach (AchievementObjective objective in route.Objectives)
				{
					if (objective.MapId == mapId)
					{
						if (objective.Bit < 0)
						{
							untagged.Add(objective);
						}
						else if (!achievementService.HasFinishedBitIndex(achievementId, objective.Bit))
						{
							tagged.Add(objective);
						}
					}
				}
			}
			List<AchievementObjective> remaining = new List<AchievementObjective>(tagged);
			HashSet<int> taggedBits = new HashSet<int>(tagged.Select((AchievementObjective o) => o.Bit));
			foreach (AchievementObjective wikiObjective in wikiLocationService.GetRemainingOnMap(achievementId, mapId))
			{
				if (!taggedBits.Contains(wikiObjective.Bit))
				{
					remaining.Add(wikiObjective);
				}
			}
			if (remaining.Count == 0)
			{
				remaining.AddRange(untagged);
			}
			return remaining;
		}

		private Vector3? NearestRemainingPosition(AchievementRoute route, int achievementId, int mapId, Vector3 player)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Vector3? nearest = null;
			float nearestDistance = float.MaxValue;
			foreach (AchievementObjective objective in RemainingOnMap(route, achievementId, mapId))
			{
				float distance = ObjectiveGeometry.DistanceMetres(player, objective);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearest = ObjectiveGeometry.ToWorld(objective);
				}
			}
			return nearest;
		}

		private string GetAchievementName(int achievementId)
		{
			if (!achievementService.AchievementsById.TryGetValue(achievementId, out var achievement))
			{
				return $"#{achievementId}";
			}
			return achievement.Name;
		}

		private string GetRowDisplayName(int achievementId, int row)
		{
			if (row < 0 || !achievementService.AchievementsById.TryGetValue(achievementId, out var achievement))
			{
				return null;
			}
			AchievementTableEntryDescription description = achievement.Description;
			CollectionDescription collection = description as CollectionDescription;
			if (collection == null)
			{
				ObjectivesDescription objectives = description as ObjectivesDescription;
				if (objectives != null && row < objectives.EntryList.Count)
				{
					return objectives.EntryList[row].DisplayName;
				}
			}
			else if (row < collection.EntryList.Count)
			{
				return collection.EntryList[row].DisplayName;
			}
			return null;
		}
	}
}
