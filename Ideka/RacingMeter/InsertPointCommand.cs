using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class InsertPointCommand : IEditorCommand
	{
		private readonly int _index;

		private readonly RacePoint _insert;

		private RacePoint _prevSelected;

		public bool Modifying => true;

		public InsertPointCommand(int index, RacePoint insert)
		{
			_index = index;
			_insert = insert;
		}

		public bool Do(EditState state)
		{
			_prevSelected = state.Selected;
			state.Selected = state.InsertPoint(_index, _insert);
			return true;
		}

		public void Undo(EditState state)
		{
			state.Selected = _prevSelected;
			state.RemovePoint(_index);
		}
	}
}
