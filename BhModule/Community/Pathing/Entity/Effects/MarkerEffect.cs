using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Entity.Effects
{
	public class MarkerEffect : SharedEffect
	{
		private readonly EffectParameter _pView;

		private readonly EffectParameter _pProj;

		private readonly EffectParameter _pPlayerView;

		private readonly EffectParameter _pPlayerPos;

		private readonly EffectParameter _pCameraPos;

		private readonly EffectParameter _pRace;

		private readonly EffectParameter _pMount;

		private readonly EffectParameter _pFadeNearCamera;

		private readonly EffectParameter _pWorld;

		private readonly EffectParameter _pTexture;

		private readonly EffectParameter _pFadeTexture;

		private readonly EffectParameter _pOpacity;

		private readonly EffectParameter _pFadeNear;

		private readonly EffectParameter _pFadeFar;

		private readonly EffectParameter _pPlayerFadeRadius;

		private readonly EffectParameter _pFadeCenter;

		private readonly EffectParameter _pTintColor;

		private readonly EffectParameter _pShowDebugWireframe;

		private Matrix _view;

		private Matrix _projection;

		private Matrix _playerView;

		private Vector3 _playerPosition;

		private Vector3 _cameraPosition;

		private int _race;

		private int _mount;

		private bool _fadeNearCamera;

		private Matrix _world;

		private Texture2D _texture;

		private Texture2D _fadeTexture;

		private float _opacity;

		private float _fadeNear;

		private float _fadeFar;

		private float _playerFadeRadius;

		private bool _fadeCenter;

		private Color _tintColor;

		private bool _showDebugWireframe;

		public Matrix View
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _view;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _view, value, _pView);
			}
		}

		public Matrix Projection
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _projection;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _projection, value, _pProj);
			}
		}

		public Matrix PlayerView
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _playerView;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _playerView, value, _pPlayerView);
			}
		}

		public Vector3 PlayerPosition
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _playerPosition;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _playerPosition, value, _pPlayerPos);
			}
		}

		public Vector3 CameraPosition
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _cameraPosition;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _cameraPosition, value, _pCameraPos);
			}
		}

		public int Race
		{
			get
			{
				return _race;
			}
			set
			{
				SetParam(ref _race, value, _pRace);
			}
		}

		public int Mount
		{
			get
			{
				return _mount;
			}
			set
			{
				SetParam(ref _mount, value, _pMount);
			}
		}

		public bool FadeNearCamera
		{
			get
			{
				return _fadeNearCamera;
			}
			set
			{
				SetParam(ref _fadeNearCamera, value, _pFadeNearCamera);
			}
		}

		public Matrix World
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _world;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _world, value, _pWorld);
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
				SetParam(ref _texture, value, _pTexture);
			}
		}

		public Texture2D FadeTexture
		{
			get
			{
				return _fadeTexture;
			}
			set
			{
				SetParam(ref _fadeTexture, value, _pFadeTexture);
			}
		}

		public float Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				SetParam(ref _opacity, value, _pOpacity);
			}
		}

		public float FadeNear
		{
			get
			{
				return _fadeNear;
			}
			set
			{
				SetParam(ref _fadeNear, value, _pFadeNear);
			}
		}

		public float FadeFar
		{
			get
			{
				return _fadeFar;
			}
			set
			{
				SetParam(ref _fadeFar, value, _pFadeFar);
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
				SetParam(ref _playerFadeRadius, value, _pPlayerFadeRadius);
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
				SetParam(ref _fadeCenter, value, _pFadeCenter);
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
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _tintColor, value, _pTintColor);
			}
		}

		public bool ShowDebugWireframe
		{
			get
			{
				return _showDebugWireframe;
			}
			set
			{
				SetParam(ref _showDebugWireframe, value, _pShowDebugWireframe);
			}
		}

		public MarkerEffect(Effect baseEffect)
			: this(baseEffect)
		{
			_pView = ((Effect)this).get_Parameters().get_Item("View");
			_pProj = ((Effect)this).get_Parameters().get_Item("Projection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPos = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPos = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pFadeNearCamera = ((Effect)this).get_Parameters().get_Item("FadeNearCamera");
			_pWorld = ((Effect)this).get_Parameters().get_Item("World");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pShowDebugWireframe = ((Effect)this).get_Parameters().get_Item("ShowDebugWireframe");
		}

		private MarkerEffect(GraphicsDevice graphicsDevice, byte[] effectCode)
			: this(graphicsDevice, effectCode)
		{
			_pView = ((Effect)this).get_Parameters().get_Item("View");
			_pProj = ((Effect)this).get_Parameters().get_Item("Projection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPos = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPos = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pFadeNearCamera = ((Effect)this).get_Parameters().get_Item("FadeNearCamera");
			_pWorld = ((Effect)this).get_Parameters().get_Item("World");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pShowDebugWireframe = ((Effect)this).get_Parameters().get_Item("ShowDebugWireframe");
		}

		private MarkerEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count)
			: this(graphicsDevice, effectCode, index, count)
		{
			_pView = ((Effect)this).get_Parameters().get_Item("View");
			_pProj = ((Effect)this).get_Parameters().get_Item("Projection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPos = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPos = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pFadeNearCamera = ((Effect)this).get_Parameters().get_Item("FadeNearCamera");
			_pWorld = ((Effect)this).get_Parameters().get_Item("World");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pShowDebugWireframe = ((Effect)this).get_Parameters().get_Item("ShowDebugWireframe");
		}

		public void SetEntityState(Matrix world, Texture2D texture, float opacity, float fadeNear, float fadeFar, bool fadeNearCamera, Color tintColor, bool showDebugWireframe)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			World = world;
			Texture = texture;
			Opacity = opacity;
			FadeNear = fadeNear;
			FadeFar = fadeFar;
			FadeNearCamera = fadeNearCamera;
			TintColor = tintColor;
			ShowDebugWireframe = showDebugWireframe;
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected I4, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected I4, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			PlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			CameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
			Mount = (int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount();
			Race = (int)GameService.Gw2Mumble.get_PlayerCharacter().get_Race();
			View = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			PlayerView = GameService.Gw2Mumble.get_PlayerCamera().get_PlayerView();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref Matrix f, Matrix v, EffectParameter p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref Vector3 f, Vector3 v, EffectParameter p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref Texture2D f, Texture2D v, EffectParameter p)
		{
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue((Texture)(object)v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref float f, float v, EffectParameter p)
		{
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref int f, int v, EffectParameter p)
		{
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref bool f, bool v, EffectParameter p)
		{
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(v);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetParam(ref Color f, Color v, EffectParameter p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (f == v)
			{
				return false;
			}
			f = v;
			p.SetValue(((Color)(ref v)).ToVector4());
			return true;
		}
	}
}
