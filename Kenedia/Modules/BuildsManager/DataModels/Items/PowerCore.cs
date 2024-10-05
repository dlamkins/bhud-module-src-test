using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class PowerCore : BaseItem
	{
		public PowerCore()
		{
			base.TemplateSlot = TemplateSlotType.PowerCore;
		}

		public PowerCore(Item item)
			: this()
		{
			Apply(item);
		}
	}
}
