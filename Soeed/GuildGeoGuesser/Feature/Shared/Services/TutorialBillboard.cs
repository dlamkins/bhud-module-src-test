using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class TutorialBillboard
	{
		private static BasicEffect? _billboardEffect;

		private readonly VertexPositionTexture[] _verts = (VertexPositionTexture[])(object)new VertexPositionTexture[4];

		private Vector2 _size = Vector2.get_One();

		private float _scale = 1f;

		private AsyncTexture2D? _texture;

		private Vector3 _position;

		public float Opacity { get; set; } = 0.8f;


		public float Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				_scale = value;
				RecalculateSize(_size);
			}
		}

		public float DistanceFromPlayer => Vector3.Distance(_position, GameService.Gw2Mumble.get_PlayerCharacter().get_Position());

		public TutorialBillboard(AsyncTexture2D texture, Vector3 position, Vector2 size)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			_texture = texture;
			_position = position;
			_size = (Vector2)((size == Vector2.get_Zero()) ? new Vector2(1.5f, 1.5f) : size);
			RecalculateSize(_size);
		}

		public void SetPosition(Vector3 position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_position = position;
		}

		public void SetWorldSize(float diameterMeters)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_scale = 1f;
			_size = new Vector2(diameterMeters, diameterMeters);
			RecalculateSize(_size);
		}

		public void SetDistanceOpacity(float distanceMeters, float nearDistance = 2f, float nearOpacity = 0.3f, float farOpacity = 0.8f)
		{
			Opacity = ((distanceMeters <= nearDistance) ? nearOpacity : farOpacity);
		}

		public void HandleRebuild(GraphicsDevice graphicsDevice)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			RecalculateSize(_size);
		}

		private void RecalculateSize(Vector2 newSize)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			_verts[0] = new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 1f));
			_verts[1] = new VertexPositionTexture(new Vector3(newSize.X * _scale, 0f, 0f), new Vector2(0f, 1f));
			_verts[2] = new VertexPositionTexture(new Vector3(0f, newSize.Y * _scale, 0f), new Vector2(1f, 0f));
			_verts[3] = new VertexPositionTexture(new Vector3(newSize.X * _scale, newSize.Y * _scale, 0f), new Vector2(0f, 0f));
		}

		public void Draw(GraphicsDevice graphicsDevice)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			if (_texture == null || !_texture!.get_HasTexture())
			{
				return;
			}
			if (_billboardEffect == null)
			{
				_billboardEffect = CreateEffect(graphicsDevice);
			}
			PlayerCamera camera = GameService.Gw2Mumble.get_PlayerCamera();
			Vector3 cameraPosition = default(Vector3);
			((Vector3)(ref cameraPosition))._002Ector(camera.get_Position().X, camera.get_Position().Y, camera.get_Position().Z);
			_billboardEffect!.set_View(camera.get_View());
			_billboardEffect!.set_Projection(camera.get_Projection());
			_billboardEffect!.set_World(Matrix.CreateTranslation(new Vector3(_size.X / -2f, _size.Y / -2f, 0f)) * Matrix.CreateScale(_scale, _scale, 1f) * Matrix.CreateBillboard(_position, cameraPosition, Vector3.get_UnitZ(), (Vector3?)camera.get_Forward()));
			_billboardEffect!.set_Alpha(Opacity);
			_billboardEffect!.set_Texture(_texture!.get_Texture());
			BlendState previousBlend = graphicsDevice.get_BlendState();
			DepthStencilState previousDepth = graphicsDevice.get_DepthStencilState();
			RasterizerState previousRaster = graphicsDevice.get_RasterizerState();
			graphicsDevice.set_BlendState(BlendState.AlphaBlend);
			graphicsDevice.set_DepthStencilState(DepthStencilState.None);
			graphicsDevice.set_RasterizerState(RasterizerState.CullNone);
			Enumerator enumerator = ((Effect)_billboardEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
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
			graphicsDevice.set_BlendState(previousBlend);
			graphicsDevice.set_DepthStencilState(previousDepth);
			graphicsDevice.set_RasterizerState(previousRaster);
		}

		private static BasicEffect CreateEffect(GraphicsDevice graphicsDevice)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			BasicEffect val = new BasicEffect(graphicsDevice);
			val.set_TextureEnabled(true);
			return val;
		}
	}
}
