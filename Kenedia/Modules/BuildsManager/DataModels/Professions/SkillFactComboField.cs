using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class SkillFactComboField : SkillFact
	{
		public SkillFactComboField(Gw2Sharp.WebApi.V2.Models.SkillFactComboField fact)
			: base(fact)
		{
		}
	}
}
