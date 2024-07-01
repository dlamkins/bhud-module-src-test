using System.Runtime.CompilerServices;

namespace Ideka.RacingMeter
{
	public class SetRaceMapCommand : IEditorCommand
	{
		[CompilerGenerated]
		private int _003CmapId_003EP;

		private int _prevMapId;

		public bool Modifying => true;

		public SetRaceMapCommand(int mapId)
		{
			_003CmapId_003EP = mapId;
			base._002Ector();
		}

		public bool Do(EditState state)
		{
			_prevMapId = state.Race.MapId;
			if (_prevMapId != _003CmapId_003EP)
			{
				return state.SetRaceMap(_003CmapId_003EP);
			}
			return false;
		}

		public void Undo(EditState state)
		{
			state.SetRaceMap(_prevMapId);
		}
	}
}
