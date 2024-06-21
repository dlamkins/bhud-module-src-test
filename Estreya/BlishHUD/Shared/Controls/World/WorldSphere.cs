using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldSphere : WorldEntity
	{
		private const int CIRCLE_AMOUNT = 90;

		private const int VERTICES_AMOUNT = 90;

		private readonly Color _color;

		private readonly List<short> _indices = new List<short>();

		private readonly List<VertexPositionNormal> _vertices = new List<VertexPositionNormal>();

		private readonly VertexBuffer vertexBuffer;

		public IndexBuffer indexBuffer;

		private int CurrentVertex => _vertices.Count;

		public WorldSphere(Vector3 position, float radius, Color color, int tessellation = 16)
			: base(position, 1f)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Expected O, but got Unknown
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			if (tessellation < 3)
			{
				throw new ArgumentOutOfRangeException("tessellation");
			}
			int horizontalSegments = tessellation * 2;
			AddVertex(Vector3.get_Down() * radius, Vector3.get_Down());
			Vector3 normal = default(Vector3);
			for (int l = 0; l < tessellation - 1; l++)
			{
				float num = (float)(l + 1) * (float)Math.PI / (float)tessellation - (float)Math.PI / 2f;
				float dy = (float)Math.Sin(num);
				float dxz = (float)Math.Cos(num);
				for (int m = 0; m < horizontalSegments; m++)
				{
					float num2 = (float)m * ((float)Math.PI * 2f) / (float)horizontalSegments;
					float dx = (float)Math.Cos(num2) * dxz;
					float dz = (float)Math.Sin(num2) * dxz;
					((Vector3)(ref normal))._002Ector(dx, dy, dz);
					AddVertex(normal * radius, normal);
				}
			}
			AddVertex(Vector3.get_Up() * radius, Vector3.get_Up());
			for (int k = 0; k < horizontalSegments; k++)
			{
				AddIndex(0);
				AddIndex(1 + (k + 1) % horizontalSegments);
				AddIndex(1 + k);
			}
			for (int j = 0; j < tessellation - 2; j++)
			{
				for (int n = 0; n < horizontalSegments; n++)
				{
					int nextI = j + 1;
					int nextJ = (n + 1) % horizontalSegments;
					AddIndex(1 + j * horizontalSegments + n);
					AddIndex(1 + j * horizontalSegments + nextJ);
					AddIndex(1 + nextI * horizontalSegments + n);
					AddIndex(1 + j * horizontalSegments + nextJ);
					AddIndex(1 + nextI * horizontalSegments + nextJ);
					AddIndex(1 + nextI * horizontalSegments + n);
				}
			}
			for (int i = 0; i < horizontalSegments; i++)
			{
				AddIndex(CurrentVertex - 1);
				AddIndex(CurrentVertex - 2 - (i + 1) % horizontalSegments);
				AddIndex(CurrentVertex - 2 - i);
			}
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				vertexBuffer = new VertexBuffer(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), typeof(VertexPositionNormal), _vertices.Count, (BufferUsage)0);
				vertexBuffer.SetData<VertexPositionNormal>(_vertices.ToArray());
				indexBuffer = new IndexBuffer(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), typeof(short), _indices.Count, (BufferUsage)0);
				indexBuffer.SetData<short>(_indices.ToArray());
				_color = color;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		private void AddVertex(Vector3 position, Vector3 normal)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_vertices.Add(new VertexPositionNormal(position, normal));
		}

		private void AddIndex(int index)
		{
			if (index > 32767)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			_indices.Add((short)index);
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			Matrix modelMatrix = GetMatrix(graphicsDevice, world, camera);
			base.RenderEffect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			base.RenderEffect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			base.RenderEffect.set_World(modelMatrix);
			BasicEffect renderEffect = base.RenderEffect;
			Color color = _color;
			renderEffect.set_DiffuseColor(((Color)(ref color)).ToVector3());
			BasicEffect renderEffect2 = base.RenderEffect;
			color = _color;
			renderEffect2.set_Alpha((float)(int)((Color)(ref color)).get_A() / 255f);
			graphicsDevice.SetVertexBuffer(vertexBuffer);
			graphicsDevice.set_Indices(indexBuffer);
			Enumerator enumerator = ((Effect)base.RenderEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormal>((PrimitiveType)0, _vertices.ToArray(), 0, _vertices.Count, _indices.ToArray(), 0, _indices.Count / 3);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		public override bool IsPlayerInside(bool includeZAxis = true)
		{
			return true;
		}
	}
}
