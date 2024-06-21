using Estreya.BlishHUD.Shared.Controls.World;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Controls.World
{
	public class EventWorldTimer : WorldPolygone
	{
		public EventWorldTimer(Vector3 position, Vector3[] points)
			: this(position, points, Color.get_White())
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public EventWorldTimer(Vector3 position, Vector3[] points, Color color)
			: base(position, points, color)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)

	}
}
