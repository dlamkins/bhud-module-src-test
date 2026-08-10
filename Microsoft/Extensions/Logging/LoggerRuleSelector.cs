using System;

namespace Microsoft.Extensions.Logging
{
	internal static class LoggerRuleSelector
	{
		public static void Select(LoggerFilterOptions options, Type providerType, string category, out LogLevel? minLevel, out Func<string, string, LogLevel, bool> filter)
		{
			filter = null;
			minLevel = options.MinLevel;
			string alias = ProviderAliasUtilities.GetAlias(providerType);
			LoggerFilterRule loggerFilterRule = null;
			foreach (LoggerFilterRule item in options.RulesInternal)
			{
				if (IsBetter(item, loggerFilterRule, providerType.FullName, category) || (!string.IsNullOrEmpty(alias) && IsBetter(item, loggerFilterRule, alias, category)))
				{
					loggerFilterRule = item;
				}
			}
			if (loggerFilterRule != null)
			{
				filter = loggerFilterRule.Filter;
				minLevel = loggerFilterRule.LogLevel;
			}
		}

		private static bool IsBetter(LoggerFilterRule rule, LoggerFilterRule current, string logger, string category)
		{
			if (rule.ProviderName != null && rule.ProviderName != logger)
			{
				return false;
			}
			string categoryName = rule.CategoryName;
			if (categoryName != null)
			{
				int num = categoryName.IndexOf('*');
				if (num != -1 && categoryName.IndexOf('*', num + 1) != -1)
				{
					throw new InvalidOperationException(_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ESR.MoreThanOneWildcard);
				}
				ReadOnlySpan<char> value;
				ReadOnlySpan<char> value2;
				if (num == -1)
				{
					value = categoryName.AsSpan();
					value2 = default(ReadOnlySpan<char>);
				}
				else
				{
					value = categoryName.AsSpan(0, num);
					value2 = categoryName.AsSpan(num + 1);
				}
				if (!category.AsSpan().StartsWith(value, StringComparison.OrdinalIgnoreCase) || !category.AsSpan().EndsWith(value2, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			if (current != null && current.ProviderName != null)
			{
				if (rule.ProviderName == null)
				{
					return false;
				}
			}
			else if (rule.ProviderName != null)
			{
				return true;
			}
			if (current != null && current.CategoryName != null)
			{
				if (rule.CategoryName == null)
				{
					return false;
				}
				if (current.CategoryName!.Length > rule.CategoryName!.Length)
				{
					return false;
				}
			}
			return true;
		}
	}
}
