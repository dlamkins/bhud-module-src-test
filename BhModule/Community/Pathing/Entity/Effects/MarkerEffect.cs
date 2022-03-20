using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Entity.Effects
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

		private const string PARAMETER_RACE = "Race";

		private const string PARAMETER_MOUNT = "Mount";

		private const string PARAMETER_FADENEARCAMERA = "FadeNearCamera";

		private int _race;

		private int _mount;

		private bool _fadeNearCamera;

		private const string PARAMETER_WORLD = "World";

		private const string PARAMETER_TEXTURE = "Texture";

		private const string PARAMETER_FADETEXTURE = "FadeTexture";

		private const string PARAMETER_OPACITY = "Opacity";

		private const string PARAMETER_FADENEAR = "FadeNear";

		private const string PARAMETER_FADEFAR = "FadeFar";

		private const string PARAMETER_PLAYERFADERADIUS = "PlayerFadeRadius";

		private const string PARAMETER_FADECENTER = "FadeCenter";

		private const string PARAMETER_TINTCOLOR = "TintColor";

		private const string PARAMETER_SHOWDEBUGWIREFRAME = "ShowDebugWireframe";

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

		public int Race
		{
			get
			{
				return _race;
			}
			set
			{
				((SharedEffect)this).SetParameter("Race", ref _race, value);
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
				((SharedEffect)this).SetParameter("Mount", ref _mount, value);
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

		public bool FadeNearCamera
		{
			get
			{
				return _fadeNearCamera;
			}
			set
			{
				((SharedEffect)this).SetParameter("FadeNearCamera", ref _fadeNearCamera, value);
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

		public bool ShowDebugWireframe
		{
			get
			{
				return _showDebugWireframe;
			}
			set
			{
				((SharedEffect)this).SetParameter("ShowDebugWireframe", ref _showDebugWireframe, value);
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
	}
}
