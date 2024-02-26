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
	internal class GrantTheftAuto : Control
	{
		private Color _textColor;

		private Texture2D _textTex;

		private Texture2D _photoTex;

		private Texture2D _flashTex;

		private SoundEffect _soundEffect;

		private float _textOpacityPercent;

		private float _flashOpacityPercent;

		private float _photoOpacityPercent;

		private float _bgOpacityPercent;

		public GrantTheftAuto()
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			_textColor = new Color(149, 31, 32);
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/gta/gta_wasted.wav");
			_textTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/gta/" + GameService.Overlay.get_UserLocale().get_Value().SupportedOrDefault()
				.Code() + "-wasted.png");
			_photoTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/gta/photo-vignette.png");
			_flashTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/gta/flash-vignette.png");
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
			Texture2D photoTex = _photoTex;
			if (photoTex != null)
			{
				((GraphicsResource)photoTex).Dispose();
			}
			Texture2D flashTex = _flashTex;
			if (flashTex != null)
			{
				((GraphicsResource)flashTex).Dispose();
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
				soundEffect.Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
			}
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<GrantTheftAuto>(this, (object)new
			{
				_photoOpacityPercent = 0.6f
			}, 0.15f, 0f, true).RepeatDelay(0.05f).Repeat(1)
				.Reflect();
			((TweenerImpl)GameService.Animation.get_Tweener()).Timer(2.25f, 0f).OnComplete((Action)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<GrantTheftAuto>(this, (object)new
				{
					_flashOpacityPercent = 0.6f
				}, 0.15f, 0f, true).RepeatDelay(0.05f).Repeat(1)
					.Reflect();
				((TweenerImpl)GameService.Animation.get_Tweener()).Timer(0.15f, 0f).OnComplete((Action)delegate
				{
					((TweenerImpl)GameService.Animation.get_Tweener()).Tween<GrantTheftAuto>(this, (object)new
					{
						_textOpacityPercent = 1f
					}, 0.008f, 0f, true).RepeatDelay(5f).Repeat(1)
						.Reflect();
				});
			});
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<GrantTheftAuto>(this, (object)new
			{
				_bgOpacityPercent = 1f
			}, 1f, 0f, true).RepeatDelay(6f).Repeat(1)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				int width = _textTex.get_Width();
				int height = _textTex.get_Height();
				int centerX = (bounds.X + bounds.Width - width) / 2;
				int centerY = (bounds.Y + bounds.Height - height) / 2;
				Rectangle textBounds = default(Rectangle);
				((Rectangle)(ref textBounds))._002Ector(centerX, centerY, width, height);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(bounds.X, bounds.Y, bounds.Width, bounds.Height + 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Black() * 0.7f * _bgOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _photoTex, rect, _textColor * _photoOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _flashTex, rect, _textColor * _flashOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textTex, textBounds, _textColor * _textOpacityPercent);
			}
		}
	}
}
