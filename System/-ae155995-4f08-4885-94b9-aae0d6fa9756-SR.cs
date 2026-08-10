using System.Resources;
using FxResources.System.IO.Pipelines;

namespace System
{
	internal static class _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR
	{
		private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

		private static ResourceManager s_resourceManager;

		internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.System.IO.Pipelines.SR)));

		internal static string AdvanceToInvalidCursor => GetResourceString("AdvanceToInvalidCursor");

		internal static string ArgumentOutOfRange_NeedPosNum => GetResourceString("ArgumentOutOfRange_NeedPosNum");

		internal static string ConcurrentOperationsNotSupported => GetResourceString("ConcurrentOperationsNotSupported");

		internal static string FlushCanceledOnPipeWriter => GetResourceString("FlushCanceledOnPipeWriter");

		internal static string GetResultBeforeCompleted => GetResourceString("GetResultBeforeCompleted");

		internal static string InvalidExaminedOrConsumedPosition => GetResourceString("InvalidExaminedOrConsumedPosition");

		internal static string InvalidExaminedPosition => GetResourceString("InvalidExaminedPosition");

		internal static string InvalidZeroByteRead => GetResourceString("InvalidZeroByteRead");

		internal static string ObjectDisposed_StreamClosed => GetResourceString("ObjectDisposed_StreamClosed");

		internal static string NoReadingOperationToComplete => GetResourceString("NoReadingOperationToComplete");

		internal static string NotSupported_UnreadableStream => GetResourceString("NotSupported_UnreadableStream");

		internal static string NotSupported_UnwritableStream => GetResourceString("NotSupported_UnwritableStream");

		internal static string ReadCanceledOnPipeReader => GetResourceString("ReadCanceledOnPipeReader");

		internal static string ReaderAndWriterHasToBeCompleted => GetResourceString("ReaderAndWriterHasToBeCompleted");

		internal static string ReadingAfterCompleted => GetResourceString("ReadingAfterCompleted");

		internal static string ReadingIsInProgress => GetResourceString("ReadingIsInProgress");

		internal static string WritingAfterCompleted => GetResourceString("WritingAfterCompleted");

		internal static string UnflushedBytesNotSupported => GetResourceString("UnflushedBytesNotSupported");

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
