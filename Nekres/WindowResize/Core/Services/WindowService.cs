using System;
using System.Drawing;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Nekres.WindowResize.Core.Services
{
	internal class WindowService : IDisposable
	{
		private bool _isWindowedMode;

		private double elapsedTime;

		private const double UPDATE_INTERVAL = 200.0;

		public bool IsWindowedMode
		{
			get
			{
				return _isWindowedMode;
			}
			set
			{
				if (_isWindowedMode != value)
				{
					_isWindowedMode = value;
					this.IsWindowedModeChanged?.Invoke(this, new ValueEventArgs<bool>(value));
				}
			}
		}

		public event EventHandler<ValueEventArgs<bool>> IsWindowedModeChanged;

		public WindowService()
		{
			IsWindowedModeChanged += OnIsWindowedModeChanged;
		}

		private void OnIsWindowedModeChanged(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Size size = WindowUtil.ParseSize(WindowResize.Instance.WindowSize.get_Value());
				WindowUtil.ResizeAndCenterWindow(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), size.Width, size.Height);
			}
		}

		public void Update(GameTime gameTime)
		{
			elapsedTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (!(elapsedTime < 200.0))
			{
				elapsedTime = 0.0;
				CheckWindowMode(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
			}
		}

		private void CheckWindowMode(IntPtr hWnd)
		{
			if (!(hWnd == IntPtr.Zero))
			{
				IsWindowedMode = WindowUtil.IsWindowedMode(hWnd);
			}
		}

		public void Dispose()
		{
			IsWindowedModeChanged -= OnIsWindowedModeChanged;
		}
	}
}
