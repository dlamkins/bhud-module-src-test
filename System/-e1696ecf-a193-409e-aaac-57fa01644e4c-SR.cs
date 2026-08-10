using System.Resources;
using FxResources.Microsoft.Bcl.TimeProvider;

namespace System
{
	internal static class _003Ce1696ecf_002Da193_002D409e_002Daaac_002D57fa01644e4c_003ESR
	{
		private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

		private static ResourceManager s_resourceManager;

		internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.Microsoft.Bcl.TimeProvider.SR)));

		internal static string ArgumentOutOfRange_Generic_MustBeNonNegativeNonZero => GetResourceString("ArgumentOutOfRange_Generic_MustBeNonNegativeNonZero");

		internal static string ArgumentOutOfRange_Generic_MustBeGreaterOrEqual => GetResourceString("ArgumentOutOfRange_Generic_MustBeGreaterOrEqual");

		internal static string ArgumentOutOfRange_Generic_MustBeLessOrEqual => GetResourceString("ArgumentOutOfRange_Generic_MustBeLessOrEqual");

		internal static string InvalidOperation_TimeProviderNullLocalTimeZone => GetResourceString("InvalidOperation_TimeProviderNullLocalTimeZone");

		internal static string InvalidOperation_TimeProviderInvalidTimestampFrequency => GetResourceString("InvalidOperation_TimeProviderInvalidTimestampFrequency");

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

		internal static string Format(string resourceFormat, object p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(resourceFormat, p1);
		}

		internal static string Format(string resourceFormat, object p1, object p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(resourceFormat, p1, p2);
		}

		internal static string Format(string resourceFormat, object p1, object p2, object p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		internal static string Format(string resourceFormat, params object[] args)
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

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(provider, resourceFormat, p1);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(provider, resourceFormat, p1, p2);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(provider, resourceFormat, p1, p2, p3);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
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
