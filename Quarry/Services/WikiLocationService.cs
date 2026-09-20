using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Models.Markers;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class WikiLocationService : IWikiLocationService
	{
		private class WikiRowLocation
		{
			public int Row { get; set; }

			public bool HasCoordinate { get; set; }

			public float ContinentX { get; set; }

			public float ContinentY { get; set; }

			public IReadOnlyList<string> PlaceNames { get; set; }
		}

		private static readonly Regex HtmlTagRegex = new Regex("<[^>]+>", RegexOptions.Compiled);

		private readonly IAchievementService achievementService;

		private readonly IWikiSubpageDataService wikiSubpageDataService;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly ICurrentMapService currentMapService;

		private readonly Logger logger;

		private readonly object buildLock = new object();

		private Dictionary<string, Vector2> continentCoordinatesByLink;

		private Dictionary<string, List<string>> placeNamesByLink = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<int, IReadOnlyList<WikiRowLocation>> rowsByAchievementId = new Dictionary<int, IReadOnlyList<WikiRowLocation>>();

		public WikiLocationService(IAchievementService achievementService, IWikiSubpageDataService wikiSubpageDataService, IBitAlignmentService bitAlignmentService, ICurrentMapService currentMapService, Logger logger)
		{
			this.achievementService = achievementService;
			this.wikiSubpageDataService = wikiSubpageDataService;
			this.bitAlignmentService = bitAlignmentService;
			this.currentMapService = currentMapService;
			this.logger = logger;
		}

		public bool HasAnyLocations(int achievementId)
		{
			return GetRows(achievementId).Count > 0;
		}

		public IReadOnlyList<AchievementObjective> GetRemainingOnMap(int achievementId, int mapId)
		{
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			IReadOnlyList<WikiRowLocation> rows = GetRows(achievementId);
			if (rows.Count == 0)
			{
				return Array.Empty<AchievementObjective>();
			}
			currentMapService.TryGetSectors(mapId, out var sectors);
			List<AchievementObjective> result = new List<AchievementObjective>();
			foreach (WikiRowLocation row in rows)
			{
				int bit = bitAlignmentService.MapRowToBit(achievementId, row.Row);
				if (bit < 0 || achievementService.HasFinishedBitIndex(achievementId, bit))
				{
					continue;
				}
				string sectorName = null;
				Vector2 world;
				if (row.HasCoordinate)
				{
					if (!currentMapService.TryContinentToWorld(mapId, row.ContinentX, row.ContinentY, out world))
					{
						continue;
					}
				}
				else
				{
					MapSector sector = FindSectorMatch(sectors, row.PlaceNames);
					if (sector == null)
					{
						continue;
					}
					world = sector.World;
					sectorName = sector.Name;
				}
				result.Add(new AchievementObjective
				{
					Namespace = null,
					Bit = bit,
					MapId = mapId,
					X = world.X,
					Y = 0f,
					Z = world.Y,
					IsTrail = false,
					Waypoint = null,
					HeightUnknown = true,
					SectorName = sectorName,
					Source = ObjectiveSource.Wiki
				});
			}
			return result;
		}

		public bool HasAreaOnlyRemaining(int achievementId, int mapId)
		{
			IReadOnlyList<WikiRowLocation> rows = GetRows(achievementId);
			if (rows.Count == 0 || !currentMapService.TryGetMapName(mapId, out var mapName) || string.IsNullOrEmpty(mapName))
			{
				return false;
			}
			foreach (WikiRowLocation row in rows)
			{
				if (row.HasCoordinate || row.PlaceNames == null)
				{
					continue;
				}
				int bit = bitAlignmentService.MapRowToBit(achievementId, row.Row);
				if (bit < 0 || achievementService.HasFinishedBitIndex(achievementId, bit))
				{
					continue;
				}
				foreach (string placeName in row.PlaceNames)
				{
					if (string.Equals(placeName, mapName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static MapSector FindSectorMatch(IReadOnlyList<MapSector> sectors, IReadOnlyList<string> placeNames)
		{
			if (sectors == null || sectors.Count == 0 || placeNames == null)
			{
				return null;
			}
			foreach (string place in placeNames)
			{
				foreach (MapSector sector in sectors)
				{
					if (string.Equals(place, sector.Name, StringComparison.OrdinalIgnoreCase))
					{
						return sector;
					}
				}
			}
			return null;
		}

		private IReadOnlyList<WikiRowLocation> GetRows(int achievementId)
		{
			lock (buildLock)
			{
				if (rowsByAchievementId.TryGetValue(achievementId, out var cached))
				{
					return cached;
				}
				IReadOnlyList<WikiRowLocation> rows = BuildRows(achievementId);
				rowsByAchievementId[achievementId] = rows;
				return rows;
			}
		}

		private IReadOnlyList<WikiRowLocation> BuildRows(int achievementId)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			IReadOnlyDictionary<string, Vector2> byLink = EnsureCoordinateIndex();
			if ((byLink.Count == 0 && placeNamesByLink.Count == 0) || !achievementService.AchievementsById.TryGetValue(achievementId, out var achievement))
			{
				return Array.Empty<WikiRowLocation>();
			}
			IReadOnlyList<string> entries = GetEntryLinks(achievement);
			if (entries == null)
			{
				return Array.Empty<WikiRowLocation>();
			}
			List<WikiRowLocation> rows = new List<WikiRowLocation>();
			for (int row = 0; row < entries.Count; row++)
			{
				string link = NormalizeLink(entries[row]);
				if (link != null)
				{
					List<string> places;
					if (byLink.TryGetValue(link, out var coordinate))
					{
						rows.Add(new WikiRowLocation
						{
							Row = row,
							HasCoordinate = true,
							ContinentX = coordinate.X,
							ContinentY = coordinate.Y
						});
					}
					else if (placeNamesByLink.TryGetValue(link, out places))
					{
						rows.Add(new WikiRowLocation
						{
							Row = row,
							PlaceNames = places
						});
					}
				}
			}
			if (rows.Count > 0)
			{
				bitAlignmentService.PrefetchAsync(achievementId, achievement);
			}
			return rows;
		}

		private static IReadOnlyList<string> GetEntryLinks(AchievementTableEntry achievement)
		{
			AchievementTableEntryDescription description = achievement.Description;
			CollectionDescription collection = description as CollectionDescription;
			if (collection == null)
			{
				return (description as ObjectivesDescription)?.EntryList.Select((TableDescriptionEntry e) => e.Link).ToList();
			}
			return collection.EntryList.Select((CollectionDescriptionEntry e) => e.Link).ToList();
		}

		private static string NormalizeLink(string link)
		{
			if (string.IsNullOrEmpty(link))
			{
				return null;
			}
			if (!link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
			{
				return "https://wiki.guildwars2.com" + link;
			}
			return link;
		}

		private IReadOnlyDictionary<string, Vector2> EnsureCoordinateIndex()
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (continentCoordinatesByLink != null)
			{
				return continentCoordinatesByLink;
			}
			Dictionary<string, Vector2> byLink = new Dictionary<string, Vector2>(StringComparer.OrdinalIgnoreCase);
			Dictionary<string, List<string>> placesByLink = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
			IReadOnlyDictionary<string, DerivedSubpage> subpages = wikiSubpageDataService.ByLink;
			if (subpages.Count == 0)
			{
				return byLink;
			}
			foreach (KeyValuePair<string, DerivedSubpage> pair in subpages)
			{
				string link = pair.Key;
				DerivedSubpage subpage = pair.Value;
				if (TryParseCoordinates(subpage.Coordinates, out var coordinate))
				{
					byLink[link] = coordinate;
				}
				if (!string.IsNullOrEmpty(subpage.Title))
				{
					AddPlace(placesByLink, link, subpage.Title.Trim());
				}
				if (subpage.Places == null)
				{
					continue;
				}
				foreach (DerivedSubpagePlace place in subpage.Places)
				{
					foreach (string name in ParsePlaceNames(place.Value))
					{
						AddPlace(placesByLink, link, name);
					}
				}
			}
			continentCoordinatesByLink = byLink;
			placeNamesByLink = placesByLink;
			logger.Info($"WikiLocationService: indexed {byLink.Count} wiki subpage coordinate(s) and {placesByLink.Count} subpage place name(s).");
			return byLink;
		}

		private static void AddPlace(Dictionary<string, List<string>> placesByLink, string link, string place)
		{
			if (!string.IsNullOrEmpty(place))
			{
				if (!placesByLink.TryGetValue(link, out var existing))
				{
					existing = (placesByLink[link] = new List<string>());
				}
				if (!existing.Contains(place, StringComparer.OrdinalIgnoreCase))
				{
					existing.Add(place);
				}
			}
		}

		private static List<string> ParsePlaceNames(string raw)
		{
			List<string> result = new List<string>();
			if (string.IsNullOrWhiteSpace(raw))
			{
				return result;
			}
			string[] array = raw.Split(new string[1] { "<br>" }, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string trimmedSegment = array[i].Trim();
				if (trimmedSegment.Length != 0 && !trimmedSegment.StartsWith("<small>", StringComparison.OrdinalIgnoreCase))
				{
					string name = WebUtility.HtmlDecode(HtmlTagRegex.Replace(trimmedSegment, string.Empty)).Trim();
					if (name.Length > 0)
					{
						result.Add(name);
					}
				}
			}
			return result;
		}

		internal static bool TryParseCoordinates(string raw, out Vector2 coordinate)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			coordinate = default(Vector2);
			if (string.IsNullOrWhiteSpace(raw))
			{
				return false;
			}
			string[] parts = raw.Trim().Trim('[', ']').Split(',');
			if (parts.Length != 2 || !float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var x) || !float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
			{
				return false;
			}
			coordinate = new Vector2(x, y);
			return true;
		}
	}
}
