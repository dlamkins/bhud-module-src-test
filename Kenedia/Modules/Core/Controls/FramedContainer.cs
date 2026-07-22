using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class FramedContainer : Container, ILocalizable
	{
		private readonly List<(Rectangle, float)> _leftBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _topBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _rightBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _bottomBorders = new List<(Rectangle, float)>();

		protected DateTime LastInteraction;

		private double _fadeTickDuration;

		private double _fadeTick;

		private double _fadePerMs;

		private Rectangle _backgroundBounds = Rectangle.Empty;

		private RectangleDimensions _contentPadding = new RectangleDimensions(0);

		private RectangleDimensions _borderWidth = new RectangleDimensions(0);

		public bool FadeOut
		{
			[CompilerGenerated]
			get
			{
				return _003CFadeOut_003Ek__BackingField;
			}
			set
			{
				_003CFadeOut_003Ek__BackingField = value;
				base.Opacity = 1f;
			}
		}

		public double FadeDelay
		{
			[CompilerGenerated]
			get
			{
				return _003CFadeDelay_003Ek__BackingField;
			}
			set
			{
				_003CFadeDelay_003Ek__BackingField = value;
				RecalculateFading();
			}
		}

		public double FadeDuration
		{
			[CompilerGenerated]
			get
			{
				return _003CFadeDuration_003Ek__BackingField;
			}
			set
			{
				_003CFadeDuration_003Ek__BackingField = value;
				RecalculateFading();
			}
		}

		public int FadeSteps
		{
			[CompilerGenerated]
			get
			{
				return _003CFadeSteps_003Ek__BackingField;
			}
			set
			{
				_003CFadeSteps_003Ek__BackingField = value;
				RecalculateFading();
			}
		}

		public RectangleDimensions BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
				RecalculateLayout();
			}
		}

		public RectangleDimensions ContentPadding
		{
			get
			{
				return _contentPadding;
			}
			set
			{
				_contentPadding = value;
				RecalculateLayout();
			}
		}

		public Color? BorderColor { get; set; }

		public Color? HoveredBorderColor { get; set; }

		public AsyncTexture2D BackgroundImage { get; set; }

		public Color? BackgroundImageColor { get; set; }

		public Color? BackgroundImageHoveredColor { get; set; }

		public new Color? BackgroundColor { get; set; }

		public Color? BackgroundHoveredColor { get; set; }

		public Rectangle? TextureRectangle { get; set; }

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public FramedContainer()
		{
			_003CFadeDelay_003Ek__BackingField = 2500.0;
			_003CFadeDuration_003Ek__BackingField = 500.0;
			_003CFadeSteps_003Ek__BackingField = 200;
			BackgroundImageColor = Color.White;
			base._002Ector();
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
			RecalculateFading();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_contentRegion = new Rectangle(_contentPadding.Left + BorderWidth.Left, _contentPadding.Top + BorderWidth.Top, base.Width - _contentPadding.Horizontal - BorderWidth.Horizontal - ((WidthSizingMode == SizingMode.AutoSize) ? base.AutoSizePadding.X : 0), base.Height - _contentPadding.Vertical - BorderWidth.Vertical - ((HeightSizingMode == SizingMode.AutoSize) ? base.AutoSizePadding.Y : 0));
			_backgroundBounds = new Rectangle(Math.Max(BorderWidth.Left - 2, 0), Math.Max(BorderWidth.Top - 2, 0), base.Width - Math.Max(BorderWidth.Horizontal - 4, 0), base.Height - Math.Max(BorderWidth.Vertical - 4, 0));
			CalculateBorders();
		}

		private void CalculateBorders()
		{
			_topBorders.Clear();
			_leftBorders.Clear();
			_bottomBorders.Clear();
			_rightBorders.Clear();
			Rectangle r = new Rectangle(-1, 0, base.Width + 2, 0);
			int strength = BorderWidth.Top;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> topBorders = _topBorders;
				r = new Rectangle(0, 0, base.Width, 1);
				topBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> topBorders2 = _topBorders;
				r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1);
				topBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> topBorders3 = _topBorders;
			r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 1, strength - fadeLines);
			topBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> topBorders4 = _topBorders;
				r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1);
				topBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_topBorders.Add((new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1), 0.5f));
			}
			r = new Rectangle(-1, -1, 0, base.Height + 2);
			strength = BorderWidth.Left;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> leftBorders = _leftBorders;
				r = new Rectangle(0, 0, 1, base.Height);
				leftBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> leftBorders2 = _leftBorders;
				r = new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2);
				leftBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> leftBorders3 = _leftBorders;
			r = new Rectangle(r.Right, r.Top + 1, strength - fadeLines, r.Height - 2);
			leftBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> leftBorders4 = _leftBorders;
				r = new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2);
				leftBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_leftBorders.Add((new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2), 0.5f));
			}
			r = new Rectangle(base.Width, -1, 0, base.Height + 2);
			strength = BorderWidth.Right;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> rightBorders = _rightBorders;
				r = new Rectangle(base.Width - 1, 0, 1, base.Height);
				rightBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> rightBorders2 = _rightBorders;
				r = new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2);
				rightBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> rightBorders3 = _rightBorders;
			r = new Rectangle(r.Left - (strength - fadeLines), r.Top + 1, strength - fadeLines, r.Height - 2);
			rightBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> rightBorders4 = _rightBorders;
				r = new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2);
				rightBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_rightBorders.Add((new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2), 0.5f));
			}
			r = new Rectangle(-1, base.Height, base.Width + 2, 2);
			strength = BorderWidth.Bottom;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> bottomBorders = _bottomBorders;
				r = new Rectangle(0, base.Height - 1, base.Width, 1);
				bottomBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> bottomBorders2 = _bottomBorders;
				r = new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1);
				bottomBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> bottomBorders3 = _bottomBorders;
			r = new Rectangle(r.Left + 1, r.Top - (strength - fadeLines), r.Width - 2, strength - fadeLines);
			bottomBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> bottomBorders4 = _bottomBorders;
				r = new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1);
				bottomBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_bottomBorders.Add((new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1), 0.5f));
			}
		}

		private void DrawBorders(SpriteBatch spriteBatch)
		{
			Color? borderColor = ((HoveredBorderColor.HasValue && base.MouseOver) ? HoveredBorderColor : BorderColor);
			if (!borderColor.HasValue)
			{
				return;
			}
			foreach (var r4 in _topBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r4.Item1, Rectangle.Empty, borderColor.Value * r4.Item2);
			}
			foreach (var r3 in _leftBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r3.Item1, Rectangle.Empty, borderColor.Value * r3.Item2);
			}
			foreach (var r2 in _bottomBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r2.Item1, Rectangle.Empty, borderColor.Value * r2.Item2);
			}
			foreach (var r in _rightBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r.Item1, Rectangle.Empty, borderColor.Value * r.Item2);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!FadeOut || !base.Visible || !(DateTime.Now.Subtract(LastInteraction).TotalMilliseconds >= FadeDelay))
			{
				return;
			}
			double timeSinceTick = gameTime.TotalGameTime.TotalMilliseconds - _fadeTick;
			if (timeSinceTick >= _fadeTickDuration)
			{
				base.Opacity -= (float)(_fadePerMs * ((_fadeTick == 0.0) ? _fadeTickDuration : timeSinceTick));
				_fadeTick = gameTime.TotalGameTime.TotalMilliseconds;
				if (base.Opacity <= 0f)
				{
					Hide();
					_fadeTick = 0.0;
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			Color? backgroundColor = ((BackgroundHoveredColor.HasValue && base.MouseOver) ? BackgroundHoveredColor : BackgroundColor);
			if (backgroundColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgroundBounds, Rectangle.Empty, backgroundColor.Value);
			}
			Color? backgroundImageColor = ((BackgroundImageHoveredColor.HasValue && base.MouseOver) ? BackgroundImageHoveredColor : BackgroundImageColor);
			if (BackgroundImage != null && backgroundImageColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, BackgroundImage, _backgroundBounds, TextureRectangle ?? BackgroundImage.Bounds, backgroundImageColor.Value);
			}
			DrawBorders(spriteBatch);
		}

		public virtual void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
			SetInteracted();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			SetInteracted();
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			if (FadeOut)
			{
				base.Opacity = 1f;
			}
		}

		protected void SetInteracted()
		{
			LastInteraction = DateTime.Now;
			base.Opacity = 1f;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			LocalizingService.LocaleChanged -= new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		private void RecalculateFading()
		{
			_fadeTickDuration = FadeDuration / (double)FadeSteps;
			_fadePerMs = 1.0 / (double)FadeSteps / _fadeTickDuration;
		}
	}
}
