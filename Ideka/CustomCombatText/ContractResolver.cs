using System;
using Newtonsoft.Json.Serialization;

namespace Ideka.CustomCombatText
{
	public class ContractResolver : DefaultContractResolver
	{
		protected override JsonContract CreateContract(Type objectType)
		{
			JsonContract contract = base.CreateContract(objectType);
			if (objectType == typeof(IAreaModelType))
			{
				contract.Converter = new AreaModelTypeConverter();
			}
			return contract;
		}
	}
}
