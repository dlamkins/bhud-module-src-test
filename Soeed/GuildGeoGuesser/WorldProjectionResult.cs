using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser
{
	public readonly struct WorldProjectionResult
	{
		public Vector2 Screen { get; }

		public bool InFront { get; }

		public bool OnScreen { get; }

		public Vector2 EdgeScreen { get; }

		public float ArrowAngleRadians { get; }

		public WorldProjectionResult(Vector2 screen, bool inFront, bool onScreen, Vector2 edgeScreen, float arrowAngleRadians)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Screen = screen;
			InFront = inFront;
			OnScreen = onScreen;
			EdgeScreen = edgeScreen;
			ArrowAngleRadians = arrowAngleRadians;
		}
	}
}
