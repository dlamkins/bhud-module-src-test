using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD._Extensions;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorf.BH.TTBlockersStuff.UI
{
	internal class TimerBar : Control
	{
		public const int Husks = 0;

		public const int Eggs = 1;

		private static readonly Texture2D textureBarTop = Module.Instance.ContentsManager.GetTexture("middle_top.png");

		private static readonly Texture2D textureBarBottom = Module.Instance.ContentsManager.GetTexture("middle_bottom.png");

		private static readonly Texture2D textureBarLeftSide = Module.Instance.ContentsManager.GetTexture("side_left.png");

		private static readonly Texture2D textureBarRightSide = Module.Instance.ContentsManager.GetTexture("right_side.png");

		private static readonly Texture2D textureBarGradient = Module.Instance.ContentsManager.GetTexture("bar_gradient.png");

		private Microsoft.Xna.Framework.Rectangle layoutBarLeftSide;

		private Microsoft.Xna.Framework.Rectangle layourBarRightSide;

		private Microsoft.Xna.Framework.Rectangle layoutBarBackground;

		private Microsoft.Xna.Framework.Rectangle layoutBarTop;

		private Microsoft.Xna.Framework.Rectangle layoutBarBottom;

		private Microsoft.Xna.Framework.Rectangle layoutBarGradient;

		protected float maxValue = 100f;

		protected float minValue;

		protected float value = 100f;

		protected string barText;

		protected int barIndex;

		public float MaxValue
		{
			get
			{
				return maxValue;
			}
			set
			{
				if (SetProperty(ref maxValue, value, invalidateLayout: false, "MaxValue"))
				{
					Value = this.value;
				}
			}
		}

		public float MinValue
		{
			get
			{
				return minValue;
			}
			set
			{
				if (SetProperty(ref minValue, value, invalidateLayout: false, "MinValue"))
				{
					Value = this.value;
				}
			}
		}

		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				SetProperty(ref this.value, MathHelper.Clamp(value, minValue, maxValue), invalidateLayout: true, "Value");
			}
		}

		public string BarText
		{
			get
			{
				return barText;
			}
			set
			{
				SetProperty(ref barText, value, invalidateLayout: false, "BarText");
			}
		}

		public event EventHandler<MouseEventArgs> InternalClick;

		public TimerBar(int barIndex)
		{
			base.Size = new Point(256, 16);
			this.barIndex = barIndex;
		}

		public override void RecalculateLayout()
		{
			layoutBarLeftSide = new Microsoft.Xna.Framework.Rectangle(0, 0, textureBarLeftSide.Width, base.Height);
			layourBarRightSide = new Microsoft.Xna.Framework.Rectangle(base.Width - textureBarRightSide.Width, 0, textureBarRightSide.Width, base.Height);
			layoutBarTop = new Microsoft.Xna.Framework.Rectangle(0, 0, base.Width, textureBarTop.Height);
			layoutBarBottom = new Microsoft.Xna.Framework.Rectangle(0, base.Height - textureBarBottom.Height, base.Width, textureBarTop.Height);
			float valueOffset = (Value - MinValue) / (MaxValue - MinValue) * (float)(base.Width - textureBarLeftSide.Width);
			layoutBarBackground = new Microsoft.Xna.Framework.Rectangle(textureBarLeftSide.Width, 2, (int)valueOffset - textureBarLeftSide.Width, base.Height - 4);
			layoutBarGradient = new Microsoft.Xna.Framework.Rectangle((int)valueOffset - textureBarGradient.Width, 2, textureBarGradient.Width, base.Height - 4);
		}

		public void OnInternalClick(object sender, MouseEventArgs e)
		{
			this.InternalClick?.Invoke(sender, e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(237, 121, 38);
			if (Module.Instance.SettingsManager.ModuleSettings.TryGetSetting("colorPickerSettingTimerBar" + barIndex, out var newColor) && Value >= MaxValue)
			{
				color = (newColor as SettingEntry<Gw2Sharp.WebApi.V2.Models.Color>).Value.Cloth?.ToXnaColor() ?? color;
			}
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Microsoft.Xna.Framework.Color.Black * 0.3f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, layoutBarBackground, color);
			if (Value < MaxValue)
			{
				spriteBatch.DrawOnCtrl(this, textureBarGradient, layoutBarGradient);
			}
			spriteBatch.DrawStringOnCtrl(this, barText, GameService.Content.DefaultFont16, bounds, Microsoft.Xna.Framework.Color.White, wrap: false, HorizontalAlignment.Center);
			spriteBatch.DrawOnCtrl(this, textureBarTop, layoutBarTop);
			spriteBatch.DrawOnCtrl(this, textureBarBottom, layoutBarBottom);
			spriteBatch.DrawOnCtrl(this, textureBarLeftSide, layoutBarLeftSide);
			spriteBatch.DrawOnCtrl(this, textureBarRightSide, layourBarRightSide);
		}

		protected override void DisposeControl()
		{
			textureBarTop?.Dispose();
			textureBarBottom?.Dispose();
			textureBarLeftSide?.Dispose();
			textureBarRightSide?.Dispose();
			textureBarGradient?.Dispose();
			base.DisposeControl();
		}
	}
}
