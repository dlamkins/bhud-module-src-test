using Microsoft.Xna.Framework;

namespace Manlaan.HPGrid.Controls
{
	public class ArrowNote
	{
		public Vector2 Loc { get; set; }

		public string Note { get; set; }

		public bool InRadius(Vector2 point, float radius)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = Loc - point;
			float distance = ((Vector2)(ref val)).Length();
			if (distance <= radius)
			{
				return true;
			}
			return false;
		}
	}
}
