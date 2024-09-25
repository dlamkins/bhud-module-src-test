using System;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	public struct KeyboardInput
	{
		internal short wVk;

		internal short wScan;

		internal KeyEventF dwFlags;

		internal int time;

		internal UIntPtr dwExtraInfo;
	}
}
