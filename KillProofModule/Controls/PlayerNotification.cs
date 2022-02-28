using System;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using KillProofModule.Controls.Views;
using KillProofModule.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillProofModule.Controls
{
	public class PlayerNotification : Container
	{
		private const int NOTIFICATION_WIDTH = 264;

		private const int NOTIFICATION_HEIGHT = 64;

		private static int _visibleNotifications;

		private Texture2D _notificationBackroundTexture;

		private AsyncTexture2D _icon;

		private Rectangle _layoutIconBounds;

		private PlayerNotification(PlayerProfile profile, string message)
			: this()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			PlayerNotification playerNotification = this;
			_notificationBackroundTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("ns-button.png");
			_icon = KillProofModule.ModuleInstance.GetProfessionRender(profile.Player);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(264, 64));
			((Control)this).set_Location(new Point(60, 60 + 79 * _visibleNotifications));
			((Control)this).set_BasicTooltipText("Click to view profile");
			string wrappedTitle = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), profile.AccountName, (float)(((Control)this).get_Width() - 64 - 20 - 32));
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(74, 0));
			((Control)val).set_Size(new Point(((Control)this).get_Width() - 64 - 10 - 32, ((Control)this).get_Height() / 2));
			val.set_Font(Control.get_Content().get_DefaultFont14());
			val.set_Text(wrappedTitle);
			string wrapped = DrawUtil.WrapText(Control.get_Content().get_DefaultFont14(), message, (float)(((Control)this).get_Width() - 64 - 20 - 32));
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(74, ((Control)this).get_Height() / 2));
			((Control)val2).set_Size(new Point(((Control)this).get_Width() - 64 - 10 - 32, ((Control)this).get_Height() / 2));
			val2.set_Text(wrapped);
			_visibleNotifications++;
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)GameService.Overlay.get_BlishHudWindow()).Show();
				MainView.LoadProfileView(profile.AccountName);
				((Control)playerNotification).Dispose();
			});
			profile.PlayerChanged += delegate(object _, ValueEventArgs<Player> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				playerNotification._icon = KillProofModule.ModuleInstance.GetProfessionRender(e.get_Value());
			};
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			int icoSize = 52;
			_layoutIconBounds = new Rectangle(32 - icoSize / 2, 32 - icoSize / 2, icoSize, icoSize);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _notificationBackroundTexture, bounds, Color.get_White() * 0.85f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutIconBounds);
		}

		private void Show(float duration)
		{
			Control.get_Content().PlaySoundEffectByName("audio/color-change");
			((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<PlayerNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay(duration)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		public static void ShowNotification(PlayerProfile profile, string message, float duration)
		{
			PlayerNotification playerNotification = new PlayerNotification(profile, message);
			((Control)playerNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			playerNotification.Show(duration);
		}

		protected override void DisposeControl()
		{
			_visibleNotifications--;
			((Container)this).DisposeControl();
		}
	}
}
