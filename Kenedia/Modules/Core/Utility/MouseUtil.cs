using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Blish_HUD;
using Kenedia.Modules.Core.Utility.WinApi;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Utility
{
	internal class MouseUtil
	{
		public enum MouseInputType
		{
			System,
			Background
		}

		public enum MouseButton
		{
			LEFT,
			RIGHT,
			MIDDLE,
			XBUTTON
		}

		[Flags]
		internal enum MouseEventF : uint
		{
			ABSOLUTE = 0x8000u,
			HWHEEL = 0x1000u,
			MOVE = 0x1u,
			MOVE_NOCOALESCE = 0x2000u,
			LEFTDOWN = 0x2u,
			LEFTUP = 0x4u,
			RIGHTDOWN = 0x8u,
			RIGHTUP = 0x10u,
			MIDDLEDOWN = 0x20u,
			MIDDLEUP = 0x40u,
			VIRTUALDESK = 0x4000u,
			WHEEL = 0x800u,
			XDOWN = 0x80u,
			XUP = 0x100u
		}

		internal struct MouseInput
		{
			internal int _dx;

			internal int _dy;

			internal int _mouseData;

			internal MouseEventF _dwFlags;

			internal uint _time;

			internal UIntPtr _dwExtraInfo;
		}

		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		private struct POINT
		{
			public int X;

			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}

		private const uint WM_MOUSEWHEEL = 522u;

		private const uint WM_MOUSEHWHEEL = 526u;

		private const int WHEEL_DELTA = 120;

		private const uint WM_MOUSEMOVE = 512u;

		private static readonly Dictionary<MouseButton, MouseEventF> ButtonPress = new Dictionary<MouseButton, MouseEventF>
		{
			{
				MouseButton.LEFT,
				MouseEventF.LEFTDOWN
			},
			{
				MouseButton.RIGHT,
				MouseEventF.RIGHTDOWN
			},
			{
				MouseButton.MIDDLE,
				MouseEventF.MIDDLEDOWN
			},
			{
				MouseButton.XBUTTON,
				MouseEventF.XDOWN
			}
		};

		private static readonly Dictionary<MouseButton, MouseEventF> ButtonRelease = new Dictionary<MouseButton, MouseEventF>
		{
			{
				MouseButton.LEFT,
				MouseEventF.LEFTUP
			},
			{
				MouseButton.RIGHT,
				MouseEventF.RIGHTUP
			},
			{
				MouseButton.MIDDLE,
				MouseEventF.MIDDLEUP
			},
			{
				MouseButton.XBUTTON,
				MouseEventF.XUP
			}
		};

		private static readonly Dictionary<MouseButton, short> VirtualButtonShort = new Dictionary<MouseButton, short>
		{
			{
				MouseButton.LEFT,
				1
			},
			{
				MouseButton.RIGHT,
				2
			},
			{
				MouseButton.MIDDLE,
				4
			},
			{
				MouseButton.XBUTTON,
				5
			}
		};

		private static readonly Dictionary<MouseButton, uint> WM_BUTTONDOWN = new Dictionary<MouseButton, uint>
		{
			{
				MouseButton.LEFT,
				513u
			},
			{
				MouseButton.RIGHT,
				516u
			},
			{
				MouseButton.MIDDLE,
				519u
			},
			{
				MouseButton.XBUTTON,
				523u
			}
		};

		private static readonly Dictionary<MouseButton, uint> WM_BUTTONUP = new Dictionary<MouseButton, uint>
		{
			{
				MouseButton.LEFT,
				514u
			},
			{
				MouseButton.RIGHT,
				517u
			},
			{
				MouseButton.MIDDLE,
				520u
			},
			{
				MouseButton.XBUTTON,
				524u
			}
		};

		private static readonly Dictionary<MouseButton, uint> WM_BUTTONDBLCLK = new Dictionary<MouseButton, uint>
		{
			{
				MouseButton.LEFT,
				515u
			},
			{
				MouseButton.RIGHT,
				518u
			},
			{
				MouseButton.MIDDLE,
				521u
			},
			{
				MouseButton.XBUTTON,
				525u
			}
		};

		[DllImport("user32.Dll", SetLastError = true)]
		private static extern long SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("user32.dll")]
		private static extern uint SendInput(uint nInputs, [In][MarshalAs(UnmanagedType.LPArray)] Kenedia.Modules.Core.Utility.WinApi.Input[] pInputs, int cbSize);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		public static void Press(MouseButton button, int xPos = -1, int yPos = -1, bool sendToSystem = false)
		{
			if (xPos == -1 || yPos == -1)
			{
				Point pos = GetPosition();
				xPos = pos.X;
				yPos = pos.Y;
			}
			if (!GameService.GameIntegration.Gw2Instance.Gw2IsRunning || sendToSystem)
			{
				Kenedia.Modules.Core.Utility.WinApi.Input[] nInputs = new Kenedia.Modules.Core.Utility.WinApi.Input[1]
				{
					new Kenedia.Modules.Core.Utility.WinApi.Input
					{
						_type = InputType.MOUSE,
						_u = new InputUnion
						{
							_mi = new MouseInput
							{
								_dx = xPos,
								_dy = yPos,
								_mouseData = 0,
								_dwFlags = ButtonPress[button],
								_time = 0u
							}
						}
					}
				};
				SendInput((uint)nInputs.Length, nInputs, Kenedia.Modules.Core.Utility.WinApi.Input.Size);
			}
			else
			{
				uint wParam = (uint)VirtualButtonShort[button];
				int lParam = xPos | (yPos << 16);
				PostMessage(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, WM_BUTTONDOWN[button], wParam, lParam);
			}
		}

		public static void Release(MouseButton button, int xPos = -1, int yPos = -1, bool sendToSystem = false)
		{
			if (xPos == -1 || yPos == -1)
			{
				Point pos = GetPosition();
				xPos = pos.X;
				yPos = pos.Y;
			}
			if (!GameService.GameIntegration.Gw2Instance.Gw2IsRunning || sendToSystem)
			{
				Kenedia.Modules.Core.Utility.WinApi.Input[] nInputs = new Kenedia.Modules.Core.Utility.WinApi.Input[1]
				{
					new Kenedia.Modules.Core.Utility.WinApi.Input
					{
						_type = InputType.MOUSE,
						_u = new InputUnion
						{
							_mi = new MouseInput
							{
								_dx = xPos,
								_dy = yPos,
								_mouseData = 0,
								_dwFlags = ButtonRelease[button],
								_time = 0u
							}
						}
					}
				};
				SendInput((uint)nInputs.Length, nInputs, Kenedia.Modules.Core.Utility.WinApi.Input.Size);
			}
			else
			{
				uint wParam = (uint)VirtualButtonShort[button];
				int lParam = xPos | (yPos << 16);
				PostMessage(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, WM_BUTTONUP[button], wParam, lParam);
			}
		}

		public static void RotateWheel(int wheelDistance, bool horizontalWheel = false, int xPos = -1, int yPos = -1, bool sendToSystem = false)
		{
			wheelDistance %= 120;
			if (wheelDistance != 0)
			{
				if (xPos == -1 || yPos == -1)
				{
					Point pos = GetPosition();
					xPos = pos.X;
					yPos = pos.Y;
				}
				if (!GameService.GameIntegration.Gw2Instance.Gw2IsRunning || sendToSystem)
				{
					Kenedia.Modules.Core.Utility.WinApi.Input[] nInputs = new Kenedia.Modules.Core.Utility.WinApi.Input[1]
					{
						new Kenedia.Modules.Core.Utility.WinApi.Input
						{
							_type = InputType.MOUSE,
							_u = new InputUnion
							{
								_mi = new MouseInput
								{
									_dx = xPos,
									_dy = yPos,
									_mouseData = wheelDistance,
									_dwFlags = (horizontalWheel ? MouseEventF.HWHEEL : MouseEventF.WHEEL),
									_time = 0u
								}
							}
						}
					};
					SendInput((uint)nInputs.Length, nInputs, Kenedia.Modules.Core.Utility.WinApi.Input.Size);
				}
				else
				{
					uint wParam = 0u | (uint)(wheelDistance << 16);
					int lParam = xPos | (yPos << 16);
					PostMessage(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, horizontalWheel ? 526u : 522u, wParam, lParam);
				}
			}
		}

		public static void SetPosition(int xPos, int yPos, bool sendToSystem = false)
		{
			if (!GameService.GameIntegration.Gw2Instance.Gw2IsRunning || sendToSystem)
			{
				SetCursorPos(xPos, yPos);
				return;
			}
			int lParam = xPos | (yPos << 16);
			PostMessage(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, 512u, 0u, lParam);
		}

		public static void SetPosition(Point point, bool sendToSystem = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SetPosition(point.X, point.Y, sendToSystem);
		}

		public static Point GetPosition()
		{
			IntPtr hWnd = GameService.GameIntegration.Gw2Instance.Gw2WindowHandle;
			if (!GetCursorPos(out var pos) || !ScreenToClient(hWnd, ref pos) || !GetWindowRect(hWnd, out var wndBounds) || !GetClientRect(hWnd, out var clientBounds))
			{
				return Point.Empty;
			}
			int widthOffset = wndBounds.Right - wndBounds.Left - (clientBounds.Right - clientBounds.Left);
			int heightOffset = wndBounds.Bottom - wndBounds.Top - (clientBounds.Bottom - clientBounds.Top);
			pos.X -= wndBounds.Left + widthOffset;
			pos.Y -= wndBounds.Top + heightOffset;
			if (ClientToScreen(hWnd, ref pos))
			{
				return new Point(pos.X, pos.Y);
			}
			return Point.Empty;
		}

		public static void Click(MouseButton button, int xPos = -1, int yPos = -1, bool sendToSystem = false)
		{
			Press(button, xPos, yPos, sendToSystem);
			Release(button, xPos, yPos, sendToSystem);
		}

		public static void Click(MouseButton button, Point point, bool sendToSystem)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Click(button, point.X, point.Y, sendToSystem);
		}

		public static void DoubleClick(MouseButton button, int xPos = -1, int yPos = -1, bool sendToSystem = false)
		{
			if (!GameService.GameIntegration.Gw2Instance.Gw2IsRunning || sendToSystem)
			{
				for (int j = 0; j <= 1; j++)
				{
					Press(button, xPos, yPos, sendToSystem);
					Release(button, xPos, yPos, sendToSystem);
				}
				return;
			}
			if (xPos == -1 || yPos == -1)
			{
				Point pos = GetPosition();
				xPos = pos.X;
				yPos = pos.Y;
			}
			uint wParam = (uint)VirtualButtonShort[button];
			int lParam = xPos | (yPos << 16);
			for (int i = 0; i <= 1; i++)
			{
				Press(button, xPos, yPos);
				Release(button, xPos, yPos);
			}
			PostMessage(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, WM_BUTTONUP[button], wParam, lParam);
		}

		public static void DoubleClick(MouseButton button, Point point, bool sendToSystem)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			DoubleClick(button, point.X, point.Y, sendToSystem);
		}
	}
}
