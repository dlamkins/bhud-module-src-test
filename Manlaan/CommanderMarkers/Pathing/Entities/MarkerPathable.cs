using System;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Pathing.Entities
{
	public class MarkerPathable : Entity
	{
		private static readonly Logger Logger;

		private static readonly MarkerEffect _sharedMarkerEffect;

		private static readonly Texture2D _fadeTexture;

		private VertexPositionTexture[] _verts;

		private Texture2D _texture;

		private BillboardVerticalConstraint _verticalConstraint;

		private Vector2 _size = Vector2.get_One();

		private float _scale = 1f;

		private float _fadeNear = 5000f;

		private float _fadeFar = 5000f;

		private float _playerFadeRadius = 0.25f;

		private bool _fadeCenter = true;

		private bool _autoResize = true;

		private Color _tintColor = Color.get_White();

		private DynamicVertexBuffer _vertexBuffer;

		private bool _mouseOver;

		public bool AutoResize
		{
			get
			{
				return _autoResize;
			}
			set
			{
				SetProperty(ref _autoResize, value, rebuildEntity: false, "AutoResize");
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

		public float FadeNear
		{
			get
			{
				return Math.Min(_fadeNear, _fadeFar);
			}
			set
			{
				SetProperty(ref _fadeNear, value, rebuildEntity: false, "FadeNear");
			}
		}

		public float FadeFar
		{
			get
			{
				return Math.Max(_fadeNear, _fadeFar);
			}
			set
			{
				SetProperty(ref _fadeFar, value, rebuildEntity: false, "FadeFar");
			}
		}

		public bool FadeCenter
		{
			get
			{
				return _fadeCenter;
			}
			set
			{
				SetProperty(ref _fadeCenter, value, rebuildEntity: false, "FadeCenter");
			}
		}

		public float PlayerFadeRadius
		{
			get
			{
				return _playerFadeRadius;
			}
			set
			{
				SetProperty(ref _playerFadeRadius, value, rebuildEntity: false, "PlayerFadeRadius");
			}
		}

		public Color TintColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tintColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _tintColor, value, rebuildEntity: false, "TintColor");
			}
		}

		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _texture, value, rebuildEntity: false, "Texture") && _texture != null)
				{
					VerticalConstraint = ((_texture.get_Height() != _texture.get_Width()) ? BillboardVerticalConstraint.PlayerPosition : BillboardVerticalConstraint.CameraPosition);
					if (_autoResize)
					{
						Size = new Vector2(WorldUtil.GameToWorldCoord((float)_texture.get_Width()), WorldUtil.GameToWorldCoord((float)_texture.get_Height()));
					}
				}
			}
		}

		public bool ShouldShow { get; set; }

		static MarkerPathable()
		{
			Logger = Logger.GetLogger<MarkerPathable>();
			_sharedMarkerEffect = new MarkerEffect(GameService.Content.get_ContentManager().Load<Effect>("effects\\marker"));
		}

		public MarkerPathable()
			: this(null, Vector3.get_Zero(), Vector2.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)


		public MarkerPathable(Texture2D texture)
			: this(texture, Vector3.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public MarkerPathable(Texture2D texture, Vector3 position)
			: this(texture, position, Vector2.get_Zero())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public MarkerPathable(Texture2D texture, Vector3 position, Vector2 size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			Initialize();
			_autoResize = size == Vector2.get_Zero();
			Position = position;
			Size = size;
			Texture = texture;
		}

		private void Initialize()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			_verts = (VertexPositionTexture[])(object)new VertexPositionTexture[4];
			GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				_vertexBuffer = new DynamicVertexBuffer(((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice(), typeof(VertexPositionTexture), 4, (BufferUsage)1);
			}
			finally
			{
				((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
			}
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
			((VertexBuffer)_vertexBuffer).SetData<VertexPositionTexture>(_verts);
		}

		public override void HandleRebuild(GraphicsDevice graphicsDevice)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			RecalculateSize(_size, _scale);
		}

		public override void Draw(GraphicsDevice graphicsDevice)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			if (!Visible || !ShouldShow || _texture == null)
			{
				return;
			}
			Matrix modelMatrix = Matrix.CreateTranslation(_size.X / -2f, _size.Y / -2f, 0f) * Matrix.CreateScale(_scale);
			modelMatrix = ((!(Rotation == Vector3.get_Zero())) ? (modelMatrix * (Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position + RenderOffset))) : (modelMatrix * Matrix.CreateBillboard(Position + RenderOffset, new Vector3(GameService.Gw2Mumble.get_PlayerCamera().get_Position().X, GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y, (_verticalConstraint == BillboardVerticalConstraint.CameraPosition) ? GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z : GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z), new Vector3(0f, 0f, 1f), (Vector3?)GameService.Gw2Mumble.get_PlayerCamera().get_Forward())));
			_sharedMarkerEffect.SetEntityState(modelMatrix, _texture, _opacity, _fadeNear, _fadeFar, _playerFadeRadius, _fadeCenter, _fadeTexture, _tintColor);
			graphicsDevice.SetVertexBuffer((VertexBuffer)(object)_vertexBuffer);
			Enumerator enumerator = ((Effect)_sharedMarkerEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}
	}
}
