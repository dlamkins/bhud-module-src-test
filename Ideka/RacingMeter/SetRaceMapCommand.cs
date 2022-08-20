namespace Ideka.RacingMeter
{
	public class SetRaceMapCommand : IEditorCommand
	{
		private readonly int _mapId;

		private int _prevMapId;

		public bool Modifying => true;

		public SetRaceMapCommand(int mapId)
		{
			_mapId = mapId;
		}

		public bool Do(EditState state)
		{
			_prevMapId = state.Race.MapId;
			if (_prevMapId != _mapId)
			{
				return state.SetRaceMap(_mapId);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetRaceMap(_prevMapId);
		}
	}
}
