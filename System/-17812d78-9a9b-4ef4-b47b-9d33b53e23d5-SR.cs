using System.Resources;
using FxResources.Microsoft.Extensions.DependencyInjection;

namespace System
{
	internal static class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR
	{
		private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

		private static ResourceManager s_resourceManager;

		internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.Microsoft.Extensions.DependencyInjection.SR)));

		internal static string AmbiguousConstructorException => GetResourceString("AmbiguousConstructorException");

		internal static string CannotResolveService => GetResourceString("CannotResolveService");

		internal static string CircularDependencyException => GetResourceString("CircularDependencyException");

		internal static string UnableToActivateTypeException => GetResourceString("UnableToActivateTypeException");

		internal static string OpenGenericServiceRequiresOpenGenericImplementation => GetResourceString("OpenGenericServiceRequiresOpenGenericImplementation");

		internal static string ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation => GetResourceString("ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation");

		internal static string TypeCannotBeActivated => GetResourceString("TypeCannotBeActivated");

		internal static string NoConstructorMatch => GetResourceString("NoConstructorMatch");

		internal static string ScopedInSingletonException => GetResourceString("ScopedInSingletonException");

		internal static string ScopedResolvedFromRootException => GetResourceString("ScopedResolvedFromRootException");

		internal static string DirectScopedResolvedFromRootException => GetResourceString("DirectScopedResolvedFromRootException");

		internal static string ConstantCantBeConvertedToServiceType => GetResourceString("ConstantCantBeConvertedToServiceType");

		internal static string ImplementationTypeCantBeConvertedToServiceType => GetResourceString("ImplementationTypeCantBeConvertedToServiceType");

		internal static string AsyncDisposableServiceDispose => GetResourceString("AsyncDisposableServiceDispose");

		internal static string GetCaptureDisposableNotSupported => GetResourceString("GetCaptureDisposableNotSupported");

		internal static string InvalidServiceDescriptor => GetResourceString("InvalidServiceDescriptor");

		internal static string ServiceDescriptorNotExist => GetResourceString("ServiceDescriptorNotExist");

		internal static string CallSiteTypeNotSupported => GetResourceString("CallSiteTypeNotSupported");

		internal static string TrimmingAnnotationsDoNotMatch => GetResourceString("TrimmingAnnotationsDoNotMatch");

		internal static string TrimmingAnnotationsDoNotMatch_NewConstraint => GetResourceString("TrimmingAnnotationsDoNotMatch_NewConstraint");

		internal static string AotCannotCreateEnumerableValueType => GetResourceString("AotCannotCreateEnumerableValueType");

		internal static string AotCannotCreateGenericValueType => GetResourceString("AotCannotCreateGenericValueType");

		internal static string NoServiceRegistered => GetResourceString("NoServiceRegistered");

		internal static string InvalidServiceKeyType => GetResourceString("InvalidServiceKeyType");

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
