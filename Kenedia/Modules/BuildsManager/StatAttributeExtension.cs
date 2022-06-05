using Kenedia.Modules.BuildsManager.Strings;

namespace Kenedia.Modules.BuildsManager
{
	internal static class StatAttributeExtension
	{
		public static string getLocalName(this API.StatAttribute attribute)
		{
			string text = attribute.Name;
			return attribute.Id switch
			{
				7 => common.Power, 
				8 => common.Precision, 
				9 => common.Toughness, 
				10 => common.Vitality, 
				5 => common.Ferocity, 
				6 => common.HealingPower, 
				3 => common.ConditionDamage, 
				2 => common.Concentration, 
				4 => common.Expertise, 
				_ => text, 
			};
		}
	}
}
