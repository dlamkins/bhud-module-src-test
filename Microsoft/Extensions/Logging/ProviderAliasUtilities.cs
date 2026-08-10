using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.Logging
{
	internal static class ProviderAliasUtilities
	{
		private const string AliasAttributeTypeFullName = "Microsoft.Extensions.Logging.ProviderAliasAttribute";

		internal static string GetAlias(Type providerType)
		{
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(providerType);
			for (int i = 0; i < customAttributes.Count; i++)
			{
				CustomAttributeData customAttributeData = customAttributes[i];
				if (customAttributeData.AttributeType.FullName == "Microsoft.Extensions.Logging.ProviderAliasAttribute" && customAttributeData.ConstructorArguments.Count > 0)
				{
					return customAttributeData.ConstructorArguments[0].Value?.ToString();
				}
			}
			return null;
		}
	}
}
