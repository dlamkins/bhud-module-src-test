using System;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace Charr.Timers_BlishHUD.Controls
{
	public interface IAlertPanel : IDisposable
	{
		float MaxFill { get; set; }

		float CurrentFill { get; set; }

		string Text { get; set; }

		string TimerText { get; set; }

		Color TextColor { get; set; }

		Color FillColor { get; set; }

		Color TimerTextColor { get; set; }

		bool ShouldShow { get; set; }

		AsyncTexture2D Icon { get; set; }
	}
}
