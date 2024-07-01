using System.Runtime.CompilerServices;

namespace Ideka.RacingMeter
{
	public class RenameRaceCommand : IEditorCommand
	{
		[CompilerGenerated]
		private string _003Cname_003EP;

		private string _prevName;

		public bool Modifying => true;

		public RenameRaceCommand(string name)
		{
			_003Cname_003EP = name;
			_prevName = "";
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevName = state.Race.Name;
			if (_prevName != _003Cname_003EP)
			{
				return state.RenameRace(_003Cname_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.RenameRace(_prevName);
		}
	}
}
