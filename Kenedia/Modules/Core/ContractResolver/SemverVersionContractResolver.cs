using System;
using System.Reflection;
using Kenedia.Modules.Core.Attributes;
using Kenedia.Modules.Core.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kenedia.Modules.Core.ContractResolver
{
	public class SemverVersionContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			JsonProperty property = ((DefaultContractResolver)this).CreateProperty(member, memberSerialization);
			if (Attribute.IsDefined(member, typeof(JsonSemverVersionAttribute)))
			{
				property.set_Converter((JsonConverter)(object)new SemverVersionConverter());
			}
			return property;
		}

		public SemverVersionContractResolver()
			: this()
		{
		}
	}
}
