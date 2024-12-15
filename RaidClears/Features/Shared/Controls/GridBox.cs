using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using RaidClears.Settings.Enums;

namespace RaidClears.Features.Shared.Controls
{
	public class GridBox : Label
	{
		protected MyEffect bgtexture { get; set; }

		public Color BackgroundColor
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return bgtexture.Tint;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				bgtexture.Tint = value;
			}
		}

		public GridBox(Container parent, string title, string tooltip, SettingEntry<float> opacity, SettingEntry<FontSize> fontSize)
			: this()
		{
			((Control)this).set_Parent(parent);
			((Label)this).set_Text(title);
			((Control)this).set_BasicTooltipText(tooltip);
			((Label)this).set_HorizontalAlignment((HorizontalAlignment)1);
			((Label)this).set_AutoSizeHeight(true);
			OpacityChange(opacity);
			FontSizeChange(fontSize);
			bgtexture = new MyEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)bgtexture);
		}

		private void OpacityChange(SettingEntry<float> opacity)
		{
			opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object _, ValueChangedEventArgs<float> e)
			{
				((Control)this).set_Opacity(e.get_NewValue());
			});
			((Control)this).set_Opacity(opacity.get_Value());
		}

		public void LayoutChange(SettingEntry<Layout> layout)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			layout.add_SettingChanged((EventHandler<ValueChangedEventArgs<Layout>>)delegate(object _, ValueChangedEventArgs<Layout> e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Label)this).set_HorizontalAlignment(LabelAlignment(e.get_NewValue()));
			});
			((Label)this).set_HorizontalAlignment(LabelAlignment(layout.get_Value()));
		}

		private static HorizontalAlignment LabelAlignment(Layout layout)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			switch (layout)
			{
			case Layout.Vertical:
			case Layout.SingleRow:
				return (HorizontalAlignment)2;
			case Layout.Horizontal:
			case Layout.SingleColumn:
				return (HorizontalAlignment)1;
			default:
				return (HorizontalAlignment)1;
			}
		}

		private void FontSizeChange(SettingEntry<FontSize> fontSize)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			fontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)delegate(object _, ValueChangedEventArgs<FontSize> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				SetFontSize(e.get_NewValue(), (Label)(object)this);
			});
			SetFontSize(fontSize.get_Value(), (Label)(object)this);
		}

		private void SetFontSize(FontSize fontSize, Label label)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
			int width = GetLabelWidthForFontSize(fontSize);
			label.set_Font(font);
			((Control)label).set_Width(width);
		}

		private static int GetLabelWidthForFontSize(FontSize size)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected I4, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected I4, but got Unknown
			switch (size - 8)
			{
			default:
				switch (size - 32)
				{
				case 4:
					return 100;
				case 0:
				case 2:
					return 80;
				}
				break;
			case 16:
				return 55;
			case 12:
			case 14:
				return 50;
			case 10:
				return 45;
			case 6:
			case 8:
				return 40;
			case 3:
			case 4:
				return 35;
			case 0:
				return 39;
			case 1:
			case 2:
			case 5:
			case 7:
			case 9:
			case 11:
			case 13:
			case 15:
				break;
			}
			return 40;
		}
	}
}
