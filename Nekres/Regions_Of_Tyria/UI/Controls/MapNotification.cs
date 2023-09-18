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
		private const int TOP_MARGIN = 20;

		private const int STROKE_DIST = 1;

		private const int UNDERLINE_SIZE = 2;

		private static readonly Color _brightGold;

		private static readonly Color _darkGold;

		private const int NOTIFICATION_COOLDOWN_MS = 2000;

		private static DateTime _lastNotificationTime;

		private static readonly SynchronizedCollection<MapNotification> _activeMapNotifications;

		private static SpriteBatchParameters _defaultParams;

		private string _header;

		private string _text;

		private float _showDuration;

		private float _fadeInDuration;

		private float _fadeOutDuration;

		private float _effectDuration;

		private SpriteBatchParameters _decode;

		private SpriteBatchParameters _reveal;

		private int _targetTop;

		private float _amount;

		private bool _isFading;

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
			_darkGold = new Color(178, 160, 145, 255);
		}

		public static void ShowNotification(string header, string footer, float showDuration = 4f, float fadeInDuration = 2f, float fadeOutDuration = 2f, float effectDuration = 0.85f)
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
				activeMapNotification.SlideDown(150);
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
			//IL_00b1: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
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
			val.set_Effect(RegionsOfTyria.Instance.DissolveEffect.Clone());
			_decode = val;
			SpriteBatchParameters val2 = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val2.set_Effect(RegionsOfTyria.Instance.DissolveEffect.Clone());
			_reveal = val2;
			_decode.get_Effect().get_Parameters().get_Item("Amount")
				.SetValue(0f);
			_decode.get_Effect().get_Parameters().get_Item("Slide")
				.SetValue(true);
			_reveal.get_Effect().get_Parameters().get_Item("Amount")
				.SetValue(1f);
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
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			if (RegionsOfTyria.Instance != null)
			{
				_decode.get_Effect().get_Parameters().get_Item("Opacity")
					.SetValue(((Control)this).get_Opacity());
				_reveal.get_Effect().get_Parameters().get_Item("Opacity")
					.SetValue(((Control)this).get_Opacity());
				_decode.get_Effect().get_Parameters().get_Item("Amount")
					.SetValue(_amount);
				_reveal.get_Effect().get_Parameters().get_Item("Amount")
					.SetValue(1f - _amount);
				spriteBatch.End();
				if (_isFading)
				{
					_reveal.get_Effect().get_Parameters().get_Item("Slide")
						.SetValue(false);
				}
				else if (RegionsOfTyria.Instance.Translate.get_Value())
				{
					SpriteBatchExtensions.Begin(spriteBatch, _decode);
					PaintText((Control)(object)this, spriteBatch, bounds, RegionsOfTyria.Instance.KrytanFont, RegionsOfTyria.Instance.KrytanFontSmall, _header, _text, underline: false);
					spriteBatch.End();
				}
				SpriteBatchExtensions.Begin(spriteBatch, _reveal);
				PaintText((Control)(object)this, spriteBatch, bounds, RegionsOfTyria.Instance.TitlingFont, RegionsOfTyria.Instance.TitlingFontSmall, _header, _text, underline: true, _amount);
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, _defaultParams);
			}
		}

		internal static void PaintText(Control ctrl, SpriteBatch spriteBatch, Rectangle bounds, BitmapFont font, BitmapFont smallFont, string header, string text, bool underline = true, float deltaAmount = 1f)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			int height = (int)Math.Round(RegionsOfTyria.Instance.VerticalPosition.get_Value() / 100f * (float)bounds.Height);
			Rectangle rect = default(Rectangle);
			if (!string.IsNullOrEmpty(header) && !header.Equals(text, StringComparison.InvariantCultureIgnoreCase))
			{
				string str = header.Wrap();
				Size2 size = ((BitmapFont)smallFont).MeasureString(str);
				int lineHeight = (int)size.Height;
				if (underline)
				{
					int lineWidth = (int)Math.Round(deltaAmount * size.Width);
					((Rectangle)(ref rect))._002Ector((bounds.Width - lineWidth) / 2, height + lineHeight + 15, (lineWidth + 2) / 2, 4);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, Color.get_Black() * 0.8f);
					((Rectangle)(ref rect))._002Ector(rect.X + 1, rect.Y + 1, lineWidth / 2, 2);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, _darkGold);
					((Rectangle)(ref rect))._002Ector(bounds.Width / 2, height + lineHeight + 15, (lineWidth + 1) / 2, 4);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, Color.get_Black() * 0.8f);
					((Rectangle)(ref rect))._002Ector(rect.X - 1, rect.Y + 1, lineWidth / 2, 2);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), rect, _darkGold);
				}
				((Rectangle)(ref rect))._002Ector(0, height, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, ctrl, str, (BitmapFont)(object)smallFont, rect, _darkGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
				height += ((BitmapFont)font).get_LineHeight() * 2 + 20;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((Rectangle)(ref rect))._002Ector(0, height, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, ctrl, text.Wrap(), (BitmapFont)(object)font, rect, _brightGold, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
			}
		}

		public override void Show()
		{
			((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
			{
				Opacity = 1f
			}, _fadeInDuration, 0f, true).OnComplete((Action)delegate
			{
				((TweenerImpl)Control.get_Animation().get_Tweener()).Timer(0.2f, 0f).OnComplete((Action)delegate
				{
					((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
					{
						_amount = 1f
					}, _effectDuration, 0f, true).OnComplete((Action)delegate
					{
						((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, (object)new
						{
							Opacity = 1f
						}, _showDuration, 0f, true).OnComplete((Action)delegate
						{
							_isFading = true;
							((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<MapNotification>(this, RegionsOfTyria.Instance.Dissolve.get_Value() ? ((object)new
							{
								Opacity = 0.9f,
								_amount = 0f
							}) : ((object)new
							{
								Opacity = 0f
							}), _fadeOutDuration, 0f, true).OnComplete((Action)((Control)this).Dispose);
						});
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
					Opacity = 0f,
					Top = _targetTop
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
			Effect effect2 = _decode.get_Effect();
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
