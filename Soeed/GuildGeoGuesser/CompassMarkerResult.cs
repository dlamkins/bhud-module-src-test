using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser
{
	public readonly struct CompassMarkerResult
	{
		public Vector2 Screen { get; }

		public bool Visible { get; }

		public bool IsCrossZone { get; }

		public CompassMarkerResult(Vector2 screen, bool visible, bool isCrossZone)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			Screen = screen;
			Visible = visible;
			IsCrossZone = isCrossZone;
		}
	}
}
