using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class StatConversion
	{
		[DataMember]
		public AttributeType SourceAttribute;

		[DataMember]
		public AttributeType TargetAttribute;

		[DataMember]
		public double Factor;

		public double Amount(int amount)
		{
			return (double)amount * Factor;
		}
	}
}
