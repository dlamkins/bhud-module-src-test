using System;
using Blish_HUD;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	[JsonConverter(typeof(LocationConverter))]
	public class Location
	{
		public int MapId { get; set; }

		public Coordinates2 MapCoord { get; set; }

		public Vector3 AvatarPosition { get; set; }

		public Vector3 AvatarDirection { get; set; }

		public Vector3 CameraPosition { get; set; }

		public Vector3 CameraDirection { get; set; }

		public string Score(Location source)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (source.MapId != MapId)
			{
				return Service.Config.ScoreWrongMap;
			}
			float diff = Vector3.Distance(AvatarPosition, source.AvatarPosition);
			return Service.Config.Scores.Find((ScoreModel score) => (float)score.Min <= diff && (float)score.Max > diff)?.Value ?? "Missing Score";
		}

		public bool IsFirstPersonCamera()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			float num = (float)Math.Sqrt(Math.Pow(AvatarPosition.X - CameraPosition.X, 2.0) + Math.Pow(AvatarPosition.Y - CameraPosition.Y, 2.0));
			float verticalDistance = Math.Abs(AvatarPosition.Z - CameraPosition.Z);
			bool num2 = num < 6f;
			bool isCameraAbove = CameraPosition.Z > AvatarPosition.Z;
			bool isVerticalInRange = verticalDistance >= 0.5f && verticalDistance <= 3f;
			return num2 && isCameraAbove && isVerticalInRange;
		}

		public string GetCameraModeString()
		{
			if (!IsFirstPersonCamera())
			{
				return "Third-Person";
			}
			return "First-Person";
		}

		public static Location SetFromMumble()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService i = GameService.Gw2Mumble;
			return new Location
			{
				MapId = i.get_CurrentMap().get_Id(),
				MapCoord = i.get_UI().get_MapPosition(),
				AvatarPosition = i.get_PlayerCharacter().get_Position(),
				AvatarDirection = i.get_PlayerCharacter().get_Forward(),
				CameraPosition = i.get_PlayerCamera().get_Position(),
				CameraDirection = i.get_PlayerCamera().get_Forward()
			};
		}
	}
}
