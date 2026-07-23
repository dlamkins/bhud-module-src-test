using Taskmaster.Models;

namespace Taskmaster.Services
{
	public sealed class PsnaLocation
	{
		public TaskPresetSlot Slot { get; }

		public string Region { get; }

		public string Npc { get; }

		public string Location { get; }

		public int MapId { get; }

		public string MapLink => PsnaRotation.EncodeMapLink(MapId);

		public PsnaLocation(TaskPresetSlot slot, string region, string npc, string location, int mapId)
		{
			Slot = slot;
			Region = region;
			Npc = npc;
			Location = location;
			MapId = mapId;
		}
	}
}
