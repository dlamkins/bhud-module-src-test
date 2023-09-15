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
		public enum RevealEffect
		{
			Decode,
			Dissolve
		}

		private const int TOP_MARGIN = 20;

		private const int STROKE_DIST = 1;

		private const int UNDERLINE_SIZE = 1;

		private static readonly Color _brightGold;

		private static readonly Color _darkGold;

		private const int NOTIFICATION_COOLDOWN_MS = 2000;

		private static DateTime _lastNotificationTime;

		private static readonly SynchronizedCollection<MapNotification> _activeMapNotifications;

		private static readonly BitmapFont _krytanFont;

		private static readonly BitmapFont _krytanFontSmall;

		private static readonly BitmapFont _titlingFont;

		private static readonly BitmapFont _titlingFontSmall;

		private static SpriteBatchParameters _defaultParams;

		private IEnumerable<string> _headerLines;

		private string _header;

		private IEnumerable<string> _textLines;

		private string _text;

		private float _showDuration;

		private float _fadeInDuration;

		private float _fadeOutDuration;

		private float _effectDuration;

		private Tween _animFadeLifecycle;

		private int _targetTop;

		private SpriteBatchParameters _dissolve;

		private SpriteBatchParameters _reveal;

		private float _amount;

		static MapNotification()
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			_lastNotificationTime = DateTime.UtcNow;
			_activeMapNotifications = new SynchronizedCollection<MapNotification>();
			_krytanFont = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/NewKrytan.ttf", 46);
			_krytanFontSmall = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/NewKrytan.ttf", 34, 30);
			_titlingFont = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/StoweTitling.ttf", 36);
			_titlingFontSmall = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/StoweTitling.ttf", 24);
			_defaultParams = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_brightGold = new Color(223, 194, 149, 255);
			_darkGold = new Color(168, 150, 135, 255);
		}

		public static void ShowNotification(string header, string footer, Texture2D icon = null, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f, float effectDuration = 0.85f)
		{
			if (DateTime.UtcNow.Subtract(_lastNotificationTime).TotalMilliseconds < 2000.0)
			{
				return;
			}
			_lastNotificationTime = DateTime.UtcNow;
			MapNotification mapNotification = new MapNotification(header, footer, showDuration, fadeInDuration, fadeOutDuration, effectDuration);
			((Control)mapNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			MapNotification nNot = mapNotification;
			((Control)nNot).set_ZIndex(_activeMapNotifications.DefaultIfEmpty(nNot).Max((MapNotification n) => ((Control)n).get_ZIndex()) + 1);
			foreach (MapNotification activeMapNotification in _activeMapNotifications)
			{
				activeMapNotification.SlideDown((int)((float)(_titlingFontSmall.get_LineHeight() + _titlingFont.get_LineHeight()) + 21f));
			}
			_activeMapNotifications.Add(nNot);
			((Control)nNot).Show();
		}

		private MapNotification(string header, string text, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f, float effectDuration = 0.85f)
			: this()
		{
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			_showDuration = showDuration;
			_fadeInDuration = fadeInDuration;
			_fadeOutDuration = fadeOutDuration;
			_effectDuration = effectDuration;
			_text = text;
			_textLines = text?.Split(new string[1] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ForEach((string x) => x.Trim());
			_header = header;
			_headerLines = header?.Split(new string[1] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ForEach((string x) => x.Trim());
			((Control)this).set_ClipsBounds(true);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_ZIndex(30);
			((Control)this).set_Location(new Point(0, 0));
			_targetTop = ((Control)this).get_Top();
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(RegionsOfTyria.Instance.ContentsManager.GetEffect("effects/dissolve.mgfx"));
			_dissolve = val;
			SpriteBatchParameters val2 = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val2.set_Effect(RegionsOfTyria.Instance.ContentsManager.GetEffect("effects/dissolve.mgfx"));
			_reveal = val2;
			Vector4 burnColor = default(Vector4);
			((Vector4)(ref burnColor))._002Ector(0.5f, 0.25f, 0f, 0.5f);
			_dissolve.get_Effect().get_Parameters().get_Item("Amount")
				.SetValue(0f);
			_dissolve.get_Effect().get_Parameters().get_Item("GlowColor")
				.SetValue(burnColor);
			_dissolve.get_Effect().get_Parameters().get_Item("Slide")
				.SetValue(true);
			_reveal.get_Effect().get_Parameters().get_Item("Amount")
				.SetValue(1f);
			_reveal.get_Effect().get_Parameters().get_Item("GlowColor")
				.SetValue(burnColor);
			_reveal.get_Effect().get_Parameters().get_Item("Slide")
				.SetValue(true);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		private void UpdateLocation(object o, ResizedEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_Location(new Point(0, 0));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			if (RegionsOfTyria.Instance != null)
			{
				bool slide = RegionsOfTyria.Instance.RevealEffectSetting.get_Value() == RevealEffect.Decode;
				_dissolve.get_Effect().get_Parameters().get_Item("Slide")
					.SetValue(slide);
				_reveal.get_Effect().get_Parameters().get_Item("Slide")
					.SetValue(slide);
				_dissolve.get_Effect().get_Parameters().get_Item("Opacity")
					.SetValue(((Control)this).get_Opacity());
				_reveal.get_Effect().get_Parameters().get_Item("Opacity")
					.SetValue(((Control)this).get_Opacity());
				_dissolve.get_Effect().get_Parameters().get_Item("Amount")
					.SetValue(_amount);
				_reveal.get_Effect().get_Parameters().get_Item("Amount")
					.SetValue(1f - _amount);
				spriteBatch.End();
				if (RegionsOfTyria.Instance.TranslateSetting.get_Value())
				{
					SpriteBatchExtensions.Begin(spriteBatch, _dissolve);
					PaintText(spriteBatch, bounds, _krytanFont, _krytanFontSmall, underline: false);
					spriteBatch.End();
				}
				SpriteBatchExtensions.Begin(spriteBatch, _reveal);
				PaintText(spriteBatch, bounds, _titlingFont, _titlingFontSmall, underline: false);
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, _defaultParams);
			}
		}

		private void PaintText(SpriteBatch spriteBatch, Rectangle bounds, BitmapFont font, BitmapFont smallFont, bool underline)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			int height = (int)(RegionsOfTyria.Instance.VerticalPositionSetting.get_Value() / 100f * (float)bounds.Height);
			if (!string.IsNullOrEmpty(_header) && !_header.Equals(_text, StringComparison.InvariantCultureIgnoreCase))
			{
				foreach (string headerLine in _headerLines)
				{
					Size2 val = smallFont.MeasureString(headerLine);
					int lineWidth = (int)val.Width;
					int lineHeight = (int)val.Height;
					Rectangle rect = new Rectangle(0, 20 + height, bounds.Width, bounds.Height);
					height += smallFont.get_LineHeight();
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, headerLine, smallFont, rect, _darkGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
					if (underline)
					{
						rect = new Rectangle((bounds.Width - (lineWidth + 2)) / 2, rect.Y + lineHeight + 5, lineWidth + 2, 3);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, Color.get_Black() * 0.8f);
						rect = new Rectangle(rect.X + 1, rect.Y + 1, lineWidth, 1);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, _darkGold);
					}
				}
				height += 20;
			}
			if (string.IsNullOrEmpty(_text))
			{
				return;
			}
			foreach (string textLine in _textLines)
			{
				Rectangle rect = new Rectangle(0, 20 + height, bounds.Width, bounds.Height);
				height += font.get_LineHeight();
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, textLine, font, rect, _brightGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
			}
		}

		public override void Show()
		{
			_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
			{
				Opacity = 1f
			}, _fadeInDuration, 0f, true).OnComplete((Action)delegate
			{
				_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
				{
					_amount = 1f
				}, _effectDuration, 0f, true).OnComplete((Action)delegate
				{
					_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
					{
						Opacity = 1f
					}, _showDuration, 0f, true).OnComplete((Action)delegate
					{
						_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
						{
							Opacity = 0f
						}, _fadeOutDuration, 0f, true).OnComplete((Action)((Control)this).Dispose);
					});
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
			}, _fadeOutDuration, 0f, true);
			if (!(((Control)this)._opacity < 1f))
			{
				_animFadeLifecycle = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
				{
					Opacity = 0f
				}, _fadeOutDuration, 0f, true).OnComplete((Action)((Control)this).Dispose);
			}
		}

		protected override void DisposeControl()
		{
			Effect effect = _reveal.get_Effect();
			if (effect != null)
			{
				((GraphicsResource)effect).Dispose();
			}
			Effect effect2 = _dissolve.get_Effect();
			if (effect2 != null)
			{
				((GraphicsResource)effect2).Dispose();
			}
			_activeMapNotifications.Remove(this);
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
			((Container)this).DisposeControl();
		}
	}
}
