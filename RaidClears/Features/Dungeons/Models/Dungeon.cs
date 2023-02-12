using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Dungeons.Models
{
	public class Dungeon : GroupModel
	{
		public Dungeon(string name, int index, string shortName, BoxModel[] boxes)
			: base(name, index, shortName, boxes)
		{
		}
	}
}
