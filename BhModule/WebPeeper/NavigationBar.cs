using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CefSharp;
using CefSharp.OffScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	public class NavigationBar : FlowPanel
	{
		public static NavigationBar Instance;

		private static readonly Texture2D _btnTexture = GameService.Content.GetTexture("784268");

		private static readonly Texture2D _bookmarkBtnTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("bookmark.png");

		private IconButton _backBtn;

		private IconButton _fowardBtn;

		private TextBox _addressInput;

		private LoadingSpinner _loading;

		private ChromiumWebBrowser WebBrowser => WebPeeperModule.Instance.CefService.WebBrowser;

		public event EventHandler<EventArgs> BookmarkBtnClicked;

		public NavigationBar()
			: this()
		{
			Instance = this;
			((Control)this).set_Height(30);
			SetChildren();
			if (WebBrowser == null)
			{
				return;
			}
			if (WebBrowser.CanExecuteJavascriptInMainFrame)
			{
				WebBrowser.EvaluateScriptAsync("document.fullscreen").ContinueWith(delegate(Task<JavascriptResponse> t)
				{
					((Control)this).set_Visible(!(bool)t.Result.Result);
				});
			}
			WebBrowser.LoadingStateChanged += HandleLoading;
			WebBrowser.AddressChanged += HandleAddress;
		}

		public void SetAddressInputText(string text)
		{
			if (_addressInput != null)
			{
				((TextInputBase)_addressInput).set_Text(text);
			}
		}

		private void SetChildren()
		{
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			IconButton iconButton = new IconButton(_btnTexture, ((Control)this).get_Height(), 2);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_Visible(WebBrowser?.CanGoBack ?? false);
			_backBtn = iconButton;
			((Control)_backBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				WebBrowser?.Back();
			});
			IconButton iconButton2 = new IconButton(_btnTexture, ((Control)this).get_Height(), 2);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_Visible(WebBrowser?.CanGoForward ?? false);
			iconButton2.IconRotation = MathHelper.ToRadians(180f);
			_fowardBtn = iconButton2;
			((Control)_fowardBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				WebBrowser?.Forward();
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
			_addressInput.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				CancellationTokenSource cts;
				if (string.IsNullOrWhiteSpace(((TextInputBase)_addressInput).get_Text()))
				{
					((TextInputBase)_addressInput).set_Text(WebBrowser?.Address ?? "");
				}
				else
				{
					HandleLoading(this, new LoadingStateChangedEventArgs(null, canGoBack: true, canGoForward: false, isLoading: true));
					WebPeeperModule.Instance.CefService.LastAddressInputText = ((TextInputBase)_addressInput).get_Text();
					cts = new CancellationTokenSource();
					if (WebBrowser != null)
					{
						WebBrowser.LoadingStateChanged += stopManuallyErrTrigger;
					}
					WebBrowser?.LoadUrlAsync(WebPeeperModule.Instance.CefService.LastAddressInputText);
					Task.Delay(1000, cts.Token).ContinueWith(delegate(Task t)
					{
						if (WebBrowser != null)
						{
							WebBrowser.LoadingStateChanged -= stopManuallyErrTrigger;
						}
						if (!t.IsCanceled && !t.IsFaulted)
						{
							HandleLoading(this, new LoadingStateChangedEventArgs(null, canGoBack: true, canGoForward: false, isLoading: false));
							WebPeeperModule.Instance.CefService.OnUrlLoadError(this, new LoadErrorEventArgs(null, null, CefErrorCode.InvalidUrl, "", ((TextInputBase)_addressInput).get_Text()));
						}
					});
				}
				void stopManuallyErrTrigger(object sender, LoadingStateChangedEventArgs e)
				{
					WebBrowser.LoadingStateChanged -= stopManuallyErrTrigger;
					cts.Cancel();
				}
			});
			((TextInputBase)_addressInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> e)
			{
				((TextInputBase)_addressInput).set_SelectionStart((!e.get_Value()) ? ((TextInputBase)_addressInput).get_Text().Length : 0);
				((TextInputBase)_addressInput).set_SelectionEnd(((TextInputBase)_addressInput).get_Text().Length);
				if (!e.get_Value() && ((TextInputBase)_addressInput).get_Text().Length == 0)
				{
					((TextInputBase)_addressInput).set_Text(WebBrowser?.Address ?? "");
				}
			});
			((TextInputBase)_addressInput).set_Text(WebBrowser?.Address ?? "");
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Visible(WebBrowser?.IsLoading ?? false);
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

		private void HandleLoading(object sender, LoadingStateChangedEventArgs e)
		{
			if (((Control)_fowardBtn).get_Visible() != e.CanGoForward || ((Control)_backBtn).get_Visible() != e.CanGoBack)
			{
				((Control)_backBtn).set_Visible(e.CanGoBack);
				((Control)_fowardBtn).set_Visible(e.CanGoForward);
				((Control)this).RecalculateLayout();
				RecalculateAddressInputWidth();
			}
			((Control)_loading).set_Visible(e.IsLoading);
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

		private void HandleAddress(object sender, AddressChangedEventArgs e)
		{
			((TextInputBase)_addressInput).set_Text(e.Address);
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
			if (WebBrowser != null)
			{
				WebBrowser.LoadingStateChanged -= HandleLoading;
				WebBrowser.AddressChanged -= HandleAddress;
			}
			this.BookmarkBtnClicked = null;
		}
	}
}
