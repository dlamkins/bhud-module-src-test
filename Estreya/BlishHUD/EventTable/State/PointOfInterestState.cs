using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.State
{
	public class PointOfInterestState : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<PointOfInterestState>();

		private readonly Gw2ApiManager _apiManager;

		private AsyncLock _pointOfInterestsLock = new AsyncLock();

		public bool Loading { get; private set; }

		public List<ContinentFloorRegionMapPoi> PointOfInterests { get; } = new List<ContinentFloorRegionMapPoi>();


		public PointOfInterestState(Gw2ApiManager apiManager)
			: base(awaitLoad: false)
		{
			_apiManager = apiManager;
		}

		public override Task Clear()
		{
			using (_pointOfInterestsLock.Lock())
			{
				PointOfInterests.Clear();
			}
			return Task.CompletedTask;
		}

		public override async Task InternalReload()
		{
			await Clear();
			await Load();
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			AsyncHelper.RunSync(Clear);
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			await LoadPointOfInterests();
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		public ContinentFloorRegionMapPoi GetPointOfInterest(string chatCode)
		{
			using (_pointOfInterestsLock.Lock())
			{
				IEnumerable<ContinentFloorRegionMapPoi> foundPointOfInterests = PointOfInterests.Where((ContinentFloorRegionMapPoi wp) => wp.get_ChatLink() == chatCode);
				return foundPointOfInterests.Any() ? foundPointOfInterests.First() : null;
			}
		}

		private async Task LoadPointOfInterests()
		{
			using (await _pointOfInterestsLock.LockAsync())
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Loading = true;
					foreach (int floorId in (IEnumerable<int>)(await ((IBulkExpandableClient<ContinentFloor, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Continents()
						.get_Item(1)
						.get_Floors()).IdsAsync(default(CancellationToken))))
					{
						foreach (ContinentFloorRegion value in (await ((IBlobClient<ContinentFloor>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Continents()
							.get_Item(1)
							.get_Floors()
							.get_Item(floorId)).GetAsync(default(CancellationToken))).get_Regions().Values)
						{
							foreach (ContinentFloorRegionMap value2 in value.get_Maps().Values)
							{
								foreach (ContinentFloorRegionMapPoi waypoint in value2.get_PointsOfInterest().Values.Where((ContinentFloorRegionMapPoi poi) => poi.get_Name() != null))
								{
									PointOfInterests.Add(waypoint);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Could not load point of interests:");
				}
				finally
				{
					Loading = false;
				}
			}
		}
	}
}
