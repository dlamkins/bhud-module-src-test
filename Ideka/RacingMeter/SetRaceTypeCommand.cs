using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class SetRaceTypeCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RaceType _003Ctype_003EP;

		private RaceType _prevType;

		public bool Modifying => true;

		public SetRaceTypeCommand(RaceType type)
		{
			_003Ctype_003EP = type;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevType = state.Race.Type;
			if (_prevType != _003Ctype_003EP)
			{
				return state.SetRaceType(_003Ctype_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetRaceType(_prevType);
		}
	}
}
