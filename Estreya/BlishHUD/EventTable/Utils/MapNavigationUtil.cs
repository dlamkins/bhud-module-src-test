using System;
using System.Drawing;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Estreya.BlishHUD.EventTable.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Utils
{
	public static class MapNavigationUtil
	{
		public enum ChangeMapLayerDirection
		{
			Up,
			Down
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(MapNavigationUtil));

		private static double GetDistance(double x1, double y1, double x2, double y2)
		{
			return GetDistance(x2 - x1, y2 - y1);
		}

		private static double GetDistance(double offsetX, double offsetY)
		{
			return Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0));
		}

		private static async Task WaitForTick(int ticks = 1)
		{
			int tick = GameService.Gw2Mumble.get_Tick();
			while (GameService.Gw2Mumble.get_Tick() - tick < ticks * 2)
			{
				await Task.Delay(10);
			}
		}

		public static async Task<bool> OpenFullscreenMap()
		{
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return true;
			}
			Keyboard.Stroke((VirtualKeyShort)77, false);
			await Task.Delay(500);
			return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		public static async Task<bool> CloseFullscreenMap()
		{
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return true;
			}
			Keyboard.Press((VirtualKeyShort)27, false);
			await WaitForTick(2);
			return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		private static async Task<bool> Zoom(double requiredZoomLevel, int steps)
		{
			int maxTries = 10;
			int remainingTries = maxTries;
			double startZoom = GetMapScale();
			bool isZoomIn = steps > 0;
			while (isZoomIn ? (startZoom > requiredZoomLevel) : (startZoom < requiredZoomLevel))
			{
				await WaitForTick(2);
				if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen())
				{
					Logger.Debug("User closed map.");
					return false;
				}
				Mouse.RotateWheel(steps, false, -1, -1, false);
				Mouse.RotateWheel(steps, false, -1, -1, false);
				Mouse.RotateWheel(steps, false, -1, -1, false);
				Mouse.RotateWheel(steps, false, -1, -1, false);
				await WaitForTick();
				double zoomAterScroll = GetMapScale();
				Logger.Debug($"Scrolled from {startZoom} to {zoomAterScroll}");
				if (startZoom == zoomAterScroll)
				{
					remainingTries--;
					if (remainingTries <= 0)
					{
						return false;
					}
				}
				else
				{
					remainingTries = maxTries;
				}
				startZoom = zoomAterScroll;
			}
			return true;
		}

		public static Task<bool> ZoomOut(double requiredZoomLevel)
		{
			return Zoom(requiredZoomLevel, -2147483647);
		}

		public static Task<bool> ZoomIn(double requiredZoomLevel)
		{
			return Zoom(requiredZoomLevel, int.MaxValue);
		}

		private static double GetMapScale()
		{
			return GameService.Gw2Mumble.get_UI().get_MapScale() * (double)GameService.Graphics.get_UIScaleMultiplier();
		}

		private static async Task<bool> MoveMap(double x, double y, double targetDistance)
		{
			while (true)
			{
				await WaitForTick(2);
				if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen())
				{
					Logger.Debug("User closed map.");
					return false;
				}
				Coordinates2 mapPos = GameService.Gw2Mumble.get_UI().get_MapCenter();
				double offsetX = ((Coordinates2)(ref mapPos)).get_X() - x;
				double offsetY = ((Coordinates2)(ref mapPos)).get_Y() - y;
				Logger.Debug($"Distance remaining: {GetDistance(((Coordinates2)(ref mapPos)).get_X(), ((Coordinates2)(ref mapPos)).get_Y(), x, y)}");
				Logger logger = Logger;
				Coordinates2 mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				object arg = ((Coordinates2)(ref mapPosition)).get_X();
				mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				logger.Debug($"Map Position: {arg}, {((Coordinates2)(ref mapPosition)).get_Y()}");
				if (Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0)) < targetDistance)
				{
					break;
				}
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, false);
				Point startPos = Mouse.GetPosition();
				Mouse.Press((MouseButton)1, -1, -1, false);
				Mouse.SetPosition(startPos.X + (int)MathHelper.Clamp((float)offsetX / (float)(GetMapScale() * 0.9), -100000f, 100000f), startPos.Y + (int)MathHelper.Clamp((float)offsetY / (float)(GetMapScale() * 0.9), -100000f, 100000f), false);
				await WaitForTick();
				startPos = Mouse.GetPosition();
				Mouse.SetPosition(startPos.X + (int)MathHelper.Clamp((float)offsetX / (float)(GetMapScale() * 0.9), -100000f, 100000f), startPos.Y + (int)MathHelper.Clamp((float)offsetY / (float)(GetMapScale() * 0.9), -100000f, 100000f), false);
				Mouse.Release((MouseButton)1, -1, -1, false);
				await Task.Delay(20);
			}
			return true;
		}

		public static async Task ChangeMapLayer(ChangeMapLayerDirection direction)
		{
			Keyboard.Press((VirtualKeyShort)16, false);
			await Task.Delay(10);
			Mouse.RotateWheel(int.MaxValue * ((direction == ChangeMapLayerDirection.Up) ? 1 : (-1)), false, -1, -1, false);
			await Task.Delay(10);
			Keyboard.Release((VirtualKeyShort)16, false);
		}

		public static Task<bool> NavigateToPosition(ContinentFloorRegionMapPoi poi)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 coord = poi.get_Coord();
			double x = ((Coordinates2)(ref coord)).get_X();
			coord = poi.get_Coord();
			return NavigateToPosition(x, ((Coordinates2)(ref coord)).get_Y(), poi.get_Type() == ApiEnum<PoiType>.op_Implicit((PoiType)2));
		}

		public static Task<bool> NavigateToPosition(double x, double y)
		{
			return NavigateToPosition(x, y, isWaypoint: false);
		}

		private static async Task<bool> NavigateToPosition(double x, double y, bool isWaypoint)
		{
			_ = 10;
			try
			{
				ScreenNotification.ShowNotification(new string[2] { "DO NOT MOVE THE CURSOR!", "Close map to cancel." }, (NotificationType)1, null, 7);
				if (!(await OpenFullscreenMap()))
				{
					Logger.Debug("Could not open map.");
				}
				await WaitForTick();
				Coordinates2 mapPos = GameService.Gw2Mumble.get_UI().get_MapCenter();
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, false);
				if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1206)
				{
					await ChangeMapLayer(ChangeMapLayerDirection.Down);
				}
				if (!(await ZoomOut(6.0)))
				{
					Logger.Debug("Zooming out did not work.");
					return false;
				}
				double totalDist = GetDistance(((Coordinates2)(ref mapPos)).get_X(), ((Coordinates2)(ref mapPos)).get_Y(), x, y) / (GetMapScale() * 0.9);
				Logger.Debug($"Distance: {totalDist}");
				if (!(await MoveMap(x, y, 50.0)))
				{
					Logger.Debug("Moving the map did not work.");
					return false;
				}
				await WaitForTick();
				int finalMouseX2 = GameService.Graphics.get_WindowWidth() / 2;
				int finalMouseY2 = GameService.Graphics.get_WindowHeight() / 2;
				Logger.Debug($"Set mouse on waypoint: x = {finalMouseX2}, y = {finalMouseY2}");
				Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
				if (!(await ZoomIn(2.0)))
				{
					Logger.Debug("Zooming in did not work.");
					return false;
				}
				if (!(await MoveMap(x, y, 5.0)))
				{
					Logger.Debug("Moving the map did not work.");
					return false;
				}
				if (isWaypoint)
				{
					Logger.Debug($"Set mouse on waypoint: x = {finalMouseX2}, y = {finalMouseY2}");
					Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
					await Task.Delay(50);
					Mouse.Click((MouseButton)0, -1, -1, false);
					await Task.Delay(50);
					finalMouseX2 -= 50;
					finalMouseY2 += 10;
					Logger.Debug($"Set mouse on waypoint yes button: x = {finalMouseX2}, y = {finalMouseY2}");
					Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
					if (EventTableModule.ModuleInstance.ModuleSettings.DirectlyTeleportToWaypoint.get_Value())
					{
						await Task.Delay(150);
						Mouse.Click((MouseButton)0, -1, -1, false);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Navigation to position failed:");
				return false;
			}
		}
	}
}
