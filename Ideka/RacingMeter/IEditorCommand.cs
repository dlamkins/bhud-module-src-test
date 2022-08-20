namespace Ideka.RacingMeter
{
	public interface IEditorCommand
	{
		bool Modifying { get; }

		bool Do(EditState state);

		void Undo(EditState state);
	}
}
