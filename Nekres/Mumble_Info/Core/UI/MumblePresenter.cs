using System;
using System.Globalization;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Mumble_Info.Core.UI
{
	internal class MumblePresenter : Presenter<MumbleView, MumbleConfig>
	{
		private const string FORMAT_2D = "xpos=\"{0}\" ypos=\"{1}\"";

		private const string FORMAT_3D = "xpos=\"{0}\" ypos=\"{1}\" zpos=\"{2}\"";

		private const string DECIMAL_FORMAT = "0.###";

		public MumblePresenter(MumbleView view, MumbleConfig model)
			: base(view, model)
		{
		}

		public string GetRace()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected I4, but got Unknown
			if (string.IsNullOrEmpty(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				return string.Empty;
			}
			return GameService.Gw2Mumble.get_PlayerCharacter().get_Name() + " (" + MumbleInfoModule.Instance.Api.GetRaceName((int)GameService.Gw2Mumble.get_PlayerCharacter().get_Race()) + ")";
		}

		public string GetPlayerProfession()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected I4, but got Unknown
			string elite = MumbleInfoModule.Instance.Api.GetSpecializationName(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
			string prof = MumbleInfoModule.Instance.Api.GetProfessionName((int)GameService.Gw2Mumble.get_PlayerCharacter().get_Profession());
			if (!string.IsNullOrEmpty(elite))
			{
				if (!string.IsNullOrEmpty(prof))
				{
					return elite + " (" + prof + ")";
				}
				return elite;
			}
			return prof;
		}

		public string GetMap()
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() <= 0)
			{
				return string.Empty;
			}
			Map map = MumbleInfoModule.Instance.Api.Map;
			string mapName = ((map != null) ? map.get_Name() : null) ?? string.Empty;
			if (!string.IsNullOrEmpty(mapName))
			{
				return $"{mapName} ({GameService.Gw2Mumble.get_CurrentMap().get_Id()})";
			}
			return $"{GameService.Gw2Mumble.get_CurrentMap().get_Id()}";
		}

		public string GetSector()
		{
			ContinentFloorRegionMapSector sector = MumbleInfoModule.Instance.Api.Sector;
			return ((sector != null) ? sector.get_Name() : null) ?? string.Empty;
		}

		public string GetClosestWaypoint()
		{
			ContinentFloorRegionMapPoi closestWaypoint = MumbleInfoModule.Instance.Api.ClosestWaypoint;
			return ((closestWaypoint != null) ? closestWaypoint.get_Name() : null) ?? string.Empty;
		}

		public string GetClosestPoi()
		{
			ContinentFloorRegionMapPoi closestPoi = MumbleInfoModule.Instance.Api.ClosestPoi;
			return ((closestPoi != null) ? closestPoi.get_Name() : null) ?? string.Empty;
		}

		private string Vec3ToStr(Vector3 vec, bool markerPackFormat)
		{
			return string.Format(markerPackFormat ? "xpos=\"{0}\" ypos=\"{1}\" zpos=\"{2}\"" : "{0} / {1} / {2}", vec.X.ToString("0.###", NumberFormatInfo.InvariantInfo), vec.Y.ToString("0.###", NumberFormatInfo.InvariantInfo), vec.Z.ToString("0.###", NumberFormatInfo.InvariantInfo));
		}

		private string Coords2ToStr(Coordinates2 vec, bool markerPackFormat)
		{
			return string.Format(markerPackFormat ? "xpos=\"{0}\" ypos=\"{1}\"" : "{0} / {1}", ((Coordinates2)(ref vec)).get_X().ToString("0.###", NumberFormatInfo.InvariantInfo), ((Coordinates2)(ref vec)).get_Y().ToString("0.###", NumberFormatInfo.InvariantInfo));
		}

		public string GetPlayerPosition(bool markerPackFormat)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Vec3ToStr(GameService.Gw2Mumble.get_PlayerCharacter().Position(base.get_Model().SwapYZ), markerPackFormat);
		}

		public string GetPlayerPosition()
		{
			return GetPlayerPosition(markerPackFormat: false);
		}

		public string GetPlayerDirection(bool markerPackFormat)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			string dir = Vec3ToStr(GameService.Gw2Mumble.get_PlayerCharacter().Forward(base.get_Model().SwapYZ), markerPackFormat);
			if (!markerPackFormat)
			{
				return dir + " (" + DirectionUtil.IsFacing(GameService.Gw2Mumble.get_RawClient().get_AvatarFront().SwapYZ()).ToString().SplitCamelCase() + ")";
			}
			return dir;
		}

		public string GetPlayerDirection()
		{
			return GetPlayerDirection(markerPackFormat: false);
		}

		public string GetCameraPosition(bool markerPackFormat)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Vec3ToStr(GameService.Gw2Mumble.get_PlayerCamera().Position(base.get_Model().SwapYZ), markerPackFormat);
		}

		public string GetCameraPosition()
		{
			return GetCameraPosition(markerPackFormat: false);
		}

		public string GetCameraDirection(bool markerPackFormat)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			string dir = Vec3ToStr(GameService.Gw2Mumble.get_PlayerCamera().Forward(base.get_Model().SwapYZ), markerPackFormat);
			if (!markerPackFormat)
			{
				return dir + " (" + DirectionUtil.IsFacing(GameService.Gw2Mumble.get_RawClient().get_CameraFront().SwapYZ()).ToString().SplitCamelCase() + ")";
			}
			return dir;
		}

		public string GetCameraDirection()
		{
			return GetCameraDirection(markerPackFormat: false);
		}

		protected override void Unload()
		{
			base.Unload();
		}

		public string GetProcessId()
		{
			return $"{GameService.Gw2Mumble.get_Info().get_ProcessId()}";
		}

		public string GetServerAddress()
		{
			return $"{GameService.Gw2Mumble.get_Info().get_ServerAddress()} : {GameService.Gw2Mumble.get_Info().get_ServerPort()}";
		}

		public string GetShardId()
		{
			return $"{GameService.Gw2Mumble.get_Info().get_ShardId()}";
		}

		public string GetUiSize()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return $"{GameService.Gw2Mumble.get_UI().get_UISize()}";
		}

		public string GetCompassBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Rectangle compass = GameService.Gw2Mumble.get_UI().CompassBounds();
			return $"{compass.X} X / {compass.Y} Y / {compass.Width} W / {compass.Height} H";
		}

		public async Task CopyToClipboard(string text)
		{
			bool copied = false;
			try
			{
				copied = await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
			}
			catch (Exception e)
			{
				MumbleInfoModule.Logger.Warn(e, e.Message);
			}
			if (copied)
			{
				ScreenNotification.ShowNotification("Copied to Clipboard", (NotificationType)0, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("color-change");
			}
			else
			{
				ScreenNotification.ShowNotification("Unable to Copy. Please try again.", (NotificationType)1, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
			}
		}

		public string GetMapPosition(bool markerPackFormat)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return Coords2ToStr(GameService.Gw2Mumble.get_UI().get_MapPosition(), markerPackFormat) ?? "";
		}

		public string GetMapPosition()
		{
			return GetMapPosition(markerPackFormat: false);
		}

		public string GetMapType()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			string comp = (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() ? " (Competitive)" : string.Empty);
			return $"{GameService.Gw2Mumble.get_CurrentMap().get_Type()}" + comp;
		}

		public string GetContinent()
		{
			Map map = MumbleInfoModule.Instance.Api.Map;
			string continent = ((map != null) ? map.get_ContinentName() : null) ?? string.Empty;
			Map map2 = MumbleInfoModule.Instance.Api.Map;
			string region = ((map2 != null) ? map2.get_RegionName() : null) ?? string.Empty;
			if (string.IsNullOrEmpty(continent) || string.IsNullOrEmpty(region))
			{
				return string.Empty;
			}
			return continent + " - " + region;
		}

		public string GetMapHash(bool discordRichPresenceFormat)
		{
			if (MumbleInfoModule.Instance.Api.Map == null)
			{
				return string.Empty;
			}
			return string.Format(discordRichPresenceFormat ? "\"{0}\": {1}, // {2} ({1})" : "{0}", MumbleInfoModule.Instance.Api.Map.GetHash(), MumbleInfoModule.Instance.Api.Map.get_Id(), MumbleInfoModule.Instance.Api.Map.get_Name());
		}

		public string GetMapHash()
		{
			return GetMapHash(discordRichPresenceFormat: false);
		}
	}
}
