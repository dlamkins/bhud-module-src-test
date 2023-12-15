using System;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Tooltips;
using Blish_HUD;
using Blish_HUD.Common.Gw2;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class SmallInteract : Control
	{
		private const int DRAW_WIDTH = 64;

		private const int DRAW_HEIGHT = 64;

		private const float LEFT_OFFSET = 0.62f;

		private const float TOP_OFFSET = 0.58f;

		private const double SUBTLE_DELAY = 0.103;

		private const double SUBTLE_DAMPER = 0.05;

		private static readonly Texture2D _interact1 = PathingModule.Instance.ContentsManager.GetTexture("png/controls/102390.png");

		private readonly IRootPackState _packState;

		private Vector3 _lastPlayerPosition = Vector3.get_Zero();

		private double _subtleTimer;

		private double _showStart;

		private Color _tint = Color.get_White();

		private IPathingEntity _activePathingEntity;

		public SmallInteract(IRootPackState packState)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			_packState = packState;
			((Control)this).set_Visible(false);
			((Control)this).set_Size(new Point(64, 64));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public void ShowInteract(IPathingEntity pathingEntity, string interactMessage)
		{
			ShowInteract(pathingEntity, (ITooltipView)(object)new BasicTooltipView(string.Format(interactMessage, "[" + KeyBindings.Interact.GetBindingDisplayText() + "]")));
		}

		public void ShowInteract(IPathingEntity pathingEntity, string interactMessage, Color tint)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			ShowInteract(pathingEntity, (ITooltipView)(object)new BasicTooltipView(string.Format(interactMessage, "[" + KeyBindings.Interact.GetBindingDisplayText() + "]")), tint);
		}

		public void ShowInteract(IPathingEntity pathingEntity, ITooltipView tooltipView, Color tint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			_tint = tint;
			_activePathingEntity = pathingEntity;
			if (_packState.UserResourceStates.Advanced.InteractGearAnimation)
			{
				_showStart = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds;
			}
			((Control)this).set_Tooltip(new Tooltip(tooltipView));
			((Control)this).set_Visible(true);
		}

		public void ShowInteract(IPathingEntity pathingEntity, ITooltipView tooltipView)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (!pathingEntity.BehaviorFiltered)
			{
				ShowInteract(pathingEntity, tooltipView, Color.FromNonPremultiplied(255, 142, 50, 255));
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			_activePathingEntity?.Interact(autoTriggered: false);
		}

		public void DisconnectInteract(IPathingEntity pathingEntity)
		{
			if (_activePathingEntity == pathingEntity)
			{
				_activePathingEntity = null;
				((Control)this).set_Visible(false);
				((Control)this).set_Tooltip((Tooltip)null);
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_Position() != _lastPlayerPosition || GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				_lastPlayerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				_subtleTimer = gameTime.get_TotalGameTime().TotalSeconds;
			}
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).set_Location(new Point((int)((float)((Control)((Control)this).get_Parent()).get_Width() * _packState.UserResourceStates.Advanced.InteractGearXOffset), (int)((float)((Control)((Control)this).get_Parent()).get_Height() * _packState.UserResourceStates.Advanced.InteractGearYOffset)));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && _packState.UserConfiguration.PackAllowInteractIcon.get_Value() && _activePathingEntity != null && !_activePathingEntity.IsFiltered(EntityRenderTarget.World))
			{
				float baseOpacity = 0.4f;
				double tCTS = Math.Max(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _subtleTimer, 0.05);
				float opacity = (((Control)this).get_MouseOver() ? baseOpacity : 0.3f) + (float)Math.Min((tCTS / 0.103 - 0.05) * 0.6000000238418579, 0.6000000238418579);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _interact1, RectangleExtension.OffsetBy(bounds, 32, 32), (Rectangle?)null, _tint * opacity, Math.Min((float)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _showStart) * 20f, (float)Math.PI * 2f), new Vector2(32f, 32f), (SpriteEffects)0);
			}
		}
	}
}
