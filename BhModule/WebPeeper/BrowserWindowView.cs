using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	public class BrowserWindowView : View
	{
		private Container _window;

		private WindowContent _windowContent;

		protected override void Build(Container window)
		{
			try
			{
				_window = window;
				if (WebPeeperModule.Instance.CefService.CreateWebBrowser().Wait(TimeSpan.FromSeconds(30.0)))
				{
					WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
					{
						//IL_0011: Unknown result type (might be due to invalid IL or missing references)
						//IL_0016: Unknown result type (might be due to invalid IL or missing references)
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						BrowserWindowView browserWindowView = this;
						Rectangle contentRegion = _window.get_ContentRegion();
						WindowContent windowContent = new WindowContent(((Rectangle)(ref contentRegion)).get_Size());
						((Control)windowContent).set_Parent(window);
						browserWindowView._windowContent = windowContent;
						((Control)_window).add_Resized((EventHandler<ResizedEventArgs>)OnWindowResize);
					});
				}
			}
			catch (Exception ex)
			{
				WebPeeperModule.Logger.Error(ex.Message);
			}
		}

		private void OnWindowResize(object sender, ResizedEventArgs evt)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			WindowContent windowContent = _windowContent;
			Rectangle contentRegion = _window.get_ContentRegion();
			((Control)windowContent).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}

		protected override void Unload()
		{
			((Control)_window).remove_Resized((EventHandler<ResizedEventArgs>)OnWindowResize);
			WindowContent windowContent = _windowContent;
			if (windowContent != null)
			{
				((Control)windowContent).Dispose();
			}
		}

		public BrowserWindowView()
			: this()
		{
		}
	}
}
