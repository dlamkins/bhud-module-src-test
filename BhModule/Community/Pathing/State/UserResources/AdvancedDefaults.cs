using System;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class AdvancedDefaults
	{
		public const string FILENAME = "advanced.yaml";

		public string[] MarkerLoadPaths { get; set; } = Array.Empty<string>();


		public bool OptimizeMarkerPacks { get; set; } = true;


		public double CopyAttributeRechargeMs { get; set; } = 8000.0;


		public float InteractGearXOffset { get; set; } = 0.62f;


		public float InteractGearYOffset { get; set; } = 0.58f;


		public bool InteractGearAnimation { get; set; } = true;


		public int InfoWindowXOffsetPixels { get; set; } = 300;


		public int InfoWindowYOffsetPixels { get; set; } = 200;

	}
}
