using System;
using System.Linq;
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
		public static void ScrollToChild(this Kenedia.Modules.Core.Controls.FlowPanel panel, Control child)
		{
			((Container)panel).ScrollToChild(child);
		}

		public static void ScrollToChild(this Kenedia.Modules.Core.Controls.Panel panel, Control child)
		{
			((Container)panel).ScrollToChild(child);
		}

		public static void ScrollToChild(this Container panel, Control child)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			Container panel2 = panel;
			if (!panel2.Children.Contains(child))
			{
				return;
			}
			Kenedia.Modules.Core.Controls.Scrollbar scrollbar = panel2.Parent.Children.OfType<Kenedia.Modules.Core.Controls.Scrollbar>().FirstOrDefault((Kenedia.Modules.Core.Controls.Scrollbar s) => s.AssociatedContainer == panel2);
			if (scrollbar == null)
			{
				return;
			}
			if (child.Location.Y == 0)
			{
				scrollbar.ScrollDistance = 0f;
				return;
			}
			scrollbar.ScrollDistance = (float)child.Location.Y / (float)(panel2.Children.Max((Control c) => c.Bottom) - scrollbar.Size.Y);
		}

		public static int GetTextHeight(BitmapFont font, string text, int maxWidth)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}
			int width = 0;
			int height = 0;
			for (int i = 0; i < text.Length; i++)
			{
				Size2 b = font.MeasureString(text[i].ToString());
				if ((float)width + b.Width <= (float)maxWidth)
				{
					width += (int)b.Width;
					continue;
				}
				width = (int)b.Width;
				height += (int)b.Height;
			}
			return height + font.get_LineHeight();
		}

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

		public static BitmapFont GetFont(ContentService.FontSize fontSize, ContentService.FontStyle style)
		{
			return GameService.Content.GetFont(ContentService.FontFace.Menomonia, fontSize, style);
		}

		public static (Kenedia.Modules.Core.Controls.Label, CtrlT) CreateLabeledControl<CtrlT>(Container parent, string text, int labelWidth = 175, int controlWidth = 100, int height = 25) where CtrlT : Control, new()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = parent,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label label = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = p,
				Width = labelWidth,
				Height = height,
				Text = text
			};
			CtrlT num = new CtrlT
			{
				Location = new Point(label.Right + 5, 0),
				Width = controlWidth,
				Height = height,
				Parent = p
			};
			num.Disposed += Disposed;
			label.Disposed += Disposed;
			return (label, num);
			void Disposed(object s, EventArgs e)
			{
				num.Disposed -= Disposed;
				label.Disposed -= Disposed;
				num.Dispose();
				label.Dispose();
				p.Dispose();
			}
		}

		public static void WrapWithLabel(Func<string> localizedLabelContent, Func<string> localizedTooltip, Container parent, int width, Control ctrl)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.FlowPanel flowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = parent,
				Width = width,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				SetLocalizedTooltip = localizedTooltip
			};
			Kenedia.Modules.Core.Controls.Label label = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Height = ctrl.Height,
				Width = (width - flowPanel.ContentPadding.Horizontal - (int)flowPanel.ControlPadding.X * 2) / 2,
				SetLocalizedText = localizedLabelContent,
				SetLocalizedTooltip = localizedTooltip,
				VerticalAlignment = VerticalAlignment.Middle
			};
			ctrl.Parent = flowPanel;
			ctrl.Width = label.Width;
			ILocalizable localizable = ctrl as ILocalizable;
			if (localizable != null)
			{
				localizable.SetLocalizedTooltip = localizedTooltip;
			}
		}
	}
}
