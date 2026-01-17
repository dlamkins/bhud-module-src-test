using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;

namespace BhModule.WebPeeper
{
	public class UIService
	{
		private CornerIcon _browserCornerIcon;

		private BrowserWindow _browserWindow;

		public BrowserWindow BrowserWindow => _browserWindow;

		public void Load()
		{
			BuildBrowserWindow();
			BuildCornerIcon();
		}

		public void Unload()
		{
			((Control)_browserWindow).Dispose();
			((Control)_browserCornerIcon).Dispose();
		}

		public void ToggleBrowser()
		{
			((StandardWindow)_browserWindow).ToggleWindow((IView)(object)new BrowserWindowView());
		}

		public void ToggleSettings()
		{
			MenuItem instanceSettingsMenuItem = WebPeeperModule.InstanceSettingsMenuItem;
			if (((Control)GameService.Overlay.get_BlishHudWindow()).get_Visible() && instanceSettingsMenuItem.get_Selected())
			{
				((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
				return;
			}
			((Control)GameService.Overlay.get_BlishHudWindow()).Show();
			instanceSettingsMenuItem.Select();
		}

		private void BuildBrowserWindow()
		{
			_browserWindow = new BrowserWindow();
		}

		private void BuildCornerIcon()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			_ = GameService.Content;
			CornerIcon val = new CornerIcon(AsyncTexture2D.op_Implicit(WebPeeperModule.Instance.ContentsManager.GetTexture("logo.png")), AsyncTexture2D.op_Implicit(WebPeeperModule.Instance.ContentsManager.GetTexture("logo-hover.png")), WebPeeperModule.InstanceModuleManager.get_Manifest().get_Name());
			val.set_Priority(int.MaxValue);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_browserCornerIcon = val;
			((Control)_browserCornerIcon).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				ToggleBrowser();
			});
			((Control)_browserCornerIcon).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetContextMenuItems));
		}

		[IteratorStateMachine(typeof(_003CGetContextMenuItems_003Ed__10))]
		private IEnumerable<ContextMenuStripItem> GetContextMenuItems()
		{
			return new _003CGetContextMenuItems_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}
	}
}
