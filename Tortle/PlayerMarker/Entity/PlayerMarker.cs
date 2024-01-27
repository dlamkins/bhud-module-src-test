using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tortle.PlayerMarker.Models;

namespace Tortle.PlayerMarker.Entity
{
	internal class PlayerMarker : IEntity, IUpdatable, IRenderable3D
	{
		private readonly VertexPositionColorTexture[] _vertex;

		private ITexture _markerTexture;

		public ITexture MarkerTexture
		{
			get
			{
				return _markerTexture;
			}
			set
			{
				_markerTexture?.Dispose();
				_markerTexture = value;
			}
		}

		public float MarkerOpacity { get; set; }

		public bool Visible { get; set; }

		public Color MarkerColor { get; set; }

		public Vector3 Size { get; set; }

		public float VerticalOffset { get; set; }

		public float DrawOrder
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				Vector3 position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				position.Z += VerticalOffset;
				return Vector3.DistanceSquared(position, GameService.Graphics.get_World().get_Camera().get_Position());
			}
		}

		public PlayerMarker()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Size = Vector3.get_Zero();
			MarkerOpacity = 1f;
			MarkerColor = Color.get_White();
			Visible = false;
			VerticalOffset = 0f;
			_vertex = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];
		}

		public void UpdateMarker()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			_vertex[0].Position = new Vector3(-1f, 1f, 1f) * Size;
			_vertex[0].TextureCoordinate = new Vector2(0f, 0f);
			_vertex[0].Color = MarkerColor;
			_vertex[1].Position = new Vector3(1f, 1f, 1f) * Size;
			_vertex[1].TextureCoordinate = new Vector2(1f, 0f);
			_vertex[1].Color = MarkerColor;
			_vertex[2].Position = new Vector3(-1f, -1f, 1f) * Size;
			_vertex[2].TextureCoordinate = new Vector2(0f, 1f);
			_vertex[2].Color = MarkerColor;
			_vertex[3].Position = new Vector3(1f, -1f, 1f) * Size;
			_vertex[3].TextureCoordinate = new Vector2(1f, 1f);
			_vertex[3].Color = MarkerColor;
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			if (!Visible || MarkerTexture == null)
			{
				return;
			}
			float x = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X;
			float y = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y;
			float z = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z + VerticalOffset;
			float panAngleRad = (float)Math.Atan2(camera.get_Forward().Y, camera.get_Forward().X);
			Matrix val = Matrix.CreateRotationX((float)((double)(float)Math.Asin(camera.get_Forward().Z) + Math.PI / 2.0));
			Matrix rotationMatrixZ = Matrix.CreateRotationZ((float)((double)panAngleRad - Math.PI / 2.0));
			Matrix translationMatrix = Matrix.CreateTranslation(x, y, z);
			Matrix worldMatrix = Matrix.Multiply(Matrix.Multiply(val, rotationMatrixZ), translationMatrix);
			BasicEffect val2 = new BasicEffect(graphicsDevice);
			val2.set_VertexColorEnabled(true);
			val2.set_TextureEnabled(true);
			val2.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			val2.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			val2.set_World(worldMatrix);
			val2.set_Texture(AsyncTexture2D.op_Implicit(MarkerTexture.Get()));
			val2.set_Alpha(MarkerOpacity);
			VertexBuffer geometryBuffer = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, 4, (BufferUsage)1);
			geometryBuffer.SetData<VertexPositionColorTexture>(_vertex);
			graphicsDevice.SetVertexBuffer(geometryBuffer, 0);
			Enumerator enumerator = ((Effect)val2).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
			graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, 2);
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
