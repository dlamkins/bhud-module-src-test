using System;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.Logging
{
	internal static class LoggerFactoryExtensions
	{
		public static ILogger<T> CreateLogger<T>(this ILoggerFactory factory)
		{
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper.ThrowIfNull(factory, "factory");
			return new Logger<T>(factory);
		}

		public static ILogger CreateLogger(this ILoggerFactory factory, Type type)
		{
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper.ThrowIfNull(factory, "factory");
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper.ThrowIfNull(type, "type");
			return factory.CreateLogger(TypeNameHelper.GetTypeDisplayName(type, fullName: true, includeGenericParameterNames: false, includeGenericParameters: false, '.'));
		}
	}
}
