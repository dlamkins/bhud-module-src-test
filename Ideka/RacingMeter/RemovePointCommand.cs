using Ideka.RacingMeterLib;

namespace Ideka.RacingMeter
{
	public class RemovePointCommand : IEditorCommand
	{
		private readonly RacePoint _remove;

		private bool _selected;

		private int _index;

		public bool Modifying => true;

		public RemovePointCommand(RacePoint remove)
		{
			_remove = remove;
		}

		public bool Do(EditState state)
		{
			_selected = state.Selected == _remove;
			_index = state.RemovePoint(_remove);
			if (_index >= 0 && _selected)
			{
				state.SelectedPointIndex = _index;
			}
			return _index >= 0;
		}

		public void Undo(EditState state)
		{
			state.InsertPoint(_index, _remove);
			if (_selected)
			{
				state.Selected = _remove;
			}
		}
	}
}
