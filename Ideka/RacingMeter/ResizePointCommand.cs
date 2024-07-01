using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class ResizePointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint _003Cpoint_003EP;

		[CompilerGenerated]
		private float _003Cradius_003EP;

		private float _prevRadius;

		public bool Modifying => true;

		public ResizePointCommand(RacePoint point, float radius)
		{
			_003Cpoint_003EP = point;
			_003Cradius_003EP = radius;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevRadius = _003Cpoint_003EP.Radius;
			if (_prevRadius != _003Cradius_003EP)
			{
				return state.ResizePoint(_003Cpoint_003EP, _003Cradius_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.ResizePoint(_003Cpoint_003EP, _prevRadius);
		}
	}
}
