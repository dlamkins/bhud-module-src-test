using System.Runtime.CompilerServices;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class SwapPointCommand : IEditorCommand
	{
		[CompilerGenerated]
		private RacePoint _003Ccheckpoint_003EP;

		[CompilerGenerated]
		private bool _003Cprevious_003EP;

		public bool Modifying => true;

		public SwapPointCommand(RacePoint checkpoint, bool previous)
		{
			_003Ccheckpoint_003EP = checkpoint;
			_003Cprevious_003EP = previous;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			return state.SwapPoint(_003Ccheckpoint_003EP, _003Cprevious_003EP);
		}

		public void Undo(EditState state)
		{
			state.SwapPoint(_003Ccheckpoint_003EP, !_003Cprevious_003EP);
		}
	}
}
