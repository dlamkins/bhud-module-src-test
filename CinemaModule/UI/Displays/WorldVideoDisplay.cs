using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using CinemaModule.Models;
using CinemaModule.UI.Controls;
using CinemaModule.UI.Displays.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Displays
{
	public class WorldVideoDisplay : Control, IVideoDisplay, IDisposable
	{
		private const float MinVisibleDistance = 0.001f;

		private const float ForwardDotThreshold = 0.01f;

		private const int MinVisibleWidth = 8;

		private const int MinVisibleHeight = 4;

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

		public WorldVideoDisplay()
			: this()
		{
			_worldPosition = new WorldPosition3D();
			_renderer = new WorldScreenRenderer(_cornerCalculator);
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
		}

		public void UpdateTexture(Texture2D texture)
		{
			if (_videoTexture == null || ((GraphicsResource)_videoTexture).get_IsDisposed())
			{
				_videoTexture = texture;
				if (CinemaModule.Instance.TextureService.IsTextureReady(texture))
				{
					_aspectRatio = (float)texture.get_Width() / (float)texture.get_Height();
					_cornerCalculator.Recalculate(_worldPosition, _worldWidth, _aspectRatio);
				}
			}
			else if (texture != _videoTexture)
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
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
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
			Vector3 cameraPos = mumble.get_PlayerCamera().get_Position();
			Vector3 cameraForward = mumble.get_PlayerCamera().get_Forward();
			Vector3 val = _worldPosition.ToVector3();
			Vector3 screenNormal = CameraProjection.SafeNormalize(_worldPosition.GetNormalDirection());
			Vector3 cameraToScreen = val - cameraPos;
			Vector3.Dot(cameraToScreen, screenNormal);
			float distanceToCamera = ((Vector3)(ref cameraToScreen)).Length();
			if (distanceToCamera > _maxDistance || distanceToCamera < 0.001f)
			{
				SetInRange(inRange: false);
				SetOffScreen();
				_currentOpacity = 0f;
				return;
			}
			_currentOpacity = CalculateOpacity(distanceToCamera);
			SetInRange(inRange: true);
			cameraToScreen /= distanceToCamera;
			if (Vector3.Dot(cameraToScreen, cameraForward) < 0.01f)
			{
				SetOffScreen();
				return;
			}
			Vector3 cameraForwardNormalized = CameraProjection.SafeNormalize(cameraForward);
			float fov = mumble.get_PlayerCamera().get_FieldOfView();
			for (int i = 0; i < 4; i++)
			{
				_screenCorners[i] = CameraProjection.WorldToScreen(_cornerCalculator.WorldCorners[i], cameraPos, cameraForwardNormalized, fov);
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
			float screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			float screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			for (int i = 0; i < 4; i++)
			{
				float cornerX = _screenCorners[i].X;
				float cornerY = _screenCorners[i].Y;
				if (float.IsNaN(cornerX) || float.IsNaN(cornerY) || float.IsInfinity(cornerX) || float.IsInfinity(cornerY))
				{
					width = (height = (minX = (minY = 0)));
					return;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				if (_screenCorners[j].X < -5000f || _screenCorners[j].Y < -5000f)
				{
					width = (height = (minX = (minY = 0)));
					return;
				}
			}
			if (Math.Abs(CalculateQuadArea(_screenCorners)) < 10f)
			{
				width = (height = (minX = (minY = 0)));
				return;
			}
			float minXf = float.MaxValue;
			float minYf = float.MaxValue;
			float maxXf = float.MinValue;
			float maxYf = float.MinValue;
			for (int k = 0; k < 4; k++)
			{
				float cornerX2 = _screenCorners[k].X;
				float cornerY2 = _screenCorners[k].Y;
				if (cornerX2 < minXf)
				{
					minXf = cornerX2;
				}
				if (cornerY2 < minYf)
				{
					minYf = cornerY2;
				}
				if (cornerX2 > maxXf)
				{
					maxXf = cornerX2;
				}
				if (cornerY2 > maxYf)
				{
					maxYf = cornerY2;
				}
			}
			width = (int)(maxXf - minXf);
			height = (int)(maxYf - minYf);
			minX = (int)minXf;
			minY = (int)minYf;
			if ((float)width > screenWidth * 1.5f || (float)height > screenHeight * 1.5f)
			{
				width = (height = (minX = (minY = 0)));
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
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			if (_controlPanel == null)
			{
				return;
			}
			if (!((Control)this).get_Visible() || !_isOnScreen || !_isInRange)
			{
				_controlPanel.Hide();
				return;
			}
			Rectangle videoBounds = default(Rectangle);
			((Rectangle)(ref videoBounds))._002Ector(((Control)this).get_Location().X, ((Control)this).get_Location().Y, ((Control)this).get_Size().X, ((Control)this).get_Size().Y);
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			bool num = ((Rectangle)(ref videoBounds)).Contains(mousePos);
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
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (_isOnScreen)
			{
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
					_renderer.Render(graphicsDevice, viewMatrix, projectionMatrix, _videoTexture, _currentOpacity);
				}
				finally
				{
					spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				}
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
			((Control)this).DisposeControl();
		}
	}
}
