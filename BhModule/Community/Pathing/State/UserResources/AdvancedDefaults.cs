using System;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class AdvancedDefaults
	{
		public const string FILENAME = "advanced.yaml";

		public double CopyAttributeRechargeMs = 8000.0;

		public string[] MarkerLoadPaths { get; set; } = Array.Empty<string>();

	}
}
