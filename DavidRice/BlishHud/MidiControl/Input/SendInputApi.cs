using System;
using System.Runtime.InteropServices;

namespace DavidRice.BlishHud.MidiControl.Input
{
	public static class SendInputApi
	{
		private const uint INPUT_KEYBOARD = 1u;

		private const uint KEYEVENTF_SCANCODE = 8u;

		private const uint KEYEVENTF_KEYUP = 2u;

		private static readonly int InputSize = Marshal.SizeOf<INPUT>();

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

		public static void SendKeyTap(uint scanCode)
		{
			ValidateScanCode(scanCode);
			INPUT[] inputs = new INPUT[2]
			{
				CreateKeyboardInput(scanCode, keyUp: false),
				CreateKeyboardInput(scanCode, keyUp: true)
			};
			SendInput((uint)inputs.Length, inputs, InputSize);
		}

		public static void SendKeyUp(uint scanCode)
		{
			ValidateScanCode(scanCode);
			INPUT input = CreateKeyboardInput(scanCode, keyUp: true);
			SendInput(1u, new INPUT[1] { input }, InputSize);
		}

		private static void ValidateScanCode(uint scanCode)
		{
			if (scanCode == 0 || scanCode > 65535)
			{
				throw new ArgumentOutOfRangeException("scanCode", "Scan code must be between 1 and 65535.");
			}
		}

		private static INPUT CreateKeyboardInput(uint scanCode, bool keyUp)
		{
			uint flags = 8u;
			if (keyUp)
			{
				flags |= 2u;
			}
			INPUT result = default(INPUT);
			result.type = 1u;
			result.u = new INPUT_UNION
			{
				ki = new KEYBDINPUT
				{
					wVk = 0,
					wScan = (ushort)scanCode,
					dwFlags = flags,
					time = 0u,
					dwExtraInfo = UIntPtr.Zero
				}
			};
			return result;
		}
	}
}
