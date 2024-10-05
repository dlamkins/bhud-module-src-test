using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Relic : BaseItem
	{
		public Relic()
		{
			base.TemplateSlot = TemplateSlotType.PveRelic;
		}

		public override void Apply(Item item)
		{
			base.Apply(item);
		}
	}
}
