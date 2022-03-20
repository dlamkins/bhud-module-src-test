using System;
using System.Drawing;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Intern;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility
{
	public static class MapNavUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(MapNavUtil));

		private static double GetDistance(double x1, double y1, double x2, double y2)
		{
			return GetDistance(x2 - x1, y2 - y1);
		}

		private static double GetDistance(double offsetX, double offsetY)
		{
			return Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0));
		}

		private static async Task WaitForTick()
		{
			int tick = GameService.Gw2Mumble.get_Tick();
			while (GameService.Gw2Mumble.get_Tick() - tick < 2)
			{
				await Task.Delay(10);
			}
		}

		public static async Task<bool> NavigateToPosition(double x, double y, double zoom)
		{
			Coordinates2 mapPos = GameService.Gw2Mumble.get_UI().get_MapCenter();
			GameService.Gw2Mumble.get_UI().get_MapScale();
			Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, false);
			Point startPos = Mouse.GetPosition();
			while (GameService.Gw2Mumble.get_UI().get_MapScale() < 12.0)
			{
				Mouse.RotateWheel(-2147483647, false, -1, -1, false);
				Mouse.RotateWheel(-2147483647, false, -1, -1, false);
				Mouse.RotateWheel(-2147483647, false, -1, -1, false);
				Mouse.RotateWheel(-2147483647, false, -1, -1, false);
				await WaitForTick();
			}
			double totalDist = GetDistance(((Coordinates2)(ref mapPos)).get_X(), ((Coordinates2)(ref mapPos)).get_Y(), x, y) / (GameService.Gw2Mumble.get_UI().get_MapScale() * 0.9);
			Logger.Debug($"Distance: {totalDist}");
			if (Math.Sqrt(Math.Pow(((Coordinates2)(ref mapPos)).get_X() - x, 2.0)) / GameService.Gw2Mumble.get_UI().get_MapScale() > (double)((float)GameService.Graphics.get_WindowWidth() / 2f))
			{
				Logger.Debug("Point is off horizontally");
			}
			if (Math.Sqrt(Math.Pow(((Coordinates2)(ref mapPos)).get_Y() - y, 2.0)) / GameService.Gw2Mumble.get_UI().get_MapScale() > (double)((float)GameService.Graphics.get_WindowHeight() / 2f))
			{
				Logger.Debug("Point is off vertically");
			}
			while (true)
			{
				mapPos = GameService.Gw2Mumble.get_UI().get_MapCenter();
				double offsetX = ((Coordinates2)(ref mapPos)).get_X() - x;
				double offsetY = ((Coordinates2)(ref mapPos)).get_Y() - y;
				double num = Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0));
				Logger.Debug($"Distance remaining: {GetDistance(((Coordinates2)(ref mapPos)).get_X(), ((Coordinates2)(ref mapPos)).get_Y(), x, y)}");
				Logger logger = Logger;
				Coordinates2 mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				object arg = ((Coordinates2)(ref mapPosition)).get_X();
				mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				logger.Debug($"Map Position: {arg}, {((Coordinates2)(ref mapPosition)).get_Y()}");
				if (num < 5.0)
				{
					break;
				}
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, false);
				Mouse.Press((MouseButton)1, -1, -1, false);
				Mouse.SetPosition(startPos.X + (int)MathHelper.Clamp((float)offsetX / (float)(GameService.Gw2Mumble.get_UI().get_MapScale() * 0.9), -100000f, 100000f), startPos.Y + (int)MathHelper.Clamp((float)offsetY / (float)(GameService.Gw2Mumble.get_UI().get_MapScale() * 0.9), -100000f, 100000f), false);
				await WaitForTick();
				Mouse.SetPosition(startPos.X + (int)MathHelper.Clamp((float)offsetX / (float)(GameService.Gw2Mumble.get_UI().get_MapScale() * 0.9), -100000f, 100000f), startPos.Y + (int)MathHelper.Clamp((float)offsetY / (float)(GameService.Gw2Mumble.get_UI().get_MapScale() * 0.9), -100000f, 100000f), false);
				Mouse.Release((MouseButton)1, -1, -1, false);
				await Task.Delay(1000);
			}
			Mouse.Click((MouseButton)0, -1, -1, false);
			return true;
		}
	}
}
