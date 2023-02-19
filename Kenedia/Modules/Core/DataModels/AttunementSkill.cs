using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.DataModels
{
	public class AttunementSkill
	{
		public int? Fire;

		public int? Water;

		public int? Earth;

		public int? Air;

		public Attunement? Attunement { get; set; }

		public int?[] Values => new int?[4] { Fire, Water, Earth, Air };

		public void Clear()
		{
			Fire = null;
			Water = null;
			Earth = null;
			Air = null;
			Attunement = null;
		}
	}
}
