using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ideka.CustomCombatText
{
	public class ContractResolver : DefaultContractResolver
	{
		protected override JsonContract CreateContract(Type objectType)
		{
			JsonContract contract = ((DefaultContractResolver)this).CreateContract(objectType);
			if (objectType == typeof(IAreaModelType))
			{
				contract.set_Converter((JsonConverter)(object)new AreaModelTypeConverter());
			}
			return contract;
		}

		public ContractResolver()
			: this()
		{
		}
	}
}
