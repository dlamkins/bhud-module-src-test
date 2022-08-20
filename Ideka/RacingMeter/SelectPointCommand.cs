using Ideka.RacingMeterLib;

namespace Ideka.RacingMeter
{
	public class SelectPointCommand : IEditorCommand
	{
		private readonly RacePoint _selection;

		private RacePoint _prevSelection;

		public bool Modifying => false;

		public SelectPointCommand(RacePoint selection)
		{
			_selection = selection;
		}

		public bool Do(EditState state)
		{
			if (state.Selected == _selection)
			{
				return false;
			}
			_prevSelection = state.Selected;
			state.Selected = _selection;
			return state.Selected == _selection;
		}

		public void Undo(EditState state)
		{
			state.Selected = _prevSelection;
		}
	}
}
