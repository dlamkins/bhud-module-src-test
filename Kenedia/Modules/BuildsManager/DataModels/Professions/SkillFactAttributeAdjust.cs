using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class SkillFactAttributeAdjust : SkillFact
	{
		public SkillFactAttributeAdjust(Gw2Sharp.WebApi.V2.Models.SkillFactAttributeAdjust fact)
			: base(fact)
		{
		}
	}
}
