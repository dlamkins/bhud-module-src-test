using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldPolygone : WorldEntity
	{
		private VertexPositionColor[] _vert;

		private readonly Color _color;

		private readonly Func<WorldEntity, bool> _renderCondition;

		public Vector3[] Points { get; private set; }

		public WorldPolygone(Vector3 position, Vector3[] points, Color color, Func<WorldEntity, bool> renderCondition = null)
			: base(position, 1f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (points.Length < 2 || points.Length % 2 != 0)
			{
				throw new ArgumentOutOfRangeException("points");
			}
			Points = points;
			_color = color;
			_renderCondition = renderCondition;
			_vert = BuildVertices();
		}

		public WorldPolygone(Vector3 position, Vector3[] points)
			: this(position, points, Color.get_White())
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		private VertexPositionColor[] BuildVertices()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			VertexPositionColor[] verts = (VertexPositionColor[])(object)new VertexPositionColor[Points.Length];
			Vector3 curPoint = Points[0];
			for (int i = 0; i < Points.Length - 1; i++)
			{
				Vector3 nextPoint = Points[i + 1];
				verts[i] = new VertexPositionColor(curPoint, _color);
				verts[i + 1] = new VertexPositionColor(nextPoint, _color);
				curPoint = nextPoint;
			}
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				new VertexBuffer(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), VertexPositionColor.VertexDeclaration, verts.Length, (BufferUsage)1).SetData<VertexPositionColor>(verts);
				return verts;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			if (_renderCondition != null && !_renderCondition(this))
			{
				return;
			}
			Matrix modelMatrix = Matrix.CreateScale(base.Scale);
			Vector3 position = base.Position + new Vector3(0f, 0f, 0f);
			modelMatrix *= Matrix.CreateTranslation(position);
			base.RenderEffect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			base.RenderEffect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			base.RenderEffect.set_World(modelMatrix);
			Enumerator enumerator = ((Effect)base.RenderEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, _vert, 0, _vert.Length / 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		public Vector3[] GetAbsolutePoints()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> newPoints = new List<Vector3>();
			Vector3[] points = Points;
			foreach (Vector3 point in points)
			{
				newPoints.Add(base.Position + point);
			}
			return newPoints.ToArray();
		}

		public override bool IsPlayerInside(bool includeZAxis = true)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			Vector3[] points = GetAbsolutePoints();
			float maxZ = points.Max((Vector3 p) => p.Z);
			float minZ = points.Min((Vector3 p) => p.Z);
			if (includeZAxis && (playerPosition.Z > maxZ || playerPosition.Z < minZ))
			{
				return false;
			}
			bool result = false;
			int j = points.Length - 1;
			for (int i = 0; i < points.Length; i++)
			{
				if (((points[i].Y < playerPosition.Y && points[j].Y >= playerPosition.Y) || (points[j].Y < playerPosition.Y && points[i].Y >= playerPosition.Y)) && points[i].X + (playerPosition.Y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X) < playerPosition.X)
				{
					result = !result;
				}
				j = i;
			}
			return result;
		}
	}
}
