using System;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Effects;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class InfoWindow : Container
	{
		private const double FADEDURATION = 300.0;

		private static readonly Texture2D _windowTexture;

		private static readonly Texture2D _windowMask;

		private static readonly Texture2D _windowClose;

		private bool _showing;

		private double _fadeCompletion;

		private readonly IPackState _packState;

		private Texture2D _croppedWindow = _windowTexture;

		private Texture2D _croppedMask = _windowMask;

		private Rectangle _closeButtonBounds;

		static InfoWindow()
		{
			_windowTexture = PathingModule.Instance.ContentsManager.GetTexture("png\\controls\\156475+156476.png");
			_windowMask = PathingModule.Instance.ContentsManager.GetTexture("png\\controls\\156477.png");
			_windowClose = PathingModule.Instance.ContentsManager.GetTexture("png\\controls\\156106.png");
		}

		public InfoWindow(IPackState packState)
			: this()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			_packState = packState;
			((Control)this).set_Size(new Point(512, 512));
			((Control)this).set_Location(new Point(_packState.UserResourceStates.Advanced.InfoWindowXOffsetPixels, _packState.UserResourceStates.Advanced.InfoWindowYOffsetPixels));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_AutoSizePadding(new Point(40, 70));
			((Control)this).set_SpriteBatchParameters(new SpriteBatchParameters((SpriteSortMode)1, BlendState.Additive, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)(object)AlphaMaskEffect.SharedInstance, (Matrix?)null));
			((Control)this).set_Opacity(0f);
			((Control)this).set_ClipsBounds(false);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			_croppedMask = Texture2DExtension.GetRegion(_windowMask, new Rectangle(0, 512 - Math.Min(((Control)this).get_Height(), _windowMask.get_Height()), Math.Min(((Control)this).get_Width(), _windowMask.get_Width()), Math.Min(((Control)this).get_Height(), _windowMask.get_Height())));
			_croppedWindow = Texture2DExtension.GetRegion(_windowTexture, new Rectangle(0, 0, Math.Min(((Control)this).get_Size().X, _windowTexture.get_Width()), Math.Min(((Control)this).get_Size().Y, _windowTexture.get_Height())));
		}

		private void TriggerFade()
		{
			float fadeOffset = (_showing ? (1f - ((Control)this).get_Opacity()) : ((Control)this).get_Opacity());
			_fadeCompletion = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds + 300.0 * (double)fadeOffset;
		}

		public override void Show()
		{
			if (PathingModule.Instance.Settings.PackInfoDisplayMode.get_Value() != MarkerInfoDisplayMode.NeverDisplay && !_showing)
			{
				((Control)this).Show();
				_showing = true;
				TriggerFade();
			}
		}

		public void Hide(bool withFade)
		{
			_showing = false;
			if (withFade)
			{
				((Control)this).Hide();
			}
			else
			{
				((Control)this).Hide();
			}
		}

		public override void Hide()
		{
			_showing = false;
			TriggerFade();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			float fadeLerp = MathHelper.Clamp((float)((_fadeCompletion - gameTime.get_TotalGameTime().TotalMilliseconds) / 300.0), 0f, 1f);
			((Control)this).set_Opacity(_showing ? (1f - fadeLerp) : fadeLerp);
			if (!_showing && fadeLerp <= 0f)
			{
				((Control)this).set_Visible(false);
			}
			if (PathingModule.Instance.Settings.PackInfoDisplayMode.get_Value() == MarkerInfoDisplayMode.NeverDisplay)
			{
				((Control)this).Hide();
			}
			((Container)this).UpdateContainer(gameTime);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			if (((Rectangle)(ref _closeButtonBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				((Control)this).Hide();
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void RecalculateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			_closeButtonBounds = new Rectangle(((Control)this).get_Width() - 64, 45, 32, 32);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (PathingModule.Instance != null && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				((Control)this).Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).TriggerMouseInput(mouseEventType, ms);
			return null;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (PathingModule.Instance != null && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && PathingModule.Instance.Settings.PackInfoDisplayMode.get_Value() != MarkerInfoDisplayMode.WithoutBackground)
			{
				AlphaMaskEffect.SharedInstance.SetEffectState(_croppedMask);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _croppedWindow, bounds, Color.get_White() * 0.9f);
				AlphaMaskEffect.SharedInstance.SetEffectState(Textures.get_Pixel());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _windowClose, _closeButtonBounds);
			}
		}
	}
}