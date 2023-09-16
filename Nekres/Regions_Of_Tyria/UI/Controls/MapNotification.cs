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

		private static BitmapFont _krytanFont;

		private static BitmapFont _krytanFontSmall;

		internal static BitmapFont TitlingFont;

		internal static BitmapFont TitlingFontSmall;

		private static SpriteBatchParameters _defaultParams;

		private string _header;

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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			_lastNotificationTime = DateTime.UtcNow;
			_activeMapNotifications = new SynchronizedCollection<MapNotification>();
			_defaultParams = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_brightGold = new Color(223, 194, 149, 255);
			_darkGold = new Color(168, 150, 135, 255);
		}

		public static void UpdateFonts(float fontSize = 0.92f)
		{
			int size = (int)Math.Round((fontSize + 0.35f) * 37f);
			_krytanFont = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/NewKrytan.ttf", size + 10);
			_krytanFontSmall = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/NewKrytan.ttf", size - 2, 30);
			TitlingFont = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/StoweTitling.ttf", size);
			TitlingFontSmall = RegionsOfTyria.Instance.ContentsManager.GetBitmapFont("fonts/StoweTitling.ttf", size - 12);
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
				activeMapNotification.SlideDown((int)((float)(TitlingFontSmall.get_LineHeight() + TitlingFont.get_LineHeight()) + 21f));
			}
			_activeMapNotifications.Add(nNot);
			((Control)nNot).Show();
		}

		private MapNotification(string header, string text, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f, float effectDuration = 0.85f)
			: this()
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			_showDuration = showDuration;
			_fadeInDuration = fadeInDuration;
			_fadeOutDuration = fadeOutDuration;
			_effectDuration = effectDuration;
			_text = text;
			_header = header;
			((Control)this).set_ClipsBounds(true);
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_ZIndex(30);
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
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			if (RegionsOfTyria.Instance != null)
			{
				bool slide = RegionsOfTyria.Instance.RevealEffect.get_Value() == RevealEffect.Decode;
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
				if (RegionsOfTyria.Instance.Translate.get_Value())
				{
					SpriteBatchExtensions.Begin(spriteBatch, _dissolve);
					PaintText((Control)(object)this, spriteBatch, bounds, _krytanFont, _krytanFontSmall, underline: false, _header, _text);
					spriteBatch.End();
				}
				SpriteBatchExtensions.Begin(spriteBatch, _reveal);
				PaintText((Control)(object)this, spriteBatch, bounds, TitlingFont, TitlingFontSmall, underline: false, _header, _text);
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, _defaultParams);
			}
		}

		internal static void PaintText(Control ctrl, SpriteBatch spriteBatch, Rectangle bounds, BitmapFont font, BitmapFont smallFont, bool underline, string header, string text)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			int height = (int)(RegionsOfTyria.Instance.VerticalPosition.get_Value() / 100f * (float)bounds.Height);
			Rectangle rect = default(Rectangle);
			if (!string.IsNullOrEmpty(header) && !header.Equals(text, StringComparison.InvariantCultureIgnoreCase))
			{
				string str = header.Wrap();
				Size2 val = smallFont.MeasureString(str);
				int lineWidth = (int)val.Width;
				int lineHeight = (int)val.Height;
				((Rectangle)(ref rect))._002Ector(0, 20 + height, bounds.Width, bounds.Height);
				height += smallFont.get_LineHeight();
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, ctrl, str, smallFont, rect, _darkGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
				if (underline)
				{
					((Rectangle)(ref rect))._002Ector((bounds.Width - (lineWidth + 2)) / 2, rect.Y + lineHeight + 5, lineWidth + 2, 3);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, Color.get_Black() * 0.8f);
					((Rectangle)(ref rect))._002Ector(rect.X + 1, rect.Y + 1, lineWidth, 1);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, _darkGold);
				}
				height += 20;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((Rectangle)(ref rect))._002Ector(0, 20 + height, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, ctrl, text.Wrap(), font, rect, _brightGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
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
			if (!(((Control)this)._opacity < 1f))
			{
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
				{
					Top = _targetTop
				}, _fadeOutDuration, 0f, true);
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
