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
	internal class AngryPepe : Control
	{
		private Texture2D _pepeTex;

		private SoundEffect _soundEffect;

		private float _ragePercent;

		public AngryPepe()
			: this()
		{
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/angrypepe/reeee.wav");
			_pepeTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/angrypepe/angry-pepe.png");
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
			Texture2D pepeTex = _pepeTex;
			if (pepeTex != null)
			{
				((GraphicsResource)pepeTex).Dispose();
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
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<AngryPepe>(this, (object)new
			{
				_ragePercent = 1f
			}, 3.5f, 0.02f, true).OnComplete((Action)((Control)this).Dispose);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				Point val = PointExtensions.ResizeKeepAspect(new Point(_pepeTex.get_Width(), _pepeTex.get_Height()), (int)Math.Round(0.8f * (float)bounds.Width), (int)Math.Round(0.8f * (float)bounds.Height), false);
				int width = val.X;
				int height = val.Y;
				int centerX = (bounds.X + bounds.Width - width) / 2 + (int)Math.Round(_ragePercent * (float)RandomUtil.GetRandom(-100, 100));
				int centerY = (bounds.Y + bounds.Height - height) / 2 + (int)Math.Round(_ragePercent * (float)RandomUtil.GetRandom(-100, 100));
				Rectangle pepeBounds = default(Rectangle);
				((Rectangle)(ref pepeBounds))._002Ector(centerX, centerY, width, height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _pepeTex, pepeBounds);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(bounds.X, bounds.Y, bounds.Width, bounds.Height + 1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Red() * 0.5f * _ragePercent);
			}
		}
	}
}
