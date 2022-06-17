using System;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public class Billboard : Entity
	{
		private VertexPositionTexture[] _verts;

		private bool _autoResizeBillboard = true;

		private Vector2 _size = Vector2.get_One();

		private float _scale = 1f;

		private AsyncTexture2D _texture;

		private BillboardVerticalConstraint _verticalConstraint;

		private static BasicEffect _billboardEffect;

		public bool AutoResizeBillboard
		{
			get
			{
				return _autoResizeBillboard;
			}
			set
			{
				SetProperty(ref _autoResizeBillboard, value, rebuildEntity: false, "AutoResizeBillboard");
			}
		}

		public BillboardVerticalConstraint VerticalConstraint
		{
			get
			{
				return _verticalConstraint;
			}
			set
			{
				SetProperty(ref _verticalConstraint, value, rebuildEntity: false, "VerticalConstraint");
			}
		}

		public Vector2 Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _size;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _size, value, rebuildEntity: false, "Size"))
				{
					RecalculateSize(_size, _scale);
				}
			}
		}

		public float Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _scale, value, rebuildEntity: false, "Scale"))
				{
					RecalculateSize(_size, _scale);
				}
			}
		}

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _texture, value, rebuildEntity: false, "Texture") && _autoResizeBillboard && _texture.HasTexture)
				{
					Rectangle bounds = _texture.Texture.get_Bounds();
					Point size = ((Rectangle)(ref bounds)).get_Size();
					Size = ((Point)(ref size)).ToVector2().ToWorldCoord();
				}
			}
		}

		public Billboard()
			: this(null, Vector3.get_Zero(), Vector2.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)


		public Billboard(Texture2D image)
			: this(image, Vector3.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public Billboard(Texture2D image, Vector3 position)
			: this(image, position, Vector2.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public Billboard(Texture2D image, Vector3 position, Vector2 size)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			Initialize();
			AutoResizeBillboard = size == Vector2.get_Zero();
			Size = size;
			Texture = image;
			Position = position;
		}

		private void Initialize()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_verts = (VertexPositionTexture[])(object)new VertexPositionTexture[4];
			_billboardEffect = (BasicEffect)(((object)_billboardEffect) ?? ((object)new BasicEffect(GameService.Graphics.GraphicsDevice)));
			_billboardEffect.set_TextureEnabled(true);
		}

		private void RecalculateSize(Vector2 newSize, float scale)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			_verts[0] = new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 1f));
			_verts[1] = new VertexPositionTexture(new Vector3(newSize.X * scale, 0f, 0f), new Vector2(0f, 1f));
			_verts[2] = new VertexPositionTexture(new Vector3(0f, newSize.Y * scale, 0f), new Vector2(1f, 0f));
			_verts[3] = new VertexPositionTexture(new Vector3(newSize.X * scale, newSize.Y * scale, 0f), new Vector2(0f, 0f));
		}

		public override void Draw(GraphicsDevice graphicsDevice)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			if (Texture == null)
			{
				return;
			}
			_billboardEffect.set_View(GameService.Gw2Mumble.PlayerCamera.View);
			_billboardEffect.set_Projection(GameService.Gw2Mumble.PlayerCamera.Projection);
			_billboardEffect.set_World(Matrix.CreateTranslation(new Vector3(Size.X / -2f, Size.Y / -2f, 0f)) * Matrix.CreateScale(_scale, _scale, 1f) * Matrix.CreateBillboard(Position + RenderOffset, new Vector3(GameService.Gw2Mumble.PlayerCamera.Position.X, GameService.Gw2Mumble.PlayerCamera.Position.Y, (_verticalConstraint == BillboardVerticalConstraint.CameraPosition) ? GameService.Gw2Mumble.PlayerCamera.Position.Z : GameService.Gw2Mumble.PlayerCharacter.Position.Z), new Vector3(0f, 0f, 1f), (Vector3?)GameService.Gw2Mumble.PlayerCamera.Forward));
			_billboardEffect.set_Alpha(Opacity);
			_billboardEffect.set_Texture(Texture.Texture);
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
		}

		public override void HandleRebuild(GraphicsDevice graphicsDevice)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			RecalculateSize(_size, _scale);
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
