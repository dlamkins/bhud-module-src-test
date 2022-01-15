using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Input
{
	public class MouseEventArgs
	{
		public Point Position { get; private set; }

		public bool DoubleClick { get; private set; }

		public MouseEventType EventType { get; private set; }

		public MouseEventArgs(Point position, bool doubleClick, MouseEventType type)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			DoubleClick = doubleClick;
			EventType = type;
		}
	}
}
