using System;
using System.Diagnostics;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FarmingTracker
{
	public class OpenUrlInBrowserButton : StandardButton
	{
		public OpenUrlInBrowserButton(string url, string buttonText, string buttonTooltip, Texture2D buttonIcon, Container parent)
			: this()
		{
			((StandardButton)this).set_Text(buttonText);
			((Control)this).set_BasicTooltipText(buttonTooltip);
			((StandardButton)this).set_Icon(AsyncTexture2D.op_Implicit(buttonIcon));
			((Control)this).set_Width(300);
			((Control)this).set_Parent(parent);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			});
		}
	}
}
