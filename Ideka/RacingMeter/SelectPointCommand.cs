using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class SelectPointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint? _003Cselection_003EP;

		private RacePoint? _prevSelection;

		public bool Modifying => false;

		public SelectPointCommand(RacePoint? selection)
		{
			_003Cselection_003EP = selection;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			if (state.Selected == _003Cselection_003EP)
			{
				return false;
			}
			_prevSelection = state.Selected;
			state.Selected = _003Cselection_003EP;
			return state.Selected == _003Cselection_003EP;
		}

		public void Undo(EditState state)
		{
			state.Selected = _prevSelection;
		}
	}
}
