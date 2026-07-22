using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Intern;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Characters.Services
{
	public static class ExtendedInputService
	{
		public static void MouseWiggle()
		{
			Point p = Blish_HUD.Controls.Intern.Mouse.GetPosition();
			Blish_HUD.Controls.Intern.Mouse.SetPosition(p.X, p.Y);
			Blish_HUD.Controls.Intern.Mouse.SetPosition(p.X, p.Y, sendToSystem: true);
		}

		public static async Task<bool> WaitForNoKeyPressed(double maxDelay = 5000.0)
		{
			double start = GameService.Overlay.CurrentGameTime.TotalGameTime.TotalMilliseconds;
			IEnumerable<Keys> keys = GameService.Input.Keyboard.KeysDown.Except(new List<Keys> { Keys.None });
			while (keys.Count() > 0)
			{
				await Task.Delay(250);
				BaseModule<Characters, MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info($"There are currently {keys.Count()} keys pressed. These keys are:");
				BaseModule<Characters, MainWindow, Settings, PathCollection, StaticHosting>.Logger.Info(string.Join(", ", keys));
				if (GameService.Overlay.CurrentGameTime.TotalGameTime.TotalMilliseconds - start >= maxDelay)
				{
					return false;
				}
			}
			await Task.Delay(25);
			return true;
		}
	}
}
