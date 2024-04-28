using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Nekres.FailScreens.Properties;

namespace Nekres.FailScreens.Core.UI.Controls.Screens
{
	internal class Sekiro : Control
	{
		private Color _kanjiFlashColor;

		private Color _textColor;

		private Texture2D _kanjiTex;

		private Texture2D _kanjiShadowTex;

		private SoundEffect _soundEffect;

		private float _textOpacityPercent;

		private float _kanjiFlashScalePercent = 1f;

		private float _kanjiFlashOpacityPercent;

		private float _bgOpacityPercent;

		private string _text;

		private BitmapFontEx _font;

		public Sekiro()
			: this()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			_textColor = new Color(177, 12, 16);
			_kanjiFlashColor = new Color(250, 120, 120);
			_text = BoldLetters((new string[3]
			{
				Resources.Death,
				"Yeet",
				"Lmaooooo"
			})[RandomUtil.GetRandom(0, 2)]);
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/sekiro/sekiro_death.wav");
			_kanjiTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/sekiro/sekiro_death.png");
			_kanjiShadowTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/sekiro/sekiro_death_out.png");
			_font = FailScreensModule.Instance.ContentsManager.GetBitmapFont("fonts/Athelas-Regular.ttf", 250);
			PlayAnimation();
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)OnPropertyChanged);
		}

		private string BoldLetters(string text)
		{
			return string.Join(" ", text.ToUpper().ToCharArray());
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
			Texture2D kanjiTex = _kanjiTex;
			if (kanjiTex != null)
			{
				((GraphicsResource)kanjiTex).Dispose();
			}
			Texture2D kanjiShadowTex = _kanjiShadowTex;
			if (kanjiShadowTex != null)
			{
				((GraphicsResource)kanjiShadowTex).Dispose();
			}
			_font?.Dispose();
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
			((TweenerImpl)GameService.Animation.get_Tweener()).Timer(4f, 0f).OnComplete((Action)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Sekiro>(this, (object)new
				{
					_textOpacityPercent = 0f
				}, 2f, 0f, true);
			});
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Sekiro>(this, (object)new
			{
				_kanjiFlashScalePercent = 1.05f
			}, 0.08f, 0f, true).Repeat(1).Reflect();
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Sekiro>(this, (object)new
			{
				_kanjiFlashOpacityPercent = 1f,
				_textOpacityPercent = 1f
			}, 0.08f, 0f, true).OnComplete((Action)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Sekiro>(this, (object)new
				{
					_kanjiFlashOpacityPercent = 0f
				}, 2f, 0f, true);
			});
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Sekiro>(this, (object)new
			{
				_bgOpacityPercent = 1f
			}, 1f, 0f, true).RepeatDelay(5f).Repeat(1)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				Size2 val = ((BitmapFont)_font).MeasureString(_text);
				int textWidth = (int)Math.Round(val.Width);
				int textHeight = (int)Math.Round(val.Height);
				int width = _kanjiTex.get_Width();
				int height = _kanjiTex.get_Height();
				int centerX = (bounds.X + bounds.Width - width) / 2;
				int centerY = (bounds.Y + bounds.Height - height - textHeight) / 2;
				Rectangle kanjiBounds = default(Rectangle);
				((Rectangle)(ref kanjiBounds))._002Ector(centerX, centerY, width, height);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(bounds.X, bounds.Y, bounds.Width, bounds.Height + 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Black() * 0.7f * _bgOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _kanjiTex, kanjiBounds, _textColor * _textOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _kanjiShadowTex, kanjiBounds, _textColor * _textOpacityPercent);
				width = (int)Math.Round((float)kanjiBounds.Width * _kanjiFlashScalePercent);
				height = (int)Math.Round((float)kanjiBounds.Height * _kanjiFlashScalePercent);
				centerX = (bounds.X + bounds.Width - width) / 2;
				centerY = (bounds.Y + bounds.Height - height - textHeight) / 2;
				((Rectangle)(ref kanjiBounds))._002Ector(centerX, centerY, width, height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _kanjiTex, kanjiBounds, _kanjiFlashColor * _kanjiFlashOpacityPercent);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _kanjiShadowTex, kanjiBounds, _kanjiFlashColor * _kanjiFlashOpacityPercent);
				Rectangle textBounds = default(Rectangle);
				((Rectangle)(ref textBounds))._002Ector((bounds.X + bounds.Width - textWidth) / 2, ((Rectangle)(ref kanjiBounds)).get_Bottom(), textWidth, textHeight);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, (BitmapFont)(object)_font, textBounds, _textColor * _textOpacityPercent, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
