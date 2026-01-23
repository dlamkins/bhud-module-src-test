using System;
using Blish_HUD;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;

namespace NpcFinder.Util
{
	public static class MumbleReader
	{
		private static readonly bool DEBUG_LOGS = false;

		private static readonly Logger Log = Logger.GetLogger(typeof(MumbleReader));

		private static DateTime _lastWarn = DateTime.MinValue;

		private static DateTime _lastDumpUtc = DateTime.MinValue;

		private static bool _dumped;

		public static void ResetDiscovery()
		{
		}

		public static bool TryGetMapId(out int mapId)
		{
			mapId = 0;
			try
			{
				Gw2MumbleService mumble = GameService.Gw2Mumble;
				if (mumble == null)
				{
					return false;
				}
				mapId = mumble.get_CurrentMap().get_Id();
				return mapId > 0;
			}
			catch (Exception ex)
			{
				WarnOccasionally("[MumbleReader] TryGetMapId failed: " + ex.Message);
				return false;
			}
		}

		public static bool TryGetUiState(out uint uiState)
		{
			uiState = 0u;
			return false;
		}

		public static bool TryGetWorldMapUi(out float centerX, out float centerY, out float scale)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			centerX = (centerY = 0f);
			scale = 1f;
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
				if (ui == null)
				{
					return false;
				}
				scale = (float)ui.get_MapScale();
				if (float.IsNaN(scale) || float.IsInfinity(scale) || Math.Abs(scale) < 1E-06f)
				{
					return false;
				}
				Coordinates2 c = ui.get_MapCenter();
				centerX = (float)((Coordinates2)(ref c)).get_X();
				centerY = (float)((Coordinates2)(ref c)).get_Y();
				return true;
			}
			catch (Exception ex)
			{
				WarnOccasionally("[MumbleReader] TryGetWorldMapUi failed: " + ex.Message);
				return false;
			}
		}

		public static void DumpUiOnce()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			if (_dumped)
			{
				return;
			}
			_dumped = true;
			if (!DEBUG_LOGS)
			{
				return;
			}
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
				if (ui == null)
				{
					Log.Warn("[MumbleUI] GameService.Gw2Mumble.UI is null");
					return;
				}
				Log.Warn($"[MumbleUI] IsMapOpen={ui.get_IsMapOpen()}");
				Log.Warn($"[MumbleUI] MapScale={(float)ui.get_MapScale()}");
				Coordinates2 c = ui.get_MapCenter();
				Log.Warn($"[MumbleUI] MapCenter=({(float)((Coordinates2)(ref c)).get_X()},{(float)((Coordinates2)(ref c)).get_Y()})");
			}
			catch (Exception ex)
			{
				Log.Warn("[MumbleUI] Dump failed: " + ex);
			}
		}

		public static void DumpUiOncePerSecond(bool requireMapOpen = true)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (!DEBUG_LOGS)
			{
				return;
			}
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
				if (ui == null)
				{
					WarnOccasionally("[MumbleUI] GameService.Gw2Mumble.UI is null");
				}
				else if (!requireMapOpen || ui.get_IsMapOpen())
				{
					DateTime now = DateTime.UtcNow;
					if (!((now - _lastDumpUtc).TotalSeconds < 1.0))
					{
						_lastDumpUtc = now;
						Coordinates2 c = ui.get_MapCenter();
						Log.Warn($"[MumbleUI] open={ui.get_IsMapOpen()} scale={(float)ui.get_MapScale()} center=({(float)((Coordinates2)(ref c)).get_X()},{(float)((Coordinates2)(ref c)).get_Y()})");
					}
				}
			}
			catch (Exception ex)
			{
				WarnOccasionally("[MumbleUI] Dump failed: " + ex.Message);
			}
		}

		private static void WarnOccasionally(string msg)
		{
			if (DEBUG_LOGS)
			{
				DateTime now = DateTime.UtcNow;
				if (!((now - _lastWarn).TotalSeconds < 2.0))
				{
					_lastWarn = now;
					Log.Warn(msg);
				}
			}
		}
	}
}
