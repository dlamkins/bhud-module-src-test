using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Services
{
	internal class WvwService
	{
		private Gw2ApiManager _api;

		private Dictionary<int, IEnumerable<WvwObjectiveEntity>> _wvwObjectiveCache;

		private DateTime _prevApiRequestTime = DateTime.UtcNow;

		private Dictionary<int, Map> _mapCache;

		public IEnumerable<WvwObjectiveEntity> CurrentObjectives
		{
			get
			{
				IEnumerable<WvwObjectiveEntity> objEntities = default(IEnumerable<WvwObjectiveEntity>);
				if (!(_wvwObjectiveCache?.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out objEntities) ?? false))
				{
					return null;
				}
				return objEntities;
			}
		}

		public Guid? CurrentGuild { get; private set; }

		public WvwService(Gw2ApiManager api)
		{
			_api = api;
			_wvwObjectiveCache = new Dictionary<int, IEnumerable<WvwObjectiveEntity>>();
			_mapCache = new Dictionary<int, Map>();
		}

		public async Task Update()
		{
			if (DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 15.0)
			{
				return;
			}
			_prevApiRequestTime = DateTime.UtcNow;
			CurrentGuild = await GetRepresentedGuild();
			if (!GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld() || !_wvwObjectiveCache.ContainsKey(GameService.Gw2Mumble.get_CurrentMap().get_Id()))
			{
				return;
			}
			int worldId = await GetWorldId();
			if (worldId != -1)
			{
				int[] array = await GetWvWMapIds(worldId);
				foreach (int id in array)
				{
					await UpdateObjectives(worldId, id);
				}
			}
		}

		public async Task<Guid?> GetRepresentedGuild()
		{
			if (!_api.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return null;
			}
			return await ((IBlobClient<Character>)(object)_api.get_Gw2ApiClient().get_V2().get_Characters()
				.get_Item(GameService.Gw2Mumble.get_PlayerCharacter().get_Name())).GetAsync(default(CancellationToken)).ContinueWith((Task<Character> t) => t.IsFaulted ? null : t.Result.get_Guild());
		}

		public async Task<int> GetWorldId()
		{
			if (!_api.HasPermission((TokenPermission)1))
			{
				return -1;
			}
			return await ((IBlobClient<Account>)(object)_api.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith((Task<Account> task) => task.IsFaulted ? (-1) : task.Result.get_World());
		}

		public async Task<int[]> GetWvWMapIds(int worldId)
		{
			return await ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith((Task<WvwMatch> t) => t.IsFaulted ? Array.Empty<int>() : (from m in t.Result.get_Maps()
				select m.get_Id()).ToArray());
		}

		private async Task UpdateObjectives(int worldId, int mapId)
		{
			IEnumerable<WvwObjectiveEntity> objEntities = await GetObjectives(mapId);
			IReadOnlyList<WvwMatchMapObjective> objectives = await RequestObjectives(worldId, GameService.Gw2Mumble.get_CurrentMap().get_Id());
			if (objectives.IsNullOrEmpty())
			{
				return;
			}
			foreach (WvwObjectiveEntity objEntity in objEntities)
			{
				WvwMatchMapObjective obj = objectives.FirstOrDefault((WvwMatchMapObjective v) => v.get_Id().Equals(objEntity.Id, StringComparison.InvariantCultureIgnoreCase));
				if (obj != null)
				{
					objEntity.LastFlipped = obj.get_LastFlipped() ?? DateTime.MinValue;
					objEntity.Owner = obj.get_Owner().get_Value();
					objEntity.ClaimedBy = obj.get_ClaimedBy() ?? Guid.Empty;
					objEntity.GuildUpgrades = obj.get_GuildUpgrades();
					objEntity.YaksDelivered = obj.get_YaksDelivered().GetValueOrDefault();
				}
			}
		}

		private async Task<IReadOnlyList<WvwMatchMapObjective>> RequestObjectives(int worldId, int mapId)
		{
			return await ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith(delegate(Task<WvwMatch> task)
			{
				if (task.IsFaulted)
				{
					return null;
				}
				WvwMatchMap obj = task.Result.get_Maps().FirstOrDefault((WvwMatchMap x) => x.get_Id() == mapId);
				return (obj == null) ? null : obj.get_Objectives();
			});
		}

		public bool TryGetMap(int mapId, out Map map)
		{
			return _mapCache.TryGetValue(mapId, out map);
		}

		public async Task<IEnumerable<WvwObjectiveEntity>> GetObjectives(int mapId)
		{
			if (_wvwObjectiveCache.TryGetValue(mapId, out var objEntities))
			{
				return objEntities;
			}
			return await ((IAllExpandableClient<WvwObjective>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Objectives()).AllAsync(default(CancellationToken)).ContinueWith(async delegate(Task<IApiV2ObjectList<WvwObjective>> task)
			{
				if (task.IsFaulted)
				{
					return Enumerable.Empty<WvwObjectiveEntity>();
				}
				Map map = await MapUtil.RequestMap(mapId);
				if (!_mapCache.ContainsKey(map.get_Id()))
				{
					_mapCache.Add(map.get_Id(), map);
				}
				IEnumerable<ContinentFloorRegionMapSector> sectors = await MapUtil.RequestSectorsForFloor(map.get_ContinentId(), map.get_DefaultFloor(), map.get_RegionId(), mapId);
				if (sectors == null)
				{
					return Enumerable.Empty<WvwObjectiveEntity>();
				}
				List<WvwObjectiveEntity> newObjectives = new List<WvwObjectiveEntity>();
				foreach (ContinentFloorRegionMapSector sector in sectors)
				{
					WvwObjective obj = ((IEnumerable<WvwObjective>)task.Result).FirstOrDefault((WvwObjective x) => x.get_SectorId() == sector.get_Id());
					if (obj != null)
					{
						WvwObjectiveEntity o = new WvwObjectiveEntity(obj, map, sector);
						newObjectives.Add(o);
					}
				}
				_wvwObjectiveCache.Add(map.get_Id(), newObjectives);
				return newObjectives;
			}).Unwrap();
		}
	}
}
