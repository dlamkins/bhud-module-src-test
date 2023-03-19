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
			Point p = Mouse.GetPosition();
			Mouse.SetPosition(p.X, p.Y, false);
			Mouse.SetPosition(p.X, p.Y, true);
		}

		public static async Task<bool> WaitForNoKeyPressed(double maxDelay = 5000.0)
		{
			double start = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			IEnumerable<Keys> keys = GameService.Input.get_Keyboard().get_KeysDown().Except(new List<Keys> { (Keys)0 });
			while (keys.Count() > 0)
			{
				await Task.Delay(250);
				BaseModule<Characters, MainWindow, Settings>.Logger.Info($"There are currently {keys.Count()} keys pressed. These keys are:");
				BaseModule<Characters, MainWindow, Settings>.Logger.Info(string.Join(", ", keys));
				if (GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - start >= maxDelay)
				{
					return false;
				}
			}
			await Task.Delay(25);
			return true;
		}
	}
}
