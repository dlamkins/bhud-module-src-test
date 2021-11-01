using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public class TrailEffect : SharedEffect
	{
		private const string PARAMETER_WORLDVIEWPROJECTION = "WorldViewProjection";

		private const string PARAMETER_PLAYERVIEW = "PlayerView";

		private const string PARAMETER_PLAYERPOSITION = "PlayerPosition";

		private const string PARAMETER_CAMERAPOSITION = "CameraPosition";

		private const string PARAMETER_TOTALMILLISECONDS = "TotalMilliseconds";

		private Matrix _worldViewProjection;

		private Matrix _playerView;

		private Vector3 _playerPosition;

		private Vector3 _cameraPosition;

		private float _totalMilliseconds;

		private const string PARAMETER_TEXTURE = "Texture";

		private const string PARAMETER_FADETEXTURE = "FadeTexture";

		private const string PARAMETER_FLOWSPEED = "FlowSpeed";

		private const string PARAMETER_FADENEAR = "FadeNear";

		private const string PARAMETER_FADEFAR = "FadeFar";

		private const string PARAMETER_OPACITY = "Opacity";

		private const string PARAMETER_TINTCOLOR = "TintColor";

		private const string PARAMETER_PLAYERFADERADIUS = "PlayerFadeRadius";

		private const string PARAMETER_FADECENTER = "FadeCenter";

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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("WorldViewProjection", ref _worldViewProjection, value);
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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("PlayerView", ref _playerView, value);
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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("PlayerPosition", ref _playerPosition, value);
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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("CameraPosition", ref _cameraPosition, value);
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
				((SharedEffect)this).SetParameter("TotalMilliseconds", ref _totalMilliseconds, value);
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
				((SharedEffect)this).SetParameter("Texture", ref _texture, value);
			}
		}

		public Texture2D FadeTexture
		{
			get
			{
				return _texture;
			}
			set
			{
				((SharedEffect)this).SetParameter("FadeTexture", ref _fadeTexture, value);
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
				((SharedEffect)this).SetParameter("FlowSpeed", ref _flowSpeed, value);
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
				((SharedEffect)this).SetParameter("FadeNear", ref _fadeNear, value);
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
				((SharedEffect)this).SetParameter("FadeFar", ref _fadeFar, value);
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
				((SharedEffect)this).SetParameter("Opacity", ref _opacity, value);
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
				((SharedEffect)this).SetParameter("PlayerFadeRadius", ref _playerFadeRadius, value);
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
				((SharedEffect)this).SetParameter("FadeCenter", ref _fadeCenter, value);
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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("TintColor", ref _tintColor, value);
			}
		}

		public TrailEffect(Effect cloneSource)
			: this(cloneSource)
		{
		}

		public TrailEffect(GraphicsDevice graphicsDevice, byte[] effectCode)
			: this(graphicsDevice, effectCode)
		{
		}

		public TrailEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count)
			: this(graphicsDevice, effectCode, index, count)
		{
		}

		public void SetEntityState(Texture2D texture, float flowSpeed, float fadeNear, float fadeFar, float opacity, float playerFadeRadius, bool fadeCenter, Texture2D fadeTexture, Color tintColor)
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
			FadeTexture = fadeTexture;
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			TotalMilliseconds = (float)gameTime.get_TotalGameTime().TotalMilliseconds;
			PlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			CameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
			WorldViewProjection = GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection();
			PlayerView = GameService.Gw2Mumble.get_PlayerCamera().get_PlayerView();
		}
	}
}
