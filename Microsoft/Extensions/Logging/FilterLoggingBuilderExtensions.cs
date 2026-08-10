using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
	internal static class FilterLoggingBuilderExtensions
	{
		public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, Func<string?, string?, LogLevel, bool> filter)
		{
			Func<string, string, LogLevel, bool> filter2 = filter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter(filter2);
			});
		}

		public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, Func<string?, LogLevel, bool> categoryLevelFilter)
		{
			Func<string, LogLevel, bool> categoryLevelFilter2 = categoryLevelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter(categoryLevelFilter2);
			});
		}

		public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, Func<string?, LogLevel, bool> categoryLevelFilter) where T : ILoggerProvider
		{
			Func<string, LogLevel, bool> categoryLevelFilter2 = categoryLevelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter<T>(categoryLevelFilter2);
			});
		}

		public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, Func<LogLevel, bool> levelFilter)
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter(levelFilter2);
			});
		}

		public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, Func<LogLevel, bool> levelFilter) where T : ILoggerProvider
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter<T>(levelFilter2);
			});
		}

		public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, string? category, LogLevel level)
		{
			string category2 = category;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter(category2, level);
			});
		}

		public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, string? category, LogLevel level) where T : ILoggerProvider
		{
			string category2 = category;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter<T>(category2, level);
			});
		}

		public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, string? category, Func<LogLevel, bool> levelFilter)
		{
			string category2 = category;
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter(category2, levelFilter2);
			});
		}

		public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, string? category, Func<LogLevel, bool> levelFilter) where T : ILoggerProvider
		{
			string category2 = category;
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return builder.ConfigureFilter(delegate(LoggerFilterOptions options)
			{
				options.AddFilter<T>(category2, levelFilter2);
			});
		}

		public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, Func<string?, string?, LogLevel, bool> filter)
		{
			return AddRule(builder, null, null, null, filter);
		}

		public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, Func<string?, LogLevel, bool> categoryLevelFilter)
		{
			Func<string, LogLevel, bool> categoryLevelFilter2 = categoryLevelFilter;
			return AddRule(builder, null, null, null, (string type, string name, LogLevel level) => categoryLevelFilter2(name, level));
		}

		public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, Func<string?, LogLevel, bool> categoryLevelFilter) where T : ILoggerProvider
		{
			Func<string, LogLevel, bool> categoryLevelFilter2 = categoryLevelFilter;
			return AddRule(builder, typeof(T).FullName, null, null, (string type, string name, LogLevel level) => categoryLevelFilter2(name, level));
		}

		public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, Func<LogLevel, bool> levelFilter)
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return AddRule(builder, null, null, null, (string type, string name, LogLevel level) => levelFilter2(level));
		}

		public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, Func<LogLevel, bool> levelFilter) where T : ILoggerProvider
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return AddRule(builder, typeof(T).FullName, null, null, (string type, string name, LogLevel level) => levelFilter2(level));
		}

		public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, string? category, LogLevel level)
		{
			return AddRule(builder, null, category, level);
		}

		public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, string? category, LogLevel level) where T : ILoggerProvider
		{
			return AddRule(builder, typeof(T).FullName, category, level);
		}

		public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, string? category, Func<LogLevel, bool> levelFilter)
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return AddRule(builder, null, category, null, (string type, string name, LogLevel level) => levelFilter2(level));
		}

		public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, string? category, Func<LogLevel, bool> levelFilter) where T : ILoggerProvider
		{
			Func<LogLevel, bool> levelFilter2 = levelFilter;
			return AddRule(builder, typeof(T).FullName, category, null, (string type, string name, LogLevel level) => levelFilter2(level));
		}

		private static ILoggingBuilder ConfigureFilter(this ILoggingBuilder builder, Action<LoggerFilterOptions> configureOptions)
		{
			builder.Services.Configure(configureOptions);
			return builder;
		}

		private static LoggerFilterOptions AddRule(LoggerFilterOptions options, string type = null, string category = null, LogLevel? level = null, Func<string, string, LogLevel, bool> filter = null)
		{
			options.Rules.Add(new LoggerFilterRule(type, category, level, filter));
			return options;
		}
	}
}
