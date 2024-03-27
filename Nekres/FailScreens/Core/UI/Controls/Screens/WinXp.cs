using System;
using System.Collections.Generic;
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
	internal class WinXp : Control
	{
		private Color _textColor;

		private Color _titleColor;

		private Texture2D _errorBoxTex;

		private SoundEffect _soundEffect;

		private List<Point> _errorBoxRands;

		private Tween _timer;

		private Color _blueScreenColor;

		private float _blueScreenOpacity;

		private string _blueScreenSmile;

		private bool _hideBoxes;

		private BitmapFontEx _smileFont;

		private BitmapFontEx _descFont;

		private BitmapFontEx _infoFont;

		private BitmapFontEx _boxFont;

		public WinXp()
			: this()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			_errorBoxRands = new List<Point>();
			_titleColor = Color.get_White();
			_textColor = Color.get_Black();
			_blueScreenColor = new Color(0, 121, 217);
			_blueScreenSmile = ":(";
			_soundEffect = FailScreensModule.Instance.ContentsManager.GetSound("screens/winxp/winxp-error.wav");
			_errorBoxTex = FailScreensModule.Instance.ContentsManager.GetTexture("screens/winxp/winxp-error.png");
			_smileFont = FailScreensModule.Instance.ContentsManager.GetBitmapFont("fonts/CONSOLA.TTF", 250);
			_descFont = FailScreensModule.Instance.ContentsManager.GetBitmapFont("fonts/CONSOLA.TTF", 40);
			_infoFont = FailScreensModule.Instance.ContentsManager.GetBitmapFont("fonts/CONSOLA.TTF", 20);
			_boxFont = FailScreensModule.Instance.ContentsManager.GetBitmapFont("fonts/CONSOLA.TTF", 16);
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
			Tween timer = _timer;
			if (timer != null)
			{
				timer.Cancel();
			}
			_smileFont?.Dispose();
			_descFont?.Dispose();
			_infoFont?.Dispose();
			_boxFont?.Dispose();
			SoundEffect soundEffect = _soundEffect;
			if (soundEffect != null)
			{
				soundEffect.Dispose();
			}
			Texture2D errorBoxTex = _errorBoxTex;
			if (errorBoxTex != null)
			{
				((GraphicsResource)errorBoxTex).Dispose();
			}
			((Control)this).DisposeControl();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		private void PlayAnimation()
		{
			_timer = ((TweenerImpl)GameService.Animation.get_Tweener()).Timer(0.2f, 0f).Repeat(10).OnRepeat((Action)CreateBox)
				.OnComplete((Action)delegate
				{
					_timer = ((TweenerImpl)GameService.Animation.get_Tweener()).Timer(0.15f, 0f).OnComplete((Action)delegate
					{
						SoundEffect soundEffect = _soundEffect;
						if (soundEffect != null)
						{
							soundEffect.Play(FailScreensModule.Instance.SoundVolume, 0f, 0f);
						}
						_timer = ((TweenerImpl)GameService.Animation.get_Tweener()).Timer(0.5f, 0f).OnComplete((Action)delegate
						{
							_blueScreenOpacity = 1f;
							_hideBoxes = true;
							_timer = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<WinXp>(this, (object)new
							{
								_blueScreenOpacity = 0f
							}, 2f, 1.25f, true).OnComplete((Action)((Control)this).Dispose);
						});
					});
				});
		}

		private void CreateBox()
		{
			SoundEffect soundEffect = _soundEffect;
			if (soundEffect != null)
			{
				soundEffect.Play(FailScreensModule.Instance.SoundVolume, 0f, 0f);
			}
			((TweenerImpl)GameService.Animation.get_Tweener()).Timer(0.5f, 0f).OnComplete((Action)delegate
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				_errorBoxRands.Add(new Point(RandomUtil.GetRandom(-500, 500), RandomUtil.GetRandom(-500, 500)));
			});
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() == null)
			{
				return;
			}
			if (!_hideBoxes)
			{
				foreach (Point box in _errorBoxRands)
				{
					DrawErrorBox(spriteBatch, bounds, box);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, _blueScreenColor * _blueScreenOpacity);
			Size2 size = ((BitmapFont)_smileFont).MeasureString(_blueScreenSmile);
			int centerX = (int)((float)(bounds.X + bounds.Width) - size.Width) / 3 - 70;
			int centerY = 240;
			Rectangle smileBounds = default(Rectangle);
			((Rectangle)(ref smileBounds))._002Ector(centerX, centerY, (int)size.Width, (int)size.Height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _blueScreenSmile, (BitmapFont)(object)_smileFont, smileBounds, Color.get_White() * _blueScreenOpacity, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			string desc = string.Format(Resources._0__ran_into_a_problem_and_needs_to_revive__We_re_just_collecting_some_tears__and_then_we_ll_grief_with_you_, GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
			Rectangle descBounds = default(Rectangle);
			((Rectangle)(ref descBounds))._002Ector(smileBounds.X + 10, smileBounds.Y + smileBounds.Height / 2, bounds.Width - smileBounds.X * 2 - 150, 500);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, desc, (BitmapFont)(object)_descFont, descBounds, Color.get_White() * _blueScreenOpacity, true, (HorizontalAlignment)0, (VerticalAlignment)1);
			Rectangle infoBounds = default(Rectangle);
			((Rectangle)(ref infoBounds))._002Ector(descBounds.X, descBounds.Y + 150, bounds.Width - descBounds.X * 2 - 150, 500);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Resources.For_more_information_about_this_issue_and_possible_fixes__bother_your_party_leader_, (BitmapFont)(object)_infoFont, infoBounds, Color.get_White() * _blueScreenOpacity, true, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private void DrawErrorBox(SpriteBatch spriteBatch, Rectangle bounds, Point rand)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			int width = _errorBoxTex.get_Width();
			int height = _errorBoxTex.get_Height();
			int centerX = (bounds.X + bounds.Width - width) / 2 + rand.X;
			int centerY = (bounds.Y + bounds.Height - height) / 2 + rand.Y;
			Rectangle textBounds = default(Rectangle);
			((Rectangle)(ref textBounds))._002Ector(centerX, centerY, width, height);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _errorBoxTex, textBounds, Color.get_White());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Resources.Error, (BitmapFont)(object)_boxFont, new Rectangle(centerX + 8, centerY + 6, 100, 20), _titleColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Resources.Fail, (BitmapFont)(object)_boxFont, new Rectangle(centerX + 50, centerY + 45, 100, 20), _textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
