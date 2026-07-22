using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Utility
{
	public static class UI
	{
		public static ContentsManager ContentsManager { get; set; }

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
			Control[] visibleChildren = panel2.Children.Where((Control c) => c.Visible).ToArray();
			if (visibleChildren.Length == 0)
			{
				scrollbar.ScrollDistance = 0f;
				return;
			}
			int maxOffset = Math.Max(Math.Max(visibleChildren.Max((Control c) => c.Bottom), panel2.ContentRegion.Height) - panel2.ContentRegion.Height, 0);
			if (maxOffset == 0)
			{
				scrollbar.ScrollDistance = 0f;
				return;
			}
			int margin = 10;
			int viewportTop;
			int num = (viewportTop = panel2.VerticalScrollOffset);
			int viewportBottom = num + panel2.ContentRegion.Height;
			int childTop = child.Top;
			int childBottom = child.Bottom;
			int targetOffset = num;
			if (childTop < viewportTop + margin)
			{
				targetOffset = Math.Max(childTop - margin, 0);
			}
			else if (childBottom > viewportBottom - margin)
			{
				targetOffset = Math.Max(childBottom - panel2.ContentRegion.Height + margin, 0);
			}
			scrollbar.ScrollDistance = Math.Max(0f, Math.Min((float)targetOffset / (float)maxOffset, 1f));
		}

		public static bool HasVisibleVerticalScrollbar(this Container container)
		{
			if (container == null)
			{
				return false;
			}
			Blish_HUD.Controls.Panel panel = container as Blish_HUD.Controls.Panel;
			if (panel != null && !panel.CanScroll)
			{
				return false;
			}
			Control[] visibleChildren = container.Children.Where((Control c) => c.Visible).ToArray();
			if (visibleChildren.Length == 0)
			{
				return false;
			}
			return Math.Max(visibleChildren.Max((Control c) => c.Bottom), container.ContentRegion.Height) > container.ContentRegion.Height + 3;
		}

		public static int GetTextHeight(BitmapFont font, string text, int maxWidth)
		{
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
			return height + font.LineHeight;
		}

		public static string GetDisplayText(BitmapFont font, string text, int maxWidth, string stringOverflow = "...")
		{
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
