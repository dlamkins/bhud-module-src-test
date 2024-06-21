using System;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public abstract class WorldEntity : IEntity, IUpdatable, IRenderable3D
	{
		protected Vector3 Position { get; set; }

		public float ScaleX { get; set; } = 1f;


		public float ScaleY { get; set; } = 1f;


		public float ScaleZ { get; set; } = 1f;


		public float RotationX { get; set; }

		public float RotationY { get; set; }

		public float RotationZ { get; set; }

		public float DistanceToPlayer { get; private set; }

		public Func<WorldEntity, bool> RenderCondition { get; set; }

		protected BasicEffect RenderEffect { get; private set; }

		public float DrawOrder => 1f;

		public WorldEntity(Vector3 position, float scale)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			ScaleX = scale;
			ScaleY = scale;
			ScaleZ = scale;
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_002d: Expected O, but got Unknown
			if (RenderCondition == null || RenderCondition(this))
			{
				if (RenderEffect == null)
				{
					BasicEffect val = new BasicEffect(graphicsDevice);
					BasicEffect val2 = val;
					RenderEffect = val;
				}
				RenderEffect.set_VertexColorEnabled(true);
				InternalRender(graphicsDevice, world, camera);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			DistanceToPlayer = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), Position);
		}

		protected virtual Matrix GetMatrix(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			return Matrix.CreateScale(ScaleX, ScaleY, ScaleZ) * Matrix.CreateRotationX(MathHelper.ToRadians(RotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(RotationY)) * Matrix.CreateRotationZ(MathHelper.ToRadians(RotationZ)) * Matrix.CreateTranslation(Position);
		}

		protected abstract void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera);

		public abstract bool IsPlayerInside(bool includeZAxis = true);
	}
}
