using System;
using Blish_HUD;
using Blish_HUD.Common.Gw2;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class SmallInteract : Control
	{
		private const int DRAW_WIDTH = 64;

		private const int DRAW_HEIGHT = 64;

		private const float LEFT_OFFSET = 0.62f;

		private const float TOP_OFFSET = 0.58f;

		private const double SUBTLE_DELAY = 0.103;

		private const double SUBTLE_DAMPER = 0.05;

		private readonly AsyncTexture2D _interact1 = AsyncTexture2D.FromAssetId(102390);

		private Vector3 _lastPlayerPosition = Vector3.get_Zero();

		private double _subtleTimer;

		private double _showStart;

		private Color _tint = Color.get_White();

		public event EventHandler Interacted;

		public SmallInteract()
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).set_Size(new Point(64, 64));
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if (((Control)this).get_Visible() && (int)e.get_Key() == 70)
			{
				this.Interacted?.Invoke(this, EventArgs.Empty);
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public void ShowInteract(string interactMessage)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			ShowInteract((ITooltipView)new BasicTooltipView(string.Format(interactMessage, "[" + KeyBindings.Interact.GetBindingDisplayText() + "]")));
		}

		public void ShowInteract(string interactMessage, Color tint)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			ShowInteract((ITooltipView)new BasicTooltipView(string.Format(interactMessage, "[" + KeyBindings.Interact.GetBindingDisplayText() + "]")), tint);
		}

		public void ShowInteract(ITooltipView tooltipView, Color tint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			_tint = tint;
			_showStart = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds;
			((Control)this).set_Tooltip(new Tooltip(tooltipView));
			((Control)this).set_Visible(true);
		}

		public void ShowInteract(ITooltipView tooltipView)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			ShowInteract(tooltipView, Color.FromNonPremultiplied(255, 142, 50, 255));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			this.Interacted?.Invoke(this, EventArgs.Empty);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_Position() != _lastPlayerPosition || GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				_lastPlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				_subtleTimer = gameTime.get_TotalGameTime().TotalSeconds;
			}
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).set_Location(new Point((int)((float)((Control)((Control)this).get_Parent()).get_Width() * 0.5f), (int)((float)((Control)((Control)this).get_Parent()).get_Height() * 0.5f)));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				float baseOpacity = 0.4f;
				double tCTS = Math.Max(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _subtleTimer, 0.05);
				float opacity = (((Control)this).get_MouseOver() ? baseOpacity : 0.3f) + (float)Math.Min((tCTS / 0.103 - 0.05) * 0.6000000238418579, 0.6000000238418579);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_interact1), RectangleExtension.OffsetBy(bounds, 32, 32), (Rectangle?)null, _tint * opacity, Math.Min((float)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _showStart) * 20f, (float)Math.PI * 2f), new Vector2(32f, 32f), (SpriteEffects)0);
			}
		}
	}
}
