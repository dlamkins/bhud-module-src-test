using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Services
{
	internal class WvwService
	{
		private enum WvwMap
		{
			DesertBorderlands = 1099,
			AlpineBorderlandsBlue = 96,
			AlpineBorderlandsGreen = 95,
			EternalBattlegrounds = 38
		}

		private Gw2ApiManager _api;

		private AsyncCache<int, List<WvwObjectiveEntity>> _wvwObjectiveCache;

		private IEnumerable<World> _worlds;

		private WvwMatchTeamList _teams;

		private DateTime _prevApiRequestTime;

		public Guid CurrentGuild { get; private set; }

		public WvwOwner CurrentTeam { get; private set; }

		public DateTime LastChange { get; private set; }

		public bool IsLoading => !string.IsNullOrEmpty(LoadingMessage);

		public string LoadingMessage { get; private set; }

		public WvwService(Gw2ApiManager api)
		{
			_prevApiRequestTime = DateTime.MinValue.ToUniversalTime();
			_api = api;
			_wvwObjectiveCache = new AsyncCache<int, List<WvwObjectiveEntity>>(RequestObjectives);
			LastChange = DateTime.MinValue.ToUniversalTime();
		}

		public async Task LoadAsync()
		{
			_worlds = (IEnumerable<World>)(await TaskUtil.RetryAsync(() => ((IAllExpandableClient<World>)(object)_api.get_Gw2ApiClient().get_V2().get_Worlds()).AllAsync(default(CancellationToken))));
		}

		public async Task Update()
		{
			if (!GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 15.0)
			{
				return;
			}
			_prevApiRequestTime = DateTime.UtcNow;
			LoadingMessage = "Refreshing";
			CurrentGuild = await GetRepresentedGuild();
			int worldId = await GetWorldId();
			if (worldId < 0)
			{
				LoadingMessage = string.Empty;
				return;
			}
			List<Task> taskList = new List<Task>();
			int[] array = await GetWvWMapIds(worldId);
			foreach (int id in array)
			{
				Task<Task> t = new Task<Task>(() => UpdateObjectives(worldId, id));
				taskList.Add(t);
				t.Start();
			}
			Task.WaitAll(taskList.ToArray());
			double mins = Math.Round(LastChange.Subtract(DateTime.UtcNow).TotalMinutes);
			if (mins > 0.0 && mins % 2.0 == 0.0)
			{
				ScreenNotification.ShowNotification($"({((Module)MistwarModule.ModuleInstance).get_Name()}) No changes in the last {mins} minutes.", (NotificationType)1, (Texture2D)null, 4);
			}
			LoadingMessage = string.Empty;
		}

		public async Task<Guid> GetRepresentedGuild()
		{
			if (!_api.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return Guid.Empty;
			}
			Character character = await TaskUtil.RetryAsync(() => ((IBlobClient<Character>)(object)_api.get_Gw2ApiClient().get_V2().get_Characters()
				.get_Item(GameService.Gw2Mumble.get_PlayerCharacter().get_Name())).GetAsync(default(CancellationToken)));
			if (character == null)
			{
				return Guid.Empty;
			}
			return character.get_Guild() ?? Guid.Empty;
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
			Account obj = await TaskUtil.RetryAsync(() => ((IBlobClient<Account>)(object)_api.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)));
			return (obj != null) ? obj.get_World() : (-1);
		}

		public async Task<int[]> GetWvWMapIds(int worldId)
		{
			WvwMatch matches = await TaskUtil.RetryAsync(() => ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)));
			if (matches == null)
			{
				return null;
			}
			return (from m in matches.get_Maps()
				select m.get_Id()).ToArray();
		}

		public async Task<List<WvwObjectiveEntity>> GetObjectives(int mapId)
		{
			return await _wvwObjectiveCache.GetItem(mapId);
		}

		private async Task UpdateObjectives(int worldId, int mapId)
		{
			List<WvwObjectiveEntity> objEntities = await GetObjectives(mapId);
			WvwMatch match = await TaskUtil.RetryAsync(() => ((IBlobClient<WvwMatch>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)));
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
				WvwMatchMapObjective obj = objectives?.FirstOrDefault((WvwMatchMapObjective v) => v.get_Id().Equals(objEntity.Id, StringComparison.InvariantCultureIgnoreCase));
				if (obj != null)
				{
					objEntity.LastFlipped = obj.get_LastFlipped() ?? DateTime.MinValue;
					objEntity.Owner = obj.get_Owner().get_Value();
					objEntity.ClaimedBy = obj.get_ClaimedBy() ?? Guid.Empty;
					objEntity.GuildUpgrades = obj.get_GuildUpgrades();
					objEntity.YaksDelivered = obj.get_YaksDelivered().GetValueOrDefault();
					if (objEntity.LastModified > LastChange)
					{
						LastChange = objEntity.LastModified;
					}
				}
			}
		}

		private async Task<List<WvwObjectiveEntity>> RequestObjectives(int mapId)
		{
			IApiV2ObjectList<WvwObjective> wvwObjectives = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<WvwObjective>)(object)_api.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Objectives()).AllAsync(default(CancellationToken)));
			if (((IEnumerable<WvwObjective>)wvwObjectives).IsNullOrEmpty())
			{
				return Enumerable.Empty<WvwObjectiveEntity>().ToList();
			}
			Map obj2 = await MapUtil.GetMap(mapId);
			ContinentFloorRegionMap mapExpanded = await MapUtil.GetMapExpanded(obj2, obj2.get_DefaultFloor());
			if (mapExpanded == null)
			{
				return Enumerable.Empty<WvwObjectiveEntity>().ToList();
			}
			List<WvwObjectiveEntity> newObjectives = new List<WvwObjectiveEntity>();
			foreach (ContinentFloorRegionMapSector sector in mapExpanded.get_Sectors().Values)
			{
				WvwObjective obj = ((IEnumerable<WvwObjective>)wvwObjectives).FirstOrDefault((WvwObjective x) => x.get_SectorId() == sector.get_Id());
				if (obj != null)
				{
					WvwObjectiveEntity o = new WvwObjectiveEntity(obj, mapExpanded);
					newObjectives.Add(o);
				}
			}
			return newObjectives;
		}
	}
}
