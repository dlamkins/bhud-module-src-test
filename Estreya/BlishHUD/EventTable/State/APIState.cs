using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using Humanizer;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.State
{
	public abstract class APIState<T> : ManagedState
	{
		private readonly Logger Logger;

		private readonly Gw2ApiManager _apiManager;

		private readonly List<TokenPermission> _neededPermissions = new List<TokenPermission>();

		private TimeSpan _updateInterval;

		private double _timeSinceUpdate;

		private Task _fetchTask;

		protected readonly AsyncLock _listLock = new AsyncLock();

		protected List<T> APIObjectList { get; } = new List<T>();


		public Func<Gw2ApiManager, Task<List<T>>> FetchAction { get; init; }

		public event EventHandler<T> APIObjectAdded;

		public event EventHandler<T> APIObjectRemoved;

		public APIState(Gw2ApiManager apiManager, List<TokenPermission> neededPermissions = null, TimeSpan? updateInterval = null, bool awaitLoad = true, int saveInterval = -1)
			: base(awaitLoad, saveInterval)
		{
			Logger = Logger.GetLogger(GetType());
			_apiManager = apiManager;
			if (neededPermissions != null)
			{
				_neededPermissions.AddRange(neededPermissions);
			}
			_updateInterval = updateInterval ?? TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0));
		}

		private void ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run((Func<Task>)base.Reload);
		}

		public sealed override Task Clear()
		{
			using (_listLock.Lock())
			{
				APIObjectList.Clear();
			}
			return DoClear();
		}

		public abstract Task DoClear();

		protected sealed override async Task InternalReload()
		{
			await Clear();
			await Load();
		}

		protected sealed override Task Initialize()
		{
			_apiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_apiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			AsyncHelper.RunSync(Clear);
			DoUnload();
		}

		protected abstract void DoUnload();

		protected override void InternalUpdate(GameTime gameTime)
		{
			if (_updateInterval != Timeout.InfiniteTimeSpan)
			{
				UpdateUtil.UpdateAsync(FetchFromAPI, gameTime, _updateInterval.TotalMilliseconds, ref _timeSinceUpdate);
			}
		}

		private Task FetchFromAPI(GameTime gameTime)
		{
			_fetchTask = Task.Run(async delegate
			{
				Logger.Info("Check for api objects.");
				if (_apiManager == null)
				{
					Logger.Warn("API Manager is null");
				}
				else if (FetchAction == null)
				{
					Logger.Warn("No fetchaction definied.");
				}
				else
				{
					try
					{
						List<T> oldAPIObjectList;
						using (await _listLock.LockAsync())
						{
							oldAPIObjectList = APIObjectList.Copy();
							APIObjectList.Clear();
						}
						Logger.Debug("Got {0} api objects from previous fetch: {1}", new object[2]
						{
							oldAPIObjectList.Count,
							JsonConvert.SerializeObject((object)oldAPIObjectList)
						});
						if (!_apiManager.HasPermissions((IEnumerable<TokenPermission>)_neededPermissions))
						{
							Logger.Warn("API Manager does not have needed permissions: {0}", new object[1] { _neededPermissions.Humanize() });
						}
						else
						{
							List<T> apiObjects = await FetchAction(_apiManager);
							Logger.Debug("API returned objects: {0}", new object[1] { JsonConvert.SerializeObject((object)apiObjects) });
							using (await _listLock.LockAsync())
							{
								APIObjectList.AddRange(apiObjects);
							}
							foreach (T apiObject in apiObjects)
							{
								if (!oldAPIObjectList.Contains(apiObject))
								{
									Logger.Info($"API Object added: {apiObject}");
									try
									{
										this.APIObjectAdded?.Invoke(this, apiObject);
									}
									catch (Exception ex3)
									{
										Logger.Error(ex3, "Error handling api object added event:");
									}
								}
							}
							for (int i = oldAPIObjectList.Count - 1; i >= 0; i--)
							{
								T oldApiObject = oldAPIObjectList[i];
								if (!apiObjects.Contains(oldApiObject))
								{
									Logger.Info($"API Object disappeared from the api: {oldApiObject}");
									oldAPIObjectList.Remove(oldApiObject);
									try
									{
										this.APIObjectRemoved?.Invoke(this, oldApiObject);
									}
									catch (Exception ex2)
									{
										Logger.Error(ex2, "Error handling api object removed event:");
									}
								}
							}
						}
					}
					catch (MissingScopesException val)
					{
						MissingScopesException msex = val;
						Logger.Warn((Exception)(object)msex, "Could not update api objects due to missing scopes:");
					}
					catch (InvalidAccessTokenException val2)
					{
						InvalidAccessTokenException iatex = val2;
						Logger.Warn((Exception)(object)iatex, "Could not update api objects due to invalid access token:");
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Error updating api objects:");
					}
				}
			});
			return _fetchTask;
		}

		public async Task WaitAsync()
		{
			if (_fetchTask == null)
			{
				Logger.Debug("{0}: First fetch did not start yet.", new object[1] { GetType().Name });
				int waitMs = 100;
				int counter = 0;
				int maxCounter = 300;
				while (_fetchTask == null)
				{
					await Task.Delay(waitMs);
					counter++;
					if (counter > maxCounter)
					{
						Logger.Debug("{0}: First fetch did not complete after {1} tries. ({2} minutes)", new object[3]
						{
							GetType().Name,
							counter,
							counter * waitMs / 1000 / 60
						});
						return;
					}
				}
			}
			await _fetchTask;
		}

		protected override async Task Load()
		{
			await FetchFromAPI(null);
		}
	}
}
