using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using CinemaModule.Models.Location;
using CinemaModule.UI.Controls;
using CinemaModule.UI.VideoDisplays.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.VideoDisplays
{
	public class WorldVideoDisplay : Control, IVideoDisplay, IDisposable
	{
		private const float MinVisibleDistance = 0.001f;

		private const float ForwardDotThreshold = 0.01f;

		private const int MinVisibleWidth = 8;

		private const int MinVisibleHeight = 4;

		private const float BehindCameraThreshold = -5000f;

		private const float MinAreaThreshold = 10f;

		private const float CrossProductEpsilon = 0.001f;

		private Texture2D _videoTexture;

		private WorldPosition3D _worldPosition;

		private float _worldWidth = 10f;

		private float _aspectRatio = 1.7777778f;

		private bool _isOnScreen;

		private bool _isInRange;

		private Vector2[] _screenCorners = (Vector2[])(object)new Vector2[4];

		private WorldVideoControls _controlPanel;

		private readonly ScreenCornerCalculator _cornerCalculator = new ScreenCornerCalculator();

		private WorldScreenRenderer _renderer;

		private VideoControlsRenderer _controlsRenderer;

		private float _fadeStartDistance = 65f;

		private float _maxDistance = 70f;

		private float _currentOpacity = 1f;

		public bool IsInRange => _isInRange;

		public bool IsPaused
		{
			get
			{
				return _controlPanel?.IsPaused ?? false;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.IsPaused = value;
				}
			}
		}

		public int Volume
		{
			get
			{
				return _controlPanel?.Volume ?? 100;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.Volume = value;
				}
			}
		}

		public bool IsTwitchStream
		{
			get
			{
				return _controlPanel?.IsTwitchStream ?? false;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.IsTwitchStream = value;
				}
			}
		}

		public bool IsSeekable
		{
			get
			{
				return _controlPanel?.IsSeekable ?? false;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.IsSeekable = value;
				}
			}
		}

		public bool IsWatchPartyViewer
		{
			get
			{
				return _controlPanel?.IsWatchPartyViewer ?? false;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.IsWatchPartyViewer = value;
				}
			}
		}

		public float CurrentPosition
		{
			get
			{
				return _controlPanel?.CurrentPosition ?? 0f;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.CurrentPosition = value;
				}
			}
		}

		public long Duration
		{
			get
			{
				return _controlPanel?.Duration ?? 0;
			}
			set
			{
				if (_controlPanel != null)
				{
					_controlPanel.Duration = value;
				}
			}
		}

		public bool IsOffline { get; set; }

		public Texture2D OfflineTexture { get; set; }

		public string RadioTrackName { get; set; }

		public WorldPosition3D WorldPosition
		{
			get
			{
				return _worldPosition;
			}
			set
			{
				_worldPosition = value;
				_cornerCalculator.Recalculate(_worldPosition, _worldWidth, _aspectRatio);
				ResetDisplayState();
			}
		}

		public float WorldWidth
		{
			get
			{
				return _worldWidth;
			}
			set
			{
				_worldWidth = value;
				_cornerCalculator.Recalculate(_worldPosition, _worldWidth, _aspectRatio);
			}
		}

		public event EventHandler PlayPauseClicked;

		public event EventHandler<int> VolumeChanged;

		public event EventHandler SettingsClicked;

		public event EventHandler<int> QualityChanged;

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public event EventHandler<bool> InRangeChanged;

		public event EventHandler<float> SeekRequested;

		public WorldVideoDisplay()
			: this()
		{
			_worldPosition = new WorldPosition3D();
			_renderer = new WorldScreenRenderer(_cornerCalculator);
			_controlsRenderer = new VideoControlsRenderer(CinemaModule.Instance.TextureService);
			((Control)this).set_ClipsBounds(false);
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (!_isOnScreen || !_isInRange)
			{
				return (CaptureType)0;
			}
			return ((Control)this).CapturesInput();
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			if (_controlPanel != null)
			{
				((Control)_controlPanel).set_Visible(true);
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			if (_controlPanel != null)
			{
				((Control)_controlPanel).set_Visible(false);
			}
		}

		public void Initialize(Container parent)
		{
			WorldVideoControls worldVideoControls = new WorldVideoControls();
			((Control)worldVideoControls).set_Parent(parent);
			((Control)worldVideoControls).set_ZIndex(((Control)this).get_ZIndex() + 1);
			_controlPanel = worldVideoControls;
			_controlPanel.PlayPauseClicked += delegate
			{
				this.PlayPauseClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlPanel.VolumeChanged += delegate(object s, int vol)
			{
				this.VolumeChanged?.Invoke(this, vol);
			};
			_controlPanel.SettingsClicked += delegate
			{
				this.SettingsClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlPanel.QualityChanged += delegate(object s, int index)
			{
				this.QualityChanged?.Invoke(this, index);
			};
			_controlPanel.TwitchChatClicked += delegate
			{
				this.TwitchChatClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlPanel.CloseClicked += delegate
			{
				this.CloseClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlPanel.SeekRequested += delegate(object s, float pos)
			{
				this.SeekRequested?.Invoke(this, pos);
			};
		}

		public void UpdateTexture(Texture2D texture)
		{
			if (texture != _videoTexture || _videoTexture == null || ((GraphicsResource)_videoTexture).get_IsDisposed())
			{
				_videoTexture = texture;
				if (CinemaModule.Instance.TextureService.IsTextureReady(texture))
				{
					_aspectRatio = (float)texture.get_Width() / (float)texture.get_Height();
					_cornerCalculator.Recalculate(_worldPosition, _worldWidth, _aspectRatio);
				}
			}
		}

		public void UpdateAvailableQualities(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			_controlPanel?.UpdateAvailableQualities(qualityNames, selectedIndex);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			UpdateScreenProjection();
			UpdateControlPanelPosition();
			UpdateControlPanelVisibility();
		}

		private void UpdateScreenProjection()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if (!_cornerCalculator.IsValid || _worldPosition == null)
			{
				SetInRange(inRange: false);
				SetOffScreen();
				return;
			}
			Gw2MumbleService mumble = GameService.Gw2Mumble;
			if (mumble == null || !mumble.get_IsAvailable() || mumble.get_CurrentMap().get_Id() != _worldPosition.MapId)
			{
				SetInRange(inRange: false);
				SetOffScreen();
				return;
			}
			if (!UpdateDistanceAndOpacity(mumble.get_PlayerCamera().get_Position()))
			{
				SetInRange(inRange: false);
				SetOffScreen();
				return;
			}
			SetInRange(inRange: true);
			if (!ProjectCornersToScreen(mumble))
			{
				SetOffScreen();
				return;
			}
			CalculateScreenBounds(out var width, out var height, out var minX, out var minY);
			if (width < 8 || height < 4)
			{
				_isOnScreen = false;
				return;
			}
			_isOnScreen = true;
			((Control)this).set_Location(new Point(minX, minY));
			((Control)this).set_Size(new Point(width, height));
		}

		private bool UpdateDistanceAndOpacity(Vector3 cameraPos)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Vector3 cameraToScreen = _worldPosition.ToVector3() - cameraPos;
			float distanceToCamera = ((Vector3)(ref cameraToScreen)).Length();
			if (distanceToCamera > _maxDistance || distanceToCamera < 0.001f)
			{
				_currentOpacity = 0f;
				return false;
			}
			_currentOpacity = CalculateOpacity(distanceToCamera);
			return true;
		}

		private bool ProjectCornersToScreen(Gw2MumbleService mumble)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Vector3 cameraPos = mumble.get_PlayerCamera().get_Position();
			Vector3 cameraForward = mumble.get_PlayerCamera().get_Forward();
			Vector3 cameraToScreen = _worldPosition.ToVector3() - cameraPos;
			float distanceToCamera = ((Vector3)(ref cameraToScreen)).Length();
			cameraToScreen /= distanceToCamera;
			if (Vector3.Dot(cameraToScreen, cameraForward) < 0.01f)
			{
				return false;
			}
			Vector3 cameraForwardNormalized = CameraProjection.SafeNormalize(cameraForward);
			float fov = mumble.get_PlayerCamera().get_FieldOfView();
			for (int i = 0; i < 4; i++)
			{
				_screenCorners[i] = CameraProjection.WorldToScreen(_cornerCalculator.WorldCorners[i], cameraPos, cameraForwardNormalized, fov);
			}
			return true;
		}

		private float CalculateOpacity(float distanceToCamera)
		{
			if (distanceToCamera <= _fadeStartDistance)
			{
				return 1f;
			}
			float fadeRange = _maxDistance - _fadeStartDistance;
			float fadeProgress = (distanceToCamera - _fadeStartDistance) / fadeRange;
			return 1f - MathHelper.Clamp(fadeProgress, 0f, 1f);
		}

		private void CalculateScreenBounds(out int width, out int height, out int minX, out int minY)
		{
			width = (height = (minX = (minY = 0)));
			if (ValidateScreenCorners() && ValidateQuadArea())
			{
				float screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
				float screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
				CalculateAxisAlignedBounds(out var minXf, out var minYf, out var maxXf, out var maxYf);
				int calculatedWidth = (int)(maxXf - minXf);
				int calculatedHeight = (int)(maxYf - minYf);
				if (!((float)calculatedWidth > screenWidth * 1.5f) && !((float)calculatedHeight > screenHeight * 1.5f))
				{
					width = calculatedWidth;
					height = calculatedHeight;
					minX = (int)minXf;
					minY = (int)minYf;
				}
			}
		}

		private bool ValidateScreenCorners()
		{
			for (int i = 0; i < 4; i++)
			{
				float cornerX = _screenCorners[i].X;
				float cornerY = _screenCorners[i].Y;
				if (float.IsNaN(cornerX) || float.IsNaN(cornerY) || float.IsInfinity(cornerX) || float.IsInfinity(cornerY))
				{
					return false;
				}
				if (cornerX < -5000f || cornerY < -5000f)
				{
					return false;
				}
			}
			return true;
		}

		private bool ValidateQuadArea()
		{
			return Math.Abs(CalculateQuadArea(_screenCorners)) >= 10f;
		}

		private void CalculateAxisAlignedBounds(out float minX, out float minY, out float maxX, out float maxY)
		{
			minX = (minY = float.MaxValue);
			maxX = (maxY = float.MinValue);
			for (int i = 0; i < 4; i++)
			{
				float cornerX = _screenCorners[i].X;
				float cornerY = _screenCorners[i].Y;
				if (cornerX < minX)
				{
					minX = cornerX;
				}
				if (cornerY < minY)
				{
					minY = cornerY;
				}
				if (cornerX > maxX)
				{
					maxX = cornerX;
				}
				if (cornerY > maxY)
				{
					maxY = cornerY;
				}
			}
		}

		private static float CalculateQuadArea(Vector2[] corners)
		{
			float area = 0f;
			for (int i = 0; i < 4; i++)
			{
				int j = (i + 1) % 4;
				area += corners[i].X * corners[j].Y;
				area -= corners[j].X * corners[i].Y;
			}
			return area * 0.5f;
		}

		private bool IsPointInScreenQuad(Vector2 point)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			bool? expectedSign = null;
			for (int i = 0; i < 4; i++)
			{
				Vector2 a = _screenCorners[i];
				Vector2 b = _screenCorners[(i + 1) % 4];
				float cross = (b.X - a.X) * (point.Y - a.Y) - (b.Y - a.Y) * (point.X - a.X);
				if (!(Math.Abs(cross) < 0.001f))
				{
					bool isPositive = cross > 0f;
					if (!expectedSign.HasValue)
					{
						expectedSign = isPositive;
					}
					else if (expectedSign != isPositive)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void SetInRange(bool inRange)
		{
			if (_isInRange != inRange)
			{
				_isInRange = inRange;
				this.InRangeChanged?.Invoke(this, inRange);
			}
		}

		private void SetOffScreen()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_isOnScreen = false;
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(Point.get_Zero());
		}

		private void ResetDisplayState()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			_isOnScreen = false;
			_isInRange = false;
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(Point.get_Zero());
			_currentOpacity = 0f;
			for (int i = 0; i < _screenCorners.Length; i++)
			{
				_screenCorners[i] = Vector2.get_Zero();
			}
			_controlPanel?.Reset();
		}

		private void UpdateControlPanelPosition()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (_controlPanel != null && _isOnScreen)
			{
				Rectangle videoBounds = default(Rectangle);
				((Rectangle)(ref videoBounds))._002Ector(((Control)this).get_Location().X, ((Control)this).get_Location().Y, ((Control)this).get_Size().X, ((Control)this).get_Size().Y);
				_controlPanel.UpdatePosition(videoBounds);
			}
		}

		private void UpdateControlPanelVisibility()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			if (_controlPanel == null)
			{
				return;
			}
			if (!((Control)this).get_Visible() || !_isOnScreen || !_isInRange)
			{
				_controlPanel.Hide();
				return;
			}
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			bool num = IsPointInScreenQuad(new Vector2((float)mousePos.X, (float)mousePos.Y));
			int num2;
			if (((Control)_controlPanel).get_Location() != Point.get_Zero())
			{
				Rectangle absoluteBounds = ((Control)_controlPanel).get_AbsoluteBounds();
				num2 = (((Rectangle)(ref absoluteBounds)).Contains(mousePos) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool isHoveringPanel = (byte)num2 != 0;
			bool isTrackBarDragging = _controlPanel.IsTrackBarDragging;
			if (num || isHoveringPanel || isTrackBarDragging)
			{
				_controlPanel.Show();
			}
			else
			{
				_controlPanel.Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (!_isOnScreen)
			{
				return;
			}
			GraphicsDevice graphicsDevice = ((GraphicsResource)spriteBatch).get_GraphicsDevice();
			_renderer.Initialize(graphicsDevice);
			spriteBatch.End();
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				Vector3 cameraPos = gw2Mumble.get_PlayerCamera().get_Position();
				Vector3 cameraForward = gw2Mumble.get_PlayerCamera().get_Forward();
				float fov = gw2Mumble.get_PlayerCamera().get_FieldOfView();
				float aspectRatio = (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
				CameraProjection.CreateViewProjectionMatrices(cameraPos, cameraForward, fov, aspectRatio, out var viewMatrix, out var projectionMatrix);
				Texture2D textureToRender = ((IsOffline && OfflineTexture != null && !((GraphicsResource)OfflineTexture).get_IsDisposed()) ? OfflineTexture : _videoTexture);
				_renderer.Render(graphicsDevice, viewMatrix, projectionMatrix, textureToRender, _currentOpacity);
				if (!string.IsNullOrEmpty(RadioTrackName))
				{
					Texture2D trackNameTexture = _controlsRenderer.GetOrCreateTrackNameTexture(graphicsDevice, RadioTrackName);
					_renderer.RenderOverlay(graphicsDevice, viewMatrix, projectionMatrix, trackNameTexture, _currentOpacity);
				}
			}
			finally
			{
				spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			}
		}

		protected override void DisposeControl()
		{
			WorldVideoControls controlPanel = _controlPanel;
			if (controlPanel != null)
			{
				((Control)controlPanel).Dispose();
			}
			_renderer?.Dispose();
			_controlsRenderer?.DisposeTrackNameTexture();
			((Control)this).DisposeControl();
		}
	}
}
