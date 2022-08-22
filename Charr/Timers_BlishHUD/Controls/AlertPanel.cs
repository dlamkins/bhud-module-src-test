using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Charr.Timers_BlishHUD.Controls.Effects;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Controls
{
	public class AlertPanel : FlowPanel, IAlertPanel, IDisposable
	{
		public static int DEFAULT_ALERTPANEL_WIDTH = 320;

		public static int DEFAULT_ALERTPANEL_HEIGHT = 128;

		private AsyncTexture2D _icon;

		private readonly Tween _animFade;

		private bool _shouldDispose;

		private float _maxFill;

		private float _currentFill;

		private Tween _animFill;

		private Color _fillColor = Color.get_LightGray();

		private string _Text;

		private Color _TextColor = Color.get_White();

		private string _timerText;

		private Color _timerTextColor = Color.get_White();

		private int _iconSize;

		private Rectangle _iconBounds;

		private float _fillPercent;

		private float _fillHeight;

		private Rectangle _topIconSource;

		private Rectangle _topIconDestination;

		private Rectangle _bottomIconSource;

		private Rectangle _bottomIconDestination;

		private Rectangle _fillDestination;

		private Rectangle _fillCrestDestination;

		private Rectangle _timerTextDestination;

		private Rectangle _alertTextDestination;

		private readonly EventHandler<ValueChangedEventArgs<Texture2D>> _textureSwapEventHandler;

		private readonly SimpleScrollingHighlightEffect _scrollEffect;

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _Text, value, false, "Text");
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _TextColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _TextColor, value, false, "TextColor");
			}
		}

		public string TimerText
		{
			get
			{
				return _timerText;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _timerText, value, false, "TimerText");
			}
		}

		public Color TimerTextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _timerTextColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _timerTextColor, value, false, "TimerTextColor");
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				if (_icon != null)
				{
					_icon.remove_TextureSwapped(_textureSwapEventHandler);
				}
				if (((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, false, "Icon"))
				{
					_icon.add_TextureSwapped(_textureSwapEventHandler);
					((Control)this).RecalculateLayout();
				}
			}
		}

		public float MaxFill
		{
			get
			{
				return _maxFill;
			}
			set
			{
				if (((Control)this).SetProperty<float>(ref _maxFill, value, false, "MaxFill"))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public float CurrentFill
		{
			get
			{
				return _currentFill;
			}
			set
			{
				if (((Control)this).SetProperty<float>(ref _currentFill, Math.Min(value, _maxFill), false, "CurrentFill"))
				{
					Tween animFill = _animFill;
					if (animFill != null)
					{
						animFill.Cancel();
					}
					_animFill = null;
					_animFill = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<AlertPanel>(this, (object)new
					{
						DisplayedFill = _currentFill
					}, TimersModule.ModuleInstance.Resources.TICKINTERVAL, 0f, false);
					((Control)this).RecalculateLayout();
				}
				if (_currentFill >= _maxFill)
				{
					SimpleScrollingHighlightEffect scrollEffect = _scrollEffect;
					if (scrollEffect != null)
					{
						((ControlEffect)scrollEffect).Enable();
					}
				}
			}
		}

		public Color FillColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _fillColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _fillColor, value, false, "FillColor");
			}
		}

		public bool ShouldShow { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public float DisplayedFill { get; set; }

		public AlertPanel()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(DEFAULT_ALERTPANEL_WIDTH, DEFAULT_ALERTPANEL_HEIGHT));
			_iconSize = DEFAULT_ALERTPANEL_HEIGHT;
			SimpleScrollingHighlightEffect simpleScrollingHighlightEffect = new SimpleScrollingHighlightEffect((Control)(object)this);
			((ControlEffect)simpleScrollingHighlightEffect).set_Enabled(false);
			_scrollEffect = simpleScrollingHighlightEffect;
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
			_textureSwapEventHandler = delegate
			{
				((Control)this).RecalculateLayout();
			};
			((Control)this).set_Opacity(0f);
			_animFade = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<AlertPanel>(this, (object)new
			{
				Opacity = 1f
			}, TimersModule.ModuleInstance._alertFadeDelaySetting.get_Value(), 0f, true).Repeat(-1).Reflect();
			Tween animFade = _animFade;
			if (animFade == null)
			{
				return;
			}
			animFade.OnComplete((Action)delegate
			{
				_animFade.Pause();
				if (((Control)this).get_Opacity() <= 0f)
				{
					((Control)this).set_Visible(false);
					SimpleScrollingHighlightEffect scrollEffect = _scrollEffect;
					if (scrollEffect != null)
					{
						((ControlEffect)scrollEffect).Disable();
					}
				}
				else if (_currentFill >= _maxFill)
				{
					SimpleScrollingHighlightEffect scrollEffect2 = _scrollEffect;
					if (scrollEffect2 != null)
					{
						((ControlEffect)scrollEffect2).Enable();
					}
				}
				if (_shouldDispose)
				{
					AsyncTexture2D icon = _icon;
					if (icon != null)
					{
						icon.Dispose();
					}
					((Control)this).Dispose();
				}
			});
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (!TimersModule.ModuleInstance._lockAlertContainerSetting.get_Value())
			{
				return ((Container)this).CapturesInput();
			}
			return (CaptureType)22;
		}

		public override void RecalculateLayout()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			_iconSize = ((Control)this).get_Height();
			_iconBounds = new Rectangle(0, 0, _iconSize, _iconSize);
			_fillPercent = ((!TimersModule.ModuleInstance._alertFillDirection.get_Value()) ? (1f - ((_maxFill > 0f) ? (_currentFill / _maxFill) : 1f)) : ((_maxFill > 0f) ? (_currentFill / _maxFill) : 1f));
			_fillHeight = (float)_iconSize * _fillPercent;
			if (_icon != null)
			{
				_topIconSource = new Rectangle(0, 0, _icon.get_Texture().get_Width(), _icon.get_Texture().get_Height() - (int)((float)_icon.get_Texture().get_Height() * _fillPercent));
				_bottomIconSource = new Rectangle(0, _icon.get_Texture().get_Height() - (int)((float)_icon.get_Texture().get_Height() * _fillPercent), _icon.get_Texture().get_Width(), (int)((float)_icon.get_Texture().get_Height() * _fillPercent));
			}
			_topIconDestination = new Rectangle(0, 0, _iconSize, _iconSize - (int)_fillHeight);
			_bottomIconDestination = new Rectangle(0, _iconSize - (int)_fillHeight, _iconSize, (int)_fillHeight);
			_fillDestination = new Rectangle(0, (int)((float)_iconSize - _fillHeight), _iconSize, (int)_fillHeight);
			_fillCrestDestination = new Rectangle(0, _iconSize - (int)_fillHeight, _iconSize, _iconSize);
			_timerTextDestination = new Rectangle(0, 0, _iconSize, (int)((float)_iconSize * 0.99f));
			_alertTextDestination = new Rectangle(_iconSize + 16, 0, ((Control)this)._size.X - _iconSize - 35, ((Control)this).get_Height());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || !ShouldShow)
			{
				return;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.2f);
			if (_icon != null)
			{
				if (_fillPercent < 1f)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _topIconDestination, (Rectangle?)_topIconSource, Color.get_DarkGray() * 0.4f);
				}
				if (_fillPercent > 0f)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _bottomIconDestination, (Rectangle?)_bottomIconSource);
				}
			}
			if (_fillPercent > 0f)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _fillDestination, _fillColor * 0.3f);
				if (_fillPercent < 0.99f)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TimersModule.ModuleInstance.Resources.TextureFillCrest, _fillCrestDestination);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TimersModule.ModuleInstance.Resources.TextureVignette, _iconBounds);
			if (!string.IsNullOrEmpty(_timerText))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _timerText ?? "", Control.get_Content().get_DefaultFont32(), _timerTextDestination, TimerTextColor, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _Text, TimersModule.ModuleInstance.Resources.Font, _alertTextDestination, TextColor, true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public void Dispose()
		{
			_shouldDispose = true;
			Tween animFade = _animFade;
			if (animFade != null)
			{
				animFade.Resume();
			}
			Tween animFade2 = _animFade;
			if (animFade2 != null)
			{
				animFade2.OnComplete((Action)delegate
				{
					((Control)this).Dispose();
				});
			}
		}
	}
}
