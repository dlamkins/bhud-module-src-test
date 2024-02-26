using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.FailScreens.Core.UI.Controls.Screens
{
	internal class RytlocksCritterRampage : Control
	{
		private class TextureData : IDisposable
		{
			private Texture _spriteSheet;

			private int _totalFrames;

			private int _framesPerRow;

			private int _framesPerColumn;

			private Point[] _frameSizes;

			private int _currentFrame;

			public TextureData(Texture2D spriteSheet, int totalFrames, int framesPerRow, int framesPerColumn, params Point[] frameSizes)
			{
				_spriteSheet = (Texture)(object)spriteSheet;
				_totalFrames = totalFrames;
				_framesPerRow = framesPerRow;
				_framesPerColumn = framesPerColumn;
				_frameSizes = frameSizes;
			}

			public int Column(int currentFrame)
			{
				return currentFrame % _framesPerRow;
			}

			public int Row(int currentFrame)
			{
				return currentFrame / _framesPerRow;
			}

			public void DrawOnCtrl(Control ctrl, Rectangle destinationRectangle)
			{
			}

			public void Dispose()
			{
				Texture spriteSheet = _spriteSheet;
				if (spriteSheet != null)
				{
					((GraphicsResource)spriteSheet).Dispose();
				}
			}
		}

		private Texture2D _sadRytlockSheet0;

		private SoundEffect _soundEffect;

		private float _spriteOpacityPercent;

		private float _bgOpacityPercent;

		public RytlocksCritterRampage()
			: this()
		{
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/rytlock/new_music_sadending.wav");
			_sadRytlockSheet0 = FailScreensModule.Instance.ContentsManager.GetTexture("screens/rytlock/sad_rytlock-sheet0.png");
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
			Texture2D sadRytlockSheet = _sadRytlockSheet0;
			if (sadRytlockSheet != null)
			{
				((GraphicsResource)sadRytlockSheet).Dispose();
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
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<RytlocksCritterRampage>(this, (object)new
			{
				_spriteOpacityPercent = 1f
			}, 1f, 0f, true).RepeatDelay(7f).Repeat(1)
				.Reflect();
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<RytlocksCritterRampage>(this, (object)new
			{
				_bgOpacityPercent = 1f
			}, 1f, 0f, true).RepeatDelay(8f).Repeat(1)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(bounds.X, bounds.Y, bounds.Width, bounds.Height + 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Black() * 0.7f * _bgOpacityPercent);
				Point frameOrigin = AnimationUtil.Animate(6, 2, 190, 153, 5);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _sadRytlockSheet0, new Rectangle(bounds.Width / 2 - 190, bounds.Height / 2, 380, 306), (Rectangle?)new Rectangle(frameOrigin.X, frameOrigin.Y, 190, 153), Color.get_White() * _spriteOpacityPercent);
			}
		}
	}
}
