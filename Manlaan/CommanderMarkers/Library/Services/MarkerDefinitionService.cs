using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Library.Models;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public static class MarkerDefinitionService
	{
		public static readonly MarkerDefinition[] AllMarkers = new MarkerDefinition[9]
		{
			new MarkerDefinition(SquadMarker.Arrow, "Arrow"),
			new MarkerDefinition(SquadMarker.Circle, "Circle"),
			new MarkerDefinition(SquadMarker.Heart, "Heart"),
			new MarkerDefinition(SquadMarker.Square, "Square"),
			new MarkerDefinition(SquadMarker.Star, "Star"),
			new MarkerDefinition(SquadMarker.Spiral, "Spiral"),
			new MarkerDefinition(SquadMarker.Triangle, "Triangle"),
			new MarkerDefinition(SquadMarker.Cross, "X"),
			new MarkerDefinition(SquadMarker.Clear, "Clear", supportsGroundTarget: true, supportsObjectTarget: true, isClearMarker: true)
		};

		public static KeyBinding GetDefaultKeyBinding(SquadMarker markerType, bool isGroundTarget)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			Keys key = (Keys)(markerType switch
			{
				SquadMarker.Arrow => 49, 
				SquadMarker.Circle => 50, 
				SquadMarker.Heart => 51, 
				SquadMarker.Square => 52, 
				SquadMarker.Star => 53, 
				SquadMarker.Spiral => 54, 
				SquadMarker.Triangle => 55, 
				SquadMarker.Cross => 56, 
				SquadMarker.Clear => 57, 
				_ => 49, 
			});
			return new KeyBinding((ModifierKeys)(isGroundTarget ? 2 : 6), key);
		}

		public static MarkerDefinition? GetByType(SquadMarker markerType)
		{
			return AllMarkers.FirstOrDefault((MarkerDefinition m) => m.MarkerType == markerType);
		}

		public static IEnumerable<MarkerDefinition> GetGroundMarkers()
		{
			return AllMarkers.Where((MarkerDefinition m) => m.SupportsGroundTarget);
		}

		public static IEnumerable<MarkerDefinition> GetObjectMarkers()
		{
			return AllMarkers.Where((MarkerDefinition m) => m.SupportsObjectTarget);
		}
	}
}
