using Ideka.RacingMeterLib;

namespace Ideka.RacingMeter
{
	public class SetPointTypeCommand : IEditorCommand
	{
		private readonly RacePoint _point;

		private readonly RacePointType _type;

		private RacePointType _prevType;

		public bool Modifying => true;

		public SetPointTypeCommand(RacePoint point, RacePointType type)
		{
			_point = point;
			_type = type;
		}

		public bool Do(EditState state)
		{
			_prevType = _point.Type;
			if (_prevType != _type)
			{
				return state.SetPointType(_point, _type);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetPointType(_point, _prevType);
		}
	}
}
