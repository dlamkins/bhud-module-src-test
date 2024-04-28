using System;
using System.ComponentModel;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Entity
{
	public abstract class PathingEntity : IPathingEntity, IEntity, IUpdatable, IRenderable3D
	{
		protected const float FADEIN_DURATION = 800f;

		protected readonly IPackState _packState;

		private double _lastFadeStart;

		private bool _needsFadeIn = true;

		private bool _wasInactive;

		[Description("A unique identifier used to track the state of certain behaviors between launch sessions.")]
		[Category("Behavior")]
		public Guid Guid { get; set; }

		[Browsable(false)]
		public SafeList<IBehavior> Behaviors { get; } = new SafeList<IBehavior>();


		public PathingCategory Category { get; }

		public abstract float TriggerRange { get; set; }

		[Browsable(false)]
		public bool DebugRender => EditTag.HasValue;

		[Description("Indicates the distance the entity is from the player.")]
		[Category("State Debug")]
		public float DistanceToPlayer { get; set; } = -1f;


		[Browsable(false)]
		public abstract float DrawOrder { get; }

		[Description("Indicates if the entity is currently filtered.")]
		[Category("State Debug")]
		public bool BehaviorFiltered { get; private set; }

		[Browsable(false)]
		public int? EditTag { get; protected set; }

		[Browsable(false)]
		public int MapId { get; set; }

		[Browsable(false)]
		public float AnimatedFadeOpacity => MathHelper.Clamp((float)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _lastFadeStart) / 800f, 0f, 1f);

		protected PathingEntity(IPackState packState, IPointOfInterest pointOfInterest)
		{
			_packState = packState;
			MapId = pointOfInterest.MapId;
			Category = pointOfInterest.ParentPathingCategory ?? _packState.RootCategory;
		}

		public abstract RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity);

		public abstract void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera);

		public virtual void Focus()
		{
		}

		public virtual void Unfocus()
		{
		}

		public virtual void Interact(bool autoTriggered)
		{
		}

		public void FadeIn()
		{
			_needsFadeIn = true;
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!_packState.UserConfiguration.GlobalPathablesEnabled.get_Value())
			{
				return;
			}
			if (_needsFadeIn)
			{
				_lastFadeStart = gameTime.get_TotalGameTime().TotalMilliseconds;
				_needsFadeIn = false;
			}
			if (!_packState.CategoryStates.GetNamespaceInactive(Category.Namespace))
			{
				if (_wasInactive)
				{
					OnCategoryActivated();
				}
				UpdateBehaviors(gameTime);
				_wasInactive = false;
			}
			else
			{
				if (!_wasInactive)
				{
					OnCategoryDeactivated();
				}
				_needsFadeIn = true;
				_wasInactive = true;
			}
		}

		protected virtual void OnCategoryActivated()
		{
		}

		protected virtual void OnCategoryDeactivated()
		{
			Unfocus();
		}

		private void UpdateBehaviors(GameTime gameTime)
		{
			bool filtered = false;
			foreach (IBehavior behavior in Behaviors)
			{
				((IUpdatable)behavior).Update(gameTime);
				ICanFilter filter = behavior as ICanFilter;
				if (filter != null)
				{
					filtered |= filter.IsFiltered();
				}
			}
			BehaviorFiltered = _packState.UserConfiguration.PackAllowMarkersToAutomaticallyHide.get_Value() && filtered;
			HandleBehavior();
		}

		public abstract void HandleBehavior();

		public bool IsFiltered(EntityRenderTarget renderTarget)
		{
			if (!_packState.UserConfiguration.GlobalPathablesEnabled.get_Value())
			{
				return true;
			}
			if (renderTarget == EntityRenderTarget.World)
			{
				if (!_packState.UserConfiguration.PackWorldPathablesEnabled.get_Value())
				{
					return true;
				}
			}
			else if (!_packState.UserConfiguration.MapPathablesEnabled.get_Value())
			{
				return true;
			}
			if (Category != null && _packState.CategoryStates.GetNamespaceInactive(Category.Namespace))
			{
				return true;
			}
			if (BehaviorFiltered)
			{
				return !_packState.UserConfiguration.PackShowHiddenMarkersReducedOpacity.get_Value();
			}
			return false;
		}

		protected Vector2 GetScaledLocation(double x, double y, double scale, double offsetX, double offsetY)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			_packState.MapStates.EventCoordsToMapCoords(x, y, out var mapX, out var mapY);
			Vector2 scaledLocation = default(Vector2);
			((Vector2)(ref scaledLocation))._002Ector((float)((mapX - _packState.CachedMumbleStates.MapCenterX) / scale), (float)((mapY - _packState.CachedMumbleStates.MapCenterY) / scale));
			if (!_packState.CachedMumbleStates.IsMapOpen && _packState.CachedMumbleStates.IsCompassRotationEnabled)
			{
				scaledLocation = Vector2.Transform(scaledLocation, Matrix.CreateRotationZ((float)_packState.CachedMumbleStates.CompassRotation));
			}
			scaledLocation += new Vector2((float)offsetX, (float)offsetY);
			return scaledLocation;
		}

		public virtual void Unload()
		{
			foreach (IBehavior behavior in Behaviors)
			{
				behavior.Unload();
			}
			Behaviors.Clear();
		}
	}
}
