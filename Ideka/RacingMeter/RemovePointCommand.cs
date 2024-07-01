using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class RemovePointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint _003Cremove_003EP;

		private bool _selected;

		private int _index;

		public bool Modifying => true;

		public RemovePointCommand(RacePoint remove)
		{
			_003Cremove_003EP = remove;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_selected = state.Selected == _003Cremove_003EP;
			_index = state.RemovePoint(_003Cremove_003EP);
			if (_index >= 0 && _selected)
			{
				state.SelectedPointIndex = _index;
			}
			return _index >= 0;
		}

		public void Undo(EditState state)
		{
			state.InsertPoint(_index, _003Cremove_003EP);
			if (_selected)
			{
				state.Selected = _003Cremove_003EP;
			}
		}
	}
}
