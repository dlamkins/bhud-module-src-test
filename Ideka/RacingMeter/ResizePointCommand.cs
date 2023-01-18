using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class ResizePointCommand : IEditorCommand
	{
		private readonly RacePoint _point;

		private readonly float _radius;

		private float _prevRadius;

		public bool Modifying => true;

		public ResizePointCommand(RacePoint point, float radius)
		{
			_point = point;
			_radius = radius;
		}

		public bool Do(EditState state)
		{
			_prevRadius = _point.Radius;
			if (_prevRadius != _radius)
			{
				return state.ResizePoint(_point, _radius);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.ResizePoint(_point, _prevRadius);
		}
	}
}
