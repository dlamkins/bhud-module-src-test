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
				if (((Control)this).SetProperty<float>(ref maxValue, value, false, "MaxValue"))
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
				if (((Control)this).SetProperty<float>(ref minValue, value, false, "MinValue"))
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
				((Control)this).SetProperty<float>(ref this.value, MathHelper.Clamp(value, minValue, maxValue), true, "Value");
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
				((Control)this).SetProperty<string>(ref barText, value, false, "BarText");
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
				((Control)this).SetProperty<Color>(ref textColor, value, false, "TextColor");
			}
		}

		public event EventHandler<MouseEventArgs> InternalClick;

		public TimerBar(int barIndex)
			: this()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(256, 16));
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
			layoutBarLeftSide = new Rectangle(0, 0, textureBarLeftSide.get_Width(), ((Control)this).get_Height());
			layourBarRightSide = new Rectangle(((Control)this).get_Width() - textureBarRightSide.get_Width(), 0, textureBarRightSide.get_Width(), ((Control)this).get_Height());
			layoutBarTop = new Rectangle(0, 0, ((Control)this).get_Width(), textureBarTop.get_Height());
			layoutBarBottom = new Rectangle(0, ((Control)this).get_Height() - textureBarBottom.get_Height(), ((Control)this).get_Width(), textureBarTop.get_Height());
			float num = (Value - MinValue) / (MaxValue - MinValue) * (float)(((Control)this).get_Width() - textureBarLeftSide.get_Width());
			layoutBarBackground = new Rectangle(textureBarLeftSide.get_Width(), 2, (int)num - textureBarLeftSide.get_Width(), ((Control)this).get_Height() - 4);
			layoutBarGradient = new Rectangle((int)num - textureBarGradient.get_Width(), 2, textureBarGradient.get_Width(), ((Control)this).get_Height() - 4);
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
			Color val = default(Color);
			((Color)(ref val))._002Ector(237, 121, 38);
			SettingEntry val3 = default(SettingEntry);
			if (Value >= MaxValue)
			{
				SettingEntry val2 = default(SettingEntry);
				if (Module.Instance.SettingsManager.get_ModuleSettings().TryGetSetting("colorPickerSettingTimerBar" + barIndex, ref val2))
				{
					ColorMaterial cloth = (val2 as SettingEntry<Color>).get_Value().get_Cloth();
					val = ((cloth != null) ? ColorExtensions.ToXnaColor(cloth) : val);
				}
			}
			else if (Module.Instance.SettingsManager.get_ModuleSettings().TryGetSetting("colorPickerSettingTimerBarRefilling" + barIndex, ref val3))
			{
				ColorMaterial cloth2 = (val3 as SettingEntry<Color>).get_Value().get_Cloth();
				val = ((cloth2 != null) ? ColorExtensions.ToXnaColor(cloth2) : val);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.3f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), layoutBarBackground, val);
			if (Value < MaxValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textureBarGradient, layoutBarGradient);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, barText, GameService.Content.get_DefaultFont16(), bounds, textColor, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textureBarTop, layoutBarTop);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textureBarBottom, layoutBarBottom);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textureBarLeftSide, layoutBarLeftSide);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textureBarRightSide, layourBarRightSide);
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
			((Control)this).DisposeControl();
		}
	}
}
