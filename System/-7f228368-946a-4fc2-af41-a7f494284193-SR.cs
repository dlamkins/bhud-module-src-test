using System.Resources;
using FxResources.Microsoft.Extensions.Options;

namespace System
{
	internal static class _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003ESR
	{
		private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

		private static ResourceManager s_resourceManager;

		internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.Microsoft.Extensions.Options.SR)));

		internal static string Error_CannotActivateAbstractOrInterface => GetResourceString("Error_CannotActivateAbstractOrInterface");

		internal static string Error_FailedBinding => GetResourceString("Error_FailedBinding");

		internal static string Error_FailedToActivate => GetResourceString("Error_FailedToActivate");

		internal static string Error_MissingParameterlessConstructor => GetResourceString("Error_MissingParameterlessConstructor");

		internal static string Error_NoConfigurationServices => GetResourceString("Error_NoConfigurationServices");

		internal static string Error_NoConfigurationServicesAndAction => GetResourceString("Error_NoConfigurationServicesAndAction");

		internal static bool UsingResourceKeys()
		{
			return s_usingResourceKeys;
		}

		private static string GetResourceString(string resourceKey)
		{
			if (UsingResourceKeys())
			{
				return resourceKey;
			}
			string result = null;
			try
			{
				result = ResourceManager.GetString(resourceKey);
				return result;
			}
			catch (MissingManifestResourceException)
			{
				return result;
			}
		}

		private static string GetResourceString(string resourceKey, string defaultString)
		{
			string resourceString = GetResourceString(resourceKey);
			if (!(resourceKey == resourceString) && resourceString != null)
			{
				return resourceString;
			}
			return defaultString;
		}

		internal static string Format(string resourceFormat, object? p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(resourceFormat, p1);
		}

		internal static string Format(string resourceFormat, object? p1, object? p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(resourceFormat, p1, p2);
		}

		internal static string Format(string resourceFormat, object? p1, object? p2, object? p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		internal static string Format(string resourceFormat, params object?[]? args)
		{
			if (args != null)
			{
				if (UsingResourceKeys())
				{
					return resourceFormat + ", " + string.Join(", ", args);
				}
				return string.Format(resourceFormat, args);
			}
			return resourceFormat;
		}

		internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(provider, resourceFormat, p1);
		}

		internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(provider, resourceFormat, p1, p2);
		}

		internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(provider, resourceFormat, p1, p2, p3);
		}

		internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
		{
			if (args != null)
			{
				if (UsingResourceKeys())
				{
					return resourceFormat + ", " + string.Join(", ", args);
				}
				return string.Format(provider, resourceFormat, args);
			}
			return resourceFormat;
		}
	}
}
