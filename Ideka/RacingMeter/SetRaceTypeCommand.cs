using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class SetRaceTypeCommand : IEditorCommand
	{
		private readonly RaceType _type;

		private RaceType _prevType;

		public bool Modifying => true;

		public SetRaceTypeCommand(RaceType type)
		{
			_type = type;
		}

		public bool Do(EditState state)
		{
			_prevType = state.Race.Type;
			if (_prevType != _type)
			{
				return state.SetRaceType(_type);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetRaceType(_prevType);
		}
	}
}
