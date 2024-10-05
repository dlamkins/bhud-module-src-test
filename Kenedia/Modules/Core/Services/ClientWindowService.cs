using System;
using Blish_HUD;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Services
{
	public class ClientWindowService
	{
		private double _resolutionTick;

		public bool Enabled { get; set; } = true;


		public int TitleBarHeight { get; set; }

		public int SideBarWidth { get; set; }

		public Point Resolution { get; private set; }

		public User32Dll.RECT ClientBounds { get; set; }

		public User32Dll.RECT WindowBounds { get; set; }

		public event EventHandler<ValueChangedEventArgs<Point>> ResolutionChanged;

		public void Run(GameTime gameTime)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			if (!Enabled || !(gameTime.get_TotalGameTime().TotalMilliseconds - _resolutionTick >= 50.0))
			{
				return;
			}
			_resolutionTick = gameTime.get_TotalGameTime().TotalMilliseconds;
			MouseState state = GameService.Input.Mouse.State;
			if ((int)((MouseState)(ref state)).get_LeftButton() != 1)
			{
				Point resolution = Resolution;
				if (!((Point)(ref resolution)).Equals(GameService.Graphics.Resolution))
				{
					Point prev = Resolution;
					Resolution = GameService.Graphics.Resolution;
					this.ResolutionChanged?.Invoke(this, new ValueChangedEventArgs<Point>(prev, Resolution));
				}
			}
			IntPtr hWnd = GameService.GameIntegration.Gw2Instance.Gw2WindowHandle;
			if (User32Dll.GetWindowRect(hWnd, out var newRect) && !WindowBounds.Matches(newRect))
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
