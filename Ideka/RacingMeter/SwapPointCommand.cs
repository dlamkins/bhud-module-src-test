using Ideka.RacingMeterLib;

namespace Ideka.RacingMeter
{
	public class SwapPointCommand : IEditorCommand
	{
		private readonly RacePoint _checkpoint;

		private readonly bool _previous;

		public bool Modifying => true;

		public SwapPointCommand(RacePoint checkpoint, bool previous)
		{
			_checkpoint = checkpoint;
			_previous = previous;
		}

		public bool Do(EditState state)
		{
			return state.SwapPoint(_checkpoint, _previous);
		}

		public void Undo(EditState state)
		{
			state.SwapPoint(_checkpoint, !_previous);
		}
	}
}
