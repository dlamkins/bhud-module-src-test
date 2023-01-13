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

		private readonly Texture2D textureBarTop = Module.Instance.ContentsManager.GetTexture("middle_top.png");

		private readonly Texture2D textureBarBottom = Module.Instance.ContentsManager.GetTexture("middle_bottom.png");

		private readonly Texture2D textureBarLeftSide = Module.Instance.ContentsManager.GetTexture("side_left.png");

		private readonly Texture2D textureBarRightSide = Module.Instance.ContentsManager.GetTexture("right_side.png");

		private readonly Texture2D textureBarGradient = Module.Instance.ContentsManager.GetTexture("bar_gradient.png");

		private Rectangle layoutBarLeftSide;

		private Rectangle layourBarRightSide;

		private Rectangle layoutBarBackground;

		private Rectangle layoutBarTop;

		private Rectangle layoutBarBottom;

		private Rectangle layoutBarGradient;

		protected float maxValue = 100f;

		protected float minValue;

		protected float value = 100f;

		protected string barText;

		protected Color textColor = Color.get_White();

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

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref textColor, value, invalidateLayout: false, "TextColor");
			}
		}

		public event EventHandler<MouseEventArgs> InternalClick;

		public TimerBar(int barIndex)
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(256, 16);
			this.barIndex = barIndex;
		}

		public override void RecalculateLayout()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			layoutBarLeftSide = new Rectangle(0, 0, textureBarLeftSide.get_Width(), base.Height);
			layourBarRightSide = new Rectangle(base.Width - textureBarRightSide.get_Width(), 0, textureBarRightSide.get_Width(), base.Height);
			layoutBarTop = new Rectangle(0, 0, base.Width, textureBarTop.get_Height());
			layoutBarBottom = new Rectangle(0, base.Height - textureBarBottom.get_Height(), base.Width, textureBarTop.get_Height());
			float valueOffset = (Value - MinValue) / (MaxValue - MinValue) * (float)(base.Width - textureBarLeftSide.get_Width());
			layoutBarBackground = new Rectangle(textureBarLeftSide.get_Width(), 2, (int)valueOffset - textureBarLeftSide.get_Width(), base.Height - 4);
			layoutBarGradient = new Rectangle((int)valueOffset - textureBarGradient.get_Width(), 2, textureBarGradient.get_Width(), base.Height - 4);
		}

		public void OnInternalClick(object sender, MouseEventArgs e)
		{
			this.InternalClick?.Invoke(sender, e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			Color color = default(Color);
			((Color)(ref color))._002Ector(237, 121, 38);
			SettingEntry newColor;
			if (Value >= MaxValue)
			{
				if (Module.Instance.SettingsManager.ModuleSettings.TryGetSetting("colorPickerSettingTimerBar" + barIndex, out var newColor2))
				{
					color = (newColor2 as SettingEntry<Color>).Value.get_Cloth()?.ToXnaColor() ?? color;
				}
			}
			else if (Module.Instance.SettingsManager.ModuleSettings.TryGetSetting("colorPickerSettingTimerBarRefilling" + barIndex, out newColor))
			{
				color = (newColor as SettingEntry<Color>).Value.get_Cloth()?.ToXnaColor() ?? color;
			}
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black() * 0.3f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, layoutBarBackground, color);
			if (Value < MaxValue)
			{
				spriteBatch.DrawOnCtrl(this, textureBarGradient, layoutBarGradient);
			}
			spriteBatch.DrawStringOnCtrl(this, barText, GameService.Content.DefaultFont16, bounds, textColor, wrap: false, HorizontalAlignment.Center);
			spriteBatch.DrawOnCtrl(this, textureBarTop, layoutBarTop);
			spriteBatch.DrawOnCtrl(this, textureBarBottom, layoutBarBottom);
			spriteBatch.DrawOnCtrl(this, textureBarLeftSide, layoutBarLeftSide);
			spriteBatch.DrawOnCtrl(this, textureBarRightSide, layourBarRightSide);
		}

		protected override void DisposeControl()
		{
			Texture2D obj = textureBarTop;
			if (obj != null)
			{
				((GraphicsResource)obj).Dispose();
			}
			Texture2D obj2 = textureBarBottom;
			if (obj2 != null)
			{
				((GraphicsResource)obj2).Dispose();
			}
			Texture2D obj3 = textureBarLeftSide;
			if (obj3 != null)
			{
				((GraphicsResource)obj3).Dispose();
			}
			Texture2D obj4 = textureBarRightSide;
			if (obj4 != null)
			{
				((GraphicsResource)obj4).Dispose();
			}
			Texture2D obj5 = textureBarGradient;
			if (obj5 != null)
			{
				((GraphicsResource)obj5).Dispose();
			}
			base.DisposeControl();
		}
	}
}
