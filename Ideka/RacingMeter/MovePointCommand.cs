using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class MovePointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint _003Cpoint_003EP;

		[CompilerGenerated]
		private Vector3 _003Cposition_003EP;

		private Vector3 _prevPosition;

		public bool Modifying => true;

		public MovePointCommand(RacePoint point, Vector3 position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_003Cpoint_003EP = point;
			_003Cposition_003EP = position;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_prevPosition = _003Cpoint_003EP.Position;
			if (_prevPosition != _003Cposition_003EP)
			{
				return state.MovePoint(_003Cpoint_003EP, _003Cposition_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			state.MovePoint(_003Cpoint_003EP, _prevPosition);
		}
	}
}
