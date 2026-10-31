using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class UtilityWindow : OverlayPanel
	{
		private int _dragging;

		private Point _lastMouse;

		private bool? _appliedLight;

		private readonly Label _heading;

		private readonly Label _subheading;

		private readonly ToolbarIcon _close;

		private readonly ToolbarIcon _resize;

		public Panel Body { get; }

		public event Action WindowResized;

		public UtilityWindow(string title, string subtitle, Point position, bool resizable = false)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(position);
			((Control)this).set_Size(new Point(540, 620));
			base.BackgroundOpacity = 1f;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(12, 3));
			((Control)val).set_Size(new Point(470, 24));
			val.set_Text(title);
			val.set_TextColor(Color.get_White());
			((Control)val).set_BasicTooltipText("Drag to move");
			_heading = val;
			((Control)_heading).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginDrag);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(12, 26));
			((Control)val2).set_Size(new Point(470, 20));
			val2.set_Text(subtitle);
			val2.set_TextColor(Color.get_LightGray());
			((Control)val2).set_BasicTooltipText("Drag to move");
			_subheading = val2;
			((Control)_subheading).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginDrag);
			ToolbarIcon toolbarIcon = new ToolbarIcon("close");
			((Control)toolbarIcon).set_Parent((Container)(object)this);
			((Control)toolbarIcon).set_Size(new Point(24, 24));
			((Control)toolbarIcon).set_BasicTooltipText("Close");
			_close = toolbarIcon;
			((Control)_close).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Hide();
			});
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(10, 53));
			((Control)val3).set_Size(new Point(520, 560));
			Body = val3;
			if (resizable)
			{
				ToolbarIcon toolbarIcon2 = new ToolbarIcon("resize");
				((Control)toolbarIcon2).set_Parent((Container)(object)this);
				((Control)toolbarIcon2).set_Size(new Point(22, 22));
				((Control)toolbarIcon2).set_BasicTooltipText("Drag to resize");
				_resize = toolbarIcon2;
				((Control)_resize).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)BeginResize);
			}
			LayoutWindow();
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)EndDrag);
			((Control)this).add_Hidden((EventHandler<EventArgs>)OnHidden);
		}

		private void BeginDrag(object sender, MouseEventArgs args)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			_lastMouse = GameService.Input.get_Mouse().get_Position();
			Interlocked.Exchange(ref _dragging, 1);
		}

		private void BeginResize(object sender, MouseEventArgs args)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			_lastMouse = GameService.Input.get_Mouse().get_Position();
			Interlocked.Exchange(ref _dragging, 2);
		}

		private void LayoutWindow()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			((Control)_heading).set_Width(((Control)this).get_Width() - 70);
			((Control)_subheading).set_Width(((Control)this).get_Width() - 70);
			((Control)_close).set_Location(new Point(((Control)this).get_Width() - 34, 6));
			((Control)Body).set_Size(new Point(((Control)this).get_Width() - 20, ((Control)this).get_Height() - 60));
			if (_resize != null)
			{
				((Control)_resize).set_Location(new Point(((Control)this).get_Width() - 23, ((Control)this).get_Height() - 23));
			}
		}

		private void EndDrag(object sender, MouseEventArgs args)
		{
			Interlocked.Exchange(ref _dragging, 0);
		}

		private void OnHidden(object sender, EventArgs args)
		{
			Interlocked.Exchange(ref _dragging, 0);
		}

		public void UpdateDrag()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			if (_appliedLight != OverlayTheme.Light)
			{
				_appliedLight = OverlayTheme.Light;
				ApplyTheme((Panel)(object)this);
				((Control)this).Invalidate();
			}
			if (!((Control)this).get_Visible() || Volatile.Read(ref _dragging) == 0)
			{
				return;
			}
			Point current = GameService.Input.get_Mouse().get_Position();
			Point delta = current - _lastMouse;
			_lastMouse = current;
			if (Volatile.Read(ref _dragging) == 1)
			{
				((Control)this).set_Location(((Control)this).get_Location() + delta);
			}
			else if (delta != Point.get_Zero())
			{
				Point size = default(Point);
				((Point)(ref size))._002Ector(Math.Max(540, Math.Min(1100, ((Control)this).get_Width() + delta.X)), Math.Max(420, Math.Min(1000, ((Control)this).get_Height() + delta.Y)));
				if (size != ((Control)this).get_Size())
				{
					((Control)this).set_Size(size);
					LayoutWindow();
					this.WindowResized?.Invoke();
				}
			}
		}

		public void RefreshTheme()
		{
			ApplyTheme((Panel)(object)this);
			((Control)this).Invalidate();
		}

		private static void ApplyTheme(Panel panel)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control child in ((Container)panel).get_Children())
			{
				Color textColor = (OverlayTheme.Light ? Color.get_Black() : Color.get_LightGray());
				Label label = (Label)(object)((child is Label) ? child : null);
				if (label != null && !(label is MechanicTypeLabel) && label.get_TextColor() != textColor)
				{
					label.set_TextColor(textColor);
				}
				Color inputColor = (OverlayTheme.Light ? Color.get_Black() : Color.get_White());
				TextInputBase input = (TextInputBase)(object)((child is TextInputBase) ? child : null);
				if (input != null && input.get_ForeColor() != inputColor)
				{
					input.set_ForeColor(inputColor);
				}
				Panel nested = (Panel)(object)((child is Panel) ? child : null);
				if (nested != null)
				{
					ApplyTheme(nested);
				}
			}
		}

		protected override void DisposeControl()
		{
			Interlocked.Exchange(ref _dragging, 0);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)EndDrag);
			((Control)this).remove_Hidden((EventHandler<EventArgs>)OnHidden);
			((Panel)this).DisposeControl();
		}
	}
}
