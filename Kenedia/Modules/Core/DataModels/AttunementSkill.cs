using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.DataModels
{
	public class AttunementSkill : BaseConnectionProperty
	{
		public Attunement? Attunement { get; set; }

		public int? Fire { get; set; }

		public int? Water { get; set; }

		public int? Earth { get; set; }

		public int? Air { get; set; }
	}
}
