using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Controls.Map;
using Estreya.BlishHUD.Shared.Extensions;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Utils
{
	public class MapUtil : IDisposable
	{
		public enum ChangeMapLayerDirection
		{
			Up,
			Down
		}

		public class NavigationResult
		{
			public bool Success { get; set; }

			public string Message { get; set; }

			public NavigationResult(bool success, string message)
			{
				Success = success;
				Message = message;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(MapUtil));

		private readonly KeyBinding _mapKeybinding;

		private readonly Gw2ApiManager _apiManager;

		private FlatMap _flatMap;

		public static int MouseMoveAndClickDelay { get; set; } = 50;


		public static int KeyboardPressDelay { get; set; } = 20;


		public MapUtil(KeyBinding mapKeybinding, Gw2ApiManager apiManager)
		{
			_mapKeybinding = mapKeybinding;
			_apiManager = apiManager;
			FlatMap flatMap = new FlatMap();
			((Control)flatMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_flatMap = flatMap;
		}

		private double GetDistance(double x1, double y1, double x2, double y2)
		{
			return GetDistance(x2 - x1, y2 - y1);
		}

		private double GetDistance(double offsetX, double offsetY)
		{
			return Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0));
		}

		private async Task WaitForTick(int ticks = 1)
		{
			int tick = GameService.Gw2Mumble.get_Tick();
			while (GameService.Gw2Mumble.get_Tick() - tick < ticks * 2)
			{
				await Task.Delay(10);
			}
		}

		public async Task WaitForMapClose(int delay = 10)
		{
			while (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				await Task.Delay(delay);
			}
		}

		public async Task<bool> OpenFullscreenMap()
		{
			if (!IsInGame())
			{
				Logger.Debug("Not in game");
				return false;
			}
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return true;
			}
			if ((int)_mapKeybinding.get_ModifierKeys() != 0)
			{
				Keyboard.Press(((Enum)(object)_mapKeybinding.get_ModifierKeys()).GetFlags().Select((Func<Enum, VirtualKeyShort>)((Enum flag) => (VirtualKeyShort)(object)flag)).Aggregate((VirtualKeyShort a, VirtualKeyShort b) => (VirtualKeyShort)(a | b)), false);
			}
			Keyboard.Stroke((VirtualKeyShort)(short)_mapKeybinding.get_PrimaryKey(), false);
			if ((int)_mapKeybinding.get_ModifierKeys() != 0)
			{
				Keyboard.Release(((Enum)(object)_mapKeybinding.get_ModifierKeys()).GetFlags().Select((Func<Enum, VirtualKeyShort>)((Enum flag) => (VirtualKeyShort)(object)flag)).Aggregate((VirtualKeyShort a, VirtualKeyShort b) => (VirtualKeyShort)(a | b)), false);
			}
			await Task.Delay(500);
			return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		public async Task<bool> CloseFullscreenMap()
		{
			if (!IsInGame())
			{
				Logger.Debug("Not in game");
				return false;
			}
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return true;
			}
			Keyboard.Press((VirtualKeyShort)27, false);
			await Task.Delay(500);
			return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		private bool IsInGame()
		{
			return GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
		}

		private async Task<bool> Zoom(double requiredZoomLevel, int steps)
		{
			if (!IsInGame())
			{
				Logger.Debug("Not in game");
				return false;
			}
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

		public Task<bool> ZoomOut(double requiredZoomLevel)
		{
			return Zoom(requiredZoomLevel, -2147483647);
		}

		public Task<bool> ZoomIn(double requiredZoomLevel)
		{
			return Zoom(requiredZoomLevel, int.MaxValue);
		}

		private double GetMapScale()
		{
			return GameService.Gw2Mumble.get_UI().get_MapScale() * (double)GameService.Graphics.get_UIScaleMultiplier();
		}

		private async Task<bool> MoveMap(double x, double y, double targetDistance)
		{
			while (true)
			{
				if (!IsInGame())
				{
					Logger.Debug("Not in game");
					return false;
				}
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
				await Task.Delay(MouseMoveAndClickDelay);
			}
			return true;
		}

		public async Task<bool> ChangeMapLayer(ChangeMapLayerDirection direction)
		{
			if (!IsInGame())
			{
				Logger.Debug("Not in game");
				return false;
			}
			Keyboard.Press((VirtualKeyShort)16, false);
			await Task.Delay(KeyboardPressDelay);
			Mouse.RotateWheel(int.MaxValue * ((direction == ChangeMapLayerDirection.Up) ? 1 : (-1)), false, -1, -1, false);
			await Task.Delay(KeyboardPressDelay);
			Keyboard.Release((VirtualKeyShort)16, false);
			return true;
		}

		public Task<NavigationResult> NavigateToPosition(ContinentFloorRegionMapPoi poi)
		{
			return NavigateToPosition(poi, directTeleport: false);
		}

		public Task<NavigationResult> NavigateToPosition(ContinentFloorRegionMapPoi poi, bool directTeleport)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 coord = poi.get_Coord();
			double x = ((Coordinates2)(ref coord)).get_X();
			coord = poi.get_Coord();
			return NavigateToPosition(x, ((Coordinates2)(ref coord)).get_Y(), poi.get_Type() == ApiEnum<PoiType>.op_Implicit((PoiType)2), directTeleport);
		}

		public Task<NavigationResult> NavigateToPosition(double x, double y)
		{
			return NavigateToPosition(x, y, isWaypoint: false, directTeleport: false);
		}

		public async Task<NavigationResult> NavigateToPosition(double x, double y, bool isWaypoint, bool directTeleport)
		{
			_ = 10;
			try
			{
				if (!IsInGame())
				{
					Logger.Debug("Not in game");
					return new NavigationResult(success: false, "Not in game.");
				}
				if (!(await OpenFullscreenMap()))
				{
					Logger.Debug("Could not open map.");
					return new NavigationResult(success: false, "Could not open map.");
				}
				ScreenNotification.ShowNotification(new string[2] { "DO NOT MOVE THE CURSOR!", "Close map to cancel." }, ScreenNotification.NotificationType.Warning, null, 7);
				await WaitForTick();
				Coordinates2 mapPos = GameService.Gw2Mumble.get_UI().get_MapCenter();
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, false);
				if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1206 && !(await ChangeMapLayer(ChangeMapLayerDirection.Down)))
				{
					Logger.Debug("Changing map layer failed.");
					return new NavigationResult(success: false, "Changing map layer failed.");
				}
				if (!(await ZoomOut(6.0)))
				{
					Logger.Debug("Zooming out did not work.");
					return new NavigationResult(success: false, "Zooming out did not work.");
				}
				double totalDist = GetDistance(((Coordinates2)(ref mapPos)).get_X(), ((Coordinates2)(ref mapPos)).get_Y(), x, y) / (GetMapScale() * 0.9);
				Logger.Debug($"Distance: {totalDist}");
				if (!(await MoveMap(x, y, 50.0)))
				{
					Logger.Debug("Moving the map did not work.");
					return new NavigationResult(success: false, "Moving the map did not work.");
				}
				await WaitForTick();
				int finalMouseX2 = GameService.Graphics.get_WindowWidth() / 2;
				int finalMouseY2 = GameService.Graphics.get_WindowHeight() / 2;
				Logger.Debug($"Set mouse on waypoint: x = {finalMouseX2}, y = {finalMouseY2}");
				Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
				if (!(await ZoomIn(2.0)))
				{
					Logger.Debug("Zooming in did not work.");
					return new NavigationResult(success: false, "Zooming in did not work.");
				}
				if (!(await MoveMap(x, y, 5.0)))
				{
					Logger.Debug("Moving the map did not work.");
					return new NavigationResult(success: false, "Moving the map did not work.");
				}
				if (isWaypoint)
				{
					Logger.Debug($"Set mouse on waypoint: x = {finalMouseX2}, y = {finalMouseY2}");
					Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
					await Task.Delay(MouseMoveAndClickDelay);
					Mouse.Click((MouseButton)0, -1, -1, false);
					await Task.Delay(MouseMoveAndClickDelay);
					finalMouseX2 -= 50;
					finalMouseY2 += 10;
					Logger.Debug($"Set mouse on waypoint yes button: x = {finalMouseX2}, y = {finalMouseY2}");
					Mouse.SetPosition(finalMouseX2, finalMouseY2, true);
					if (directTeleport)
					{
						await Task.Delay(250);
						Mouse.Click((MouseButton)0, -1, -1, false);
					}
				}
				return new NavigationResult(success: true, null);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Navigation to position failed:");
				return new NavigationResult(success: false, ex.Message);
			}
		}

		public async Task<(double X, double Y)> EventMapCoordinatesToContinentCoordinates(int mapId, double[] coordinates)
		{
			Map obj = await ((IBulkExpandableClient<Map, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken));
			Rectangle continent_rect = obj.get_ContinentRect();
			Rectangle map_rect = obj.get_MapRect();
			Coordinates2 val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double x = ((Coordinates2)(ref val)).get_X();
			double num = coordinates[0];
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num2 = 1.0 * (num - ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double x2 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num3 = num2 / (x2 - ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref continent_rect)).get_BottomRight();
			double x3 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double item = Math.Round(x + num3 * (x3 - ((Coordinates2)(ref val)).get_X()));
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double y2 = ((Coordinates2)(ref val)).get_Y();
			double num4 = coordinates[1];
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double num5 = -1.0 * (num4 - ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double y3 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num6 = num5 / (y3 - ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref continent_rect)).get_BottomRight();
			double y4 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double y = Math.Round(y2 + num6 * (y4 - ((Coordinates2)(ref val)).get_Y()));
			return (item, y);
		}

		public async Task<double> EventMapLengthToContinentLength(int mapId, double length)
		{
			Rectangle map_rect = (await ((IBulkExpandableClient<Map, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken))).get_MapRect();
			length /= 0.041666666666666664;
			double num = length;
			Coordinates2 val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num2 = num - ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double x = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num3 = num2 / (x - ((Coordinates2)(ref val)).get_X());
			double num4 = length;
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num5 = num4 - ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double y = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double scaley = num5 / (y - ((Coordinates2)(ref val)).get_Y());
			return Math.Sqrt(num3 * num3 + scaley * scaley);
		}

		public MapEntity AddCircle(double x, double y, double radius, Color color, float thickness = 1f)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			MapCircle circle = new MapCircle((float)x, (float)y, (float)radius, color, thickness);
			_flatMap.AddEntity(circle);
			return circle;
		}

		public MapEntity AddBorder(double x, double y, float[][] points, Color color, float thickness = 1f)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			MapBorder border = new MapBorder((float)x, (float)y, points, color, thickness);
			_flatMap.AddEntity(border);
			return border;
		}

		public void ClearMapEntities()
		{
			_flatMap.ClearEntities();
		}

		private async Task<NavigationResult> MoveMouse(int x, int y, bool sendToSystem = false)
		{
			Mouse.GetPosition();
			Mouse.SetPosition(x, y, sendToSystem);
			await WaitForTick();
			return new NavigationResult(success: true, null);
		}

		public void Dispose()
		{
			FlatMap flatMap = _flatMap;
			if (flatMap != null)
			{
				((Control)flatMap).Dispose();
			}
			_flatMap = null;
		}
	}
}
