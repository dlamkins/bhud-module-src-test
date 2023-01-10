using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Controls.BigWigs
{
	public class BigWigAlert : Control, IAlertPanel, IDisposable
	{
		private const int DEFAULT_WIDTH = 336;

		private const int ICON_SIZE = 32;

		private const int TIMER_SIZE = 48;

		private const int TOP_BORDER = 2;

		private const int BOTTOM_BORDER = 1;

		private float _maxFill;

		private float _currentFill;

		private string _text;

		private Rectangle _iconBounds = Rectangle.get_Empty();

		private Rectangle _progressBounds = Rectangle.get_Empty();

		private Rectangle _filledBounds = Rectangle.get_Empty();

		private Rectangle _timerBounds = Rectangle.get_Empty();

		public float MaxFill
		{
			get
			{
				return _maxFill;
			}
			set
			{
				SetProperty(ref _maxFill, value, invalidateLayout: true, "MaxFill");
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
				SetProperty(ref _currentFill, value, invalidateLayout: true, "CurrentFill");
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
				value = value.Replace("\n", " - ");
				SetProperty(ref _text, value, invalidateLayout: true, "Text");
			}
		}

		public string TimerText { get; set; }

		public Color TextColor { get; set; }

		public Color FillColor { get; set; }

		public Color TimerTextColor { get; set; }

		public bool ShouldShow { get; set; }

		public AsyncTexture2D Icon { get; set; }

		protected override CaptureType CapturesInput()
		{
			if (!TimersModule.ModuleInstance._lockAlertContainerSetting.Value)
			{
				return base.CapturesInput();
			}
			return CaptureType.DoNotBlock;
		}

		public BigWigAlert()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(336, 35);
			Icon = new AsyncTexture2D(ContentService.Textures.Error);
		}

		public override void RecalculateLayout()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			_iconBounds = new Rectangle(2, 2, 32, 32);
			_timerBounds = new Rectangle(base.Width - 48, 0, 48, base.Height);
			int progressLeft = 35;
			int progressFill = (int)((float)_progressBounds.Width * (CurrentFill / MaxFill));
			_progressBounds = new Rectangle(progressLeft, 2, base.Width - progressLeft - 1, base.Height - 2 - 1);
			_filledBounds = new Rectangle(_progressBounds.X, _progressBounds.Y, TimersModule.ModuleInstance._alertFillDirection.Value ? progressFill : (_progressBounds.Width - progressFill), _progressBounds.Height);
			base.RecalculateLayout();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			if (!ShouldShow)
			{
				return;
			}
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black() * 0.75f);
			spriteBatch.DrawOnCtrl((Control)this, (Texture2D)Icon, _iconBounds);
			spriteBatch.DrawOnCtrl(this, TimersModule.ModuleInstance.Resources.BigWigBackground, _progressBounds, _progressBounds.OffsetBy(TimersModule.ModuleInstance.Resources.BigWigBackground.get_Width() / 3, 0), Color.get_White() * 0.7f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _filledBounds, FillColor * 0.45f);
			spriteBatch.DrawStringOnCtrl(this, Text, GameService.Content.DefaultFont18, _progressBounds.OffsetBy(11, -1), Color.get_Black());
			spriteBatch.DrawStringOnCtrl(this, Text, GameService.Content.DefaultFont18, _progressBounds.OffsetBy(10, -2), TextColor);
			float remainingTime = MaxFill - CurrentFill;
			string timerFormat = "0";
			Color timerColor = TextColor;
			if (remainingTime > 0f)
			{
				if (!(remainingTime > 5f))
				{
					timerFormat = "0.0";
					timerColor = TimerTextColor;
				}
			}
			else if (!(remainingTime <= 0f) || remainingTime != -1f)
			{
				goto IL_01ac;
			}
			spriteBatch.DrawStringOnCtrl(this, remainingTime.ToString(timerFormat), GameService.Content.DefaultFont18, _timerBounds.OffsetBy(1, 1), Color.get_Black(), wrap: false, HorizontalAlignment.Center);
			spriteBatch.DrawStringOnCtrl(this, remainingTime.ToString(timerFormat), GameService.Content.DefaultFont18, _timerBounds, timerColor, wrap: false, HorizontalAlignment.Center);
			goto IL_01ac;
			IL_01ac:
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(_progressBounds.X, _progressBounds.Y, _progressBounds.Width, 1), Color.get_White() * 0.25f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(_progressBounds.X, ((Rectangle)(ref _progressBounds)).get_Bottom() - 1, _progressBounds.Width, 1), Color.get_White() * 0.25f);
		}
	}
}
