using System.Drawing;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Intern;

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
			while (GameService.Input.get_Keyboard().get_KeysDown().Count > 0)
			{
				await Task.Delay(250);
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
