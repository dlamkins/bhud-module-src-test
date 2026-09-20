using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.Interfaces;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class BitAlignmentService : IBitAlignmentService
	{
		private const int ApiBatchSize = 200;

		private static readonly Dictionary<int, Func<int, int>> LegacySpecialSnowflakeTable = new Dictionary<int, Func<int, int>>
		{
			{
				5693,
				(int index) => index switch
				{
					5 => 8, 
					4 => 7, 
					3 => 6, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5700,
				(int index) => index switch
				{
					3 => 8, 
					2 => 5, 
					1 => 2, 
					0 => 1, 
					_ => -1, 
				}
			},
			{
				5704,
				(int index) => index switch
				{
					3 => 8, 
					2 => 5, 
					1 => 2, 
					0 => 1, 
					_ => -1, 
				}
			},
			{
				5703,
				(int index) => index switch
				{
					6 => 7, 
					5 => 5, 
					4 => 4, 
					3 => 3, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5697,
				(int index) => index switch
				{
					4 => 6, 
					3 => 5, 
					2 => 3, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5688,
				(int index) => index switch
				{
					4 => 8, 
					3 => 7, 
					2 => 6, 
					1 => 4, 
					0 => 3, 
					_ => -1, 
				}
			},
			{
				5709,
				(int index) => index switch
				{
					4 => 7, 
					3 => 6, 
					2 => 4, 
					1 => 2, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5698,
				(int index) => index switch
				{
					3 => 6, 
					2 => 4, 
					1 => 3, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5691,
				(int index) => index switch
				{
					3 => 7, 
					2 => 6, 
					1 => 5, 
					0 => 4, 
					_ => -1, 
				}
			},
			{
				5708,
				(int index) => index switch
				{
					4 => 8, 
					3 => 5, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			}
		};

		private static readonly Dictionary<int, Func<int, int>> ManualOverrides = new Dictionary<int, Func<int, int>> { 
		{
			5691,
			(int index) => index switch
			{
				3 => 7, 
				2 => 6, 
				1 => 5, 
				0 => 4, 
				_ => -1, 
			}
		} };

		private readonly IAchievementService achievementService;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly Logger logger;

		private readonly object achievementCacheLock = new object();

		private readonly Dictionary<int, Achievement> achievementCacheById = new Dictionary<int, Achievement>();

		private readonly object nameCacheLock = new object();

		private readonly Dictionary<int, string> skinNamesById = new Dictionary<int, string>();

		private readonly Dictionary<int, string> miniNamesById = new Dictionary<int, string>();

		private readonly object alignmentLock = new object();

		private readonly Dictionary<int, int[]> rowToBitByAchievementId = new Dictionary<int, int[]>();

		private readonly Dictionary<int, Dictionary<int, int>> bitToRowByAchievementId = new Dictionary<int, Dictionary<int, int>>();

		private readonly HashSet<int> alignmentInFlight = new HashSet<int>();

		public event Action<int> AlignmentLoaded;

		public BitAlignmentService(IAchievementService achievementService, Gw2ApiManager gw2ApiManager, Logger logger)
		{
			this.achievementService = achievementService;
			this.gw2ApiManager = gw2ApiManager;
			this.logger = logger;
		}

		public int MapRowToBit(int achievementId, int rowIndex)
		{
			lock (alignmentLock)
			{
				if (rowToBitByAchievementId.TryGetValue(achievementId, out var map))
				{
					if (rowIndex >= 0)
					{
						if (rowIndex < map.Length)
						{
							return map[rowIndex];
						}
						return rowIndex;
					}
					return rowIndex;
				}
				return rowIndex;
			}
		}

		public int MapBitToRow(int achievementId, int bit)
		{
			lock (alignmentLock)
			{
				int row;
				if (bitToRowByAchievementId.TryGetValue(achievementId, out var map))
				{
					return map.TryGetValue(bit, out row) ? row : (-1);
				}
				return bit;
			}
		}

		public async Task PrefetchAsync(int achievementId, AchievementTableEntry achievement, CancellationToken cancellationToken = default(CancellationToken))
		{
			lock (alignmentLock)
			{
				if (rowToBitByAchievementId.ContainsKey(achievementId) || !alignmentInFlight.Add(achievementId))
				{
					return;
				}
			}
			try
			{
				if (await GetOrComputeRowToBitAsync(achievement, cancellationToken) != null)
				{
					this.AlignmentLoaded?.Invoke(achievementId);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"BitAlignmentService: failed to align achievement {achievementId}; falling back to identity mapping.");
			}
			finally
			{
				lock (alignmentLock)
				{
					alignmentInFlight.Remove(achievementId);
				}
			}
		}

		private async Task<int[]> GetOrComputeRowToBitAsync(AchievementTableEntry achievement, CancellationToken cancellationToken)
		{
			lock (alignmentLock)
			{
				if (rowToBitByAchievementId.TryGetValue(achievement.Id, out var cached))
				{
					return cached;
				}
			}
			IReadOnlyList<BitAlignmentMatcher.Row> rows = BitAlignmentMatcher.GetRows(achievement.Description);
			if (rows == null || rows.Count == 0)
			{
				return null;
			}
			if (!(await GetAchievementsAsync(new int[1] { achievement.Id }, cancellationToken)).TryGetValue(achievement.Id, out var apiAchievement) || apiAchievement.get_Bits() == null || apiAchievement.get_Bits().Count == 0)
			{
				return null;
			}
			List<int> skinIds = (from b in apiAchievement.get_Bits().OfType<AchievementSkinBit>()
				select b.get_Id()).Distinct().ToList();
			List<int> miniIds = (from b in apiAchievement.get_Bits().OfType<AchievementMinipetBit>()
				select b.get_Id()).Distinct().ToList();
			await EnsureSkinNamesAsync(skinIds, cancellationToken);
			await EnsureMiniNamesAsync(miniIds, cancellationToken);
			IReadOnlyDictionary<int, string> skinNamesSnapshot;
			IReadOnlyDictionary<int, string> miniNamesSnapshot;
			lock (nameCacheLock)
			{
				skinNamesSnapshot = new Dictionary<int, string>(skinNamesById);
				miniNamesSnapshot = new Dictionary<int, string>(miniNamesById);
			}
			int[] rowToBit = BitAlignmentMatcher.ComputeRowToBit(rows, apiAchievement.get_Bits(), skinNamesSnapshot, miniNamesSnapshot);
			if (ManualOverrides.TryGetValue(achievement.Id, out var overrideFunc))
			{
				for (int row2 = 0; row2 < rowToBit.Length; row2++)
				{
					int overridden = overrideFunc(row2);
					if (overridden != -1)
					{
						rowToBit[row2] = overridden;
					}
				}
			}
			if (!BitAlignmentMatcher.IsIdentity(rowToBit))
			{
				int unresolvedCount = rowToBit.Count((int b) => b == -1);
				logger.Debug($"BitAlignmentService: achievement {achievement.Id} ('{achievement.Name}') needed row/bit alignment; {unresolvedCount} of {rowToBit.Length} row(s) unresolved.");
			}
			Dictionary<int, int> bitToRow = new Dictionary<int, int>();
			for (int row = 0; row < rowToBit.Length; row++)
			{
				if (rowToBit[row] >= 0)
				{
					bitToRow[rowToBit[row]] = row;
				}
			}
			lock (alignmentLock)
			{
				rowToBitByAchievementId[achievement.Id] = rowToBit;
				bitToRowByAchievementId[achievement.Id] = bitToRow;
			}
			return rowToBit;
		}

		public bool TryGetCachedAchievement(int achievementId, out Achievement achievement)
		{
			lock (achievementCacheLock)
			{
				return achievementCacheById.TryGetValue(achievementId, out achievement);
			}
		}

		public async Task<IReadOnlyDictionary<int, Achievement>> GetAchievementsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken))
		{
			List<int> idList = ids.Distinct().ToList();
			List<int> missing;
			lock (achievementCacheLock)
			{
				missing = idList.Where((int id) => !achievementCacheById.ContainsKey(id)).ToList();
			}
			for (int i = 0; i < missing.Count; i += 200)
			{
				List<int> chunk = missing.Skip(i).Take(200).ToList();
				try
				{
					IReadOnlyList<Achievement> fetched = await ((IBulkExpandableClient<Achievement, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()).ManyAsync((IEnumerable<int>)chunk, cancellationToken);
					lock (achievementCacheLock)
					{
						foreach (Achievement fetchedAchievement in fetched)
						{
							achievementCacheById[fetchedAchievement.get_Id()] = fetchedAchievement;
						}
					}
				}
				catch (Exception ex)
				{
					logger.Warn(ex, $"BitAlignmentService: failed to fetch {chunk.Count} achievement(s); they'll be missing from this pass' results.");
				}
			}
			lock (achievementCacheLock)
			{
				return idList.Where(achievementCacheById.ContainsKey).ToDictionary((int id) => id, (int id) => achievementCacheById[id]);
			}
		}

		private async Task EnsureSkinNamesAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken)
		{
			List<int> missing;
			lock (nameCacheLock)
			{
				missing = ids.Where((int id) => !skinNamesById.ContainsKey(id)).ToList();
			}
			if (missing.Count == 0)
			{
				return;
			}
			try
			{
				IReadOnlyList<Skin> skins = await ((IBulkExpandableClient<Skin, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Skins()).ManyAsync((IEnumerable<int>)missing, cancellationToken);
				lock (nameCacheLock)
				{
					foreach (Skin skin in skins)
					{
						skinNamesById[skin.get_Id()] = skin.get_Name();
					}
				}
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"BitAlignmentService: failed to fetch {missing.Count} skin name(s).");
			}
		}

		private async Task EnsureMiniNamesAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken)
		{
			List<int> missing;
			lock (nameCacheLock)
			{
				missing = ids.Where((int id) => !miniNamesById.ContainsKey(id)).ToList();
			}
			if (missing.Count == 0)
			{
				return;
			}
			try
			{
				IReadOnlyList<Mini> minis = await ((IBulkExpandableClient<Mini, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Minis()).ManyAsync((IEnumerable<int>)missing, cancellationToken);
				lock (nameCacheLock)
				{
					foreach (Mini mini in minis)
					{
						miniNamesById[mini.get_Id()] = mini.get_Name();
					}
				}
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"BitAlignmentService: failed to fetch {missing.Count} minipet name(s).");
			}
		}

		public async Task RunValidationAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			List<AchievementTableEntry> candidates = achievementService.Achievements.Where((AchievementTableEntry a) => BitAlignmentMatcher.GetRows(a.Description) != null).ToList();
			logger.Info($"BitAlignmentService validation: checking {candidates.Count} collection/objective achievement(s)...");
			int checkedCount = 0;
			int unresolvedAchievements = 0;
			int totalUnresolvedRows = 0;
			List<string> legacyMismatches = new List<string>();
			foreach (AchievementTableEntry achievement in candidates)
			{
				cancellationToken.ThrowIfCancellationRequested();
				int[] rowToBit;
				try
				{
					rowToBit = await GetOrComputeRowToBitAsync(achievement, cancellationToken);
				}
				catch (Exception ex)
				{
					logger.Warn(ex, $"BitAlignmentService validation: failed to align achievement {achievement.Id} ('{achievement.Name}').");
					continue;
				}
				if (rowToBit == null)
				{
					continue;
				}
				checkedCount++;
				int unresolvedCount = rowToBit.Count((int b) => b == -1);
				if (unresolvedCount > 0)
				{
					unresolvedAchievements++;
					totalUnresolvedRows += unresolvedCount;
				}
				if (!LegacySpecialSnowflakeTable.TryGetValue(achievement.Id, out var legacyFunc))
				{
					continue;
				}
				for (int row = 0; row < rowToBit.Length; row++)
				{
					int legacyBit = legacyFunc(row);
					if (legacyBit != -1 && legacyBit != rowToBit[row])
					{
						legacyMismatches.Add($"id={achievement.Id} ('{achievement.Name}') row={row} legacy_bit={legacyBit} generated_bit={rowToBit[row]}");
					}
				}
			}
			logger.Info($"BitAlignmentService validation: checked {checkedCount} achievement(s) with API bits; {unresolvedAchievements} had at least one unresolved row ({totalUnresolvedRows} unresolved row(s) total).");
			if (legacyMismatches.Count == 0)
			{
				logger.Info("BitAlignmentService validation: generated maps for the specialSnowflakeCompletedHandling ids match the legacy table exactly.");
				return;
			}
			foreach (string mismatch in legacyMismatches)
			{
				logger.Warn("BitAlignmentService validation: legacy/generated mismatch: " + mismatch);
			}
		}
	}
}
