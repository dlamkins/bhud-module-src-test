using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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
				SetProperty(ref _Text, value, invalidateLayout: false, "Text");
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
				SetProperty(ref _TextColor, value, invalidateLayout: false, "TextColor");
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
				SetProperty(ref _timerText, value, invalidateLayout: false, "TimerText");
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
				SetProperty(ref _timerTextColor, value, invalidateLayout: false, "TimerTextColor");
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
					_icon.TextureSwapped -= _textureSwapEventHandler;
				}
				if (SetProperty(ref _icon, value, invalidateLayout: false, "Icon"))
				{
					_icon.TextureSwapped += _textureSwapEventHandler;
					RecalculateLayout();
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
				if (SetProperty(ref _maxFill, value, invalidateLayout: false, "MaxFill"))
				{
					RecalculateLayout();
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
				if (SetProperty(ref _currentFill, Math.Min(value, _maxFill), invalidateLayout: false, "CurrentFill"))
				{
					_animFill?.Cancel();
					_animFill = null;
					_animFill = Control.Animation.Tweener.Tween(this, new
					{
						DisplayedFill = _currentFill
					}, TimersModule.ModuleInstance.Resources.TICKINTERVAL, 0f, overwrite: false);
					RecalculateLayout();
				}
				if (_currentFill >= _maxFill)
				{
					_scrollEffect?.Enable();
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
				SetProperty(ref _fillColor, value, invalidateLayout: false, "FillColor");
			}
		}

		public bool ShouldShow { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public float DisplayedFill { get; set; }

		public AlertPanel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(DEFAULT_ALERTPANEL_WIDTH, DEFAULT_ALERTPANEL_HEIGHT);
			_iconSize = DEFAULT_ALERTPANEL_HEIGHT;
			_scrollEffect = new SimpleScrollingHighlightEffect(this)
			{
				Enabled = false
			};
			base.EffectBehind = _scrollEffect;
			_textureSwapEventHandler = delegate
			{
				RecalculateLayout();
			};
			base.Opacity = 0f;
			_animFade = Control.Animation.Tweener.Tween(this, new
			{
				Opacity = 1f
			}, TimersModule.ModuleInstance._alertFadeDelaySetting.Value).Repeat().Reflect();
			_animFade?.OnComplete(delegate
			{
				_animFade.Pause();
				if (base.Opacity <= 0f)
				{
					base.Visible = false;
					_scrollEffect?.Disable();
				}
				else if (_currentFill >= _maxFill)
				{
					_scrollEffect?.Enable();
				}
				if (_shouldDispose)
				{
					_icon?.Dispose();
					base.Dispose();
				}
			});
		}

		protected override CaptureType CapturesInput()
		{
			if (!TimersModule.ModuleInstance._lockAlertContainerSetting.Value)
			{
				return base.CapturesInput();
			}
			return CaptureType.DoNotBlock;
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
			_iconSize = base.Height;
			_iconBounds = new Rectangle(0, 0, _iconSize, _iconSize);
			_fillPercent = ((!TimersModule.ModuleInstance._alertFillDirection.Value) ? (1f - ((_maxFill > 0f) ? (_currentFill / _maxFill) : 1f)) : ((_maxFill > 0f) ? (_currentFill / _maxFill) : 1f));
			_fillHeight = (float)_iconSize * _fillPercent;
			if (_icon != null)
			{
				_topIconSource = new Rectangle(0, 0, _icon.Texture.get_Width(), _icon.Texture.get_Height() - (int)((float)_icon.Texture.get_Height() * _fillPercent));
				_bottomIconSource = new Rectangle(0, _icon.Texture.get_Height() - (int)((float)_icon.Texture.get_Height() * _fillPercent), _icon.Texture.get_Width(), (int)((float)_icon.Texture.get_Height() * _fillPercent));
			}
			_topIconDestination = new Rectangle(0, 0, _iconSize, _iconSize - (int)_fillHeight);
			_bottomIconDestination = new Rectangle(0, _iconSize - (int)_fillHeight, _iconSize, (int)_fillHeight);
			_fillDestination = new Rectangle(0, (int)((float)_iconSize - _fillHeight), _iconSize, (int)_fillHeight);
			_fillCrestDestination = new Rectangle(0, _iconSize - (int)_fillHeight, _iconSize, _iconSize);
			_timerTextDestination = new Rectangle(0, 0, _iconSize, (int)((float)_iconSize * 0.99f));
			_alertTextDestination = new Rectangle(_iconSize + 16, 0, _size.X - _iconSize - 35, base.Height);
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
			if (!base.Visible || !ShouldShow)
			{
				return;
			}
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black() * 0.2f);
			if (_icon != null)
			{
				if (_fillPercent < 1f)
				{
					spriteBatch.DrawOnCtrl(this, _icon, _topIconDestination, _topIconSource, Color.get_DarkGray() * 0.4f);
				}
				if (_fillPercent > 0f)
				{
					spriteBatch.DrawOnCtrl((Control)this, (Texture2D)_icon, _bottomIconDestination, (Rectangle?)_bottomIconSource);
				}
			}
			if (_fillPercent > 0f)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _fillDestination, _fillColor * 0.3f);
				if (_fillPercent < 0.99f)
				{
					spriteBatch.DrawOnCtrl(this, TimersModule.ModuleInstance.Resources.TextureFillCrest, _fillCrestDestination);
				}
			}
			spriteBatch.DrawOnCtrl(this, TimersModule.ModuleInstance.Resources.TextureVignette, _iconBounds);
			if (!string.IsNullOrEmpty(_timerText))
			{
				spriteBatch.DrawStringOnCtrl(this, _timerText ?? "", Control.Content.DefaultFont32, _timerTextDestination, TimerTextColor, wrap: false, stroke: true, 1, HorizontalAlignment.Center);
			}
			spriteBatch.DrawStringOnCtrl(this, _Text, TimersModule.ModuleInstance.Resources.Font, _alertTextDestination, TextColor, wrap: true, stroke: true);
		}

		public new void Dispose()
		{
			_shouldDispose = true;
			_animFade?.Resume();
			_animFade?.OnComplete(delegate
			{
				base.Dispose();
			});
		}
	}
}
