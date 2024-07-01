using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class InsertPointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private int _003Cindex_003EP;

		[CompilerGenerated]
		private RacePoint _003Cinsert_003EP;

		private RacePoint? _prevSelected;

		public bool Modifying => true;

		public InsertPointCommand(int index, RacePoint insert)
		{
			_003Cindex_003EP = index;
			_003Cinsert_003EP = insert;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevSelected = state.Selected;
			state.Selected = state.InsertPoint(_003Cindex_003EP, _003Cinsert_003EP);
			return true;
		}

		public void Undo(EditState state)
		{
			state.Selected = _prevSelected;
			state.RemovePoint(_003Cindex_003EP);
		}
	}
}
