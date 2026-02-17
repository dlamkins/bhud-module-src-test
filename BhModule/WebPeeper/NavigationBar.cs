using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CefHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class NavigationBar : FlowPanel
	{
		private static readonly Texture2D _btnTexture = GameService.Content.GetTexture("784268");

		private static readonly Texture2D _bookmarkBtnTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("bookmark.png");

		private IconButton _backBtn;

		private IconButton _fowardBtn;

		private TextBox _addressInput;

		private LoadingSpinner _loading;

		public event EventHandler<EventArgs> BookmarkBtnClicked;

		public NavigationBar()
			: this()
		{
			((Control)this).set_Height(30);
			SetChildren();
			Browser.GetFullscreenState().ContinueWith(delegate(Task<bool> t)
			{
				((Control)this).set_Visible(!t.Result);
			});
			Browser.add_LoadingStateChanged((Action<bool, bool, bool>)HandleLoading);
			Browser.add_AddressChanged((Action<string>)HandleAddress);
			Browser.add_UrlLoadError((Action<string>)HandleAddress);
			Browser.add_FullscreenModeChanged((Action<bool>)HandleFullscreen);
		}

		private void SetChildren()
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			IconButton iconButton = new IconButton(_btnTexture, ((Control)this).get_Height(), 2);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_Visible(Browser.get_CanGoBack());
			_backBtn = iconButton;
			((Control)_backBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Browser.Back();
			});
			IconButton iconButton2 = new IconButton(_btnTexture, ((Control)this).get_Height(), 2);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_Visible(Browser.get_CanGoForward());
			iconButton2.IconRotation = MathHelper.ToRadians(180f);
			_fowardBtn = iconButton2;
			((Control)_fowardBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Browser.Forward();
			});
			IconButton iconButton3 = new IconButton(_bookmarkBtnTexture, ((Control)this).get_Height(), 2);
			((Control)iconButton3).set_Parent((Container)(object)this);
			((Control)iconButton3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.BookmarkBtnClicked?.Invoke(this, EventArgs.Empty);
			});
			TextBox val = new TextBox();
			((TextInputBase)val).set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Height(((Control)this).get_Height());
			((Control)val).set_Parent((Container)(object)this);
			_addressInput = val;
			((TextInputBase)_addressInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object s, ValueEventArgs<bool> e)
			{
				if (e.get_Value())
				{
					string text = Clipboard.GetText();
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				}
			});
			_addressInput.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				if (string.IsNullOrWhiteSpace(((TextInputBase)_addressInput).get_Text()))
				{
					((TextInputBase)_addressInput).set_Text(Browser.get_Address());
				}
				else
				{
					HandleLoading(canGoBack: true, canGoForward: false, isLoading: true);
					WebPeeperModule.Instance.CefService.Search(((TextInputBase)_addressInput).get_Text());
				}
			});
			((TextInputBase)_addressInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> e)
			{
				((TextInputBase)_addressInput).set_SelectionStart((!e.get_Value()) ? ((TextInputBase)_addressInput).get_Text().Length : 0);
				((TextInputBase)_addressInput).set_SelectionEnd(((TextInputBase)_addressInput).get_Text().Length);
				if (!e.get_Value() && ((TextInputBase)_addressInput).get_Text().Length == 0)
				{
					((TextInputBase)_addressInput).set_Text(Browser.get_Address());
				}
			});
			((TextInputBase)_addressInput).set_Text(Browser.get_Address());
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Visible(Browser.get_IsLoading());
			((Control)val2).set_Enabled(false);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(((Control)_addressInput).get_Height(), ((Control)_addressInput).get_Height()));
			_loading = val2;
		}

		public override void RecalculateLayout()
		{
			((FlowPanel)this).RecalculateLayout();
			RecalculatetLoadingLocation();
		}

		private void HandleLoading(bool canGoBack, bool canGoForward, bool isLoading)
		{
			if (((Control)_fowardBtn).get_Visible() != canGoForward || ((Control)_backBtn).get_Visible() != canGoBack)
			{
				((Control)_backBtn).set_Visible(canGoBack);
				((Control)_fowardBtn).set_Visible(canGoForward);
				((Control)this).RecalculateLayout();
				RecalculateAddressInputWidth();
			}
			((Control)_loading).set_Visible(isLoading);
			RecalculatetLoadingLocation();
		}

		private void RecalculatetLoadingLocation()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_loading != null)
			{
				((Control)_loading).set_Location(new Point(((Control)this).get_Width() - ((Control)_loading).get_Width(), 0));
			}
		}

		private void HandleFullscreen(bool isFullscreen)
		{
			if (isFullscreen)
			{
				((Control)this).Hide();
			}
			else
			{
				((Control)this).Show();
			}
		}

		private void HandleAddress(string address)
		{
			((TextInputBase)_addressInput).set_Text(address);
		}

		private void RecalculateAddressInputWidth()
		{
			if (_addressInput == null)
			{
				return;
			}
			int num = 0;
			foreach (Control child in ((Container)this).get_Children())
			{
				if (child.get_Visible())
				{
					if (child == _addressInput)
					{
						break;
					}
					num = child.get_Right();
				}
			}
			((Control)_addressInput).set_Width(((Control)this).get_Width() - num - 1);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			RecalculateAddressInputWidth();
			((Container)this).OnResized(e);
		}

		protected override void DisposeControl()
		{
			Browser.remove_LoadingStateChanged((Action<bool, bool, bool>)HandleLoading);
			Browser.remove_AddressChanged((Action<string>)HandleAddress);
			Browser.remove_UrlLoadError((Action<string>)HandleAddress);
			Browser.remove_FullscreenModeChanged((Action<bool>)HandleFullscreen);
			this.BookmarkBtnClicked = null;
		}
	}
}
