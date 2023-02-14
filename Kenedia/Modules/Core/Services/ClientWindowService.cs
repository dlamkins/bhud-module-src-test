using System;
using Blish_HUD;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Services
{
	public class ClientWindowService
	{
		private double _resolutionTick;

		public bool Enabled { get; set; } = true;


		public int TitleBarHeight { get; set; }

		public int SideBarWidth { get; set; }

		public User32Dll.RECT ClientBounds { get; set; }

		public User32Dll.RECT WindowBounds { get; set; }

		public void Run(GameTime gameTime)
		{
			if (!Enabled || !(gameTime.get_TotalGameTime().TotalMilliseconds - _resolutionTick >= 50.0))
			{
				return;
			}
			_resolutionTick = gameTime.get_TotalGameTime().TotalMilliseconds;
			IntPtr hWnd = GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle();
			User32Dll.RECT newRect = default(User32Dll.RECT);
			if (User32Dll.GetWindowRect(hWnd, ref newRect) && !WindowBounds.Matches(newRect))
			{
				WindowBounds = newRect;
				if (User32Dll.GetClientRect(hWnd, out var rect))
				{
					ClientBounds = rect;
				}
				TitleBarHeight = WindowBounds.Bottom - WindowBounds.Top - (ClientBounds.Bottom - ClientBounds.Top);
				SideBarWidth = WindowBounds.Right - WindowBounds.Left - (ClientBounds.Right - ClientBounds.Left);
			}
		}
	}
}
