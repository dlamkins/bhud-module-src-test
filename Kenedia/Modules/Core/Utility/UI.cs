using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Utility
{
	public static class UI
	{
		public static string GetDisplayText(BitmapFont font, string text, int maxWidth, string stringOverflow = "...")
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			int width = 0;
			string lastMatchingString = string.Empty;
			string overflowedString = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (font.MeasureString(overflowedString + c + stringOverflow).Width <= (float)maxWidth)
				{
					overflowedString += c;
				}
				Size2 b = font.MeasureString(c.ToString());
				if ((float)width + b.Width <= (float)maxWidth)
				{
					lastMatchingString += c;
					width += (int)b.Width;
					continue;
				}
				return overflowedString + stringOverflow;
			}
			return lastMatchingString;
		}

		public static BitmapFont GetFont(FontSize fontSize, FontStyle style)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Content.GetFont((FontFace)0, fontSize, style);
		}

		public static (Label, CtrlT) CreateLabeledControl<CtrlT>(Container parent, string text, int labelWidth = 175, int controlWidth = 100, int height = 25) where CtrlT : Control, new()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent(parent);
			((Container)panel).set_WidthSizingMode((SizingMode)1);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			Panel p = panel;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)p);
			((Control)label2).set_Width(labelWidth);
			((Control)label2).set_Height(height);
			((Label)label2).set_Text(text);
			Label label = label2;
			CtrlT val = new CtrlT();
			((Control)val).set_Location(new Point(((Control)label).get_Right() + 5, 0));
			((Control)val).set_Width(controlWidth);
			((Control)val).set_Height(height);
			((Control)val).set_Parent((Container)(object)p);
			CtrlT num = val;
			((Control)num).add_Disposed((EventHandler<EventArgs>)Disposed);
			((Control)label).add_Disposed((EventHandler<EventArgs>)Disposed);
			return (label, num);
			void Disposed(object s, EventArgs e)
			{
				((Control)num).remove_Disposed((EventHandler<EventArgs>)Disposed);
				((Control)label).remove_Disposed((EventHandler<EventArgs>)Disposed);
				((Control)num).Dispose();
				((Control)label).Dispose();
				((Control)p).Dispose();
			}
		}

		public static void WrapWithLabel(Func<string> localizedLabelContent, Func<string> localizedTooltip, Container parent, int width, Control ctrl)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent(parent);
			((Control)flowPanel2).set_Width(width);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)2);
			flowPanel2.SetLocalizedTooltip = localizedTooltip;
			FlowPanel flowPanel = flowPanel2;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)flowPanel);
			((Control)label2).set_Height(ctrl.get_Height());
			((Control)label2).set_Width((width - flowPanel.ContentPadding.Horizontal - (int)((FlowPanel)flowPanel).get_ControlPadding().X * 2) / 2);
			label2.SetLocalizedText = localizedLabelContent;
			label2.SetLocalizedTooltip = localizedTooltip;
			((Label)label2).set_VerticalAlignment((VerticalAlignment)1);
			Label label = label2;
			ctrl.set_Parent((Container)(object)flowPanel);
			ctrl.set_Width(((Control)label).get_Width());
			ILocalizable localizable = ctrl as ILocalizable;
			if (localizable != null)
			{
				localizable.SetLocalizedTooltip = localizedTooltip;
			}
		}
	}
}
