using System.Collections.Generic;
using BhModule.Community.Pathing.Behavior;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Entity
{
	public interface IPathingEntity : IEntity, IUpdatable, IRenderable3D
	{
		IList<IBehavior> Behaviors { get; }

		float TriggerRange { get; set; }

		float DistanceToPlayer { get; set; }

		int MapId { get; }

		PathingCategory Category { get; }

		bool DebugRender { get; }

		float AnimatedFadeOpacity { get; }

		RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, (double X, double Y) offsets, double scale, float opacity);

		void Focus();

		void Unfocus();

		void Interact(bool autoTriggered);

		void FadeIn();

		void Unload();
	}
}
