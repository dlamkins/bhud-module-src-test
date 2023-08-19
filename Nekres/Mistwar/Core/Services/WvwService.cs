using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Modules;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Core.Services
{
	internal class WvwService : IDisposable
	{
		private enum WvwMap
		{
			DesertBorderlands = 1099,
			AlpineBorderlandsBlue = 96,
			AlpineBorderlandsGreen = 95,
			EternalBattlegrounds = 38
		}

		private AsyncCache<int, List<WvwObjectiveEntity>> _wvwObjectiveCache;

		private IEnumerable<World> _worlds;

		private WvwMatchTeamList _teams;

		private DateTime _prevApiRequestTime;

		public Guid CurrentGuild { get; private set; }

		public WvwOwner CurrentTeam { get; private set; }

		public DateTime LastChange { get; private set; }

		public string RefreshMessage { get; private set; }

		public bool IsRefreshing => !string.IsNullOrEmpty(RefreshMessage);

		public bool IsLoading
		{
			get
			{
				IEnumerable<World> worlds = _worlds;
				if (worlds == null)
				{
					return true;
				}
				return !worlds.Any();
			}
		}

		public WvwService()
		{
			_prevApiRequestTime = DateTime.MinValue.ToUniversalTime();
			_wvwObjectiveCache = new AsyncCache<int, List<WvwObjectiveEntity>>(RequestObjectives);
			LastChange = DateTime.MinValue.ToUniversalTime();
		}

		public async Task LoadAsync()
		{
			_worlds = (IEnumerable<World>)(await TaskUtil.RetryAsync(() => ((IAllExpandableClient<World>)(object)MistwarModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Worlds()).AllAsync(default(CancellationToken))));
		}

		public async Task Update()
		{
			if (IsLoading || IsRefreshing || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || DateTime.UtcNow.Subtract(_prevApiRequestTime).TotalSeconds < 15.0)
			{
				return;
			}
			_prevApiRequestTime = DateTime.UtcNow;
			RefreshMessage = "Refreshing";
			CurrentGuild = await GetRepresentedGuild();
			int worldId = await GetWorldId();
			if (worldId >= 0)
			{
				int[] wvWMapIds = GetWvWMapIds();
				List<Task> taskList = new List<Task>();
				int[] array = wvWMapIds;
				foreach (int id in array)
				{
					Task<Task> t = new Task<Task>(() => UpdateObjectives(worldId, id));
					taskList.Add(t);
					t.Start();
				}
				if (taskList.Any())
				{
					await Task.WhenAll(taskList.ToArray());
				}
			}
			double mins = Math.Round(LastChange.Subtract(DateTime.UtcNow).TotalMinutes);
			if (mins > 0.0 && mins % 2.0 == 0.0)
			{
				ScreenNotification.ShowNotification($"({((Module)MistwarModule.ModuleInstance).get_Name()}) No changes in the last {mins} minutes.", (NotificationType)1, (Texture2D)null, 4);
			}
			RefreshMessage = string.Empty;
		}

		public async Task<Guid> GetRepresentedGuild()
		{
			if (!MistwarModule.ModuleInstance.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return Guid.Empty;
			}
			Character obj = await TaskUtil.TryAsync(() => ((IBlobClient<Character>)(object)MistwarModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()
				.get_Item(GameService.Gw2Mumble.get_PlayerCharacter().get_Name())).GetAsync(default(CancellationToken)));
			return ((obj != null) ? obj.get_Guild() : null) ?? Guid.Empty;
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
			World obj = _worlds.OrderByDescending((World x) => x.get_Population().get_Value()).FirstOrDefault((World y) => team.Contains(y.get_Id()));
			return ((obj != null) ? obj.get_Name() : null) ?? string.Empty;
		}

		public async Task<int> GetWorldId()
		{
			if (!MistwarModule.ModuleInstance.Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				return -1;
			}
			Account obj = await TaskUtil.TryAsync(() => ((IBlobClient<Account>)(object)MistwarModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)));
			return (obj != null) ? obj.get_World() : (-1);
		}

		public int[] GetWvWMapIds()
		{
			return Enum.GetValues(typeof(WvwMap)).Cast<int>().ToArray();
		}

		public async Task<List<WvwObjectiveEntity>> GetObjectives(int mapId)
		{
			return await _wvwObjectiveCache.GetItem(mapId);
		}

		private async Task UpdateObjectives(int worldId, int mapId)
		{
			List<WvwObjectiveEntity> objEntities = await GetObjectives(mapId);
			WvwMatch match = await TaskUtil.TryAsync(() => ((IBlobClient<WvwMatch>)(object)MistwarModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
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
			IApiV2ObjectList<WvwObjective> wvwObjectives = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<WvwObjective>)(object)MistwarModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Objectives()).AllAsync(default(CancellationToken)));
			if (((IEnumerable<WvwObjective>)wvwObjectives).IsNullOrEmpty())
			{
				return Enumerable.Empty<WvwObjectiveEntity>().ToList();
			}
			Map map = await MistwarModule.ModuleInstance.Resources.GetMap(mapId);
			ContinentFloorRegionMap mapExpanded = await MistwarModule.ModuleInstance.Resources.GetMapExpanded(map, map.get_DefaultFloor());
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

		public void Dispose()
		{
			_wvwObjectiveCache?.Clear();
		}
	}
}
