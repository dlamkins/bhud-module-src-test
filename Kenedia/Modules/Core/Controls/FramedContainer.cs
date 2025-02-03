using System;
using System.Collections.Generic;
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

		private Func<string> _setLocalizedTooltip;

		protected DateTime LastInteraction;

		private bool _fadeOut;

		private double _fadeTickDuration;

		private double _fadeTick;

		private double _fadeDelay = 2500.0;

		private double _fadeDuration = 500.0;

		private double _fadePerMs;

		private int _fadeSteps = 200;

		private Rectangle _backgroundBounds = Rectangle.get_Empty();

		private RectangleDimensions _contentPadding = new RectangleDimensions(0);

		private RectangleDimensions _borderWidth = new RectangleDimensions(0);

		public bool FadeOut
		{
			get
			{
				return _fadeOut;
			}
			set
			{
				_fadeOut = value;
				base.Opacity = 1f;
			}
		}

		public double FadeDelay
		{
			get
			{
				return _fadeDelay;
			}
			set
			{
				_fadeDelay = value;
				RecalculateFading();
			}
		}

		public double FadeDuration
		{
			get
			{
				return _fadeDuration;
			}
			set
			{
				_fadeDuration = value;
				RecalculateFading();
			}
		}

		public int FadeSteps
		{
			get
			{
				return _fadeSteps;
			}
			set
			{
				_fadeSteps = value;
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

		public Color? BackgroundImageColor { get; set; } = Color.get_White();


		public Color? BackgroundImageHoveredColor { get; set; }

		public new Color? BackgroundColor { get; set; }

		public Color? BackgroundHoveredColor { get; set; }

		public Rectangle? TextureRectangle { get; set; }

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public FramedContainer()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
			RecalculateFading();
		}

		public override void RecalculateLayout()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_contentRegion = new Rectangle(_contentPadding.Left + BorderWidth.Left, _contentPadding.Top + BorderWidth.Top, base.Width - _contentPadding.Horizontal - BorderWidth.Horizontal - ((WidthSizingMode == SizingMode.AutoSize) ? base.AutoSizePadding.X : 0), base.Height - _contentPadding.Vertical - BorderWidth.Vertical - ((HeightSizingMode == SizingMode.AutoSize) ? base.AutoSizePadding.Y : 0));
			_backgroundBounds = new Rectangle(Math.Max(BorderWidth.Left - 2, 0), Math.Max(BorderWidth.Top - 2, 0), base.Width - Math.Max(BorderWidth.Horizontal - 4, 0), base.Height - Math.Max(BorderWidth.Vertical - 4, 0));
			CalculateBorders();
		}

		private void CalculateBorders()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			_topBorders.Clear();
			_leftBorders.Clear();
			_bottomBorders.Clear();
			_rightBorders.Clear();
			Rectangle r = default(Rectangle);
			((Rectangle)(ref r))._002Ector(-1, 0, base.Width + 2, 0);
			int strength = BorderWidth.Top;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> topBorders = _topBorders;
				((Rectangle)(ref r))._002Ector(0, 0, base.Width, 1);
				topBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> topBorders2 = _topBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1);
				topBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> topBorders3 = _topBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 1, strength - fadeLines);
			topBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> topBorders4 = _topBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1);
				topBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_topBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(-1, -1, 0, base.Height + 2);
			strength = BorderWidth.Left;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> leftBorders = _leftBorders;
				((Rectangle)(ref r))._002Ector(0, 0, 1, base.Height);
				leftBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> leftBorders2 = _leftBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				leftBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> leftBorders3 = _leftBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, strength - fadeLines, r.Height - 2);
			leftBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> leftBorders4 = _leftBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				leftBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_leftBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(base.Width, -1, 0, base.Height + 2);
			strength = BorderWidth.Right;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> rightBorders = _rightBorders;
				((Rectangle)(ref r))._002Ector(base.Width - 1, 0, 1, base.Height);
				rightBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> rightBorders2 = _rightBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				rightBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> rightBorders3 = _rightBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - (strength - fadeLines), ((Rectangle)(ref r)).get_Top() + 1, strength - fadeLines, r.Height - 2);
			rightBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> rightBorders4 = _rightBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				rightBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_rightBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(-1, base.Height, base.Width + 2, 2);
			strength = BorderWidth.Bottom;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> bottomBorders = _bottomBorders;
				((Rectangle)(ref r))._002Ector(0, base.Height - 1, base.Width, 1);
				bottomBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> bottomBorders2 = _bottomBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1);
				bottomBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> bottomBorders3 = _bottomBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - (strength - fadeLines), r.Width - 2, strength - fadeLines);
			bottomBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> bottomBorders4 = _bottomBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1);
				bottomBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_bottomBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1), 0.5f));
			}
		}

		private void DrawBorders(SpriteBatch spriteBatch)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			Color? borderColor = ((HoveredBorderColor.HasValue && base.MouseOver) ? HoveredBorderColor : BorderColor);
			if (!borderColor.HasValue)
			{
				return;
			}
			foreach (var r4 in _topBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r4.Item1, Rectangle.get_Empty(), borderColor.Value * r4.Item2);
			}
			foreach (var r3 in _leftBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r3.Item1, Rectangle.get_Empty(), borderColor.Value * r3.Item2);
			}
			foreach (var r2 in _bottomBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r2.Item1, Rectangle.get_Empty(), borderColor.Value * r2.Item2);
			}
			foreach (var r in _rightBorders)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r.Item1, Rectangle.get_Empty(), borderColor.Value * r.Item2);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!FadeOut || !base.Visible || !(DateTime.Now.Subtract(LastInteraction).TotalMilliseconds >= FadeDelay))
			{
				return;
			}
			double timeSinceTick = gameTime.get_TotalGameTime().TotalMilliseconds - _fadeTick;
			if (timeSinceTick >= _fadeTickDuration)
			{
				base.Opacity -= (float)(_fadePerMs * ((_fadeTick == 0.0) ? _fadeTickDuration : timeSinceTick));
				_fadeTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (base.Opacity <= 0f)
				{
					Hide();
					_fadeTick = 0.0;
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			Color? backgroundColor = ((BackgroundHoveredColor.HasValue && base.MouseOver) ? BackgroundHoveredColor : BackgroundColor);
			if (backgroundColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgroundBounds, Rectangle.get_Empty(), backgroundColor.Value);
			}
			Color? backgroundImageColor = ((BackgroundImageHoveredColor.HasValue && base.MouseOver) ? BackgroundImageHoveredColor : BackgroundImageColor);
			if (BackgroundImage != null && backgroundImageColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, BackgroundImage, _backgroundBounds, (Rectangle)(((_003F?)TextureRectangle) ?? BackgroundImage.Bounds), backgroundImageColor.Value);
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
