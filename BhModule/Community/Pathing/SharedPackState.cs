using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Content;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Entity.Effects;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Common.Gw2;
using Blish_HUD.Entities;
using Microsoft.Xna.Framework;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing
{
	public class SharedPackState : IRootPackState, IPackState, IUpdatable
	{
		private ManagedState[] _managedStates;

		private bool _initialized;

		private bool _loadingPack;

		private bool _loadingState;

		public ModuleSettings UserConfiguration { get; }

		public int CurrentMapId { get; set; }

		public PathingCategory RootCategory { get; private set; }

		public MarkerEffect SharedMarkerEffect { get; private set; }

		public TrailEffect SharedTrailEffect { get; private set; }

		public BehaviorStates BehaviorStates { get; private set; }

		public AchievementStates AchievementStates { get; private set; }

		public CategoryStates CategoryStates { get; private set; }

		public MapStates MapStates { get; private set; }

		public UserResourceStates UserResourceStates { get; private set; }

		public UiStates UiStates { get; private set; }

		public EditorStates EditorStates { get; private set; }

		public SafeList<IPathingEntity> Entities { get; private set; } = new SafeList<IPathingEntity>();


		public SharedPackState(ModuleSettings moduleSettings)
		{
			UserConfiguration = moduleSettings;
			InitShaders();
			KeyBindings.Interact.add_Activated((EventHandler<EventArgs>)OnInteractPressed);
		}

		public async Task Load()
		{
			await InitStates();
		}

		private async Task ReloadStates()
		{
			if (_initialized && !_loadingState)
			{
				_loadingState = true;
				await Task.WhenAll(_managedStates.Select((ManagedState state) => state.Reload()));
				_loadingState = false;
			}
		}

		private async Task InitStates()
		{
			CategoryStates categoryStates2 = (CategoryStates = new CategoryStates(this));
			ManagedState managedState = await categoryStates2.Start();
			AchievementStates achievementStates2 = (AchievementStates = new AchievementStates(this));
			ManagedState managedState2 = await achievementStates2.Start();
			BehaviorStates behaviorStates2 = (BehaviorStates = new BehaviorStates(this));
			ManagedState managedState3 = await behaviorStates2.Start();
			MapStates mapStates2 = (MapStates = new MapStates(this));
			ManagedState managedState4 = await mapStates2.Start();
			UserResourceStates userResourceStates2 = (UserResourceStates = new UserResourceStates(this));
			ManagedState managedState5 = await userResourceStates2.Start();
			UiStates uiStates2 = (UiStates = new UiStates(this));
			ManagedState managedState6 = await uiStates2.Start();
			EditorStates editorStates2 = (EditorStates = new EditorStates(this));
			ManagedState managedState7 = await editorStates2.Start();
			_managedStates = new ManagedState[7] { managedState, managedState2, managedState3, managedState4, managedState5, managedState6, managedState7 };
			_initialized = true;
		}

		private IPathingEntity BuildEntity(IPointOfInterest pointOfInterest)
		{
			return pointOfInterest.Type switch
			{
				PointOfInterestType.Marker => new StandardMarker(this, pointOfInterest), 
				PointOfInterestType.Trail => new StandardTrail(this, pointOfInterest as ITrail), 
				PointOfInterestType.Route => throw new NotImplementedException("Routes have not been implemented."), 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}

		public async Task LoadPackCollection(IPackCollection collection)
		{
			while (_loadingPack)
			{
				await Task.Delay(100);
			}
			_loadingPack = true;
			RootCategory = collection.Categories;
			await ReloadStates();
			await InitPointsOfInterest(collection.PointsOfInterest);
			_loadingPack = false;
		}

		private static async Task PreloadTextures(IPointOfInterest pointOfInterest)
		{
			string texture = pointOfInterest.Type switch
			{
				PointOfInterestType.Marker => pointOfInterest.GetAggregatedAttributeValue("iconfile"), 
				PointOfInterestType.Trail => pointOfInterest.GetAggregatedAttributeValue("texture"), 
				_ => throw new InvalidOperationException(), 
			};
			if (texture != null)
			{
				await TextureResourceManager.GetTextureResourceManager(pointOfInterest.ResourceManager).PreloadTexture(texture);
			}
		}

		private async Task InitPointsOfInterest(IList<PointOfInterest> pois)
		{
			ConcurrentBag<IPathingEntity> poiBag = new ConcurrentBag<IPathingEntity>();
			await pois.AsParallel().ParallelForEachAsync(PreloadTextures, Environment.ProcessorCount);
			pois.AsParallel().Select(BuildEntity).ForAll(poiBag.Add);
			Entities.AddRange(poiBag);
			GameService.Graphics.get_World().AddEntities((IEnumerable<IEntity>)poiBag);
			await Task.CompletedTask;
		}

		private void InitShaders()
		{
			SharedMarkerEffect = new MarkerEffect(PathingModule.Instance.ContentsManager.GetEffect("hlsl\\marker.mgfx"));
			SharedTrailEffect = new TrailEffect(PathingModule.Instance.ContentsManager.GetEffect("hlsl\\trail.mgfx"));
			SharedMarkerEffect.FadeTexture = PathingModule.Instance.ContentsManager.GetTexture("png\\42975.png");
		}

		private void OnInteractPressed(object sender, EventArgs e)
		{
			foreach (IPathingEntity entity in Entities)
			{
				StandardMarker marker = entity as StandardMarker;
				if (marker != null && marker.Focused)
				{
					marker.Interact(autoTriggered: false);
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			if (_managedStates != null)
			{
				ManagedState[] managedStates = _managedStates;
				for (int i = 0; i < managedStates.Length; i++)
				{
					managedStates[i].Update(gameTime);
				}
			}
		}

		public async Task Unload()
		{
			foreach (IPathingEntity entity in Entities)
			{
				entity.Unload();
			}
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)Entities);
			Entities = new SafeList<IPathingEntity>();
			RootCategory = null;
			await TextureResourceManager.UnloadAsync();
		}
	}
}
