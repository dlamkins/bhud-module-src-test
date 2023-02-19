using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public abstract class WorldEntity : IEntity, IUpdatable, IRenderable3D
	{
		private float _scale = 1f;

		public float DrawOrder => 1f;

		protected Vector3 Position { get; set; }

		protected float Scale => _scale;

		public float DistanceToPlayer { get; private set; }

		protected BasicEffect RenderEffect { get; private set; }

		public WorldEntity(Vector3 position, float scale)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			_scale = scale;
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_0016: Expected O, but got Unknown
			if (RenderEffect == null)
			{
				BasicEffect val = new BasicEffect(graphicsDevice);
				BasicEffect val2 = val;
				RenderEffect = val;
			}
			InternalRender(graphicsDevice, world, camera);
		}

		protected abstract void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera);

		public virtual void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			DistanceToPlayer = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), Position);
		}

		public abstract bool IsPlayerInside(bool includeZAxis = true);
	}
}
