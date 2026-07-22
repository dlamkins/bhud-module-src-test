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
			JsonProperty property = base.CreateProperty(member, memberSerialization);
			if (Attribute.IsDefined(member, typeof(JsonSemverVersionAttribute)))
			{
				property.Converter = new SemverVersionConverter();
			}
			return property;
		}
	}
}
