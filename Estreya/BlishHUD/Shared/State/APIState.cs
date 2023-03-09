using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using Humanizer;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.State
{
	public abstract class APIState : ManagedState
	{
		protected readonly Gw2ApiManager _apiManager;

		private readonly EventWaitHandle _eventWaitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);

		private AsyncRef<double> _timeSinceUpdate = 0.0;

		private readonly AsyncLock _loadingLock = new AsyncLock();

		protected new APIStateConfiguration Configuration { get; }

		public bool Loading { get; protected set; }

		public string ProgressText { get; private set; } = string.Empty;


		public event EventHandler Updated;

		public APIState(Gw2ApiManager apiManager, APIStateConfiguration configuration)
			: base(configuration)
		{
			_apiManager = apiManager;
			Configuration = configuration;
		}

		protected sealed override Task Initialize()
		{
			_apiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			return Task.CompletedTask;
		}

		protected virtual Task DoInitialize()
		{
			return Task.CompletedTask;
		}

		private void ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Logger.Info("Received new subtoken with permissions: {0}", new object[1] { e.get_Value().Humanize() });
			if (Configuration.NeededPermissions.Count > 0)
			{
				AsyncHelper.RunSync(Clear);
				_timeSinceUpdate.Value = Configuration.UpdateInterval.TotalMilliseconds;
			}
		}

		protected sealed override void InternalUnload()
		{
			_apiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)ApiManager_SubtokenUpdated);
			DoUnload();
		}

		protected virtual void DoUnload()
		{
		}

		protected sealed override void InternalUpdate(GameTime gameTime)
		{
			if (Configuration.UpdateInterval != Timeout.InfiniteTimeSpan)
			{
				UpdateUtil.UpdateAsync(Load, gameTime, Configuration.UpdateInterval.TotalMilliseconds, _timeSinceUpdate);
			}
			DoUpdate(gameTime);
		}

		protected virtual void DoUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			await LoadFromAPI();
		}

		protected async Task LoadFromAPI(bool resetCompletion = true)
		{
			if (!_loadingLock.IsFree())
			{
				Logger.Warn("Tried to load again while already loading.");
				return;
			}
			using (await _loadingLock.LockAsync())
			{
				if (resetCompletion)
				{
					_eventWaitHandle.Reset();
				}
				Loading = true;
				try
				{
					IProgress<string> progress = new Progress<string>(delegate(string newProgress)
					{
						ProgressText = newProgress;
					});
					progress.Report("Loading " + GetType().Name);
					await FetchFromAPI(_apiManager, progress);
					this.Updated?.Invoke(this, EventArgs.Empty);
					ProgressText = string.Empty;
				}
				finally
				{
					Loading = false;
					SignalCompletion();
				}
			}
		}

		protected abstract Task FetchFromAPI(Gw2ApiManager apiManager, IProgress<string> progress);

		public void SignalCompletion()
		{
			_eventWaitHandle.Set();
		}

		public Task<bool> WaitForCompletion()
		{
			return WaitForCompletion(Timeout.InfiniteTimeSpan);
		}

		public async Task<bool> WaitForCompletion(TimeSpan timeout)
		{
			return await _eventWaitHandle.WaitOneAsync(timeout, _cancellationTokenSource.Token);
		}
	}
	public abstract class APIState<T> : APIState
	{
		protected readonly AsyncLock _apiObjectListLock = new AsyncLock();

		protected List<T> APIObjectList { get; } = new List<T>();


		protected event EventHandler<T> APIObjectAdded;

		protected event EventHandler<T> APIObjectRemoved;

		public APIState(Gw2ApiManager apiManager, APIStateConfiguration configuration)
			: base(apiManager, configuration)
		{
		}

		protected sealed override async Task Clear()
		{
			using (await _apiObjectListLock.LockAsync())
			{
				APIObjectList.Clear();
			}
			await DoClear();
		}

		protected virtual Task DoClear()
		{
			return Task.CompletedTask;
		}

		protected override async Task FetchFromAPI(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			Logger.Info("Check for api objects.");
			if (apiManager == null)
			{
				Logger.Warn("API Manager is null");
				return;
			}
			if (base.Configuration.NeededPermissions.Count > 0 && !apiManager.HasPermission((TokenPermission)1))
			{
				Logger.Debug("No token yet.");
				return;
			}
			try
			{
				using (await _apiObjectListLock.LockAsync())
				{
					List<T> oldAPIObjectList = APIObjectList.ToArray().ToList();
					APIObjectList.Clear();
					Logger.Debug("Got {0} api objects from previous fetch.", new object[1] { oldAPIObjectList.Count });
					if (!_apiManager.HasPermissions((IEnumerable<TokenPermission>)base.Configuration.NeededPermissions))
					{
						Logger.Warn("API Manager does not have needed permissions: {0}", new object[1] { base.Configuration.NeededPermissions.Humanize() });
						return;
					}
					List<T> apiObjects = await Fetch(apiManager, progress).ConfigureAwait(continueOnCapturedContext: false);
					Logger.Debug("API returned {0} objects.", new object[1] { apiObjects.Count });
					APIObjectList.AddRange(apiObjects);
					progress.Report("Check what api objects are new..");
					foreach (T apiObject2 in apiObjects)
					{
						if (!oldAPIObjectList.Any((T oldApiObject) => oldApiObject.GetHashCode() == apiObject2.GetHashCode()))
						{
							if (apiObjects.Count <= 25)
							{
								Logger.Debug($"API Object added: {apiObject2}");
							}
							try
							{
								this.APIObjectAdded?.Invoke(this, apiObject2);
							}
							catch (Exception ex3)
							{
								Logger.Error(ex3, "Error handling api object added event:");
							}
						}
					}
					progress.Report("Check what api objects are removed..");
					for (int i = oldAPIObjectList.Count - 1; i >= 0; i--)
					{
						T oldApiObject2 = oldAPIObjectList[i];
						if (!apiObjects.Any((T apiObject) => apiObject.GetHashCode() == oldApiObject2.GetHashCode()))
						{
							Logger.Debug($"API Object disappeared from the api: {oldApiObject2}");
							oldAPIObjectList.Remove(oldApiObject2);
							try
							{
								this.APIObjectRemoved?.Invoke(this, oldApiObject2);
							}
							catch (Exception ex2)
							{
								Logger.Error(ex2, "Error handling api object removed event:");
							}
						}
					}
				}
				Logger.Info("Check for api objects finished.");
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

		protected abstract Task<List<T>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress);
	}
}
