using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal class WindowContent : Panel
	{
		private readonly NavigationBar _navigationBar;

		private Control _mainContent;

		private readonly BookmarkPanel _bookmarkPanel;

		private const int _gap = 10;

		public WindowContent(Point size)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			((Control)this)._size = size;
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
			CreateMainContent();
			BookmarkPanel bookmarkPanel = new BookmarkPanel();
			((Control)bookmarkPanel).set_Parent((Container)(object)this);
			_bookmarkPanel = bookmarkPanel;
			((Control)_bookmarkPanel).add_Shown((EventHandler<EventArgs>)delegate
			{
				WebPainter webPainter2 = _mainContent as WebPainter;
				if (webPainter2 != null)
				{
					webPainter2.Disabled = true;
				}
			});
			((Control)_bookmarkPanel).add_Hidden((EventHandler<EventArgs>)delegate
			{
				WebPainter webPainter = _mainContent as WebPainter;
				if (webPainter != null)
				{
					webPainter.Disabled = false;
				}
			});
			((Control)this).OnResized(new ResizedEventArgs(size, size));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			ResetNavigationBarRect();
			ResetMainContentRect();
			ResetBookmarkRect();
			((Container)this).OnResized(e);
		}

		private void CreateMainContent()
		{
			WebPainter webPainter = new WebPainter();
			((Control)webPainter).set_Parent((Container)(object)this);
			_mainContent = (Control)(object)webPainter;
			_mainContent.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_bookmarkPanel).get_Visible())
				{
					((Control)_bookmarkPanel).Hide();
				}
			});
			if (_bookmarkPanel != null)
			{
				((Control)_bookmarkPanel).set_Parent((Container)null);
				((Control)_bookmarkPanel).set_Parent((Container)(object)this);
			}
		}

		private void ResetNavigationBarRect()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_navigationBar).set_Location(Point.get_Zero());
			((Control)_navigationBar).set_Width(((Control)this).get_Width());
		}

		private void ResetMainContentRect()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_navigationBar).get_Visible())
			{
				_mainContent.set_Location(new Point(0, ((Control)_navigationBar).get_Bottom() + 10));
				_mainContent.set_Size(new Point(((Control)this).get_Size().X - 1, ((Control)this).get_Size().Y - ((Control)_navigationBar).get_Height() - 1 - 10));
			}
			else
			{
				_mainContent.set_Location(Point.get_Zero());
				_mainContent.set_Size(new Point(((Control)this).get_Size().X - 1, ((Control)this).get_Size().Y - 1));
			}
		}

		private void ResetBookmarkRect()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			((Control)_bookmarkPanel).set_Location(_mainContent.get_Location());
			((Control)_bookmarkPanel).set_Size(new Point(MathHelper.Max(_mainContent.get_Width() / 2, 150), MathHelper.Min(_mainContent.get_Height(), 700)));
		}

		private void OnNavigationBarVisibleChanged(object sender, EventArgs e)
		{
			ResetMainContentRect();
		}
	}
}
