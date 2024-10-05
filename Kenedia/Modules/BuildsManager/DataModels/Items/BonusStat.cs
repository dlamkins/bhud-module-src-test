using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	public class BonusStat
	{
		public BonusType Type { get; set; }

		public int? Amount { get; set; }

		public double? Factor { get; set; }

		public AttributeType? ConversionSourceType { get; set; }
	}
}
