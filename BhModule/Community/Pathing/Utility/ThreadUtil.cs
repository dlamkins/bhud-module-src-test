using System;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility
{
	internal static class ThreadUtil
	{
		public static void RunOnMainThread(Action call)
		{
			if (Program.get_IsMainThread())
			{
				call();
				return;
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				call();
			});
		}
	}
}
