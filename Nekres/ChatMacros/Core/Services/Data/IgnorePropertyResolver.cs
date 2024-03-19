using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nekres.ChatMacros.Core.Services.Data
{
	public class IgnorePropertyResolver : DefaultContractResolver
	{
		private readonly string[] _propertyNamesToIgnore;

		public IgnorePropertyResolver(params string[] propertyNamesToIgnore)
			: this()
		{
			_propertyNamesToIgnore = propertyNamesToIgnore;
		}

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			JsonProperty property = ((DefaultContractResolver)this).CreateProperty(member, memberSerialization);
			property.set_ShouldSerialize((Predicate<object>)((object instance) => !_propertyNamesToIgnore.Contains(property.get_PropertyName())));
			return property;
		}

		public static JsonSerializerSettings Settings(params string[] propertyNamesToIgnore)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_ContractResolver((IContractResolver)(object)new IgnorePropertyResolver(propertyNamesToIgnore));
			val.set_Formatting((Formatting)0);
			val.set_NullValueHandling((NullValueHandling)1);
			val.set_DefaultValueHandling((DefaultValueHandling)1);
			return val;
		}
	}
}
