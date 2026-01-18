using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CefSharp;
using CefSharp.OffScreen;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BhModule.WebPeeper
{
	public class BookmarkPanel : Panel
	{
		public static BookmarkPanel Instance;

		private static readonly Point _bgOverSize = new Point(75, 50);

		private static readonly Texture2D _bgTexture = Control.get_Content().GetTexture("controls/window/502049");

		private static readonly Texture2D _editBtnTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("edit.png");

		private static readonly Texture2D _addBtnTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("add.png");

		private static readonly Texture2D _emptyTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("empty.png");

		private static readonly Color _emptyColor = new Color(808464432u);

		private static readonly Regex _notSupportStringMatcher = new Regex("[^A-z0-9\\s\\W]");

		private const string _bookmarksJsonFileName = "bookmarks.json";

		private readonly OrderedDictionary _bookmarkMenuItems = new OrderedDictionary();

		private readonly IconButton _editBtn;

		private readonly IconButton _addBtn;

		private readonly Menu _menuContainer;

		private readonly Tween _animFade;

		private readonly string _jsonPath;

		private Rectangle _bgDestRect = Rectangle.get_Empty();

		private Rectangle _bgSourceRect = Rectangle.get_Empty();

		private Rectangle _emptyDestRect = Rectangle.get_Empty();

		private bool _editing;

		private ChromiumWebBrowser WebBrowser => WebPeeperModule.Instance.CefService.WebBrowser;

		public BookmarkPanel()
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			Instance = this;
			((Panel)this).set_Title(" ");
			((Panel)this).set_CanScroll(true);
			((Control)this).set_Padding(new Thickness(0f, (float)_bgOverSize.X, (float)_bgOverSize.Y, 0f));
			IconButton iconButton = new IconButton(_editBtnTexture, 20, 1.5f);
			((Control)iconButton).set_BasicTooltipText("Edit Bookmarks");
			_editBtn = iconButton;
			((Control)_editBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetChildrenEditState(!_editing);
			});
			IconButton iconButton2 = new IconButton(_addBtnTexture, 20, 1.5f);
			((Control)iconButton2).set_BasicTooltipText("Bookmark Current Page");
			_addBtn = iconButton2;
			((Control)_addBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (WebBrowser != null && WebBrowser.CanExecuteJavascriptInMainFrame)
				{
					Task<JavascriptResponse> task = WebBrowser.EvaluateScriptAsync("document.title");
					task.Wait(TimeSpan.FromSeconds(1.0));
					string input = (task.IsCanceled ? ((WindowBase2)WebPeeperModule.Instance.UIService.BrowserWindow).get_Subtitle() : ((string)task.Result.Result));
					AddBookmark(new Bookmark
					{
						Name = _notSupportStringMatcher.Replace(input, "-"),
						URL = WebBrowser.Address
					});
				}
			});
			BookmarkMenu bookmarkMenu = new BookmarkMenu();
			((Control)bookmarkMenu).set_Size(((Control)this).get_Size());
			((Control)bookmarkMenu).set_Parent((Container)(object)this);
			_menuContainer = (Menu)(object)bookmarkMenu;
			((Control)this).set_Visible(false);
			((Control)this).set_Opacity(0f);
			_animFade = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<BookmarkPanel>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(-1).Reflect();
			_animFade.Pause();
			_animFade.OnUpdate((Action)delegate
			{
				((Control)_addBtn).set_Opacity(((Control)this)._opacity);
				((Control)_editBtn).set_Opacity(((Control)this)._opacity);
			});
			_animFade.OnComplete((Action)delegate
			{
				_animFade.Pause();
				if (((Control)this)._opacity <= 0f)
				{
					((Control)this).set_Visible(false);
				}
				else
				{
					((Control)_menuContainer).set_Opacity(1f);
				}
			});
			_jsonPath = Path.Combine(CefService.CefSettingFolder, "bookmarks.json");
			if (!File.Exists(_jsonPath))
			{
				return;
			}
			try
			{
				Bookmark[] array = JsonConvert.DeserializeObject<Bookmark[]>(File.ReadAllText(_jsonPath));
				foreach (Bookmark bookmark in array)
				{
					_bookmarkMenuItems[bookmark] = CreateBookmarkItem(bookmark);
				}
			}
			catch
			{
			}
		}

		public override void Show()
		{
			if (!((Control)this).get_Visible())
			{
				((Control)this).set_Opacity(0f);
				((Control)this).set_Visible(true);
				_animFade.Resume();
			}
		}

		public override void Hide()
		{
			if (((Control)this).get_Visible())
			{
				((Control)_menuContainer).set_Opacity(0.5f);
				_animFade.Resume();
			}
		}

		protected override void OnShown(EventArgs e)
		{
			ShowBtns();
			((Control)this).OnShown(e);
		}

		protected override void OnHidden(EventArgs e)
		{
			HideBtns();
			SetChildrenEditState(edit: false);
			((Control)this).OnHidden(e);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			SetBtnsLocation();
			((Control)this).OnMoved(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			_bgDestRect = new Rectangle(0, 0, ((Control)this).get_Width() + _bgOverSize.X, ((Control)this).get_Height() + _bgOverSize.Y);
			_bgSourceRect = new Rectangle(_bgTexture.get_Width() - _bgDestRect.Width, _bgTexture.get_Height() - _bgDestRect.Height, _bgDestRect.Width, _bgDestRect.Height);
			int num = (int)((double)((Control)this).get_Size().X * 0.3);
			double num2 = (double)_emptyTexture.get_Height() / (double)_emptyTexture.get_Width() * (double)num;
			_emptyDestRect = new Rectangle((((Control)this).get_Size().X - num) / 2, (((Control)this).get_Size().Y - (int)num2) / 2, num, (int)num2);
			if (_menuContainer != null)
			{
				((Control)_menuContainer).set_Size(e.get_CurrentSize());
			}
			SetBtnsLocation();
			((Container)this).OnResized(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _bgTexture, _bgDestRect, (Rectangle?)_bgSourceRect);
			if (_bookmarkMenuItems.Count == 0)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _emptyTexture, _emptyDestRect, _emptyColor);
			}
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			((Control)_editBtn).Dispose();
			((Control)_addBtn).Dispose();
			((Panel)this).DisposeControl();
		}

		public void SetChildrenEditState(bool edit)
		{
			if (_editing == edit)
			{
				return;
			}
			_editing = edit;
			foreach (object value in _bookmarkMenuItems.Values)
			{
				(value as BookmarkItem)?.SetEditState(edit);
			}
			if (!_editing)
			{
				WriteJson();
			}
		}

		private void ShowBtns()
		{
			((Control)_editBtn).set_Parent(((Control)this).get_Parent());
			((Control)_editBtn).set_ZIndex(((Control)this).get_ZIndex() + 1);
			((Control)_editBtn).set_Visible(_bookmarkMenuItems.Count > 0);
			((Control)_addBtn).set_Parent(((Control)this).get_Parent());
			((Control)_addBtn).set_ZIndex(((Control)this).get_ZIndex() + 1);
			((Control)_addBtn).set_Visible(true);
		}

		private void SetBtnsLocation()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			((Control)_editBtn).set_Location(new Point(((Control)this).get_Location().X + 10, ((Control)this).get_Location().Y + 10));
			((Control)_addBtn).set_Location(new Point(((Control)this).get_Location().X + ((Control)this).get_Width() - 50, ((Control)_editBtn).get_Location().Y));
		}

		private void HideBtns()
		{
			((Control)_editBtn).set_Visible(false);
			((Control)_addBtn).set_Visible(false);
		}

		private BookmarkItem CreateBookmarkItem(Bookmark bookmark)
		{
			BookmarkItem bookmarkItem = new BookmarkItem(bookmark);
			((Control)bookmarkItem).set_Parent((Container)(object)_menuContainer);
			BookmarkItem bookmarkItem2 = bookmarkItem;
			bookmarkItem2.Removed += delegate
			{
				_bookmarkMenuItems.Remove(bookmark);
				if (_bookmarkMenuItems.Count == 0)
				{
					SetChildrenEditState(edit: false);
					((Control)_editBtn).Hide();
				}
			};
			bookmarkItem2.Dragged += ReorderBookmarks;
			if (_editing)
			{
				bookmarkItem2.SetEditState(_editing);
			}
			return bookmarkItem2;
		}

		private void ReorderBookmarks(object sender, BookmarkDraggedEventArgs e)
		{
			int num = -1;
			int num2 = -1;
			int num3 = _bookmarkMenuItems.Values.Count - 1;
			Bookmark bookmark = e.Bookmark;
			object obj = _bookmarkMenuItems[e.Bookmark];
			int num4 = 0;
			bool flag = false;
			bool flag2 = false;
			foreach (object value in _bookmarkMenuItems.Values)
			{
				num++;
				num2++;
				BookmarkItem bookmarkItem = value as BookmarkItem;
				if (bookmarkItem == null)
				{
					continue;
				}
				int num5 = num4;
				int num6 = ((Control)bookmarkItem).get_Top() + ((Control)bookmarkItem).get_Height() / 2;
				num4 = num6;
				if (((object)bookmarkItem).Equals(obj))
				{
					num2--;
					flag = num != num3 - 1;
				}
				else if (flag)
				{
					flag = false;
				}
				else if ((e.Position >= num5 && e.Position <= num6) || (flag2 = num == num3 && e.Position >= num4))
				{
					if (flag2)
					{
						num2++;
					}
					_bookmarkMenuItems.Remove(bookmark);
					_bookmarkMenuItems.Insert(num2, bookmark, obj);
					break;
				}
			}
			foreach (object value2 in _bookmarkMenuItems.Values)
			{
				BookmarkItem bookmarkItem2 = value2 as BookmarkItem;
				if (bookmarkItem2 != null)
				{
					((Control)bookmarkItem2).set_Parent((Container)null);
				}
			}
			foreach (object value3 in _bookmarkMenuItems.Values)
			{
				BookmarkItem bookmarkItem3 = value3 as BookmarkItem;
				if (bookmarkItem3 != null)
				{
					((Control)bookmarkItem3).set_Parent((Container)(object)_menuContainer);
				}
			}
		}

		private void AddBookmark(Bookmark bookmark)
		{
			_bookmarkMenuItems[bookmark] = CreateBookmarkItem(bookmark);
			((Control)_editBtn).Show();
			WriteJson();
		}

		private void WriteJson()
		{
			File.WriteAllText(_jsonPath, JsonConvert.SerializeObject((object)_bookmarkMenuItems.Keys));
		}
	}
}
