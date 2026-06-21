using System;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class GameThreadUtil
	{
		public static void Enqueue(Action action)
		{
			Action action2 = action;
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				action2();
			});
		}
	}
}
