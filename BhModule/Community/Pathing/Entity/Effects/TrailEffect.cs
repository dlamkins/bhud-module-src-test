using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Entity.Effects
{
	public class TrailEffect : SharedEffect
	{
		private readonly EffectParameter _pWorldViewProjection;

		private readonly EffectParameter _pPlayerView;

		private readonly EffectParameter _pPlayerPosition;

		private readonly EffectParameter _pCameraPosition;

		private readonly EffectParameter _pTotalMilliseconds;

		private readonly EffectParameter _pRace;

		private readonly EffectParameter _pMount;

		private readonly EffectParameter _pTexture;

		private readonly EffectParameter _pFadeTexture;

		private readonly EffectParameter _pFlowSpeed;

		private readonly EffectParameter _pFadeNear;

		private readonly EffectParameter _pFadeFar;

		private readonly EffectParameter _pOpacity;

		private readonly EffectParameter _pTintColor;

		private readonly EffectParameter _pPlayerFadeRadius;

		private readonly EffectParameter _pFadeCenter;

		private Matrix _worldViewProjection;

		private Matrix _playerView;

		private Vector3 _playerPosition;

		private Vector3 _cameraPosition;

		private float _totalMilliseconds;

		private int _race;

		private int _mount;

		private Texture2D _texture;

		private Texture2D _fadeTexture;

		private float _flowSpeed;

		private float _fadeNear;

		private float _fadeFar;

		private float _opacity;

		private float _playerFadeRadius;

		private bool _fadeCenter;

		private Color _tintColor;

		public Matrix WorldViewProjection
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _worldViewProjection;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				SetParam(ref _worldViewProjection, value, _pWorldViewProjection);
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
				SetParam(ref _playerPosition, value, _pPlayerPosition);
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
				SetParam(ref _cameraPosition, value, _pCameraPosition);
			}
		}

		public float TotalMilliseconds
		{
			get
			{
				return _totalMilliseconds;
			}
			set
			{
				SetParam(ref _totalMilliseconds, value, _pTotalMilliseconds);
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

		public float FlowSpeed
		{
			get
			{
				return _flowSpeed;
			}
			set
			{
				SetParam(ref _flowSpeed, value, _pFlowSpeed);
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

		public TrailEffect(Effect cloneSource)
			: this(cloneSource)
		{
			_pWorldViewProjection = ((Effect)this).get_Parameters().get_Item("WorldViewProjection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPosition = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPosition = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pTotalMilliseconds = ((Effect)this).get_Parameters().get_Item("TotalMilliseconds");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pFlowSpeed = ((Effect)this).get_Parameters().get_Item("FlowSpeed");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
		}

		public TrailEffect(GraphicsDevice graphicsDevice, byte[] effectCode)
			: this(graphicsDevice, effectCode)
		{
			_pWorldViewProjection = ((Effect)this).get_Parameters().get_Item("WorldViewProjection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPosition = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPosition = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pTotalMilliseconds = ((Effect)this).get_Parameters().get_Item("TotalMilliseconds");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pFlowSpeed = ((Effect)this).get_Parameters().get_Item("FlowSpeed");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
		}

		public TrailEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count)
			: this(graphicsDevice, effectCode, index, count)
		{
			_pWorldViewProjection = ((Effect)this).get_Parameters().get_Item("WorldViewProjection");
			_pPlayerView = ((Effect)this).get_Parameters().get_Item("PlayerView");
			_pPlayerPosition = ((Effect)this).get_Parameters().get_Item("PlayerPosition");
			_pCameraPosition = ((Effect)this).get_Parameters().get_Item("CameraPosition");
			_pTotalMilliseconds = ((Effect)this).get_Parameters().get_Item("TotalMilliseconds");
			_pRace = ((Effect)this).get_Parameters().get_Item("Race");
			_pMount = ((Effect)this).get_Parameters().get_Item("Mount");
			_pTexture = ((Effect)this).get_Parameters().get_Item("Texture");
			_pFadeTexture = ((Effect)this).get_Parameters().get_Item("FadeTexture");
			_pFlowSpeed = ((Effect)this).get_Parameters().get_Item("FlowSpeed");
			_pFadeNear = ((Effect)this).get_Parameters().get_Item("FadeNear");
			_pFadeFar = ((Effect)this).get_Parameters().get_Item("FadeFar");
			_pOpacity = ((Effect)this).get_Parameters().get_Item("Opacity");
			_pTintColor = ((Effect)this).get_Parameters().get_Item("TintColor");
			_pPlayerFadeRadius = ((Effect)this).get_Parameters().get_Item("PlayerFadeRadius");
			_pFadeCenter = ((Effect)this).get_Parameters().get_Item("FadeCenter");
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

		public void SetEntityState(Texture2D texture, float flowSpeed, float fadeNear, float fadeFar, float opacity, float playerFadeRadius, bool fadeCenter, Color tintColor)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Texture = texture;
			FlowSpeed = flowSpeed;
			FadeNear = fadeNear;
			FadeFar = fadeFar;
			Opacity = opacity;
			PlayerFadeRadius = playerFadeRadius;
			FadeCenter = fadeCenter;
			TintColor = tintColor;
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected I4, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected I4, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			TotalMilliseconds = (float)gameTime.get_TotalGameTime().TotalMilliseconds;
			PlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			CameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
			Mount = (int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount();
			Race = (int)GameService.Gw2Mumble.get_PlayerCharacter().get_Race();
			WorldViewProjection = GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection();
			PlayerView = GameService.Gw2Mumble.get_PlayerCamera().get_PlayerView();
		}
	}
}
