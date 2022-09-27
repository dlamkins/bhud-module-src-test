using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Regions_Of_Tyria.UI.Controls
{
	internal sealed class MapNotification : Container
	{
		private static readonly BitmapFont SmallFont;

		private static readonly BitmapFont MediumFont;

		private const int TopMargin = 20;

		private const int StrokeDist = 1;

		private const int UnderlineSize = 1;

		private static readonly Color BrightGold;

		private const int NotificationCooldownMs = 2000;

		private static DateTime _lastNotificationTime;

		private static readonly SynchronizedCollection<MapNotification> ActiveMapNotifications;

		private IEnumerable<string> _headerLines;

		private string _header;

		private IEnumerable<string> _textLines;

		private string _text;

		private float _showDuration;

		private float _fadeInDuration;

		private float _fadeOutDuration;

		private Tween _animFadeLifecycle;

		private int _targetTop;

		public string Header
		{
			get
			{
				return _header;
			}
			set
			{
				_headerLines = value?.Split(new string[1] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ForEach((string x) => x.Trim());
				((Control)this).SetProperty<string>(ref _header, value, false, "Header");
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_textLines = value?.Split(new string[1] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ForEach((string x) => x.Trim());
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
			}
		}

		public float ShowDuration
		{
			get
			{
				return _showDuration;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _showDuration, value, false, "ShowDuration");
			}
		}

		public float FadeInDuration
		{
			get
			{
				return _fadeInDuration;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _fadeInDuration, value, false, "FadeInDuration");
			}
		}

		public float FadeOutDuration
		{
			get
			{
				return _fadeOutDuration;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _fadeOutDuration, value, false, "FadeOutDuration");
			}
		}

		static MapNotification()
		{
			_lastNotificationTime = DateTime.Now;
			ActiveMapNotifications = new SynchronizedCollection<MapNotification>();
			SmallFont = Control.get_Content().GetFont((FontFace)0, (FontSize)24, (FontStyle)0);
			MediumFont = Control.get_Content().GetFont((FontFace)0, (FontSize)36, (FontStyle)0);
			BrightGold = new Color(223, 194, 149, 255);
		}

		private MapNotification(string header, string text, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f)
			: this()
		{
			_showDuration = showDuration;
			_fadeInDuration = fadeInDuration;
			_fadeOutDuration = fadeOutDuration;
			Text = text;
			Header = header;
			((Control)this).set_ClipsBounds(true);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_ZIndex(30);
			((Control)this).set_Location(new Point(0, 0));
			_targetTop = ((Control)this).get_Top();
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		private void UpdateLocation(object o, ResizedEventArgs e)
		{
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_Location(new Point(0, 0));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			int height = (int)(RegionsOfTyriaModule.ModuleInstance.VerticalPositionSetting.get_Value() / 100f * (float)bounds.Height);
			if (!string.IsNullOrEmpty(Header) && !Header.Equals(Text, StringComparison.InvariantCultureIgnoreCase))
			{
				foreach (string headerLine in _headerLines)
				{
					Size2 size = SmallFont.MeasureString(headerLine);
					int lineWidth = (int)size.Width;
					int lineHeight = (int)size.Height;
					Rectangle rect = new Rectangle(0, 20 + height, bounds.Width, bounds.Height);
					height += SmallFont.LetterSpacing + lineHeight;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, headerLine, SmallFont, rect, BrightGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
					rect = new Rectangle((bounds.Width - (lineWidth + 2)) / 2, rect.Y + lineHeight + 2, lineWidth + 2, 3);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.Black * 0.8f);
					rect = new Rectangle(rect.X + 1, rect.Y + 1, lineWidth, 1);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, BrightGold);
				}
				height += 20;
			}
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			foreach (string textLine in _textLines)
			{
				Rectangle rect = new Rectangle(0, 20 + height, bounds.Width, bounds.Height);
				height += MediumFont.LetterSpacing + (int)MediumFont.MeasureString(textLine).Height;
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, textLine, MediumFont, rect, BrightGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
			}
		}

		public override void Show()
		{
			_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
			{
				Opacity = 1f
			}, FadeInDuration, 0f, true).OnComplete((Action)delegate
			{
				_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
				{
					Opacity = 1f
				}, ShowDuration, 0f, true).OnComplete((Action)delegate
				{
					_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
					{
						Opacity = 0f
					}, FadeOutDuration, 0f, true).OnComplete((Action)((Control)this).Dispose);
				});
			});
			((Control)this).Show();
		}

		private void SlideDown(int distance)
		{
			_targetTop += distance;
			((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
			{
				Top = _targetTop
			}, FadeOutDuration, 0f, true);
			if (!(((Control)this)._opacity < 1f))
			{
				_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
				{
					Opacity = 0f
				}, FadeOutDuration, 0f, true).OnComplete((Action)((Control)this).Dispose);
			}
		}

		protected override void DisposeControl()
		{
			ActiveMapNotifications.Remove(this);
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
			((Container)this).DisposeControl();
		}

		public static void ShowNotification(string header, string footer, Texture2D icon = null, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f)
		{
			if (DateTime.Now.Subtract(_lastNotificationTime).TotalMilliseconds < 2000.0)
			{
				return;
			}
			_lastNotificationTime = DateTime.Now;
			MapNotification mapNotification = new MapNotification(header, footer, showDuration, fadeInDuration, fadeOutDuration);
			((Control)mapNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			MapNotification nNot = mapNotification;
			((Control)nNot).set_ZIndex(ActiveMapNotifications.DefaultIfEmpty(nNot).Max((MapNotification n) => ((Control)n).get_ZIndex()) + 1);
			foreach (MapNotification activeMapNotification in ActiveMapNotifications)
			{
				activeMapNotification.SlideDown((int)((float)(SmallFont.LineHeight + MediumFont.LineHeight) + 21f));
			}
			ActiveMapNotifications.Add(nNot);
			((Control)nNot).Show();
		}
	}
}
