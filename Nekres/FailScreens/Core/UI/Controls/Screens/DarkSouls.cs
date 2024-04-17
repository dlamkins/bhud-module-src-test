using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.FailScreens.Core.UI.Controls.Screens
{
	internal class DarkSouls : Control
	{
		private Color _textColor;

		private Texture2D _textTex;

		private SoundEffect _soundEffect;

		private float _textOpacityPercent;

		private float _textScalePercent = 1f;

		private float _bgOpacityPercent;

		public DarkSouls()
			: this()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			_textColor = new Color(149, 31, 32);
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/darksouls/darksouls_death.wav");
			_textTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/darksouls/" + GameService.Overlay.get_UserLocale().get_Value().SupportedOrDefault()
				.Code() + "-darksouls.png");
			PlayAnimation();
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)OnPropertyChanged);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (e.PropertyName.Equals("Parent") && ((Control)this).get_Parent() != null)
			{
				((Control)this).set_Size(((Control)((Control)this).get_Parent()).get_Size());
			}
		}

		protected override void DisposeControl()
		{
			SoundEffect soundEffect = _soundEffect;
			if (soundEffect != null)
			{
				soundEffect.Dispose();
			}
			Texture2D textTex = _textTex;
			if (textTex != null)
			{
				((GraphicsResource)textTex).Dispose();
			}
			((Control)this).DisposeControl();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		private void PlayAnimation()
		{
			SoundEffect soundEffect = _soundEffect;
			if (soundEffect != null)
			{
				soundEffect.Play(FailScreensModule.Instance.SoundVolume, 0f, 0f);
			}
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DarkSouls>(this, (object)new
			{
				_textOpacityPercent = 1f
			}, 2f, 0f, true).RepeatDelay(3f).Repeat(1)
				.Reflect();
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DarkSouls>(this, (object)new
			{
				_textScalePercent = 1.7f
			}, 7f, 0f, true);
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DarkSouls>(this, (object)new
			{
				_bgOpacityPercent = 1f
			}, 1f, 0f, true).RepeatDelay(6f).Repeat(1)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				int width = (int)Math.Round(_textScalePercent * (float)_textTex.get_Width());
				int height = (int)Math.Round(_textScalePercent * (float)_textTex.get_Height());
				int centerX = (bounds.X + bounds.Width - width) / 2;
				int centerY = (bounds.Y + bounds.Height - height) / 2;
				Rectangle textBounds = default(Rectangle);
				((Rectangle)(ref textBounds))._002Ector(centerX, centerY, width, height);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(bounds.X, bounds.Y, bounds.Width, bounds.Height + 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Black() * 0.7f * _bgOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textTex, textBounds, _textColor * _textOpacityPercent);
			}
		}
	}
}
