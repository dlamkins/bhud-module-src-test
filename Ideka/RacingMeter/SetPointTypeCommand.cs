using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class SetPointTypeCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint _003Cpoint_003EP;

		[CompilerGenerated]
		private RacePointType _003Ctype_003EP;

		private RacePointType _prevType;

		public bool Modifying => true;

		public SetPointTypeCommand(RacePoint point, RacePointType type)
		{
			_003Cpoint_003EP = point;
			_003Ctype_003EP = type;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevType = _003Cpoint_003EP.Type;
			if (_prevType != _003Ctype_003EP)
			{
				return state.SetPointType(_003Cpoint_003EP, _003Ctype_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetPointType(_003Cpoint_003EP, _prevType);
		}
	}
}
