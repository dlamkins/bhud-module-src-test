using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Services
{
	internal class WvwService
	{
		private Gw2ApiManager _api;

		private AsyncCache<int, IEnumerable<WvwObjectiveEntity>> _wvwObjectiveCache;

		private IEnumerable<World> _worlds;

		private WvwMatchTeamList _teams;

		private DateTime _prevApiRequestTime;

		public Guid? CurrentGuild { get; private set; }

		public WvwOwner CurrentTeam { get; private set; }

		public bool IsLoading => !string.IsNullOrEmpty(LoadingMessage);

		public string LoadingMessage { get; private set; }

		public WvwService(Gw2ApiManager api)
		{
			_prevApiRequestTime = DateTime.MinValue.ToUniversalTime();
			_api = api;
			_wvwObjectiveCache = new AsyncCache<int, IEnumerable<WvwObjectiveEntity>>(RequestObjectives);
		}

		public async Task LoadAsync()
		{
			_worlds = (IEnumerable<World>)(await ((IAllExpandableClient<World>)(object)_api.get_Gw2ApiClient().get_V2().get_Worlds()).AllAsync(default(CancellationToken)));
		}

		public async Task Update()
		{
			if (!IsLoading && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld() && !(DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 15.0))
			{
				LoadingMessage = "Refreshing";
				CurrentGuild = await GetRepresentedGuild();
				int worldId = await GetWorldId();
				if (worldId == -1)
				{
					LoadingMessage = string.Empty;
					_prevApiRequestTime = DateTime.UtcNow;
				}
				else
				{
					UpdateInBackground(worldId);
				}
			}
		}

		private void UpdateInBackground(int worldId)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				UpdateObjectives(worldId);
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private async Task UpdateObjectives(int worldId)
		{
			List<Task> taskList = new List<Task>();
			int[] array = await GetWvWMapIds(worldId);
			foreach (int id in array)
			{
				Task<Task> t = new Task<Task>(() => UpdateObjectives(worldId, id));
				taskList.Add(t);
				t.Start();
			}
			Task.WaitAll(taskList.ToArray());
			LoadingMessage = string.Empty;
			_prevApiRequestTime = DateTime.UtcNow;
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

		public string GetWorldName(WvwOwner owner)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected I4, but got Unknown
			IReadOnlyList<int> team;
			switch (owner - 2)
			{
			case 0:
				team = _teams.get_Red();
				break;
			case 1:
				team = _teams.get_Blue();
				break;
			case 2:
				team = _teams.get_Green();
				break;
			default:
				return string.Empty;
			}
			World obj = _worlds.OrderBy((World x) => x.get_Population().get_Value()).Reverse().FirstOrDefault((World y) => team.Contains(y.get_Id()));
			return ((obj != null) ? obj.get_Name() : null) ?? string.Empty;
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
			if (worldId < 0)
			{
				return Array.Empty<int>();
			}
			return await ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith((Task<WvwMatch> t) => t.IsFaulted ? Array.Empty<int>() : (from m in t.Result.get_Maps()
				select m.get_Id()).ToArray());
		}

		public async Task<IEnumerable<WvwObjectiveEntity>> GetObjectives(int mapId)
		{
			return await _wvwObjectiveCache.GetItem(mapId);
		}

		private async Task UpdateObjectives(int worldId, int mapId)
		{
			IEnumerable<WvwObjectiveEntity> objEntities = await GetObjectives(mapId);
			WvwMatch match;
			try
			{
				match = await ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
					.get_Matches()
					.World(worldId)).GetAsync(default(CancellationToken));
			}
			catch (RequestException)
			{
				return;
			}
			if (match == null)
			{
				return;
			}
			_teams = match.get_AllWorlds();
			CurrentTeam = (WvwOwner)(_teams.get_Blue().Contains(worldId) ? 3 : (_teams.get_Red().Contains(worldId) ? 2 : (_teams.get_Green().Contains(worldId) ? 4 : 0)));
			WvwMatchMap obj2 = match.get_Maps().FirstOrDefault((WvwMatchMap x) => x.get_Id() == mapId);
			IReadOnlyList<WvwMatchMapObjective> objectives = ((obj2 != null) ? obj2.get_Objectives() : null);
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

		private async Task<IEnumerable<WvwObjectiveEntity>> RequestObjectives(int mapId)
		{
			return await ((IAllExpandableClient<WvwObjective>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Objectives()).AllAsync(default(CancellationToken)).ContinueWith(async delegate(Task<IApiV2ObjectList<WvwObjective>> task)
			{
				if (task.IsFaulted)
				{
					return Enumerable.Empty<WvwObjectiveEntity>();
				}
				Map obj2 = await MapUtil.GetMap(mapId);
				ContinentFloorRegionMap mapExpanded = await MapUtil.GetMapExpanded(obj2, obj2.get_DefaultFloor());
				if (mapExpanded == null)
				{
					return Enumerable.Empty<WvwObjectiveEntity>();
				}
				List<WvwObjectiveEntity> newObjectives = new List<WvwObjectiveEntity>();
				foreach (ContinentFloorRegionMapSector sector in mapExpanded.get_Sectors().Values)
				{
					WvwObjective obj = ((IEnumerable<WvwObjective>)task.Result).FirstOrDefault((WvwObjective x) => x.get_SectorId() == sector.get_Id());
					if (obj != null)
					{
						WvwObjectiveEntity o = new WvwObjectiveEntity(obj, mapExpanded);
						newObjectives.Add(o);
					}
				}
				return newObjectives;
			}).Unwrap();
		}
	}
}
