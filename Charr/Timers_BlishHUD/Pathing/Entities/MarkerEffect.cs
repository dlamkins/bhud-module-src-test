using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public class MarkerEffect : SharedEffect
	{
		private const string PARAMETER_VIEW = "View";

		private const string PARAMETER_PROJECTION = "Projection";

		private const string PARAMETER_PLAYERVIEW = "PlayerView";

		private const string PARAMETER_PLAYERPOSITION = "PlayerPosition";

		private const string PARAMETER_CAMERAPOSITION = "CameraPosition";

		private Matrix _view;

		private Matrix _projection;

		private Matrix _playerView;

		private Vector3 _playerPosition;

		private Vector3 _cameraPosition;

		private const string PARAMETER_WORLD = "World";

		private const string PARAMETER_TEXTURE = "Texture";

		private const string PARAMETER_FADETEXTURE = "FadeTexture";

		private const string PARAMETER_OPACITY = "Opacity";

		private const string PARAMETER_FADENEAR = "FadeNear";

		private const string PARAMETER_FADEFAR = "FadeFar";

		private const string PARAMETER_PLAYERFADERADIUS = "PlayerFadeRadius";

		private const string PARAMETER_FADECENTER = "FadeCenter";

		private const string PARAMETER_TINTCOLOR = "TintColor";

		private Matrix _world;

		private Texture2D _texture;

		private Texture2D _fadeTexture;

		private float _opacity;

		private float _fadeNear;

		private float _fadeFar;

		private float _playerFadeRadius;

		private bool _fadeCenter;

		private Color _tintColor;

		public Matrix View
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _view;
			}
			set
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("View", ref _view, value);
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
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("Projection", ref _projection, value);
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

		public Matrix World
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _world;
			}
			set
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((SharedEffect)this).SetParameter("World", ref _world, value);
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
				return _fadeTexture;
			}
			set
			{
				((SharedEffect)this).SetParameter("FadeTexture", ref _fadeTexture, value);
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

		public MarkerEffect(Effect baseEffect)
			: this(baseEffect)
		{
		}

		private MarkerEffect(GraphicsDevice graphicsDevice, byte[] effectCode)
			: this(graphicsDevice, effectCode)
		{
		}

		private MarkerEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count)
			: this(graphicsDevice, effectCode, index, count)
		{
		}

		public void SetEntityState(Matrix world, Texture2D texture, float opacity, float fadeNear, float fadeFar, float playerFadeRadius, bool fadeCenter, Texture2D fadeTexture, Color tintColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			World = world;
			Texture = texture;
			Opacity = opacity;
			FadeNear = fadeNear;
			FadeFar = fadeFar;
			PlayerFadeRadius = playerFadeRadius;
			FadeCenter = fadeCenter;
			TintColor = tintColor;
			FadeTexture = fadeTexture;
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			PlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			CameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
			View = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			PlayerView = GameService.Gw2Mumble.get_PlayerCamera().get_PlayerView();
		}
	}
}
