using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal static class Utils
	{
		public static readonly NotifyClass Notify = new NotifyClass();

		[DllImport("imm32.dll")]
		internal static extern bool ImmAssociateContextEx(IntPtr hWnd, int hIMC, int iace);

		[DllImport("imm32.dll")]
		internal static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("imm32.dll")]
		internal static extern bool ImmSetOpenStatus(IntPtr hWnd, bool isOpen);

		[DllImport("imm32.dll")]
		internal static extern bool ImmSetCompositionWindow(IntPtr hIMC, CompositionForm pos);

		[DllImport("imm32.dll")]
		internal static extern bool ImmSetCandidateWindow(IntPtr hIMC, CompositionForm pos);

		[DllImport("Imm32.dll")]
		internal static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("Imm32.dll")]
		internal static extern int ImmGetCompositionStringW(IntPtr hIMC, int gcs, byte[] buffer, int bufferLen);

		[DllImport("Imm32.dll")]
		internal static extern bool ImmNotifyIME(IntPtr hIMC, uint action, uint index, uint value);

		[DllImport("user32.dll")]
		internal static extern int GetKeyboardLayout(uint threadId);

		[DllImport("user32.dll")]
		internal static extern int SetFocus(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetFocus();

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, UIntPtr dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
		public static extern bool NativeSetForegroundWindow(IntPtr hWnd);

		public static bool SetForegroundWindow(IntPtr hWnd)
		{
			keybd_event(0, 0, 0, 0);
			bool result = NativeSetForegroundWindow(hWnd);
			SetFocus(hWnd);
			return result;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

		[DllImport("kernel32.dll")]
		public static extern bool SetDllDirectory(string lpPathName);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesRead);

		public static bool IsForeground(this GameWindow window)
		{
			return GetForegroundWindow() == window.get_Handle();
		}

		public static void SafeInvoke(this Form form, Action cb)
		{
			if (!form.IsDisposed && form.IsHandleCreated)
			{
				form.Invoke(cb);
			}
		}
	}
}
