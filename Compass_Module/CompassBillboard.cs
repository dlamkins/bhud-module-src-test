using System;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Compass_Module
{
	public class CompassBillboard : IEntity, IUpdatable, IRenderable3D
	{
		private static VertexPositionTexture[] _verts;

		private static BasicEffect _sharedEffect;

		public float DrawOrder => Vector3.Distance(GetPosition(), GameService.Gw2Mumble.get_PlayerCharacter().get_Position());

		public Vector3 Offset { get; set; }

		public Texture2D Texture { get; set; }

		public float Opacity { get; set; } = 1f;


		public float Scale { get; set; }

		static CompassBillboard()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			_verts = (VertexPositionTexture[])(object)new VertexPositionTexture[4];
			_verts[0] = new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 1f));
			_verts[1] = new VertexPositionTexture(new Vector3(1f, 0f, 0f), new Vector2(0f, 1f));
			_verts[2] = new VertexPositionTexture(new Vector3(0f, 1f, 0f), new Vector2(1f, 0f));
			_verts[3] = new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(0f, 0f));
			_sharedEffect = new BasicEffect(GameService.Graphics.get_GraphicsDevice());
			_sharedEffect.set_TextureEnabled(true);
		}

		private Vector3 GetPosition()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Gw2Mumble.get_PlayerCharacter().get_Position() + Offset;
		}

		public CompassBillboard(Texture2D texture)
		{
			Texture = texture;
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			_sharedEffect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			_sharedEffect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			_sharedEffect.set_World(Matrix.CreateScale(Scale, Scale, 1f) * Matrix.CreateTranslation(new Vector3(Scale / -2f, Scale / -2f, 0f)) * Matrix.CreateBillboard(GetPosition(), GameService.Gw2Mumble.get_PlayerCamera().get_Position(), new Vector3(0f, 0f, 1f), (Vector3?)GameService.Gw2Mumble.get_PlayerCamera().get_Forward()));
			_sharedEffect.set_Alpha(Opacity);
			_sharedEffect.set_Texture(Texture);
			Enumerator enumerator = ((Effect)_sharedEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionTexture>((PrimitiveType)1, _verts, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}
	}
}
