using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Content;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Entity.Effects;
using BhModule.Community.Pathing.State;
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

		public PathingModule Module { get; }

		[Obsolete("Use 'ModuleSettings' on the 'Module' property instead.")]
		public ModuleSettings UserConfiguration => Module.Settings;

		public int CurrentMapId { get; set; }

		public PathingCategory RootCategory { get; private set; }

		public MarkerEffect SharedMarkerEffect { get; private set; }

		public TrailEffect SharedTrailEffect { get; private set; }

		public BehaviorStates BehaviorStates { get; private set; }

		public AchievementStates AchievementStates { get; private set; }

		public RaidStates RaidStates { get; private set; }

		public CategoryStates CategoryStates { get; private set; }

		public MapStates MapStates { get; private set; }

		public UserResourceStates UserResourceStates { get; private set; }

		public UiStates UiStates { get; private set; }

		public EditorStates EditorStates { get; private set; }

		public CachedMumbleStates CachedMumbleStates { get; private set; }

		public SafeList<IPathingEntity> Entities { get; private set; } = new SafeList<IPathingEntity>();


		public SharedPackState(PathingModule module)
		{
			Module = module;
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
			RaidStates raidStates2 = (RaidStates = new RaidStates(this));
			ManagedState managedState3 = await raidStates2.Start();
			BehaviorStates behaviorStates2 = (BehaviorStates = new BehaviorStates(this));
			ManagedState managedState4 = await behaviorStates2.Start();
			MapStates mapStates2 = (MapStates = new MapStates(this));
			ManagedState managedState5 = await mapStates2.Start();
			UserResourceStates userResourceStates2 = (UserResourceStates = new UserResourceStates(this));
			ManagedState managedState6 = await userResourceStates2.Start();
			UiStates uiStates2 = (UiStates = new UiStates(this));
			ManagedState managedState7 = await uiStates2.Start();
			EditorStates editorStates2 = (EditorStates = new EditorStates(this));
			ManagedState managedState8 = await editorStates2.Start();
			CachedMumbleStates cachedMumbleStates2 = (CachedMumbleStates = new CachedMumbleStates(this));
			ManagedState managedState9 = await cachedMumbleStates2.Start();
			_managedStates = new ManagedState[9] { managedState, managedState2, managedState3, managedState4, managedState5, managedState6, managedState7, managedState8, managedState9 };
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
			PointOfInterestType type = pointOfInterest.Type;
			var (texture, shouldSample) = type switch
			{
				PointOfInterestType.Marker => (pointOfInterest.GetAggregatedAttributeValue("iconfile"), false), 
				PointOfInterestType.Trail => (pointOfInterest.GetAggregatedAttributeValue("texture"), true), 
				_ => throw new SwitchExpressionException((object)type), 
			};
			if (texture != null)
			{
				await TextureResourceManager.GetTextureResourceManager(pointOfInterest.ResourceManager).PreloadTexture(texture, shouldSample);
			}
		}

		public IPathingEntity InitPointOfInterest(PointOfInterest pointOfInterest)
		{
			IPathingEntity entity = BuildEntity(pointOfInterest);
			Entities.Add(entity);
			GameService.Graphics.get_World().AddEntity((IEntity)entity);
			return entity;
		}

		public void RemovePathingEntity(IPathingEntity entity)
		{
			Entities.Remove(entity);
			GameService.Graphics.get_World().RemoveEntity((IEntity)entity);
		}

		private async Task InitPointsOfInterest(IEnumerable<PointOfInterest> pointsOfInterest)
		{
			PointOfInterest[] pois = pointsOfInterest.ToArray();
			PointOfInterest[] array = pois;
			foreach (PointOfInterest poi in array)
			{
				await PreloadTextures(poi);
				InitPointOfInterest(poi);
			}
			await Task.CompletedTask;
		}

		private void InitShaders()
		{
			SharedMarkerEffect = new MarkerEffect(Module.ContentsManager.GetEffect("hlsl\\marker.mgfx"));
			SharedTrailEffect = new TrailEffect(Module.ContentsManager.GetEffect("hlsl\\trail.mgfx"));
			SharedMarkerEffect.FadeTexture = Module.ContentsManager.GetTexture("png\\42975.png");
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
