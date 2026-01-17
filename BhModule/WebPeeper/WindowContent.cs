using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	public class WindowContent : FlowPanel
	{
		private NavigationBar _navigationBar;

		private WebPainter _webPainter;

		private BookmarkPanel _bookmarkPanel;

		public WindowContent()
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 10f));
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			SetChildren();
		}

		private void SetChildren()
		{
			NavigationBar navigationBar = new NavigationBar();
			((Control)navigationBar).set_Parent((Container)(object)this);
			_navigationBar = navigationBar;
			_navigationBar.BookmarkBtnClicked += delegate
			{
				if (((Control)_bookmarkPanel).get_Visible())
				{
					((Control)_bookmarkPanel).Hide();
				}
				else
				{
					((Control)_bookmarkPanel).Show();
				}
			};
			((Control)_navigationBar).add_Hidden((EventHandler<EventArgs>)OnNavigationBarVisibleChanged);
			((Control)_navigationBar).add_Shown((EventHandler<EventArgs>)OnNavigationBarVisibleChanged);
			WebPainter webPainter = new WebPainter();
			((Control)webPainter).set_Parent((Container)(object)this);
			_webPainter = webPainter;
			((Control)_webPainter).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_bookmarkPanel).get_Visible())
				{
					((Control)_bookmarkPanel).Hide();
				}
			});
			BookmarkPanel bookmarkPanel = new BookmarkPanel();
			((Control)bookmarkPanel).set_Parent((Container)(object)this);
			_bookmarkPanel = bookmarkPanel;
			((Control)_bookmarkPanel).add_Shown((EventHandler<EventArgs>)delegate
			{
				_webPainter.Disabled = true;
			});
			((Control)_bookmarkPanel).add_Hidden((EventHandler<EventArgs>)delegate
			{
				_webPainter.Disabled = false;
			});
		}

		public override void RecalculateLayout()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			if (_bookmarkPanel != null)
			{
				((Control)_bookmarkPanel).set_Location(((Control)_webPainter).get_Location());
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			ResetNavigationBarSize();
			ResetWebPainterSize();
			ResetBookmarkSize();
			((Container)this).OnResized(e);
		}

		private void ResetNavigationBarSize()
		{
			((Control)_navigationBar).set_Width(((Control)this).get_Width());
		}

		private void ResetWebPainterSize()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_navigationBar).get_Visible())
			{
				((Control)_webPainter).set_Size(new Point(((Control)this).get_Size().X - 1, ((Control)this).get_Size().Y - ((Control)_navigationBar).get_Height() - 1 - (int)((FlowPanel)this).get_ControlPadding().Y));
			}
			else
			{
				((Control)_webPainter).set_Size(new Point(((Control)this).get_Size().X - 1, ((Control)this).get_Size().Y - 1));
			}
		}

		private void ResetBookmarkSize()
		{
			((Control)_bookmarkPanel).set_Height(MathHelper.Min(((Control)_webPainter).get_Height(), 700));
			((Control)_bookmarkPanel).set_Width(MathHelper.Max(((Control)_webPainter).get_Width() / 2, 150));
		}

		private void OnNavigationBarVisibleChanged(object sender, EventArgs e)
		{
			ResetWebPainterSize();
			((Control)this).RecalculateLayout();
		}
	}
}
