using System;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	public struct MouseInput
	{
		internal int dx;

		internal int dy;

		internal int mouseData;

		internal MouseEventF dwFlags;

		internal uint time;

		internal UIntPtr dwExtraInfo;
	}
}
