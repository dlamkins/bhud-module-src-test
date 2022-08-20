using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class MovePointCommand : IEditorCommand
	{
		private readonly RacePoint _point;

		private readonly Vector3 _position;

		private Vector3 _prevPosition;

		public bool Modifying => true;

		public MovePointCommand(RacePoint point, Vector3 position)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_point = point;
			_position = position;
		}

		public bool Do(EditState state)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_prevPosition = _point.Position;
			if (_prevPosition != _position)
			{
				return state.MovePoint(_point, _position);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			state.MovePoint(_point, _prevPosition);
		}
	}
}
