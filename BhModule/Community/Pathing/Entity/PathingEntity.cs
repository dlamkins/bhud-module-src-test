using System.Collections.Generic;
using System.ComponentModel;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Gw2Sharp.Models;
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

		[Browsable(false)]
		public IList<IBehavior> Behaviors { get; } = new SafeList<IBehavior>();


		public PathingCategory Category { get; }

		public abstract float TriggerRange { get; set; }

		[Browsable(false)]
		public bool DebugRender => false;

		[Description("Indicates the distance the entity is from the player.")]
		[Category("State Debug")]
		public float DistanceToPlayer { get; set; } = -1f;


		[Browsable(false)]
		public abstract float DrawOrder { get; }

		[Description("Indicates if the entity is currently filtered.")]
		[Category("State Debug")]
		public bool BehaviorFiltered { get; private set; }

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

		public abstract RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, (double X, double Y) offsets, double scale, float opacity);

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
			if (_packState.CategoryStates.GetNamespaceInactive(Category.Namespace))
			{
				return true;
			}
			return BehaviorFiltered;
		}

		protected Vector2 GetScaledLocation(double x, double y, double scale, (double X, double Y) offsets)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			(double X, double Y) tuple = _packState.MapStates.EventCoordsToMapCoords(x, y, MapId);
			double mapX = tuple.X;
			double mapY = tuple.Y;
			Coordinates2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
			float num = (float)((mapX - ((Coordinates2)(ref mapCenter)).get_X()) / scale);
			mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
			Vector2 scaledLocation = default(Vector2);
			((Vector2)(ref scaledLocation))._002Ector(num, (float)((mapY - ((Coordinates2)(ref mapCenter)).get_Y()) / scale));
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled())
			{
				scaledLocation = Vector2.Transform(scaledLocation, Matrix.CreateRotationZ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()));
			}
			scaledLocation += new Vector2((float)offsets.X, (float)offsets.Y);
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
