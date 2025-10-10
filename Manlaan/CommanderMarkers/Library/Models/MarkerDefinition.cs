using Manlaan.CommanderMarkers.Library.Enums;

namespace Manlaan.CommanderMarkers.Library.Models
{
	public readonly struct MarkerDefinition
	{
		public SquadMarker MarkerType { get; }

		public string DisplayName { get; }

		public bool SupportsGroundTarget { get; }

		public bool SupportsObjectTarget { get; }

		public bool IsClearMarker { get; }

		public MarkerDefinition(SquadMarker markerType, string displayName, bool supportsGroundTarget = true, bool supportsObjectTarget = true, bool isClearMarker = false)
		{
			MarkerType = markerType;
			DisplayName = displayName;
			SupportsGroundTarget = supportsGroundTarget;
			SupportsObjectTarget = supportsObjectTarget;
			IsClearMarker = isClearMarker;
		}

		public string GetGroundTooltip()
		{
			return DisplayName + " Ground";
		}

		public string GetObjectTooltip()
		{
			return DisplayName + " Object";
		}

		public string GetGroundBindingKey()
		{
			return "CmdMrk" + DisplayName + "GndBinding";
		}

		public string GetObjectBindingKey()
		{
			return "CmdMrk" + DisplayName + "ObjBinding";
		}
	}
}
