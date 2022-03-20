using System;
using System.Collections.Generic;
using BhModule.Community.Pathing.Entity;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Editor.Entity
{
	public class TranslateTool : IEntity, IUpdatable, IRenderable3D
	{
		private IPathingEntity _target;

		private TranslateAxisHandle _xAxis;

		private TranslateAxisHandle _yAxis;

		private TranslateAxisHandle _zAxis;

		public float DrawOrder => 0f;

		public Vector3 ToolOrigin { get; set; }

		public TranslateTool(StandardMarker target)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			_target = target;
			BuildAxis(ToolOrigin = target.Position);
		}

		public TranslateTool(Vector3 toolOrigin)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			BuildAxis(ToolOrigin = toolOrigin);
		}

		private void BuildAxis(Vector3 origin)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			_xAxis = new TranslateAxisHandle(Color.get_Blue(), Matrix.CreateRotationX((float)Math.PI / 2f))
			{
				Origin = origin
			};
			_yAxis = new TranslateAxisHandle(Color.get_Red(), Matrix.CreateRotationY((float)Math.PI / 2f))
			{
				Origin = origin
			};
			_zAxis = new TranslateAxisHandle(Color.get_LightGreen(), Matrix.get_Identity())
			{
				Origin = origin
			};
			GameService.Graphics.get_World().AddEntities((IEnumerable<IEntity>)(object)new TranslateAxisHandle[3] { _xAxis, _yAxis, _zAxis });
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
		}
	}
}
