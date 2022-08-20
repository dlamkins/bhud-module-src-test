namespace Ideka.RacingMeter
{
	public class RenameRaceCommand : IEditorCommand
	{
		private readonly string _name;

		private string _prevName;

		public bool Modifying => true;

		public RenameRaceCommand(string name)
		{
			_name = name;
		}

		public bool Do(EditState state)
		{
			_prevName = state.Race.Name;
			if (_prevName != _name)
			{
				return state.RenameRace(_name);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.RenameRace(_prevName);
		}
	}
}
