using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class SkillFactBuff : SkillFact
	{
		public SkillFactBuff(Gw2Sharp.WebApi.V2.Models.SkillFactBuff fact)
			: base(fact)
		{
		}
	}
}
